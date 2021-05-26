<template>
	<view>
		<home v-if="PageCur=='home'"></home>
		<product v-if="PageCur=='product'"></product>
		<user v-if="PageCur=='user'"></user>
		<view class="cu-bar tabbar bg-white shadow foot">

			<view class="action" @click="NavChange" data-cur="home">
				<view class='cuIcon-cu-image'>
					<image :src="'/static/tabbar/home' + [PageCur=='home'?'_active':''] + '.png'"></image>
				</view>
				<view :class="PageCur=='home'?'text-green':'text-gray'">首页</view>
			</view>

			<view class="action" @click="NavChange" data-cur="product">
				<view class='cuIcon-cu-image'>
					<image :src="'/static/tabbar/product' + [PageCur == 'product'?'_active':''] + '.png'"></image>
				</view>
				<view :class="PageCur=='product'?'text-green':'text-gray'">产品列表</view>
			</view>
			<view class="action" @click="NavChange" data-cur="user">
				<view class='cuIcon-cu-image'>
					<image :src="'/static/tabbar/user' + [PageCur == 'user'?'_active':''] + '.png'"></image>
				</view>
				<view :class="PageCur=='user'?'text-green':'text-gray'">个人中心</view>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				PageCur: 'product'
			}
		},
		methods: {
			NavChange: function(e) {
				this.PageCur = e.currentTarget.dataset.cur
			}
		}
	}
</script>

<style>

</style>
