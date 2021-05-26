""
import util from './util';

const baseRequestURL = 'http://dcweb.jingdaishangwu.com/api';

const urls = ['', '']
const ajax = (opt) => {
	opt = opt || {};
	opt.url = opt.url || '';
	opt.method = opt.method || 'GET';
	opt.data = opt.data || {};
	opt.success = opt.success || function() {};
	opt.token = util.storageUtil.getStorageData('dl_token')
	uni.request({
		url: baseRequestURL + opt.url,
		data: opt.data,
		method: opt.method,
		dataType: 'json',
		success: function(res) {
			if (res.statusCode == 200) {
				opt.success(res.data);
			} else if (res.statusCode == 300) {
				uni.showToast({
					title: res.data.msg
				});
			} else {
				uni.showToast({
					title: res.data.msg
				});
			}
		},
		fail: function(err) {
			if (err.response != undefined) {
				// if (err.response.statusText == "Unauthorized") {
				// 	router.push({
				// 		path: '/login/login'
				// 	});
				// }
			} else {
				uni.showToast({
					title: '网络慢,请稍后重试'
				});
			}
		}
	})

}

const showMsg = function(msg) {
	uni.showModal({
		content: msg,
		showCancel: false
	});
};

//export default ajax
export default {
	ajax,
	showMsg
}
