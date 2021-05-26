<template>
  <div>
    <!--查询参数-->
    <params v-show="!modifyEntity.showModify" ref="params" @handleRefresh="handleRefresh" :loading="loading"></params>
    <!--表格页面-->
    <my-table
      ref="myTable"
      v-show="!modifyEntity.showModify"
      :dataSource="dataSource"
      :columns="columns"
      :loading="loading"
      :hasPage="false"
      @handleAdd="handleAdd"
      @removeSubmit="removeSubmit"
      @handleEdit="handleEdit"
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
import { menuColumns, queryMenu, deleteMenu } from '@/api/menu'
export default {
  name: 'Menu',
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
      columns: menuColumns,
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
      this.loadData()
    },
    // 加载数据
    loadData() {
      this.loading = true
      let that = this
      queryMenu(this.$refs.params.params)
        .then((res) => {
          that.dataSource = res.data
          that.loading = false
        })
        .catch(() => {
          that.loading = false
        })
    },
    //#endregion

    //#region 添加
    // 打开添加页面
    handleAdd() {
      this.modifyEntity = {
        title: '新增菜单',
        showModify: true,
        operation: this.operationFlag.Add,
      }
    },
    //#endregion

    //#region 删除
    // 删除
    removeSubmit(ids) {
      deleteMenu(ids).then((res) => {
        if (res.success) {
          this.$notification.success({ description: res.msg })
          this.handleRefresh()
        } else {
          this.$notification.error({ description: res.msg })
        }
      })
    },
    //#endregion

    //#region 修改
    // 打开编辑页面
    handleEdit(item) {
      this.modifyEntity = {
        id: item.id,
        title: '修改菜单',
        showModify: true,
        operation: this.operationFlag.Edit,
      }
    },
    //#endregion

    //#region 详情
    // 打开详情页面
    handleDetails(item) {
      this.modifyEntity = {
        id: item.id,
        title: '菜单详情',
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


