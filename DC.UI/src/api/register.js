import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const registerColumns = [
    {
        title: "姓名",
        dataIndex: "name",
    },
    {
        title: "手机号",
        dataIndex: "tel",
    },
    // {
    //     title: "所属推手",
    //     dataIndex: "userName",
    // },
    {
        title: "所属产品",
        dataIndex: "productName",
    },
    {
        title: "IP地址",
        dataIndex: "ip",
    },
    {
        title: "来源",
        dataIndex: "systemType",
    },
    {
        width: 200,
        align: 'center',
        title: "创建时间",
        dataIndex: "createTime",
        sorter: true,
    },
    {
        align: 'center',
        title: "状态",
        dataIndex: "status",
        scopedSlots: { customRender: "registerStatus" },
    },
    {
        width: 260,
        align: 'center',
        fixed: 'right',
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const registerDevColumns = [
    {
        width:100,
        title: "姓名",
        dataIndex: "name",
    },
    {
        width:100,
        title: "手机号",
        dataIndex: "tel",
        scopedSlots: { customRender: "tel" },
    },
    {
        width:100,
        title: "所属产品",
        dataIndex: "productName",
    },
    {
        width:100,
        title: "IP地址",
        dataIndex: "ip",
    },
    {
        width:160,
        title: "来源",
        dataIndex: "systemType",
    },
    {
        width: 160,
        align: 'center',
        title: "创建时间",
        dataIndex: "createTime",
        sorter: true,
    },
    {
        width: 160,
        align: 'center',
        title: "状态",
        dataIndex: "status",
        scopedSlots: { customRender: "registerStatus" },
    },
    {
        width: 100,
        align: 'center',
        fixed: 'right',
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const registerDev2Columns = [
    {
        width:100,
        title: "姓名",
        dataIndex: "name",
    },
    {
        width:100,
        title: "手机号",
        dataIndex: "tel",
        scopedSlots: { customRender: "tel" },
    },
    {
        width:100,
        title: "所属产品",
        dataIndex: "productName",
    },
    {
        width:100,
        title: "IP地址",
        dataIndex: "ip",
    },
    {
        width:160,
        title: "来源",
        dataIndex: "systemType",
    },
    {
        width: 160,
        align: 'center',
        title: "创建时间",
        dataIndex: "createTime",
        sorter: true,
    },
    {
        width: 100,
        align: 'center',
        fixed: 'right',
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const addRegister = (parameter) => post('api/register/addRegister', parameter)
export const deleteRegister = (parameter) => post('api/register/deleteRegister', parameter)
export const updateRegister = (parameter) => put('api/register/updateRegister', parameter)
export const queryRegister = (parameter) => get('api/register/queryRegister', parameter)
export const getRegister = (parameter) => get('api/register/getRegister', parameter)
export const queryRegisterList = (parameter) => get('api/register/queryRegisterList', parameter)
export const setRegisterStatus = (parameter) => put('api/register/setRegisterStatus', parameter)

