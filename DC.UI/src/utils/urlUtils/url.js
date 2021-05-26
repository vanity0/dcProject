
/* eslint-disable */
/**
 * 思路：
 * 首先网站页面url都有对应的不同路径组成，这个地方不用修改(当然也可以自己扩展)，后边可能会拼接一些参数对这串参数进行处理 ；
 * 思路就是在页面跳转前进行拦截修改参数部分，及在改变页面某些状态需要改变url内容时进行修改参数部分；
 * 那么页面在重新载入或者进入时，首先需要进行对页面的加密参数解密到对应字段上进行后边的参数处理
 * 
 * 1.
 * 加密方法EncryUrl，首先判断传入的数据是否是Object的，然后把json转成字符串，
 * 使用encodeURIComponent进行URI编码(encodeURIComponent方法执行，让浏览器能够接受和理解，
 * 若中文在使用后边的window.btoa会报错)，然后使用window.btoa再次转换输出；解密DecryptUrl，
 * 以同样的方式反过来执行进行解密，至于为什么后边没有使用JSON.parse
 * 
 * 2.
 * GetQueryParam是封装了获取指定链接参数的代码块，GetQueryParamOfObjEntry是获取以key这个指定参数后方的解密数据，
 * 这里可以看到有JSON.parse这个方法，因为我这里约定了链接后方参数是以key为key的一个参数，当然可以换成其他的参数
 * 
 * 3.
 * encryUrlOfRouter这个方法放在router.beforeEach里边执行，首先判断当前链接是否有参数，若有且如果不存在key，
 * 那么就使用EncryUrl进行处理query加密，然后修改当前链接参数，这时，router会再次执行一次
 * 
 * 4.
 * 那么在页面中使用时，首先是初次渲染拿出数据了，在created中执行时可以使用GetQueryParamOfObjEntry来获取数据，
 * 然后对应参数赋值；其次是在一些改变url参数的操作，(场景：比如分页，触发分页后，把这个链接复制到其他的窗口中，
 * 既要让参数加密，又必须让这个链接打开窗口的分页的状态和被复制页面的分页一样)，
 * 这个时候在每次触发改变链接的时候执行这个方法就没问题了
 * 
 * 5.
 * 当然这时候并没有改变页面位置，也不会刷新页面，只是走了一遍路由，下方代码就是对应改变的方法了， searchCondition就是我存放需要改变url参数的对象
 *  editUrlQuery() {
 *      this.$router.push({ name: this.$route.name, query: this.searchCondition });
 * }
 * 
 * 6. 最后总结一下，如果是对安全性要求比较高在执行加密时可以换其他的方式来加密，我这里只是简单的加密了一下，
 * 让别人篡改参数不是那么很轻松 ，也直接看出url里边带的参数是什么；如果在控制台中进行解密测试，你需要执行window.decodeURIComponent这个方法两次，why？不解释，哈哈，实践一下就知道了；
 * 如果别人修改了key后边的参数，那么解析会出错，或者解析不完全，至于修改，随他改吧，看你的加密方式了；
 */


//#region 1. url加密和解密
/**
 * url参数加密
 * 传入json格式的串
 * @param {*Object} query
 */
export const EncryUrl = query => {
    if (query) {
        try {
            // query = JSON.stringify(query);
            query = window.encodeURIComponent(query);
            return window.btoa(query); // 编码
        } catch (err) {
            // console.log('%c url-encry-error：' + JSON.stringify(err), 'color:red;');
            return ""
        }
    }
    return "";
}

/**
 * url参数解密
 * 传入加密的json串
 * @param {*string} val
 */
export const DecryptUrl = val => {
    if (val) {
        try {
            let decryStr = window.atob(val); // 解码
            return window.decodeURIComponent(decryStr);
        } catch (err) {
            // console.log('%c url-decry-error：' + JSON.stringify(err.stack), 'color:red;');
            return ""
        }
    }
    return false;
}
//#endregion

//#region 2. 获取路由地址传递的参数

/**
 * 从地址栏获取指定参数值
 * @param {*string} param
 */
export const GetQueryParam = (param) => {
    let h = window.location.href;
    let reg = new RegExp("[\?(\&)]?" + param + "=([,-\w]+)[(\&)]?", "i");
    if (reg.test(h)) {
        let v = reg.exec(h)[1];
        return v;
    }
    return "";
};

/**
 * 返回 地址栏 加密 Object
 * @param {*string} param
 */
export const GetQueryParamOfObjEntry = (key) => {
    let keyStr = GetQueryParam(key);
    if (!keyStr) {
        return "";
    }
    try {
        let objStr = DecryptUrl(keyStr);
        let obj = JSON.parse(objStr);
        return obj;
    } catch (err) {
        console.log('%c url-json-parse-error：' + JSON.stringify(err.stack), 'color:red;');
    }
    return "";
}
//#endregion

//#region 3. url参数加密
/**
 * encryUrlOfRouter这个方法放在router.beforeEach里边执行，首先判断当前链接是否有参数，若有且如果不存在key，
 * 那么就使用EncryUrl进行处理query加密，然后修改当前链接参数，这时，router会再次执行一次
 * @param {*} to 
 * @param {*} from 
 * @param {*} next 
 * @returns 
 */
function encryUrlOfRouter(to, from, next) {
    // 这里对路由参数key进行加密
    if (Object.keys(to.query).length > 0 && !to.query.key) {
        let urlEntry = EncryUrl(to.query);
        if (urlEntry) {
            next({
                path: to.path,
                query: {
                    key: urlEntry
                }
            })
            return false;
        }
        next({
            path: to.path
        })
        return false;
    }
    return true;
}

  //#endregion