import { post, put, get } from '@/utils/hettpUtils/httpRequest'
 

// 生成短网址
export const getDwzUrl = (parameter) => post('api/dwz/getDwzUrl', parameter)

export const getAllDwzUrl = () => post('api/dwz/getAllDwzUrl')