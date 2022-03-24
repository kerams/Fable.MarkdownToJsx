namespace Fable.MarkdownToJsx

open Fable.Core.JsInterop
open Fable.React
open Fable.Core

type IParsingOption = interface end    

type IOverride = interface end

[<Erase>]
type Override =
    [<Emit("({ children, ...props }) => $0(Object.entries(props), children)")>]
    static member inline childrenPropsSpread (_f: 'a -> seq<ReactElement> -> ReactElement): obj = jsNative

    static member inline tag (tag: string, replacement: 'props -> ReactElement): IOverride = !!(tag, replacement)
    static member inline tag (tag: string, replacement: #seq<Props.IHTMLProp> -> seq<ReactElement> -> ReactElement): IOverride = !!(tag, Override.childrenPropsSpread replacement)
    
    static member inline tag (tag: string, props: seq<string * string>, replacement: 'props -> ReactElement): IOverride =
        !!(tag, {|
            ``component`` = replacement
            props = createObj !!props
        |})

    static member inline tag (tag: string, props: seq<string * obj>, replacement: 'props -> ReactElement): IOverride =
        !!(tag, {|
            ``component`` = replacement
            props = createObj props
        |})

    static member inline tag (tag: string, props: seq<string * string>, replacement: seq<Props.IHTMLProp> -> seq<ReactElement> -> ReactElement): IOverride =
        !!(tag, {|
            ``component`` = Override.childrenPropsSpread replacement
            props = createObj !!props
        |})

    static member inline tag (tag: string, props: seq<string * obj>, replacement: seq<Props.IHTMLProp> -> seq<ReactElement> -> ReactElement): IOverride =
        !!(tag, {|
            ``component`` = Override.childrenPropsSpread replacement
            props = createObj props
        |})

[<Erase>]
type ParsingOption =
    static member inline forceInline: IParsingOption = !!("forceInline", true)
    static member inline forceBlock: IParsingOption = !!("forceBlock", true)
    static member inline forceWrapper: IParsingOption = !!("forceWrapper", true)
    static member inline slugify (func: string -> string): IParsingOption = !!("slugify", func)
    static member inline disableParsingRawHTML: IParsingOption = !!("disableParsingRawHTML", true)
    static member inline wrapper (element: string): IParsingOption = !!("wrapper", element)
    static member inline createElement createElement: IParsingOption = !!("createElement", System.Func<'element, 'props, ReactElement, ReactElement> createElement)
    static member inline namedCodesToUnicode (replacements: seq<string * string>): IParsingOption = !!("namedCodesToUnicode", createObj !!replacements)
    static member inline overrides (overrides: seq<IOverride>): IParsingOption = !!("overrides", createObj !!overrides)

[<Erase>]
type Markdown =
    /// Create an options object that can be passed to `render (md, options: obj)`.
    /// This might be slightly faster than defining the options inline in `render (md, options: seq<IParsingOption>)`, but is generally not necessary.
    static member inline prepareOptions (options: seq<IParsingOption>) = createObj [ "options", createObj !!options ]

    /// Render markdown text with customized parsing options
    static member inline render (md, options: seq<IParsingOption>) = ReactBindings.React.createElement (importDefault "markdown-to-jsx", Markdown.prepareOptions options, [ str md ])

    /// Render markdown text with options processed by `prepareOptions` in advance
    static member inline render (md, options: obj) = ReactBindings.React.createElement (importDefault "markdown-to-jsx", options, [ str md ])

    /// Render markdown text with default parsing options
    static member inline render md = ReactBindings.React.createElement (importDefault "markdown-to-jsx", null, [ str md ])
