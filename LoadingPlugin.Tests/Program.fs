open FSharp.Charting

open LoadingPlugin.Tests.BuildingGlassesTest
open LoadingPlugin.Tests.BuildingChart
open LoadingPlugin.Tests.Series
open LoadingPlugin.Tests.Views

open System
open System.Windows.Forms
open System.IO
open System

[<EntryPoint>]
let main argv = 
    let defaultNameFile = "loading.data"
    let pathData = Application.StartupPath + defaultNameFile 

    let maxCountBuilding() = 
        printfn "Enter max count building of glasses:"
        let k = Console.ReadLine() |> int
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Ok: %d" k
        Console.ForegroundColor <- ConsoleColor.White
        k

    let tb() =  
        maxCountBuilding() 
        |> calcTimeBuildingGlasses 

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

    let showViewChart() =
        printfn "Enter path of data loading building test:"
        printfn "(default path: %s)" pathData
        let path = Console.ReadLine()
        let showExp(msg : Exception) = 
            Console.ForegroundColor <- ConsoleColor.Red
            printfn "%s" msg.Message
            Console.ForegroundColor <- ConsoleColor.White
            None
        let isOkPath path = 
            Console.ForegroundColor <- ConsoleColor.Green
            printfn "Data loaded: %s" path
            Console.ForegroundColor <- ConsoleColor.White
        let tryLoadData =
            try
                let series = path |> readSeries
                series |> Some
            with
            | :? FileNotFoundException as ex -> 
                ex |> showExp

            | :? ArgumentException as ex ->
                ex |> showExp

        let next =
            if Option.isSome tryLoadData then
                path |> isOkPath
        next
    
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

    let showMenu() = 
        while(true) do
            printfn "Menu program, enter number:"
            printfn "1) Run loading testing;"
            printfn "2) Build chart;"
            printfn "0) Exit."
            let number() = Console.ReadLine() |> int
            number() |> toMenu |> runMenuTask |> ignore
    showMenu()
    0