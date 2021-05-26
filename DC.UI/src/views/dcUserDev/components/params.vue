<template>
  <a-collapse activeKey="1" expand-icon-position="right">
    <a-collapse-panel key="1" header="筛选条件">
      <a-form-model layout="inline" :model="params" ref="paramsForm">
        <a-form-model-item label="名称">
          <a-input v-keyupEnter="{ callback: handleRefresh }" v-model.trim="params.name" placeholder="请输入名称" />
        </a-form-model-item>
        <!-- <a-form-model-item label="所属角色">
          <a-tree-select
            style="width: 100%"
            v-model="params.roleId"
            placeholder="请选择所属角色"
            :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
            :tree-data="roles"
            tree-default-expand-all
          >
          </a-tree-select>
        </a-form-model-item> -->
        <a-form-model-item>
          <a-button icon="search" type="primary" :loading="loading" @click="handleRefresh">查询</a-button>
          <a-divider type="vertical" />
          <a-button icon="reload" @click="handleReset">重置</a-button>
        </a-form-model-item>
      </a-form-model>
    </a-collapse-panel>
  </a-collapse>
</template>
<script>
// import { mapGetters } from 'vuex'
export default {
  props: {
    loading: {
      type: Boolean,
    },
  },
  data() {
    return {
      // 所属角色下拉列表
      // roles: [],
      // 要查询的参数
      params: {
        // 名称
        name: '',
        // 所属角色
        roleId: '',
        parentId: '',
      },
    }
  },
  mounted() {
    // this.loadData()
  },
  methods: {
    // ...mapGetters(['roles']),
    //#region 查询数据
    // loadData() {
    //   queryRoleSelectTree({ parentId: this.roles[0] }).then((res) => {
    //     this.roles = res.data
    //   })
    // },
    handleRefresh() {
      this.$emit('handleRefresh')
    },
    //#endregion

    //#region 重置
    handleReset() {
      this.params.name = ''
      this.params.roleId = ''
      this.params.roleId = '' 
      this.$refs.paramsForm.resetFields()
    },
    //#endregion
  },
}
</script>
