const API = {
		// post请求
		post(url, data, onoff = true) {
			return new Promise((resolve, reject) => {
					uni.request({
						url: `http://127.0.0.1:2021/api{url}`,
						data,
						method: 'POST',
						header: {
							'content-type': 'application/x-www-form-urlencoded'
						},
						success: (res) => {
							debugger
							if (!onoff) {
								resolve(res);
								return;
							}
							if (res.data.code !== 200) {
								uni.showToast({
									icon: 'none',
									title: `${res.data.message}`,
									success: () => {
										setTimeout(() => {
											uni.hideToast();
										}, 1000)
									}
								})
								return;
							}

							res = res.data;
							if (res.code !== 200) {
								uni.showToast({
									icon: 'none',
									title: res.message,
									success: () => {
										setTimeout(() => {
											uni.hideToast();
										}, 1000)
									}
								});
								return;
							}
							resolve(res.data);
						},
						fail: (err) => {
							reject(err);
						}
					})
				}
			})
	},
	//图片上传
	upload(tempFilePaths) {
		let Authorization = null;
		return new Promise((resolve, reject) => {
			uni.getStorage({
				key: 'member-userinfo',
				complete: (options) => {
					if (options.data) {
						Authorization = 'Bearer ' + options.data.token;
						uni.uploadFile({
							url: 'http://member.yunheban.com/member/userInfo/uploadImg', //仅为示例，非真实的接口地址
							filePath: tempFilePaths,
							header: {
								Authorization,
							},
							name: 'file',
							success: (res) => {
								if (res.data.code !== 200) {
									uni.showToast({
										icon: 'none',
										title: `${res.data.message}`,
										success: () => {
											setTimeout(() => {
												uni.hideToast();
											}, 1000)
										}
									})
									return;
								}
								let _res = JSON.parse(res.data);
								if (_res.code !== 200) {
									uni.showToast({
										icon: 'none',
										title: `出错啦~`,
										success: () => {
											setTimeout(() => {
												uni.hideToast();
											}, 1000)
										}
									})
									return;
								}
								resolve(_res.data); //返回图片接口
							},
							fail: (err) => {
								reject(err);
							}
						});
					}
				}
			})
		})

	},
	// 底部导航栏切换
	tab(url) {
		return new Promise((resolve, reject) => {
			uni.switchTab({
				url,
				success: (res) => {
					resolve(res);
				},
				fail: (err) => {
					reject(err);
				}
			});
		});
	},
	// 页面跳转
	to(url) {
		return new Promise((resolve, reject) => {
			uni.navigateTo({
				url,
				success: (res) => {
					resolve(res);
				},
				fail: (err) => {
					reject(err);
				}
			})
		});
	},
	//页面后退
	bank(delta = 1) {
		uni.navigateBack({
			delta
		});
	},
	redirectTo(url) {
		return new Promise((resolve, reject) => {
			uni.redirectTo({
				url,
				success: (res) => {
					resolve(res);
				},
				fail: (err) => {
					reject(err);
				}
			})
		});
	},
	// 设置本地缓存
	setStorage(key, data) {
		return new Promise((resolve, reject) => {
			uni.setStorage({
				key,
				data,
				success: (res) => {
					resolve(res);
				},
				fail: (err) => {
					reject(err);
				}
			})
		})
	},
	// 获取本地缓存
	getStorage(key) {
		return new Promise((resolve, reject) => {
			uni.getStorage({
				key,
				success: (res) => {
					resolve(res);
				},
				fail: (err) => {
					reject(err);
				}
			})
		})
	},
	//删除本地缓存
	removeStorage(key) {
		return new Promise((resolve, reject) => {
			uni.removeStorage({
				key,
				success: (res) => {
					resolve(res);
				},
				fail: (err) => {
					reject(err);
				}
			})
		})
	},
	//设置标题
	setTitle(title) {
		uni.setNavigationBarTitle({
			title,
		})
	},
	//禁用左边导航栏
	disLeftNav() {
		let pages = getCurrentPages();
		let page = pages[pages.length - 1];
		// #ifdef APP-PLUS
		let currentWebview = page.$getAppWebview();
		let titleObj = currentWebview.getStyle().titleNView;
		titleObj.autoBackButton = false;
		currentWebview.setStyle({
			titleNView: titleObj
		});
		// #endif
	},
	toScroll() {
		uni.pageScrollTo({
			scrollTop: 0,
			duration: 0,
		})
	},
	hideKey() {
		uni.hideKeyboard()
	},
	toast(msg) {
		uni.showToast({
			icon: 'none',
			title: msg,
			success: () => {
				setTimeout(() => {
					uni.hideToast();
				}, 1000)
			}
		})
	},
	loding(success) {
		uni.showLoading({
			title: 'Loding....',
			success: () => {
				setTimeout(() => {
					uni.hideLoading();
					success();
				}, 1000)
			}
		})
	},
	alert(content, title = '提示') {
		return new Promise((resolve, reject) => {
			uni.showModal({
				title,
				content,
				success: (res) => {
					if (res.confirm) {
						resolve('用户点击了确定')
					} else {
						reject('用户点击了取消');
					}

				}
			})
		})
	},
	getSystemInfo() {
		return new Promise((resolve, reject) => {
			uni.getSystemInfo({
				success: (res) => {
					resolve(res);
				},
				fail: (err) => {
					reject(err);
				}
			})
		})
	}
};
export default API;
