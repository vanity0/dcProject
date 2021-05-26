
import * as loginService from '@/api/login'
import { BasicLayout, BlankLayout, PageView, RouteView } from '@/layouts'

// 前端路由表
const constantRouterComponents = {
  // 基础页面 layout 必须引入
  BasicLayout: BasicLayout,
  BlankLayout: BlankLayout,
  RouteView: RouteView,
  PageView: PageView,
  '401': () => import(/* webpackChunkName: "error" */ '@/views/exception/401'),
  '404': () => import(/* webpackChunkName: "error" */ '@/views/exception/404'),
}

// 根级菜单
let rootRouter = {
  key: '',
  name: 'home',
  path: '/',
  component: 'BasicLayout',
  redirect: '/home',
  meta: {
    title: '烟花联盟',
    hidden: false,
    hideChildren: false,
    hiddenHeaderContent: false,
  },
  children: []
}

/**
 * 动态生成菜单
 * @param token
 * @returns {Promise<Router>}
 */
export const generatorDynamicRouter = (token) => {
  return new Promise((resolve, reject) => {
    loginService.getNavRouters(token).then(res => {
      let menuNav = []
      // 如果没有返回菜单，则这是默认跳转菜单为空页面菜单
      if (res.data.length > 0) {
        rootRouter.children = res.data
      } else {
        // 前端未找到页面路由（固定不用改）
        rootRouter.children = {
          path: '*', redirect: '/404', hidden: true
        }
      }
      // rootRouter.children = rootRouter.children.concat(staticRoute)
      menuNav.push(rootRouter)
      const routers = generator(menuNav)
      // 设置路由菜单为第一个页面
      if (routers[0].children) {
        for (let i = 0; i < routers[0].children.length; i++) {
          // 如果当前菜单存在子集,则设置该子集为首页
          if (routers[0].children[i].children) {
            routers[0].redirect = routers[0].children[i].children[0].path
          } else {
            routers[0].redirect = routers[0].children[i].path
          }
          break;
        }
      }
      console.log(routers)
      resolve(routers)
    }).catch(err => {
      reject(err)
    })
  })
}

/**
 * 格式化树形结构数据 生成 vue-router 层级路由表
 *
 * @param routerMap
 * @param parent
 * @returns {*}
 */
export const generator = (routerMap, parent) => {
  return routerMap.map(item => {
    const { title, hidden, hideChildren, hiddenHeaderContent, target, icon } = item.meta || {}

    // 按钮权限 
    let buttonLimit = []
    if (item.buttons) {
      item.buttons.forEach((code) => {
        if (code) {
          buttonLimit.push(code)
        }
      })
    }

    const currentRouter = {
      // 如果路由设置了 path，则作为默认 path，否则 路由地址 动态拼接生成如 /dashboard/workplace
      path: item.path, //|| `${parent && parent.path || ''}/${item.name}`,
      // 路由名称，建议唯一
      name: item.name || item.key || '',
      // 该路由对应页面的 组件 :方案2 (动态加载)
      component: (constantRouterComponents[item.component]) || (() => import(`@/views/${item.component}`)),
      // meta: 页面标题, 菜单图标, 页面权限(供指令权限用，可去掉)
      meta: {
        title: title,
        icon: icon || undefined,
        hiddenHeaderContent: hiddenHeaderContent || false,
        target: target,
        buttonLimit: buttonLimit,
      }
    }
    // 是否设置了隐藏菜单
    currentRouter.hidden = hidden || false
    // 是否设置了隐藏子菜单
    currentRouter.hideChildrenInMenu = hideChildren || false
    // 为了防止出现后端返回结果不规范，处理有可能出现拼接出两个 反斜杠
    if (!currentRouter.path.startsWith('http')) {
      currentRouter.path = currentRouter.path.replace('//', '/')
    }
    // 重定向
    item.redirect && (currentRouter.redirect = item.redirect)
    // 是否有子菜单，并递归处理
    if (item.children) {
      if (item.children.length > 0) {
        currentRouter.children = generator(item.children, currentRouter)
      }
    }
    return currentRouter
  })
}

/**
 * 数组转树形结构
 * @param list 源数组
 * @param tree 树
 * @param parentId 父ID
 */
const listToTree = (list, tree, parentId) => {
  list.forEach(item => {
    // 判断是否为父级菜单
    if (item.parentId === parentId) {
      const child = {
        ...item,
        key: item.key || item.name,
        children: []
      }
      // 迭代 list， 找到当前菜单相符合的所有子菜单
      listToTree(list, child.children, item.id)
      // 删掉不存在 children 值的属性
      if (child.children.length <= 0) {
        delete child.children
      }
      // 加入到树中
      tree.push(child)
    }
  })
}
