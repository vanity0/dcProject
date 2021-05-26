
import Vue from 'vue'

const drag = Vue.directive('dragModal', {
    inserted: function (el, binding, vnode) {
        let { visible, destroyOnClose } = vnode.componentInstance
        // 防止未定义 destroyOnClose 关闭弹窗时dom未被销毁，指令被重复调用
        if (!visible) return
        let modal = el.getElementsByClassName('ant-modal')[0]
        let header = el.getElementsByClassName('ant-modal-header')[0]
        let left = 0
        let top = 0
        // 未定义 destroyOnClose 时，dom未被销毁，关闭弹窗再次打开，弹窗会停留在上一次拖动的位置
        if (!destroyOnClose) {
            left = modal.left || 0
            top = modal.top || 0
        }
        // top 初始值为 offsetTop
        top = top || modal.offsetTop
        // 鼠标手型
        header.style.cursor = 'move';
        header.onmousedown = (e) => {
            // 当前控件起始坐标
            let mouseDownX = e.clientX
            let mouseDownY = e.clientY
            header.left = header.offsetLeft;
            header.top = header.offsetTop;
            el.onmousemove = (event) => {
                let endX = event.clientX;
                let endY = event.clientY;
                modal.left = header.left + (endX - mouseDownX) + left;
                modal.top = header.top + (endY - mouseDownY) + top;
                modal.style.left = modal.left + 'px'
                modal.style.top = modal.top + 'px'
            }
            // 鼠标松开时，拖拽结束
            el.onmouseup = (e) => {
                left = modal.left
                top = modal.top
                el.onmousemove = null;
                el.onmouseup = null;
                header.releaseCapture && header.releaseCapture();
            }
            header.setCapture && header.setCapture();
        }
    },
    // 每次重新打开 dialog 时，要将其还原
    // update(el) {
    //     let modal = el.getElementsByClassName('ant-modal')[0]
    //     if (modal) {
    //         modal.left = 0
    //         modal.top = 0
    //         modal.style.left = '0px'
    //         modal.style.top = '0px'
    //     }
    // },
    // // 最后卸载时，清除事件绑定
    // unbind(el) {
    //     if (el.nextElementSibling) {
    //         let header = el.nextElementSibling.getElementsByClassName('ant-modal-header')[0]
    //         header.onmousedown = null
    //         let modal = el.nextElementSibling.getElementsByClassName('ant-modal')[0]
    //         modal.left = 0
    //         modal.top = 0
    //         modal.style.left = '0px'
    //         modal.style.top = '0px'
    //     }
    // }
})
export default drag;