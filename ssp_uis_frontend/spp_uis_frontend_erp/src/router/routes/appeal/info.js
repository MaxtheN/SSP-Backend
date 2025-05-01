const breadcrumbDef = [
    {
        text: 'Appeal',
        to: 'Appeal'
    }
]


export default [
    {
        path: '/appeal/info/AppealDescription',
        name: 'AppealDescription',
        component: () => import('@/views/appeal/info/AppealDescription/index.vue'),
        meta: {
            pageTitle: 'AppealDescription',
            breadcrumb: [
                ...breadcrumbDef,
                {
                    text: 'AppealDescription',
                    active: true
                }
            ]
        }
    },
    {
        path: '/appeal/info/AppealDescription/edit/id=:id',
        name: 'EditAppealDescription',
        component: () => import('@/views/appeal/info/AppealDescription/edit.vue'),
        meta: {
            pageTitle: 'AppealDescription',
            navActiveLink: 'AppealDescription',
            breadcrumb: [
                ...breadcrumbDef,
                {
                    text: 'AppealDescription',
                    active: true
                }
            ]
        }
    },

    {
        path: '/appeal/info/AppealTypeArrive',
        name: 'AppealTypeArrive',
        component: () => import('@/views/appeal/info/AppealTypeArrive/index.vue'),
        meta: {
            pageTitle: 'AppealTypeArrive',
            breadcrumb: [
                ...breadcrumbDef,
                {
                    text: 'AppealTypeArrive',
                    active: true
                }
            ]
        }
    },
    {
        path: '/appeal/info/AppealTypeArrive/edit/id=:id',
        name: 'EditAppealTypeArrive',
        component: () => import('@/views/appeal/info/AppealTypeArrive/edit.vue'),
        meta: {
            pageTitle: 'AppealTypeArrive',
            navActiveLink: 'AppealTypeArrive',
            breadcrumb: [
                ...breadcrumbDef,
                {
                    text: 'AppealTypeArrive',
                    active: true
                }
            ]
        }
    },
]
