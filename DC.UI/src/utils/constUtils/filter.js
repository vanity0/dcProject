import Vue from 'vue'
import moment from 'moment'
import 'moment/locale/zh-cn'
moment.locale('zh-cn')

Vue.filter('NumberFormat', function (value) {
  if (!value) {
    return '0'
  }
  const intPartFormat = value.toString().replace(/(\d)(?=(?:\d{3})+$)/g, '$1,') // 将整数部分逢三一断
  return intPartFormat
})

Vue.filter('dayjs', function (dataStr, pattern = 'YYYY-MM-DD HH:mm:ss') {
  return moment(dataStr).format(pattern)
})

Vue.filter('moment', function (dataStr, pattern = 'YYYY-MM-DD HH:mm:ss') {
  return moment(dataStr).format(pattern)
})

Vue.filter('linkModelFilter', function (dataStr) {
  return dataStr.substr(0, 1)
})
Vue.filter('telFilter', function (dataStr) {
  return hidetelMobile(dataStr)
})

/*
* @desc：隐藏手机号
* @param：tel 手机号，如：13987654321
* @param：num 隐藏的位数，默认4位
* @return ret 回显的手机号，如：139xxxx4321
*/
const hidetelMobile = (tel, num = 4) => {
  tel = tel.toString()
  let len = tel.length
  let start = Math.floor((len - num) / 2)
  let end = Math.ceil((len - num) / 2)
  let head = tel.slice(0, start)
  let arr = new Array(num + 1)
  let body = arr.join('*')
  let foot = tel.slice((start + num), len)
  let ret = head + '' + body + '' + foot
  return ret
}


//#region  界面上用于是、否、冻结、正常显示的

const statusFlag = {
  disable: {
    status: 'default',
    text: '冻结'
  },
  enable: {
    status: 'success',
    text: '正常'
  }
}

Vue.filter('statusTextFilter', function (data) {
  return data ? statusFlag.enable.text : statusFlag.disable.text;
})

Vue.filter('statusTypeFilter', function (data) {
  return data ? statusFlag.enable.status : statusFlag.disable.status;
})

const yesNoFlag = {
  /**
   * 后端值为：0
   */
  no: {
    status: 'default',
    text: 'NO'
  },
  /**
   * 后台值为1
   */
  yes: {
    status: 'success',
    text: 'YES'
  }
}

Vue.filter('yesNoTextFilter', function (data) {
  return data ? yesNoFlag.yes.text : yesNoFlag.no.text;
})

Vue.filter('yesNoTypeFilter', function (data) {
  return data ? yesNoFlag.yes.status : yesNoFlag.no.status;
})
//#endregion


//#region  产品状态

const h5Flag = {
  no: {
    status: 'orange',
    text: '未上架'
  },
  yes: {
    status: 'green',
    text: '上架中'
  }
}

Vue.filter('h5TextFilter', function (data) {
  return data ? h5Flag.yes.text : h5Flag.no.text;
})

Vue.filter('h5TypeFilter', function (data) {
  return data ? h5Flag.yes.status : h5Flag.no.status;
})

const prodStatusFlag = {
  '待申请': {
    status: 'cyan',
    text: '待申请'
  },
  '审核中': {
    status: 'orange',
    text: '审核中'
  },
  '审核通过': {
    status: 'blue',
    text: '审核通过'
  },
  '拒绝通过': {
    status: 'red',
    text: '拒绝通过'
  },
  '上架中': {
    status: 'green',
    text: '上架中'
  },
  '下架中': {
    status: 'pink',
    text: '下架中'
  }
}

Vue.filter('prodStatusTextFilter', function (data) {
  return prodStatusFlag[data].text
})

Vue.filter('prodStatusTypeFilter', function (data) {
  return prodStatusFlag[data].status
})

//#endregion


//#region  推广数据状态

const registerStatusFlag = {
  '待更新': {
    status: 'blue',
    text: '待更新'
  },
  '审核通过': {
    status: 'green',
    text: '审核通过'
  },
  '拒绝通过': {
    status: 'red',
    text: '拒绝通过'
  }
}

Vue.filter('registerStatusTextFilter', function (data) {
  return registerStatusFlag[data].text
})

Vue.filter('registerStatusTypeFilter', function (data) {
  return registerStatusFlag[data].status
})

//#endregion