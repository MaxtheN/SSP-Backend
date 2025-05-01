export default [
    {
        path: '/dualedu/subsidyrequest',
        name: 'SubsidyRequest',
        component: () => import('@/views/dualedu/subsidyrequest/index.vue'),
        meta: {
            pageTitle: 'SubsidyRequest',
            breadcrumb: [
                {
                    text: 'document'
                },
                {
                    text: 'SubsidyRequest',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dualedu/subsidyrequest/view/id=:id',
        name: 'ViewSubsidyRequest',
        component: () => import('@/views/dualedu/subsidyrequest/view.vue'),
        meta: {
            pageTitle: 'SubsidyRequest',
            navActiveLink: 'SubsidyRequest',
            breadcrumb: [
                {
                    text: 'document'
                },
                {
                    text: 'SubsidyRequest',
                    active: true
                }
            ]
        }
    },
]