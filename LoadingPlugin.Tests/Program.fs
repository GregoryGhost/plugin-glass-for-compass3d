open FSharp.Charting
open LoadingPlugin.Tests.BuildingGlassesTest
open System

[<EntryPoint>]
let main argv = 
    let maxCountBuilding = 
        printfn "Enter max count building of glasses:"
        let k = Console.ReadLine() |> int
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Ok: %d" k
        Console.ForegroundColor <- ConsoleColor.White
        k
    
    let tb =  
        maxCountBuilding 
        |> timeBuild 

    let timesBuilding = 
        tb 
        |> timeBuilding

    let printTimesBuilding =
        let times, _ = timesBuilding |> List.unzip
        let timesBuildingOfGlasses = 
            times |> List.sum 
        printSummary timesBuildingOfGlasses
        printForEachGlass timesBuilding   

    let repeatDrawingPlot =
        let mutable repeat = true
        while(repeat) do
            tb 
            |> chartsBuilding 
            |> charts |> Chart.Show
            printfn "Repeat drawing plot?(y/n)"
            let r = Console.ReadLine() |> string
            if(r <> "y") then repeat <- false
    0