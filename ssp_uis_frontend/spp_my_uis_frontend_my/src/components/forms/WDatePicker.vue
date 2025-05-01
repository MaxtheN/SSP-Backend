<template>
	<ValidationProvider :vid="vid" :name="$attrs.name" :rules="rules" v-slot="{ valid, errors, required }">
		<b-form-group v-bind="$attrs">
			<template v-slot:label>
				{{ $attrs.label }}
				<span class="text-danger" v-if="required">*</span>
			</template>
			<date-picker ref="datepicker" v-model="updateVal" :format="format" :type="type" value-type="format" v-bind="$attrs" v-on="$listeners">
				<template #input>
					<b-form-input
						v-model="updateVal"
						v-mask="type == 'datetime' ? '##.##.#### ##:##' : type != 'year' && type != 'month' ? '##.##.####' : ''"
						v-bind="$attrs"
						maxlength="10"
						:state="errors[0] ? false : valid ? true : null"
						@keyup="$emit('keyup', updateVal)"
						class="bg-white"
					/>
				</template>
			</date-picker>

			<b-form-invalid-feedback :state="errors[0] ? false : valid ? true : null">{{ errors[0] }}</b-form-invalid-feedback>
		</b-form-group>
	</ValidationProvider>
</template>

<script>
import { ValidationProvider } from "vee-validate";
import DatePicker from "vue2-datepicker";
import "vue2-datepicker/index.css";

export default {
	components: {
		DatePicker,
		ValidationProvider,
	},
	
	props: {
		vid: {
			type: String,
			default: "",
		},
		rules: {
			type: [Object, String],
			default: "",
		},
		format: {
			type: String,
			default: "DD.MM.YYYY",
		},
		mask: {
			type: String,
			default: "",
		},
		value: {
			type: null,
			default: "",
		},
		type: {
			type: String,
			default: "",
		},
	},
	data() {
		return {
			inputVal: false,
			todayDate: "",
			lang: {},
			lang_Uz: {
				formatLocale: {
					// MMMM
					months: ["Yanvar", "Fevral", "Mart", "Aprel", "May", "Iyun", "Iyul", "August", "Sentabr", "Oktabr", "Noyabr", "Dekabr"],
					// MMM
					monthsShort: ["Yan", "Fev", "Mar", "Apr", "May", "Iyun", "Iyul", "Avg", "Sen", "Okt", "Noy", "Dek"],
					// dddd
					weekdays: ["Yakshanba", "Dushanba", "Seshanba", "Chorshanba", "Payshanba", "Juma", "Shanba"],
					// ddd
					weekdaysShort: ["Yak", "Dush", "Sesh", "Chor", "Pay", "Jum", "Shan"],
					// dd
					weekdaysMin: ["Ya", "Du", "Se", "Ch", "Pa", "Ju", "Sh"],
					// first day of week
					firstDayOfWeek: 1,
				},
			},
			lang_Ru: {
				formatLocale: {
					// MMMM
					months: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
					// MMM
					monthsShort: ["Янв", "Фев", "Март", "Апрь", "Май", "Июнь", "Июль", "Авг", "Сен", "Окт", "Ноя", "Дек"],
					// dddd
					weekdays: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"],
					// ddd
					weekdaysShort: ["Вос", "Пон", "Вто", "Сре", "Чет", "Пят", "Суб"],
					// dd
					weekdaysMin: ["Во", "По", "Вт", "Ср", "Че", "Пят", "Су"],
					// first day of week
					firstDayOfWeek: 1,
				},
			},
			lang_UzCyrl: {
				formatLocale: {
					// MMMM
					months: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
					// MMM
					monthsShort: ["Янв", "Фев", "Март", "Апрь", "Май", "Июнь", "Июль", "Авг", "Сен", "Окт", "Ноя", "Дек"],
					// dddd
					weekdays: ["Якшанба", "Душанба", "Сешанба", "Чоршанба", "Пайшанба", "Жума", "Шанба"],
					// ddd
					weekdaysShort: ["Якш", "Ду", "Се", "Чор", "Пай", "Жу", "Шан"],
					// dd
					weekdaysMin: ["Якш", "Ду", "Се", "Чор", "Пай", "Жу", "Шан"],
					// first day of week
					firstDayOfWeek: 1,
				},
			},
		};
	},
	computed: {
		updateVal: {
			get() {
				return this.value;
			},
			set(val) {
				this.$emit("input", val);
			},
		},
		language() {
			return localStorage.getItem("locale") || "uz";
		},
	},
	created() {
		const today = new Date();
		const day = String(today.getDate()).padStart(2, "0");
		const month = String(today.getMonth() + 1).padStart(2, "0");
		const year = today.getFullYear();
		this.todayDate = day + "." + month + "." + year;
		if (this.language == "ru") {
			this.lang = this.lang_Ru;
		} else if (this.language == "") {
			this.lang = this.lang_UZ;
		} else {
			this.lang = this.lang_UzCyrl;
		}
	},
};
</script>

<style lang="scss" scoped>
@import "./style.scss";
::v-deep .mx-icon-calendar,
::v-deep .mx-icon-clear {
	top: 50% !important;
}

::v-deep .is-invalid + i {
	right: 30px !important;
}

.mx-datepicker {
	width: 100%;
}
</style>
