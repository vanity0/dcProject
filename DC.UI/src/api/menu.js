import { post, put, get } from '@/utils/hettpUtils/httpRequest'

export const menuColumns = [
    {
        title: "名称",
        dataIndex: "name",
    },
    {
        title: "代码",
        dataIndex: "code",
    },
    {
        title: "页面位置",
        dataIndex: "component",
    },
    {
        title: '请求地址',
        dataIndex: 'path',
    },
    {
        title: '跳转地址',
        dataIndex: 'redirect',
    },
    {
        title: "图标",
        dataIndex: "icon",
    },
    {
        title: "菜单类型",
        dataIndex: "menuType",
        scopedSlots: { customRender: "menuType" },
    }, 
    {
        title: "排序",
        dataIndex: "sort",
        sorter: true,
    },
    {
        align: 'center',
        title: "操作",
        dataIndex: "action",
        scopedSlots: { customRender: "action" },
    },
];

export const addMenu = (parameter) => post('api/menu/addMenu', parameter)
export const deleteMenu = (parameter) => post('api/menu/deleteMenu', parameter)
export const updateMenu = (parameter) => put('api/menu/updateMenu', parameter)
export const queryMenu = (parameter) => get('api/menu/queryMenu', parameter)
export const getMenu = (parameter) => get('api/menu/getMenu', parameter)
export const queryMenuTreeSelect = (parameter) => get('api/menu/queryMenuTreeSelect', parameter)
export const queryMenuTree = (parameter) => get('api/menu/queryMenuTree', parameter)
 