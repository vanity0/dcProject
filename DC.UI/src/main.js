/**
 * ../ 上一级目录
 * /   项目根目录
 * ./ 当前同一级目录
 */

// with polyfills
import 'core-js/stable'
import 'regenerator-runtime/runtime'

import Vue from 'vue'
import App from './App.vue'
// 引入路由管理依赖库
import router from './router'
import store from './store/'
import { VueAxios } from './utils/hettpUtils/httpRequest'
// mount axios Vue.$http and this.$http
Vue.use(VueAxios)

import bootstrap from './core/bootstrap'
import './core/lazy_use'
// 引入路由守卫
import './permission' // permission control
import './utils/constUtils/filter' // global filter
import './components/global.less'
import { Dialog } from '@/components'
Vue.use(Dialog)

Vue.config.productionTip = false

//引入全局常量
import constant from './utils/constUtils/const'
Vue.use(constant);

import '@/utils/appUtils/app'

// 引入图标库
import "./assets/iconfont/iconfont.js"

// 引入图表依赖库，https://echarts.apache.org/zh/tutorial.html#5%20%E5%88%86%E9%92%9F%E4%B8%8A%E6%89%8B%20ECharts
import * as echarts from 'echarts'
Vue.prototype.$echarts = echarts

// 引入复制到粘贴板依赖库
import VueClipboard from 'vue-clipboard2'
VueClipboard.config.autoSetContainer = true
Vue.use(VueClipboard)

import './styles/public.less'

// 以阻止 vue 在启动时生成生产提示
Vue.config.productionTip = false
new Vue({
  router, //默认表示 router:router
  store, //将我们创建的Vuex实例挂载到这个vue实例中
  // init localstorage, vuex
  created: bootstrap,
  render: h => h(App)
})
  //手动挂载, 当Vue实例没有el属性时，则该实例尚没有挂载到某个dom中；假如需要延迟挂载，可以在之后手动调用vm.$mount()方法来挂载
  .$mount('#app')