<template>
  <a-row>
    <a-col :span="24">
      <a-table
        bordered
        size="small"
        :columns="columns"
        :row-key="(row) => row.id"
        :data-source="data"
        :pagination="pagination"
        :row-selection="rowSelection"
        :loading="loading"
        @change="handleTableChange"
      >
        <template slot="deleteFlag" slot-scope="text">
          <a-badge :status="text | deleteFlagTypeFilter" :text="text | deleteFlagTextFilter" />
        </template>
        <span slot="action" slot-scope="text, item">
          <a @click="handleDelete(item)" v-action:delete_registerCps> <a-icon type="delete" />删除</a>
        </span>
        <template slot="title">
          <a-row :gutter="8">
            <a-col :span="5">
              <a-input-search
                v-model="queryParameter.key"
                compact
                placeholder="请输入关键字查询"
                @search="handleSearch"
              />
            </a-col>

            <a-col :span="5">
              <a-select allowClear v-model="queryParameter.status" placeholder="请选择状态" style="width: 100%">
                <a-select-option v-for="item in registerStatus" :key="item" :value="item">{{ item }}</a-select-option>
              </a-select>
            </a-col>

            <a-col :span="5">
              <a-date-picker
                style="width: 100%"
                placeholder="请选择日期"
                valueFormat="YYYY-MM-DD"
                v-model="queryParameter.time"
              />
            </a-col>

            <a-col :span="5">
              <a-button type="primary" icon="search" @click="handleSearch">查询</a-button>
            </a-col>
          </a-row>
        </template>
      </a-table>
    </a-col>
  </a-row>
</template>
<script>
import { registerUvColumns, queryUvHistory } from '@/api/uvHistory'
import { deleteRegister } from '@/api/register'
export default {
  name: 'cpsUvHistory',
  data() {
    return {
      // 要查询的参数
      queryParameter: {},
      // 绑定的数据源
      data: [],
      // 表格绑定的列
      columns: registerUvColumns,
      // 分页插件
      pagination: {},
      // 加载动画
      loading: false,
      // 当前选择所有行Key数据
      selectedRowKeys: [],
      // 表格选择列
      rowSelection: {
        // 选中项发生变化时的回调
        onChange: this.onRowSelectChange,
      },
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    // 加载数据
    loadData() {
      this.loading = true
      this.queryParameter.linkModel = 'CPS'
      let queryParams = {
        ...this.queryParameter,
        current: this.pagination.current,
        pageSize: this.pagination.pageSize,
      }
      queryUvHistory(queryParams).then((res) => {
        let { data, totalCount } = res.data
        const pagination = { ...this.pagination }
        pagination.total = totalCount
        this.loading = false
        this.data = data
        this.pagination = pagination
      })
    },
    handleSearch() {
      this.loadData()
    },
    // 刷新表格
    handleRefresh() {
      this.pagination.current = 1
      this.loadData()
    },
    // 单个删除
    handleDelete(item) {
      var that = this
      this.$confirm({
        title: '确定要删除吗?',
        onOk() {
          that.removeSubmit([item.id])
        },
        onCancel() {},
      })
    },
    // 批量选择删除
    handleBatchDelete() {
      let that = this
      if (this.selectedRowKeys.length > 0) {
        this.$confirm({
          title: '确定要删除吗?',
          onOk() {
            that.removeSubmit(that.selectedRowKeys)
          },
          onCancel() {},
        })
      } else {
        this.$message.warning('请选择至少一项!')
      }
    },
    removeSubmit(ids) {
      deleteRegister(ids).then((res) => {
        if (res.success) {
          this.$notification.success({ description: res.msg })
          this.handleRefresh()
        } else {
          this.$notification.error({ description: res.msg })
        }
      })
    },
    // 分页、排序、筛选变化时触发
    handleTableChange(page, filters, sorter) {
      const pager = { ...this.pagination }
      pager.current = page.current
      pager.pageSize = page.pageSize
      this.pagination = pager
      this.loadData()
    },
    // 选中项发生变化时的回调
    onRowSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
    },
  },
}
</script>


