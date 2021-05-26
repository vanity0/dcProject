<template>
  <a-spin :spinning="spinning" :delay="delayTime">
    <div class="page-header-index-wide page-header-wrapper-grid-content-main" v-show="showProd">
      <a-row>
        <a-col :md="24">
          <a-card :bordered="false">
            <div class="account-center-avatarHolder">
              <div class="avatar">
                <img :src="product.logo" />
              </div>
              <div class="username">{{ product.name }}</div>
              <div class="bio">{{ product.name }}</div>
            </div>
            <a-divider />
            <a-card :bordered="false">
              <a-tag color="#f50" v-if="product.type">{{ product.type }}</a-tag>
              <a-tag color="#2db7f5" v-if="product.tag">{{ product.tag }}</a-tag>
            </a-card>

            <a-divider orientation="left">
              <a-icon type="heart" theme="twoTone" two-tone-color="#eb2f96" />申请流程</a-divider
            >
            <a-card :bordered="false">
              <a-list>
                <a-list-item>
                  <a-list-item-meta>
                    <a slot="title"> </a>
                  </a-list-item-meta>
                  <a-steps size="small">
                    <a-step status="wait" title="手机号">
                      <a-icon slot="icon" type="phone" />
                    </a-step>
                    <a-step status="wait" title="身份证">
                      <a-icon slot="icon" type="solution" />
                    </a-step>
                    <a-step status="wait" title="银行卡">
                      <a-icon slot="icon" type="credit-card" />
                    </a-step>
                    <a-step status="wait" title="成功">
                      <a-icon slot="icon" type="smile-o" />
                    </a-step>
                  </a-steps>
                </a-list-item>
              </a-list>
            </a-card>
          </a-card>
        </a-col>
      </a-row>

      <a-modal
        v-model="showModal"
        centered
        :maskClosable="false"
        title="我要申请"
        @ok="handleSubmit"
        @cancel="handleCancel"
      >
        <a-spin :spinning="spinning">
          <a-form
            :form="form"
            :labelCol="{ md: { span: 4 }, xs: { span: 24 }, sm: { span: 12 } }"
            :wrapperCol="{ md: { span: 20 }, xs: { span: 24 }, sm: { span: 12 } }"
          >
            <a-row>
              <a-col>
                <a-form-item label="姓名">
                  <a-input
                    placeholder="请输入姓名"
                    allowClear
                    v-decorator="['name', { rules: [{ required: true, message: '请输入姓名' }] }]"
                  />
                </a-form-item>
              </a-col>
              <a-col>
                <a-form-item label="电话">
                  <a-input
                    placeholder="请输入电话"
                    allowClear
                    v-decorator="['phone', { rules: [{ required: true, message: '请输入电话' }] }]"
                  />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </a-spin>
      </a-modal>

      <footer-tool-bar>
        <a-button icon="form" type="danger" @click="handleOpenRegister" block>我要申请</a-button>
      </footer-tool-bar>
    </div>

    <a-result
      v-show="!showProd"
      status="404"
      title="链接已经失效"
      sub-title="Sorry, the page you visited does not exist."
    >
    </a-result>
  </a-spin>
</template>

<script>
import FooterToolBar from '@/components/FooterToolbar'
import { getProductByEnCode } from '@/api/pvHistory'
import { addUvHistory } from '@/api/uvHistory'
import { mobileInfo } from '@/utils/mobileUtils/mobile'
export default {
  components: { FooterToolBar },
  data() {
    return {
      delayTime: 1000,
      spinning: true,
      showProd: true,
      product: {
        logo: '',
        name: '',
        tag: '',
        type: '',
        uId: '',
        id: '',
      },
      mobile: '',
      showModal: false,
      // 模态框表单
      form: this.$form.createForm(this),
      url: '',
    }
  },
  mounted() {},
  created() {
    this.loadMobie()
    this.getProd()
  },
  methods: {
    //#region 获取手机型号
    loadMobie() {
      let m = mobileInfo()
      if (m) {
        this.mobile = m.os + ' ' + m.model + ' ' + m.version
      }
    },
    //#endregion

    //#region 获取产品详情，计算PV点击量
    getProd() {
      let url = this.$route.fullPath.split('?')
      if (url.length > 1) {
        if (url[1]) {
          // 计算PV访问量
          getProductByEnCode({ c: url[1], m: this.mobile })
            .then((res) => {
              if (res.success) {
                this.product = res.data
                this.showProd = true
                this.spinning = false
              }
            })
            .catch((error) => {
              this.showProd = false
              this.spinning = false
            })
        }
      }
    },
    //#endregion

    //#region 提交申请，计算UV,跳转产品地址
    // 弹出申请页面
    handleOpenRegister() {
      this.showModal = true
    },
    handleCancel() {
      this.showModal = false
      this.form.resetFields()
    },
    // 提交申请数据
    handleSubmit() {
      let that = this
      this.form.validateFields((error, values) => {
        that.loadingModel = true
        if (!error) {
          // 提交申请数据，计算UV访问量
          addUvHistory({
            name: values.name,
            phone: values.phone,
            u: that.product.uId,
            p: that.product.id,
            m: that.mobile,
          })
            .then((res) => {
              if (res.success) {
                that.handleCancel()
                window.location = res.data.url
              }
              // 跳转到产品页面
              // that.$router.push({
              //   path: '/r',
              //   query: {
              //     p: that.product.id,
              //     u: that.product.uId,
              //     url: res.data.url,
              //     regId: res.data.regId,
              //   },
              // })
            })
            .catch((error) => {
              that.showModal = false
            })
        }
      })
    },
    //#endregion
  },
}
</script>

<style lang="less" scoped>
.page-header-wrapper-grid-content-main {
  width: 100%;
  height: 100%;
  min-height: 100%;
  transition: 0.3s;

  .account-center-avatarHolder {
    text-align: center;
    & > .avatar {
      margin: 0 auto;
      width: 104px;
      height: 104px;
      border-radius: 50%;
      overflow: hidden;
      img {
        height: 100%;
        width: 100%;
      }
    }
    .username {
      color: rgba(0, 0, 0, 0.85);
      font-size: 20px;
      line-height: 28px;
      font-weight: 500;
      margin-bottom: 4px;
    }
  }
}
.ant-divider-horizontal {
  margin: 10px 0;
}
.ant-divider-horizontal.ant-divider-with-text-center,
.ant-divider-horizontal.ant-divider-with-text-left,
.ant-divider-horizontal.ant-divider-with-text-right {
  margin: 10px 0;
}
</style>