
import Vue from 'vue'

/**
 * 节流指令，使用方式：
 * v-throttle="{
                    events: [{ event: 'select', callback: handleSelectChange, delay: 3000, params: this.v_Model }],
                }"
 */
export const throttle = Vue.directive('throttle', {
    inserted: function (el, binding, vnode) {
        let { events } = binding.value
        // 如果是组件
        if (vnode.componentOptions) {
            // 判断组件类型以及绑定的事件
            switch (vnode.componentOptions.tag) {
                case 'a-auto-complete':
                    events.forEach(item => {
                        if (item.event === 'select') {
                            vnode.componentOptions.listeners.select = throttleFunc(item.callback, item.params, item.delay)
                        }
                        if (item.event === 'search') {
                            vnode.componentOptions.listeners.search = throttleFunc(item.callback, item.params, item.delay)
                        }
                        if (item.event === 'change') {
                            vnode.componentOptions.listeners.search = throttleFunc(item.callback, item.params, item.delay)
                        }
                    });
                    break;
                case 'a-input-search':
                    break;
                case 'a-button':
                    break;
            }
        } else {
            console.warn('<debounce> 组件内只能出现下面组件的任意一个且唯一 el-button、el-input、button、input')
            return vnode
        }

        // // 如果传递了事件绑定
        // if (events) {
        //     // 绑定属性
        //     events.map(event => {
        //         el.addEventListener(event, () => { throttleFunc(item.callback, item.params, item.delay) })
        //     })
        // } else { 
    }
    // }
})

/**
 * 节流函数,在极短的时间内多次点击，定义时间内停止点击才会触发
 * @param {回调函数} callbackFunc 
*  @param {参数} params 
 * @param {多少时间内不执行，默认：1500，单位：毫秒} delay 
 */
let throttleFunc = (callbackFunc, params, delay = 1500) => {
    let timer = null, startTime = Date.now()
    return () => {
        let that = this
        // 获取当前时间
        let curTime = Date.now()
        // 倒计时时间减去 当前时间减去开始时间
        let remaining = delay - (curTime - startTime);
        timer && clearTimeout(timer);
        if (remaining <= 0) {
            callbackFunc.apply(that, params);
            startTime = Date.now()
        } else {
            timer = setTimeout(callbackFunc, remaining);
        }
    }
}

