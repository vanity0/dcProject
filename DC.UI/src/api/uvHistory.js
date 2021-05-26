import { post, put, get } from '@/utils/hettpUtils/httpRequest'


export const registerUvColumns = [
    {
        title: "姓名",
        dataIndex: "name",
    },
    {
        title: "手机号",
        dataIndex: "phone",
    },
    {
        title: "IP",
        dataIndex: "ip",
    },
    {
        align: 'center',
        title: "系统",
        dataIndex: "systemType",
    },
    {
        align: 'center',
        title: "推广人",
        dataIndex: "dcUserDevName",
    },
    {
        align: 'center',
        title: "申请产品",
        dataIndex: "productName",
    },
    {
        align: 'center',
        title: "状态",
        dataIndex: "status",
        scopedSlots: { customRender: "status" },
    },
    {
        align: 'center',
        title: "创建日期",
        dataIndex: "createTime",
    },
    {
        width: 70,
        align: 'center',
        ellipsis: true,
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

// 点击注册，记录uv
export const addUvHistory = (parameter) => post('api/uvHistory/addUvHistory', parameter)
export const deleteUvHistory = (parameter) => post('api/uvHistory/deleteUvHistory', parameter)
export const updateUvHistory = (parameter) => put('api/uvHistory/updateUvHistory', parameter)
export const queryUvHistory = (parameter) => get('api/uvHistory/queryUvHistory', parameter)


