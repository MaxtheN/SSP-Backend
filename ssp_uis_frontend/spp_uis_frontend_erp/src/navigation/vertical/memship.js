const visibles = [
   'MemshipApplicationView',
   'MemshipContractView',
   'MemshipNewContractorView',
   'MemshipCertificateView',
   'MemshipPaymentOrderView',
   'MemshipYearlyPlanViewAll',
   'ContractorCategoryCriterionView',
   'DebtView'
];
const visibles2 = [
   'ContractorActivityTypeView',
   'NeedChamberServiceView',
   'ContractorActivityGroupView',
   'MemshipApplicationAndContractInfoReport'
];

const visible = [
   'ReportMemshipApplicationAndContractInfoView',
   'ReportMemshipApplicationAndContractInfoView',
   'ReportMemshipFreeOfChargeView',
   'ReportMemshipPaidOfChargeView',
   'ReportMemshipApplicationByPersonTypeView',
   'ReportMemshipApplicationByOrganizationView',
   'ReportMemshipPaidOfChargeView',
   'ReportMemshipPaidOfChargeView'
];

export default [
   {
      header: 'Memship',
      visible: [...visibles, ...visibles2, ...visible]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles2,
      children: [
         {
            title: 'ContractorActivityType',
            route: 'ContractorActivityType',
            visible: 'ContractorActivityTypeView'
         },
         {
            title: 'ContractorActivityGroup',
            route: 'ContractorActivityGroup',
            visible: 'ContractorActivityGroupView'
         },
         {
            title: 'GetQqsAylanma',
            route: 'GetQqsAylanma',
            visible: true
         },
         {
            title: 'ContractorRating',
            route: 'ContractorRating',
            visible: true
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles,
      children: [
         {
            title: 'MemshipApplication',
            route: 'MemshipApplication',
            visible: 'MemshipApplicationView'
         },
         {
            title: 'MemshipContract',
            route: 'MemshipContract',
            visible: 'MemshipContractView'
         },
         {
            title: 'MemshipNewContractor',
            route: 'MemshipNewContractor',
            visible: 'MemshipNewContractorView'
         },
         {
            title: 'MemshipCertificate',
            route: 'MemshipCertificate',
            visible: 'MemshipCertificateView'
         },
         {
            title: 'MemshipPaymentOrder',
            route: { name: 'MemshipPaymentOrder', query: { applicationTypeId: 3 } },
            visible: 'MemshipPaymentOrderView'
         },
         {
            title: 'MemshipYearlyPlan',
            route: 'MemshipYearlyPlan',
            visible: 'MemshipYearlyPlanViewAll'
         },
         {
            title: 'ContractorCategoryCriterion',
            route: 'ContractorCategoryCriterion',
            visible: 'ContractorCategoryCriterionView'
         },
         {
            title: 'Debt',
            route: 'Debt',
            visible: 'DebtView'
         }
      ]
   },
   {
      title: 'Report',
      icon: 'ClipboardIcon',
      visible: visible,
      children: [
         {
            title: 'MemshipApplicationAndContractInfoReport',
            route: 'MemshipApplicationAndContractInfoReport',
            visible: 'ReportMemshipApplicationAndContractInfoView'
         },
         {
            title: 'GetMemshipDocsInfoReestr',
            route: 'GetMemshipDocsInfoReestr',
            visible: 'ReportMemshipApplicationAndContractInfoView'
         },
         {
            title: 'GetMemshipReports',
            route: 'GetMemshipReports',
            visible: 'ReportMemshipFreeOfChargeView'
         },
         {
            title: 'GetPaidMemshipReport',
            route: 'GetPaidMemshipReport',
            visible: 'ReportMemshipPaidOfChargeView'
         },
         {
            title: 'GetMemshipApplicationReportByPersonType',
            route: 'GetMemshipApplicationReportByPersonType',
            visible: 'ReportMemshipApplicationByPersonTypeView'
         },
         {
            title: 'GetMemshipApplicationReportByOrganizations',
            route: 'GetMemshipApplicationReportByOrganizations',
            visible: 'ReportMemshipApplicationByOrganizationView'
         },
         {
            title: 'GetMemshipContract',
            route: 'GetMemshipContract',
            visible: 'ReportMemshipPaidOfChargeView'
         },
         {
            title: 'GetContractorCategoryType',
            route: 'GetContractorCategoryType',
            visible: true
         },
         // QQS
         {
            title: 'GetTaxQqsAylanmaReport',
            route: 'GetTaxQqsAylanmaReport',
            visible: 'ReportMemshipPaidOfChargeView'
         },
         {
            title: 'GetPayDocsByAccEqualsAndBankDateBetween',
            route: 'GetPayDocsByAccEqualsAndBankDateBetween',
            visible: 'MemshipPaymentOrderView'
         }
      ]
   }
];
