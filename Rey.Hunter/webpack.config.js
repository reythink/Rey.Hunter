var webpack = require('webpack');
var path = require('path');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    entry: {
        main: './wwwroot/app',
        vendor: './wwwroot/app/vendor.js'
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, './wwwroot/dist')
    },
    module: {
        rules: [
            { test: /\.css$/, use: ExtractTextPlugin.extract({ use: 'css-loader' }) },
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: { presets: ['env'] }
                }
            }
        ]
    },
    plugins: [
        new ExtractTextPlugin('style.bundle.css'),
        new webpack.optimize.CommonsChunkPlugin({
            name: 'vendor',
            minChunks: function (module) {
                return module.context && module.context.indexOf('node_modules') !== -1;
            }
        }),
        new webpack.optimize.CommonsChunkPlugin({ name: 'manifest' }),
    ]
};