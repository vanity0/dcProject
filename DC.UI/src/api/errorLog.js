import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const errorLogColumns = [
    {
        width: 100,
        title: "用户IP",
        dataIndex: "remoteIp",
    },
    {
        width: 160,
        ellipsis: true,
        title: "异常名",
        dataIndex: "exceptionName",
    },
    {
        width: 160,
        ellipsis: true,
        title: "异常信息",
        dataIndex: "message",
    },
    {
        width: 100,
        title: "创建人",
        dataIndex: "createUser",
    },
    {
        width: 180,
        align: 'center',
        title: "创建时间",
        dataIndex: "createTime",
        sorter: true,
    },
    {
        width: 100,
        align: 'center',
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const deleteErrorLog = (parameter) => post('api/errorLog/deleteErrorLog', parameter)
export const queryErrorLog = (parameter) => get('api/errorLog/queryErrorLog', parameter)
export const getErrorLog = (parameter) => get('api/errorLog/getErrorLog', parameter)

