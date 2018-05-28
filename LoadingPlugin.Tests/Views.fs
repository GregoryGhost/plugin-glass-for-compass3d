namespace LoadingPlugin.Tests

module Views = 
    open LoadingPlugin.Tests.BuildingGlassesTest
    open LoadingPlugin.Tests.BuildingChart
    open LoadingPlugin.Tests.Series
    
    open System
    open System.IO
    open System.Windows.Forms
    open FSharp.Charting

    type Menu =
        | LoadingTest
        | BuildChart
        | Exit
    

    let defaultNameFile = "/loading.data"
    let pathData = Application.StartupPath + defaultNameFile 

    let maxCountBuilding() = 
        printfn "Enter max count building of glasses:"
        let k = Console.ReadLine() |> int
        //TODO: обработка неверных k
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Ok: %d" k
        Console.ForegroundColor <- ConsoleColor.White
        k

    let tb() =  
        let data =
            maxCountBuilding() 
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

    let showExp(msg : Exception) = 
        Console.ForegroundColor <- ConsoleColor.Red
        printfn "%s" msg.Message
        Console.ForegroundColor <- ConsoleColor.White
        None

    let isOkPath path = 
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Data loaded: %s" path
        Console.ForegroundColor <- ConsoleColor.White
    
    let tryLoadData(path : string) =
            try
                let series = 
                    if(path.Length = 0) then 
                        pathData |> readSeries
                    else 
                        path |> readSeries
                series |> Some
            with
            | :? FileNotFoundException as ex -> 
                ex |> showExp

            | :? ArgumentException as ex ->
                ex |> showExp

    let printChart(data : Series option) =
            if data.IsSome then
                data.Value
                |> convertToData
                |> chartsTimeBuilding
                |> charts |> Chart.Show

    let showViewChart() =
        printfn "Enter path of data loading building test:"
        printfn "(default path: %s)" pathData

        let enterPath = Console.ReadLine()
        enterPath
        |> tryLoadData
        |> printChart
    
    let toMenu item = 
        match item with
        | 1 -> Menu.LoadingTest
        | 2 -> Menu.BuildChart
        | 0 -> Menu.Exit
        | _ -> Menu.Exit

    let runMenuTask task = 
        match task with
        | Menu.LoadingTest -> repeatDrawingPlot()

        | Menu.BuildChart -> showViewChart()

        | Menu.Exit -> Environment.ExitCode |> exit 