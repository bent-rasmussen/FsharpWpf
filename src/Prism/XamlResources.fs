namespace Statical.Prism

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Resources
open System.Windows.Input
open Serilog
//open FsXaml

// TODO modernize with FsXaml type provider -- open question: how about WinUI 3.0 - will it even work?
//type IconsResourceDictionary = XAML<"Xaml/ResourceDictionaries/Icons.xaml">

[<RequireQualifiedAccess>]
module XamlLoader =

    // var assembly = typeof(MyLibrary.MyClass).GetTypeInfo().Assembly;
    // Stream resource = assembly.GetManifestResourceStream("MyLibrary._fonts.OpenSans.ttf");

    let ResourcePath = "/StaticalPrism;component/"

    let Resource (name : string) : Uri =
        new Uri(ResourcePath + name, UriKind.Relative)

    let LoadComponent (name : string) : obj =
        try
            Application.LoadComponent (Resource name)
        with ex ->
            Log.Error(ex, "Could not load component {resourceName}", name)
            //Ex.throwCapture ex
            reraise()

    let LoadUserControl (name : string) : UserControl =
        let win = (LoadComponent name) :?> UserControl
        win.Tag <- name
        win

    let LoadWindow (name : string) : Window =
        let win = (LoadComponent name) :?> Window
        win.Tag <- name
        win

    let LoadStream (name : string) : StreamResourceInfo =
        try
            Application.GetResourceStream (Resource name)
        with ex ->
            Log.Error(ex, "Could not load stream {resourceName}", name)
            //Ex.throwCapture ex
            reraise()

    let LoadCursor (name : string) : Cursor =
        let sri = LoadStream name
        new Cursor (sri.Stream)

[<RequireQualifiedAccess>]
module XamlResources =

    //let Icons = new IconsResourceDictionary()

    let MainWindow () = 
        XamlLoader.LoadWindow "Xaml/MainWindow.xaml"
