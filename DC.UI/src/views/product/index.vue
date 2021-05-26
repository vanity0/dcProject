<template>
  <div>
    <!--查询参数-->
    <params ref="params" v-show="!modifyEntity.showModify" @handleRefresh="handleRefresh" :loading="loading"></params>
    <!--表格页面-->
    <my-table
      ref="myTable"
      v-show="!modifyEntity.showModify"
      :dataSource="dataSource"
      :columns="columns"
      :loading="loading"
      :scroll="{ x: 1300 }"
      @handleAdd="handleAdd"
      @removeSubmit="removeSubmit"
      @handleEdit="handleEdit"
      @handleImport="handleImport"
      @handleExport="handleExport"
      @handleDetails="handleDetails"
      @loadData="loadData"
      @handleUp="handleUp"
      @handleDown="handleDown"
    >
      <template v-slot:buttons="{ item }" v-if="$route.meta.showMore">
        <a-dropdown>
          <a-menu slot="overlay">
            <a-menu-item v-if="item.h5">
              <a @click="handleSetProductH5(item, false)" v-action:product_H5Up><a-icon type="fall" />下架H5</a>
            </a-menu-item>
            <a-menu-item v-if="!item.h5">
              <a @click="handleSetProductH5(item, true)" v-action:product_H5Down><a-icon type="rise" />上架H5</a>
            </a-menu-item>

            <a-menu-item v-if="item.auditStatus === '待申请' || item.auditStatus === '拒绝通过'">
              <a @click="handleSetProductStatus(item, '审核中')" v-action:product_Audit
                ><a-icon type="audit" />申请审核</a
              >
            </a-menu-item>
            <a-menu-item v-if="item.auditStatus === '审核中'">
              <a @click="handleSetProductStatus(item, '审核通过')" v-action:product_AuditSuccess style="color: green"
                ><a-icon type="smile" />审核通过</a
              >
            </a-menu-item>
            <a-menu-item v-if="item.auditStatus === '审核中'">
              <a @click="handleSetProductStatus(item, '拒绝通过')" v-action:product_AuditFail style="color: red"
                ><a-icon type="frown" />拒绝通过</a
              >
            </a-menu-item>
            <a-menu-item v-if="item.auditStatus === '下架中' || item.auditStatus === '审核通过'">
              <a @click="handleSetProductStatus(item, '上架中')" v-action:product_Up><a-icon type="rise" />上架推手</a>
            </a-menu-item>
            <a-menu-item v-if="item.auditStatus === '上架中'">
              <a @click="handleSetProductStatus(item, '下架中')" v-action:product_Down
                ><a-icon type="fall" />下架推手</a
              >
            </a-menu-item>
            <a-menu-item v-if="item.auditStatus === '上架中' || item.auditStatus === '下架中'">
              <a @click="handleCopyUrl(item)" v-action:product_CopyUrl><a-icon type="share-alt" />复制链接</a>
            </a-menu-item>
            <a-menu-item v-if="item.auditStatus === '上架中' || item.auditStatus === '下架中'">
              <a @click="handleRegisterView(item)" v-action:product_RegisterData><a-icon type="solution" />推广数据</a>
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
    <modify v-if="modifyEntity.showModify" @handleRefresh="handleRefresh" :modifyEntity="modifyEntity" />
    <!--推广数据-->
    <register v-if="showRegister" :visible="showRegister" :productId="productId" :goBack="handleGoBack"></register>
  </div>
</template>
<script>
import modify from './modify'
import myTable from '@/views/components/MyTable/index'
import params from './components/params'
import register from './components/register'
import { productColumns, queryProduct, deleteProduct, setProductStatus, setProducth5, sortProduct } from '@/api/product'

export default {
  name: 'product',
  components: { params, modify, myTable, register },
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
      columns: productColumns,
      // 用户ID
      id: '',
      // 产品ID
      productId: '',
      // 是否显示推广数据
      showRegister: false,
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    //#region 上移，下移
    handleUp(item) {
      let that = this
      this.loading = true
      sortProduct({ id: item.id, hasUp: true })
        .then((res) => {
          if (res.success) {
            that.$notification.success({ description: res.msg })
            that.handleRefresh()
          } else {
            that.$notification.error({ description: res.msg })
          }
          that.loading = false
        })
        .catch((error) => {
          that.loading = false
        })
    },
    handleDown(item) {
      let that = this
      this.loading = true
      sortProduct({ id: item.id, hasUp: false })
        .then((res) => {
          if (res.success) {
            that.$notification.success({ description: res.msg })
            that.handleRefresh()
          } else {
            that.$notification.error({ description: res.msg })
          }
          that.loading = false
        })
        .catch((error) => {
          that.loading = false
        })
    },
    //#endregion
    //#region 上下架h5
    handleSetProductH5(item, h5) {
      let that = this
      this.$confirm({
        title: '确定执行该操作吗?',
        onOk() {
          setProducth5({
            id: item.id,
            h5: h5,
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

    //#region "待申请", "审核中","审核通过","拒绝通过", "上架中", "下架中"
    handleSetProductStatus(item, auditStatus) {
      let that = this
      this.$confirm({
        title: '确定执行该操作吗?',
        onOk() {
          setProductStatus({
            id: item.id,
            auditStatus: auditStatus,
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

    //#region 复制链接
    handleCopyUrl(item) {
      var that = this
      this.$copyText(item.url).then(
        function (e) {
          that.$message.success('复制成功')
        },
        function (e) {
          that.$message.error('复制失败')
        }
      )
    },
    //#endregion

    //#region 推广数据
    handleRegisterView(item) {
      this.productId = item.id
      this.showRegister = true
    },
    handleGoBack() {
      this.productId = ''
      this.showRegister = false
    },
    //#endregion

    //#region 查询列表
    // 保存/修改后刷新表格
    handleRefresh() {
      this.modifyEntity.showModify = false 
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
      queryProduct(queryParams)
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

    //#region 添加
    // 打开添加页面
    handleAdd() {
      this.modifyEntity = {
        parentName: '',
        parentId: '',
        title: '新增产品信息',
        showModify: true,
        operation: this.operationFlag.Add,
      }
    },
    //#endregion

    //#region 删除
    // 删除
    removeSubmit(ids, selectRows) {
      let d = selectRows.filter((m) => m.auditStatus === '上架中' || m.h5)
      if (d.length > 0) {
        this.$notification.warning({ description: '要删除的产品处于上架中,不可删除!' })
      } else {
        deleteProduct(ids).then((res) => {
          if (res.success) {
            this.$notification.success({ description: res.msg })
            this.handleRefresh()
          } else {
            this.$notification.error({ description: res.msg })
          }
        })
      }
    },
    //#endregion

    //#region 修改
    // 打开编辑页面
    handleEdit(item) {
      this.modifyEntity = {
        parentName: item.parentName,
        parentId: item.parentId,
        id: item.id,
        title: '修改产品信息',
        showModify: true,
        operation: this.operationFlag.Edit,
      }
    },
    //#endregion

    //#region 详情
    // 打开详情页面
    handleDetails(item) {
      this.modifyEntity = {
        parentName: item.parentName,
        parentId: item.parentId,
        id: item.id,
        title: '产品信息详情',
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


