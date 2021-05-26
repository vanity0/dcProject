import { post, put, get } from '@/utils/hettpUtils/httpRequest'


export const productReportColumns = [
    {
        title: "产品名称",
        dataIndex: "name",
    },
    {
        align: 'center',
        title: "PV",
        dataIndex: "pv",
        sorter: (a, b) => a.pv - b.pv,
    },
    {
        align: 'center',
        title: "UV",
        dataIndex: "uv",
        sorter: (a, b) => a.uv - b.uv,
    },
    {
        align: 'center',
        title: "IOS",
        dataIndex: "ios",
        sorter: (a, b) => a.ios - b.ios,
    },
    {
        align: 'center',
        title: "Android",
        dataIndex: "android",
        sorter: (a, b) => a.android - b.android,
    }
];
export const queryList = (parameter) => get('api/report/queryList', parameter)

