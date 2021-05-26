import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const queryReport = (parameter) => get('api/home/queryReport', parameter)
export const queryBacklog = (parameter) => get('api/home/queryBacklog', parameter)
export const queryDCCount = (parameter) => get('api/home/queryDCCount', parameter)
export const queryProxyCount = (parameter) => get('api/home/queryProxyCount', parameter)
export const queryDevCount = (parameter) => get('api/home/queryDevCount', parameter)
// export const queryProxyBacklog = (parameter) => get('api/home/queryProxyBacklog', parameter)
// export const queryProxyReport = (parameter) => get('api/home/queryProxyReport', parameter)
export const queryDevReport = (parameter) => get('api/home/queryDevReport', parameter)
export const queryDevBacklog = (parameter) => get('api/home/queryDevBacklog', parameter)

