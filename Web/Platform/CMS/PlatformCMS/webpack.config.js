'use strict';
const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const bundleOutputDir = './wwwroot/dist';
const merge = require('webpack-merge');
const prodn = process.env.NODE_ENV === 'production';
process.noDeprecation = true;
module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    const sharedConfig = () => ({
        performance: {
            hints: false
        },
        stats: { modules: false },
        resolve: {
            extensions: ['.js', '.vue']
        },

        output: {
            //path: path.join(__dirname, bundleOutputDir),
            filename: '[name].js',
            publicPath: '/dist/'
        },

        module: {
            rules: [
                //{ test: /\.vue$/, include: /ClientApp/, use: 'vue-loader' },
                { test: /\.js$/, include: /ClientApp/, use: 'babel-loader' },
                { test: /\.css$/, use: isDevBuild ? ['style-loader', 'css-loader'] : ExtractTextPlugin.extract({ use: 'css-loader', allChunks: true }) },
                {
                    test: /\.vue$/,
                    include: /ClientApp/,
                    loader: 'vue-loader',
                    options: {
                        loaders: {
                            'scss': [
                                'vue-style-loader',
                                'css-loader',
                                {
                                    loader: "sass-loader"
                                }
                            ]

                        },

                    }
                },
                { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                {
                    test: /\.(woff|woff2|eot|ttf|svg)(\?.*$|$)/,
                    loader: 'file-loader'
                }
            ],

        },

        plugins: [
            // new webpack.DllReferencePlugin({
            //    context: __dirname,
            //    manifest: require('./wwwroot/dist/vendor-manifest.json')
            // }),

           //new webpack.optimize.LimitChunkCountPlugin({
           //    maxChunks: 1
           //})
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(bundleOutputDir, '[resourcePath]'), // Point sourcemap entries to the original file locations on disk
                devtool: false
            })
        ] : [
                // Plugins that apply in production builds only
                new webpack.optimize.UglifyJsPlugin(),
                new ExtractTextPlugin('site.css')
            ])
    });


    const clientBundleConfig = merge(sharedConfig(), {
        entry: { 'main-client': './ClientApp/entry-client.js' },
        output: {
            path: path.join(__dirname, bundleOutputDir)
        }

    });

    //const serverBundleConfig = merge(sharedConfig(), {
    //    target: 'node',
    //    entry: { 'main-server': './ClientApp/entry-server.js' },
    //    output: {
    //        libraryTarget: 'commonjs2',
    //        path: path.join(__dirname, bundleOutputDir)
    //    },
    //    module: {
    //        rules: [
    //            {
    //                test: /\.json?$/,
    //                loader: 'json-loader',
    //                exclude: [/lang/, /wwwroot/, /node_modules/]
    //            }
    //        ]
    //    }
    //});

    //return [clientBundleConfig, serverBundleConfig];
    return clientBundleConfig;
    ;
};
