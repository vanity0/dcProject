<template>
  <a-modal centered :width="800" :title="modifyEntity.title" :visible="modifyEntity.showModify" @cancel="handleRefresh">
    <!--保存按钮-->
    <template slot="footer">
      <a-row>
        <a-col :span="8">
          <a-tag color="blue" v-if="!readOnly"> 操作完是否继续？</a-tag>
          <a-switch v-model="hasClose" :loading="loading" v-if="!readOnly">
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
      <a-card title="基础信息">
        <a-form :form="form">
          <a-row :gutter="10">
            <a-col :span="12">
              <a-form-item label="姓名" :label-col="labelCol" :wrapper-col="wrapperCol">
                <a-input
                  placeholder="请输入姓名"
                  :disabled="readOnly"
                  v-decorator="['name', { rules: [{ required: true, message: '请输入姓名' }] }]"
                />
              </a-form-item>
            </a-col>

            <a-col :span="12">
              <a-form-item label="所属角色" :label-col="labelCol" :wrapper-col="wrapperCol">
                <a-select
                  :disabled="readOnly"
                  v-decorator="['roleId', { rules: [{ required: true, message: '请选择所属角色' }] }]"
                  @change="handleRoleChange"
                  placeholder="请选择所属角色"
                  style="width: 100%"
                >
                  <a-select-option v-if="modifyEntity.parentRoleId === '总平台'" key="总平台" value="总平台"
                    >总平台</a-select-option
                  >
                  <a-select-option
                    key="商家"
                    v-if="modifyEntity.parentRoleId === '商家' || modifyEntity.parentRoleId === '总平台'"
                    value="商家"
                    >商家</a-select-option
                  >
                  <a-select-option
                    key="一级推手"
                    v-if="modifyEntity.parentRoleId === '一级推手' || modifyEntity.parentRoleId === '总平台'"
                    value="一级推手"
                    >一级推手</a-select-option
                  >
                  <a-select-option
                    key="二级推手"
                    v-if="modifyEntity.parentRoleId === '一级推手' || modifyEntity.parentRoleId === '总平台'"
                    value="二级推手"
                    >二级推手</a-select-option
                  >
                </a-select>
              </a-form-item>
            </a-col>
          </a-row>

          <a-form :form="form">
            <a-row :gutter="10">
              <a-col :span="12">
                <a-form-item label="账号" :label-col="labelCol" :wrapper-col="wrapperCol">
                  <a-input
                    placeholder="请输入账号（手机号）"
                    :disabled="readOnly"
                    v-decorator="['account', { rules: [{ required: true, message: '请输入账号（手机号）' }] }]"
                  />
                </a-form-item>
              </a-col>
              <a-col :span="12">
                <a-form-item label="密码" :label-col="labelCol" :wrapper-col="wrapperCol">
                  <a-input-password
                    placeholder="请输入密码"
                    :disabled="readOnly || showPwd"
                    v-decorator="['pwd', { rules: [{ required: true, message: '请输入密码' }] }]"
                  />
                </a-form-item>
              </a-col>
            </a-row>

            <a-row>
              <a-col :span="12" v-if="modifyEntity.parentId">
                <a-form-item label="所属上级" :label-col="labelCol" :wrapperCol="wrapperCol">
                  <a-input disabled v-model="modifyEntity.parentName" />
                  <a-input disabled v-decorator="['parentId']" v-show="false" />
                </a-form-item>
              </a-col>

              <a-col :span="12">
                <a-form-item label="状态" :label-col="labelCol" :wrapperCol="wrapperCol">
                  <a-switch
                    :disabled="readOnly"
                    v-decorator="['status', { valuePropName: 'checked', initialValue: true }]"
                    un-checked-children="冻结"
                    checked-children="正常"
                  />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </a-form>
      </a-card>

      <a-card v-if="readOnly" style="margin-top: 8px" title="其他信息">
        <a-form :form="form">
          <a-row>
            <a-col :span="12">
              <a-form-item label="登录总次数" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input :disabled="readOnly" v-decorator="['loginTotalCount']" />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row>
            <a-col :span="12">
              <a-form-item label="上次登录时间" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input :disabled="readOnly" v-decorator="['lastLoginTime']" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item label="上次登录IP" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input :disabled="readOnly" v-decorator="['lastLoginIp']" />
              </a-form-item>
            </a-col>
          </a-row>

          <a-row>
            <a-col :span="12">
              <a-form-item label="创建时间" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input :disabled="readOnly" v-decorator="['createTime']" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item label="创建人" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input :disabled="readOnly" v-decorator="['createUser']" />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row>
            <a-col :span="12">
              <a-form-item label="修改时间" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input :disabled="readOnly" v-decorator="['updateTime']" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item label="修改人" :labelCol="labelCol" :wrapperCol="wrapperCol">
                <a-input :disabled="readOnly" v-decorator="['updateUser']" />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
      </a-card>
    </a-spin>
  </a-modal>
