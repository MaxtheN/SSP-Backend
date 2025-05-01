const defBread = [
    {
        text: 'yigirmaming_tadbirkor',
        to: 'Partnership'
    },
]

export default [
    {
        path: '/partnership',
        name: 'Partnership',
        component: () => import('@/views/partnership/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                {
                    text: 'yigirmaming_tadbirkor',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/application',
        name: 'PartnershipApplication',
        component: () => import('@/views/partnership/application/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PartnershipApplication',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/application/edit/id=:id',
        name: 'PartnershipApplicationEdit',
        component: () => import('@/views/partnership/application/edit.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PartnershipApplication',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/contract',
        name: 'PartnershipContract',
        component: () => import('@/views/partnership/contract/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PartnershipApplication',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/contract/edit/id=:id',
        name: 'PartnershipContractEdit',
        component: () => import('@/views/partnership/contract/edit.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PartnershipContract',
                    to: 'PartnershipContract'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/certificate',
        name: 'PartnershipCertificate',
        component: () => import('@/views/partnership/certificate/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PartnershipCertificate',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/certificate/edit/id=:id',
        name: 'PartnershipCertificateEdit',
        component: () => import('@/views/partnership/certificate/edit.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PartnershipCertificate',
                    to: 'PartnershipCertificate'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/stateassetapplication',
        name: 'StateAssetApplication',
        component: () => import('@/views/partnership/stateassetapplication/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'StateAssetApplication',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/stateassetapplication/edit/id=:id',
        name: 'EditStateAssetApplication',
        component: () => import('@/views/partnership/stateassetapplication/edit.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'StateAssetApplication',
                    to: 'StateAssetApplication'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/prtncreditdemand',
        name: 'PrtnCreditDemand',
        component: () => import('@/views/partnership/prtncreditdemand/index.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PrtnCreditDemand',
                    active: true
                }
            ]
        }
    },
    {
        path: '/partnership/prtncreditdemand/edit/id=:id',
        name: 'PrtnCreditDemandEdit',
        component: () => import('@/views/partnership/prtncreditdemand/edit.vue'),
        meta: {
            isAuth: true,
            breadcrumbs: [
                ...defBread,
                {
                    text: 'PrtnCreditDemand',
                    to: 'PrtnCreditDemand'
                },
                {
                    text: 'View',
                    active: true
                }
            ]
        }
    },
]