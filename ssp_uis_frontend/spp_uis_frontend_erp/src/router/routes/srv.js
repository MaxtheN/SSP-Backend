export default [
   {
      path: '/srv/serviceapplication',
      name: 'ServiceApplication',
      component: () => import('@/views/srv/ServiceApplication/index.vue'),
      meta: {
         pageTitle: 'ServiceApplication',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/serviceapplication/view/id=:id',
      name: 'ViewServiceApplication',
      component: () => import('@/views/srv/ServiceApplication/view.vue'),
      meta: {
         pageTitle: 'ServiceApplication',
         navActiveLink: 'ServiceApplication',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/servicecontract',
      name: 'ServiceContract',
      component: () => import('@/views/srv/ServiceContract/index.vue'),
      meta: {
         pageTitle: 'ServiceContract',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceContract',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/servicecontract/edit/id=:id',
      name: 'EditServiceContract',
      component: () => import('@/views/srv/ServiceContract/edit.vue'),
      meta: {
         pageTitle: 'ServiceContract',
         navActiveLink: 'ServiceContract',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceContract',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/servicedeed',
      name: 'ServiceDeed',
      component: () => import('@/views/srv/ServiceDeed/index.vue'),
      meta: {
         pageTitle: 'ServiceDeed',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceDeed',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/servicedeed/edit/id=:id',
      name: 'EditServiceDeed',
      component: () => import('@/views/srv/ServiceDeed/edit.vue'),
      meta: {
         pageTitle: 'ServiceDeed',
         navActiveLink: 'ServiceDeed',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceDeed',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/serviceprice',
      name: 'ServicePrice',
      component: () => import('@/views/srv/ServicePrice/index.vue'),
      meta: {
         pageTitle: 'ServicePrice',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServicePrice',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/serviceprice/edit/id=:id',
      name: 'EditServicePrice',
      component: () => import('@/views/srv/ServicePrice/edit.vue'),
      meta: {
         pageTitle: 'ServicePrice',
         navActiveLink: 'ServicePrice',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServicePrice',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/needchamberservicegroup',
      name: 'NeedChamberServiceGroup',
      component: () => import('@/views/srv/needchamberservicegroup/index.vue'),
      meta: {
         pageTitle: 'NeedChamberServiceGroup',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'NeedChamberServiceGroup',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/needchamberservicegroup/edit/id=:id',
      name: 'EditNeedChamberServiceGroup',
      component: () => import('@/views/srv/needchamberservicegroup/edit.vue'),
      meta: {
         pageTitle: 'NeedChamberServiceGroup',
         navActiveLink: 'NeedChamberServiceGroup',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'NeedChamberServiceGroup',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/completeservice',
      name: 'CompleteService',
      component: () => import('@/views/srv/completeservice/index.vue'),
      meta: {
         pageTitle: 'CompleteService',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'CompleteService',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/completeservice/edit/id=:id',
      name: 'EditCompleteService',
      component: () => import('@/views/srv/completeservice/edit.vue'),
      meta: {
         pageTitle: 'CompleteService',
         navActiveLink: 'CompleteService',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'CompleteService',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/SrvYearlyPlan',
      name: 'SrvYearlyPlan',
      component: () => import('@/views/srv/SrvYearlyPlan/index.vue'),
      meta: {
         pageTitle: 'SrvYearlyPlan',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'SrvYearlyPlan',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/SrvYearlyPlan/edit/id=:id',
      name: 'EditSrvYearlyPlan',
      component: () => import('@/views/srv/SrvYearlyPlan/edit.vue'),
      meta: {
         pageTitle: 'SrvYearlyPlan',
         navActiveLink: 'SrvYearlyPlan',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'SrvYearlyPlan',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/ServiceInfo',
      name: 'ServiceInfo',
      component: () => import('@/views/srv/ServiceInfo/index.vue'),
      meta: {
         pageTitle: 'ServiceInfo',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceInfo',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/ServiceInfoNew',
      name: 'ServiceInfoNew',
      component: () => import('@/views/srv/ServiceInfoNew/index.vue'),
      meta: {
         pageTitle: 'ServiceInfoNew',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'ServiceInfoNew',
               active: true
            }
         ]
      }
   },
   {
      path: '/srv/FinanceIntegration',
      name: 'FinanceIntegration',
      component: () => import('@/views/srv/FinanceIntegration/index.vue'),
      meta: {
         pageTitle: 'FinanceIntegration',
         breadcrumb: [
            {
               text: 'Srv'
            },
            {
               text: 'FinanceIntegration',
               active: true
            }
         ]
      }
   }
];
