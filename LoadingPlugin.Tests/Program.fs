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
    let charts = 
        maxCountBuilding
        |> timeBuild 
        |> Chart.Combine
    charts |> Chart.Show
    0