open FSharp.Charting
open LoadingPlugin.Tests.BuildingGlassesTest

[<EntryPoint>]
let main argv = 
    let charts = timeBuild |> Chart.Combine
    charts |> Chart.Show
    0