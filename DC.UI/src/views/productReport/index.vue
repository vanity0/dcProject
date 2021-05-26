<template>
  <a-row>
    <a-table
      bordered
      size="small"
      :columns="columns"
      :row-key="(row) => row.id"
      :data-source="data"
      :pagination="false"
      :loading="loading"
    >
      <template slot="title">
        <a-row :gutter="8">
          <a-col :span="5">
            <a-input v-model="queryParameter.name" placeholder="请输入关键字查询" />
          </a-col>

          <a-col :span="5">
            <a-date-picker
              style="width: 100%"
              placeholder="请选择日期"
              valueFormat="YYYY-MM-DD"
              v-model="queryParameter.date"
            />
          </a-col>
          <a-col :span="14">
            <a-button type="primary" icon="search" @click="handleRefresh">查询</a-button>
          </a-col>
        </a-row>
      </template>
    </a-table>
  </a-row>
</template>
<script>
import { productReportColumns, queryList } from '@/api/report'
export default {
  name: 'ProductReport',
  data() {
    return {
      // 要查询的参数
      queryParameter: {},
      // 绑定的数据源
      data: [],
      // 表格绑定的列
      columns: productReportColumns,
      // 加载动画
      loading: false,
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    // 加载数据
    loadData() {
      this.loading = true
      queryList(this.queryParameter).then((res) => {
        this.data = res.data
        this.loading = false
      })
    },
    // 刷新表格
    handleRefresh() {
      this.loadData()
    },
  },
}
</script>


