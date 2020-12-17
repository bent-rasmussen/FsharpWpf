module MainApp

open Statical.Prism
open Statical.Prism.ViewModel
open System
open System.Windows

[<STAThread()>]
[<EntryPoint>]
let main(args : string[]) =
    let app = XamlLoader.LoadComponent("Xaml/App.xaml") :?> Application
    let window = XamlResources.MainWindow()
    window.DataContext <- new MainViewModel()
    window.ShowDialog() |> ignore
    0
