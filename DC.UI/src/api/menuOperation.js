import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const menuOperationColumns = [
    {
        title: "名称",
        dataIndex: "name",
        scopedSlots: { customRender: "name" },
    },
    {
        title: "标识",
        dataIndex: "code",
        scopedSlots: { customRender: "code" },
    },
    {
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
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const deleteMenuOperation = (parameter) => post('api/menuoperation/deleteMenuOperation', parameter)
export const updateMenuOperation = (parameter) => put('api/menuoperation/updateMenuOperation', parameter)
export const queryMenuOperation = (parameter) => get('api/menuoperation/queryMenuOperation', parameter)
