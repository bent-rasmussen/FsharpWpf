namespace Statical.Prism.ViewModel

open Statical.Prism

type MainViewModel () =
    inherit ViewModelBase()

    let mutable foo = 0

    member this.WorldName = "Elmish"

    member this.PropWithSetter
        with get () = foo
        and set value = this.Set(&foo, value)
