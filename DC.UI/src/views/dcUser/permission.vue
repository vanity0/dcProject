<template>
  <a-spin :spinning="confirmLoading">
    <a-table
      bordered
      ref="table"
      size="small"
      :columns="columns"
      :row-key="(row) => row.id"
      :data-source="data"
      :pagination="pagination"
      :loading="loading"
    >
      <template slot="serial" slot-scope="text, item, index">
        <a-checkbox :value="index" :checked="item.checked" @change="onCheckAllChange(item, $event)"></a-checkbox>
      </template>

      <template slot="operations" slot-scope="text, item">
        <template v-if="item.operations">
          <a-checkbox
            v-for="o in item.operations"
            @change="onCkOperationChange(item, o, $event)"
            :key="o.value"
            :value="o.value"
            :checked="o.checked"
            >{{ o.label }}</a-checkbox
          >
        </template>
      </template>
    </a-table>

    <footer-tool-bar>
      <a-button icon="left-circle" @click="handleHidePermission">返回</a-button>
      <a-divider type="vertical" />
      <a-button type="primary" icon="check-circle" @click="handleSubmit">保存</a-button>
    </footer-tool-bar>
  </a-spin>
</template>

<script>
import FooterToolBar from '@/components/FooterToolbar'
import { queryPermission, addPermission } from '@/api/permission'
export default {
  components: {
    FooterToolBar,
  },
  props: {
    roleId: {
      type: String,
    },
  },
  data() {
    return {
      labelCol: { xs: { span: 24 }, sm: { span: 8 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 14 } },
      pagination: false,
      loading: false,
      // 是否加载提示
      confirmLoading: false,
      data: [],
      // 内容勾选
      ckOperationList: [],
      checkAll: false,
      // 表头
      columns: [
        {
          title: '#',
          width: '100px',
          scopedSlots: { customRender: 'serial' },
        },
        {
          width: '160px',
          title: '菜单名称',
          dataIndex: 'name',
        },
        {
          title: '包含操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ],
    }
  },
  created() {},
  mounted() {
    this.initData()
  },
  methods: {
    initData() {
      //加载列表
      queryPermission({ roleId: this.roleId }).then((res) => {
        this.data = res.data
      })
    },
    // 隐藏当前页面
    handleHidePermission() {
      this.$emit('handleHidePermission')
    },
    // 单个Operation操作
    onCkOperationChange(row, item, e) {
      item.checked = e.target.checked
      if (e.target.checked) {
        row.checked = e.target.checked
      }
    },
    // 选择每行所有操作
    onCheckAllChange(item, e) {
      item.checked = e.target.checked
      if (item.operations) {
        item.operations.forEach((element) => {
          element.checked = e.target.checked
        })
      }
      if (item.children) {
        // 所有子项都选中
        item.children.forEach((element) => {
          this.onCheckAllChange(element, e)
        })
      }
      // 所有子项都选中，则父级也选中
      this.$refs.table.dataSource.forEach((row) => {
        // 获取父级
        if (row.id === item.parentId) {
          // 判断父级下面是否全部已选中
          row.checked = this.hasCheck(row.children)
          return false
        }
      })
    },
    hasCheck(childrens) {
      let checked = false
      for (let i = 0; i < childrens.length; i++) {
        // 遍历所有子集
        if (childrens[i].checked) {
          // 如果选中则继续看子集
          if (childrens[i].children) {
            checked = this.hasCheck(childrens[i].children)
          } else {
            return true
          }
        }
      }
      return checked
    },
    handleSubmit() {
      var saveData = Object.assign({
        roleId: this.roleId,
        permissions: this.$refs.table.dataSource,
      })
      addPermission(saveData).then((res) => {
        if (res.success) {
          this.$message.success(res.msg)
        } else {
          this.$message.error(res.msg)
        }
      })
    },
    handleCancel() {
      this.confirmLoading = false
    },
  },
}
</script>
