<template>
  <a-card>
    <!--推手端后台显示-->
    <a-row :xs="24" :sm="24" :md="24" :lg="24" :xl="24" :xxl="24" :gutter="10">
      <a-col :xs="24" :sm="24" :md="10" :lg="10" :xl="10" :xxl="10">
        <a-row>
          <a-col :span="24">
            <a-card >
              <strong>{{ title }}</strong><br/>
              <a-tag color="blue" style="margin-top:5px;">上次登录时间：{{ lastLoginTime() }}</a-tag>
              <a-tag color="blue" style="margin-top:5px;">上次登录时间：{{ lastLoginTime() }}</a-tag>
            </a-card>
          </a-col>
        </a-row>
      </a-col>
      <a-col :xs="24" :sm="24" :md="14" :lg="14" :xl="14" :xxl="14">
        <a-row :gutter="10">
          <a-col :xs="8" :sm="8" :md="8" :lg="8" :xl="8" :xxl="8" style="text-align: center">
            <a-card hoverable style="background: rgb(255, 157, 77)">
              <a-icon slot="cover" type="eye" :style="{ fontSize: '48px', marginTop: '30px', color: '#ffff' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.pvCount" :duration="3000" style="color: #ffff" />
                  <strong style="color: #ffff">今日PV</strong></template
                >
              </a-card-meta>
            </a-card>
          </a-col>

          <a-col :xs="8" :sm="8" :md="8" :lg="8" :xl="8" :xxl="8" style="text-align: center">
            <a-card hoverable style="background: #87d068">
              <a-icon slot="cover" type="monitor" :style="{ fontSize: '48px', marginTop: '30px', color: '#ffff' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.uvCount" :duration="3000" style="color: #ffff" />
                  <strong style="color: #ffff">今日UV</strong></template
                >
              </a-card-meta>
            </a-card>
          </a-col>

          <a-col :xs="8" :sm="8" :md="8" :lg="8" :xl="8" :xxl="8"  style="text-align: center">
            <a-card hoverable style="background: #2db7f5">
              <a-icon slot="cover" type="solution" :style="{ fontSize: '48px', marginTop: '30px', color: '#ffff' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.cpaCount" :duration="3000" style="color: #ffff" />
                  <strong style="color: #ffff">今日CPA</strong></template
                >
              </a-card-meta>
            </a-card>
          </a-col>
        </a-row>
      </a-col>
    </a-row>

    <!--第三行-->
    <a-row :xs="24" :sm="16" :md="24" :lg="8" :xl="4" :gutter="10" class="el-top">
      <!--图表-->
      <a-col :span="16">
        <a-card>
          <div style="height: 300px" ref="ee1" id="ee1"></div>
        </a-card>
      </a-col>
      <a-col :span="8">
        <!--更新日志-->
        <a-row>
          <a-col :span="24">
            <a-card title="今日申请">
              <a-timeline :reverse="false" class="element-top">
                <a-timeline-item color="green" v-for="(item, index) in backlogs" :key="index">
                  <a-icon slot="dot" type="clock-circle-o" style="font-size: 16px" />{{ item }}
                </a-timeline-item>
              </a-timeline>
            </a-card>
          </a-col>
        </a-row>
      </a-col>
    </a-row>
  </a-card>
</template>
<script>
import { mapGetters } from 'vuex'
import countTo from 'vue-count-to'
import { queryDevCount, queryDevBacklog, queryDevReport } from '@/api/home'
import { timeFix } from '@/utils/util'
export default {
  name: 'DevHome',
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
      backlogs: [],
    }
  },
  created() {},
  mounted() {
    this.title = timeFix() + ',' + this.$store.getters.nickname
    queryDevCount().then((res) => {
      if (res.success) {
        this.entity = res.data
      }
    })
    queryDevBacklog().then((res) => {
      if (res.success) {
        this.backlogs = res.data
      }
    })
    this.loadCharts()
  },
  methods: {
    loadCharts() {
      let that = this
      queryDevReport().then((res) => {
        if (res.success) {
          var dateList = res.data.map(function (item) {
            return item.hour + '点'
          })
          var valueList = res.data.map(function (item) {
            return item.totalCount
          })
          that.initCharts(dateList, valueList)
        }
      })
    },
    initCharts(dateList, valueList) {
      let option = {
        title: [
          {
            left: 'center',
            text: '24小时访问量',
          },
        ],
        tooltip: {
          trigger: 'axis',
        },
        xAxis: [
          {
            data: dateList,
          },
        ],
        yAxis: [{}],
        series: [
          {
            type: 'line',
            showSymbol: false,
            data: valueList,
          },
        ],
      }
      var eel = document.getElementById('ee1') //this.$refs.ee1;
      let myChart = this.$echarts.init(eel)
      myChart.setOption(option)
    },
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