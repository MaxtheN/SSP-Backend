export default [
   {
      path: '/document/joinanticorruptionapplication',
      name: 'JoinAntiCorruptionApplication',
      component: () => import('@/views/corruption/JoinAntiCorruptionApplication/index.vue'),
      meta: {
         pageTitle: 'JoinAntiCorruptionApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'JoinAntiCorruptionApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/joinanticorruptionapplication/view/id=:id',
      name: 'ViewJoinAntiCorruptionApplication',
      component: () => import('@/views/corruption/JoinAntiCorruptionApplication/view.vue'),
      meta: {
         pageTitle: 'JoinAntiCorruptionApplication',
         navActiveLink: 'JoinAntiCorruptionApplication',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'JoinAntiCorruptionApplication',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/joinanticorruptionresult',
      name: 'JoinAntiCorruptionResult',
      component: () => import('@/views/corruption/JoinAntiCorruptionResult/index.vue'),
      meta: {
         pageTitle: 'JoinAntiCorruptionResult',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'JoinAntiCorruptionResult',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/joinanticorruptionresult/edit/id=:id',
      name: 'EditJoinAntiCorruptionResult',
      component: () => import('@/views/corruption/JoinAntiCorruptionResult/edit.vue'),
      meta: {
         pageTitle: 'JoinAntiCorruptionResult',
         navActiveLink: 'JoinAntiCorruptionResult',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'JoinAntiCorruptionResult',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/joinanticorruptioncertificate',
      name: 'JoinAntiCorruptionCertificate',
      component: () => import('@/views/corruption/JoinAntiCorruptionCertificate/index.vue'),
      meta: {
         pageTitle: 'JoinAntiCorruptionCertificate',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'JoinAntiCorruptionCertificate',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/joinanticorruptioncertificate/view/id=:id',
      name: 'ViewJoinAntiCorruptionCertificate',
      component: () => import('@/views/corruption/JoinAntiCorruptionCertificate/view.vue'),
      meta: {
         pageTitle: 'JoinAntiCorruptionCertificate',
         navActiveLink: 'JoinAntiCorruptionCertificate',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'JoinAntiCorruptionCertificate',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/questionnarie',
      name: 'Questionnarie',
      component: () => import('@/views/corruption/questionnarie/index.vue'),
      meta: {
         pageTitle: 'Questionnarie',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Questionnarie',
               active: true
            }
         ]
      }
   },
   {
      path: '/document/questionnarie/edit/id=:id',
      name: 'EditQuestionnarie',
      component: () => import('@/views/corruption/questionnarie/edit.vue'),
      meta: {
         pageTitle: 'Questionnarie',
         navActiveLink: 'Questionnarie',
         breadcrumb: [
            {
               text: 'Document'
            },
            {
               text: 'Questionnarie',
               active: true
            }
         ]
      }
   },
];
