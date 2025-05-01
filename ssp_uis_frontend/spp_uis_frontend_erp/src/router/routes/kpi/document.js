const breadcrumbDef = [
   {
      text: 'KpiGrating',
      to: 'KpiGrating'
   }
];

export default [
   {
      path: '/kpi/document/kpigrating',
      name: 'KpiGrating',
      component: () => import('@/views/kpi/document/kpigrating/index.vue'),
      meta: {
         pageTitle: 'KpiGrating',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'KpiGrating',
               active: true
            }
         ]
      }
   },
   {
      path: '/kpi/document/kpigrating/edit/id=:id',
      name: 'EditKpiGrating',
      component: () => import('@/views/kpi/document/kpigrating/edit.vue'),
      meta: {
         pageTitle: 'KpiGrating',
         navActiveLink: 'KpiGrating',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'KpiGrating',
               active: true
            }
         ]
      }
   },

   {
      path: '/kpi/document/kpiplanforemployee',
      name: 'KpiPlanForEmployee',
      component: () => import('@/views/kpi/document/kpiplanforemployee/index.vue'),
      meta: {
         pageTitle: 'KpiPlanForEmployee',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'KpiPlanForEmployee',
               active: true
            }
         ]
      }
   },
   {
      path: '/kpi/document/kpiplanforemployee/id=:id',
      name: 'EditKpiPlanForEmployee',
      component: () => import('@/views/kpi/document/kpiplanforemployee/edit.vue'),
      meta: {
         pageTitle: 'KpiPlanForEmployee',
         navActiveLink: 'KpiPlanForEmployee',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'KpiPlanForEmployee',
               active: true
            }
         ]
      }
   },
   {
      path: '/kpi/document/kpiratingemployee',
      name: 'KpiRatingEmployee',
      component: () => import('@/views/kpi/document/kpiratingemployee/index.vue'),
      meta: {
         pageTitle: 'KpiRatingEmployee',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'KpiRatingEmployee',
               active: true
            }
         ]
      }
   },
   {
      path: '/kpi/document/kpiratingemployee/id=:id',
      name: 'EditKpiRatingEmployee',
      component: () => import('@/views/kpi/document/kpiratingemployee/edit.vue'),
      meta: {
         pageTitle: 'KpiRatingEmployee',
         navActiveLink: 'KpiRatingEmployee',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'KpiRatingEmployee',
               active: true
            }
         ]
      }
   }
];
