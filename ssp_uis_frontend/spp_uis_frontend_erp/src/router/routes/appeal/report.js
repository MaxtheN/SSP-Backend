const breadcrumbDef = [
   {
      text: 'Appeal',
      to: 'Appeal'
   }
];

export default [
   {
      path: '/appeal/report/GetReportByAppealType',
      name: 'GetReportByAppealType',
      component: () => import('@/views/appeal/report/GetReportByAppealType.vue'),
      meta: {
         pageTitle: 'GetReportByAppealType',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'GetReportByAppealType',
               active: true
            }
         ]
      }
   },
   {
      path: '/appeal/report/AppealApplicationReport',
      name: 'AppealApplicationReport',
      component: () => import('@/views/appeal/report/AppealApplicationReport.vue'),
      meta: {
         pageTitle: 'AppealApplicationReport',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'AppealApplicationReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/appeal/report/CallCenterAppealReportByOkedType',
      name: 'CallCenterAppealReportByOkedType',
      component: () => import('@/views/appeal/report/CallCenterAppealReportByOkedType.vue'),
      meta: {
         pageTitle: 'CallCenterAppealReportByOkedType',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'CallCenterAppealReportByOkedType',
               active: true
            }
         ]
      }
   },
   {
      path: '/appeal/report/GetCallCenterByRegion',
      name: 'GetCallCenterByRegion',
      component: () => import('@/views/appeal/report/GetCallCenterByRegion.vue'),
      meta: {
         pageTitle: 'GetCallCenterByRegion',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'GetCallCenterByRegion',
               active: true
            }
         ]
      }
   },
   {
      path: '/appeal/report/GetCallCenterReportByWeek',
      name: 'GetCallCenterReportByWeek',
      component: () => import('@/views/appeal/report/GetCallCenterReportByWeek.vue'),
      meta: {
         pageTitle: 'GetCallCenterReportByWeek',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'GetCallCenterReportByWeek',
               active: true
            }
         ]
      }
   }
];
