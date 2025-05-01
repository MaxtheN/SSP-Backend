export default [

    {
        path: '/report/getprtncreditdemandinfo',
        name: 'GetPrtnCreditDemandInfo',
        component: () => import('@/views/report/GetPrtnCreditDemandInfo/index.vue'),
        meta: {
            pageTitle: 'GetPrtnCreditDemandInfo',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'GetPrtnCreditDemandInfo',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/getprtncreditdemandinfobybank',
        name: 'GetPrtnCreditDemandInfoByBank',
        component: () => import('@/views/report/GetPrtnCreditDemandInfoByBank/index.vue'),
        meta: {
            pageTitle: 'GetPrtnCreditDemandInfoByBank',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'GetPrtnCreditDemandInfoByBank',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/bankreport',
        name: 'BankReport',
        component: () => import('@/views/report/bankreport/index.vue'),
        meta: {
            pageTitle: 'BankReport',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'BankReport',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/bankreport-second',
        name: 'BankReportSecond',
        component: () => import('@/views/report/bankreport/indexSecond.vue'),
        meta: {
            pageTitle: 'BankReportSecond',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'BankReportSecond',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/bankreport-third',
        name: 'BankReportThird',
        component: () => import('@/views/report/bankreport/indexThird.vue'),
        meta: {
            pageTitle: 'BankReportThird',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'BankReportThird',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/getbusinessactivitytypereport',
        name: 'GetBusinessActivityTypeReport',
        component: () => import('@/views/report/getbusinessactivitytypereport/index.vue'),
        meta: {
            pageTitle: 'GetBusinessActivityTypeReport',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'GetBusinessActivityTypeReport',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/getbusinessactivitytypereportbyregion',
        name: 'GetBusinessActivityTypeReportByRegion',
        component: () => import('@/views/report/getbusinessactivitytypereportbyregion/index.vue'),
        meta: {
            pageTitle: 'GetBusinessActivityTypeReportByRegion',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'GetBusinessActivityTypeReportByRegion',
                    active: true
                }
            ]
        }
    },
];
