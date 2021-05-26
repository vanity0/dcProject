import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const actionLogColumns = [
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

export const deleteActionLog = (parameter) => post('api/actionLog/deleteActionLog', parameter)
export const queryActionLog = (parameter) => get('api/actionLog/queryActionLog', parameter)
export const getActionLog = (parameter) => get('api/actionLog/getActionLog', parameter)

