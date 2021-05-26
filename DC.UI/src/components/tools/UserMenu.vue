<template>
  <div class="user-wrapper">
    <div class="content-box">
      <screenfull-bar class="action" />
 
      <!-- <notice-icon class="action" /> -->
      <!-- <a-dropdown> -->
        <span class="action ant-dropdown-link user-dropdown-menu">
          <span>{{ nickname }}</span>
        </span>
      <a href="javascript:;" @click="handleLogout">
        <span class="action">
          <a-icon type="logout"></a-icon>
          <span>退出登录</span>
        </span>
      </a>
    </div>
  </div>
</template>

<script>
import ScreenfullBar from '_c/ScreenfullBar'
import NoticeIcon from '@/components/NoticeIcon'
import { mapActions, mapGetters } from 'vuex'

export default {
  name: 'UserMenu',
  components: {
    NoticeIcon,
    ScreenfullBar,
  },
  computed: {
    ...mapGetters(['nickname']),
  },
  methods: {
    ...mapActions(['logout']),
    handleLogout() {
      let that = this
      this.$confirm({
        title: '提示',
        content: '真的要注销登录吗 ?',
        onOk: () => {
          return that
            .logout({})
            .then(() => {
              setTimeout(() => {
                window.location.reload()
              }, 16)
            })
            .catch((err) => {
              that.$message.error({
                title: '错误',
                description: err.message,
              })
            })
        },
        onCancel() {},
      })
    },
  },
}
</script>
