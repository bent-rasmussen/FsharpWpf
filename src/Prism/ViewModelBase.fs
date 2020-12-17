namespace Statical.Prism

open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System
open System.Globalization
open System.Threading.Tasks
open System.Windows
open System.Windows.Controls
open System.Windows.Input
open System.Windows.Threading
open System.Windows.Markup
open System.Windows.Data
open System.ComponentModel
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns
open Serilog

[<AllowNullLiteral>]
type ViewModelBase () =

    let propertyChanged = new Event<_, _>()
    let toPropName(query : Expr) = 
        match query with
        | PropertyGet(a, b, list) ->
            b.Name
        | _ -> ""

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member x.PropertyChanged = propertyChanged.Publish

    // https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/caller-information
    member this.OnPropertyChanged([<CallerMemberName; Optional; DefaultParameterValue("")>] memberName: string) =
        propertyChanged.Trigger(this, new PropertyChangedEventArgs(memberName))

    member this.OnPropertiesChanged([<ParamArray>]memberNames : string[]) =
        for memberName in memberNames do
            propertyChanged.Trigger(this, new PropertyChangedEventArgs(memberName))

    member this.SetAndThen(field : byref<'T>, value : 'T, [<CallerMemberName; Optional; DefaultParameterValue("")>] memberName: string) : bool =
        if field <> value then
            field <- value
            propertyChanged.Trigger(this, new PropertyChangedEventArgs(memberName))
            true
        else
            false

    member this.Set(field : byref<'T>, value : 'T, [<CallerMemberName; Optional; DefaultParameterValue("")>] memberName: string) : unit =
        if field <> value then
            field <- value
            propertyChanged.Trigger(this, new PropertyChangedEventArgs(memberName))

    // Special-use
    member x.OnPropertyExprChanged(expr : Expr) =
        let propName = toPropName(expr)
        propertyChanged.Trigger(x, new PropertyChangedEventArgs(propName))

    member x.PropertyChanged =
        (x :> INotifyPropertyChanged).PropertyChanged
