// import storage from 'store'

import { login, getUserInfo, logout } from '@/api/login'
import { clearTokenCache, setAccessToken } from '@/utils/tokenUtils/accessTokenCache'

const user = {
  //  存放状态 类似vue data属性
  state: {
    id: '',
    name: '',
    account: '',
    isSystem: '',
    accessToken: '',
    lastLoginTime:'',
    lastLoginIp:'',
    roles: [],
  },
  // 操作state数据的方法的集合，比如对该数据的修改、增加、删除等等，参数一是当前VueX对象中的state，参数二是该方法在被调用时传递参数使用的
  mutations: {
    setUerInfo(state, payLoad) {
      state.id = payLoad.id
      state.name = payLoad.name
      state.lastLoginTime =payLoad.lastLoginTime
      state.lastLoginIp =payLoad.lastLoginIp
      state.account = payLoad.account
      state.roles = [{ role: payLoad.roleId }]
    },
    setAccessToken: (state, accessToken) => {
      if (accessToken) state.accessToken = accessToken
    }
  },
  getters: {
    accessToken(state) {
      return state.accessToken
    }
  },
  // 异步操作
  actions: {
    /**
    * 登录，获取令牌
    * @param {} userInfo 
    */
    async login({ commit }, userInfo) {
      return new Promise((resolve, reject) => {
        login(userInfo).then(res => {
          // 记录token
          commit("setAccessToken", res.data);
          setAccessToken(res.data);
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },

    /**
    * 调用getUserInfo方法以后，进入actions，
    * 然后通过commit调用setUserInfo，将res（用户信息）作为参数传入传入进去，并将相对应的属性值赋值给state，
    * @param { 可以理解为它是整个Store的对象.类似于this.$store， 他里面包含了state，getter，mutations，actions } context
    */
    async getUserInfo(context) {
      return new Promise((resolve, reject) => {
        getUserInfo().then(res => {
          //相当于 this.$store.commit,第一个参数是方法名，第二个参数是要传入的数据
          context.commit('setUerInfo', res.data)
          resolve(res.data)
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 登出
    async logout({ commit, state }) {
      return new Promise((resolve) => {
        logout(state.accessToken).then(() => {
          clearTokenCache();
          resolve()
        }).catch(() => {
          resolve()
        }).finally(() => {
          commit('setAccessToken', '')
          clearTokenCache();
        })
      })
    }

  }
}

export default user
