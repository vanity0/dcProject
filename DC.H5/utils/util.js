/*帮助工具*/
//时间格式化
function formatTime(time) {
	if (typeof time !== 'number' || time < 0) {
		return time
	}

	var hour = parseInt(time / 3600)
	time = time % 3600
	var minute = parseInt(time / 60)
	time = time % 60
	var second = time

	return ([hour, minute, second]).map(function(n) {
		n = n.toString()
		return n[1] ? n : '0' + n
	}).join(':')
}

function formatLocation(longitude, latitude) {
	if (typeof longitude === 'string' && typeof latitude === 'string') {
		longitude = parseFloat(longitude)
		latitude = parseFloat(latitude)
	}

	longitude = longitude.toFixed(2)
	latitude = latitude.toFixed(2)

	return {
		longitude: longitude.toString().split('.'),
		latitude: latitude.toString().split('.')
	}
}
var dateUtils = {
	UNITS: {
		'年': 31557600000,
		'月': 2629800000,
		'天': 86400000,
		'小时': 3600000,
		'分钟': 60000,
		'秒': 1000
	},
	humanize: function(milliseconds) {
		var humanize = '';
		for (var key in this.UNITS) {
			if (milliseconds >= this.UNITS[key]) {
				humanize = Math.floor(milliseconds / this.UNITS[key]) + key + '前';
				break;
			}
		}
		return humanize || '刚刚';
	},
	format: function(dateStr, formatStr) {
		if(!dateStr){
			return '';
		}
		var date = this.parse(dateStr)
		var diff = Date.now() - date.getTime();
		if (diff < this.UNITS['天']) {
			return this.humanize(diff);
		}
		var _format = function(number) {
			return (number < 10 ? ('0' + number) : number);
		};
		if (formatStr == "yyyy") {
			return date.getFullYear();
		} else if (formatStr == "yyyy-MM") {
			return date.getFullYear() + '-' + _format(date.getMonth() + 1);
		} else if (formatStr == "yyyy-MM-dd") {
			return date.getFullYear() + '-' + _format(date.getMonth() + 1) + '-' + _format(date.getDay());
		} else if (formatStr == "HH:mm") {
			return _format(date.getHours()) + ':' + _format(date.getMinutes());
		} else {
			return date.getFullYear() + '-' + _format(date.getMonth() + 1) + '-' + _format(date.getDay()) + ' ' +
				_format(date.getHours()) + ':' + _format(date.getMinutes());
		}
	},
	parse: function(str) { //将"yyyy-mm-dd HH:MM:ss"格式的字符串，转化为一个Date对象
		var a = str.split(/[^0-9]/);
		return new Date(a[0], a[1] - 1, a[2], a[3], a[4], a[5]);
	}
};

