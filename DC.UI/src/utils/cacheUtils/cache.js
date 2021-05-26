const storage = 'localStorage'
import cookie from 'js-cookie'

/**
 * @description 获取
 * @returns {string|ActiveX.IXMLDOMNode|Promise<any>|any|IDBRequest<any>|MediaKeyStatus|FormDataEntryValue|Function|Promise<Credential | null>}
 */
export function getCache(key) {
  if (storage) {
    if ('localStorage' === storage) {
      return localStorage.getItem(key)
    } else if ('sessionStorage' === storage) {
      return sessionStorage.getItem(key)
    } else if ('cookie' === storage) {
      return cookie.get(key)
    } else {
      return localStorage.getItem(key)
    }
  } else {
    return localStorage.getItem(key)
  }
}

/**
 * @description 存储
 * @param accessToken
 * @returns {void|*}
 */
export function setCache(key, value) {
  if (storage) {
    if ('localStorage' === storage) {
      return localStorage.setItem(key, value)
    } else if ('sessionStorage' === storage) {
      return sessionStorage.setItem(key, value)
    } else if ('cookie' === storage) {
      return cookie.set(key, value)
    } else {
      return localStorage.setItem(key, value)
    }
  } else {
    return localStorage.setItem(key, value)
  }
}

/**
 * @description 移除
 * @returns {void|Promise<void>}
 */
export function removeCache(key) {
  if (storage) {
    if ('localStorage' === storage) {
      return localStorage.removeItem(key)
    } else if ('sessionStorage' === storage) {
      return sessionStorage.clear()
    } else if ('cookie' === storage) {
      return cookie.remove(key)
    } else {
      return localStorage.removeItem(key)
    }
  } else {
    return localStorage.removeItem(key)
  }
}
