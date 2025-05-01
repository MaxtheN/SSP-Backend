const visibles = ['ArbitrationCourtApplicationView'];
const visibles2 = ['ArbitrationJudgeView'];
const visibles3 = ['ArbitrationJudgeView'];
export default [
   {
      header: 'arbitrationcourt',
      visible: [...visibles, ...visibles2, ...visibles3]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles2,
      children: [
         {
            title: 'arbitrationjudge',
            route: 'arbitrationjudge',
            visible: 'ArbitrationJudgeView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles,
      children: [
         {
            title: 'arbitrationcourt',
            route: 'arbitrationcourt',
            visible: 'ArbitrationCourtApplicationView'
         },
         {
            title: 'arbitrationdiscussion',
            route: 'arbitrationdiscussion',
            visible: true
         },
         {
            title: 'arbitrationdelay',
            route: 'arbitrationdelay',
            visible: true
         },
         {
            title: 'arbitrationresult',
            route: 'arbitrationresult',
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
            title: 'GetArbitrationApplicationReport',
            route: 'GetArbitrationApplicationReport',
            visible: 'ArbitrationJudgeView'
         }
      ]
   }
];
