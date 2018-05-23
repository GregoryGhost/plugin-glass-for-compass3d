namespace LoadingPlugin.Tests

module BuildingGlassesTest =
    open GlassViewsModel
    open System.Diagnostics

    let sw = new Stopwatch()
    let glassesVM = new GlassesViewModel()

    let glasses = [
        for i in 0..glassesVM.CountGlasses-1 ->
            let g = glassesVM.GetGlassByIndex(i)
            g.Filleted <- true
            g ]
    
    let measureTime (glass : GlassViewModel, maxCount : int) = [
        for x in 1..maxCount ->
            sw.Start()
            glass.BuildModel() |> ignore
            sw.Stop()
            let mls = 
                sw.ElapsedMilliseconds |> double
            let timeBuilding = 
                mls / 1000.
            sw.Reset()
            printfn "Building %s glass №%d" glass.Name x
            (x, timeBuilding)  ]
    
    let calcTimeBuild glass n = 
        let maxCountBuilding = n
        let time =
            measureTime(glass, maxCountBuilding)
        let name = glass.Name
        (time, name)

    let timeBuild n =
        let calc g = calcTimeBuild g n
        let times = 
            glasses |> List.map calc
        times