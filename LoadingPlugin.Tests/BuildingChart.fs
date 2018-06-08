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
    type Series = (Serie * string) list
        
    ///Время, затраченное на построение разных типов стакана за серию.
    type SummaryTimeBuilding = (double * string) list

    ///Сырые данные, снятые во время нагрузочного тестирования,
    /// за серию.
    type SourceDataSerie = (int * double) list

    ///Сырые данные, снятые во время нагрузочного тестирования,
    /// за серии.
    type SourceDataSeries = ((int * double) list * string) list

    ///Конвертация серии данных в JSON-формат. 
    let toJson data = Json.serialize data

    ///Получение серии данных из JSON-формата.
    let toSeries jData = Json.deserialize<Series> jData

    ///Конвертировании полученных данных при работе нагрузочных тестов в 
    /// серии нагрузочных тестов
    let convertToSeries(data : ((int * double) list * string) list) =
        let series =
            data
            |> List.map (fun (points, name) -> 
                    let serie = 
                        points 
                        |> List.map (fun (x, y) ->
                            {TimeBuildingGlass = y; NumberBuildingGlass = x})
                    (serie, name)  )
        series
    
    ///Конвертирование серии нагрузочных тестов в данные по
    /// нагрузочным тестам
    let convertToData(series : Series) =
        let data =
            series
            |> List.map (fun (points, name) ->
                    let sData =
                        points
                        |> List.map (fun p -> 
                            (p.NumberBuildingGlass, p.TimeBuildingGlass))
                    (sData, name)   )
        data

    ///Прочитать серию данных из указанного файла.
    let readSeries(path : string) = 
        let sr = new StreamReader(path)
        let series = 
            sr.ReadToEnd()
            |> toSeries
        sr.Close()
        sr.Dispose()
        series 
    
    ///Записать серию данных (в JSON) в указанный файл.
    let writeSeries(series : string, path : string) =
        let sw = new StreamWriter(path)
        sw.WriteLine(series) |> ignore
        sw.Close()
        sw.Dispose()

    ///Посчитать общее время для каждой нагрузочной серии
    let sumForEach serie =
        let sum (serie : Serie) =
            serie 
            |> List.map (fun s -> s.TimeBuildingGlass)
            |> List.sum
        serie |> sum
    
    ///Посчитать общее время всех нагрузочных серий.
    let sumForAll (series : Series) =
        series 
        |> List.map (fun (d, n) -> 
                let s = d |> sumForEach
                (s, n)  )


module BuildingChart =
    open FSharp.Charting
    open System
    open Series

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

    let printChart(data : Series) =
        data
        |> convertToData
        |> chartsTimeBuilding
        |> charts |> Chart.Show |> Some