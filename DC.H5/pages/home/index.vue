<template>
	<view>
		<view class="bg-img flex align-center" style="background-image: url('/static/home.gif');height: 480upx;">
			<view class="padding-xl text-white">
				<view class="padding-xs text-bold text-shadow" style="color: #dcc8a5;">
					您说提多少
				</view>
				<view class="padding-xs text-bold text-shadow" style="color: #dcc8a5;">
					就提多少！
				</view>
			</view>
		</view>

		<!--产品类别-->
		<view class="cu-list grid col-3 no-border'">
			<view class="cu-item" v-for="(item,index) in cuIconList" :key="index">
				<view :class="['cuIcon-' + item.cuIcon,'text-' + item.color]">
					<view class="cu-tag badge" v-if="item.badge!=0">
						<block v-if="item.badge!=1">{{item.badge>99?'99+':item.badge}}</block>
					</view>
				</view>
				<text>{{item.name}}</text>
			</view>
		</view>

		<!--类别-->
		<view class="nav-list">
			<navigator hover-class='none' :url="'/pages/component/' + item.name" class="nav-li" navigateTo
				:class="'bg-'+item.color" :style="[{animation: 'show ' + ((index+1)*0.2+1) + 's 1'}]"
				v-for="(item,index) in elements" :key="index">
				<view class="nav-title">{{item.title}}</view>
				<view class="nav-name">{{item.name}}</view>
				<text :class="'cuIcon-' + item.cuIcon"></text>
			</navigator>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				elements: [{
						title: '小额大额',
						name: 'bar',
						color: 'green',
						cuIcon: 'vipcard'
					},
					{
						title: '权益列表',
						name: 'nav',
						color: 'orange',
						cuIcon: 'formfill'
					},
					{
						title: '助贷系列',
						name: 'list',
						color: 'pink',
						cuIcon: 'list'
					},
					{
						title: '贷超列表',
						name: 'card',
						color: 'cyan',
						cuIcon: 'newsfill'
					}
				],
				cuIconList: [{
					cuIcon: 'cardboardfill',
					color: 'red',
					badge: 120,
					name: '利息低'
				}, {
					cuIcon: 'recordfill',
					color: 'orange',
					badge: 1,
					name: '审批快'
				}, {
					cuIcon: 'picfill',
					color: 'yellow',
					badge: 0,
					name: '门槛低'
				}, {
					cuIcon: 'noticefill',
					color: 'olive',
					badge: 22,
					name: '额度高'
				}, {
					cuIcon: 'upstagefill',
					color: 'cyan',
					badge: 0,
					name: '可分期'
				}, {
					cuIcon: 'clothesfill',
					color: 'blue',
					badge: 0,
					name: '还款灵活'
				}],
			}
		},
		methods: {
			Gridchange(e) {
				this.gridCol = e.detail.value
			},
			Gridswitch(e) {
				this.gridBorder = e.detail.value
			},
		}
	}
</script>

<style>

</style>
