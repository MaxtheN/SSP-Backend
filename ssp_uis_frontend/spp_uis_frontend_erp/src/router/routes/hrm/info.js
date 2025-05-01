const breadcrumbDef = [
    {
        text: 'Hrm',
        to: 'Hrm'
    },
    {
        text: 'Info',
        to: 'HrmInfo'
    }
]


export default [
    {
        path: '/hrm/info',
        name: 'HrmInfo',
        component: () => import('@/components/WRouterView.vue'),
        redirect: '/',
        children: [
            {
                path: '/',
                name: 'HrmInfo',
                component: () => import('@/views/hrm/index.vue'),
                meta: {
                    pageTitle: 'Info',
                    breadcrumb: [...breadcrumbDef]
                }
            },
            {
                path: '/hrm/info/employee',
                name: 'Employee',
                component: () => import('@/views/hrm/info/employee/index.vue'),
                meta: {
                    pageTitle: 'employee',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'employee',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/employee/edit/id=:id',
                name: 'EditEmployee',
                component: () => import('@/views/hrm/info/employee/edit.vue'),
                meta: {
                    pageTitle: 'employee',
                    navActiveLink: 'Employee',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'employee',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/employeecard',
                name: 'EmployeeCard',
                component: () => import('@/views/hrm/info/employeecard/index.vue'),
                meta: {
                    pageTitle: 'EmployeeCard',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'EmployeeCard',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/department',
                name: 'Department',
                component: () => import('@/views/hrm/info/department/index.vue'),
                meta: {
                    pageTitle: 'Department',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'Department',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/department/edit/id=:id',
                name: 'EditDepartment',
                component: () => import('@/views/hrm/info/department/edit.vue'),
                meta: {
                    pageTitle: 'Department',
                    navActiveLink: 'Department',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'Department',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/positioncategory',
                name: 'PositionCategory',
                component: () => import('@/views/hrm/info/positioncategory/index.vue'),
                meta: {
                    pageTitle: 'PositionCategory',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'PositionCategory',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/positioncategory/edit/id=:id',
                name: 'EditPositionCategory',
                component: () => import('@/views/hrm/info/positioncategory/edit.vue'),
                meta: {
                    pageTitle: 'PositionCategory',
                    navActiveLink: 'PositionCategory',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'PositionCategory',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/positiontype',
                name: 'PositionType',
                component: () => import('@/views/hrm/info/positiontype/index.vue'),
                meta: {
                    pageTitle: 'PositionType',
                    breadcrumb: [
                        ...breadcrumbDef,

                        {
                            text: 'PositionType',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/positiontype/edit/id=:id',
                name: 'EditPositionType',
                component: () => import('@/views/hrm/info/positiontype/edit.vue'),
                meta: {
                    pageTitle: 'PositionType',
                    navActiveLink: 'PositionType',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'PositionType',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/positionclassification',
                name: 'PositionClassification',
                component: () => import('@/views/hrm/info/positionclassification/index.vue'),
                meta: {
                    pageTitle: 'PositionClassification',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'PositionClassification',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/positionclassification/edit/id=:id',
                name: 'EditPositionClassification',
                component: () => import('@/views/hrm/info/positionclassification/edit.vue'),
                meta: {
                    pageTitle: 'PositionClassification',
                    navActiveLink: 'PositionClassification',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'PositionClassification',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/position',
                name: 'Position',
                component: () => import('@/views/hrm/info/position/index.vue'),
                meta: {
                    pageTitle: 'position',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'position',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/position/edit/id=:id',
                name: 'EditPosition',
                component: () => import('@/views/hrm/info/position/edit.vue'),
                meta: {
                    pageTitle: 'position',
                    navActiveLink: 'Position',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'position',
                            active: true
                        }
                    ]
                }
            },

            {
                path: '/hrm/info/academicdegree',
                name: 'AcademicDegree',
                component: () => import('@/views/hrm/info/academicdegree/index.vue'),
                meta: {
                    pageTitle: 'academicDegree',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'academicDegree',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/academicdegree/edit/id=:id',
                name: 'EditAcademicDegree',
                component: () => import('@/views/hrm/info/academicdegree/edit.vue'),
                meta: {
                    pageTitle: 'AcademicDegree',
                    navActiveLink: 'AcademicDegree',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'AcademicDegree',
                            active: true
                        }
                    ]
                }
            },

            {
                path: '/hrm/info/scientificdegree/edit/id=:id',
                name: 'EditScientificDegree',
                component: () => import('@/views/hrm/info/scientificdegree/edit.vue'),
                meta: {
                    pageTitle: 'scientificDegree',
                    navActiveLink: 'ScientificDegree',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'scientificDegree',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/scientificdegree',
                name: 'ScientificDegree',
                component: () => import('@/views/hrm/info/scientificdegree/index.vue'),
                meta: {
                    pageTitle: 'scientificDegree',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'scientificDegree',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/degreetitle',
                name: 'DegreeTitle',
                component: () => import('@/views/hrm/info/degreetitle/index.vue'),
                meta: {
                    pageTitle: 'degreeTitles',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'degreeTitles',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/degreetitle/edit/id=:id',
                name: 'EditDegreeTitle',
                component: () => import('@/views/hrm/info/degreetitle/edit.vue'),
                meta: {
                    pageTitle: 'degreeTitles',
                    navActiveLink: 'DegreeTitle',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'degreeTitles',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/militaryrank',
                name: 'MilitaryRank',
                component: () => import('@/views/hrm/info/militaryrank/index.vue'),
                meta: {
                    pageTitle: 'militaryRanks',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'militaryRanks',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/militaryrank/edit/id=:id',
                name: 'EditMilitaryRank',
                component: () => import('@/views/hrm/info/militaryrank/edit.vue'),
                meta: {
                    pageTitle: 'militaryRanks',
                    navActiveLink: 'MilitaryRank',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'militaryRanks',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/languageproficiency',
                name: 'LanguageProficiency',
                component: () => import('@/views/hrm/info/languageproficiency/index.vue'),
                meta: {
                    pageTitle: 'languageProficiencys',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'languageProficiencys',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/languageproficiency/edit/id=:id',
                name: 'EditLanguageProficiency',
                component: () => import('@/views/hrm/info/languageproficiency/edit.vue'),
                meta: {
                    pageTitle: 'languageProficiencys',
                    navActiveLink: 'LanguageProficiency',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'languageProficiencys',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/stateaward',
                name: 'StateAward',
                component: () => import('@/views/hrm/info/stateaward/index.vue'),
                meta: {
                    pageTitle: 'stateAwards',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'stateAwards',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/stateaward/edit/id=:id',
                name: 'EditStateAward',
                component: () => import('@/views/hrm/info/stateaward/edit.vue'),
                meta: {
                    pageTitle: 'stateAwards',
                    navActiveLink: 'StateAward',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'stateAwards',
                            active: true
                        }
                    ]
                }
            },

            {
                path: '/hrm/info/partisanship/edit/id:=id',
                name: 'EditPartisanship',
                component: () => import('@/views/hrm/info/partisanship/edit.vue'),
                meta: {
                    pageTitle: 'partisanships',
                    navActiveLink: 'Partisanship',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'partisanships',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/partisanship',
                name: 'Partisanship',
                component: () => import('@/views/hrm/info/partisanship/index.vue'),
                meta: {
                    pageTitle: 'partisanships',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'partisanships',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/electionmember',
                name: 'ElectionMember',
                component: () => import('@/views/hrm/info/electionmember/index.vue'),
                meta: {
                    pageTitle: 'electionMember',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'electionMember',
                            active: true
                        }
                    ]
                }
            },
            {
                path: '/hrm/info/electionmember/edit/id=:id',
                name: 'EditElectionMember',
                component: () => import('@/views/hrm/info/electionmember/edit.vue'),
                meta: {
                    pageTitle: 'electionMember',
                    navActiveLink: 'ElectionMember',
                    breadcrumb: [
                        ...breadcrumbDef,
                        {
                            text: 'electionMember',
                            active: true
                        }
                    ]
                }
            },
        ]
    },
]