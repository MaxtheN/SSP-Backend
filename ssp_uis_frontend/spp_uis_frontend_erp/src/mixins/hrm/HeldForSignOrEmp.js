export default {
   data() {
      return {
         OrderList: [
            {
               value: 85,
               text: this.$t('appointemployee'),
               route: 'ViewAppointEmployee'
            },
            {
               value: 88,
               text: this.$t('employeesendtrain'),
               route: 'ViewEmployeeSendTrain'
            },
            {
               value: 89,
               text: this.$t('employeeleaveorder'),
               route: 'ViewEmployeeLeaveOrder'
            },
            {
               value: 90,
               text: this.$t('RecallLeave'),
               route: 'ViewRecallLeave'
            },
            {
               value: 91,
               text: this.$t('WorkDayOff'),
               route: 'ViewWorkDayOff'
            },
            {
               value: 92,
               text: this.$t('tempcalckind'),
               route: 'ViewTempCalcKind'
            },
            {
               value: 135,
               text: this.$t('OrderToSendBusinessTrip'),
               route: 'ViewOrderToSendBusinessTrip'
            },
            {
               value: 136,
               text: this.$t('Chastisement'),
               route: 'ViewChastisement'
            },
            {
               value: 143,
               text: this.$t('employeesendstudy'),
               route: 'ViewEmployeeSendStudy'
            }
            // {
            //    value: 143,
            //    text: this.$t('employeesendstudy'),
            //    route: 'ViewEmployeeSendStudy'
            // }
         ],
         filter: {
            search: '',
            docNumber: '',
            statusId: null,
            tableId: null,
            employee: null,
            personId: null,
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         fields: [
            {
               key: 'id',
               label: this.$t('ID'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('ondate'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'tableId',
               label: this.$t('orders'),
               thStyle: {
                  minWidth: '300px'
               },
               sortable: true
            },
            {
               key: 'employee',
               label: this.$t('employee')
            },
            {
               key: 'details',
               label: this.$t('details')
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ]
      };
   },
   computed: {
      viewName() {
         return (tableId) => {
            return this.OrderList.find((e) => e.value == tableId)?.route;
         };
      },
      TableName() {
         return (tableId) => {
            return this.OrderList.find((e) => e.value == tableId)?.text;
         };
      }
   }
};
