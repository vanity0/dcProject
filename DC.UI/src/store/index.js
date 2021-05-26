import Vue from 'vue'
import Vuex from 'vuex'

// 系统配置
import app from './modules/app'
// 用户信息
import user from './modules/user'

// 菜单路由配置在后端
import permission from './modules/async-router'
// 获取状态管理
import getters from './getters'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    app,
    user,
    permission
  },
  state: {

  },
  mutations: {

  },
  actions: {

  },
  getters
})
