<template>
  <div>
    <!--查询参数-->
    <params
      ref="params"
      @handleRefresh="handleRefresh"
      :loading="loading"
    ></params>
    <!--表格页面-->
    <a-table
      ref="myTable"
      class="element-top"
      bordered
      size="small"
      :columns="columns"
      :row-key="(row) => row.id"
      :data-source="dataSource"
      :pagination="pagination"
      :row-selection="rowSelection"
      :loading="loading"
      @change="handleTableChange"
    >
      <template slot="status" slot-scope="text">
        <a-badge :status="text | statusTypeFilter" :text="text | statusTextFilter" />
      </template>

      <span slot="action" slot-scope="text, item">
        <a-space>
          <a @click="handleDetails(item)" v-action:dcUserDev_details> <a-icon type="eye" />详情 </a>
          <a @click="handleEdit(item)" v-action:dcUserDev_edit><a-icon type="edit" />修改</a>
          <a @click="handleDelete(item)" v-action:dcUserDev_delete><a-icon type="delete" />删除</a>
          <a-dropdown v-if="$route.meta.showMore">
            <a-menu slot="overlay">
              <a-menu-item>
                <a @click="handleResetPwd(item)" v-action:dcUserDev_resetPwd><a-icon type="key" />重置密码</a>
              </a-menu-item>
            </a-menu>
            <a>
              更多
              <a-icon type="down" />
            </a>
          </a-dropdown>
        </a-space>
      </span>

      <template slot="title">
        <a-row>
          <a-col :span="24">
            <a-popover placement="top">
              <template slot="content">刷新</template>
              <a-button class="element-right" @click="handleRefresh" :loading="loading" icon="sync"></a-button>
            </a-popover>
            <a-space>
              <a-button type="primary" icon="plus-circle" v-action:dcUserDev_create @click="handleAdd">添加</a-button>
              <a-button type="danger" icon="minus-circle" v-action:dcUserDev_delete @click="handleBatchDelete"
                >删除</a-button
              >
            </a-space>
          </a-col>
        </a-row>
      </template>
    </a-table>

    <!--修改页面-->
    <modify v-if="modifyEntity.showModify" @handleRefresh="handleRefresh" :modifyEntity="modifyEntity" />
  </div>
</template>
<script>
import modify from './modify'
import myTable from '@/views/components/MyTable/index'
import params from './components/params'
import { dcUserColumns, queryAllDCUser, deleteDCUser, resetPwd } from '@/api/dcUser'
export default {
  name: 'dcUser',
  components: { params, modify, myTable },
  data() {
    return {
      modifyEntity: {
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
      // 加载动画
      loading: false,
      // 绑定的数据源
      dataSource: [],
      // 表格绑定的列
      columns: dcUserColumns,
      // 当前选择所有行Key数据
      selectedRowKeys: [],
      selectedRows: [],
      // 分页插件
      pagination: {
        current: 1, // 当前页
        total: 0, // 总页数
        pageSize: 10, // 每页中显示10条数据
        showQuickJumper: true, // 是否可以快速跳转至某页
        showSizeChanger: true, // 是否可以改变 pageSize
        pageSizeOptions: ['10', '20', '50', '100'], //每页中显示的数据
        showTotal: (total) => `共${total}条数据`, // 分页中显示总的数据
      },
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
    // 分页、排序、筛选变化时触发
    handleTableChange(page, filters, sorter) {
      const pager = { ...this.pagination }
      pager.current = page.current
      pager.pageSize = page.pageSize
      this.pagination = pager
      this.loadData()
    },
    // 表格多选框
    onRowSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },

    //#region 查询列表
    // 保存/修改后刷新表格
    handleRefresh() {
      this.modifyEntity.showModify = false
      this.pagination.current = 1
      this.loadData()
    },
    // 加载数据
    loadData() {
      this.loading = true
      let queryParams = {
        ...this.$refs.params.params,
        current: this.pagination.current,
        pageSize: this.pagination.pageSize,
      }
      let that = this
      queryAllDCUser(queryParams)
        .then((res) => {
          let { data, totalCount } = res.data
          that.dataSource = data
          that.pagination.total = totalCount
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

    // 删除
    handleDelete(item) {
      var that = this
      this.$confirm({
        title: '确定要删除吗?',
        onOk() {
          that.removeSubmit([item.id], [item])
        },
        onCancel() {},
      })
    },
    // 批量删除
    handleBatchDelete() {
      let that = this
      if (this.selectedRowKeys.length > 0) {
        this.$confirm({
          title: '确定要删除吗?',
          onOk() {
            that.removeSubmit(that.selectedRowKeys, that.selectedRows)
          },
          onCancel() {},
        })
      } else {
        this.$message.warning('请选择至少一项!')
      }
    },
    // 打开添加页面
    handleAdd() {
      this.modifyEntity = {
        parentRoleId: '总平台',
        parentName: '',
        parentId: '',
        id: '',
        title: '新增平台员工',
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
      this.modifyEntity = {
        parentRoleId: '',
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
      this.modifyEntity = {
        parentRoleId: '',
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


