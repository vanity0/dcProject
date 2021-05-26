<template>
  <a-spin :spinning="confirmLoading">
    <a-form class="ant-advanced-search-form" :form="form">
      <a-row :gutter="24">
        <a-col :span="12">
          <a-form-item label="请求地址" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-input disabled v-decorator="['requestUrl']" />
          </a-form-item>
        </a-col>

        <a-col :span="12">
          <a-form-item label="请求方式" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-input disabled v-decorator="['method']" />
          </a-form-item>
        </a-col>
      </a-row>

      <a-row :gutter="24">
        <a-col :span="12">
          <a-form-item label="用户代理" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-input disabled v-decorator="['userAgent']" />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item label="用户IP地址" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-input disabled v-decorator="['remoteIp']" />
          </a-form-item>
        </a-col>
      </a-row>
      <a-row :gutter="24">
        <a-col :span="12">
          <a-form-item label="异常名称" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-input disabled v-decorator="['exceptionName']" />
          </a-form-item>
        </a-col>
      </a-row>

      <a-row :gutter="24">
        <a-col :span="12">
          <a-form-item label="异常信息" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-textarea disabled v-decorator="['message']" :rows="4" />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item label="堆栈信息" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-textarea disabled v-decorator="['stackTrace']" :rows="4" />
          </a-form-item>
        </a-col>
      </a-row>

      <a-row :gutter="24">
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
      <a-row :gutter="24">
        <a-col :span="12">
          <a-form-item label="请求参数" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-input disabled v-decorator="['requestParams']" />
          </a-form-item>
        </a-col>
        <a-col :span="12">
          <a-form-item label="请求时间" :label-col="labelCol" :wrapper-col="wrapperCol">
            <a-input disabled v-decorator="['requestTime']" />
          </a-form-item>
        </a-col>
      </a-row>
      <a-row :gutter="24">
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
    <div class="table-operator element-right">
      <a-button icon="left-circle" @click="handleHideModal">返回</a-button>
    </div>
  </a-spin>
</template>

<script>
import { getErrorLog } from "@/api/errorLog";
export default {
  data() {
    return {
      labelCol: { span: 4 },
      wrapperCol: { span: 20 },
      // 是否加载提示
      confirmLoading: false,
      form: this.$form.createForm(this),
      formFields: {},
      entity: {},
    };
  },
  created() {
    this.initData();
  },
  methods: {
    initData() {
      this.form.resetFields(); // 重置参数值
      //详情获取详细信息
      this.$nextTick(() => {
        this.formFields = this.form.getFieldsValue();
        getErrorLog({ id: this.id }).then((res) => {
          this.entity =  res.data;
          var setData = {};
          Object.keys(this.formFields).forEach((item) => {
            setData[item] = this.entity[item];
          });
          this.form.setFieldsValue(setData);
        });
      });
    },
    // 传值到父页面
    handleHideModal() {
      this.$emit("handleHideModal");
    },
  },
  props: {
    id: {
      type: String,
      required: true,
    },
  },
};
</script>
