<template>
  <div>
    <!--查询参数-->
    <params ref="params" @handleRefresh="handleRefresh" :loading="loading"></params>
    <!--表格页面-->
    <a-table
      bordered
      size="small"
      :columns="columns"
      :row-key="(row) => row.name"
      :data-source="dataSource"
      :pagination="pagination"
      :loading="loading"
      @change="handleTableChange"
    >
      <template slot="logo" slot-scope="text">
        <img width="50" style="cursor: pointer" height="50" :src="text" />
      </template>
      <template slot="title">
        <a-row>
          <a-col>
            <a-popover placement="top">
              <template slot="content">刷新</template>
              <a-button class="element-right" @click="handleRefresh" :loading="loading" icon="sync"></a-button>
            </a-popover>
            <a-space>
              <a-button
                type="primary"
                icon="plus-circle"
                v-clipboard:copy="inputAllUrl"
                v-clipboard:success="handleSuccess"
                v-clipboard:error="handleError"
                v-action:productDev_bathCopyUrl
                >一键复制</a-button
              >
            </a-space>
          </a-col>
        </a-row>
      </template>
      <!-- <template v-slot:buttons="{ item }" style="cursor: pointer">
        <a
          v-clipboard:copy="localhostPath + item.ecode"
          v-clipboard:success="handleSuccess"
          v-clipboard:error="handleError"
          v-action:productDev_copyUrl
          ><a-icon type="share-alt" />复制</a
        >
        <a style="margin-left: 5px" @click="handleRegisterView(item)" v-action:productDev_register
          ><a-icon type="solution" />推广数据</a
        >
      </template> -->
    </a-table>
    <!-- <a-input v-model="inputAllUrl" disabled /> -->
    <!-- <a-modal centered title="点击复制进行推广" :visible="visible" @cancel="handleCancel">
      <a-spin :spinning="spinning">
        <a-input v-model="inputUrl" disabled />
      </a-spin>
      <template slot="footer">
        <a-button
          v-clipboard:copy="inputUrl"
          :loading="spinning"
          v-clipboard:success="handleSuccess"
          v-clipboard:error="onError"
        >
          复制
        </a-button>
      </template>
    </a-modal> -->

    <register v-if="showRegister" :visible="showRegister" :productId="productId" :goBack="handleGoBack"></register>
  </div>
</template>
<script>
import modify from './modify'
import params from './components/params'
import register from './components/register'
// import { getAllDwzUrl } from '@/api/dwz'
import { productDevColumns, productDev2Columns, queryProductDev } from '@/api/product'
export default {
  name: 'productDev',
  components: { params, modify, register },
  data() {
    return {
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
      // 加载动画
      loading: false,
      // 绑定的数据源
      dataSource: [],
      // 表格绑定的列
      columns: productDevColumns,
      // 用户ID
      id: '',
      // 产品ID
      productId: '',
      // 是否显示推广数据
      showRegister: false,
      visible: false,
      spinning: false,
      inputAllUrl: '',
      localhostPath: '',
    }
  },
  mounted() {
    this.columns = this.$store.getters.roles[0].role === '一级推手' ? productDevColumns : productDev2Columns
    this.loadAllUrl()
    this.loadData()
  },
  methods: {
    // 获取所有
    loadAllUrl() {
      // 获取当前页面地址，如http://localhost:8080/admin/index
      let wPath = window.document.location.href
      // 获取当前页面主机地址之后的目录，如：/admin/index
      let pathName = this.$route.path
      let pos = wPath.indexOf(pathName)
      // 获取主机地址，如：http://localhost:8080
      this.localhostPath = wPath.substring(0, pos) + '/d?'

      // let that = this
      // getAllDwzUrl()
      //   .then((res) => {
      //     if (res.success) {
      //       var data = res.data
      //       for (let i; i < data.length; i++) {
      //         that.inputAllUrl += data[i].prodName + ': ' + that.localhostPath + data[i].url
      //       }
      //     } else {
      //       that.$message.warning('加载失败1!', res.msg)
      //     }
      //   })
      //   .catch((error) => {
      //     that.$message.error('加载失败2!', error.message)
      //   })
    },
    // var that = this
    // this.visible = true
    // this.spinning = true
    // getDwzUrl({ id: item.id })
    //   .then((res) => {
    //     if (res.success) {
    //       // 获取当前页面地址，如http://localhost:8080/admin/index
    //       let wPath = window.document.location.href
    //       // 获取当前页面主机地址之后的目录，如：/admin/index
    //       let pathName = this.$route.path
    //       let pos = wPath.indexOf(pathName)
    //       // 获取主机地址，如：http://localhost:8080
    //       let localhostPath = wPath.substring(0, pos) + '/d?'
    //       that.inputUrl = localhostPath + res.data
    //     } else {
    //       that.$message.error('复制失败', res.msg)
    //       this.visible = false
    //     }
    //     this.spinning = false
    //   })
    //   .catch(() => {
    //     that.$message.error('复制失败')
    //     this.spinning = false
    //     this.visible = false
    //   })
    // },
    // handleCancel() {
    //   this.visible = false
    // },
    handleSuccess() {
      this.$message.success('复制成功')
      // that.$copyText(this.inputUrl).then(
      //   function (e) {
      //   },
      //   function (e) {
      //   }
      // )
    },
    handleError() {
      this.$message.error('复制失败')
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
    // 加载数据
    loadData() {
      this.loading = true
      let queryParams = {
        ...this.$refs.params.params,
        current: this.pagination.current,
        pageSize: this.pagination.pageSize,
      }
      let that = this
      queryProductDev(queryParams)
        .then((res) => {
          if (res.data.dwzList) {
            let dwzs = res.data.dwzList
            for (let i = 0; i < dwzs.length; i++) {
              if (dwzs[i].aliasName) {
                that.inputAllUrl +=
                  dwzs[i].prodName + '(' + dwzs[i].aliasName + ')' + ': ' + that.localhostPath + dwzs[i].url + '\r\n'
              } else {
                that.inputAllUrl += dwzs[i].prodName + ': ' + that.localhostPath + dwzs[i].url + '\r\n'
              }
            }
          }

          let { data, totalCount } = res.data.porductList
          that.dataSource = data
          that.pagination.total = totalCount
          that.loading = false
        })
        .catch(() => {
          that.loading = false
        })
    },
    //#endregion
  },
}
</script>

