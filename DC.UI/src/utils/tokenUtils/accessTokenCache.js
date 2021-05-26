
import {
  ACCESS_TOKEN, ACCESS_TOKEN_EXPIRES_IN,
  REFRESH_TOKEN, REFRESH_TOKEN_EXPIRES_IN
} from '@/store/mutation-types'

import { getCache, setCache, removeCache } from '../cacheUtils/cache'

// 获取token缓存
export const getAccessToken = () => getCache(ACCESS_TOKEN)
export const getAccessTokenExpiresIn = () => getCache(ACCESS_TOKEN_EXPIRES_IN)
export const getRefreshToken = () => getCache(REFRESH_TOKEN)
export const getRefreshTokenExpiresIn = () => getCache(REFRESH_TOKEN_EXPIRES_IN)

// 设置token缓存
export const setAccessToken = (value) => setCache(ACCESS_TOKEN, value)
export const setAccessTokenExpiresIn = (value) => setCache(ACCESS_TOKEN_EXPIRES_IN, value)
export const setRefreshToken = (value) => setCache(REFRESH_TOKEN, value)
export const setRefreshTokenExpiresIn = (value) => setCache(REFRESH_TOKEN_EXPIRES_IN, value)

// 删除token缓存
export const removeAccessToken = () => removeCache(ACCESS_TOKEN)
export const removeAccessTokenExpiresIn = () => removeCache(ACCESS_TOKEN_EXPIRES_IN)
export const removeRefreshToken = () => removeCache(REFRESH_TOKEN)
export const removeRefreshTokenExpiresIn = () => removeCache(REFRESH_TOKEN_EXPIRES_IN)

/**
 * 清除所有token缓存
 */
export const clearTokenCache = () => {
  removeAccessToken()
  removeAccessTokenExpiresIn()
  removeRefreshToken()
  removeRefreshTokenExpiresIn()
}
