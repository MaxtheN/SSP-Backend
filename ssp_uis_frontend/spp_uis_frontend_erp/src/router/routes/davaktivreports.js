export default [
    {
        path: '/report/GetStateAssetApplicationReport',
        name: 'GetStateAssetApplicationReport',
        component: () => import('@/views/report/GetStateAssetApplicationReport/index.vue'),
        meta: {
            pageTitle: 'GetStateAssetApplicationReport',
            breadcrumb: [
                {
                    text: 'Report'
                },
                {
                    text: 'GetStateAssetApplicationReport',
                    active: true
                }
            ]
        }
    },
]