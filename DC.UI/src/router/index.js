import Vue from 'vue'
import Router from 'vue-router'
import { constantRouterMap } from '@/config/router.config'

// 解决路由跳转相同的地址报错
const originalPush = Router.prototype.push
Router.prototype.push = function push(location, onResolve, onReject) {
  try {
    if (onResolve || onReject) return originalPush.call(this, location, onResolve, onReject)
    return originalPush.call(this, location).catch(err => err)
  } catch (error) {
    console.error('error:', error);
  }
}

Vue.use(Router)

export default new Router({
  // 解决vue框架页面跳转有白色不可追踪色块的bug
  scrollBehavior(to, from, savedPosition) {
    return { x: 0, y: 0 }
  },
  mode: 'history',
  base: process.env.BASE_URL,
  routes: constantRouterMap
})


