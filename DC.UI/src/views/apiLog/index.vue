<template>
  <div>
    <!--查询参数-->
    <params ref="params" @handleRefresh="handleRefresh" :loading="loading"></params>
    <!--表格页面-->
    <my-table
      ref="myTable"
      :dataSource="dataSource"
      :columns="columns"
      :loading="loading"
      @removeSubmit="removeSubmit"
      @handleImport="handleImport"
      @handleExport="handleExport"
      @handleDetails="handleDetails"
      @loadData="loadData"
    />
    <!--修改页面-->
    <modify v-if="modifyEntity.showModify" @handleRefresh="handleRefresh" :modifyEntity="modifyEntity" />
  </div>
</template>
<script>
import modify from './modify'
import myTable from '@/views/components/MyTable/index'
import params from './components/params'
import { apiLogColumns, queryApiLog, deleteApiLog } from '@/api/apiLog'

export default {
  name: 'ApiLog',
  components: { params, modify, myTable },
  data() {
    return {
      modifyEntity: {
        // 传递的主键
        id: '',
        // 页面标题
        title: '',
        // 页面操作类型
        operation: '',
        // 是否展示编辑页面
        showModify: false,
      },
      // 加载动画
      loading: false,
      // 绑定的数据源
      dataSource: [],
      // 表格绑定的列
      columns: apiLogColumns,
    }
  },

  mounted() {
    this.loadData()
  },
  methods: {
    //#region 查询列表
    // 保存/修改后刷新表格
    handleRefresh() {
      this.modifyEntity.showModify = false
      this.$refs.myTable.pagination.current = 1
      this.loadData()
    },
    // 加载数据
    loadData() {
      this.loading = true
        let queryParams = {
        ...this.$refs.params.params,
        current: this.$refs.myTable.pagination.current,
        pageSize: this.$refs.myTable.pagination.pageSize,
      }
      let that = this
      queryApiLog(queryParams)
        .then((res) => {
          let { data, totalCount } = res.data
          that.dataSource = data
          that.$refs.myTable.pagination.total = totalCount
          that.loading = false
        })
        .catch(() => {
          that.loading = false
        })
    },
    //#endregion

    //#region 删除
    // 删除
    removeSubmit(ids) {
      deleteApiLog(ids).then((res) => {
        if (res.success) {
          this.$notification.success({ description: res.msg })
          this.handleRefresh()
        } else {
          this.$notification.error({ description: res.msg })
        }
      })
    },
    //#endregion

    //#region 详情
    // 打开详情页面
    handleDetails(item) {
      this.modifyEntity = {
        id: item.id,
        title: '请求日志详情',
        showModify: true,
        operation: this.operationFlag.Details,
      }
    },
    //#endregion

    //#region 导入
    // 导入数据
    handleImport() {},
    //#endregion

    //#region 导出
    // 导出数据
    handleExport() {},
    //#endregion
  },
}
</script>


