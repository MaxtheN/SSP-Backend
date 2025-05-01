const visible1 = [
   'ProposalView',
   'CallCenterReportByWeekReport',
   'CallCenterAppealReportByOkedType',
   'CallCenterByRegionReportView'
];
const visibles3 = ['AppealDescriptionView', 'AppealTypeArriveView'];
const visible2 = ['ProposalView', 'CallCenterAppealView'];
const visibles = [];
export default [
   {
      header: 'Proposal1',
      visible: [...visible1, ...visible2, ...visibles]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles3,
      children: [
         {
            title: 'AppealDescription',
            route: 'AppealDescription',
            visible: 'AppealDescriptionView'
         },
         {
            title: 'AppealTypeArrive',
            route: 'AppealTypeArrive',
            visible: 'AppealTypeArriveView'
         }
      ]
   },
   {
      title: 'Proposal',
      icon: 'SettingsIcon',
      visible: visible1,
      children: [
         {
            title: 'Proposal',
            route: 'Proposal',
            visible: 'ProposalView'
         }
      ]
   },
   {
      title: 'CallCenterAppeal',
      icon: 'FileTextIcon',
      visible: visible2,
      children: [
         {
            title: 'CallCenterAppeal',
            route: 'CallCenterAppeal',
            visible: 'CallCenterAppealView'
         }
      ]
   },
   {
      title: 'Report',
      icon: 'ClipboardIcon',
      visible: visible1,
      children: [
         {
            title: 'CallCenterAppealReportByOkedType',
            route: 'CallCenterAppealReportByOkedType',
            visible: 'CallCenterAppealReportByOkedType'
         },
         {
            title: 'GetCallCenterByRegion',
            route: 'GetCallCenterByRegion',
            visible: true
         },
         {
            title: 'GetCallCenterReportByWeek',
            route: 'GetCallCenterReportByWeek',
            visible: 'CallCenterReportByWeekReport'
         }
      ]
   }
];
