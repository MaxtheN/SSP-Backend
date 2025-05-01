const visibles = [
   'PrtnContractView',
   'OrganizationView',
   'ApplicationView',
   'PrtnCertificateView',
   'PrtnCreditDemandView'
];

const visibles2 = ['EducationItemView'];

const visible = [...visibles, ...visibles2];

export default [
   {
      header: 'prtncontractH',
      visible: visible
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles2,
      children: [
         {
            title: 'EducationItem',
            route: 'EducationItem',
            visible: 'EducationItemView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles,
      children: [
         {
            title: 'Application',
            route: 'Application',
            visible: 'ApplicationView'
         },
         {
            title: 'StateAssetApplication',
            route: 'StateAssetApplication',
            visible: 'ApplicationView'
         },
         {
            title: 'ApplicationCustom',
            route: 'ApplicationCustom',
            visible: 'PrtnContractCreateManually'
         },
         {
            title: 'prtncontract',
            route: 'PrtnContract',
            visible: 'PrtnContractView'
         },
         {
            title: 'prtncertificate',
            route: 'PrtnCertificate',
            visible: 'PrtnCertificateView'
         },
         {
            title: 'PrtnCreditDemand',
            route: 'PrtnCreditDemand',
            visible: 'PrtnCreditDemandView'
         },
         {
            title: 'monoaplication',
            route: 'monoaplication',
            visible: 'PrtnContractView'
         }
      ]
   }
];
