open FSharp.Charting

open LoadingPlugin.Tests.Views
open LoadingPlugin.Tests.ViewsHelpers

open System.IO
open System

[<EntryPoint>]
let main argv = 
    let selectedNumber() = 
        try
            printfn "Input number of item menu:"
            let number = Console.ReadLine() |> int
            let numberOutRange = 
                (number >= 0 && number <= Menu.CountMenuItem-1) = false
            if numberOutRange then
                raise (FormatException "Selected correct number")
            number |> Some  
        with
        | :? System.FormatException as ex -> 
            ex |> showExp

    let rec showMenu() =
        showMainMenu()
        selectedNumber()
        |> toMenu 
        |> runMenuTask 
        |> Console.Clear
        showMenu()
    showMenu()
    0