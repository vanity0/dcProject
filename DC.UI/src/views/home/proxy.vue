<template>
  <a-card title="商家端">
    <a-row>
      <a-col :span="24">
        <!--商家后台显示-->
        <a-row :gutter="10">
          <a-col :span="16">
            <a-row :gutter="10">
              <a-col :span="6" style="text-align: center">
                <a-card hoverable style="background: rgb(255, 157, 77)">
                  <a-icon slot="cover" type="eye" :style="{ fontSize: '48px', marginTop: '30px', color: '#ffff' }" />
                  <a-card-meta title>
                    <template slot="description">
                      <countTo :startVal="0" :endVal="entity.pvCount" :duration="3000" style="color: #ffff" />
                      <strong style="color: #ffff">PV-访问量</strong></template
                    >
                  </a-card-meta>
                </a-card>
              </a-col>

              <a-col :span="6" style="text-align: center">
                <a-card hoverable style="background: #87d068">
                  <a-icon
                    slot="cover"
                    type="monitor"
                    :style="{ fontSize: '48px', marginTop: '30px', color: '#ffff' }"
                  />
                  <a-card-meta title>
                    <template slot="description">
                      <countTo :startVal="0" :endVal="entity.uvCount" :duration="3000" style="color: #ffff" />
                      <strong style="color: #ffff">UV-点击量</strong></template
                    >
                  </a-card-meta>
                </a-card>
              </a-col>

              <a-col :span="6" style="text-align: center">
                <a-card hoverable style="background: #2db7f5">
                  <a-icon
                    slot="cover"
                    type="solution"
                    :style="{ fontSize: '48px', marginTop: '30px', color: '#ffff' }"
                  />
                  <a-card-meta title>
                    <template slot="description">
                      <countTo :startVal="0" :endVal="entity.cpaCount" :duration="3000" style="color: #ffff" />
                      <strong style="color: #ffff">CPA-注册量</strong></template
                    >
                  </a-card-meta>
                </a-card>
              </a-col>

              <a-col :span="6" style="text-align: center">
                <a-card hoverable style="background: #f50">
                  <a-icon
                    slot="cover"
                    type="android"
                    :style="{ fontSize: '48px', marginTop: '30px', color: '#ffff' }"
                  />
                  <a-card-meta title>
                    <template slot="description">
                      <countTo :startVal="0" :endVal="entity.androidCount" :duration="3000" style="color: #ffff" />
                      <span style="color: #ffff">/</span>
                      <countTo :startVal="0" :endVal="entity.iosCount" :duration="3000" style="color: #ffff" />
                      <strong style="color: #ffff">andrord/ios</strong></template
                    >
                  </a-card-meta>
                </a-card>
              </a-col>
            </a-row>
          </a-col>

          <a-col :span="8">
            <a-row>
              <a-col :span="24">
                <a-card :title="title">
                  <a-tag color="pink">上次登录IP：{{ lastLoginIp() }}</a-tag>
                  <a-tag color="blue">上次登录时间：{{ lastLoginTime() }}</a-tag>
                </a-card>
              </a-col>
            </a-row>
          </a-col>
        </a-row>
      </a-col>
    </a-row>
  </a-card>
</template>
<script>
import countTo from 'vue-count-to'
import { queryProxyCount } from '@/api/home'
import { timeFix } from '@/utils/util'
import { mapGetters } from 'vuex'
export default {
  name: 'Home',
  components: { countTo },
  data() {
    return {
      ...mapGetters(['lastLoginIp', 'lastLoginTime']),
      title: '',
      entity: {
        pvCount: 0,
        uvCount: 0,
        cpaCount: 0,
        iosCount: 0,
        androidCount: 0,
      },
      backlogs:[]
    }
  },
  created() {},
  mounted() {
    this.title = timeFix() + ',' + this.$store.getters.nickname
    queryProxyCount().then((res) => {
      if (res.success) {
        this.entity = res.data
      }
    })
    // queryProxyBacklog().then((res) => {
    //   if (res.success) {
    //     this.backlogs = res.data
    //   }
    // })
    // this.loadCharts()
  },
  methods: {
  },
}
</script>
<style scoped>
.el-top {
  margin-top: 10px;
}

.ant-carousel >>> .slick-slide {
  text-align: center;
  height: 160px;
  line-height: 160px;
  background: #364d79;
  overflow: hidden;
}

.ant-carousel >>> .custom-slick-arrow {
  width: 25px;
  height: 25px;
  font-size: 25px;
  color: #fff;
  background-color: rgba(31, 45, 61, 0.11);
  opacity: 0.3;
}
.ant-carousel >>> .custom-slick-arrow:before {
  display: none;
}
.ant-carousel >>> .custom-slick-arrow:hover {
  opacity: 0.5;
}

.ant-carousel >>> .slick-slide h3 {
  color: #fff;
}
</style>