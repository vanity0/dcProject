<template>
  <a-row>
    <a-col v-show="!visibleModal" :span="24">
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
        <span slot="action" slot-scope="text, item">
          <a @click="handleDetails(item)"> <a-icon type="eye" />详情 </a>
          <a-divider type="vertical" />
          <a @click="handleDelete(item)"> <a-icon type="delete" />删除 </a>
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
            <a-col :span="19">
              <a-button type="danger" icon="minus-circle" @click="handleBatchDelete">删除</a-button>
              <a-divider type="vertical" />
              <a-button type="dashed" icon="vertical-align-bottom" @click="handleExport">导出</a-button>
              <a-divider type="vertical" />
              <a-popover placement="top">
                <template slot="content">刷新</template>
                <a-button @click="handleRefresh" :loading="loading" icon="sync"></a-button>
              </a-popover>
            </a-col>
          </a-row>
        </template>
      </a-table>
    </a-col>
    <a-col :span="24" v-if="visibleModal">
      <modify :id="id" @handleHideModal="handleHideModal" />
    </a-col>
  </a-row>
</template>
<script>
import { errorLogColumns, queryErrorLog, deleteErrorLog } from '@/api/errorLog'
import modify from './modify'
export default {
  name: 'ErrorLog',
  components: { modify },
  data() {
    return {
      // 要查询的参数
      queryParameter: {},
      // 绑定的数据源
      data: [],
      // 表格绑定的列
      columns: errorLogColumns,
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
      // 是否显示详情页面
      visibleModal: false,
      // 传递到详情页面的主键ID
      id: '',
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    // 加载数据
    loadData() {
      this.loading = true
       let queryParams = {
        ...this.queryParameter,
        current: this.pagination.current,
        pageSize: this.pagination.pageSize,
      }
      queryErrorLog(queryParams).then((res) => {
        let { data, totalCount } = res.data
        const pagination = { ...this.pagination }
        pagination.total = totalCount
        this.loading = false
        this.data = data
        this.pagination = pagination
      })
    },
    // 打开详情页面
    handleDetails(item) {
      this.id = item.id
      this.visibleModal = true
    },
    // 隐藏显示详情页
    handleHideModal() {
      this.visibleModal = false
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
      deleteErrorLog(ids).then((res) => {
        if (res.success) {
          this.$notification.success({ description: res.msg })
          this.handleRefresh()
        } else {
          this.$notification.error({ description: res.msg })
        }
      })
    },
    // 导出数据
    handleExport() {},
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


