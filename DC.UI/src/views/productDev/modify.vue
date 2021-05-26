<template>
  <a-spin :spinning="spinning">
    <a-card title="基础信息">
      <a-form :form="form">
        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="产品名称" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input
                placeholder="请输入产品名称"
                :disabled="readOnly"
                v-decorator="['name', { rules: [{ required: true, message: '请输入产品名称' }] }]"
              />
            </a-form-item>
          </a-col>

          <a-col :span="12">
            <a-form-item label="产品分类" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-select
                :disabled="readOnly"
                v-decorator="['productType', { rules: [{ required: true, message: '请选择产品分类' }] }]"
                @change="handleProductTypeChange"
                placeholder="请选择产品分类"
                style="width: 100%"
              >
                <a-select-option v-for="item in productTypes" :key="item" :value="item">{{ item }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
        </a-row>

        <!--系列名称,链接地址-->
        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="系列名称" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input placeholder="请输入系列名称" :disabled="readOnly" v-decorator="['aliasName']" />
            </a-form-item>
          </a-col>

          <a-col :span="12">
            <a-form-item label="链接地址" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input
                placeholder="请输入链接地址"
                :disabled="readOnly"
                v-decorator="['url', { rules: [{ required: true, message: '请输入链接地址' }] }]"
              >
              </a-input>
            </a-form-item>
          </a-col>
        </a-row>

        <!--推广模式,上架推手-->
        <a-row>
          <a-col :span="12">
            <a-form-item label="推广模式" :label-col="labelCol" :wrapperCol="wrapperCol">
              <a-select
                :disabled="readOnly"
                v-decorator="['linkModel', { rules: [{ required: true, message: '请选择推广模式' }] }]"
                @change="handleLinkModelChange"
                placeholder="请选择推广模式"
                style="width: 100%"
              >
                <a-select-option v-for="item in linkModels" :key="item" :value="item">{{ item }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>

          <a-col :span="12">
            <a-form-item label="上架推手" :label-col="labelCol" :wrapperCol="wrapperCol">
              <a-select
                disabled
                v-decorator="['auditStatus', { rules: [{ required: true, message: '请选择上架推手' }] }]"
                @change="handleAuditStatusChange"
                placeholder="请选择上架推手"
                style="width: 100%"
              >
                <a-select-option v-for="item in auditStatus" :key="item" :value="item">{{ item }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
        </a-row>

        <!--接入价格,推广价格-->
        <a-row>
          <a-col :span="12">
            <a-form-item label="推广价格" :label-col="labelCol" :wrapperCol="wrapperCol">
              <a-input-number
                style="width: 100%"
                :min="extendMoneyMin"
                placeholder="请输入推广价格"
                :disabled="readOnly"
                v-decorator="['extendMoney']"
              />
            </a-form-item>
          </a-col>

        </a-row>

        <!--推广链接,排序-->
        <a-row>
          <a-col :span="12">
            <a-form-item label="推广链接" :label-col="labelCol" :wrapperCol="wrapperCol">
              <a-input placeholder="请输入推广链接" disabled v-decorator="['extendUrl']" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="排序" style="width: 100%" :label-col="labelCol" :wrapperCol="wrapperCol">
              <a-input-number
                style="width: 100%"
                :min="1"
                placeholder="请输入排序"
                :disabled="readOnly"
                v-decorator="['sort']"
              />
            </a-form-item>
          </a-col>
        </a-row>

        <!--上架H5,所属标签-->
        <a-row>
          <a-col :span="12">
            <a-form-item label="上架H5" :label-col="labelCol" :wrapperCol="wrapperCol">
              <a-switch
                :disabled="readOnly"
                v-decorator="['h5', { valuePropName: 'checked', initialValue: true }]"
                un-checked-children="否"
                checked-children="是"
              />
            </a-form-item>
          </a-col>

          <a-col :span="12">
            <a-form-item label="所属标签" :label-col="labelCol" :wrapperCol="wrapperCol">
              <a-radio-group :disabled="readOnly" v-decorator="['tag']">
                <a-radio v-for="item in productTags" :key="item" :value="item">{{ item }}</a-radio>
              </a-radio-group>
            </a-form-item>
          </a-col>
        </a-row>

        <!--Logo,描述-->
        <a-row>
          <a-col :span="12">
            <a-form-item label="Logo" :label-col="labelCol" :wrapperCol="wrapperCol">
              <div class="clearfix">
                <a-upload
                  list-type="picture-card"
                  :file-list="fileList"
                  @preview="handlePreview"
                  @change="handleChange"
                  :before-upload="beforeUpload"
                >
                  <div v-if="fileList.length < 1">
                    <a-icon type="plus" />
                    <div class="ant-upload-text">上传LOGO</div>
                  </div>
                </a-upload>
                <a-modal :visible="previewVisible" :footer="null" @cancel="handleCancel">
                  <img alt="" style="width: 100%" :src="previewImage" />
                </a-modal>
              </div>
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item
              label="描述"
              :label-col="{ xs: { span: 24 }, sm: { span: 6 } }"
              :wrapperCol="{ xs: { span: 24 }, sm: { span: 16 } }"
            >
              <a-textarea placeholder="请输入描述" :disabled="readOnly" v-decorator="['descption']" :rows="4" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-card>
 
    <a-card v-if="readOnly" class="element-top" title="其他信息">
      <a-form>
        <a-row>
          <a-col :span="12">
            <a-form-item label="创建人" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input :disabled="readOnly" v-model="entity.createUser" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="创建时间" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input :disabled="readOnly" v-model="entity.createTime" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row>
          <a-col :span="12">
            <a-form-item label="修改人" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input :disabled="readOnly" v-model="entity.updateUser" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="修改时间" :labelCol="labelCol" :wrapperCol="wrapperCol">
              <a-input :disabled="readOnly" v-model="entity.updateTime" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
    </a-card>

    <footer-tool-bar>
      <a-button icon="rollback" @click="handleRefresh">返回</a-button>
      <a-divider type="vertical" v-if="!readOnly" />
      <a-button icon="reload" v-if="!readOnly" @click="handleReset" :loading="loading">重置</a-button>
      <a-divider type="vertical" v-if="!readOnly" />
      <a-button icon="check-circle" v-if="!readOnly" :loading="loading" @click="handleSubmit">保存</a-button>
    </footer-tool-bar>
  </a-spin>
</template>
 0
<script>
import { getProduct, addProduct, updateProduct } from '@/api/product'
import { getBase64 } from '@/utils/convertUtils/convert'
import FooterToolBar from '@/components/FooterToolbar'
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
          // 传递的主键
          id: '',
          // 页面标题
          title: '新增产品信息',
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
      // 图片上传
      previewVisible: false,
      previewImage: '',
      fileList: [],
      // 推广最小价格
      extendMoneyMin: 0,
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    //#region 价格设置
    // 联盟价格
    leagueMoneyChange(val) {
      this.extendMoneyMin = Number(val)
      let money = this.form.getFieldValue('extendMoney') || 0
      if (Number(money) > this.extendMoneyMin || Number(money) === 0) {
        this.form.setFieldsValue({ extendMoney: this.extendMoneyMin })
      }
    },
    //#endregion

    //#region 上传图片
    handleCancel() {
      this.previewVisible = false
    },
    async handlePreview(file) {
      if (!file.url && !file.preview) {
        file.preview = await getBase64(file.originFileObj)
      }
      this.previewImage = file.url || file.preview
      this.previewVisible = true
    },
    handleChange(info) {
      if (this.fileList.length > 0) {
        let fileList = [...info.fileList]
        // 1. 限制上传文件数量
        //只显示最近上传的1个文件，旧文件将被新文件取代
        fileList = fileList.slice(-1)
        // 2. 从响应读取并显示文件链接
        fileList = fileList.map((file) => {
          if (file.response) {
            // 组件将显示文件。url链接
            file.url = file.response.url
          }
          return file
        })
        this.fileList = fileList
      }
    },
    // 上传前对文件限制
    beforeUpload(file) {
      const isJpgOrPng =
        file.type === 'image/jpeg' ||
        file.type === 'image/jpg' ||
        file.type === 'image/png' ||
        file.type === 'image/bmp'
      if (!isJpgOrPng) {
        this.$message.error('图片格式有误!')
      }
      const isLt2M = file.size / 1024 / 1024 < 2
      if (!isLt2M) {
        this.$message.error('图片大小最大为2MB!')
      }
      if (isLt2M && isJpgOrPng) {
        this.fileList = [...this.fileList, file]
      }
      return false
    },

    //#endregion

    //#region 加载数据
    loadData() {
      // 当新增的时候，显示密码框
      this.showPwd = this.modifyEntity.operation !== this.operationFlag.Add
      this.readOnly = this.modifyEntity.operation === this.operationFlag.Details ? true : false
      this.form.resetFields() // 重置参数值

      //修改，详情获取详细信息
      if (this.modifyEntity.operation !== this.operationFlag.Add) {
        this.formFields = this.form.getFieldsValue()
        let that = this
        getProduct({ id: this.modifyEntity.id }).then((res) => {
          that.entity = res.data
          var setData = {}
          Object.keys(that.formFields).forEach((item) => {
            setData[item] = that.entity[item]
          })
          that.previewVisible = false
          if (that.entity.logo) {
            that.fileList = [
              {
                uid: '-1',
                name: 'image.png',
                status: 'done',
                url: that.entity.logo,
              },
            ]
          }
          that.form.setFieldsValue(setData)
        })
      } else {
        // 新增，判断是否有上级
        if (this.modifyEntity.parentId) {
          this.form.setFieldsValue({ parentId: this.modifyEntity.parentId })
        }
        this.handleAuditStatusChange('待申请')
      }
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
            if (that.fileList.length > 0) {
              values.logo = that.fileList[0].thumbUrl
            }
            addProduct(values).then((res) => {
              that.submitResult(res)
            })
          } else {
            that.entity = Object.assign(that.entity, that.form.getFieldsValue())
            if (that.fileList.length > 0) {
              that.entity.logo = that.fileList[0].thumbUrl
            } else {
              that.entity.logo = ''
            }
            updateProduct(that.entity).then((res) => {
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
    handleAuditStatusChange(val) {
      if (val) {
        this.form.setFieldsValue({ auditStatus: val })
      }
    },
    handleProductTypeChange(val) {
      if (val) {
        this.form.setFieldsValue({ productType: val })
      }
    },
    handleLinkModelChange(val) {
      if (val) {
        this.form.setFieldsValue({ LINKmODEL: val })
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