</template>

<script>
import FooterToolBar from '@/components/FooterToolbar'
import { getDCUser, addDCUser, updateDCUser } from '@/api/dcUser'
export default {
  components: {
    FooterToolBar,
  },
  props: {
    modifyEntity: {
      type: Object,
      required: true,
      default: function () {
        return {
          // 父级role
          parentRoleId: '',
          // 父级名称
          parentName: '',
          // 父级id
          parentId: '',
          // 传递的主键
          id: '',
          // 页面标题
          title: '新增平台员工',
          // 页面操作类型
          operation: this.operationFlag.Add,
          // 是否展示编辑页面
          showModify: false,
          // 是否展示子集页面
          showChilder: false,
        }
      },
    },
  },
  data() {
    return {
      labelCol: { xs: { span: 24 }, sm: { span: 6 } },
      wrapperCol: { xs: { span: 24 }, sm: { span: 16 } },
      // 执行保存后是否关闭当前页面
      hasClose: false,
      // 遮罩层加载
      spinning: false,
      // 按钮加载状态
      loading: false,
      // 只读状态
      readOnly: false,
      // 默认显示输入密码
      showPwd: false,
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
      // 当新增的时候，显示密码框
      this.showPwd = this.modifyEntity.operation !== this.operationFlag.Add
      this.readOnly = this.modifyEntity.operation === this.operationFlag.Details ? true : false

      let that = this
      this.$nextTick(() => {
        that.form.resetFields() // 重置参数值
        //修改，详情获取详细信息
        if (that.modifyEntity.operation !== that.operationFlag.Add) {
          that.formFields = that.form.getFieldsValue()
          getDCUser({ id: that.modifyEntity.id }).then((res) => {
            that.entity = res.data
            var setData = {}
            Object.keys(that.formFields).forEach((item) => {
              setData[item] = that.entity[item]
            })
            that.form.setFieldsValue(setData)
          })
        } else {
          // 新增，判断是否有上级
          if (that.modifyEntity.parentId) {
            that.form.setFieldsValue({ parentId: that.modifyEntity.parentId })
          }
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
            addDCUser(values).then((res) => {
              that.submitResult(res)
            })
          } else {
            that.entity = Object.assign(that.entity, that.form.getFieldsValue())
            updateDCUser(that.entity).then((res) => {
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

    //#region 下拉设置值
    handleRoleChange(val) {
      if (val) {
        this.form.setFieldsValue({ roleId: val })
      }
    },
    //#endregion
  },
}
</script>
<style scoped>
.avatar-uploader > .ant-upload {
  width: 128px;
  height: 128px;
}
.avatar {
  width: 128px;
  height: 128px;
}
.ant-upload-select-picture-card i {
  font-size: 32px;
  color: #999;
}

.ant-upload-select-picture-card .ant-upload-text {
  margin-top: 8px;
  color: #666;
}
</style>
