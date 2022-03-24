module Fable.MarkdownToJsx.Docs

open Fable.React
open Fable.React.Props
open Browser
open Fable.MarkdownToJsx

let md = """
# This is Markdown

#### You can edit me!

[Markdown](http://daringfireball.net/projects/markdown/) lets you write content in a really natural way.

  * You can have lists, like this one
  * Make things **bold** or *italic*
  * Embed snippets of `code`
  * Create [links](/)
  * Replace named HTML symbols 'This text is &le; than this text.'
  * Create tables

| Column1    | Column2  |
| ---------: | :------- |
| Cell1      | ✓        |
| Cell2      | ✓        |
| Cell3      | ✓        |
| Cell4      | ✓        |

<small>Sample content borrowed with thanks from [elm-markdown](http://elm-lang.org/examples/markdown) ❤️</small>

You can even include custom React components if you declare them in the "overrides" option.

## Or override tags like this h2 with a custom class!

<MyComponent>Isn't that cool?</MyComponent>

<AnotherComponent text="value" number={8}></AnotherComponent>

This snipped was rendered from F# in Fable using

```fsharp
let AnotherComponent = FunctionComponent.Of (fun (x: {| text: string; number: int |}) ->
    div [] [
        strf "In a function component with provided text `%s` and number `%d`." x.text x.number
    ])

let myView props children =
    button (OnClick (fun _ -> window.alert "I'm sort of a component!") :: Class "shiny" :: props) children

Markdown.render (md.current, [
    ParsingOption.namedCodesToUnicode [ "le", "\u2264" ]
    ParsingOption.overrides [
        Override.tag ("h2", [ "className", "red" ], h4)
        Override.tag ("AnotherComponent", AnotherComponent)
        Override.tag ("MyComponent", myView)
    ]
])
```"""

let AnotherComponent = FunctionComponent.Of (fun (x: {| text: string; number: int |}) ->
    div [] [
        strf "In a function component with provided text `%s` and number `%d`." x.text x.number
    ])

let myView props children =
    button (OnClick (fun _ -> window.alert "I'm sort of a component!") :: Class "shiny" :: props) children

let View = FunctionComponent.Of (fun () ->
    let md = Hooks.useState md

    main [] [
        header [] [
            a [ Target "_blank"; Href "https://github.com/kerams/Fable.MarkdownToJsx" ] [ img [ Src "https://probablyup.com/markdown-to-jsx/images/logo.svg" ] ]
            p [] [
                h1 [] [
                    code [] [ str "Fable.MarkdownToJsx" ]
                    str " is a Fable F# binding for "
                    code [] [ a [ Target "_blank"; Href "https://github.com/probablyup/markdown-to-jsx" ] [ str "markdown-to-jsx" ] ]
                    str ", which is an easy-to-use markdown component that takes Github-flavored Markdown (GFM) and makes native JSX without dangerous hacks."
                ]
                h2 [] [
                    a [ Target "_blank"; Href "https://github.com/kerams/Fable.MarkdownToJsx/blob/master/Fable.MarkdownToJsx.Docs/Main.fs" ] [ str "Find the source code for this page here." ]
                ]
            ]
        ]
        section [ Class "demo" ] [
            textarea [ Class "md"; Value md.current; OnChange (fun x -> md.update x.Value) ] []

            div [ Class "compiled" ] [
                Markdown.render (md.current, [
                    ParsingOption.namedCodesToUnicode [ "le", "\u2264" ]
                    ParsingOption.overrides [
                        Override.tag ("h2", [ "className", "red" ], h4)
                        Override.tag ("AnotherComponent", AnotherComponent)
                        Override.tag ("MyComponent", myView)
                    ]
                ])
            ]
        ]
    ])


ReactDom.render (View (), document.getElementById "a")