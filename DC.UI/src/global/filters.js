import Vue from 'vue'
import { STATUS } from './models.js'

Vue.filter('StatusFilter', function (value) {
    if (!value) {
        return '0'
    }
    const target = STATUS.filter(item => {
        return item.value == value;
    })
    let text = target.length ? target[0].text : value;
    return text;
})


