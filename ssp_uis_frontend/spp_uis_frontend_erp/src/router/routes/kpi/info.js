const breadcrumbDef = [
   {
      text: 'Indicator',
      to: 'Indicator'
   },
   {
      text: 'IndicatorDepartment',
      to: 'IndicatorDepartment'
   }
];

export default [
   {
      path: '/kpi/info/indicator',
      name: 'Indicator',
      component: () => import('@/views/kpi/info/indicator/index.vue'),
      meta: {
         pageTitle: 'Indicator',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'Indicator',
               active: true
            }
         ]
      }
   },
   {
      path: '/kpi/info/indicator/edit/id=:id',
      name: 'EditIndicator',
      component: () => import('@/views/kpi/info/indicator/edit.vue'),
      meta: {
         pageTitle: 'Indicator',
         navActiveLink: 'Indicator',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'Indicator',
               active: true
            }
         ]
      }
   },
   {
      path: '/kpi/info/indicatordepartment',
      name: 'IndicatorDepartment',
      component: () => import('@/views/kpi/info/indicatordepartment/index.vue'),
      meta: {
         pageTitle: 'IndicatorDepartment',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'IndicatorDepartment',
               active: true
            }
         ]
      }
   },
   {
      path: '/kpi/info/indicatordepartment/edit/id=:id',
      name: 'EditIndicatorDepartment',
      component: () => import('@/views/kpi/info/indicatordepartment/edit.vue'),
      meta: {
         pageTitle: 'IndicatorDepartment',
         navActiveLink: 'IndicatorDepartment',
         breadcrumb: [
            ...breadcrumbDef,
            {
               text: 'IndicatorDepartment',
               active: true
            }
         ]
      }
   }
];
