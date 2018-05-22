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
            (x, timeBuilding)    ]
    
    let calcTimeBuild glass n = 
        let maxCountBuilding = n
        let time =
            measureTime(glass, maxCountBuilding)
        let name = glass.Name
        (time, name)

    let genChart(glass : (int * double) list * string) =
        let (data, name) = glass
        Chart.Line(data, Name = name)

    let timeBuild n =
        let calc g = calcTimeBuild g n
        let times = 
            glasses |> List.map calc
        times

    let timeBuilding(times : ((int * double) list * string) list) = 
        times
        |> List.map (fun (points, name) -> 
            let _, y = points |> List.unzip
            let y1 = List.sum y
            (y1, name)   )

    let chartsTimeBuilding times = 
        times |> List.map genChart

    let printSummary s =
        Console.ForegroundColor <- ConsoleColor.Green
        printfn "Summary time building of glasses: %f (sec)" s
        Console.ForegroundColor <- ConsoleColor.White

    let printForEachGlass timeBuilding =
            timeBuilding
            |> List.iter (fun (t, n) -> 
                Console.ForegroundColor <- ConsoleColor.Yellow
                printfn "Time building %s glass: %f (sec)" n t
                Console.ForegroundColor <- ConsoleColor.White )
     
    let chartsBuilding tb = 
        tb 
        |> chartsTimeBuilding

    let charts c =
        c
        |> Chart.Combine
        |> Chart.WithLegend(
            TitleAlignment = Drawing.StringAlignment.Center,
            Docking = ChartTypes.Docking.Right, 
            InsideArea = true)
        |> Chart.WithXAxis(
            Title = "Количество построенных стаканов",
            Min = 1.0) 
        |> Chart.WithYAxis(
            Title = "Время построения, с",
            Min = 0.0) 