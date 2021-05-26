// index.html中引入mobile-detect.js文件
// 下载地址：https://github.com/hgoebl/mobile-detect.js
/* <script src="./mobile-detect.js"></script> */
const MobileDetect = require('mobile-detect')

export const mobileInfo = () => {
    //判断数组中是否包含某字符串  
    // Array.prototype.contains = function (needle) {
    //     for (i in this) {
    //         if (this[i].indexOf(needle) > 0)
    //             return i
    //     }
    //     return -1
    // }
    let deviceType = navigator.userAgent//获取userAgent信息  
    // document.write(deviceType)//打印到页面  
    // const md = new MobileDetect(req.headers['user-agent'])
    let md = new MobileDetect(deviceType);//初始化mobile-detect  
    let os = md.os()//获取系统  
    let version = ""//系统的版本号   
    let model = ""  //设备型号
    if (os == "iOS") {//ios系统的处理  
        version = md.version("iPhone")
        os = md.os()
        model = md.mobile()
    } else if (os == "AndroidOS") {//Android系统的处理  
        os = md.os()
        version = md.version("Android")
        let sss = deviceType.split(";")
        for (let i = 0; i < sss.length; i++) {
            if (sss[i].indexOf("Build/") > -1) {
                model = sss[i].substring(0, sss[i].indexOf("Build/"))
            }
        }
        // let i = sss.contains("Build/")
        // if (i > -1) {
        //     model = sss[i].substring(0, sss[i].indexOf("Build/"))
        // }
    }
    //组装信息
    let info = {
        os: os,//系统类型
        version: version,//系统版本号
        model: model //设备型号
    }
    return info
}


