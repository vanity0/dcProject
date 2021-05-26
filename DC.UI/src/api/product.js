import { post, put, get } from '@/utils/hettpUtils/httpRequest'

/**
 * 平台显示
 */
export const productColumns = [
    {
        width: 100,
        title: "名称",
        dataIndex: "name",
    },
    {
        width: 80,
        align: 'center',
        title: "上架推手",
        dataIndex: "auditStatus",
        scopedSlots: { customRender: "auditStatus" },
    },
    {
        width: 80,
        align: 'center',
        title: "上架H5",
        dataIndex: "h5",
        scopedSlots: { customRender: "h5" },
    },
    {
        width: 100,
        align: 'center',
        title: "产品类型",
        dataIndex: "productType",
    },
    {
        width: 80,
        align: 'center',
        title: "推广模式",
        dataIndex: "linkModel",
    },
    {
        width: 80,
        align: 'center',
        title: "推广价格",
        dataIndex: "extendMoney",
        sorter: true,
    },
    {
        width: 50,
        align: 'center',
        title: "排序",
        dataIndex: "sort",
        sorter: true,
    },
    {
        width: 160,
        align: 'center',
        title: "创建人",
        dataIndex: "createUser",
    },
    {
        width: 200,
        align: 'center',
        title: "创建时间",
        dataIndex: "createTime",
        sorter: true,
    },
    {
        width: 320,
        align: 'center',
        fixed: 'right',
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

/**
 * 一级推手显示
 */
export const productDevColumns = [
    {
        width: 50,
        align: 'center',
        title: "图片",
        dataIndex: "logo",
        scopedSlots: { customRender: "logo" },
    },
    {
        width: 30,
        align: 'left',
        title: "名称",
        dataIndex: "name",
    },
    {
        width: 30,
        align: 'left',
        title: "系列名称",
        dataIndex: "aliasName",
    },
    {
        width: 30,
        align: 'center',
        title: "推广价格",
        dataIndex: "extendMoney",
    },
    {
        width: 30,
        align: 'left',
        title: "模式",
        dataIndex: "linkModel",
    },
];

export const productDev2Columns = [
    {
        width: 50,
        align: 'center',
        title: "图片",
        dataIndex: "logo",
        scopedSlots: { customRender: "logo" },
    },
    {
        width: 30,
        align: 'left',
        title: "名称",
        dataIndex: "name",
    },
    {
        width: 30,
        align: 'left',
        title: "系列名称",
        dataIndex: "aliasName",
    },
    {
        width: 30,
        align: 'center',
        title: "模式",
        dataIndex: "linkModel",
    },
];


export const setProducth5 = (parameter) => put('api/product/setProducth5', parameter)
export const setProductStatus = (parameter) => put('api/product/setProductStatus', parameter)
export const addProduct = (parameter) => post('api/product/addProduct', parameter)
export const deleteProduct = (parameter) => post('api/product/deleteProduct', parameter)
export const updateProduct = (parameter) => put('api/product/updateProduct', parameter)
export const queryProduct = (parameter) => get('api/product/queryProduct', parameter)
export const getProduct = (parameter) => get('api/product/getProduct', parameter)
export const queryProductList = (parameter) => get('api/product/queryProductList', parameter)
// 获取推手端显示的产品列表
export const queryProductDev = (parameter) => get('api/product/queryProductDev', parameter)

export const sortProduct = (parameter) => put('api/product/sortProduct', parameter)
