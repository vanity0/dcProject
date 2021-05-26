module.exports = {
  root: true,
  // 标记不同的运行环境
  env: {
      node: true
  },
  // 继承共享配置
  extends: [
      'plugin:vue/essential', 'eslint:recommended'
  ],
  // 配置ESLint中每个规则的开启或关闭，属性为内置属性，属性value是off,warn,error
  rules: {
      'no-console': process.env.NODE_ENV === 'production' ? 'off' : 'off',
      'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
      "no-unused-vars": ['off'],
      "vue/no-unused-components": ['off']
  },
  // 设置js的版本，代码低于该版本会无法执行
  parserOptions: {
      parser: 'babel-eslint'
  }
}
