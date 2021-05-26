<template>
  <a-modal centered :width="640" :title="modifyEntity.title" :visible="modifyEntity.showModify" @cancel="handleRefresh">
    <!--保存按钮-->
    <template slot="footer">
      <a-row>
        <a-col :span="8">
          <a-tag color="blue"> 操作完是否继续？</a-tag>
          <a-switch v-model="hasClose" :loading="loading">
            <a-icon slot="checkedChildren" type="check" />
            <a-icon slot="unCheckedChildren" type="close" />
          </a-switch>
        </a-col>
        <a-col :span="8" :offset="8">
          <a-button @click="handleRefresh" v-if="readOnly">取消</a-button>
          <a-button icon="reload" v-if="!readOnly" @click="handleReset" :loading="loading">重置</a-button>
          <a-divider type="vertical" v-if="!readOnly" />
          <a-button icon="check-circle" v-if="!readOnly" :loading="loading" @click="handleSubmit">保存</a-button>
        </a-col>
      </a-row>
    </template>

    <a-spin :spinning="spinning">
      <a-form :form="form">
        <a-form-item label="姓名" :labelCol="{ span: 4 }" :wrapperCol="{ span: 18 }">
          <a-input
            :disabled="readOnly"
            v-decorator="['name', { rules: [{ required: true, message: '请输入名称' }] }]"
          />
        </a-form-item>
        <a-form-item label="电话" :labelCol="{ span: 4 }" :wrapperCol="{ span: 18 }">
          <a-input :disabled="readOnly" v-decorator="['tel', { rules: [{ required: true, message: '请输入电话' }] }]" />
        </a-form-item>

        <a-row v-if="readOnly">
          <a-col :span="12">
            <a-form-item label="状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input disabled v-model="entity.status" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="所属推手" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input disabled v-model="entity.userName" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row v-if="readOnly">
          <a-col :span="12">
            <a-form-item label="创建人" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input disabled v-model="entity.createUser" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="创建时间" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input disabled v-model="entity.createTime" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row v-if="readOnly">
          <a-col :span="12">
            <a-form-item label="修改人" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input disabled v-model="entity.updateUser" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="修改时间" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input disabled v-model="entity.updateTime" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { getRegister, updateRegister } from '@/api/register'
export default {
  props: {
    modifyEntity: {
      type: Object,
      required: true,
      default: function () {
        return {
          productId: '',
          // 传递的主键
          id: '',
          // 页面标题
          title: '推广数据详情',
          // 页面操作类型
          operation: this.operationFlag.Add,
          // 是否展示编辑页面
          showModify: false,
        }
      },
    },
  },
  data() {
    return {
      labelCol: { xs: { span: 24 }, sm: { span: 8 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 12 } },
      // 执行保存后是否关闭当前页面
      hasClose: false,
      // 遮罩层加载
      spinning: false,
      // 按钮加载状态
      loading: false,
      // 只读状态
      readOnly: false,
      form: this.$form.createForm(this),
      formFields: {},
      entity: {},
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    //#region 加载数据

    loadData() {
      let that = this
      this.readOnly = this.modifyEntity.operation === this.operationFlag.Details ? true : false
      this.$nextTick(() => {
        that.form.resetFields() // 重置参数值
        //修改，详情获取详细信息
        if (that.modifyEntity.operation !== that.operationFlag.Add) {
          that.formFields = that.form.getFieldsValue()
          getRegister({ id: that.modifyEntity.id }).then((res) => {
            that.entity = res.data
            var setData = {}
            Object.keys(that.formFields).forEach((item) => {
              setData[item] = that.entity[item]
            })
            that.form.setFieldsValue(setData)
          })
        }
      })
    },
    //#endregion

    //#region 保存
    // 保存
    handleSubmit() {
      let that = this
      this.spinning = true
      this.form.validateFields((errors, values) => {
        if (!errors) {
          if (that.modifyEntity.operation === that.operationFlag.Add) {
            // values.productId = that.modifyEntity.productId
            // addRegister(values).then((res) => {
            //   that.submitResult(res)
            // })
          } else {
            that.entity = Object.assign(that.entity, that.form.getFieldsValue())
            that.entity.productId = that.modifyEntity.productId
            updateRegister(that.entity).then((res) => {
              that.submitResult(res)
            })
          }
        } else {
          this.spinning = false
        }
      })
    },
    // 保存结果
    submitResult(res) {
      if (res.success) {
        this.$notification.success({ description: res.msg })
        if (!this.hasClose) {
          this.$emit('handleRefresh')
        }
      } else {
        this.$notification.error({ description: res.msg })
      }
      this.spinning = false
    },
    //#endregion

    //#region  取消、关闭、重置
    // 取消、关闭
    handleRefresh() {
      this.spinning = false
      this.$emit('handleRefresh')
    },
    // 重置
    handleReset() {
      this.loadData()
    },
    //#endregion
  },
}
</script>
