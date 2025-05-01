export default [
    {
        path: '/report/soliq',
        name: 'soliq',
        component: () => import('@/views/report/soliq/index.vue'),
        meta: {
            pageTitle: 'soliq',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'soliq',
                    active: true
                }
            ]
        }
    },
    {
        path: '/report/gettaxcreditreport',
        name: 'GetTaxCreditReport',
        component: () => import('@/views/report/GetTaxCreditReport/index.vue'),
        meta: {
           pageTitle: 'GetTaxCreditReport',
           breadcrumb: [
              {
                 text: 'Report'
              },
              {
                 text: 'GetTaxCreditReport',
                 active: true
              }
           ]
        }
     },
     {
        path: '/report/getsoliqreportbycontractor',
        name: 'GetSoliqReportByContractor',
        component: () => import('@/views/report/GetSoliqReportByContractor/index.vue'),
        meta: {
           pageTitle: 'GetSoliqReportByContractor',
           breadcrumb: [
              {
                 text: 'Report'
              },
              {
                 text: 'GetSoliqReportByContractor',
                 active: true
              }
           ]
        }
     },
];
