const path = require('path')
const webpack = require('webpack')
const createThemeColorReplacerPlugin = require('./config/plugin.config')

// 引用gzip暴力压缩
const CompressionPlugin = require('compression-webpack-plugin')

function resolve(dir) {
    return path.join(__dirname, dir)
}

const isProd = process.env.NODE_ENV === 'tst'

const assetsCDN = { // webpack build externals
    externals: {
        vue: 'Vue',
        'vue-router': 'VueRouter',
        vuex: 'Vuex',
        axios: 'axios'
    },
    css: [],
    // https://unpkg.com/browse/vue@2.6.10/
    js: ['//cdn.jsdelivr.net/npm/vue@2.6.10/dist/vue.min.js', '//cdn.jsdelivr.net/npm/vue-router@3.1.3/dist/vue-router.min.js', '//cdn.jsdelivr.net/npm/vuex@3.1.1/dist/vuex.min.js', '//cdn.jsdelivr.net/npm/axios@0.19.0/dist/axios.min.js']
}
const Timestamp = new Date().getTime();
// vue.config.js
const vueConfig = {
    configureWebpack: { // webpack plugins
        devtool: 'source-map',
        output: { // 输出重构  打包编译后的 文件名称  【模块名称.时间戳】
            filename: `[name].${Timestamp}.js`,
            chunkFilename: `[name].${Timestamp}.js`
        },
        plugins: [
            // Ignore all locale files of moment.js
            new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),
            //提供带 Content-Encoding 编码的压缩版的资源
            new CompressionPlugin({
                algorithm: 'gzip',
                test: /\.js$|\.html$|\.css/,// 匹配文件名
                // test: /\.(js|css)$/,         
                threshold: 10240,            // 对超过10k的数据压缩
                deleteOriginalAssets: false, // 不删除源文件
                minRatio: 0.8                // 压缩比
            }),
        ],
        // if prod, add externals
        // externals: isProd ? assetsCDN.externals : {}
    },

    chainWebpack: (config) => {
        config.resolve.alias
            .set('@$', resolve('src'))
            .set('_c', resolve('src/components'))

        const svgRule = config.module.rule('svg')
        svgRule.uses.clear()
        svgRule.oneOf('inline').resourceQuery(/inline/).use('vue-svg-icon-loader').loader('vue-svg-icon-loader').end().end().oneOf('external').use('file-loader').loader('file-loader').options({ name: 'assets/[name].[hash:8].[ext]' })

        // if prod is on
        // assets require on cdn
        if (isProd) {
            config.plugin('html').tap(args => {
                args[0].cdn = assetsCDN
                return args
            })
        }
    },

    css: {
        loaderOptions: {
            less: {
                modifyVars: { // less vars，customize ant design theme
                    'primary-color': '#007BF9',
                    'layout-color': '#007BF9',
                    'border-radius-base': '2px'
                },
                // DO NOT REMOVE THIS LINE
                javascriptEnabled: true
            }
        }
    },

    devServer: {
        // development server port 8000
        host: '0.0.0.0',
        open: true,
        port: 8000,
        // If you want to turn on the proxy, please remove the mockjs /src/main.jsL11
        // proxy: {
        //     '/pms': {
        //         //这里后台的地址模拟的;应该填写你们真实的后台接口
        //         target: 'http://www.baudu.com/pms',
        //         ws: true,//代理 websockets
        //         changeOrigin: true//允许跨域
        //     }
        // }
    },

    // disable source map in production
    productionSourceMap: false,
    lintOnSave: undefined,
    // babel-loader no-ignore node_modules/*
    transpileDependencies: [],
    publicPath: "/"
}

// preview.pro.loacg.com only do not use in your production;
if (process.env.VUE_APP_PREVIEW === 'true') {
    // add `ThemeColorReplacer` plugin to webpack plugins
    vueConfig.configureWebpack.plugins.push(createThemeColorReplacerPlugin())
}

module.exports = vueConfig
