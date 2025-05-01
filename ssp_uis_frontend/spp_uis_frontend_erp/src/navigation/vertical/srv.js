const visibles1 = ['NeedChamberServiceGroupView', 'SrvServicePriceView'];
const visibles2 = [
   'SrvServiceApplicationView',
   'NeedChamberServiceView',
   'ServiceContractView',
   'CompleteServiceView',
   'SrvApplicationYearlyPlanView'
];
const visibles3 = ['ReportSrvServiceGetInfo', 'DeedSwotReportView'];

export default [
   {
      header: 'Srv',
      visible: [...visibles1, ...visibles2, ...visibles3]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles1,
      children: [
         {
            title: 'NeedChamberServiceGroup',
            route: 'NeedChamberServiceGroup',
            visible: 'NeedChamberServiceGroupView'
         },
         {
            title: 'NeedChamberService',
            route: 'NeedChamberService',
            visible: 'NeedChamberServiceView'
         },
         {
            title: 'ServicePrice',
            route: 'ServicePrice',
            visible: 'SrvServicePriceView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles2,
      children: [
         {
            title: 'ServiceApplication',
            route: 'ServiceApplication',
            visible: 'SrvServiceApplicationView'
         },
         {
            title: 'ServiceContract',
            route: 'ServiceContract',
            visible: 'ServiceContractView'
         },
         {
            title: 'ServiceDeed',
            route: 'ServiceDeed',
            visible: 'ServiceDeedView'
         },
         {
            title: 'CompleteService',
            route: 'CompleteService',
            visible: 'CompleteServiceView'
         },
         {
            title: 'SrvYearlyPlan',
            route: 'SrvYearlyPlan',
            visible: 'SrvApplicationYearlyPlanView'
         },
         {
            title: 'MemshipPaymentOrder',
            route: { name: 'MemshipPaymentOrder', query: { applicationTypeId: 7 } },
            visible: 'ServicePaymentOrderView'
         },
         {
            title: 'FinanceIntegration',
            route: 'FinanceIntegration',
            visible: true
         }
      ]
   },
   {
      title: 'Report',
      icon: 'ClipboardIcon',
      visible: visibles3,
      children: [
         {
            title: 'ServiceInfo',
            route: 'ServiceInfo',
            visible: 'ReportSrvServiceGetInfo'
         },
         {
            title: 'ServiceInfoNew',
            route: 'ServiceInfoNew',
            visible: 'DeedSwotReportView'
         }
      ]
   }
];
