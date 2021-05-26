<template>
	<view>
		<scroll-view :scroll-y="true" class="page">
			<!--轮播图 square-dot-->
			<swiper class="screen-swiper square-dot" :indicator-dots="true" :circular="true" :autoplay="true"
				interval="3000" duration="500">
				<swiper-item v-for="(item,index) in swiperList" :key="index">
					<image :src="item.url" mode="aspectFill" v-if="item.type=='image'"></image>
					<video :src="item.url" autoplay loop muted :show-play-btn="false" :controls="false"
						objectFit="cover" v-if="item.type=='video'"></video>
				</swiper-item>
			</swiper>


			<!-- <scroll-view scroll-x class="bg-white nav" scroll-with-animation :scroll-left="scrollLeft">
				<view class="flex text-center">
					<view class="cu-item flex-sub" :class="'0'==TabCur?'text-orange cur':''" key="0" @tap="tabSelect"
						data-id="0">小额
					</view>
					<view class="cu-item flex-sub" :class="'1'==TabCur?'text-orange cur':''" key="1" @tap="tabSelect"
						data-id="1">
						大额
					</view>
					<view class="cu-item flex-sub" :class="'2'==TabCur?'text-orange cur':''" key="2" @tap="tabSelect"
						data-id="2">
						权益
					</view>
					<view class="cu-item flex-sub" :class="'3'==TabCur?'text-orange cur':''" key="3" @tap="tabSelect"
						data-id="3">
						助贷
					</view>
					<view class="cu-item flex-sub" :class="'4'==TabCur?'text-orange cur':''" key="4" @tap="tabSelect"
						data-id="4">
						超贷
					</view>
				</view>
			</scroll-view> -->

			<view class="cu-list menu-avatar">
				<view class="cu-item" v-for="item in productList" :key="item.id">
					<view class="cu-avatar round lg" :style="'background-image:url('+item.logo+');'">
						<view class="cu-tag badge">{{item.tag}}</view>
					</view>
					<view class="content">
						<view class="text-grey">
							<view class="text-cut">{{item.name}}</view>
							<view class="cu-tag round bg-orange sm">已有{{item.edu}}人放款</view>
						</view>
						<view class="text-gray text-sm flex">
							<!-- <view class="text-grey text-xs">
								<text class="text-gray">日利率</text><text
									class="text-price text-bold">{{item.lilv}}</text>
							</view> -->
							<view class="text-cut margin-left-xs">
								<text class="text-orange">{{item.edu}}人数申请</text>
							</view>
						</view>
					</view>
					<view class="action">
						<button class="cu-btn round sm bg-red shadow">立即申请</button>
					</view>
				</view>
			</view>

			<view class="solids-bottom padding-xs flex align-center" v-if="this.productList.length==0">
				<view class="flex-sub text-center">
					<view class="padding cuIcon-emoji text-gray">空空如也</view>
				</view>
			</view>

			<view class="loadingbottom" v-if="this.productList.length>0">
				<text @click="loadList(2)"> {{this.loadingText}}</text>
			</view>

			<view class="cu-tabbar-height"></view>
		</scroll-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				scrollLeft: 0,
				TabCur: '0',
				productList: [],
				cardCur: 0,
				swiperList: [{
					id: 0,
					type: 'image',
					url: '/static/2.png'
				}, 
				{
					id: 1,
					type: 'image',
					url: '/static/3.png'
				}, 
				{
					id: 2,
					type: 'image',
					url: '/static/4.png'
				}],
				loadingStatus: true,
				loadingText: '查看更多...',
				showLoadingText: false,
				// 每页多少条
				pageSize: 5,
				// 当前第几页
				pageNo: 1,
			};
		},
		created() {
			this.loadList(1);
		},
		methods: {
			loadList(actionType) {
				if (actionType == 1) //刷新
				{
					this.pageNo = 1;
					this.productList = [];
					this.getProducts();
					this.loadingStatus = true;
					this.loadingText = '查看更多...';
					uni.stopPullDownRefresh();
				}
				if (actionType == 2) //上拉加载
				{
					if (this.loadingStatus) {
						this.loadingText = '加载中...';
						this.getProducts();
					}
				}
			},
			async getProducts() {
				var requestData = {
					pageNo: this.pageNo,
					pageSize: this.pageSize, 
				};
				this.$api.ajax({
					url: '/api/product/queryProductH5',
					method: 'get',
					data: requestData,
					success: (res) => {
						if (res.statusCode == 200) {
							if (res.data.data != null) {
								if (res.data.data.length > 0) {
									this.productList = this.productList.concat(res.data.data);
								}
							}
							if (res.data.totalPage >requestData.pageNo) {
								this.pageNo++;
								this.loadingText = '查看更多...';
							} else {
								this.loadingStatus = false;
								this.loadingText = '没有更多了';
							}

						}
					}
				});
			},
			tabSelect(e) {
				this.TabCur = e.currentTarget.dataset.id;
				if (this.TabCur == '0') {
					this.productList = [{
						id: 15,
						tag: '利息低',
						name: '速贷直通车',
						number: '6578',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					}]
				} else if (this.TabCur == '1') {
					this.productList = [{
						id: 14,
						tag: '利息低',
						name: '速贷直通车',
						number: '6578',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					}, {
						id: 13,
						tag: '利息低',
						name: '速贷直通车',
						number: '6578',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					}, {
						id: 12,
						tag: '利息低',
						name: '速贷直通车',
						number: '6578',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					}, ]
				} else if (this.TabCur == '2') {
					this.productList = [{
						id: 1,
						tag: '审批快',
						name: '秒想借',
						number: '123',
						lilv: '1.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					}, {
						id: 2,
						tag: '审批快',
						name: '秒想借',
						number: '123',
						lilv: '1.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					}, {
						id: 3,
						tag: '审批快',
						name: '秒想借',
						number: '123',
						lilv: '1.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					}, {
						id: 4,
						tag: '审批快',
						name: '秒想借',
						number: '123',
						lilv: '1.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					}, {
						id: 5,
						tag: '审批快',
						name: '秒想借',
						number: '123',
						lilv: '1.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					}, ]
				} else if (this.TabCur == '3') {
					this.productList = [{
							id: 6,
							tag: '门槛低',
							name: '钱袋',
							number: '0',
							lilv: '0.33%',
							edu: '20000~80000',
							url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
						},
						{
							id: 7,
							tag: '额度高',
							name: '芝麻分期',
							number: '23',
							lilv: '0.03%',
							edu: '20000~80000',
							url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
						}, {
							id: 8,
							tag: '门槛低',
							name: '钱袋',
							number: '0',
							lilv: '0.33%',
							edu: '20000~80000',
							url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
						},
						{
							id: 9,
							tag: '额度高',
							name: '芝麻分期',
							number: '23',
							lilv: '0.03%',
							edu: '20000~80000',
							url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
						},
					]
				} else if (this.TabCur == '4') {
					this.productList = [{
							id: 10,
							tag: '门槛低',
							name: '钱袋',
							number: '0',
							lilv: '0.33%',
							edu: '20000~80000',
							url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
						},
						{
							id: 11,
							tag: '额度高',
							name: '芝麻分期',
							number: '23',
							lilv: '0.03%',
							edu: '20000~80000',
							url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
						},
					]
				}
			},
			// cardSwiper
			cardSwiper(e) {
				this.cardCur = e.detail.current
			},
		}
	}
</script>

<style>
	.page {
		height: 100Vh;
		width: 100vw;
	}

	.page.show {
		overflow: hidden;
	}

	.switch-sex::after {
		content: "\e716";
	}

	.switch-sex::before {
		content: "\e7a9";
	}

	.switch-music::after {
		content: "\e66a";
	}

	.switch-music::before {
		content: "\e6db";
	}

	.page {
		height: 92vh;
	}

	.loadingbottom {
		text-align: center;
		padding: 0.68rem 0rem 1.2rem 0rem;
		color: dimgray;
		background-color: rgb(248, 248, 248);
	}
</style>

<!-- {
						id: 10,
						tag: '利息低',
						name: '速贷直通车',
						number: '6578',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					},
					{
						id: 19,
						tag: '审批快',
						name: '秒想借',
						number: '123',
						lilv: '1.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					},
					{
						id: 18,
						tag: '门槛低',
						name: '钱袋',
						number: '0',
						lilv: '0.33%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					},
					{
						id: 17,
						tag: '额度高',
						name: '芝麻分期',
						number: '23',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					},
					{
						id: 16,
						tag: '可分期',
						name: '天居消费',
						number: '99',
						lilv: '4.23%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big21003.jpg'
					},
					{
						id: 15,
						tag: '利息低',
						name: '狗贷',
						number: '3445',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big21002.jpg'
					},
					{
						id: 14,
						tag: '还款灵活',
						name: '钱钱来',
						number: '32434',
						lilv: '0.03%',
						edu: '20000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					},
					{
						id: 133,
						tag: '额度高',
						name: '宝贝盆',
						number: '9234',
						lilv: '0.13%',
						edu: '2000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big21002.jpg'
					},
					{
						id: 122,
						tag: '可分期',
						name: '有钱人',
						number: '21313',
						lilv: '0.03%',
						edu: '2000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/img/champion/Taric.png'
					},
					{
						id: 12,
						tag: '额度高',
						name: '速通车',
						number: '1122',
						lilv: '1.03%',
						edu: '2000~80000',
						url: 'https://ossweb-img.qq.com/images/lol/web201310/skin/big81020.jpg'
					}, -->
