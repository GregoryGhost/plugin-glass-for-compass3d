namespace LoadingPlugin.Tests

module ViewsHelpers =
    open Microsoft.FSharp.Reflection
    open System
    open System.IO
    open LoadingPlugin.Tests.Series

    type Menu =
        | MainMenu
        | LoadingTest
        | BuildChart
        | Exit
         with static member CountMenuItem()=
                FSharpType.GetUnionCases(typeof<Menu>).Length

    let toMenu(item : int option) = 
        if item.IsNone then
            MainMenu
        else
            match item.Value with
            | 1 -> Menu.LoadingTest
            | 2 -> Menu.BuildChart
            | 0 -> Menu.Exit
            | _ -> Menu.MainMenu
    
    let printSuccessOperation str =
        let fc = Console.ForegroundColor
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Ok: %A" str       
        Console.ForegroundColor <- fc
    
    let showExp(msg : Exception) =
        let fc = Console.ForegroundColor
        Console.ForegroundColor <- ConsoleColor.Red
        printfn "%s" msg.Message
        Console.ForegroundColor <- fc
        Console.ReadKey() |> ignore
        None

    let (>>=) twoTrackInput switchFunction = 
        Option.bind switchFunction twoTrackInput
    
    let printEndTest() = 
        let msg = "Finished loading test."
        msg |> printSuccessOperation |> ignore

    let printStartTest() =
        let msg = "Start loading test ..."
        msg |> printSuccessOperation |> ignore

    let isOkPath path = 
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Data loaded: %s" path
        Console.ForegroundColor <- ConsoleColor.White

    let trySaveData(path : string, data : SourceDataSeries) =
        try    
            writeSeries(
                data 
                |> convertToSeries 
                |> toJson, path)
            path |> isOkPath 
            data |> Some          
        with
        | :? Exception as ex ->
            ex |> showExp

    let tryLoadData(path : string) =
        try    
            let data = 
                path
                |> readSeries
                |> Some
            path |> isOkPath
            data
        with
        | :? FileNotFoundException as ex -> 
            ex |> showExp

        | :? ArgumentException as ex ->
            ex |> showExp 

module Views = 
    open LoadingPlugin.Tests.BuildingGlassesTest
    open LoadingPlugin.Tests.BuildingChart
    open LoadingPlugin.Tests.Series
    open ViewsHelpers

    open System
    open System.Windows.Forms

    let defaultFileName = "loading.data"
    let getFullPath fileName = sprintf "%s\%s" Application.StartupPath fileName
    let pathData = defaultFileName |> getFullPath
    
    let printSelectedPath(path:string) =
        if(path.Length = 0) then 
            printfn "Selected data at default path ..."
            pathData
        else 
            printfn "Selected data at %s ..." path
            defaultFileName
    
    let fileAtProgPath fileName = 
            fileName |> getFullPath |> printSelectedPath

    let saveSeriesToFile(data : SourceDataSeries) =
        printfn ""
        printfn "Enter file name for save data of loading building test at program path:"
        printfn "(default full path(path App + file name): %s)" pathData
        printfn "Press \"Enter\" key for save data at default full path."

        let writeData path =
            trySaveData(path, data)
        let pathWriteData = Console.ReadLine() |> string

        pathWriteData
        |> fileAtProgPath
        |> writeData

    let maxCountBuilding() = 
        printfn "Enter max count building of glasses:"
        try
            let k = Console.ReadLine() |> int
            let kOutRange = 
                (k > 0 && k <= maxCountBuildingPossible) = false
            if kOutRange then
                let msg = 
                    sprintf "Entered value is outside \
                        of allowed range (%d, %d]" 0 maxCountBuildingPossible
                raise (FormatException msg)
            k |> Some
        with
        | :? FormatException as ex ->
            ex |> showExp

    let printTimesBuilding data =
        let times, _ = data |> List.unzip
        let timesBuildingOfGlasses = 
            times |> List.sum 
        printSummary timesBuildingOfGlasses
        printForEachGlass <| data 
    
    let rec tryWriteData series =
        let isOk = series |> saveSeriesToFile

        if isOk.IsSome then 
            isOk
        else 
            tryWriteData series

    let runLoadingTest() =
        let series =
            let loadTest count =
                printStartTest()
                count |> calcTimeBuildingGlasses |> Some

            let test =
                maxCountBuilding() 
                >>= loadTest
                >>= tryWriteData

            if test.IsSome then
                printEndTest()
                printfn "For continue press any key..."
                Console.ReadKey() |> ignore
            test
        series  

    let showViewChart() =
        printfn "Loading data will be from path:\n%s" Application.StartupPath
        printfn "Enter file name of data loading building test:"
        printfn "(default path: %s)" pathData
        printfn "Press \"Enter\" key for load data at default path."

        let enterPath = 
            let path = Console.ReadLine()
            path |> printSelectedPath

        enterPath
        |> tryLoadData
        >>= printChart |> ignore
    
    let showMainMenu() =
        printfn "Menu program, enter number:"
        printfn "1) Run loading testing;"
        printfn "2) Build chart;"
        printfn "0) Exit."

    let runMenuTask task = 
        match task with
        | Menu.LoadingTest -> runLoadingTest() |> ignore

        | Menu.BuildChart -> showViewChart()

        | Menu.Exit -> Environment.ExitCode |> exit

        | Menu.MainMenu -> showMainMenu()