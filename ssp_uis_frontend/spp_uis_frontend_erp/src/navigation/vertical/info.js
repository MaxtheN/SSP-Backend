const visibles1 = ['CountryView', 'RegionView', 'DistrictView', 'MfyView'];
const visibles2 = [
   'PrtnContractTypeView',
   'PrtnRejectReasonView',
   'OrganizationLegalFormView',
   'OrganizationalStructureView',
   'RelativeDegreeView',
   'IdentityDocumentView',
   'WorkScheduleView',
   'ContractorView',
   'ReportSmsLogView',
   'UniteOfMeasureView'
];
const visibles3 = ['OkedView', 'BusinessmanCardView', 'BankView'];

export default [
   {
      header: 'InfoGlob',
      visible: [...visibles1, ...visibles2, ...visibles3]
   },
   {
      title: 'InfoCountty',
      icon: 'GlobeIcon',
      visible: visibles1,
      children: [
         {
            title: 'Country',
            route: 'country',
            visible: 'CountryView'
         },
         {
            title: 'Oblast',
            route: 'region',
            visible: 'RegionView'
         },
         {
            title: 'Region',
            route: 'District',
            visible: 'DistrictView'
         },
         {
            title: 'mfy',
            route: 'Mfy',
            visible: 'MfyView'
         }
      ]
   },
   {
      title: 'InfoGlob',
      icon: 'LayersIcon',
      visible: visibles2,
      children: [
         {
            title: 'prtnContractType',
            route: 'PrtnContractType',
            visible: 'PrtnContractTypeView'
         },
         {
            title: 'prtnRejectReason',
            route: 'PrtnRejectReason',
            visible: 'PrtnRejectReasonView'
         },
         {
            title: 'OrganizationLegalForm',
            route: 'OrganizationLegalForm',
            visible: 'OrganizationLegalFormView'
         },
         {
            title: 'organizationalstructure',
            route: 'OrganizationalStructure',
            visible: 'OrganizationalStructureView'
         },
         {
            title: 'RelativeDegree',
            route: 'RelativeDegree',
            visible: 'RelativeDegreeView'
         },
         {
            title: 'IdentityDocument',
            route: 'IdentityDocument',
            visible: 'IdentityDocumentView'
         },
         {
            title: 'WorkSchedule',
            route: 'WorkSchedule',
            visible: 'WorkScheduleView'
         },
         {
            title: 'contractor',
            route: 'Contractor',
            visible: 'ContractorView'
         },
         {
            title: 'GetSmsLogReport',
            route: 'GetSmsLogReport',
            visible: 'ReportSmsLogView'
         },
         {
            title: 'UniteOfMeasure',
            route: 'UniteOfMeasure',
            visible: 'UniteOfMeasureView'
         }
      ]
   },
   {
      title: 'InfoSchool',
      icon: 'LayersIcon',
      visible: visibles3,
      children: [
         {
            title: 'Oked',
            route: 'oked',
            visible: 'OkedView'
         },
         {
            title: 'bankid',
            route: 'Bank',
            visible: 'BankView'
         },
         {
            title: 'BusinessmanCard',
            route: 'BusinessmanCard',
            visible: 'BusinessmanCardView'
         },
         {
            title: 'BojxonaImtiyozReportByContractorShort',
            route: 'Bojxona',
            visible: 'BusinessmanCardView'
         },
         {
            title: 'GetFromInvestment',
            route: 'GetFromInvestment',
            visible: 'BusinessmanCardView'
         }
      ]
   }
];
