import {
    UserLayout,
} from '@/layouts' 
export const constantRouterMap = [
    {
        path: '/d',
        name: 'devprod',
        meta:{title:'产品详情'},
        component: () => import('@/views/productDev/details')
    },
    {
        path: '/r',
        name: 'regprod',
        meta:{title:'产品详情'},
        component: () => import('@/views/productDev/prodRegister')
    },
    {
        path: '/user',
        component: UserLayout,
        redirect: '/user/login',
        hidden: true,
        children: [
            {
                path: 'login',
                name: 'login',
                component: () => import('@/views/login/index')
            },
            {
                path: 'recover',
                name: 'recover',
                component: undefined
            }
        ]
    },
    {
        path: '/404',
        component: () => import(/* webpackChunkName: "fail" */ '@/views/exception/404')
    }
]
