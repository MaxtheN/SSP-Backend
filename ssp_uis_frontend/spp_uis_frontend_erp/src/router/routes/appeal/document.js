const breadcrumbDef = [
    {
        text: 'Appeal',
        to: 'Appeal'
    }
]


export default [
    {
        path: '/appeal/document/AppealApplication',
        name: 'AppealApplication',
        component: () => import('@/views/appeal/document/AppealApplication/index.vue'),
        meta: {
            pageTitle: 'AppealApplication',
            breadcrumb: [
                ...breadcrumbDef,
                {
                    text: 'AppealApplication',
                    active: true
                }
            ]
        }
    },
    {
        path: '/appeal/document/AppealApplication/edit/id=:id',
        name: 'EditAppealApplication',
        component: () => import('@/views/appeal/document/AppealApplication/edit.vue'),
        meta: {
            pageTitle: 'AppealApplication',
            navActiveLink: 'AppealApplication',
            breadcrumb: [
                ...breadcrumbDef,
                {
                    text: 'AppealApplication',
                    active: true
                }
            ]
        }
    },
]
