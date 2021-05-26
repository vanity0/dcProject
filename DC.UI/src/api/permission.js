import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const permissionColumns = [
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

export const addPermission = (parameter) => post('api/permission/addPermission', parameter)
export const deletePermission = (parameter) => post('api/permission/deletePermission', parameter)
export const updatePermission = (parameter) => put('api/permission/updatePermission', parameter)
export const queryPermission = (parameter) => get('api/permission/queryPermission', parameter)
export const getPermission = (parameter) => get('api/permission/getPermission', parameter)
export const queryPermissionList = (parameter) => get('api/permission/queryPermissionList', parameter)

