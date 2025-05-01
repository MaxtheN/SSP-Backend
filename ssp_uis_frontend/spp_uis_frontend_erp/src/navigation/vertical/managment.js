const visibles = [
   'OrganizationView',
   'UserView',
   'RoleView',
   'CustomJobView',
   'RestrictionSendingAppView',
   'SignCriterionView',
   'SendSmsConfigView',
   'CurrencyView',
   'AppErrorView'
];
export default [
   {
      header: 'Management',
      visible: visibles
   },
   {
      title: 'Management',
      icon: 'SettingsIcon',
      visible: visibles,
      children: [
         {
            title: 'Organization',
            route: 'Organization',
            visible: 'OrganizationView'
         },
         {
            title: 'User',
            route: 'user',
            visible: 'UserView'
         },
         {
            title: 'Role',
            route: 'role',
            visible: 'RoleView'
         },
         {
            title: 'integration',
            route: 'integration',
            visible: 'Integration'
         },
         {
            title: 'AppError',
            route: 'appError',
            visible: 'AppErrorView'
         },
         {
            title: 'CustomJob',
            route: 'CustomJob',
            visible: 'CustomJobView'
         },
         {
            title: 'SignCriterion',
            route: 'SignCriterion',
            visible: 'SignCriterionView'
         },
         {
            title: 'currency',
            route: 'Currency',
            visible: 'CurrencyView'
         },
         {
            title: 'SendSmsConfig',
            route: 'SendSmsConfig',
            visible: 'SendSmsConfigView'
         },

         {
            title: 'RestrictionSendingApp',
            route: 'RestrictionSendingApp',
            visible: 'RestrictionSendingAppView'
         }
      ]
   }
];
