const visibles = [
   'TariffScaleView',
   'TariffScaleCoefView',
   'FixedMinimumValueView',
   'TaxBenefitTypeView',
   'ItemOfExpenseView',
   'CalculationKindView',
   'PlannedCalculationView',
   'SourceCodeView',
   'StaffTypeBasicTariffView',
   'StaffingIndicatorView',
   'SettlementAccountSourceView',
   'LevelCodeView',
   'QualificationCategoryView'
];
const visibles2 = ['TaxBenefitView'];

export default [
   {
      header: 'Salary',
      visible: [...visibles, ...visibles2]
   },
   {
      title: 'Info',
      icon: 'LayersIcon',
      visible: visibles,
      children: [
         {
            title: 'FixedMinimumValue',
            route: 'FixedMinimumValue',
            visible: 'FixedMinimumValueView'
         },
         {
            title: 'TariffScale',
            route: 'TariffScale',
            visible: 'TariffScaleView'
         },
         {
            title: 'TariffScaleCoef',
            route: 'TariffScaleCoef',
            visible: 'TariffScaleCoefView'
         },
         {
            title: 'taxbenefittype',
            route: 'TaxBenefitType',
            visible: 'TaxBenefitTypeView'
         },
         {
            title: 'itemOfExpense',
            route: 'ItemOfExpense',
            visible: 'ItemOfExpenseView'
         },
         {
            title: 'calculationkind',
            route: 'CalculationKind',
            visible: 'CalculationKindView'
         },
         {
            title: 'plannedcalculation',
            route: 'PlannedCalculation',
            visible: 'PlannedCalculationView'
         },
         {
            title: 'sourceCode',
            route: 'SourceCode',
            visible: 'SourceCodeView'
         },
         {
            title: 'staffTypeBasicTariff',
            route: 'StaffTypeBasicTariff',
            visible: 'StaffTypeBasicTariffView'
         },
         {
            title: 'staffingindicator',
            route: 'StaffingIndicator',
            visible: 'StaffingIndicatorView'
         },
         {
            title: 'settlementaccountsource',
            route: 'SettlementAccountSource',
            visible: 'SettlementAccountSourceView'
         },
         {
            title: 'levelCode',
            route: 'LevelCode',
            visible: 'LevelCodeView'
         },
         {
            title: 'QualificationCategory',
            route: 'QualificationCategory',
            visible: 'QualificationCategoryView'
         }
      ]
   },
   {
      title: 'document',
      icon: 'FileTextIcon',
      visible: visibles2,
      children: [
         {
            title: 'taxbenefit',
            route: 'TaxBenefit',
            visible: 'TaxBenefitView'
         }
      ]
   }
];
