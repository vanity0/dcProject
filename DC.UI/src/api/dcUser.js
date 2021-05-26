import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const dcUserColumns = [
    {
        width:100,
        title: "姓名",
        dataIndex: "name",
    },
    {
        width:180,
        title: "账号",
        dataIndex: "account",
    },
    {
        width:180,
        title: "所属角色",
        dataIndex: "roleId",
    }, 
    {
        width:100,
        align: 'center',
        title: "状态",
        dataIndex: "status",
        scopedSlots: { customRender: "status" },
    },
    {
        width:180,
        align: 'center',
		ellipsis: true,
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const resetPwd = (parameter) => put('api/dcUser/resetPwd', parameter)
export const addDCUser = (parameter) => post('api/dcUser/addDCUser', parameter)
export const deleteDCUser = (parameter) => post('api/dcUser/deleteDCUser', parameter)
export const updateDCUser = (parameter) => put('api/dcUser/updateDCUser', parameter)
export const getDCUser = (parameter) => get('api/dcUser/getDCUser', parameter)
export const queryAllDCUser = (parameter) => get('api/dcUser/queryAllDCUser', parameter)
export const queryDCUserChilder = (parameter) => get('api/dcUser/queryDCUserChilder', parameter)

