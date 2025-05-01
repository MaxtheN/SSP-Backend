const visibles1 = ['IndicatorView', 'IndicatorDepartmentView'];
const visibles2 = ['IndicatorView', 'KpiGratingView', 'KpiPlanForEmployeeView', 'KpiRatingEmployeeView'];

export default [
   {
      header: 'KPI',
      visible: [...visibles1, ...visibles2]
   },

   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles1,
      children: [
         {
            title: 'Indicator',
            route: 'Indicator',
            visible: 'IndicatorView'
         },
         {
            title: 'IndicatorDepartment',
            route: 'IndicatorDepartment',
            visible: 'IndicatorDepartmentView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles2,
      children: [
         {
            title: 'KpiGrating',
            route: 'KpiGrating',
            visible: 'KpiGratingView'
         },
         {
            title: 'KpiPlanForEmployee',
            route: 'KpiPlanForEmployee',
            visible: 'KpiPlanForEmployeeView'
         },
         {
            title: 'KpiRatingEmployee',
            route: 'KpiRatingEmployee',
            visible: 'KpiRatingEmployeeView'
         }
      ]
   }
];
