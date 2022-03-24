var path = require("path");
var HtmlWebpackPlugin = require('html-webpack-plugin');
const TerserPlugin = require("terser-webpack-plugin");

var CONFIG = {
    indexHtmlTemplate: "./Fable.MarkdownToJsx.Docs/index.html",
    fsharpEntry: "./Fable.MarkdownToJsx.Docs/Main.fs.js",
    outputDir: "./docs",
    devServerPort: 8088
}

var isProduction = !process.argv.find(v => v.indexOf('serve') !== -1);
//var isProduction = true;
console.log("Bundling for " + (isProduction ? "production" : "development") + "...");

module.exports = [{
    entry: {
        a: [resolve(CONFIG.fsharpEntry)]
    },
    output: {
        path: resolve(CONFIG.outputDir),
        filename: '[name].js'
    },
    mode: isProduction ? "production" : "development",
    devServer: {
        port: CONFIG.devServerPort,
        hot: !isProduction
    },
    optimization: isProduction ? {
        minimize: isProduction,
        minimizer: [
            new TerserPlugin({
                extractComments: false,
                terserOptions: {
                    format: {
                        comments: false
                    }
                }
            })
        ],
    } : {},
    plugins: [
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: resolve(CONFIG.indexHtmlTemplate)
        })
    ]
}];

function resolve(filePath) {
    return path.isAbsolute(filePath) ? filePath : path.join(__dirname, filePath);
}