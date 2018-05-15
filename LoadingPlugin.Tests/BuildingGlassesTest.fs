namespace LoadingPlugin.Tests

module BuildingGlassesTest =
    open GlassViewsModel
    open FSharp.Charting
    open System.Diagnostics
    open System

    let sw = new Stopwatch()
    let glassesVM = new GlassesViewModel()

    let glasses = [
        for i in 0..glassesVM.CountGlasses-1 ->
            glassesVM.GetGlassByIndex(i) ]
    
    let measureTime (glass : GlassViewModel, maxCount : int) = [
        for x in 0..maxCount ->
            sw.Start()
            glass.BuildModel() |> ignore
            sw.Stop()
            let mls = 
                sw.ElapsedMilliseconds |> double
            let timeBuilding = 
                mls / 1000.
            (x, timeBuilding)    ]
    
    let calcTimeBuild glass n = 
        let maxCountBuilding = n
        let time =
            measureTime(glass, maxCountBuilding)
        let name = glass.Name
        (time, name)

    let genChart(glass : (int * double) list * string) =
        let (data, name) = glass
        Chart.Line (data, Name = name)

    let timeBuild n =
        let calc g = calcTimeBuild g n
        glasses
        |> List.map calc
        |> List.map genChart