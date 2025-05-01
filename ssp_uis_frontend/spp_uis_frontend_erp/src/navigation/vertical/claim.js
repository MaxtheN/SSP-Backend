const visibles1 = ['ClaimApplicationView', 'MediationPlanView', 'MediationView', 'ApplicationForCourtViewAll'];
const visibles2 = ['ClaimThemeView', 'ClaimOrganizationView', 'ClaimOrganizationTypeView'];

export default [
   {
      header: 'Claim',
      visible: [...visibles1, ...visibles2]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles2,
      children: [
         {
            title: 'ClaimTheme',
            route: 'ClaimTheme',
            visible: 'ClaimThemeView'
         },
         {
            title: 'ClaimOrganization',
            route: 'ClaimOrganization',
            visible: 'ClaimOrganizationView'
         },
         {
            title: 'ClaimOrganizationType',
            route: 'ClaimOrganizationType',
            visible: 'ClaimOrganizationTypeView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles1,
      children: [
         {
            title: 'ClaimApplication',
            route: 'ClaimApplication',
            visible: 'ClaimApplicationView'
         },
         {
            title: 'MediationPlan',
            route: 'MediationPlan',
            visible: 'MediationPlanView'
         },
         {
            title: 'Mediation',
            route: 'Mediation',
            visible: 'MediationView'
         },
         {
            title: 'ClaimApplication2',
            route: 'ClaimApplication2',
            visible: 'ClaimApplicationView'
         },
         {
            title: 'ApplicationForCourt',
            route: 'ApplicationForCourt',
            visible: 'ApplicationForCourtViewAll'
         }
      ]
   },
   {
      title: 'Report',
      icon: 'ClipboardIcon',
      visible: visibles1,
      children: [
         {
            title: 'SummaOfClaimApplicationReport',
            route: 'SummaOfClaimApplicationReport',
            visible: true
         },
         {
            title: 'ReceivedClaimApplicationReport',
            route: 'ReceivedClaimApplicationReport',
            visible: true
         },
         {
            title: 'AppealsSentToClaimApplicationReport',
            route: 'AppealsSentToClaimApplicationReport',
            visible: true
         },
         {
            title: 'GetClaimApplicationReport',
            route: 'GetClaimApplicationReport',
            visible: true
         },
         {
            title: 'ClaimApplicationReport',
            route: 'ClaimApplicationReport',
            visible: true
         },
         {
            title: 'ClaimApplicationAmount',
            route: 'ClaimApplicationAmount',
            visible: true
         },
         {
            title: 'GetClaimApplicationAmount',
            route: 'GetClaimApplicationAmount',
            visible: true
         }
      ]
   }
];
