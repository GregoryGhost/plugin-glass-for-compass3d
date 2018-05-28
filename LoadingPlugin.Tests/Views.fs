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

    let printMaxCountBuilding k =
        let fc = Console.ForegroundColor
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Ok: %d" k
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

    let tb() =  
        let data =
            maxCountBuilding()
            |> Option.get
            |> calcTimeBuildingGlasses
        let writeDefault data = writeSeries(data, pathData)
        data |> convertToSeries |> toJson |> writeDefault
        data

    let timesBuilding() = 
        tb()
        |> timeBuilding

    let printTimesBuilding() =
        let times, _ = timesBuilding() |> List.unzip
        let timesBuildingOfGlasses = 
            times |> List.sum 
        printSummary timesBuildingOfGlasses
        printForEachGlass <| timesBuilding() 

    let repeatDrawingPlot() =
        let mutable repeat = true
        while(repeat) do
            tb() 
            |> chartsTimeBuilding 
            |> charts |> Chart.Show
            printfn "Drawing plot?(y/n)"
            let r = Console.ReadLine() |> string
            if(r <> "y") then repeat <- false

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