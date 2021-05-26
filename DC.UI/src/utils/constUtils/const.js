

/**
 * 页面操作类型
 */
const operationFlag = {
    /**
     * 添加操作
     */
    Add: 'ADD',
    /**
     * 修改操作
     */
    Edit: 'EDIT',
    /**
     * 详情操作
     */
    Details: 'DETAILS'
}

/**
 * 页面操作类型
 */
const menuTypes = ['模块', '菜单']

/**
 * 产品分类
 */
const productTypes = ["小额", "大额", "权益", "助贷", "贷超"]

/**
 * 推广模式
 */
const linkModels = ["CPA申请","CPA", "CPS"]


/**
 * 产品状态
 */
const auditStatus = ["待申请", "审核中", "审核通过", "拒绝通过", "上架中", "下架中"]

/**
 * 产品状态
 */
const registerStatus = ["待更新","审核通过", "拒绝通过"]

/**
* 产品标签
*/
const productTags = ["利息低", "审批快", "门槛低", "额度高", "可分期", "还款灵活"]

/**
 * 角色
 */
 const roles = ["平台", "商家", "一级推手","二级推手"]

/**
 * //通过js方式使用：
 * this.global.title
 * //或在 html 结构中使用
 * {{global.title}}
 */
export default {
    install(Vue, options) {
        Vue.prototype.operationFlag = operationFlag,
            Vue.prototype.menuTypes = menuTypes,
            Vue.prototype.productTypes = productTypes,
            Vue.prototype.linkModels = linkModels,
            Vue.prototype.auditStatus = auditStatus,
            Vue.prototype.registerStatus = registerStatus,
            Vue.prototype.productTags = productTags,
            Vue.prototype.roles = roles
    }
}
