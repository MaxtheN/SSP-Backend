export default [
   {
      path: '/dualedu/institute',
      name: 'Institute',
      component: () => import('@/views/dualedu/institute/index.vue'),
      meta: {
         pageTitle: 'Institutes',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'Institutes',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/institute/edit/id=:id',
      name: 'EditInstitute',
      component: () => import('@/views/dualedu/institute/edit.vue'),
      meta: {
         pageTitle: 'Institutes',
         navActiveLink: 'Institute',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'Institutes'
            },
            {
               text: 'create',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/specialty',
      name: 'Specialty',
      component: () => import('@/views/dualedu/specialty/index.vue'),
      meta: {
         pageTitle: 'Specialty',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'Specialty',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/specialty/edit/id=:id',
      name: 'EditSpecialty',
      component: () => import('@/views/dualedu/specialty/edit.vue'),
      meta: {
         pageTitle: 'Specialty',
         navActiveLink: 'Specialty',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'Specialty',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/dualeducationtype',
      name: 'DualEducationType',
      component: () => import('@/views/dualedu/dualeducationtype/index.vue'),
      meta: {
         pageTitle: 'DualEducationType',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'DualEducationType',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/dualeducationtype/edit/id=:id',
      name: 'EditDualEducationType',
      component: () => import('@/views/dualedu/dualeducationtype/edit.vue'),
      meta: {
         pageTitle: 'DualEducationType',
         navActiveLink: 'DualEducationType',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'DualEducationType',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/dualapplication',
      name: 'DualApplication',
      component: () => import('@/views/dualedu/dualapplication/index.vue'),
      meta: {
         pageTitle: 'DualApplication',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'DualApplication',
               active: true
            }
         ]
      }
   },

   {
      path: '/dualedu/dualapplication/edit/id=:id',
      name: 'EditDualApplication',
      component: () => import('@/views/dualedu/dualapplication/edit.vue'),
      meta: {
         pageTitle: 'DualApplication',
         navActiveLink: 'DualApplication',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'DualApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/dualcontract',
      name: 'DualContract',
      component: () => import('@/views/dualedu/dualcontract/index.vue'),
      meta: {
         pageTitle: 'DualContract',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'DualContract',
               active: true
            }
         ]
      }
   },
   {
      path: '/dualedu/dualcontract/edit/id=:id',
      name: 'EditDualContract',
      component: () => import('@/views/dualedu/dualcontract/view.vue'),
      meta: {
         pageTitle: 'DualContract',
         breadcrumb: [
            {
               text: 'dualedu'
            },
            {
               text: 'DualContract',
               active: true
            }
         ]
      }
   }
];
