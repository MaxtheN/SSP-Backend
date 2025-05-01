export default [
   {
      path: '/salary/plannedcalculation',
      name: 'PlannedCalculation',
      component: () => import('@/views/salary/plannedcalculation/index.vue'),
      meta: {
         pageTitle: 'plannedcalculation',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'plannedcalculation',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/plannedcalculation/edit/id=:id',
      name: 'EditPlannedCalculation',
      component: () => import('@/views/salary/plannedcalculation/edit.vue'),
      meta: {
         pageTitle: 'plannedcalculation',
         navActiveLink: 'PlannedCalculation',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'plannedcalculation',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/taxbenefit',
      name: 'TaxBenefit',
      component: () => import('@/views/salary/taxbenefit/index.vue'),
      meta: {
         pageTitle: 'taxbenefit',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'taxbenefit',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/taxbenefit/edit/id=:id',
      name: 'EditTaxBenefit',
      component: () => import('@/views/salary/taxbenefit/edit.vue'),
      meta: {
         pageTitle: 'taxbenefit',
         navActiveLink: 'TaxBenefit',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'taxbenefit',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/taxbenefittype',
      name: 'TaxBenefitType',
      component: () => import('@/views/salary/taxbenefittype/index.vue'),
      meta: {
         pageTitle: 'taxbenefittype',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'taxbenefittype',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/taxbenefittype/edit/id=:id',
      name: 'EditTaxBenefitType',
      component: () => import('@/views/salary/taxbenefittype/edit.vue'),
      meta: {
         pageTitle: 'taxbenefittype',
         navActiveLink: 'TaxBenefitType',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'taxbenefittype',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/calculationkind',
      name: 'CalculationKind',
      component: () => import('@/views/salary/calculationkind/index.vue'),
      meta: {
         pageTitle: 'calculationkind',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'calculationkind',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/calculationkind/edit/id=:id',
      name: 'EditCalculationKind',
      component: () => import('@/views/salary/calculationkind/edit.vue'),
      meta: {
         pageTitle: 'calculationkind',
         navActiveLink: 'CalculationKind',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'calculationkind',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/itemofexpense',
      name: 'ItemOfExpense',
      component: () => import('@/views/salary/itemofexpense/index.vue'),
      meta: {
         pageTitle: 'itemOfExpense',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'itemOfExpense',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/itemofexpense/edit/id=:id',
      name: 'EditItemOfExpense',
      component: () => import('@/views/salary/itemofexpense/edit.vue'),
      meta: {
         pageTitle: 'itemOfExpense',
         navActiveLink: 'ItemOfExpense',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'itemOfExpense',
               active: true
            }
         ]
      }
   },
  
  
   {
      path: '/salary/tariffscale',
      name: 'TariffScale',
      component: () => import('@/views/salary/tariffscale/index.vue'),
      meta: {
         pageTitle: 'TariffScale',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'TariffScale',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/tariffscale/edit/id=:id',
      name: 'EditTariffScale',
      component: () => import('@/views/salary/tariffscale/edit.vue'),
      meta: {
         pageTitle: 'TariffScale',
         navActiveLink: 'TariffScale',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'TariffScale',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/tariffscalecoef',
      name: 'TariffScaleCoef',
      component: () => import('@/views/salary/tariffscalecoef/index.vue'),
      meta: {
         pageTitle: 'TariffScaleCoef',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'TariffScaleCoef',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/tariffscalecoef/edit/id=:id',
      name: 'EditTariffScaleCoef',
      component: () => import('@/views/salary/tariffscalecoef/edit.vue'),
      meta: {
         pageTitle: 'TariffScaleCoef',
         navActiveLink: 'TariffScaleCoef',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'TariffScaleCoef',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/fixedminimumvalue',
      name: 'FixedMinimumValue',
      component: () => import('@/views/salary/fixedminimumvalue/index.vue'),
      meta: {
         pageTitle: 'FixedMinimumValue',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'FixedMinimumValue',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/fixedminimumvalue/edit/id=:id',
      name: 'EditFixedMinimumValue',
      component: () => import('@/views/salary/fixedminimumvalue/edit.vue'),
      meta: {
         pageTitle: 'FixedMinimumValue',
         navActiveLink: 'FixedMinimumValue',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'FixedMinimumValue',
               active: true
            }
         ]
      }
   },

   {
      path: '/salary/staffTypeBasicTariff',
      name: 'StaffTypeBasicTariff',
      component: () => import('@/views/salary/staffTypeBasicTariff/index.vue'),
      meta: {
         pageTitle: 'staffTypeBasicTariff',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'staffTypeBasicTariff',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/staffTypeBasicTariff/edit/id=:id',
      name: 'EditStaffTypeBasicTariff',
      component: () => import('@/views/salary/staffTypeBasicTariff/edit.vue'),
      meta: {
         pageTitle: 'staffTypeBasicTariff',
         navActiveLink: 'StaffTypeBasicTariff',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'staffTypeBasicTariff',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/sourceCode',
      name: 'SourceCode',
      component: () => import('@/views/salary/sourcecode/index.vue'),
      meta: {
         pageTitle: 'sourceCode',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'sourceCode',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/sourceCode/edit/id=:id',
      name: 'EditSourceCode',
      component: () => import('@/views/salary/sourcecode/edit.vue'),
      meta: {
         pageTitle: 'sourceCode',
         navActiveLink: 'SourceCode',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'sourceCode',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/settlementaccountsource',
      name: 'SettlementAccountSource',
      component: () => import('@/views/salary/settlementaccountsource/index.vue'),
      meta: {
         pageTitle: 'settlementaccountsource',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'settlementaccountsource',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/settlementaccountsource/edit/id=:id',
      name: 'EditSettlementAccountSource',
      component: () => import('@/views/salary/settlementaccountsource/edit.vue'),
      meta: {
         pageTitle: 'settlementaccountsource',
         navActiveLink: 'SettlementAccountSource',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'settlementaccountsource',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/staffing-indicator',
      name: 'StaffingIndicator',
      component: () => import('@/views/salary/staffingindicator/index.vue'),
      meta: {
         pageTitle: 'staffingindicator',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'staffingindicator',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/staffing-indicator/edit/id=:id',
      name: 'EditStaffingIndicator',
      component: () => import('@/views/salary/staffingindicator/edit.vue'),
      meta: {
         pageTitle: 'staffingindicator',
         navActiveLink: 'StaffingIndicator',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'staffingindicator',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/levelCode',
      name: 'LevelCode',
      component: () => import('@/views/salary/levelCode/index.vue'),
      meta: {
         pageTitle: 'levelCode',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'levelCode',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/levelCode/edit/id=:id',
      name: 'EditLevelCode',
      component: () => import('@/views/salary/levelCode/edit.vue'),
      meta: {
         pageTitle: 'levelCode',
         navActiveLink: 'LevelCode',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'levelCode',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/contractoractivitygroup',
      name: 'ContractorActivityGroup',
      component: () => import('@/views/salary/contractoractivitygroup/index.vue'),
      meta: {
         pageTitle: 'ContractorActivityGroup',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'ContractorActivityGroup',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/contractoractivitygroup/edit/id=:id',
      name: 'EditContractorActivityGroup',
      component: () => import('@/views/salary/contractoractivitygroup/edit.vue'),
      meta: {
         pageTitle: 'ContractorActivityGroup',
         navActiveLink: 'ContractorActivityGroup',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'ContractorActivityGroup',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/contractoractivitytype',
      name: 'ContractorActivityType',
      component: () => import('@/views/salary/contractoractivitytype/index.vue'),
      meta: {
         pageTitle: 'ContractorActivityType',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'ContractorActivityType',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/contractoractivitytype/edit/id=:id',
      name: 'EditContractorActivityType',
      component: () => import('@/views/salary/contractoractivitytype/edit.vue'),
      meta: {
         pageTitle: 'ContractorActivityType',
         navActiveLink: 'ContractorActivityType',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'ContractorActivityType',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/qualificationcategory',
      name: 'QualificationCategory',
      component: () => import('@/views/salary/qualificationcategory/index.vue'),
      meta: {
         pageTitle: 'QualificationCategory',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'QualificationCategory',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/qualificationcategory/edit/id=:id',
      name: 'EditQualificationCategory',
      component: () => import('@/views/salary/qualificationcategory/edit.vue'),
      meta: {
         pageTitle: 'QualificationCategory',
         navActiveLink: 'QualificationCategory',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'QualificationCategory',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/needchamberservice',
      name: 'NeedChamberService',
      component: () => import('@/views/salary/needchamberservice/index.vue'),
      meta: {
         pageTitle: 'NeedChamberService',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'NeedChamberService',
               active: true
            }
         ]
      }
   },
   {
      path: '/salary/needchamberservice/edit/id=:id',
      name: 'EditNeedChamberService',
      component: () => import('@/views/salary/needchamberservice/edit.vue'),
      meta: {
         pageTitle: 'NeedChamberService',
         navActiveLink: 'NeedChamberService',
         breadcrumb: [
            {
               text: 'Hrm'
            },
            {
               text: 'NeedChamberService',
               active: true
            }
         ]
      }
   },
];
