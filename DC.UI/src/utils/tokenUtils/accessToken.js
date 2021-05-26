import { getAccessTokenByRefresh } from '@/api/login'
import {
    getRefreshToken, getAccessTokenExpiresIn,
    setAccessToken, setRefreshToken,
    setAccessTokenExpiresIn, setRefreshTokenExpiresIn,
    clearTokenCache
} from './accessTokenCache'

//#region 私有属性、方法

// 是否正在刷新的标志
window.isRefreshing = false;

// 存储请求的数组
let refreshSubscribers = [];

/**
 * 访问令牌的过期时间判断
 * @param {访问令牌的过期时间戳} oDate 
 */
function isAccessTokenExpired(oDate) {
    if (oDate) {
        // 获取当前时间戳
        let nDate = new Date().getTime()
        let stamp = oDate - nDate;
        const sec = Math.floor(stamp / 1000);
        if (sec <= 60) {
            return true;
        }
    }
    return false;
}

/**
 * 将所有的请求都push到数组中
 */
function subscribeTokenRefresh(callback) {
    refreshSubscribers.push(callback);
}

// 数组中的请求得到新的token之后自执行，用新的token去请求数据
function onRrefreshed(token) {
    for (let i = 0; i < refreshSubscribers.length; i++) {
        refreshSubscribers[i](token)
    }
}

//#endregion

//#region 公用方法

/**
 * 登录的页面地址
 */
export const loginUrl = '/user/login'

const allowList = ['api/pvHistory/addPvHistory', 'api/uvHistory/addUvHistory', 'api/cpaHistory/addCpaHistory']

/**
 * 处理刷新令牌过期
 */
export const CheckRefrshToken = (config) => {
    // 不检查token存在与否
    if (allowList.includes(config.url)) {
        return config
    }
    let accessTokenExpireTime = getAccessTokenExpiresIn()
    // 判断访问token是否即将过期 并且请求地址不是刷新token的地址
    if (isAccessTokenExpired(accessTokenExpireTime) && config.url.indexOf('/api/user/refresh_token') === -1) {
        // 判断是否正在刷新
        if (!window.isRefreshing) {
            let refreshTokenCache = getRefreshToken()
            if (refreshTokenCache) {
                // console.log('重新请求access token')
                window.isRefreshing = true;
                // 发起刷新token的请求
                getAccessTokenByRefresh(refreshTokenCache).then(res => {
                    let { accessToken, refreshToken, expireTime } = res.msgBody
                    window.isRefreshing = false;
                    // 存储令牌
                    saveToken(accessToken, refreshToken, expireTime)

                    // 执行数组里的函数,重新发起被挂起的请求
                    config.headers['Authorization'] = 'Bearer ' + accessToken

                    // 数组中的请求得到新的token之后自执行，用新的token去请求数据
                    onRrefreshed(accessToken);
                    // 执行onRefreshed函数后清空数组中保存的请求
                    refreshSubscribers = []
                }).catch((e) => {
                    /*清除本地保存的*/
                    clearTokenCache();
                    window.location.href = loginUrl
                }).finally(() => {
                    window.isRefreshing = false;
                });

                return new Promise((resolve) => {
                    subscribeTokenRefresh((token) => {
                        config.headers['Authorization'] = 'Bearer ' + token
                        /*将请求挂起*/
                        resolve(config)
                    })
                });
            } else {
                clearTokenCache();
                window.location.href = loginUrl
            }
        } else {
            /*把请求(token)=>{....}都push到一个数组中*/
            return new Promise((resolve) => {
                /*(token) => {...}这个函数就是回调函数*/
                subscribeTokenRefresh((token) => {
                    config.headers['Authorization'] = 'Bearer ' + token
                    /*将请求挂起*/
                    resolve(config)
                })
            });
        }
    } else {
        return config
    }
}

/**
 * 存储令牌信息到sessionStorge
 * @param {访问令牌} accessToken 
 * @param {刷新令牌} refreshToken 
 * @param {访问令牌过期时间，单位：秒} accessTokenExpireTime 
 * @param {刷新令牌过期时间，单位：秒} refreshTokenExpireTime 
 */
export const saveToken = (accessToken, refreshToken, accessTokenExpireTime = 240, refreshTokenExpireTime = 7200) => {
    setAccessToken(accessToken)
    setRefreshToken(refreshToken)
    setRefreshTokenExpiresIn(refreshTokenExpireTime)
    // 获取当前时间
    var curTime = new Date();
    // 获取当前秒
    let second = curTime.getSeconds();
    // 设置当前时间+过期时间单位：秒
    curTime.setSeconds(second + Number(accessTokenExpireTime));
    // 转成时间戳
    setAccessTokenExpiresIn(curTime.getTime())
}

//#endregion