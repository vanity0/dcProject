import Vue from 'vue'
import App from './App'

import home from './pages/home/index.vue'
Vue.component('home',home)

import product from './pages/product/index.vue'
Vue.component('product',product)

import user from './pages/user/index.vue'
Vue.component('user',user)

import cuCustom from './colorui/components/cu-custom.vue'
Vue.component('cu-custom',cuCustom)

import api from './utils/api';
Vue.prototype.$api = api;

Vue.config.productionTip = false

App.mpType = 'app'

const app = new Vue({
    ...App
})
app.$mount()

 



