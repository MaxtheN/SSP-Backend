const breadcrumbDef = [
   {
      text: 'Hrm',
      to: 'Hrm'
   },
   {
      text: 'document',
      to: 'HrmDocument'
   }
];

export default [
   {
      path: '/hrm/document',
      name: 'HrmDocument',
      component: () => import('@/components/WRouterView.vue'),
      redirect: '/',
      children: [
         {
            path: '/',
            name: 'HrmDocument',
            component: () => import('@/views/hrm/index.vue'),
            meta: {
               pageTitle: 'HrmDocument',
               breadcrumb: [...breadcrumbDef]
            }
         },
         {
            path: '/hrm/document/staffing',
            name: 'Staffing',
            component: () => import('@/views/hrm/document/staffing/index.vue'),
            meta: {
               pageTitle: 'staffing',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'staffing',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/staffingHeader',
            name: 'StaffingHeader',
            component: () => import('@/views/hrm/document/staffing/staffingHeader.vue'),
            meta: {
               pageTitle: 'staffingHeader',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'staffingHeader',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/staffingOwnOrg',
            name: 'StaffingOwnOrg',
            component: () => import('@/views/hrm/document/staffing/staffingOwnOrg.vue'),
            meta: {
               pageTitle: 'StaffingOwnOrg',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'StaffingOwnOrg',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/staffing/edit/id=:id',
            name: 'EditStaffing',
            component: () => import('@/views/hrm/document/staffing/edit.vue'),
            meta: {
               pageTitle: 'staffing',
               navActiveLink: 'Staffing',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'staffing',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/getstaffingsinglereport',
            name: 'GetStaffingSingleReport',
            component: () => import('@/views/hrm/document/GetStaffingSingleReport/index.vue'),
            meta: {
               pageTitle: 'GetStaffingSingleReport',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'GetStaffingSingleReport',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/getstaffingsinglereportforparent',
            name: 'GetStaffingSingleReportForParent',
            component: () => import('@/views/hrm/document/GetStaffingSingleReportForParent/index.vue'),
            meta: {
               pageTitle: 'GetStaffingSingleReportForParent',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'GetStaffingSingleReportForParent',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/employeemanage',
            name: 'EmployeeManage',
            component: () => import('@/views/hrm/document/employeemanage/index.vue'),
            meta: {
               pageTitle: 'employeeManage',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeeManage',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/timesheet',
            name: 'Timesheet',
            component: () => import('@/views/hrm/document/timesheet/index.vue'),
            meta: {
               pageTitle: 'timesheet',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'timesheet',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/timesheet/edit/id=:id',
            name: 'EditTimesheet',
            component: () => import('@/views/hrm/document/timesheet/edit.vue'),
            meta: {
               pageTitle: 'timesheet',
               navActiveLink: 'Timesheet',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'timesheet',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/CandidatesConfirmation',
            name: 'CandidatesConfirmation',
            component: () => import('@/views/hrm/document/CandidatesConfirmation/index.vue'),
            meta: {
               pageTitle: 'CandidatesConfirmation',
               breadcrumb: [
                  {
                     text: 'document'
                  },
                  {
                     text: 'CandidatesConfirmation',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/CandidatesConfirmation/edit/id=:id',
            name: 'EditCandidatesConfirmation',
            component: () => import('@/views/hrm/document/CandidatesConfirmation/edit.vue'),
            meta: {
               pageTitle: 'CandidatesConfirmation',
               breadcrumb: [
                  {
                     text: 'document'
                  },
                  {
                     text: 'CandidatesConfirmation',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesickleave',
            name: 'EmployeeSickLeave',
            component: () => import('@/views/hrm/order/employeesickleave/index.vue'),
            meta: {
               pageTitle: 'employeesickleave',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesickleave',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesickleave/edit/id=:id',
            name: 'EditEmployeeSickLeave',
            component: () => import('@/views/hrm/order/employeesickleave/edit.vue'),
            meta: {
               pageTitle: 'employeesickleave',
               navActiveLink: 'EmployeeSickLeave',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesickleave',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/employeemissedday',
            name: 'EmployeeMissedDay',
            component: () => import('@/views/hrm/document/EmployeeMissedDay/index.vue'),
            meta: {
               pageTitle: 'EmployeeMissedDay',
               breadcrumb: [
                  {
                     text: 'document'
                  },
                  {
                     text: 'EmployeeMissedDay',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/document/employeemissedday/edit/id=:id',
            name: 'EditEmployeeMissedDay',
            component: () => import('@/views/hrm/document/EmployeeMissedDay/edit.vue'),
            meta: {
               pageTitle: 'EmployeeMissedDay',
               breadcrumb: [
                  {
                     text: 'document'
                  },
                  {
                     text: 'EmployeeMissedDay',
                     active: true
                  }
               ]
            }
         }
      ]
   }
];
