export default [
   {
      path: '/info/prtncontracttype',
      name: 'PrtnContractType',
      component: () => import('@/views/info/prtncontracttype/index.vue'),
      meta: {
         pageTitle: 'prtnContractType',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'prtnContractType',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/prtncontracttype/edit/id=:id',
      name: 'EditPrtnContractType',
      component: () => import('@/views/info/prtncontracttype/edit.vue'),
      meta: {
         pageTitle: 'prtnContractType',
         navActiveLink: 'PrtnContractType',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'prtnContractType',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/prtnrejectreason',
      name: 'PrtnRejectReason',
      component: () => import('@/views/info/prtnrejectreason/index.vue'),
      meta: {
         pageTitle: 'prtnRejectReason',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'prtnRejectReason',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/prtnrejectreason/edit/id=:id',
      name: 'EditPrtnRejectReason',
      component: () => import('@/views/info/prtnrejectreason/edit.vue'),
      meta: {
         pageTitle: 'prtnRejectReason',
         navActiveLink: 'PrtnRejectReason',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'prtnRejectReason',
               active: true
            }
         ]
      }
   },
   // EducationItem
   {
      path: '/info/electionitem',
      name: 'EducationItem',
      component: () => {
         return import('@/views/info/educationitem/index.vue');
      },
      meta: {
         pageTitle: 'EducationItem',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'EducationItem',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/electionitem/edit/id=:id',
      name: 'EditElectionItem',
      component: () => import('@/views/info/educationitem/edit.vue'),
      meta: {
         pageTitle: 'EducationItem',
         navActiveLink: 'EducationItem',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'EducationItem',
               active: true
            }
         ]
      }
   },
   // EducationItem

   {
      path: '/info/mfy',
      name: 'Mfy',
      component: () => import('@/views/info/mfy/index.vue'),
      meta: {
         pageTitle: 'mfy',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'mfy',
               active: true
            }
         ]
      }
   },

   {
      path: '/info/country',
      name: 'country',
      component: () => import('@/views/info/country/index.vue'),
      meta: {
         pageTitle: 'Country',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'Country',
               active: true
            }
         ]
      }
   },

   {
      path: '/info/country/edit/id=:id',
      name: 'EditCountry',
      component: () => import('@/views/info/country/edit.vue'),
      meta: {
         pageTitle: 'country',
         navActiveLink: 'country',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'country',
               active: true
            }
         ]
      }
   },

   {
      path: '/info/contractor',
      name: 'Contractor',
      component: () => import('@/views/info/contractor/index.vue'),
      meta: {
         pageTitle: 'contractor',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'contractor',
               active: true
            }
         ]
      }
   },

   {
      path: '/info/contractor/view/id=:id',
      name: 'ViewContractor',
      component: () => import('@/views/info/contractor/view.vue'),
      meta: {
         pageTitle: 'contractor',
         navActiveLink: 'Contractor',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'contractor',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/district',
      name: 'District',
      component: () => import('@/views/info/district/index.vue'),
      meta: {
         pageTitle: 'Region',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'Region',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/district/edit/id=:id',
      name: 'EditDistrict',
      component: () => import('@/views/info/district/edit.vue'),
      meta: {
         pageTitle: 'district',
         navActiveLink: 'District',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'district',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/oked',
      name: 'oked',
      component: () => import('@/views/info/oked/index.vue'),
      meta: {
         pageTitle: 'Oked',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'Oked',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/region',
      name: 'region',
      component: () => import('@/views/info/region/index.vue'),
      meta: {
         pageTitle: 'Oblast',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'Oblast',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/region/edit/id=:id',
      name: 'EditRegion',
      component: () => import('@/views/info/region/edit.vue'),
      meta: {
         pageTitle: 'region',
         navActiveLink: 'region',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'region',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/organizationlegalform',
      name: 'OrganizationLegalForm',
      component: () => import('@/views/info/organizationlegalform/index.vue'),
      meta: {
         pageTitle: 'OrganizationLegalForm',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'OrganizationLegalForm',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/organizationlegalform/edit/id=:id',
      name: 'EditOrganizationLegalForm',
      component: () => import('@/views/info/organizationlegalform/edit.vue'),
      meta: {
         pageTitle: 'OrganizationLegalForm',
         navActiveLink: 'OrganizationLegalForm',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'OrganizationLegalForm',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/relativedegree',
      name: 'RelativeDegree',
      component: () => import('@/views/info/relativedegree/index.vue'),
      meta: {
         pageTitle: 'RelativeDegree',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'RelativeDegree',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/relativedegree/edit/id=:id',
      name: 'EditRelativeDegree',
      component: () => import('@/views/info/relativedegree/edit.vue'),
      meta: {
         pageTitle: 'RelativeDegree',
         navActiveLink: 'RelativeDegree',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'RelativeDegree',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/uniteofmeasure',
      name: 'UniteOfMeasure',
      component: () => import('@/views/info/uniteofmeasure/index.vue'),
      meta: {
         pageTitle: 'UniteOfMeasure',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'UniteOfMeasure',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/uniteofmeasure/edit/id=:id',
      name: 'EditUniteOfMeasure',
      component: () => import('@/views/info/uniteofmeasure/edit.vue'),
      meta: {
         pageTitle: 'UniteOfMeasure',
         navActiveLink: 'UniteOfMeasure',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'UniteOfMeasure',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/identitydocument',
      name: 'IdentityDocument',
      component: () => import('@/views/info/identitydocument/index.vue'),
      meta: {
         pageTitle: 'IdentityDocument',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'IdentityDocument',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/identitydocument/edit/id=:id',
      name: 'EditIdentityDocument',
      component: () => import('@/views/info/identitydocument/edit.vue'),
      meta: {
         pageTitle: 'IdentityDocument',
         navActiveLink: 'IdentityDocument',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'IdentityDocument',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/workschedule',
      name: 'WorkSchedule',
      component: () => import('@/views/info/workschedule/index.vue'),
      meta: {
         pageTitle: 'WorkSchedule',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'WorkSchedule',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/workschedule/edit/id=:id',
      name: 'EditWorkSchedule',
      component: () => import('@/views/info/workschedule/edit.vue'),
      meta: {
         pageTitle: 'WorkSchedule',
         navActiveLink: 'WorkSchedule',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'WorkSchedule',
               active: true
            }
         ]
      }
   },

   {
      path: '/info/organizationalstructure',
      name: 'OrganizationalStructure',
      component: () => import('@/views/info/organizationalstructure/index.vue'),
      meta: {
         pageTitle: 'organizationalstructure',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'organizationalstructure',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/organizationalstructure/edit/id=:id',
      name: 'EditOrganizationalStructure',
      component: () => import('@/views/info/organizationalstructure/edit.vue'),
      meta: {
         pageTitle: 'organizationalstructure',
         navActiveLink: 'OrganizationalStructure',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'organizationalstructure',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/claimtheme',
      name: 'ClaimTheme',
      component: () => import('@/views/info/claimtheme/index.vue'),
      meta: {
         pageTitle: 'ClaimTheme',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ClaimTheme',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/claimtheme/edit/id=:id',
      name: 'EditClaimTheme',
      component: () => import('@/views/info/claimtheme/edit.vue'),
      meta: {
         pageTitle: 'ClaimTheme',
         navActiveLink: 'ClaimTheme',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ClaimTheme',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/claimorganization',
      name: 'ClaimOrganization',
      component: () => import('@/views/info/claimorganization/index.vue'),
      meta: {
         pageTitle: 'ClaimOrganization',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ClaimOrganization',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/claimorganization/edit/id=:id',
      name: 'EditClaimOrganization',
      component: () => import('@/views/info/claimorganization/edit.vue'),
      meta: {
         pageTitle: 'ClaimOrganization',
         navActiveLink: 'ClaimOrganization',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ClaimOrganization',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/claimorganizationtype',
      name: 'ClaimOrganizationType',
      component: () => import('@/views/info/claimorganizationtype/index.vue'),
      meta: {
         pageTitle: 'ClaimOrganizationType',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ClaimOrganizationType',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/claimorganizationtype/edit/id=:id',
      name: 'EditClaimOrganizationType',
      component: () => import('@/views/info/claimorganizationtype/edit.vue'),
      meta: {
         pageTitle: 'ClaimOrganizationType',
         navActiveLink: 'ClaimOrganizationType',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ClaimOrganizationType',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/contractorunionactivitytype',
      name: 'ContractorUnionActivityType',
      component: () => import('@/views/info/contractorunionactivitytype/index.vue'),
      meta: {
         pageTitle: 'ContractorUnionActivityType',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ContractorUnionActivityType',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/contractorunionactivitytype/edit/id=:id',
      name: 'EditContractorUnionActivityType',
      component: () => import('@/views/info/contractorunionactivitytype/edit.vue'),
      meta: {
         pageTitle: 'ContractorUnionActivityType',
         navActiveLink: 'ContractorUnionActivityType',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'ContractorUnionActivityType',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/bank',
      name: 'Bank',
      component: () => import('@/views/info/bank/index.vue'),
      meta: {
         pageTitle: 'bankid',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'bankid',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/bank/edit/id=:id',
      name: 'EditBank',
      component: () => import('@/views/info/bank/edit.vue'),
      meta: {
         pageTitle: 'bankid',
         navActiveLink: 'Bank',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'bankid',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/GetSmsLogReport',
      name: 'GetSmsLogReport',
      component: () => import('@/views/info/getSmslogreport/index.vue'),
      meta: {
         pageTitle: 'GetSmsLogReport',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'GetSmsLogReport',
               active: true
            }
         ]
      }
   },
   {
      path: '/info/GetQqsAylanma',
      name: 'GetQqsAylanma',
      component: () => import('@/views/info/memship/GetQqsAylanma.vue'),
      meta: {
         pageTitle: 'GetQqsAylanma',
         breadcrumb: [
            {
               text: 'Info'
            },
            {
               text: 'GetQqsAylanma',
               active: true
            }
         ]
      }
   }
];
