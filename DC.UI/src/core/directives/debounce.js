import Vue from 'vue'

export const debounce = Vue.directive('debounce', {
    inserted: function (el, binding, vnode) {
        let { events, delay, callback, params } = binding.value
        // 如果传递了事件绑定
        if (events) {
            // 绑定属性
            events.map(event => {
                el.addEventListener(event, () => { debounceFun(callback, delay, params) })
            })
        }
    }
})

/**
 * 防抖函数,极短时间内多次点击，定义时间内只触发一次 
 * @param {回调函数} callbackFunc 
 * @param {参数} params 
 * @param {定时执行时间，默认：1500，单位：毫秒} delay 
 * @param {是否即时执行,默认：true} immdiate 
 */
let debounceFun = (callbackFunc, params, delay = 1500, immdiate = false,) => {
    
    let timer = null;
    console.log('倒计时:', delay)
    console.log('是否立即执行:', immdiate)
    return () => {
        let that = this;
        // 立即执行
        if (immdiate) {
            callbackFunc.apply(that, params);
            immdiate = false;
        } else {
            if (timer) {
                console.log('间隔低于 ' + delay + ' ms');
                clearTimeout(timer);// 清除定时器
            }
            timer = setTimeout(() => {
                console.log('执行函数');
                callbackFunc.apply(that, params);
                immdiate = true;
            }, delay);
        }
    }
};
