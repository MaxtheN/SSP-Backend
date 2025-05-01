export default [
   {
      path: '/document/arbitrationcourt',
      name: 'arbitrationcourt',
      component: () => import('@/views/arbitrationcourt/document/arbitrationcourtapplication/index.vue'),
      meta: {
         pageTitle: 'arbitrationcourt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationcourt',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/arbitrationcourt/id=:id',
      name: 'EditArbitrationcourt',
      component: () => import('@/views/arbitrationcourt/document/arbitrationcourtapplication/edit.vue'),
      meta: {
         pageTitle: 'arbitrationcourt',
         navActiveLink: 'arbitrationcourt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationcourt',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/arbitrationcourt/id=:id',
      name: 'ViewArbitrationcourt',
      component: () => import('@/views/arbitrationcourt/document/arbitrationcourtapplication/view.vue'),
      meta: {
         pageTitle: 'arbitrationcourt',
         navActiveLink: 'arbitrationcourt',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationcourt',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/arbitrationdiscussion',
      name: 'arbitrationdiscussion',
      component: () => import('@/views/arbitrationcourt/document/arbitrationdiscussion/index.vue'),
      meta: {
         pageTitle: 'arbitrationdiscussion',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationdiscussion',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/arbitrationdiscussion/id=:id',
      name: 'EditArbitrationDiscussion',
      component: () => import('@/views/arbitrationcourt/document/arbitrationdiscussion/edit.vue'),
      meta: {
         pageTitle: 'arbitrationdiscussion',
         navActiveLink: 'arbitrationdiscussion',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationdiscussion',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/arbitrationdelay',
      name: 'arbitrationdelay',
      component: () => import('@/views/arbitrationcourt/document/arbitrationdelay/index.vue'),
      meta: {
         pageTitle: 'arbitrationdelay',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationdelay',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/arbitrationdelay/id=:id',
      name: 'EditArbitrationDelay',
      component: () => import('@/views/arbitrationcourt/document/arbitrationdelay/edit.vue'),
      meta: {
         pageTitle: 'arbitrationdelay',
         navActiveLink: 'arbitrationdelay',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationdelay',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/arbitrationresult',
      name: 'arbitrationresult',
      component: () => import('@/views/arbitrationcourt/document/arbitrationresult/index.vue'),
      meta: {
         pageTitle: 'arbitrationresult',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationresult',
               active: true
            }
         ]
      }
   },

   {
      path: '/document/arbitrationresult/id=:id',
      name: 'EditArbitrationResult',
      component: () => import('@/views/arbitrationcourt/document/arbitrationresult/edit.vue'),
      meta: {
         pageTitle: 'arbitrationresult',
         navActiveLink: 'arbitrationresult',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'arbitrationresult',
               active: true
            }
         ]
      }
   },

   {
      path: '/info/arbitrationjudge',
      name: 'arbitrationjudge',
      component: () => import('@/views/arbitrationcourt/info/arbitrationjudge/index.vue'),
      meta: {
         pageTitle: 'arbitrationjudge',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'arbitrationjudge',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/arbitrationjudge/id=:id',
      name: 'EditArbitrationjudge',
      component: () => import('@/views/arbitrationcourt/info/arbitrationjudge/edit.vue'),
      meta: {
         pageTitle: 'arbitrationjudge',
         navActiveLink: 'arbitrationjudge',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'arbitrationjudge',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetArbitrationApplicationReport',
      name: 'GetArbitrationApplicationReport',
      component: () => import('@/views/arbitrationcourt/Report/GetArbitrationApplicationReport.vue'),
      meta: {
         pageTitle: 'GetArbitrationApplicationReport',
         navActiveLink: 'GetArbitrationApplicationReport',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'GetArbitrationApplicationReport',
               active: true
            }
         ]
      }
   }
];
