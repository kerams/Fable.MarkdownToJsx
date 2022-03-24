# Fable.MarkdownToJsx

Fable bindings for [markdown-to-jsx](https://github.com/probablyup/markdown-to-jsx) ([NPM package](https://www.npmjs.com/package/markdown-to-jsx)) version 7.1.0+.
Online demo at https://kerams.github.io/Fable.MarkdownToJsx/.

## Nuget package
[![Nuget](https://img.shields.io/nuget/v/Fable.MarkdownToJsx.svg?colorB=green)](https://www.nuget.org/packages/Fable.MarkdownToJsx)

## Installation with [Femto](https://github.com/Zaid-Ajaj/Femto)

```
femto install Fable.MarkdownToJsx
```

## Standard installation

Nuget package

```
paket add Fable.MarkdownToJsx -p YourProject.fsproj
```

NPM package

```
npm install markdown-to-jsx@7.1.7
```

## Usage

Use the `Markdown` object to render markdown source texts into a `ReactElement`. Passing in just the text will use default options.
Parsing customization is done with a [Feliz](https://github.com/Zaid-Ajaj/Feliz)-flavored API. Check the [`markdown-to-jsx` documentation](https://github.com/probablyup/markdown-to-jsx#parsing-options) for all options.

```fsharp
open Fable.MarkdownToJsx
open Fable.React
open Fable.React.Props

let simpleRender: ReactElement = Markdown.render "# Markdown!"

let AnotherComponent = FunctionComponent.Of (fun (x: {| text: string; number: int |}) ->
    div [] [
        strf "In a function component with provided text `%s` and number `%d`." x.text x.number
    ])

let myView props children =
    button (OnClick (fun _ -> window.alert("I'm a real component!")) :: Class "shiny" :: props) children

let customizedRender source =
	Markdown.render (source, [
		// Adds a replacement for the `&le;` (≤) HTML symbol
		ParsingOption.namedCodesToUnicode [ "le", "\u2264" ]
		// Defines replacements for tags
		ParsingOption.overrides [
			// h2 (##) will be replaced with h4 with a custom class
			Override.tag ("h2", [ "className", "red" ], h4)
			// The tag AnotherComponent will be mapped onto a React component defined beforehand
			// The props object is passed to the component
			Override.tag ("AnotherComponent", AnotherComponent)
			// The tag MyComponent will be mapped onto a view function defined beforehand
			// Props and children are passed to the function
			Override.tag ("MyComponent", myView)
		]
	])
```
