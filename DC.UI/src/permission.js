import router from './router'
import store from './store'
// import storage from 'store'
import NProgress from 'nprogress' // progress bar
import '@/components/NProgress/nprogress.less' // progress bar custom style
// import notification from 'ant-design-vue/es/notification'
import { setDocumentTitle, domTitle } from '@/utils/domUtil'
import { getAccessToken } from '@/utils/tokenUtils/accessTokenCache'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const allowList = ['login', 'devprod', 'regprod'] // no redirect allowList
const loginRoutePath = '/user/login'
const defaultRoutePath = '/home/index'

router.beforeEach((to, from, next) => {
  NProgress.start() 
  if (to.name === 'regprod' || to.name==='devprod') {
    to.meta && (typeof to.meta.title !== 'undefined' && setDocumentTitle(`详情`))
  } else {
    to.meta && (typeof to.meta.title !== 'undefined' && setDocumentTitle(`${to.meta.title} - ${domTitle}`))
  }

  // 获取token
  let token = getAccessToken();
  // token存在
  if (token) {
    if (to.path === loginRoutePath) {
      next({ path: defaultRoutePath })
      NProgress.done()
    } else {
      if (store.getters.roles.length === 0) {
        store.dispatch('getUserInfo').then(res => {
          const roles = res.data && res.data.role
          // generate dynamic router
          store.dispatch('GenerateRoutes', { roles }).then(() => {
            // 根据roles权限生成可访问的路由表
            // 动态添加可访问路由表
            router.addRoutes(store.getters.addRouters)
            // 请求带有 redirect 重定向时，登录自动重定向到该地址
            const redirect = decodeURIComponent(from.query.redirect || to.path)
            if (to.path === redirect) {
              next({ ...to, replace: true }) // 设置replace: true，这样导航就不会留下历史记录
            } else {
              // 跳转到目的路由
              next({ path: redirect })
            }
          })
        })
          .catch((error) => {
            // notification.error({
            //   message: '错误',
            //   description: '请求用户信息失败，请重试:' + error || ''
            // })
            // 失败时，获取用户信息失败时，调用登出，来清空历史保留信息
            store.dispatch('logout').then(() => {
              // next({ path: loginRoutePath,  query: { redirect: to.fullPath } })
              next({ path: loginRoutePath })
            })
          })
      } else {
        next()
      }
    }
  } else {
    if (allowList.includes(to.name)) {
      // 在免登录名单，直接进入
      next()
    } else {
      // 跳转到登录界面
      next({ path: loginRoutePath })
      // next({ path: loginRoutePath, query: { redirect: to.fullPath } })
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  NProgress.done()
})

// 解决Loading chunk (\d)+ failed问题
router.onError((error) => {
  const pattern = /Loading chunk (\d)+ failed/g;
  const isChunkLoadFailed = error.message.match(pattern);
  if (isChunkLoadFailed) {
    // 用路由的replace方法，并没有相当于F5刷新页面，失败的js文件并没有从新请求，会导致一直尝试replace页面导致死循环，
    // const targetPath = router.history.pending.fullPath;
    // router.replace(targetPath);
    // 而用 location.reload 方法，相当于触发F5刷新页面，虽然用户体验上来说会有刷新加载察觉，但不会导致页面卡死及死循环
    location.reload();
  }
});