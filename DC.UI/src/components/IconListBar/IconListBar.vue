<template>
  <a-card class="icon-card">
    <a-card-grid
      v-for="(item,index) in iconList"
      :key="index"
      :class="defaultIcon === item.font_class?'active':''"
      @click.stop="handleClickIcon(item.font_class)"
    >
      <icon-font :type="item.font_class"></icon-font>
    </a-card-grid>
  </a-card>
</template>
<script>
import IconFont from "_c/IconFont";
import iconfontData from "@/assets/iconfont/iconfont.json";
export default {
  name: "IconListBar",
  components: {
    IconFont,
  },
  data() {
    return {
      iconList: [],
    };
  },
  created() {
    this.loadIconList();
  },
  methods: {
    loadIconList() {
      this.iconList = iconfontData.glyphs;
    },
    handleClickIcon(icon) {
      this.$emit("callBackIcon", icon);
    },
  },
  props: {
    defaultIcon: {
      type: String,
      required: false,
    },
  },
};
</script>
<style lang="less" scoped>
.icon-card {
  width: 400px;
  height: 360px;
  overflow-y: auto;
}
.ant-card-grid {
  width: 20%;
  text-align: center;
  cursor: pointer;
}
.active {
  background-color: #1890ff;
  color: #fff;
}
.ant-card-grid:hover {
  background-color: #1890ff;
  color: #fff;
  .ant-card-meta-title:hover {
    color: #fff !important;
  }
}
</style>
 