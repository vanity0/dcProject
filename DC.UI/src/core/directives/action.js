import Vue from 'vue'
import store from '@/store'

/**
 * Action 权限指令
 * 指令用法：
 *  - 在需要控制 action 级别权限的组件上使用 v-action:[method] , 如下：
  * <i-button v-action:create >添加用户</a-button>
  * <a-button v-action="remove">删除用户</a-button>
 */
const action = Vue.directive('action', {
  inserted: function (el, binding, vnode) {
    // 是否有权限，默认没有权限
    let hasPermission = false
    // 路由里保存的权限
    const buttonLimit = vnode.context.$route.meta.buttonLimit

    // 指令参数单个权限
    if (binding.arg) {
      hasPermission = buttonLimit.toString().includes(binding.arg)
    }

    // 指令多个权限参数
    let intersection = []
    if (binding.value) {
      intersection = binding.value.filter(function (val) { return buttonLimit.indexOf(val) > -1 })
    }
    if (intersection.length > 0) {
      hasPermission = true
    }

    if (!hasPermission) { // 没有权限就隐藏元素
      el.style.display = 'none'
      el.style.width='0'
      el.parentNode && el.parentNode.removeChild(el) || (el.style.display = 'none')
    }

// 不显示更多按钮
let showMore = false
//更多按钮包含的按钮
const moreBtns = ['product_H5Up', 'product_H5Down',
  'product_Audit', 'product_AuditSuccess',
  'product_AuditFail', 'product_Up', 'product_Down', 'product_CopyUrl', 'product_RegisterData',
  'dcUser_childer', 'dcUser_resetPwd', 'dcUser_add_childer',
  'productProxy_H5Down', 'productProxy_Audit', 'productProxy_Up', 'productProxy_Down', 'productProxy_CopyUrl', 'productProxy_RegisterData']
//如果包含更多操作，则显示
moreBtns.map((item) => {
  if (buttonLimit.toString().includes(item)) {
    showMore = true
  }
})
vnode.context.$route.meta.showMore = showMore
  }
})


/**
 * Action 权限指令
 * 指令用法：
 *  - 在需要控制 action 级别权限的组件上使用 v-action:[method] , 如下：
 *    <i-button v-action:add >添加用户</a-button>
 *    <a-button v-action:delete>删除用户</a-button>
 *    <a v-action:edit @click="edit(record)">修改</a>
 *
 *  - 当前用户没有权限时，组件上使用了该指令则会被隐藏
 *  - 当后台权限跟 pro 提供的模式不同时，只需要针对这里的权限过滤进行修改即可
 *
 *  @see https://github.com/sendya/ant-design-pro-vue/pull/53
 */
// const action = Vue.directive('action', {
//   inserted: function (el, binding, vnode) {
//     const actionName = binding.arg
//     const roles = store.getters.roles
//     const elVal = vnode.context.$route.meta.permission
//     const permissionId = elVal instanceof String && [elVal] || elVal
//     roles.permissions.forEach(p => {
//       if (!permissionId.includes(p.permissionId)) {
//         return
//       }
//       if (p.actionList && !p.actionList.includes(actionName)) {
//         el.parentNode && el.parentNode.removeChild(el) || (el.style.display = 'none')
//       }
//     })
//   }
// })

export default action
