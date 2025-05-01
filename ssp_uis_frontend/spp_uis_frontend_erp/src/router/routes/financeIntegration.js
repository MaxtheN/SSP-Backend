export default [
    {
        path: '/financeIntegration/GetPayDocsByAccEqualsAndBankDateBetween',
        name: 'GetPayDocsByAccEqualsAndBankDateBetween',
        component: () => import('@/views/financeIntegration/GetPayDocsByAccEqualsAndBankDateBetween.vue'),
        meta: {
            pageTitle: 'GetPayDocsByAccEqualsAndBankDateBetween',
            breadcrumb: [
                {
                    text: 'report'
                },
                {
                    text: 'GetPayDocsByAccEqualsAndBankDateBetween',
                    active: true
                }
            ]
        }
    }]