import ToastificationContent from '@core/components/toastification/ToastificationContent.vue';

export default {
   components: {},
   data() {
      return {};
   },
   computed: {},
   methods: {
      isEmpty(obj) {
         // eslint-disable-next-line no-restricted-syntax
         for (const prop in obj) {
            if (Object.hasOwn(obj, prop)) {
               return false;
            }
         }
         return true;
      },
      forceFileDownload(response, name, type = '.xlsx') {
         var blob = new Blob([response.data]);
         const url = window.URL.createObjectURL(blob);
         const link = document.createElement('a');
         link.href = url;
         link.setAttribute('download', name + type); //or any other extension
         document.body.appendChild(link);
         link.click();
      },
      currency(amount, fractionCount = 0) {
         return this.$options.filters.currency(
            amount,

            {
               symbol: '',
               thousandsSeparator: ' ',
               fractionCount: fractionCount
            }
         );
      },
      getColor(item) {
         if (
            item.statusId == 24 ||
            item.statusId == 25 ||
            item.statusId == 23 ||
            item.statusId == 5 ||
            item.statusId == 3 ||
            item.statusId == 29 ||
            (item.statusId == 10 && !item.step)
         ) {
            return 'danger';
         } else if (
            (item.statusId == 13 && !item.step) ||
            item.statusId == 11 ||
            item.statusId == 26 ||
            item.statusId == 9 ||
            item.statusId == 14 ||
            item.statusId == 16 ||
            item.statusId == 17 ||
            item.statusId == 18 ||
            (item.statusId == 21 && !item.step) ||
            item.statusId == 28 ||
            item.statusId == 2
         ) {
            return 'success';
         } else if (item.statusId == 6 || item.statusId == 30) {
            return 'warning';
         } else if (item.statusId == 7) {
            return 'info';
         } else if (item.statusId == 13 && item.step) {
            return 'danger';
         } else if (item.statusId == 21 && item.step) {
            return 'danger';
         } else if (item.statusId == 10 && item.step) {
            return 'info';
         } else {
            return 'primary';
         }
      },
      getColorStatus(item) {
         if (
            item.prtnContractStatusId == 24 ||
            item.prtnContractStatusId == 25 ||
            item.prtnContractStatusId == 23 ||
            item.prtnContractStatusId == 5 ||
            item.prtnContractStatusId == 3 ||
            item.prtnContractStatusId == 10
         ) {
            return 'danger';
         } else if (
            item.prtnContractStatusId == 13 ||
            item.prtnContractStatusId == 11 ||
            item.prtnContractStatusId == 26 ||
            item.prtnContractStatusId == 9 ||
            item.prtnContractStatusId == 14 ||
            item.prtnContractStatusId == 16 ||
            item.prtnContractStatusId == 17 ||
            item.prtnContractStatusId == 18 ||
            item.prtnContractStatusId == 21 ||
            item.prtnContractStatusId == 2
         ) {
            return 'success';
         } else if (item.prtnContractStatusId == 6) {
            return 'warning';
         } else if (item.prtnContractStatusId == 7) {
            return 'info';
         } else if (item.prtnContractStatusId == 1) {
            return 'primary';
         } else {
            return 'primary';
         }
      },
      makeToast(message, variant, caption) {
         this.$toast({
            component: ToastificationContent,
            props: {
               title: message,
               text: caption,
               icon: variant == 'success' ? 'CheckSquareIcon' : 'AlertTriangleIcon',
               variant: variant
            }
         });
      },
      showValidateError(errors) {
         Object.values(errors)
            .flat()
            .slice(0, 5)
            .forEach((e) => {
               this.makeToast(e, 'danger');
            });
      },
      showApiError(err) {
         if (err?.response?.data?.errors) {
            const { errors } = err.response?.data;
            Object.keys(errors).forEach((key) => {
               this.makeToast(key + ' : ' + errors[key], 'danger');
            });
         } else {
            this.makeToast(this.$t(err), 'danger');
         }
      },
      SwalError(error) {
         if (error?.response?.data?.errors) {
            const errorMessage = Object.values(error.response?.data?.errors)[0];
            this.$swal.showValidationMessage(`Ошибка: ${errorMessage}`);
         } else {
            this.$swal.showValidationMessage(`Ошибка: ${error}`);
         }
      },
      getPdfLang() {
         let lang = localStorage.getItem('locale') || 'ru';
         if (lang == 'uz_cyrl') {
            lang = 'uz-cyrl';
         }
         if (lang == 'uz_latn') {
            lang = 'uz-latn';
         }
         return lang;
      }
   }
};
