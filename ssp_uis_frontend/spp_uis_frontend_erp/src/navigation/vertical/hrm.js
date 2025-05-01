const visiblesTurniket = ['EmployeeTurnstileReport'];

const visibles = [
   'StaffingView',
   'StaffingHeaderView',
   'StaffingForOwnOrgView',
   'StaffingSingleReportView',
   'GetStaffingSingleReportForParentView',
   'EmployeeManageView',
   'TimesheetView',
   'CandidatesConfirmationView',
   'EmployeeSickLeaveView',
   'EmployeeMissedDayView'
];

const visiblesR = [
   'ReportHrmEmployeeActivityInfoView',
   'ReportHrmEmployeeActivityInfoByRegionView',
   ...visiblesTurniket
];

const visibles2 = [
   'PositionClassificationView',
   'EmployeeView',
   'PositionView',
   'DepartmentView',
   'AcademicDegreeView',
   'PartisanshipView',
   'ScientificDegreeView',
   'LanguageProficiencyView',
   'StateAwardView',
   'ElectionMemberView',
   'EducationItemView'
];

const visiblesSigners = [
   'EmployeeLeaveOrderSignerView',
   'OrderToSendBusinessTripSignerView',
   'EmployeeSendTrainSignerView',
   'RecallLeaveSignerView',
   'ChastisementSignerView',
   'TempCalcKindSignerView',
   'WorkDayOffSignerView'
];

const visibles3 = [
   ...visiblesSigners,
   'AppointEmployeeHeaderView',
   'EmployeeLeaveOrderView',
   'AppointEmployeeView',
   'OrderToSendBusinessTripView',
   'EmployeeSendTrainView',
   'EmployeeSendStudyView',
   'EmployeeSickLeaveView',
   'WorkDayOffView',
   'RecallLeaveView',

   'ChastisementView',
   'TempCalcKindView',
   'DocumentHeldForSignView',
   'SignerView',
   'HrmDocumentReport'
];

