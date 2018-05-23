namespace LoadingPlugin.Tests

module Series =
    open System.IO
    open FSharp.Json

    ///Нагрузочный тест - постройка одного стакана в САПР.
    type LoadingTest = {
        TimeBuildingGlass    : double
        ;NumberBuildingGlass : int }

    ///Серия - несколько нагрузочных тестов.
    type Serie = LoadingTest list

    ///Серии - нагрузочные тесты для разных типов стакана.
    type Series = {
        CleanGlass   : Serie
        FacetedGlass : Serie
        CrimpGlass   : Serie }

    ///Время, затраченное на построение разных типов стакана за серию.
    type SummaryTimeBuilding = {
        CleanGlasses   : double
        FacetedGlasses : double
        CrimpGlasses   : double }

    ///Конвертация серии данных в JSON-формат. 
    let toJson data = Json.serialize data

    ///Получение серии данных из JSON-формата.
    let toSeries jData = Json.deserialize<Series> jData

    ///Прочитать серию данных из указанного файла.
    let readSeries(path : string) = 
        use sr = new StreamReader(path)
        let jSeries = sr.ReadToEnd()
        sr.Close()
        jSeries |> toSeries
    
    ///Записать серию данных (в JSON) в указанный файл.
    let writeSeries(series : string, path : string) =
        use sw = new StreamWriter(path)
        sw.WriteLineAsync(series) |> ignore
        sw.Close()

    ///Посчитать общее время для каждой нагрузочной серии
    let sumForEach series =
        let sum (series : Serie) =
            series 
            |> List.map (fun s -> s.TimeBuildingGlass)
            |> List.sum
        let clean = 
            series.CleanGlass |> sum 
        let faceted =
            series.FacetedGlass |> sum
        let crimp =
            series.CrimpGlass |> sum
        {CleanGlasses = clean
            ;FacetedGlasses = faceted
            ;CrimpGlasses = crimp }
    
    ///Посчитать общее время всех нагрузочных серий.
    let sumForAll series =
        series.CleanGlasses 
        + series.FacetedGlasses 
        + series.CrimpGlasses


module BuildingChart =
    open FSharp.Charting
    open System

    let timeBuilding(times : ((int * double) list * string) list) = 
        times
        |> List.map (fun (points, name) -> 
            let _, y = points |> List.unzip
            let y1 = List.sum y
            (y1, name)   )

    let genChart(glass : (int * double) list * string) =
        let (data, name) = glass
        Chart.Line(data, Name = name)

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
     
    let charts c =
        c
        |> Chart.Combine
        |> Chart.WithLegend(
            TitleAlignment = Drawing.StringAlignment.Center,
            Docking = ChartTypes.Docking.Right, 
            InsideArea = true)
        |> Chart.WithXAxis(
            Title = "Количество построенных стаканов",
            Min = 0.0) 
        |> Chart.WithYAxis(
            Title = "Время построения, с",
            Min = 0.0)