export default [
   {
      path: '/managment/organization',
      name: 'Organization',
      component: () => import('@/views/managment/organization/index.vue'),
      meta: {
         pageTitle: 'Organization',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'Organization',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/integration',
      name: 'integration',
      component: () => import('@/views/managment/integration/index.vue'),
      meta: {
         pageTitle: 'integration',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'integration',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/app-error',
      name: 'appError',
      component: () => import('@/views/managment/appError/index.vue'),
      meta: {
         pageTitle: 'AppError',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'AppError',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/organization/edit/id=:id',
      name: 'EditOrganization',
      component: () => import('@/views/managment/organization/edit.vue'),
      meta: {
         pageTitle: 'Organization',
         navActiveLink: 'Organization',
         breadcrumb: [
            {
               text: 'Management'
            },
            // {
            //   text: "Organization",
            //   active: true,
            // },
            {
               text: 'Organization',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/organization/view/id=:id',
      name: 'ViewOrganization',
      component: () => import('@/views/managment/organization/view.vue'),
      meta: {
         pageTitle: 'ViewOrganization',
         navActiveLink: 'Organization',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'Organization',
               active: true
            },
            {
               text: 'ViewOrganization'
            }
         ]
      }
   },
   {
      path: '/managment/user',
      name: 'user',
      component: () => import('@/views/managment/user/index.vue'),
      meta: {
         pageTitle: 'User',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'User',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/user/edit/id=:id',
      name: 'EditUser',
      component: () => import('@/views/managment/user/edit.vue'),
      meta: {
         pageTitle: 'User',
         navActiveLink: 'user',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'User',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/role',
      name: 'role',
      component: () => import('@/views/managment/role/index.vue'),
      meta: {
         pageTitle: 'Role',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'Role',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/role/edit/id=:id',
      name: 'EditRole',
      component: () => import('@/views/managment/role/edit.vue'),
      meta: {
         pageTitle: 'EditRole',
         navActiveLink: 'role',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'Role',
               active: true
            },
            {
               text: 'EditRole',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/businessmancard',
      name: 'BusinessmanCard',
      component: () => import('@/views/managment/businessmancard/index.vue'),
      meta: {
         pageTitle: 'BusinessmanCard',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'BusinessmanCard',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/bojxona',
      name: 'Bojxona',
      component: () => import('@/views/managment/bojxona/index.vue'),
      meta: {
         pageTitle: 'Bojxona',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'Bojxona',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/GetFromInvestment',
      name: 'GetFromInvestment',
      component: () => import('@/views/managment/GetFromInvestment/index.vue'),
      meta: {
         pageTitle: 'GetFromInvestment',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'GetFromInvestment',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/videocategory/edit/id=:id',
      name: 'EditVideoCategory',
      component: () => import('@/views/managment/videocategory/edit.vue'),
      meta: {
         pageTitle: 'EditVideoCategory',
         navActiveLink: 'VideoCategory',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'VideoCategory',
               active: true
            },
            {
               text: 'EditVideoCategory',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/videocategory',
      name: 'VideoCategory',
      component: () => import('@/views/managment/videocategory/index.vue'),
      meta: {
         pageTitle: 'VideoCategory',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'VideoCategory',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/videolesson/edit/id=:id',
      name: 'EditVideoLesson',
      component: () => import('@/views/managment/videolesson/edit.vue'),
      meta: {
         pageTitle: 'EditVideoLesson',
         navActiveLink: 'VideoLesson',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'VideoLesson',
               active: true
            },
            {
               text: 'EditVideoLesson',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/videolesson',
      name: 'VideoLesson',
      component: () => import('@/views/managment/videolesson/index.vue'),
      meta: {
         pageTitle: 'VideoLesson',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'VideoLesson',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/news/edit/id=:id',
      name: 'EditNews',
      component: () => import('@/views/managment/news/edit.vue'),
      meta: {
         pageTitle: 'News',
         navActiveLink: 'News',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'News',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/news',
      name: 'News',
      component: () => import('@/views/managment/news/index.vue'),
      meta: {
         pageTitle: 'News',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'News',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/newstag/edit/id=:id',
      name: 'EditNewsTag',
      component: () => import('@/views/managment/newstag/edit.vue'),
      meta: {
         pageTitle: 'NewsTag',
         navActiveLink: 'NewsTag',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'NewsTag',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/newstag',
      name: 'NewsTag',
      component: () => import('@/views/managment/newstag/index.vue'),
      meta: {
         pageTitle: 'NewsTag',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'NewsTag',
               active: true
            }
         ]
      }
   },

   {
      path: '/managment/businessactivitytype',
      name: 'BusinessActivityType',
      component: () => import('@/views/managment/businessactivitytype/index.vue'),
      meta: {
         pageTitle: 'BusinessActivityType',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'BusinessActivityType',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/customjob/edit/id=:id',
      name: 'EditCustomJob',
      component: () => import('@/views/managment/customjob/edit.vue'),
      meta: {
         pageTitle: 'EditCustomJob',
         navActiveLink: 'CustomJob',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'CustomJob',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/customjob',
      name: 'CustomJob',
      component: () => import('@/views/managment/customjob/index.vue'),
      meta: {
         pageTitle: 'CustomJob',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'CustomJob',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/currency/edit/id=:id',
      name: 'EditCurrency',
      component: () => import('@/views/managment/currency/edit.vue'),
      meta: {
         pageTitle: 'currency',
         navActiveLink: 'Currency',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'currency',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/currency',
      name: 'Currency',
      component: () => import('@/views/managment/currency/index.vue'),
      meta: {
         pageTitle: 'currency',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'currency',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/restrictionsendingapp',
      name: 'RestrictionSendingApp',
      component: () => import('@/views/managment/restrictionsendingapp/index.vue'),
      meta: {
         pageTitle: 'RestrictionSendingApp',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'RestrictionSendingApp',
               active: true
            }
         ]
      }
   },

   {
      path: '/managment/restrictionsendingapp/edit/id=:id',
      name: 'EditRestrictionSendingApp',
      component: () => import('@/views/managment/restrictionsendingapp/edit.vue'),
      meta: {
         pageTitle: 'RestrictionSendingApp',
         navActiveLink: 'RestrictionSendingApp',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'RestrictionSendingApp',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/signcriterion',
      name: 'SignCriterion',
      component: () => import('@/views/managment/signcriterion/index.vue'),
      meta: {
         pageTitle: 'SignCriterion',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'SignCriterion',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/signcriterion/edit/id=:id',
      name: 'EditSignCriterion',
      component: () => import('@/views/managment/signcriterion/edit.vue'),
      meta: {
         pageTitle: 'SignCriterion',
         navActiveLink: 'SignCriterion',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'SignCriterion',
               active: true
            }
         ]
      }
   },
   {
      path: '/managment/universalprint',
      name: 'UniversalPrint',
      component: () => import('@/views/managment/universalprint/index.vue'),
      meta: {
         pageTitle: 'UniversalPrint',
         breadcrumb: [
            {
               text: 'Management'
            },
            {
               text: 'UniversalPrint',
               active: true
            }
         ]
      }
   }
];
