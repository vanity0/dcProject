<template>
	<view>
		<scroll-view scroll-y class="page">
			<!--顶部-->
			<view class="cu-bar bg-white solid-bottom">
				<view class="action">
					<text class="cuIcon-titles text-orange "></text> 个人中心
				</view>
				<view class="action">
					<navigator class="action" url="design" hover-class="none">
						<text class="cuIcon-settings"></text>
					</navigator>
				</view>
			</view>

			<!--顶部-->
			<view class="cu-card case no-card">
				<view class="cu-item shadow">

					<view class="cu-list menu-avatar">
						<view class="cu-item">
							<view class="cu-avatar xl round"
								style="background-image:url(https://ossweb-img.qq.com/images/lol/web201310/skin/big99008.jpg);">
							</view>
							<view class="content flex-sub margin-left">
								<view class="text-grey">185****4125</view>
								<view class="text-gray text-sm flex justify-between">
									普通用户
								</view>
							</view>
						</view>
					</view>
					<view class="image" style="width: 100%;height: 70px;margin: 16px 0px 0;">
						<view class="cu-tag bg-red">VIP专属</view>
						<view class="cu-bar bg-shadeBottom"> <text class="text-cut">让借款更简单，让生活更多彩</text></view>
					</view>
				</view>
			</view>

			<!--菜单sm-border-->
			<view class="cu-list menu margin-top">
				<view class="cu-item arrow">
					<button class="cu-btn content" open-type="contact">
						<text class="cuIcon-btn text-red  margin-right-xs"></text>
						<text class="text-grey">免费福利</text>
					</button>
					<view class="action">
						<view class="cu-tag round bg-orange light">音乐</view>
						<view class="cu-tag round bg-olive light">电影</view>
						<view class="cu-tag round bg-blue light">旅行</view>
					</view>
				</view>

				<view class="cu-item arrow">
					<button class="cu-btn content" open-type="contact">
						<text class="cuIcon-discoverfill text-orange"></text>
						<text class="text-grey">关于我们</text>
					</button>
				</view>

				<view class="cu-item arrow">
					<button class="cu-btn content" open-type="contact">
						<text class="cuIcon-btn text-yellow"></text>
						<text class="text-grey">免责声明</text>
					</button>
				</view>

				<view class="cu-item arrow">
					<button class="cu-btn content" open-type="contact">
						<text class="cuIcon-tagfill text-cyan"></text>
						<text class="text-grey">投诉反馈</text>
					</button>
				</view>

				<view class="cu-item arrow">
					<button class="cu-btn content" open-type="contact">
						<text class="cuIcon-btn text-yellow"></text>
						<text class="text-grey">退出登录</text>
					</button>
				</view>
				<view class="cu-item">
					<view class="content">
						<text class="cuIcon-warn text-green"></text>
						<text class="text-grey">当前版本</text>
					</view>
					<view class="action">
						<text class="text-grey text-sm">V1.0.2</text>
					</view>
				</view>
			</view>


			<view class="cu-tabbar-height"></view>
		</scroll-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {

			}
		},
		methods: {

		}
	}
</script>

<style scoped>
	.cu-bar .action:last-child {
		margin-right: 6px;
	}
</style>
