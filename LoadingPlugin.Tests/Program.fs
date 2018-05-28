open FSharp.Charting

open LoadingPlugin.Tests.Views
open LoadingPlugin.Tests.ViewsHelpers

open System.IO
open System

[<EntryPoint>]
let main argv = 
    let showMenu() = 
        while(true) do
            printfn "Menu program, enter number:"
            printfn "1) Run loading testing;"
            printfn "2) Build chart;"
            printfn "0) Exit."
            let number() = Console.ReadLine() |> int
            number() |> toMenu |> runMenuTask |> ignore
    showMenu()
    0