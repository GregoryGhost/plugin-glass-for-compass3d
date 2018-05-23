open FSharp.Charting
open LoadingPlugin.Tests.BuildingGlassesTest
open LoadingPlugin.Tests.BuildingChart
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
        |> calcTimeBuildingGlasses 

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
            |> chartsTimeBuilding 
            |> charts |> Chart.Show
            printfn "Drawing plot?(y/n)"
            let r = Console.ReadLine() |> string
            if(r <> "y") then repeat <- false
    0