export default [
   {
      header: 'Hrm',
      route: 'Hrm',
      isParent: true,
      visible: [...visibles, ...visibles2, ...visibles3, ...visiblesR]
   },
   {
      title: 'Info',
      route: 'HrmInfo',
      icon: 'LayersIcon',
      visible: visibles2,
      children: [
         {
            title: 'employee',
            route: 'Employee',
            visible: 'EmployeeView'
         },
         {
            title: 'EmployeeCard',
            route: 'EmployeeCard',
            visible: 'EmployeeView'
         },
         {
            title: 'Department',
            route: 'Department',
            visible: 'DepartmentView'
         },
         {
            title: 'PositionCategory',
            route: 'PositionCategory',
            visible: 'PositionCategoryView'
         },
         {
            title: 'PositionType',
            route: 'PositionType',
            visible: 'PositionTypeView'
         },
         {
            title: 'PositionClassification',
            route: 'PositionClassification',
            visible: 'PositionClassificationView'
         },
         {
            title: 'position',
            route: 'Position',
            visible: 'PositionView'
         },
         {
            title: 'academicDegree',
            route: 'AcademicDegree',
            visible: 'AcademicDegreeView'
         },
         {
            title: 'scientificDegree',
            route: 'ScientificDegree',
            visible: 'ScientificDegreeView'
         },
         {
            title: 'degreeTitles',
            route: 'DegreeTitle',
            visible: 'DegreeTitleView'
         },
         {
            title: 'militaryRanks',
            route: 'MilitaryRank',
            visible: 'MilitaryRankView'
         },
         {
            title: 'languageProficiencys',
            route: 'LanguageProficiency',
            visible: 'LanguageProficiencyView'
         },
         {
            title: 'stateAwards',
            route: 'StateAward',
            visible: 'StateAwardView'
         },
         {
            title: 'partisanships',
            route: 'Partisanship',
            visible: 'PartisanshipView'
         },
         {
            title: 'electionMember',
            route: 'ElectionMember',
            visible: 'ElectionMemberView'
         }
      ]
   },
   {
      title: 'document',
      route: 'HrmDocument',
      icon: 'FileTextIcon',
      visible: visibles,
      children: [
         {
            title: 'staffing',
            route: 'Staffing',
            visible: 'StaffingView'
         },
         {
            title: 'staffingHeader',
            route: 'StaffingHeader',
            visible: 'StaffingHeaderView'
         },

         {
            title: 'StaffingOwnOrg',
            route: 'StaffingOwnOrg',
            visible: 'StaffingForOwnOrgView'
         },
         {
            title: 'GetStaffingSingleReport',
            route: 'GetStaffingSingleReport',
            visible: 'StaffingSingleReportView'
         },
         {
            title: 'GetStaffingSingleReportForParent',
            route: 'GetStaffingSingleReportForParent',
            visible: 'GetStaffingSingleReportForParentView'
         },
         {
            title: 'employeeManage',
            route: 'EmployeeManage',
            visible: 'EmployeeManageView'
         },
         {
            title: 'timesheet',
            route: 'Timesheet',
            visible: 'TimesheetView'
         },
         {
            title: 'CandidatesConfirmation',
            route: 'CandidatesConfirmation',
            visible: 'CandidatesConfirmationView'
         },
         {
            title: 'employeesickleave',
            route: 'EmployeeSickLeave',
            visible: 'EmployeeSickLeaveView'
         },
         {
            title: 'EmployeeMissedDay',
            route: 'EmployeeMissedDay',
            visible: 'EmployeeMissedDayView'
         }
      ]
   },
   {
      title: 'orders',
      route: 'HrmOrder',
      icon: 'ArchiveIcon',
      visible: visibles3,
      children: [
         {
            title: 'DocumentHeldForSign',
            visible: 'SignerView',
            route: 'DocumentHeldForSign'
         },
         {
            title: 'appointemployeeTitle',
            visible: [
               'AppointEmployeeView',
               'WorkDayOffView',
               'ChastisementSignerView',
               'ChastisementSignerView',
               'TempCalcKindView'
            ],
            children: [
               {
                  title: 'appointemployee',
                  visible: 'AppointEmployeeView',
                  route: 'AppointEmployee'
               },
               {
                  title: 'Chastisement',
                  route: 'Chastisement',
                  visible: 'ChastisementView'
               },
               // {
               //    title: 'Chastisement',
               //    route: 'ChastisementForSigner',
               //    visible: 'ChastisementSignerView'
               // },
               {
                  title: 'WorkDayOff',
                  route: 'WorkDayOff',
                  visible: 'WorkDayOffView'
               },
               // {
               //    title: 'WorkDayOff',
               //    route: 'WorkDayOffForSigner',
               //    visible: 'WorkDayOffSignerView'
               // },
               {
                  title: 'tempcalckind',
                  route: 'TempCalcKind',
                  visible: 'TempCalcKindView'
               }
               // {
               //    title: 'tempcalckind',
               //    route: 'TempCalcKindForSigner',
               //    visible: 'TempCalcKindSignerView'
               // }
            ]
         },
         {
            title: 'OrderToSendBusinessTripTitle',
            visible: ['OrderToSendBusinessTripView', 'OrderToSendBusinessTripSignerView'],
            children: [
               {
                  title: 'OrderToSendBusinessTrip',
                  route: 'OrderToSendBusinessTrip',
                  visible: 'OrderToSendBusinessTripView'
               }
               // {
               //    title: 'OrderToSendBusinessTrip',
               //    route: 'OrderToSendBusinessTripForSigner',
               //    visible: 'OrderToSendBusinessTripSignerView'
               // }
            ]
         },
         {
            title: 'employeeleaveorderTitle',
            visible: [
               'EmployeeLeaveOrderView',
               'EmployeeLeaveOrderSignerView',
               'EmployeeSendTrainView',
               'EmployeeSendStudyView',
               'EmployeeSendTrainSignerView',
               'RecallLeaveSigner',
               'RecallLeaveSignerView'
            ],
            children: [
               {
                  title: 'employeeleaveorder',
                  route: 'EmployeeLeaveOrder',
                  visible: 'EmployeeLeaveOrderView'
               },
               // {
               //    title: 'employeeleaveorder',
               //    route: 'EmployeeLeaveOrderForSigner',
               //    visible: 'EmployeeLeaveOrderSignerView'
               // },
               {
                  title: 'employeesendtrain',
                  route: 'EmployeeSendTrain',
                  visible: 'EmployeeSendTrainView'
               },
               // {
               //    title: 'employeesendtrain',
               //    route: 'EmployeeSendTrainForSigner',
               //    visible: 'EmployeeSendTrainSignerView'
               // },
               {
                  title: 'employeesendstudy',
                  route: 'EmployeeSendStudy',
                  visible: 'EmployeeSendStudyView'
               },
               // {
               //    title: 'employeesendstudy',
               //    route: 'EmployeeSendStudyForSigner',
               //    visible: 'EmployeeSendStudySignerView'
               // },
               {
                  title: 'RecallLeave',
                  route: 'RecallLeave',
                  visible: 'RecallLeaveView'
               },
               // {
               //    title: 'RecallLeave',
               //    route: 'RecallLeaveForSigner',
               //    visible: 'RecallLeaveSignerView'
               // },
               {
                  title: 'bolaparvarishi',
                  route: 'Bolaparvarishi',
                  visible: 'EmployeeLeaveOrderView'
               },
               {
                  title: 'Homiladorliktatili',
                  route: 'Homiladorliktatili',
                  visible: 'EmployeeLeaveOrderView'
               }
            ]
         }
      ]
   },
   {
      title: 'Report',
      route: 'HrmReport',
      icon: 'ClipboardIcon',
      visible: visiblesR,
      children: [
         {
            title: 'GetStaffCountReport',
            route: 'GetStaffCountReport',
            visible: true
         },
         {
            title: 'GetStaffCountByGenderReport',
            route: 'GetStaffCountByGenderReport',
            visible: true
         },
         {
            title: 'gethrmemployeeactivityreport',
            route: 'GetHrmEmployeeActivityReport',
            visible: 'ReportHrmEmployeeActivityInfoView'
         },
         {
            title: 'GetHrmEmployeeActivityRegionReport',
            route: 'GetHrmEmployeeActivityRegionReport',
            visible: 'ReportHrmEmployeeActivityInfoByRegionView'
         },
         {
            title: 'TurnstileReport',
            route: 'HrmReport',
            icon: 'ClipboardIcon',
            visible: visiblesTurniket,
            children: [
               {
                  title: 'EmployeeTurnstileReport',
                  route: 'EmployeeTurnstileReport',
                  visible: 'EmployeeTurnstileReport'
               },
               {
                  title: 'EmployeeTurnstileReportById',
                  route: 'EmployeeTurnstileReportById',
                  visible: 'EmployeeTurnstileReport'
               },
               {
                  title: 'NewViewReport',
                  route: 'NewViewReport',
                  visible: 'EmployeeTurnstileReport'
               }
            ]
         },
         {
            title: 'GetReportDocumentsForHrm',
            route: 'GetReportDocumentsForHrm',
            visible: 'HrmDocumentReport'
         }
      ]
   }
];
