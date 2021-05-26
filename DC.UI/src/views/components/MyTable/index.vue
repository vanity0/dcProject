<template>
  <a-table
    class="element-top"
    bordered
    size="small"
    :scroll="scroll"
    :columns="columns"
    :row-key="(row) => row[rowKey]"
    :data-source="dataSource"
    :pagination="hasPage ? pagination : hasPage"
    :row-selection="rowSelection"
    :loading="loading"
    @change="handleTableChange"
  >
    <template slot="linkModel" slot-scope="text">
      {{ text | linkModelFilter }}
    </template>

    <template slot="tel" slot-scope="text">
      {{ text | telFilter }}
    </template>

    <!--扩展按钮-->
    <slot name="expandedRowRender"></slot>
    <template slot="logo" slot-scope="text">
      <img width="60" style="cursor: pointer" height="60" :src="text" />
    </template>

    <!--推广数据状态-->
    <template slot="registerStatus" slot-scope="text">
      <a-tag :color="text | registerStatusTypeFilter">{{ text | registerStatusTextFilter }}</a-tag>
    </template>

    <!--产品状态-->
    <template slot="auditStatus" slot-scope="text">
      <a-tag :color="text | prodStatusTypeFilter">{{ text | prodStatusTextFilter }}</a-tag>
    </template>

    <template slot="h5" slot-scope="text">
      <a-tag :color="text | h5TypeFilter">{{ text | h5TextFilter }}</a-tag>
    </template>

    <template slot="status" slot-scope="text">
      <a-badge :status="text | statusTypeFilter" :text="text | statusTextFilter" />
    </template>

    <span slot="action" slot-scope="text, item">
      <a :disabled="item.sort === 1" @click="handleUp(item)" v-action:product_up> <a-icon type="arrow-up" />上移</a>
      <a
        style="margin-left: 5px"
        :disabled="item.sort === pagination.total"
        @click="handleDown(item)"
        v-action:product_down
      >
        <a-icon type="arrow-down" />下移</a
      >
      <a style="margin-left: 5px" @click="handleDetails(item)" v-action="detailsActions"> <a-icon type="eye" />详情 </a>
      <a style="margin-left: 5px" @click="handleEdit(item)" v-action="editActions"> <a-icon type="edit" />修改 </a>
      <a
        style="margin-left: 5px"
        @click="handleDelete(item)"
        v-action="deleteActions"
        :disabled="item.auditStatus === '上架中'"
      >
        <a-icon type="delete" />删除
      </a>
      <slot style="margin-left: 5px" name="buttons" :item="item"></slot>
    </span>

    <template slot="title">
      <a-row>
        <a-col>
          <a-popover placement="top">
            <template slot="content">刷新</template>
            <a-button class="element-right" @click="handleRefresh" :loading="loading" icon="sync"></a-button>
          </a-popover>
          <a-space>
            <!-- <a-button
              type="primary"
              icon="plus-circle"
              v-clipboard:copy="inputAllUrl"
              v-clipboard:success="handleSuccess"
              v-clipboard:error="handleError"
              v-action:productDev_bathCopyUrl
              >一键复制</a-button
            > -->
            <a-button type="primary" icon="plus-circle" v-action="createActions" @click="handleAdd">添加</a-button>
            <a-button type="danger" icon="minus-circle" v-action="deleteActions" @click="handleBatchDelete"
              >删除</a-button
            >
            <a-button
              type="dashed"
              icon="vertical-align-top"
              v-action="importAction"
              @click="handleImport"
              v-action:import
              >导入</a-button
            >
            <a-button
              type="dashed"
              icon="vertical-align-bottom"
              v-action="exportAction"
              @click="handleExport"
              v-action:export
              >导出</a-button
            >
          </a-space>
        </a-col>
      </a-row>
    </template>
  </a-table>
</template>
<script>
export default {
  props: {
    scroll: {
      type: Object,
      require: false,
    },
    // 数据源
    dataSource: {
      type: Array,
      require: true,
    },
    // 绑定列
    columns: {
      type: Array,
      require: true,
    },
    // 行唯一键
    rowKey: {
      type: String,
      require: false,
      default: 'id',
    },
    // 加载动画
    loading: {
      type: Boolean,
      require: true,
      default: false,
    },
    // 是否分页
    hasPage: {
      type: Boolean,
      require: false,
      default: true,
    },
  },
  data() {
    return {
      createActions: ['menu_create', 'role_create', 'dcUser_add_childer', 'product_create', 'productProxy_create'],
      deleteActions: [
        'registerDev_delete',
        'apiLog_delete',
        'errorLog_delete',
        'actionLog_delete',
        'menu_delete',
        'role_delete',
        'dcUser_delete_childer',
        'product_delete',
        'productProxy_delete',
      ],
      editActions: ['menu_edit', 'role_edit', 'dcUser_edit_childer', 'product_edit', 'productProxy_edit'],
      detailsActions: [
        'apiLog_details',
        'errorLog_details',
        'actionLog_details',
        'menu_details',
        'role_details',
        'dcUser_details_childer',
        'product_details',
        'productProxy_details',
      ],
      importAction: ['role_import', 'product_import', 'productProxy_import'],
      exportAction: ['role_export', 'product_export', 'productDev_export', 'productProxy_export'],
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
        columnWidth: 50,
        // 选中项发生变化时的回调
        onChange: this.onRowSelectChange,
      },
    }
  },
  mounted() {},
  methods: {
    handleUp(item) {
      this.$emit('handleUp', item)
    },
    handleDown(item) {
      this.$emit('handleDown', item)
    },
    // 新增
    handleAdd() {
      this.$emit('handleAdd')
    },
    // 删除
    handleDelete(item) {
      var that = this
      this.$confirm({
        title: '确定要删除吗?',
        onOk() {
          that.$emit('removeSubmit', [item.id], [item])
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
            that.$emit('removeSubmit', that.selectedRowKeys, that.selectedRows)
          },
          onCancel() {},
        })
      } else {
        this.$message.warning('请选择至少一项!')
      }
    },
    // 编辑
    handleEdit(item) {
      this.$emit('handleEdit', item)
    },
    // 导入
    handleImport() {
      this.$emit('handleImport')
    },
    // 导出
    handleExport() {
      this.$emit('handleExport')
    },
    // 详情
    handleDetails(item) {
      this.$emit('handleDetails', item)
    },
    // 刷新
    handleRefresh() {
      // if (this.hasPage) {
      //   this.pagination.current = 1
      // }
      this.$emit('loadData')
    },
    // 分页、排序、筛选变化时触发
    handleTableChange(page, filters, sorter) {
      const pager = { ...this.pagination }
      pager.current = page.current
      pager.pageSize = page.pageSize
      this.pagination = pager
      this.$emit('loadData')
    },
    // 表格多选框
    onRowSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys
      this.selectedRows = selectedRows
    },
  },
}
</script> 