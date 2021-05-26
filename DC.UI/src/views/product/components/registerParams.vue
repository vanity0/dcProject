<template>
  <a-collapse activeKey="1" expand-icon-position="right">
    <a-collapse-panel key="1" header="筛选条件">
      <a-form-model layout="inline" :model="params" ref="paramsForm">
        <a-form-model-item label="姓名或电话">
          <a-input
            v-keyupEnter="{ callback: handleRefresh }"
            v-model.trim="params.name"
            placeholder="请输入姓名或电话"
          />
        </a-form-model-item>
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
export default {
  props: {
    loading: {
      type: Boolean,
    },
  },
  data() {
    return {
      // 要查询的参数
      params: {
        // 名称
        name: '',
        productId:'',
      },
    }
  },
  mounted() {},
  methods: {
    //#region 查询数据
    handleRefresh() {
      this.$emit('handleRefresh')
    },
    //#endregion

    //#region 重置
    handleReset() {
      this.params.name = ''
      this.$refs.paramsForm.resetFields()
    },
    //#endregion
  },
}
</script>
