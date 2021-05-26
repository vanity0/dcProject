<template>
  <div>
    <!--查询参数-->
    <params ref="params" @handleRefresh="handleRefresh" :loading="loading"></params>
    <!--表格页面-->
    <my-table
      ref="myTable"
      :scroll="{ x: 1200 }"
      :dataSource="dataSource"
      :columns="columns"
      :loading="loading"
      @removeSubmit="removeSubmit"
      @loadData="loadData"
    >
    </my-table>
  </div>
</template>
<script>
import myTable from '@/views/components/MyTable/index'
import params from './components/params'
import { registerDevColumns, queryRegister, deleteRegister } from '@/api/register'
export default {
  name: 'ApiLog',
  components: { params, myTable },
  data() {
    return {
      // 加载动画
      loading: false,
      // 绑定的数据源
      dataSource: [],
      // 表格绑定的列
      columns: registerDevColumns,
    }
  },

  mounted() {
    this.loadData()
  },
  methods: {
    //#region 查询列表
    // 保存/修改后刷新表格
    handleRefresh() {
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
      queryRegister(queryParams)
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
      deleteRegister(ids).then((res) => {
        if (res.success) {
          this.$notification.success({ description: res.msg })
          this.handleRefresh()
        } else {
          this.$notification.error({ description: res.msg })
        }
      })
    },
    //#endregion
  },
}
</script>


