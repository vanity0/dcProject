import Vue from 'vue'

/**
 * 给input框元素绑定回车事件
 * 使用方式：v-keyupEnter="{ callback: handleRefresh }"
 */
const keyupEnter = Vue.directive('keyupEnter', {
    inserted: function (el, binding, vnode) {
        let { callback } = binding.value
        el.addEventListener('keyup', (event) => {
            if (event.keyCode === 13 && event.key=== "Enter") {
                //回车执行函数
                if (callback) { callback() }
            }
        })
    }
})

export default keyupEnter