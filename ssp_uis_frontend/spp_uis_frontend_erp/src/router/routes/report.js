export default [
   {
      path: '/report/prtnapplicationandcontractinfo',
      name: 'PrtnApplicationAndContractInfo',
      component: () => import('@/views/report/PrtnApplicationAndContractInfo/index.vue'),
      meta: {
         pageTitle: 'prtnapplicationandcontractinfo',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'prtnapplicationandcontractinfo',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/prtnapplicationandcontractinfo2',
      name: 'PrtnApplicationAndContractInfo2',
      component: () => import('@/views/report/PrtnApplicationAndContractInfo2/index.vue'),
      meta: {
         pageTitle: 'prtnapplicationandcontractinfo2',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'prtnapplicationandcontractinfo2',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/prtnapplicationandcontractinfbyregion',
      name: 'GetPrtnApplicationAndContractInfoByRegion',
      component: () => import('@/views/report/GetPrtnApplicationAndContractInfoByRegion/index.vue'),
      meta: {
         pageTitle: 'GetPrtnApplicationAndContractInfoByRegion',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetPrtnApplicationAndContractInfoByRegion',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/getprtnapplicationbycontracttype',
      name: 'GetPrtnApplicationByContractType',
      component: () => import('@/views/report/GetPrtnApplicationByContractType/index.vue'),
      meta: {
         pageTitle: 'getprtnapplicationbycontracttype',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'getprtnapplicationbycontracttype',
               active: true
            }
         ]
      }
   },
   // 5 reports

   {
      path: '/report/davaktiv',
      name: 'davaktiv',
      component: () => import('@/views/report/davaktiv/index.vue'),
      meta: {
         pageTitle: 'davaktiv',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'davaktiv',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/jamgarma',
      name: 'jamgarma',
      component: () => import('@/views/report/jamgarma/index.vue'),
      meta: {
         pageTitle: 'jamgarma',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'jamgarma',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetTadbirkorFundReport',
      name: 'GetTadbirkorFundReport',
      component: () => import('@/views/report/GetTadbirkorFundReport/index.vue'),
      meta: {
         pageTitle: 'GetTadbirkorFundReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetTadbirkorFundReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/iqtisodiyot',
      name: 'iqtisodiyot',
      component: () => import('@/views/report/iqtisodiyot/index.vue'),
      meta: {
         pageTitle: 'iqtisodiyot',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'iqtisodiyot',
               active: true
            }
         ]
      }
   },
   //the end

   {
      path: '/report/creditbyname',
      name: 'creditbyname',
      component: () => import('@/views/report/creditbyname/index.vue'),
      meta: {
         pageTitle: 'creditbyname',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'creditbyname',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/fundbyname',
      name: 'fundbyname',
      component: () => import('@/views/report/fundbyname/index.vue'),
      meta: {
         pageTitle: 'fundbyname',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'fundbyname',
               active: true
            }
         ]
      }
   },

   {
      path: '/report/GetOffertaCalculate',
      name: 'GetOffertaCalculate',
      component: () => import('@/views/report/GetOffertaCalculate/index.vue'),
      meta: {
         pageTitle: 'GetOffertaCalculate',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetOffertaCalculate',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetFreeAreaFromBandlik',
      name: 'GetFreeAreaFromBandlik',
      component: () => import('@/views/report/GetFreeAreaFromBandlik/index.vue'),
      meta: {
         pageTitle: 'GetFreeAreaFromBandlik',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetFreeAreaFromBandlik',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/PrtnEmploymentGraphReport',
      name: 'PrtnEmploymentGraphReport',
      component: () => import('@/views/report/PrtnEmploymentGraphReport/index.vue'),
      meta: {
         pageTitle: 'PrtnEmploymentGraphReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'PrtnEmploymentGraphReport',
               active: true
            }
         ]
      }
   },

   {
      path: '/report/MemshipApplicationAndContractInfoReport',
      name: 'MemshipApplicationAndContractInfoReport',
      component: () => import('@/views/report/memship/MemshipApplicationAndContractInfoReport.vue'),
      meta: {
         pageTitle: 'MemshipApplicationAndContractInfoReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'MemshipApplicationAndContractInfoReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetMemshipReports',
      name: 'GetMemshipReports',
      component: () => import('@/views/report/memship/GetMemshipReports.vue'),
      meta: {
         pageTitle: 'GetMemshipReports',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetMemshipReports',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetPaidMemshipReport',
      name: 'GetPaidMemshipReport',
      component: () => import('@/views/report/memship/GetPaidMemshipReport.vue'),
      meta: {
         pageTitle: 'GetPaidMemshipReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetPaidMemshipReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/Report/GetMemshipContract',
      name: 'GetMemshipContract',
      component: () => import('@/views/report/memship/GetMemshipContract.vue'),
      meta: {
         pageTitle: 'GetMemshipContract',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetMemshipContract',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetMemshipApplicationReportByPersonType',
      name: 'GetMemshipApplicationReportByPersonType',
      component: () => import('@/views/report/memship/GetMemshipApplicationReportByPersonType.vue'),
      meta: {
         pageTitle: 'GetMemshipApplicationReportByPersonType',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetMemshipApplicationReportByPersonType',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetMemshipApplicationReportByOrganizations',
      name: 'GetMemshipApplicationReportByOrganizations',
      component: () => import('@/views/report/memship/GetMemshipApplicationReportByOrganizations.vue'),
      meta: {
         pageTitle: 'GetMemshipApplicationReportByOrganizations',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetMemshipApplicationReportByOrganizations',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetBankCreditApplicationReportByRegionAndDistrict',
      name: 'GetBankCreditApplicationReportByRegionAndDistrict',
      component: () => import('@/views/report/GetBankCreditApplicationReportByRegionAndDistrict/index.vue'),
      meta: {
         pageTitle: 'GetBankCreditApplicationReportByRegionAndDistrict',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetBankCreditApplicationReportByRegionAndDistrict',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetBankCreditApplicationReportByRegionAndDistrict2',
      name: 'GetBankCreditApplicationReportByRegionAndDistrict2',
      component: () => import('@/views/report/GetBankCreditApplicationReportByRegionAndDistrict2/index.vue'),
      meta: {
         pageTitle: 'GetBankCreditApplicationReportByRegionAndDistrict2',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetBankCreditApplicationReportByRegionAndDistrict2',
               active: true
            }
         ]
      }
   },

   {
      path: '/report/GetTaxQqsAylanmaReport',
      name: 'GetTaxQqsAylanmaReport',
      component: () => import('@/views/report/memship/GetTaxQqsAylanmaReport.vue'),
      meta: {
         pageTitle: 'GetTaxQqsAylanmaReport',
         breadcrumb: [
            { text: 'Report' },
            {
               text: 'GetTaxQqsAylanmaReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetMemshipDocsInfoReestr',
      name: 'GetMemshipDocsInfoReestr',
      component: () => import('@/views/report/memship/GetMemshipDocsInfoReestr.vue'),
      meta: {
         pageTitle: 'GetMemshipDocsInfoReestr',
         breadcrumb: [
            { text: 'Report' },
            {
               text: 'GetMemshipDocsInfoReestr',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetContractorCategoryType',
      name: 'GetContractorCategoryType',
      component: () => import('@/views/report/memship/GetContractorCategoryType.vue'),
      meta: {
         pageTitle: 'GetContractorCategoryType',
         breadcrumb: [
            { text: 'Report' },
            {
               text: 'GetContractorCategoryType',
               active: true
            }
         ]
      }
   },
   // claimReport
   {
      path: '/report/SummaOfClaimApplicationReport',
      name: 'SummaOfClaimApplicationReport',
      component: () => import('@/views/report/claimReport/summaOfClaim/index.vue'),
      meta: {
         pageTitle: 'SummaOfClaimApplicationReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'SummaOfClaimApplicationReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/ClaimApplicationAmount',
      name: 'ClaimApplicationAmount',
      component: () => import('@/views/report/claimReport/ClaimApplicationAmount/index.vue'),
      meta: {
         pageTitle: 'ClaimApplicationAmount',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'ClaimApplicationAmount',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/ReceivedClaimApplicationReport',
      name: 'ReceivedClaimApplicationReport',
      component: () => import('@/views/report/claimReport/ReceivedClaimApplicationReport/index.vue'),
      meta: {
         pageTitle: 'ReceivedClaimApplicationReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'ReceivedClaimApplicationReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/AppealsSentToClaimApplicationReport',
      name: 'AppealsSentToClaimApplicationReport',
      component: () => import('@/views/report/claimReport/AppealsSentToClaimApplicationReport/index.vue'),
      meta: {
         pageTitle: 'AppealsSentToClaimApplicationReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'AppealsSentToClaimApplicationReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetClaimApplicationReport',
      name: 'GetClaimApplicationReport',
      component: () => import('@/views/report/claimReport/GetClaimApplicationReport/index.vue'),
      meta: {
         pageTitle: 'GetClaimApplicationReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetClaimApplicationReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/ClaimApplicationReport',
      name: 'ClaimApplicationReport',
      component: () => import('@/views/report/claimReport/ClaimApplicationReport/index.vue'),
      meta: {
         pageTitle: 'ClaimApplicationReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'ClaimApplicationReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetClaimApplicationAmount',
      name: 'GetClaimApplicationAmount',
      component: () => import('@/views/report/claimReport/GetClaimApplicationAmount/index.vue'),
      meta: {
         pageTitle: 'GetClaimApplicationAmount',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetClaimApplicationAmount',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/PrtnEmploymentGraphNewReport',
      name: 'PrtnEmploymentGraphNewReport',
      component: () => import('@/views/report/PrtnEmploymentGraphNewReport/index.vue'),
      meta: {
         pageTitle: 'PrtnEmploymentGraphNewReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'PrtnEmploymentGraphNewReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/newreport',
      name: 'newreport',
      component: () => import('@/views/report/newreport/index.vue'),
      meta: {
         pageTitle: 'newreport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'newreport',
               active: true
            }
         ]
      }
   },

   // coruptionreport

   {
      path: '/report/GetCharterMembersRegisterReport',
      name: 'GetCharterMembersRegisterReport',
      component: () => import('@/views/corruption/report/GetCharterMembersRegisterReport.vue'),
      meta: {
         pageTitle: 'GetCharterMembersRegisterReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetCharterMembersRegisterReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetAntiCorruptionByRegion',
      name: 'GetCorruptionByRegion',
      component: () => import('@/views/corruption/report/GetCorruptionByRegion.vue'),
      meta: {
         pageTitle: 'GetCorruptionByRegion',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetCorruptionByRegion',
               active: true
            }
         ]
      }
   },

   {
      path: '/report/MonoApplicationReport',
      name: 'MonoApplicationReport',
      component: () => import('@/views/report/MonoApplicationReport/index.vue'),
      meta: {
         pageTitle: 'MonoApplicationReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'MonoApplicationReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/ReportOnProjectImplement',
      name: 'ReportOnProjectImplement',
      component: () => import('@/views/report/ReportOnProjectImplement/index.vue'),
      meta: {
         pageTitle: 'ReportOnProjectImplement',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'ReportOnProjectImplement',
               active: true
            }
         ]
      }
   },
   {
      path: '/report/GetExpiredContractorsReport',
      name: 'GetExpiredContractorsReport',
      component: () => import('@/views/report/GetExpiredContractorsReport/index.vue'),
      meta: {
         pageTitle: 'GetExpiredContractorsReport',
         breadcrumb: [
            {
               text: 'Report'
            },
            {
               text: 'GetExpiredContractorsReport',
               active: true
            }
         ]
      }
   }
];