var storageUtil = { //localStorage 使用
	/**
	 * 说明:app本地存储工具
	 * 可用于本地数据的存取和是否登录的判断，以及退出时用于清空数据缓存
	 * */
	//用于保存用户数据 登录成功后使用
	saveUserData: function(array) { //存储用户storage数据
		for (var item in array) {
			uni.setStorageSync('local_user_' + item, array[item]);
		}
	},
	getUserData: function(key) { //获取用户storage数据
		var result = uni.getStorageSync('local_user_' + key);
		if (!result || result == null || result.length < 1 || result == 'null' || result == 'undefined') {
			result = '';
		}
		return result;
	},
	UpdateUserData: function(key, value) { //存储storage数据
		uni.setStorageSync('local_user_' + key, value);
	},
	saveStorageData: function(key, value) { //存储storage数据
		uni.setStorageSync('temp_' + key, value);
	},
	getStorageData: function(key) { //获取storage数据
		var result = uni.getStorageSync('temp_' + key);
		if (!result || result == null || result.length < 1 || result == 'null' || result == 'undefined') {
			result = '';
		}
		return result;
	},
	removeStorageData: function(key) { //移除数据
		uni.removeStorageSync('temp_' + key);
	},
	removeAllUserData: function() { //清空用户登录存储的数据
		const res = uni.getStorageInfoSync(); //获取localstorage中所有数据
		for (var item in res.keys) { //遍历数据
			var start = res.keys[item].indexOf("local_user_");
			if (start == 0) {
				uni.removeStorageSync(res.keys[item]); //移除相应的键值对
			}
		}
	},
	hasLogin: function() { //校验是否登录
		var isLogined = this.getStorageData('isLogin');
		if (isLogined && isLogined == 'true') {
			return true;
		} else {
			return false;
		}
	},
	hasRealNameVerify: function() { //是否实名认证
		var isRealNameCheck = this.getStorageData('IsRealNameCheck');
		if (isRealNameCheck && isRealNameCheck == 'true') {
			return true;
		} else {
			return false;
		}
	},

};
var commonUtil = { //公用方法
	getBase64Image: function(img) { //将图片压缩转成base64 		

		var canvas = document.createElement("canvas");
		var width = img.width;
		var height = img.height;
		// calculate the width and height, constraining the proportions 
		if (width > height) {
			if (width > 480) {
				height = Math.round(height *= 480 / width);
				width = 480;
			}
		} else {
			if (height > 480) {
				width = Math.round(width *= 480 / height);
				height = 480;
			}
		}
		canvas.width = width; /*设置新的图片的宽度*/
		canvas.height = height; /*设置新的图片的长度*/
		var ctx = canvas.getContext("2d");
		ctx.drawImage(img, 0, 0, width, height); /*绘图*/
		var dataURL = canvas.toDataURL("image/jpeg", 1 || 0.8);
		return dataURL.replace("data:image/jpeg;base64,", "");

	},

	// 图片路径与base64互转
	pathToBase64: function(path) {
		return new Promise(function(resolve, reject) {
			if (typeof window === 'object' && 'document' in window) {
				var canvas = document.createElement('canvas');

				var c2x = canvas.getContext('2d')
				var img = new Image



				img.onload = function() {
					//重排图片大小
					var width = img.width;
					var height = img.height;
					console.log("原始" + width + "，" + height);
					if (width > height) {
						if (width > 480) {
							height = Math.round(height *= 480 / width);
							width = 480;
						}
					} else {
						if (height > 480) {
							width = Math.round(width *= 480 / height);
							height = 480;
						}
					}
					console.log("最终" + width + "，" + height);
					canvas.width = width;
					canvas.height = height;
					c2x.drawImage(img, 0, 0, width, height);
					resolve(canvas.toDataURL())
				}
				img.onerror = reject
				img.src = path
				return
			}
			if (typeof plus === 'object') {
				var bitmap = new plus.nativeObj.Bitmap('bitmap' + Date.now())
				bitmap.load(path, function() {
					try {
						var base64 = bitmap.toBase64Data()
					} catch (error) {
						reject(error)
					}
					bitmap.clear()
					resolve(base64)
				}, function(error) {
					bitmap.clear()
					reject(error)
				})
				return
			}
			if (typeof wx === 'object' && wx.canIUse('getFileSystemManager')) {
				wx.getFileSystemManager().readFile({
					filePath: path,
					encoding: 'base64',
					success: function(res) {
						resolve('data:image/png;base64,' + res.data)
					},
					fail: function(error) {
						reject(error)
					}
				})
				return
			}
			reject(new Error('not support'))
		})
	},
	base64ToPath: function(base64) {
		return new Promise(function(resolve, reject) {
			if (typeof window === 'object' && 'document' in window) {
				base64 = base64.split(',')
				var type = base64[0].match(/:(.*?);/)[1]
				var str = atob(base64[1])
				var n = str.length
				var array = new Uint8Array(n)
				while (n--) {
					array[n] = str.charCodeAt(n)
				}
				return resolve((window.URL || window.webkitURL).createObjectURL(new Blob([array], {
					type: type
				})))
			}
			var extName = base64.match(/data\:image\/(.+);/)
			if (extName) {
				extName = extName[1]
			} else {
				reject(new Error('base64 error'))
			}
			var fileName = Date.now() + '.' + extName
			if (typeof plus === 'object') {
				var bitmap = new plus.nativeObj.Bitmap('bitmap' + Date.now())
				bitmap.loadBase64Data(base64, function() {
					var filePath = '_doc/uniapp_temp/' + fileName
					bitmap.save(filePath, {}, function() {
						bitmap.clear()
						resolve(filePath)
					}, function(error) {
						bitmap.clear()
						reject(error)
					})
				}, function(error) {
					bitmap.clear()
					reject(error)
				})
				return
			}
			if (typeof wx === 'object' && wx.canIUse('getFileSystemManager')) {
				var filePath = wx.env.USER_DATA_PATH + '/' + fileName
				wx.getFileSystemManager().writeFile({
					filePath: filePath,
					data: base64,
					encoding: 'base64',
					success: function() {
						resolve(filePath)
					},
					fail: function(error) {
						reject(error)
					}
				})
				return
			}
			reject(new Error('not support'))
		})
	},

};

module.exports = {
	formatTime: formatTime,
	formatLocation: formatLocation,
	dateUtils: dateUtils,
	storageUtil: storageUtil,
	commonUtil: commonUtil,
}
