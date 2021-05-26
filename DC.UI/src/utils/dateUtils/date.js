/**
 * 获取当前时间  yy+'-'+mm+'-'+dd+' '+hh+':'+mf+':'+ss
 */
export function currentTime() {
    let yy = new Date().getFullYear()
    let mm = new Date().getMonth() + 1
    let dd = new Date().getDate()
    let hh = new Date().getHours()
    let mf = new Date().getMinutes() < 10 ? '0' + new Date().getMinutes() : new Date().getMinutes()
    let ss = new Date().getSeconds() < 10 ? '0' + new Date().getSeconds() : new Date().getSeconds()
    return yy + '-' + mm + '-' + dd + ' ' + hh + ':' + mf + ':' + ss
}


/**
 * 时间戳转换成时间
 * @param {时间戳} stamp 
 */
export const timestampToTime = (stamp) => {
    var date = new Date(stamp) //时间戳为10位需*1000，时间戳为13位的话不需乘1000
    var Y = date.getFullYear() + '-'
    var M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-'
    var D = date.getDate() + ' '
    var h = date.getHours() + ':'
    var m = date.getMinutes() + ':'
    var s = date.getSeconds()
    return Y + M + D + h + m + s
}