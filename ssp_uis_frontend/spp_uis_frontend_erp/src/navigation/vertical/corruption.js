const visibles1 = ['JoinAntiCorruptionApplicationView', 'JoinAntiCorruptionResultView'];
const visibles2 = ['ContractorUnionActivityTypeView'];
const visibles3 = ['ContractorUnionActivityTypeView', 'CorruptionApplicationView'];

export default [
   {
      header: 'Corruption',
      visible: [...visibles1, ...visibles2, ...visibles3]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles2,
      children: [
         {
            title: 'ContractorUnionActivityType',
            route: 'ContractorUnionActivityType',
            visible: 'ContractorUnionActivityTypeView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles1,
      children: [
         {
            title: 'JoinAntiCorruptionApplication',
            route: 'JoinAntiCorruptionApplication',
            visible: 'JoinAntiCorruptionApplicationView'
         },
         {
            title: 'JoinAntiCorruptionResult',
            route: 'JoinAntiCorruptionResult',
            visible: 'JoinAntiCorruptionResultView'
         },
         {
            title: 'JoinAntiCorruptionCertificate',
            route: 'JoinAntiCorruptionCertificate',
            visible: 'JoinAntiCorruptionApplicationView'
         },
         {
            title: 'Questionnarie',
            route: 'Questionnarie',
            visible: 'QuestionnaireView'
         }
      ]
   },
   {
      title: 'Report',
      icon: 'ClipboardIcon',
      visible: visibles3,
      children: [
         {
            title: 'GetCharterMembersRegisterReport',
            route: 'GetCharterMembersRegisterReport',
            visible: true
         },
         {
            title: 'GetCorruptionByRegion',
            route: 'GetCorruptionByRegion',
            visible: 'CorruptionApplicationView'
         }
      ]
   }
];
