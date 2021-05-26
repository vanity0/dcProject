<template>
  <a-modal centered width="80%" title="推广数据" :visible="visible" @cancel="handleCancel">
    <template slot="footer">
      <a-button @click="handleCancel">取消</a-button>
    </template>

    <!--查询参数-->
    <register-params ref="registerParamsRef" @handleRefresh="handleRefresh" :loading="loading"></register-params>
    <!--表格页面-->
    <a-table
      style="margin-top: 10px"
      ref="myTable"
      :columns="columns"
      bordered
      size="small"
      :row-key="(row) => row.id"
      :data-source="dataSource"
      :pagination="pagination"
      :row-selection="rowSelection"
      :loading="loading"
      @change="handleTableChange"
    >
      <span slot="action" slot-scope="text, item">
        <a-space>
          <a v-if="item.status === '待更新'" v-action:productProxy_registAuditSuccess @click="handleSetStatus(item, '审核通过')" style="color: green"
            ><a-icon type="smile" />审核通过</a
          >
          <a v-if="item.status === '待更新'" v-action:productProxy_registAuditFail  @click="handleSetStatus(item, '拒绝通过')" style="color: red"
            ><a-icon type="frown" />拒绝通过</a
          >
        </a-space>
      </span>

      <template slot="title">
        <a-row type="flex" justify="end">
          <a-col :span="24">
            <a-space>
              <a-button type="danger" icon="minus-circle"  @click="handleBatchDelete" v-action:product_registDelete>删除</a-button>
              <a-popover placement="top">
                <template slot="content">刷新</template>
                <a-button @click="handleRefresh" :loading="loading" icon="sync"></a-button>
              </a-popover>
            </a-space>
          </a-col>
        </a-row>
      </template>
    </a-table>
  </a-modal>
</template>

<script>
import registerParams from './registerParams'
import { registerColumns, queryRegister, setRegisterStatus, deleteRegister } from '@/api/register'
export default {
  name: 'register',
  components: { registerParams },
  props: {
    // 产品ID
    productId: {
      type: String,
      required: true,
    },
    // 是否显示
    visible: {
      type: Boolean,
      required: true,
      default: false,
    },
    // 返回
    goBack: {
      type: Function,
      required: true,
    },
  },
  data() {
    return {
      // 加载动画
      loading: false,
      // 绑定的数据源
      dataSource: [],
      // 表格绑定的列
      columns: registerColumns,
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
    //#region 审核通过、拒绝
    handleSetStatus(item, status) {
      let that = this
      this.$confirm({
        title: '确定执行该操作吗?',
        onOk() {
          setRegisterStatus({
            id: item.id,
            status: status,
          })
            .then((res) => {
              if (res.success) {
                that.$notification.success({ description: res.msg })
                that.handleRefresh()
              } else {
                that.$notification.error({ description: res.msg })
              }
              that.loading = false
            })
            .catch(() => {
              that.loading = false
            })
        },
        onCancel() {},
      })
    },
    //#endregion

    //#region  返回
    handleCancel() {
      this.loading = false
      this.goBack()
    },
    //#endregion

    //#region 查询列表
    // 加载数据
    loadData() {
      let that = this
      this.$nextTick(() => {
        that.loading = true
        that.$refs.registerParamsRef.params.productId = that.productId
        let queryParams = {
          ...that.$refs.registerParamsRef.params,
          current: that.pagination.current,
          pageSize: that.pagination.pageSize,
        }
        queryRegister(queryParams)
          .then((res) => {
            let { data, totalCount } = res.data
            that.dataSource = data
            that.pagination.total = totalCount
            that.loading = false
          })
          .catch(() => {
            that.loading = false
          })
      })
    },
    //#endregion

    //#region 表格
    // 刷新
    handleRefresh() {
      this.pagination.current = 1
      this.loadData()
    },
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
    //#endregion

    //#region  删除
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
    //#endregion
  },
}
</script>