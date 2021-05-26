<template>
  <a-spin :spinning="spinning">
    <a-form :form="form">
      <a-card title="基础信息">
        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="名称" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input
                :disabled="readOnly"
                v-decorator="['name', { rules: [{ required: true, message: '请输入名称' }] }]"
              />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="编码" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input
                :disabled="readOnly"
                v-decorator="[
                  'code',
                  { rules: [{ required: true, message: '请输入编码' }, { validator: this.handleCodeChange }] },
                ]"
              />
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="所属父级" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-tree-select
                tree-data-simple-mode
                tree-default-expand-all
                placeholder="请选择所属父级"
                :disabled="readOnly"
                v-decorator="['parentId']"
                :treeData="parents"
                @change="handleParentChange"
                :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
              ></a-tree-select>
            </a-form-item>
          </a-col>

          <a-col :span="12">
            <a-form-item label="类型" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-select
                :disabled="readOnly"
                v-decorator="['menuType', { rules: [{ required: true, message: '请选择所属类型' }] }]"
                @change="handleMenuTypeChange"
                placeholder="请选择所属类型"
                style="width: 100%"
              >
                <a-select-option v-for="item in menuTypes" :key="item" :value="item">{{ item }}</a-select-option>
              </a-select>
            </a-form-item>
          </a-col>
        </a-row>

        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="请求地址" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input
                :disabled="readOnly"
                v-decorator="['path', { rules: [{ required: true, message: '请输入请求地址' }] }]"
              >
                <template slot="addonBefore">
                  {{ BaseUrl }}
                </template>
              </a-input>
            </a-form-item>
          </a-col>

          <a-col :span="12">
            <a-form-item label="页面位置" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input
                addon-before="@/views/"
                :disabled="readOnly || componentReadOnly"
                v-decorator="['component', { rules: [{ required: true, message: '请输入页面位置' }] }]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="图标" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-popover
                @visibleChange="handlePopoverChange"
                :visible="popoverVisible"
                placement="bottom"
                trigger="click"
              >
                <template slot="content">
                  <icon-list-bar :defaultIcon="defaultIcon" @callBackIcon="callBackIcon"></icon-list-bar>
                </template>
                <a-input :disabled="readOnly" v-decorator="['icon']" />
              </a-popover>
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="排序" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input-number style="width: 100%" :max="200" :min="1" :disabled="readOnly" v-decorator="['sort']" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-card>

      <a-card title="页面按钮" class="element-top" v-if="menuType !== '模块'">
        <operation
          :operation="modifyEntity.operation"
          :operations="operations"
          @add="handleAdd"
          @remove="handleRemove"
        />
      </a-card>

      <a-card title="操作历史" class="element-top" v-if="this.readOnly">
        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="创建时间" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input :disabled="readOnly" v-decorator="['createTime']" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="创建人" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input :disabled="readOnly" v-decorator="['createUser']" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="10">
          <a-col :span="12">
            <a-form-item label="修改时间" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input :disabled="readOnly" v-decorator="['updateTime']" />
            </a-form-item>
          </a-col>
          <a-col :span="12">
            <a-form-item label="修改人" :label-col="labelCol" :wrapper-col="wrapperCol">
              <a-input :disabled="readOnly" v-decorator="['updateUser']" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-card>
    </a-form>

    <footer-tool-bar>
      <a-space>
        <div v-if="!readOnly">
          <a-tag color="blue"> 操作完是否继续？</a-tag>
          <a-switch v-model="hasClose" :loading="loading">
            <a-icon slot="checkedChildren" type="check" />
            <a-icon slot="unCheckedChildren" type="close" />
          </a-switch>
        </div>

        <a-button icon="rollback" @click="handleRefresh">返回</a-button>
        <a-button icon="reload" v-if="!readOnly" @click="handleReset" :loading="loading">重置</a-button>
        <a-button icon="check-circle" v-if="!readOnly" :loading="loading" @click="handleSubmit">保存</a-button>
      </a-space>
    </footer-tool-bar>
  </a-spin>
</template>

