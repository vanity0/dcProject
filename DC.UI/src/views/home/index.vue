<template>
  <a-card title="总后台">
    <a-row>
      <a-col :span="24">
        <!--总后台显示-->
        <a-row :gutter="10">
          <a-col :span="4" style="text-align: center">
            <a-card hoverable>
              <a-icon slot="cover" type="team" :style="{ fontSize: '48px', marginTop: '30px', color: '#2db7f5' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.devCount" :duration="3000" />
                  <strong>推客总数</strong></template
                >
              </a-card-meta>
            </a-card>
          </a-col>
          <a-col :span="4" style="text-align: center">
            <a-card hoverable>
              <a-icon slot="cover" type="smile" :style="{ fontSize: '48px', marginTop: '30px', color: '#87d068' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.productCount" :duration="3000" />
                  <strong>产品总数</strong></template
                >
              </a-card-meta>
            </a-card>
          </a-col>
          <a-col :span="4" style="text-align: center">
            <a-card hoverable>
              <a-icon slot="cover" type="monitor" :style="{ fontSize: '48px', marginTop: '30px', color: '#108ee9' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.pvCount" :duration="3000" />
                  <strong>今日PV</strong>
                </template>
              </a-card-meta>
            </a-card>
          </a-col>
          <a-col :span="4" style="text-align: center">
            <a-card hoverable>
              <a-icon slot="cover" type="eye" :style="{ fontSize: '48px', marginTop: '30px', color: '#FA8C16' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.uvCount" :duration="3000" />
                  <strong>今日UV</strong></template
                >
              </a-card-meta>
            </a-card>
          </a-col>
          <a-col :span="4" style="text-align: center">
            <a-card hoverable>
              <a-icon slot="cover" type="solution" :style="{ fontSize: '48px', marginTop: '30px', color: '#722ed1' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.cpaCount" :duration="3000" />
                  <strong>今日CPA</strong>
                </template>
              </a-card-meta>
            </a-card>
          </a-col>
          <a-col :span="4" style="text-align: center">
            <a-card hoverable>
              <a-icon slot="cover" type="android" :style="{ fontSize: '48px', marginTop: '30px', color: '#13c2c2' }" />
              <a-card-meta title>
                <template slot="description">
                  <countTo :startVal="0" :endVal="entity.iosCount" :duration="3000" />/
                  <countTo :startVal="0" :endVal="entity.androidCount" :duration="3000" />
                  <strong>安卓/IOS</strong></template
                >
              </a-card-meta>
            </a-card>
          </a-col>
        </a-row>

        <!--第三行-->
        <a-row :gutter="10" class="el-top">
          <!--图表-->
          <a-col :span="16">
            <a-card>
              <div style="height: 300px" ref="ee1" id="ee1"></div>
            </a-card>
          </a-col>
          <a-col :span="8">
            <!--待办项-->
            <a-row>
              <a-col :span="24">
                <a-card title="待处理产品">
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
      </a-col>
    </a-row>
  </a-card>
</template>
<script>
import countTo from 'vue-count-to'
import { queryDCCount, queryBacklog, queryReport } from '@/api/home'
export default {
  name: 'Home',
  components: { countTo },
  data() {
    return {
      entity: {
        devCount: 0,
        productCount: 0,
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
    this.loadCharts()
    queryDCCount().then((res) => {
      if (res.success) {
        this.entity = res.data
      }
    })

    queryBacklog().then((res) => {
      if (res.success) {
        this.backlogs = res.data
      }
    })
  },
  methods: {
    loadCharts() {
      let that = this
      queryReport().then((res) => {
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