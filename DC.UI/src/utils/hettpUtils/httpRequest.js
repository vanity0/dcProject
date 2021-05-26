import axios from 'axios'
import notification from 'ant-design-vue/es/notification'
import { VueAxios } from './axios'
import { CheckRefrshToken } from '../tokenUtils/accessToken'
import { getAccessToken } from '../tokenUtils/accessTokenCache'


// 创建axios实例，在这里可以设置请求的默认配置
const service = axios.create({
	timeout: 6000 * 10, // 设置超时时间30秒
	baseURL: process.env.VUE_APP_API_BASE_URL,
})

// 统一设置post请求头
service.defaults.headers.post['Content-Type'] = 'application/json;charset=UTF-8'

/**
 * 
 * @param {error,success,warning,warn,info,open} type 
 * @param {消息頭} message 
 * @param {消息内容} description 
 * @param {超時時間，默認5S，單位：秒} duration 
 */
let openNotification = (type, message, description, duration = 5) => {
	notification[type]({
		duration: duration,
		message: message,
		onClose: close,
		description: h => {
			return (description)
		},
	});
}

// 全局异常处理
let err = error => {
	// 请求超时处理
	if (error.code == 'ECONNABORTED' && error.message.indexOf('timeout') != -1) {
		// openNotification('error', '系统异常', ((error.response || {}).data || {}).message || 'Request timeout')
		return Promise.reject(error)
	}
	// if(error.isAxiosError){
	// 	openNotification('error', '系统异常-01', error)
	// }
	return Promise.reject(error)
}

/** 添加请求拦截器 **/
service.interceptors.request.use(config => {
	// 判断token是否存在
	let token = getAccessToken()
	if (token) {
		// 在请求头携带token进行请求
		config.headers['Authorization'] = 'Bearer ' + token
	}

	// 导出文件的接口，返回的是二进制流，设置请求响应类型为blob
	if (config.url) {
		if (config.url.includes('/download')) {
			config.headers['responseType'] = 'blob'
		}
		// 文件上传，发送的是二进制流，所以需要设置请求头的'Content-Type'
		if (config.url.includes('file/upload')) {
			config.headers['Content-Type'] = 'multipart/form-data'
		}
	}
	// 判断访问token是否即将过期 并且请求地址不是刷新token的地址
	return CheckRefrshToken(config)
}, err)

/** 添加响应拦截器 **/
service.interceptors.response.use(response => {
	let { status, statusText, config } = response
	if (status === 200) {
		let { data, msg, statusCode } = response.data
		if (config.url.includes('/export') || config.url.includes('/download')) {
			return response
		} else if (statusCode === 200) {
			return response.data
		} else if (statusCode === 300) {
			return Promise.reject(msg)
		} else if (statusCode === 203) {
			// openNotification('warning', '系统提示', msg)
			return Promise.reject(msg)
		}
		else {
			// openNotification('error', '系统异常01', msg || '服务器异常')
			return Promise.reject(msg)
		}
	} else {
		openNotification('error', '系统异常02', statusText || '服务器异常')
		return Promise.reject(statusText)
	}
}, err)


/* 统一封装post请求 */
export const post = (url, data, config = {}) => {
	return new Promise((resolve, reject) => {
		service({
			method: 'post',
			url,
			data,
			...config,
		})
			.then(response => {
				resolve(response)
			})
			.catch(error => {
				reject(error)
			})
	})
}

/* 统一封装get请求 */
export const get = (url, params, config = {}) => {
	return new Promise((resolve, reject) => {
		// 添加时间戳参数，防止浏览器（IE）对get请求的缓存
		url = url + '?t=' + new Date().getTime().toString()
		service({
			method: 'get',
			url,
			params,
			...config,
		})
			.then(response => {
				resolve(response)
			})
			.catch(error => {
				reject(error)
			})
	})
}

/* 统一封装put请求  处理结果 */
export const put = (url, data, config = {}) => {
	return new Promise((resolve, reject) => {
		service({
			method: 'put', url, data, ...config
		}).then(response => {
			resolve(response)
		}).catch(error => {
			reject(error)
		})
	})
}

// 上传文件
export const postFile = (url, data, config = {}) => {
	return new Promise((resolve, reject) => {
		service({
			method: 'post',
			url,
			data: data,
			...config,
		})
			.then(response => {
				resolve(response)
			})
			.catch(error => {
				reject(error)
			})
	})
}

// 导出excel
export function postExportExcel(url, data) {
	return service({
		method: 'post',
		responseType: 'blob',
		url: url,
		data: data,
	})
		.then(res => {
			let fileName = ''
			let val = res.data
			if (val !== null) {
				fileName = res.headers["content-disposition"].split(";")[1].split("filename=")[1];
			}
			let blob = new Blob([val])
			if (window.navigator.msSaveOrOpenBlob) {
				navigator.msSaveBlob(blob, fileName)
			} else {
				let link = document.createElement('a')
				link.href = window.URL.createObjectURL(blob)
				link.download = fileName
				document.body.appendChild(link)
				link.click()
				document.body.removeChild(link)
			}
		})
		.catch(e => {
			openNotification('error', '系统异常', e.message || '服务器异常')
		})
}

const installer = {
	vm: {},
	install(vue) {
		vue.use(VueAxios, service)
	}
}
export { installer as VueAxios, service as axios }
