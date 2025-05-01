export default [
   {
      path: '/document/claimapplication2',
      name: 'ClaimApplication2',
      component: () => import('@/views/document/claimapplication2/index.vue'),
      meta: {
         pageTitle: 'ClaimApplication2',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ClaimApplication2',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/claimapplication2/edit/id=:id',
      name: 'EditClaimApplication2',
      component: () => import('@/views/document/claimapplication2/edit.vue'),
      meta: {
         pageTitle: 'ClaimApplication2',
         navActiveLink: 'ClaimApplication2',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ClaimApplication2',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/claimapplication',
      name: 'ClaimApplication',
      component: () => import('@/views/document/claimapplication/index.vue'),
      meta: {
         pageTitle: 'ClaimApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ClaimApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/claimapplication/edit/id=:id',
      name: 'EditClaimApplication',
      component: () => import('@/views/document/claimapplication/edit.vue'),
      meta: {
         pageTitle: 'ClaimApplication',
         navActiveLink: 'ClaimApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'ClaimApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/mediationplan/ijrochitayinlash',
      name: 'Ijrochitayinlash',
      component: () => import('@/views/document/mediationplan/ijrochitayinlash.vue'),
      meta: {
         pageTitle: 'Ijrochi tayinlash',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Ijrochi tayinlash',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/mediationplan',
      name: 'MediationPlan',
      component: () => import('@/views/document/mediationplan/index.vue'),
      meta: {
         pageTitle: 'MediationPlan',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MediationPlan',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/mediationplan/edit/id=:id',
      name: 'EditMediationPlan',
      component: () => import('@/views/document/mediationplan/edit.vue'),
      meta: {
         pageTitle: 'MediationPlan',
         navActiveLink: 'MediationPlan',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MediationPlan',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/mediationplan/view/id=:id',
      name: 'ViewMediationPlan',
      component: () => import('@/views/document/mediationplan/view.vue'),
      meta: {
         pageTitle: 'MediationPlan',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'MediationPlan',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/mediation',
      name: 'Mediation',
      component: () => import('@/views/document/mediation/index.vue'),
      meta: {
         pageTitle: 'Mediation',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Mediation',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/mediation/edit/id=:id',
      name: 'EditMediation',
      component: () => import('@/views/document/mediation/edit.vue'),
      meta: {
         pageTitle: 'Mediation',
         navActiveLink: 'Mediation',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Mediation',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/mediation/view/id=:id',
      name: 'ViewMediation',
      component: () => import('@/views/document/mediation/view.vue'),
      meta: {
         pageTitle: 'Mediation',
         navActiveLink: 'Mediation',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Mediation',
               active: true
            }
         ]
      }
   }
];
