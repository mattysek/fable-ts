module Client

open Elmish
open Elmish.React

open Fable.Core
open Fable.Core.JsInterop

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Helpers.React.ReactiveComponents

//Types representing model and commands
module Types = 

    // The model holds data that you want to keep track of while the application is running
    type Model = { Counter: int }

    // The Msg type defines what events/actions can occur while the application is running
    // the state of the application changes *only* in reaction to these events
    type Msg =
        | Increment
        | Decrement

//State management
module State =

    open Types

    // defines the initial state and initial command (= side-effect) of the application
    let init () : Model * Cmd<Msg> =
        { Counter = 0 }, Cmd.none

    // The update function computes the next state of the application based on the current state and the incoming events/messages
    // It can also run side-effects encoded as commands
    // these commands in turn, can dispatch messages to which the update function will react.
    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
        match msg with
        | Increment ->
            { model with Counter = model.Counter + 1 }, Cmd.none
        | Decrement ->
            { model with Counter = model.Counter - 1 }, Cmd.none

// Type wrapper for TS import
module Counter =

    type Procedure = unit -> unit //aka () => void

    // interface Props
    type Props =
      | Name of string
      | OnIncrement of Procedure
      | OnDecrement of Procedure

    // function Counter({ name, onIncrement, onDecrement }: Props)
    let inline counter (props : Props list) : Fable.Import.React.ReactElement =
        ofImport "default" "./Counter.tsx" (keyValueList CaseRules.LowerFirst props) []

//View of our application
module View =

    open Types
    open Counter

    let simpleButton text onClick =
        button [ OnClick onClick ] [ str text ] 

    let view (model : Model) (dispatch : Msg -> unit) =
        div [] [ span [] [ str (sprintf "COUNT: %i" model.Counter) ]
            
                 div [] [ str "Fable Counter" ]
                 div [] [ simpleButton "-" (fun _ -> dispatch Decrement)
                          simpleButton "+" (fun _ -> dispatch Increment) ]

                 counter [ Name "TS Counter"
                           OnIncrement (fun _ -> dispatch Increment)
                           OnDecrement (fun _ -> dispatch Decrement) ] ]


open State
open View

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
