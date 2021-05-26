<template>
  <a-card :bordered="true" title="下级员工">
    <a-popover slot="extra" placement="top" href="javascript:void(0)">
      <template slot="content">返回</template>
      <a @click="handleGoBack">
        <a-icon type="rollback" />
      </a>
    </a-popover>

    <a-row :gutter="10">
      <a-col :span="6">
        <a-form-item label="上级用户" :label-col="labelCol" :wrapperCol="wrapperCol">
          <a-input disabled v-model="modifyEntity.parentName" />
        </a-form-item>
      </a-col>
    </a-row>

    <!--表格页面-->
    <my-table
      ref="myTable"
      :dataSource="dataSource"
      :columns="columns"
      :loading="loading"
      @handleAdd="handleAdd"
      @removeSubmit="removeSubmit"
      @handleEdit="handleEdit"
      @handleImport="handleImport"
      @handleExport="handleExport"
      @handleDetails="handleDetails"
      @loadData="loadData"
    >
      <template v-slot:buttons="{ item }" v-if="$route.meta.showMore">
        <a-dropdown>
          <a-menu slot="overlay">
            <a-menu-item>
              <a @click="handleResetPwd(item)" v-action:dcUser_resetPwd_childer><a-icon type="key" />重置密码</a>
            </a-menu-item>
          </a-menu>
          <a>
            更多
            <a-icon type="down" />
          </a>
        </a-dropdown>
      </template>
    </my-table>

    <!--修改页面-->
    <modify v-if="modifyChilderEntity.showModify" @handleRefresh="handleRefresh" :modifyEntity="modifyChilderEntity" />
  </a-card>
</template>
<script>
import modify from '@/views/dcUser/modify'
import myTable from '@/views/components/MyTable/index'
import { dcUserColumns, queryDCUserChilder, deleteDCUser, resetPwd } from '@/api/dcUser'

export default {
  name: 'childer',
  components: { modify, myTable },
  props: {
    modifyEntity: {
      type: Object,
      required: true,
      default: function () {
        return {
          // 父级role
          parentRoleId: '',
          // 父级名称
          parentName: '',
          // 父级id
          parentId: '',
          // 传递的主键
          id: '',
          // 页面标题
          title: '新增平台员工',
          // 页面操作类型
          operation: this.operationFlag.Add,
          // 是否展示编辑页面
          showModify: false,
          // 是否展示子集页面
          showChilder: false,
        }
      },
    },
  },
  data() {
    return {
      labelCol: { xs: { span: 24 }, sm: { span: 6 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 16 } },
      // 加载动画
      loading: false,
      // 绑定的数据源
      dataSource: [],
      // 表格绑定的列
      columns: dcUserColumns,
      modifyChilderEntity: {
        // 是否展示子集页面
        showChilder: false,
        // 上级角色ID
        parentRoleId: '',
        // 父级名称
        parentName: '',
        // 父级id
        parentId: '',
        // 传递的主键
        id: '',
        // 页面标题
        title: '',
        // 页面操作类型
        operation: '',
        // 是否展示编辑页面
        showModify: false,
      },
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    handleGoBack() {
      this.$emit('goBack')
    },
    //#region 查询列表
    // 保存/修改后刷新表格
    handleRefresh() {
      this.modifyChilderEntity.showModify = false
      this.$refs.myTable.pagination.current = 1
      this.loadData()
    },
    // 加载数据
    loadData() {
      this.loading = true
      let queryParams = {
        parentId: this.modifyEntity.parentId,
        current: this.$refs.myTable.pagination.current,
        pageSize: this.$refs.myTable.pagination.pageSize,
      }
      let that = this
      queryDCUserChilder(queryParams)
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

    //#region 重置密码
    handleResetPwd(item) {
      var that = this
      this.$confirm({
        title: '确定要重置密码为123456吗?',
        onOk() {
          resetPwd(item.id).then((res) => {
            if (res.success) {
              this.$notification.success({ description: res.msg })
              this.handleRefresh()
            } else {
              this.$notification.error({ description: res.msg })
            }
          })
        },
        onCancel() {},
      })
    },
    //#endregion

    //#region 添加
    // 打开添加页面
    handleAdd() {
      this.modifyChilderEntity = {
        parentRoleId: this.modifyEntity.parentRoleId,
        parentName: this.modifyEntity.parentName,
        parentId: this.modifyEntity.parentId,
        title: '新增下级平台员工',
        showModify: true,
        operation: this.operationFlag.Add,
      }
    },
    //#endregion

    //#region 删除
    // 删除
    removeSubmit(ids) {
      deleteDCUser(ids).then((res) => {
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
      this.modifyChilderEntity = {
        parentName: item.parentName,
        parentId: item.parentId,
        id: item.id,
        title: '修改平台员工',
        showModify: true,
        operation: this.operationFlag.Edit,
      }
    },
    //#endregion

    //#region 详情
    // 打开详情页面
    handleDetails(item) {
      this.modifyChilderEntity = {
        parentName: item.parentName,
        parentId: item.parentId,
        id: item.id,
        title: '平台员工详情',
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


