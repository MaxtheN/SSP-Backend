export default {
    methods: {
        makeToast(message, type) {
            var a = '';
            if (message.status == 500) {
                a = message.title;
            }
            if (message.status == 400) {
                var errors = Object.values(message.errors);
                var a = errors.map((el, item) => item + 1 + '.' + el).join('\n');
            } else {
                a = message;
            }
            this.$toast.open({
                message: a,
                type: type,
                duration: 5000,
                dismissible: true
            });
        },
        forceFileDownload(response, name, type = 'pdf') {
            var blob = new Blob([response.data]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;

            link.setAttribute('download', name + '.' + type); //or any other extension
            document.body.appendChild(link);
            link.click();
        },
        showApiError(err) {
            if (err?.response?.data?.errors) {
                const errors = err.response.data.errors;
                Object.keys(errors).forEach((key) => {
                    this.makeToast(key + ' : ' + errors[key], 'error');
                });
            } else {
                this.makeToast(err, 'error');
            }
        },
        showValidateError(errors) {
            Object.values(errors)
                .flat()
                .slice(0, 5)
                .forEach((e) => {
                    this.makeToast(e, 'error');
                });
        },
        getColor(item) {
            if (item.statusId == 24 || item.statusId == 25 || item.statusId == 23 || item.statusId == 5 || item.statusId == 3 || item.statusId == 10) {
                return 'danger';
            } else if (
                item.statusId == 13 ||
                item.statusId == 11 ||
                item.statusId == 9 ||
                item.statusId == 14 ||
                item.statusId == 16 ||
                item.statusId == 17 ||
                item.statusId == 18 ||
                item.statusId == 21 ||
                item.statusId == 2
            ) {
                return 'success';
            } else if (item.statusId == 6) {
                return 'orange';
            } else if (item.statusId == 7) {
                return 'info';
            } else {
                return 'primary';
            }
        },
        getPdfLang() {
            let lang = localStorage.getItem('locale') || 'uz_latn';
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
