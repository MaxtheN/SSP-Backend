export default [
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
   }
];
