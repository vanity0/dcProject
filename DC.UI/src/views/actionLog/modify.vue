<template>
  <a-modal centered :width="720" :title="modifyEntity.title" :visible="modifyEntity.showModify" @cancel="handleRefresh">
    <!--保存按钮-->
    <template slot="footer">
      <a-row>
        <a-col>
          <a-button @click="handleRefresh" v-if="readOnly">取消</a-button>
          <a-button icon="reload" v-if="!readOnly" @click="handleReset" :loading="loading">重置</a-button>
          <a-divider type="vertical" v-if="!readOnly" />
          <a-button icon="check-circle" v-if="!readOnly" :loading="loading" @click="handleSubmit">保存</a-button>
        </a-col>
      </a-row>
    </template>

    <a-spin :spinning="spinning">
      <a-form class="ant-advanced-search-form" :form="form">
        <a-row>
          <a-col :span="24">
            <a-form-item
              label="请求地址"
              :label-col="{ xs: { span: 24 }, sm: { span: 4 } }"
              :wrapper-col="{ xs: { span: 24 }, sm: { span: 20 } }"
            >
              <a-input disabled v-decorator="['requestUrl']" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row>
          <a-col :span="24">
            <a-form-item
              label="用户代理"
              :label-col="{ xs: { span: 24 }, sm: { span: 4 } }"
              :wrapper-col="{ xs: { span: 24 }, sm: { span: 20 } }"
            >
              <a-textarea auto-size disabled v-decorator="['userAgent']" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row>
          <a-col :span="12">
            <a-form-item label="请求方式" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input disabled v-decorator="['method']" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="IP地址" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input disabled v-decorator="['remoteIp']" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row>
          <a-col :span="12">
            <a-form-item label="类名" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input disabled v-decorator="['className']" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="方法名" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input disabled v-decorator="['methodName']" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-item label="请求时间" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input disabled v-decorator="['requestTime']" />
            </a-form-item>
          </a-col> 
        </a-row>

        <a-row>
          <a-col :span="24">
            <a-form-item
              label="请求参数"
              :label-col="{ xs: { span: 24 }, sm: { span: 4 } }"
              :wrapper-col="{ xs: { span: 24 }, sm: { span: 20 } }"
            >
              <a-textarea auto-size disabled v-decorator="['requestParams']" />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row>
          <a-col :span="12">
            <a-form-item label="创建人" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input disabled v-decorator="['createUser']" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="创建时间" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input disabled v-decorator="['createTime']" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-spin>
  </a-modal>
</template>

<script>
import { getActionLog } from '@/api/actionLog'
export default {
  props: {
    modifyEntity: {
      type: Object,
      required: true,
      default: function () {
        return {
          // 传递的主键
          id: '',
          // 页面标题
          title: '操作日志详情',
          // 页面操作类型
          operation: this.operationFlag.Details,
          // 是否展示编辑页面
          showModify: false,
        }
      },
    },
  },
  data() {
    return {
      labelCol: { xs: { span: 24 }, sm: { span: 8 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 16 } },
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
      this.readOnly = this.modifyEntity.operation === this.operationFlag.Details ? true : false
      this.$nextTick(() => {
        this.form.resetFields() // 重置参数值
        //修改，详情获取详细信息
        if (this.modifyEntity.operation !== this.operationFlag.Add) {
          this.formFields = this.form.getFieldsValue()
          let that= this
          getActionLog({ id: this.modifyEntity.id }).then((res) => {
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
