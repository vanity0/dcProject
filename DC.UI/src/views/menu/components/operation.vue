<template>
  <!--表格-->
  <a-table
    bordered
    size="small"
    :columns="columns"
    :row-key="(row) => row.id"
    :data-source="operations"
    :pagination="false"
  >
    <!-- 表格编辑的时候显示-->
    <template slot="name" slot-scope="text, item">
      <a-input :maxLength="50" v-if="item.isEdit" v-model.trim="item.name" />
      <template v-else>{{ item.name }}</template>
    </template>

    <template slot="code" slot-scope="text, item">
      <a-input :maxLength="50" v-if="item.isEdit" v-model.trim="item.code" />
      <template v-else>{{ item.code }}</template>
    </template>

    <template slot="title">
      <a-button type="primary" :disabled="disabled" icon="plus-circle" @click="handleAdd"></a-button>
    </template>
    <template slot="action" slot-scope="text, item">
      <a-button type="primary" :disabled="disabled" icon="minus-circle" @click="handleRemove(item)"></a-button>
    </template>
  </a-table>
</template>
<script>
import { menuOperationColumns } from '@/api/menuOperation'
export default {
  name: 'Operation',
  props: {
    operations: {
      type: Array,
      require: false,
    },
    operation: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      disabled: false,
      columns: menuOperationColumns,
    }
  },
  mounted() {
    if (this.operation == this.operationFlag.Details) {
      this.disabled = true
    } else {
      this.disabled = false
    } 
  },
  methods: {
    //#region 新增
    // 添加新行
    handleAdd() {
      this.$emit('add')
    },
    //#endregion

    //#region
    // 删除新增行
    handleRemove(item) {
      this.$emit('remove', item)
    },
    //#endregion
  },
}
</script>