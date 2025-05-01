export default [
   // Application
   {
      path: '/document/application',
      name: 'Application',
      component: () => import('@/views/document/application/index.vue'),
      meta: {
         pageTitle: 'Application',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Application',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/stateassetapplication',
      name: 'StateAssetApplication',
      component: () => import('@/views/document/stateassetapplication/index.vue'),
      meta: {
         pageTitle: 'StateAssetApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'StateAssetApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/stateassetapplication/edit/id=:id',
      name: 'EditStateAssetApplication',
      component: () => import('@/views/document/stateassetapplication/edit.vue'),
      meta: {
         pageTitle: 'StateAssetApplication',
         navActiveLink: 'StateAssetApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'StateAssetApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/application/edit/id=:id',
      name: 'EditApplication',
      component: () => import('@/views/document/application/edit.vue'),
      meta: {
         pageTitle: 'Application',
         navActiveLink: 'Application',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Application',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/applicationcustom',
      name: 'ApplicationCustom',
      component: () => import('@/views/document/applicationcustom/index.vue'),
      meta: {
         pageTitle: 'ApplicationCustom',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ApplicationCustom',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/applicationcustom/edit/id=:id',
      name: 'EditApplicationCustom',
      component: () => import('@/views/document/applicationcustom/edit.vue'),
      meta: {
         pageTitle: 'ApplicationCustom',
         navActiveLink: 'ApplicationCustom',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ApplicationCustom',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/ExecutionApplication',
      name: 'ExecutionApplication',
      component: () => import('@/views/document/executionapplication/index.vue'),
      meta: {
         pageTitle: 'ExecutionApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ExecutionApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/ExecutionApplication/edit/id=:id',
      name: 'EditExecutionApplication',
      component: () => import('@/views/document/executionapplication/edit.vue'),
      meta: {
         pageTitle: 'ExecutionApplication',
         navActiveLink: 'ExecutionApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ExecutionApplication',
               active: true
            }
         ]
      }
   },
   // PrtnContract
   {
      path: '/document/prtncontract',
      name: 'PrtnContract',
      component: () => import('@/views/document/prtncontract/index.vue'),
      meta: {
         pageTitle: 'prtncontract',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'prtncontract',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/prtncontract/view/id=:id',
      name: 'ViewPrtnContract',
      component: () => import('@/views/document/prtncontract/view.vue'),
      meta: {
         pageTitle: 'prtncontract',
         navActiveLink: 'PrtnContract',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'prtncontract',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/prtncontract/edit/id=:id',
      name: 'EditPrtnContract',
      component: () => import('@/views/document/prtncontract/edit.vue'),
      meta: {
         pageTitle: 'prtncontract',
         navActiveLink: 'PrtnContract',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'prtncontract',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/monoaplication',
      name: 'monoaplication',
      component: () => import('@/views/document/manoaplication/index.vue'),
      meta: {
         pageTitle: 'monoaplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'monoaplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/monoaplication/view/id=:id',
      name: 'monoaplicationView',
      component: () => import('@/views/document/manoaplication/view.vue'),
      meta: {
         pageTitle: 'monoaplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'manoaplication',
               active: true
            }
         ]
      }
   },
   // PrtnCertificate
   {
      path: '/document/prtncertificate',
      name: 'PrtnCertificate',
      component: () => import('@/views/document/prtncertificate/index.vue'),
      meta: {
         pageTitle: 'prtncertificate',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'prtncertificate',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/prtncertificate/view/id=:id',
      name: 'ViewPrtnCertificate',
      component: () => import('@/views/document/prtncertificate/view.vue'),
      meta: {
         pageTitle: 'prtncertificate',
         navActiveLink: 'PrtnCertificate',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'prtncertificate',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/prtncertificate/edit/id=:id',
      name: 'EditPrtnCertificate',
      component: () => import('@/views/document/prtncertificate/edit.vue'),
      meta: {
         pageTitle: 'prtncertificate',
         navActiveLink: 'PrtnCertificate',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'prtncertificate',
               active: true
            }
         ]
      }
   },

   {
      path: '/document/contractorcategorycriterion',
      name: 'ContractorCategoryCriterion',
      component: () => import('@/views/document/contractorcategorycriterion/index.vue'),
      meta: {
         pageTitle: 'ContractorCategoryCriterion',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ContractorCategoryCriterion',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/contractorcategorycriterion/edit/id=:id',
      name: 'EditContractorCategoryCriterion',
      component: () => import('@/views/document/contractorcategorycriterion/edit.vue'),
      meta: {
         pageTitle: 'ContractorCategoryCriterion',
         navActiveLink: 'ContractorCategoryCriterion',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ContractorCategoryCriterion',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/prtncreditdemand',
      name: 'PrtnCreditDemand',
      component: () => import('@/views/document/prtncreditdemand/index.vue'),
      meta: {
         pageTitle: 'PrtnCreditDemand',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'PrtnCreditDemand',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/prtncreditdemand/view/id=:id',
      name: 'ViewPrtnCreditDemand',
      component: () => import('@/views/document/prtncreditdemand/view.vue'),
      meta: {
         pageTitle: 'PrtnCreditDemand',
         navActiveLink: 'PrtnCreditDemand',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'PrtnCreditDemand',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/applicationforcourt',
      name: 'ApplicationForCourt',
      component: () => import('@/views/document/applicationforcourt/index.vue'),
      meta: {
         pageTitle: 'ApplicationForCourt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ApplicationForCourt',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/applicationforcourt/edit/id=:id',
      name: 'EditApplicationForCourt',
      component: () => import('@/views/document/applicationforcourt/edit.vue'),
      meta: {
         pageTitle: 'ApplicationForCourt',
         navActiveLink: 'ApplicationForCourt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ApplicationForCourt',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/applicationforcourt/view/id=:id',
      name: 'ViewApplicationForCourt',
      component: () => import('@/views/document/applicationforcourt/view.vue'),
      meta: {
         pageTitle: 'ApplicationForCourt',
         navActiveLink: 'ApplicationForCourt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ApplicationForCourt',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/additionalagreement/edit/id=:id',
      name: 'EditAdditionalAgreement',
      component: () => import('@/views/document/additionalagreement/edit.vue'),
      meta: {
         pageTitle: 'AdditionalAgreement',
         navActiveLink: 'AdditionalAgreement',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'AdditionalAgreement',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/additionalagreement/view/id=:id',
      name: 'ViewAdditionalAgreement',
      component: () => import('@/views/document/additionalagreement/view.vue'),
      meta: {
         pageTitle: 'AdditionalAgreement',
         navActiveLink: 'AdditionalAgreement',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'AdditionalAgreement',
               active: true
            }
         ]
      }
   }
];
