export default [
    {
        path: '/dual',
        name: 'dual',
        component: () => import('@/views/dual/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dual/application',
        name: 'DualApplication',
        component: () => import('@/views/dual/application/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    to: 'dual'
                },
                {
                    text: 'DualApplication',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dual/application/edit/id=:id',
        name: 'DualApplicationEdit',
        component: () => import('@/views/dual/application/edit.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    to: 'dual'
                },
                {
                    text: 'DualApplication',
                    to: 'DualApplication'
                },
                {
                    text: 'edit',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dual/application/view/id=:id',
        name: 'DualApplicationView',
        component: () => import('@/views/dual/application/view.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    to: 'dual'
                },
                {
                    text: 'DualApplication',
                    to: 'DualApplication'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dual/subsidyrequest',
        name: 'SubsidyRequest',
        component: () => import('@/views/dual/SubsidyRequest/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    to: 'dual'
                },
                {
                    text: 'SubsidyRequest',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dual/subsidyrequest/edit/id=:id',
        name: 'SubsidyRequestEdit',
        component: () => import('@/views/dual/SubsidyRequest/edit.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    to: 'dual'
                },
                {
                    text: 'SubsidyRequest',
                    to: 'SubsidyRequest'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dual/contract',
        name: 'DualContract',
        component: () => import('@/views/dual/contract/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    to: 'dual'
                },
                {
                    text: 'DualContract',
                    active: true
                }
            ]
        }
    },
    {
        path: '/dual/contract/view/id=:id',
        name: 'DualContractView',
        component: () => import('@/views/dual/contract/view.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'Dual',
                    to: 'dual'
                },
                {
                    text: 'DualContract',
                    to: 'DualContract'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    }
];
