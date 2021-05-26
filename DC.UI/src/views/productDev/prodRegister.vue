<template>
  <iframe id="prod" style="height: 100%; width: 100%" :src="url"></iframe>
</template>

<script>
import { addRegister } from '@/api/register'
import { mobileInfo } from '@/utils/mobileUtils/mobile'
export default {
  name: 'Register',
  components: {},
  data() {
    return {
      p: '',
      u: '',
      registerId: '',
      url: '',
      mobile: '',
      times: 30,
      timer: null,
    }
  },
  created() {
    this.loadMobie()
    this.loadData()
    this.startInterval()
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

    //#region 加载页面URL地址
    loadData() {
      let query = this.$route.query
      this.p = query.p
      this.u = query.u
      this.url = query.url
      this.regId = query.regId
      // window.location = query.url
      // window.history.pushState(query.url)
    },
    //#endregion

    //#region 倒计时30s
    // 开始倒计时30s后，增加CPA
    startInterval() {
      let that = this
      this.timer = setInterval(() => {
        that.times--
        console.log(that.times)
        if (that.times === 0) {
          clearInterval(that.timer)
          that.addCpa()
        }
      }, 1000)
    },
    addCpa() {
      addRegister({
        u: this.u,
        p: this.p,
        m: this.mobile,
        regId: this.regId,
      })
    },
    //#endregion
  },
}
</script> 