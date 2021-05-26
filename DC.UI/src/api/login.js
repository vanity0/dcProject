import { post, put, get } from '@/utils/hettpUtils/httpRequest'
export const login = (parameter) => post('api/login/login', parameter)
export const logout = (parameter) => post('api/login/logout', parameter)
export const getUserInfo = (parameter) => get('api/login/getUserInfo', parameter)
export const getNavRouters = (parameter) => get('api/login/getNavRouters', parameter)
export const getAccessTokenByRefresh =  (parameter) => post('api/login/getAccessTokenByRefresh', parameter)
