export default [
   {
      path: '/dualedu/memshippaymentorder',
      name: 'MemshipPaymentOrder',
      component: () => import('@/views/dualedu/memshippaymentorder/index.vue'),
      meta: {
         pageTitle: 'MemshipPaymentOrder',
         breadcrumb: [
            {
               text: 'document'
            },
            {
               text: 'MemshipPaymentOrder',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/memshippaymentorder/edit/id=:id',
      name: 'EditMemshipPaymentOrder',
      component: () => import('@/views/dualedu/memshippaymentorder/edit.vue'),
      meta: {
         pageTitle: 'MemshipPaymentOrder',
         navActiveLink: 'MemshipPaymentOrder',
         breadcrumb: [
            {
               text: 'document'
            },
            {
               text: 'MemshipPaymentOrder',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/memshipyearlyplan',
      name: 'MemshipYearlyPlan',
      component: () => import('@/views/dualedu/memshipyearlyplan/index.vue'),
      meta: {
         pageTitle: 'MemshipYearlyPlan',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'MemshipYearlyPlan',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/memshipyearlyplan/edit/id=:id',
      name: 'EditMemshipYearlyPlan',
      component: () => import('@/views/dualedu/memshipyearlyplan/edit.vue'),
      meta: {
         pageTitle: 'MemshipYearlyPlan',
         navActiveLink: 'MemshipYearlyPlan',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'MemshipYearlyPlan',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipapplication',
      name: 'MemshipApplication',
      component: () => import('@/views/document/memshipapplication/index.vue'),
      meta: {
         pageTitle: 'MemshipApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/memshipapplication/edit/id=:id',
      name: 'EditMemshipApplication',
      component: () => import('@/views/document/memshipapplication/edit.vue'),
      meta: {
         pageTitle: 'MemshipApplication',
         navActiveLink: 'MemshipApplication',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'MemshipApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipapplication/view/id=:id',
      name: 'ViewMemshipApplication',
      component: () => import('@/views/document/memshipapplication/view.vue'),
      meta: {
         pageTitle: 'MemshipApplication',
         navActiveLink: 'MemshipApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipcontract',
      name: 'MemshipContract',
      component: () => import('@/views/document/memshipcontract/index.vue'),
      meta: {
         pageTitle: 'MemshipContract',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipContract',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipcontract/view/id=:id',
      name: 'ViewMemshipContract',
      component: () => import('@/views/document/memshipcontract/view.vue'),
      meta: {
         pageTitle: 'MemshipContract',
         navActiveLink: 'MemshipContract',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipContract',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipnewcontractor',
      name: 'MemshipNewContractor',
      component: () => import('@/views/document/memshipnewcontractor/index.vue'),
      meta: {
         pageTitle: 'MemshipNewContractor',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipNewContractor',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipnewcontractor/edit/id=:id',
      name: 'EditMemshipNewContractor',
      component: () => import('@/views/document/memshipnewcontractor/edit.vue'),
      meta: {
         pageTitle: 'MemshipNewContractor',
         navActiveLink: 'MemshipNewContractor',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipNewContractor',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipcertificate',
      name: 'MemshipCertificate',
      component: () => import('@/views/document/memshipcertificate/index.vue'),
      meta: {
         pageTitle: 'MemshipCertificate',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipCertificate',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/memshipcertificate/edit/id=:id',
      name: 'EditMemshipCertificate',
      component: () => import('@/views/document/memshipcertificate/edit.vue'),
      meta: {
         pageTitle: 'MemshipCertificate',
         navActiveLink: 'MemshipCertificate',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MemshipCertificate',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/debt',
      name: 'Debt',
      component: () => import('@/views/document/debt/index.vue'),
      meta: {
         pageTitle: 'Debt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Debt',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/debt/edit/id=:id',
      name: 'DebtEdit',
      component: () => import('@/views/document/debt/edit.vue'),
      meta: {
         pageTitle: 'Debt',
         navActiveLink: 'Debt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Debt',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/contractorrating',
      name: 'ContractorRating',
      component: () => import('@/views/info/contractorrating/index.vue'),
      meta: {
         pageTitle: 'ContractorRating',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ContractorRating',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/contractorrating/edit/id=:id',
      name: 'ContractorRatingEdit',
      component: () => import('@/views/info/contractorrating/edit.vue'),
      meta: {
         pageTitle: 'ContractorRating',
         navActiveLink: 'ContractorRating',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ContractorRating',
               active: true
            }
         ]
      }
   }
];
