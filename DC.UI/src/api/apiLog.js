import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const apiLogColumns = [
    {
        width:100,
        align: 'center',
        title: "请求方式",
        dataIndex: "method",
    },
    {
        width:200,
        ellipsis: true,
        title: "请求地址",
        dataIndex: "requestUrl",
    },
    {
        width:100,
        align: 'center',
        title: "耗时(毫秒)",
        dataIndex: "elapsedTime",
    },
    {
        width:100,
        title: "用户IP",
        dataIndex: "remoteIp",
    },
    {
        width:100,
        title: "创建人",
        dataIndex: "createUser",
    },
    {
        width:180,
        align: 'center',
        title: "创建时间",
        dataIndex: "createTime",
        sorter: true,
    },
    {
        width:100,
        align: 'center',
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const deleteApiLog = (parameter) => post('api/apiLog/deleteApiLog', parameter)
export const queryApiLog = (parameter) => get('api/apiLog/queryApiLog', parameter)
export const getApiLog = (parameter) => get('api/apiLog/getApiLog', parameter)

