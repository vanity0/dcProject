import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const pvHistoryColumns = [
    {
        title: "位置",
        dataIndex: "location",
    },
    {
        title: "类型",
        dataIndex: "bannerType",
        scopedSlots: { customRender: "bannerType" },
    },
    {
        title: "链接地址",
        dataIndex: "LinkUrl",
    },
    {
        align: 'center',
        title: "创建人",
        dataIndex: "createUser",
    },
    {
        align: 'center',
        title: "创建时间",
        dataIndex: "createTime",
        sorter: true,
    },
    {
        align: 'center',
        title: "状态",
        dataIndex: "status",
        scopedSlots: { customRender: "status" },
    },
    {
        align: 'center',
        title: "是否删除",
        dataIndex: "deleteFlag",
        scopedSlots: { customRender: "deleteFlag" },
    },
    {
        align: 'center',
		ellipsis: true,
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const addPvHistory = (parameter) => post('api/pvHistory/addPvHistory', parameter)
export const deletePvHistory = (parameter) => post('api/pvHistory/deletePvHistory', parameter)
export const updatePvHistory = (parameter) => put('api/pvHistory/updatePvHistory', parameter)
export const queryPvHistory = (parameter) => get('api/pvHistory/queryPvHistory', parameter)
export const getPvHistory = (parameter) => get('api/pvHistory/getPvHistory', parameter)
export const queryPvHistoryList = (parameter) => get('api/pvHistory/queryPvHistoryList', parameter)

// 根据复制链接的加密短码获取产品信息
export const getProductByEnCode = (parameter) => post('api/pvHistory/getProductByEnCode', parameter)
