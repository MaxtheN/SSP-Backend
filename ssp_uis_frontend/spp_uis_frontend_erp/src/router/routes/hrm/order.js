const breadcrumbDef = [
   {
      text: 'Hrm',
      to: 'Hrm'
   },
   {
      text: 'orders',
      to: 'HrmOrder'
   }
];

export default [
   {
      path: '/hrm/order',
      name: 'HrmOrderIndex',
      component: () => import('@/components/WRouterView.vue'),
      redirect: '/hrm/order/list',
      children: [
         {
            path: '/hrm/order/list',
            name: 'HrmOrder',
            component: () => import('@/views/hrm/index.vue'),
            meta: {
               pageTitle: 'orders',
               breadcrumb: [...breadcrumbDef]
            }
         },
         {
            path: '/hrm/order/documentheldforsign',
            name: 'DocumentHeldForSign',
            component: () => import('@/views/hrm/order/DocumentHeldForSign/index.vue'),
            meta: {
               pageTitle: 'DocumentHeldForSign',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'DocumentHeldForSign',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/appointemployee',
            name: 'AppointEmployee',
            component: () => import('@/views/hrm/order/appointemployee/index.vue'),
            meta: {
               pageTitle: 'appointemployee',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'appointemployee',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/appointemployee/edit/id=:id',
            name: 'EditAppointEmployee',
            component: () => import('@/views/hrm/order/appointemployee/edit.vue'),
            meta: {
               pageTitle: 'appointemployee',
               navActiveLink: 'AppointEmployee',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'appointemployee',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/appointemployee/view/id=:id',
            name: 'ViewAppointEmployee',
            component: () => import('@/views/hrm/order/appointemployee/view.vue'),
            meta: {
               pageTitle: 'appointemployee',
               navActiveLink: 'AppointEmployee',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'appointemployee',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/ordertosendbusinesstrip',
            name: 'OrderToSendBusinessTrip',
            component: () => import('@/views/hrm/order/ordertosendbusinesstrip/index.vue'),
            meta: {
               pageTitle: 'OrderToSendBusinessTrip',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'OrderToSendBusinessTrip',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/ordertosendbusinesstripsign',
            name: 'OrderToSendBusinessTripForSigner',
            component: () => import('@/views/hrm/order/ordertosendbusinesstrip/index.vue'),
            meta: {
               pageTitle: 'OrderToSendBusinessTrip',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'OrderToSendBusinessTrip',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/ordertosendbusinesstrip/edit/id=:id',
            name: 'EditOrderToSendBusinessTrip',
            component: () => import('@/views/hrm/order/ordertosendbusinesstrip/edit.vue'),
            meta: {
               pageTitle: 'OrderToSendBusinessTrip',
               navActiveLink: 'OrderToSendBusinessTrip',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'OrderToSendBusinessTrip',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/ordertosendbusinesstrip/view/id=:id',
            name: 'ViewOrderToSendBusinessTrip',
            component: () => import('@/views/hrm/order/ordertosendbusinesstrip/view.vue'),
            meta: {
               pageTitle: 'OrderToSendBusinessTrip',
               navActiveLink: 'OrderToSendBusinessTrip',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'OrderToSendBusinessTrip',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeeleaveorder',
            name: 'EmployeeLeaveOrder',
            component: () => import('@/views/hrm/order/employeeleaveorder/index.vue'),
            meta: {
               pageTitle: 'employeeleaveorder',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeeleaveorder',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeeleaveordersign',
            name: 'EmployeeLeaveOrderForSigner',
            component: () => import('@/views/hrm/order/employeeleaveorder/index.vue'),
            meta: {
               pageTitle: 'employeeleaveorder',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeeleaveorder',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeeleaveorder/edit/id=:id',
            name: 'EditEmployeeLeaveOrder',
            component: () => import('@/views/hrm/order/employeeleaveorder/edit.vue'),
            meta: {
               pageTitle: 'employeeleaveorder',
               navActiveLink: 'EmployeeLeaveOrder',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeeleaveorder',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeeleaveorder/view/id=:id',
            name: 'ViewEmployeeLeaveOrder',
            component: () => import('@/views/hrm/order/employeeleaveorder/view.vue'),
            meta: {
               pageTitle: 'employeeleaveorder',
               navActiveLink: 'EmployeeLeaveOrder',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeeleaveorder',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesendtrain',
            name: 'EmployeeSendTrain',
            component: () => import('@/views/hrm/order/employeesendtrain/index.vue'),
            meta: {
               pageTitle: 'employeesendtrain',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendtrain',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesendtrainsign',
            name: 'EmployeeSendTrainForSigner',
            component: () => import('@/views/hrm/order/employeesendtrain/index.vue'),
            meta: {
               pageTitle: 'employeesendtrain',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendtrain',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesendtrain/edit/id=:id',
            name: 'EditEmployeeSendTrain',
            component: () => import('@/views/hrm/order/employeesendtrain/edit.vue'),
            meta: {
               pageTitle: 'employeesendtrain',
               navActiveLink: 'EmployeeSendTrain',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendtrain',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesendtrain/view/id=:id',
            name: 'ViewEmployeeSendTrain',
            component: () => import('@/views/hrm/order/employeesendtrain/view.vue'),
            meta: {
               pageTitle: 'employeesendtrain',
               navActiveLink: 'EmployeeSendTrain',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendtrain',
                     active: true
                  }
               ]
            }
         },

         {
            path: '/hrm/order/employeesendstudy',
            name: 'EmployeeSendStudy',
            component: () => import('@/views/hrm/order/employeesendstudy/index.vue'),
            meta: {
               pageTitle: 'employeesendstudy',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendstudy',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesendstudy',
            name: 'EmployeeSendStudyForSigner',
            component: () => import('@/views/hrm/order/employeesendstudy/index.vue'),
            meta: {
               pageTitle: 'employeesendstudy',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendstudy',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesendstudy/edit/id=:id',
            name: 'EditEmployeeSendStudy',
            component: () => import('@/views/hrm/order/employeesendstudy/edit.vue'),
            meta: {
               pageTitle: 'employeesendstudy',
               navActiveLink: 'EmployeeSendStudy',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendstudy',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/employeesendstudy/view/id=:id',
            name: 'ViewEmployeeSendStudy',
            component: () => import('@/views/hrm/order/employeesendstudy/view.vue'),
            meta: {
               pageTitle: 'employeesendstudy',
               navActiveLink: 'EmployeeSendTrain',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'employeesendstudy',
                     active: true
                  }
               ]
            }
         },

         {
            path: '/hrm/order/workdayoff',
            name: 'WorkDayOff',
            component: () => import('@/views/hrm/order/workdayoff/index.vue'),
            meta: {
               pageTitle: 'WorkDayOff',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'WorkDayOff',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/workdayoffsign',
            name: 'WorkDayOffForSigner',
            component: () => import('@/views/hrm/order/workdayoff/index.vue'),
            meta: {
               pageTitle: 'WorkDayOff',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'WorkDayOff',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/workdayoff/edit/id=:id',
            name: 'EditWorkDayOff',
            component: () => import('@/views/hrm/order/workdayoff/edit.vue'),
            meta: {
               pageTitle: 'WorkDayOff',
               navActiveLink: 'WorkDayOff',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'WorkDayOff',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/workdayoff/view/id=:id',
            name: 'ViewWorkDayOff',
            component: () => import('@/views/hrm/order/workdayoff/view.vue'),
            meta: {
               pageTitle: 'WorkDayOff',
               navActiveLink: 'WorkDayOff',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'WorkDayOff',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/recallleave',
            name: 'RecallLeave',
            component: () => import('@/views/hrm/order/recallleave/index.vue'),
            meta: {
               pageTitle: 'RecallLeave',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'RecallLeave',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/recallleavesign',
            name: 'RecallLeaveForSigner',
            component: () => import('@/views/hrm/order/recallleave/index.vue'),
            meta: {
               pageTitle: 'RecallLeave',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'RecallLeave',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/recallleave/edit/id=:id',
            name: 'EditRecallLeave',
            component: () => import('@/views/hrm/order/recallleave/edit.vue'),
            meta: {
               pageTitle: 'RecallLeave',
               navActiveLink: 'RecallLeave',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'RecallLeave',
                     active: true
                  }
               ]
            }
         },

         {
            path: '/hrm/order/bolaparvarishi',
            name: 'Bolaparvarishi',
            component: () => import('@/views/hrm/order/bolaparvarishi/index.vue'),
            meta: {
               pageTitle: 'bolaparvarishi',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'bolaparvarishi',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/bolaparvarishi/edit/id=:id',
            name: 'EditBolaparvarishi',
            component: () => import('@/views/hrm/order/bolaparvarishi/edit.vue'),
            meta: {
               pageTitle: 'bolaparvarishi',
               navActiveLink: 'bolaparvarishi',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'bolaparvarishi',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/bolaparvarishi/view/id=:id',
            name: 'ViewBolaparvarishi',
            component: () => import('@/views/hrm/order/bolaparvarishi/view.vue'),
            meta: {
               pageTitle: 'bolaparvarishi',
               navActiveLink: 'bolaparvarishi',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'bolaparvarishi',
                     active: true
                  }
               ]
            }
         },

         // homiladarolk

         {
            path: '/hrm/order/homiladorliktatili',
            name: 'Homiladorliktatili',
            component: () => import('@/views/hrm/order/homiladorliktatili/index.vue'),
            meta: {
               pageTitle: 'Homiladorliktatili',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'Homiladorliktatili',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/homiladorliktatili/edit/id=:id',
            name: 'EditHomiladorliktatili',
            component: () => import('@/views/hrm/order/homiladorliktatili/edit.vue'),
            meta: {
               pageTitle: 'Homiladorliktatili',
               navActiveLink: 'Homiladorliktatili',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'Homiladorliktatili',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/homiladorliktatili/view/id=:id',
            name: 'ViewHomiladorliktatili',
            component: () => import('@/views/hrm/order/homiladorliktatili/view.vue'),
            meta: {
               pageTitle: 'Homiladorliktatili',
               navActiveLink: 'homiladorliktatili',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'Homiladorliktatili',
                     active: true
                  }
               ]
            }
         },

         {
            path: '/hrm/order/recallleave/view/id=:id',
            name: 'ViewRecallLeave',
            component: () => import('@/views/hrm/order/recallleave/view.vue'),
            meta: {
               pageTitle: 'RecallLeave',
               navActiveLink: 'RecallLeave',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'RecallLeave',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/chastisement',
            name: 'Chastisement',
            component: () => import('@/views/hrm/order/chastisement/index.vue'),
            meta: {
               pageTitle: 'Chastisement',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'Chastisement',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/chastisementsign',
            name: 'ChastisementForSigner',
            component: () => import('@/views/hrm/order/chastisement/index.vue'),
            meta: {
               pageTitle: 'Chastisement',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'Chastisement',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/chastisement/edit/id=:id',
            name: 'EditChastisement',
            component: () => import('@/views/hrm/order/chastisement/edit.vue'),
            meta: {
               pageTitle: 'Chastisement',
               navActiveLink: 'Chastisement',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'Chastisement',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/chastisement/view/id=:id',
            name: 'ViewChastisement',
            component: () => import('@/views/hrm/order/chastisement/view.vue'),
            meta: {
               pageTitle: 'Chastisement',
               navActiveLink: 'Chastisement',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'Chastisement',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/tempcalckind',
            name: 'TempCalcKind',
            component: () => import('@/views/hrm/order/tempcalckind/index.vue'),
            meta: {
               pageTitle: 'tempcalckind',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'tempcalckind',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/tempcalckindsign',
            name: 'TempCalcKindForSigner',
            component: () => import('@/views/hrm/order/tempcalckind/index.vue'),
            meta: {
               pageTitle: 'tempcalckind',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'tempcalckind',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/tempcalckind/edit/id=:id',
            name: 'EditTempCalcKind',
            component: () => import('@/views/hrm/order/tempcalckind/edit.vue'),
            meta: {
               pageTitle: 'tempcalckind',
               navActiveLink: 'TempCalcKind',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'tempcalckind',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/tempcalckind/view/id=:id',
            name: 'ViewTempCalcKind',
            component: () => import('@/views/hrm/order/tempcalckind/view.vue'),
            meta: {
               pageTitle: 'tempcalckind',
               navActiveLink: 'TempCalcKind',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'tempcalckind',
                     active: true
                  }
               ]
            }
         },
         {
            path: '/hrm/order/tempcalckind/salom',
            name: 'ViewTempCalcKind',
            component: () => import('@/views/components/hrm/EmployeeSickLeaveList.vue'),
            meta: {
               pageTitle: 'tempcalckind',
               navActiveLink: 'TempCalcKind',
               breadcrumb: [
                  ...breadcrumbDef,
                  {
                     text: 'tempcalckind',
                     active: true
                  }
               ]
            }
         }
      ]
   }
];
