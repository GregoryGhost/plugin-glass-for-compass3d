namespace LoadingPlugin.Tests

module ViewsHelpers =
    open Microsoft.FSharp.Reflection

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


module Views = 
    open LoadingPlugin.Tests.BuildingGlassesTest
    open LoadingPlugin.Tests.BuildingChart
    open LoadingPlugin.Tests.Series
    open ViewsHelpers

    open System
    open System.IO
    open System.Windows.Forms
    open FSharp.Charting

    let defaultNameFile = "/loading.data"
    let pathData = Application.StartupPath + defaultNameFile 

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

    let maxCountBuilding() = 
        printfn "Enter max count building of glasses:"
        try
            let k = Console.ReadLine() |> int
            let kOutRange = 
                (k > 0 && k <= maxCountBuildingPossible) = false
            if kOutRange then
                let msg = 
                    sprintf "Entered value is outside \
                        of allowed range [%d, %d]" 0 maxCountBuildingPossible
                raise (FormatException msg)
            k |> Some
        with
        | :? FormatException as ex ->
            ex |> showExp

    let (>>=) twoTrackInput switchFunction = 
        Option.bind switchFunction twoTrackInput
    
    let printEndTest() = 
        let msg = "Finished loading test."
        msg |> printSuccessOperation |> ignore

    let printStartTest() =
        let msg = "Start loading test ..."
        msg |> printSuccessOperation |> ignore

    let tb() =  
        let writeDefault data = writeSeries(data, pathData) 
        let data =
            let mc = maxCountBuilding()
            if mc.IsSome then
                printStartTest() |> ignore
                let data = mc.Value |> calcTimeBuildingGlasses
                data |> convertToSeries |> toJson |> writeDefault
                printEndTest() |> ignore
                data |> Some
            else 
                None
        data
    
    let adapterTimeBuilding data =
        data |> timeBuilding |> Some  

    let timesBuilding() = 
        tb()
        >>= adapterTimeBuilding |> Option.get

    let printTimesBuilding() =
        let times, _ = timesBuilding() |> List.unzip
        let timesBuildingOfGlasses = 
            times |> List.sum 
        printSummary timesBuildingOfGlasses
        printForEachGlass <| timesBuilding() 

    let adapterChartsTimeBuilding times =
        times |> chartsTimeBuilding |> Some
    
    let adapterCharts c =
        c |> charts |> Some
    
    let adapterChartShow c =
        c |> Chart.Show |> Some
    
    let next() =
        printfn "Redrawing plot?(y/n)"
        let r = Console.ReadLine() |> string
        if(r = "y") then 
            Some ()
        else None

    let repeatDrawingPlot() =
        let rec repeat() =
            tb()
            >>= adapterChartsTimeBuilding
            >>= adapterCharts
            >>= adapterChartShow
            >>= next
            >>= repeat
        repeat() |> ignore

    let isOkPath path = 
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Data loaded: %s" path
        Console.ForegroundColor <- ConsoleColor.White
    
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

    let printSelectedPath(path:string) =
        if(path.Length = 0) then 
            printfn "Loading data at default path ..."
            pathData
        else 
            printfn "Loading data at %s ..." path
            path

    let showViewChart() =
        printfn "Enter path of data loading building test:"
        printfn "(default path: %s)" pathData
        printfn "Press \"Enter\" key for load data at default path."

        let enterPath = 
            let path = Console.ReadLine()
            path |> printSelectedPath

        enterPath
        |> tryLoadData
        |> printChart
    
    let showMainMenu() =
        printfn "Menu program, enter number:"
        printfn "1) Run loading testing;"
        printfn "2) Build chart;"
        printfn "0) Exit."

    let runMenuTask task = 
        match task with
        | Menu.LoadingTest -> repeatDrawingPlot()

        | Menu.BuildChart -> showViewChart()

        | Menu.Exit -> Environment.ExitCode |> exit

        | Menu.MainMenu -> showMainMenu()