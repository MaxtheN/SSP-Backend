export default [
    {
        path: '/report/bojxona',
        name: 'bojxona',
        component: () => import('@/views/report/bojxona/index.vue'),
        meta: {
            pageTitle: 'bojxona',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'bojxona',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/BojxonaImtiyozReportByContractor',
        name: 'BojxonaImtiyozReportByContractor',
        component: () => import('@/views/report/BojxonaImtiyozReportByContractor/index.vue'),
        meta: {
            pageTitle: 'BojxonaImtiyozReportByContractor',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'BojxonaImtiyozReportByContractor',
                    active: true
                }
            ]
        }
    },
]