<script>
import operation from './components/operation'
import { queryMenuOperation, deleteMenuOperation } from '@/api/menuOperation'
import { queryMenuTreeSelect, getMenu, addMenu, updateMenu } from '@/api/menu'
import IconListBar from '_c/IconListBar'
import FooterToolBar from '@/components/FooterToolbar'
export default {
  components: {
    operation,
    IconListBar,
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
          title: '新增菜单',
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
      BaseUrl: process.env.VUE_APP_API_BASE_URL,
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
      form: this.$form.createForm(this),
      formFields: {},
      entity: {},
      // 图标下拉
      popoverVisible: false,
      // 默认图标
      defaultIcon: '',
      // 父级菜单
      parents: [],
      // 页面位置只读
      componentReadOnly: false,
      // 按钮操作
      operations: [
        { id: '1', name: '新增', code: 'create', isNew: true, isEdit: true },
        { id: '2', name: '修改', code: 'edit', isNew: true, isEdit: true },
        { id: '3', name: '删除', code: 'delete', isNew: true, isEdit: true },
        { id: '4', name: '详情', code: 'details', isNew: true, isEdit: true },
        { id: '5', name: '导入', code: 'import', isNew: true, isEdit: true },
        { id: '6', name: '导出', code: 'export', isNew: true, isEdit: true },
      ],
      // 菜单类型
      menuType: '模块',
    }
  },
  mounted() {
    this.loadData()
  },
  methods: {
    handleCodeChange(rule, value, callback) {
      if (value && this.modifyEntity.operation === this.operationFlag.Add) {
        this.operations.map((m) => {
          if (m.code.indexOf('_') >= 0) {
            let arry = m.code.split('_')
            m.code = value + '_' + arry.length === 1 ? arry[0] : arry[1]
          } else {
            m.code = value + '_' + m.code
          }
          return m
        })
      }
      callback()
    },
    // 父级菜单选择
    handleParentChange(val) {
      if (val) {
        this.form.setFieldsValue({ parentId: val })
      }
    },
    // 菜单类型选择
    handleMenuTypeChange(val) {
      if (val) {
        if (val === '模块') {
          this.componentReadOnly = true
          this.form.setFieldsValue({ component: 'PageView' })
        } else {
          this.componentReadOnly = false
          this.form.setFieldsValue({ component: this.entity.component || '' })
        }

        this.menuType = val
        this.form.setFieldsValue({ menuType: val })
      }
    },
    // 图标气泡显示
    handlePopoverChange() {
      this.popoverVisible = true
    },
    // 图标选择后回调
    callBackIcon(val) {
      if (val) {
        this.form.setFieldsValue({ icon: val })
      }
      this.popoverVisible = false
    },
    //#region 加载数据
    // 按钮列表
    queryOperation() {
      queryMenuOperation({ menuId: this.modifyEntity.id }).then((res) => {
        this.operations = res.data.map((m) => {
          m.isNew = false
          if (this.modifyEntity.operation === this.operationFlag.Details) {
            m.isEdit = false
          } else {
            m.isEdit = true
          }
          return m
        })
      })
    },
    loadData() {
      // 父级菜单
      queryMenuTreeSelect().then((res) => {
        this.parents = res.data
      })
      this.readOnly = this.modifyEntity.operation === this.operationFlag.Details ? true : false
      let that = this
      this.$nextTick(() => {
        that.form.resetFields() // 重置参数值
        //修改，详情获取详细信息
        if (that.modifyEntity.operation !== that.operationFlag.Add) {
          // 按钮列表
          that.queryOperation()
          that.formFields = that.form.getFieldsValue()
          getMenu({ id: that.modifyEntity.id }).then((res) => {
            that.entity = res.data
            var setData = {}
            Object.keys(that.formFields).forEach((item) => {
              setData[item] = that.entity[item]
            })
            that.defaultIcon = that.entity.icon
            that.menuType = that.entity.menuType
            that.form.setFieldsValue(setData)
          })
        } else {
          this.handleMenuTypeChange('模块')
        }
      })
    },
    //#endregion

    //#region 操作按钮
    // 添加新行
    handleAdd() {
      let id = this.operations.length + 1
      this.operations.push({
        id: id + '',
        name: '',
        code: '',
        isNew: true,
        isEdit: true,
      })
    },
    // 删除
    handleRemove(item) {
      if (item.isNew) {
        this.operations.splice(this.operations.indexOf(item), 1)
      } else {
        var that = this
        this.$confirm({
          title: '确定要删除吗?',
          onOk() {
            // 提交到后端删除
            deleteMenuOperation([item.id]).then((res) => {
              if (res.success) {
                that.$notification.success({ description: res.msg })
                that.queryOperation()
              } else {
                that.$notification.error({ description: res.msg })
              }
            })
          },
          onCancel() {},
        })
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
            values.operations = that.operations.filter((m) => m.isNew)
            addMenu(values).then((res) => {
              that.submitResult(res)
            })
          } else {
            that.entity = Object.assign(that.entity, that.form.getFieldsValue())
            that.entity.operations = that.operations
            updateMenu(that.entity).then((res) => {
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
