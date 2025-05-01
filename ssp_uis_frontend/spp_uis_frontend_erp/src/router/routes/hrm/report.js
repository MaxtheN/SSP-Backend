const breadcrumbDef = [
   {
      text: 'Hrm',
      to: 'Hrm'
   },
   {
      text: 'Report',
      to: 'HrmReport'
   }
];
export default [
   {
      path: '/hrm/report',
      name: 'HrmReport',
      component: () => import('@/components/WRouterView.vue'),
      redirect: '/',
      children: [
         {
            path: '/',
            name: 'HrmReport',
            component: () => import('@/views/hrm/index.vue'),
            meta: {
               pageTitle: 'HrmReport',
               breadcrumb: [...breadcrumbDef]
            }
         },
         {
            path: '/hrm/report/getstaffcountreport',
            name: 'GetStaffCountReport',
            component: () => import('@/views/hrm/report/GetStaffCountReport/index.vue'),
            meta: {
               pageTitle: 'GetStaffCountReport',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'GetStaffCountReport',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/report/getstaffcountbygenderreport',
            name: 'GetStaffCountByGenderReport',
            component: () => import('@/views/hrm/report/getstaffcountbygenderreport/index.vue'),
            meta: {
               pageTitle: 'GetStaffCountByGenderReport',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'GetStaffCountByGenderReport',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/report/gethrmemployeeactivityreport',
            name: 'GetHrmEmployeeActivityReport',
            component: () => import('@/views/hrm/report/gethrmemployeeactivityreport/index.vue'),
            meta: {
               pageTitle: 'gethrmemployeeactivityreport',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'gethrmemployeeactivityreport',
                     active: true
                  }
               ]
            }
         },

         {
            path: '/hrm/report/EmployeeTurnstileReport',
            name: 'EmployeeTurnstileReport',
            component: () => import('@/views/hrm/report/EmployeeTurnstileReport/index.vue'),
            meta: {
               pageTitle: 'EmployeeTurnstileReport',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'EmployeeTurnstileReport',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/report/EmployeeTurnstileReportById',
            name: 'EmployeeTurnstileReportById',
            component: () => import('@/views/hrm/report/EmployeeTurnstileReportById/index.vue'),
            meta: {
               pageTitle: 'EmployeeTurnstileReportById',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'EmployeeTurnstileReportById',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/report/NewViewReport',
            name: 'NewViewReport',
            component: () => import('@/views/hrm/report/NewViewReport/index.vue'),
            meta: {
               pageTitle: 'NewViewReport',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'NewViewReport',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/report/gethrmemployeeactivityregionreport',
            name: 'GetHrmEmployeeActivityRegionReport',
            component: () => import('@/views/hrm/report/gethrmemployeeactivityregionreport/index.vue'),
            meta: {
               pageTitle: 'GetHrmEmployeeActivityRegionReport',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'GetHrmEmployeeActivityRegionReport',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/report/GetReportDocumentsForHrm',
            name: 'GetReportDocumentsForHrm',
            component: () => import('@/views/hrm/report/GetReportDocumentsForHrm/index.vue'),
            meta: {
               pageTitle: 'GetReportDocumentsForHrm',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'GetReportDocumentsForHrm',
                     active: true
                  }
               ]
            }
         }
      ]
   }
];
