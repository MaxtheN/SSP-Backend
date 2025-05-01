import document from "./document";
import info from "./info";
import report from "./report";
import order from "./order";

export default [
    {
        path: '/hrm',
        component: () => import('@/components/WRouterView.vue'),
        redirect: { name: 'Hrm' },
        children: [
            {
                path: '/',
                name: 'Hrm',
                component: () => import('@/views/hrm/index.vue'),
                meta: {
                    pageTitle: 'Hrm',
                    breadcrumb: [
                        {
                            text: 'Hrm',
                            active: true
                        }
                    ]
                }
            },
            ...document,
            ...info,
            ...order,
            ...report
        ]
    }
]