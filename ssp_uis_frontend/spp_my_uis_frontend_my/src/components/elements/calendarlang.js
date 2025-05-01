import DatePicker from 'vue2-datepicker';

// O'zbek tili uchun kalendar o'rnatmalari
const locale = {
    months: ['Yanvar', 'Fevral', 'Mart', 'Aprel', 'May', 'Iyun', 'Iyul', 'Avgust', 'Sentabr', 'Oktabr', 'Noyabr', 'Dekabr'],
    monthsShort: ['Yan.', 'Fev.', 'Mart', 'Apr.', 'May', 'Iyun', 'Iyul', 'Avg.', 'Sent.', 'Okt.', 'Noy.', 'Dek.'],
    weekdays: ['Yakshanba', 'Dushanba', 'Seshanba', 'Chorshanba', 'Payshanba', 'Juma', 'Shanba'],
    weekdaysShort: ['Yak', 'Du', 'Se', 'Chor', 'Pay', 'Jum', 'Shan'],
    weekdaysMin: ['Yak', 'Du', 'Se', 'Chor', 'Pay', 'Jum', 'Shan'],
    firstDayOfWeek: 1,
    firstWeekContainsDate: 1
};

const uz = {
    formatLocale: locale,
    yearFormat: 'YYYY',
    monthFormat: 'MMM',
    monthBeforeYear: true
};

// Vue2 DatePicker uchun o'zbekcha locale ni o'rnatish
DatePicker.locale('uz', uz);

// Eksport qilish
export default uz;
