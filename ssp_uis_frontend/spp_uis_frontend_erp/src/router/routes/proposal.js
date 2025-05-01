export default [
   {
      path: '/proposal/callcenterappial',
      name: 'CallCenterAppeal',
      component: () => import('@/views/proposal/document/index.vue'),
      meta: {
         pageTitle: 'CallCenterAppeal',
         breadcrumb: [
            {
               text: 'CallCenterAppeal'
            },
            {
               text: 'CallCenterAppeal',
               active: true
            }
         ]
      }
   },
   {
      path: '/proposal/callcenterappial/edit/id=:id',
      name: 'EditCallCenterAppeal',
      component: () => import('@/views/proposal/document/edit.vue'),
      meta: {
         pageTitle: 'CallCenterAppeal',
         navActiveLink: 'CallCenterAppeal',
         breadcrumb: [
            {
               text: 'CallCenterAppeal'
            },
            {
               text: 'CallCenterAppeal',
               active: true
            }
         ]
      }
   },

   {
      path: '/proposal/proposal',
      name: 'Proposal',
      component: () => import('@/views/proposal/proposal/index.vue'),
      meta: {
         pageTitle: 'Proposal',
         breadcrumb: [
            {
               text: 'Proposal'
            },
            {
               text: 'Proposal',
               active: true
            }
         ]
      }
   },
   {
      path: '/proposal/proposal/edit/id=:id',
      name: 'EditProposal',
      component: () => import('@/views/proposal/proposal/edit.vue'),
      meta: {
         pageTitle: 'Proposal',
         navActiveLink: 'Proposal',
         breadcrumb: [
            {
               text: 'Proposal'
            },
            {
               text: 'Proposal',
               active: true
            }
         ]
      }
   }
];
