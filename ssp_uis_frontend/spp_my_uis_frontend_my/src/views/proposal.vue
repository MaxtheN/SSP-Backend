<template>
	<div class="home-page">
		<div class="container">
			<AppListHeader title="Murojaat yollash" />
			<b-card class="form-card p-lg-4">
				<validation-observer ref="ValidationDTO">
					<b-row>
						<b-col sm="12" md="6" lg="4">
							<WSelect
								:placeholder="$t('select')"
								:options="ApplicantTypeSelectList"
								:label="$t('Murojaatchi turi')"
								rules="required"
								v-model="filter.proposalTypeId"
							/>
						</b-col>
						<b-col sm="12" md="6" lg="4">
							<WSelect
								:placeholder="$t('select')"
								:options="CompanyTypeSelectList"
								:label="$t('Tashkilot turi')"
								v-model="filter.companyTypeId"
								rules="required"
							/>
						</b-col>
						<b-col sm="12" md="6" lg="4">
							<WSelect
								:placeholder="$t('select')"
								:options="BusinessSectorSelectList"
								:label="$t('Tadbirkorlik sohasi')"
								v-model="filter.businessSectorId"
								rules="required"
							/>
						</b-col>
					</b-row>
					<hr class="my-4" />
					<b-row v-if="filter.proposalTypeId == 1" class="d-flex align-items-center">
						<b-col sm="12" lg="5" md="6">
							<WInput v-model="filter.companyInn" mask="#########" rules="required" :label="$t('Tashkilot INN')" />
						</b-col>
						<b-col lg="1">
							<b-button @click="GetFromSoliq" variant="primary" size="lg">
								<b-icon icon="arrow-clockwise" v-if="!SoliqLoading"></b-icon>
								<b-spinner v-if="SoliqLoading" small></b-spinner>
							</b-button>
						</b-col>
						<b-col sm="12" lg="6" md="6">
							<WInput v-if="!GetContractorFromSoliq.fullName" rules="required" v-model="filter.companyName" :label="$t('Tashkilot nomi')" />
							<WInput disabled v-if="GetContractorFromSoliq.fullName" v-model="filter.companyName" :label="$t('Tashkilot nomi')" />
						</b-col>
					</b-row>
					<hr v-if="filter.proposalTypeId == 1" style="margin: 45px 0" />
					<b-row>
						<b-col sm="12" lg="6" md="6">
							<WInput v-model="filter.surnameLatin" rules="required" :label="$t('familyname')" />
						</b-col>
						<b-col sm="12" lg="6" md="6">
							<WInput v-model="filter.nameLatin" rules="required" :label="$t('firstname')" />
						</b-col>
					</b-row>
					<b-row class="mt-3">
						<b-col sm="12" lg="4" md="6">
							<WSelect :placeholder="$t('select')" :options="GenderList" v-model="filter.genderId" rules="required" :label="$t('gender')" />
						</b-col>
						<b-col sm="12" lg="4" md="6">
							<WPhoneInput v-model="filter.phoneNumber" rules="required" placeholder="+998 (##) ### ## ##" />
						</b-col>
					</b-row>
					<hr class="my-4" />
					<b-row>
						<b-col sm="12" md="6" lg="6">
							<WSelect
								:placeholder="$t('select')"
								:options="RegionList"
								:label="$t('liveoblastname')"
								v-model="filter.regionId"
								rules="required"
								@input="ChangeRegion"
							/>
						</b-col>
						<b-col sm="12" md="6" lg="6">
							<WSelect
								:placeholder="$t('select')"
								:options="DistrictList"
								:label="$t('liveregionname')"
								rules="required"
								v-model="filter.districtId"
								@input="ChangeDistrict"
							/>
						</b-col>
					</b-row>
					<b-row class="mt-3">
						<b-col sm="12" md="6" lg="6">
							<WSelect :placeholder="$t('select')" :options="MfySelectList" :label="$t('livemfyname')" v-model="filter.mfyId" />
						</b-col>
						<b-col sm="12" lg="6" md="6">
							<WInput v-model="filter.address" :label="$t('Manzil')" />
						</b-col>
					</b-row>

					<hr class="my-4" />
					<b-row>
						<b-col sm="12" md="6" lg="6">
							<WSelect
								:placeholder="$t('select')"
								:options="ProposalSubjectSelectList"
								:label="$t('Taklif mavzusi')"
								v-model="filter.proposalSubjectId"
								rules="required"
							/>
						</b-col>
					</b-row>
					<hr class="my-4" />
					<b-row>
						<b-col lg="6" sm="12" md="6">
							<WTextarea rows="4" v-model="filter.appealText" rules="required" :label="$t('Taklif matni')" />
						</b-col>
						<b-col sm="12" lg="6" md="6">
							<WTextarea rows="4" v-model="filter.proposalText" rules="required" :label="$t('appealText')" />
						</b-col>
						<hr style="margin: 45px 0" />
						<b-col lg="4" md="6" sm="12">
							<h6 class="inputTitle">{{ $t("Taklifga ilovalar (Fayl yuklash)") }}<span> *</span></h6>
							<b-form-file type="file" :placeholder="$t('selectFile')" class="mt-2" @change="UploadFile" :browse-text="$t('select')"></b-form-file>
						</b-col>
					</b-row>
					<b-row class="w-100 mt-3" style="text-align: end">
						<b-col lg="12" class="float-right">
							<b-button @click="CheckProposal" variant="primary">{{ $t("Murojaat yollash") }}</b-button>
						</b-col>
					</b-row>
				</validation-observer>
			</b-card>
		</div>
		<b-modal v-model="ApproveModal" no-close-on-backdrop hide-footer :title="$t('Approve')">
			<b-row class="my-3">
				<b-col class="text-center">
					{{ $t("Haqiqatdan ham ushbu taklifni yubormoqchimisiz!!!") }}
				</b-col>
			</b-row>
			<b-row class="mt-4">
				<b-col lg="12" class="text-end d-flex justify-content-end">
					<b-button @click="ApproveModal = false" class="mr-2" variant="danger">{{ $t("back") }}</b-button>
					<b-button @click="Save" variant="success">
						<b-spinner v-if="ApproveLoading" small></b-spinner>
						{{ $t("confirm") }}
					</b-button>
				</b-col>
			</b-row>
		</b-modal>
	</div>
</template>

<script>
import vClickOutside from "v-click-outside";
import vSelect from "vue-select";
import axios from "axios";
import ManualService from "@/services/manual.service";
import ProposalService from "@/services/proposal.service";
import AppListHeader from "../components/application/AppListHeader.vue";
import WSelect from "../components/forms/WSelect.vue";
import WInput from "../components/forms/WInput.vue";
import WPhoneInput from "../components/forms/WPhoneInput.vue";
import WTextarea from "../components/forms/WTextarea.vue";
export default {
	components: {
		vSelect,
		AppListHeader,
		WSelect,
		WInput,
		WPhoneInput,
		WTextarea,
	},
	directives: {
		clickOutside: vClickOutside.directive,
	},
	data() {
		return {
			axios,
			sidebar: false,
			offerType: {},
			businessSector: {},
			ApplicantTypeSelectList: [],
			BusinessSectorSelectList: [],
			Proposal: {},
			GenderList: [],
			RegionList: [],
			DistrictList: [],
			MfySelectList: [],
			EmploymentTypeSelectList: [],
			ProposalSubjectSelectList: [],
			ProposalDisclosureSelectList: [],
			CompanyTypeSelectList: [],
			GetContractorFromSoliq: {},
			items: [],
			dialogImageUrl: "",
			imageUrl: "",
			dialogVisible: false,
			ApproveModal: false,
			ApproveLoading: false,
			SoliqLoading: false,
			personphoto: "",
			langId: localStorage.getItem("langId") || 3,
			filter: {
				regionId: "",
				districtId: "",
				nameLatin: "",
				surnameLatin: "",
				patronymLatin: "",
				genderId: "",
				birthDate: "",
				phoneNumber: "",
				email: "",
				mfyId: "",
				proposalText: "",
				proposalTypeId: "",
				businessSectorId: "",
				externalSourceTypeId: 2,
				proposalEmployementTypeId: "",
				proposalSubjectId: "",
				toOrganizationId: null,
				proposalDisclosureId: "",
				companyName: "",
				companyInn: "",
				companyTypeId: "",
				appealText: "",
				// answer: "",
				files: [],
				inn: "",
			},
		};
	},
	created() {
		ManualService.RegionSelectList(this.langId)
			.then((res) => {
				this.RegionList = res.data;
			})
			.catch((error) => {
				this.showApiError(error);
			});

		ManualService.GenderSelectList(this.langId)
			.then((res) => {
				this.GenderList = res.data;
			})
			.catch((error) => {
				this.showApiError(error);
			});

		ManualService.BusinessSectorSelectList(this.langId)
			.then((res) => {
				this.BusinessSectorSelectList = res.data;
			})
			.catch((error) => {
				this.showApiError(error);
			});

		ManualService.ApplicantTypeSelectList(this.langId)
			.then((res) => {
				this.ApplicantTypeSelectList = res.data;
			})
			.catch((error) => {
				this.showApiError(error);
			});

		ManualService.EmploymentTypeSelectList(this.langId)
			.then((res) => {
				this.EmploymentTypeSelectList = res.data;
			})
			.catch((error) => {
				this.showApiError(error);
			});

		ManualService.ProposalSubjectSelectList(this.langId)
			.then((res) => {
				this.ProposalSubjectSelectList = res.data;
			})
			.catch((error) => {
				this.showApiError(error);
			});

		ManualService.ProposalDisclosureSelectList(this.langId)
			.then((res) => {
				this.ProposalDisclosureSelectList = res.data;
			})
			.catch((error) => {
				this.showApiError(error);
			});

		ManualService.CompanyTypeSelectList(this.langId).then((res) => {
			this.CompanyTypeSelectList = res.data;
		});
	},
	methods: {
		ChangeRegion() {
			this.filter.districtId = "";
			this.filter.mfyId = "";
			this.getDistrictList();
		},
		ChangeDistrict() {
			this.filter.mfyId = "";
			this.getMfyList();
		},
		getDistrictList() {
			if (!!this.filter.regionId) {
				ManualService.DistrictSelectList(this.filter.regionId, this.langId)
					.then((res) => {
						this.DistrictList = res.data;
					})
					.catch((error) => {
						this.showApiError(error);
					});
			}
		},
		getMfyList() {
			if (!!this.filter.districtId) {
				ManualService.MfySelectList(this.filter.districtId, this.langId)
					.then((res) => {
						this.MfySelectList = res.data;
					})
					.catch((error) => {
						this.showApiError(error);
					});
			}
		},
		GetFromSoliq() {
			this.SoliqLoading = true;
			ProposalService.GetContractorFromSoliq(this.filter.companyInn)
				.then((res) => {
					if (this.filter.companyInn) {
						this.GetContractorFromSoliq = res.data;

						this.filter.companyName = this.GetContractorFromSoliq.fullName;
					}
					this.SoliqLoading = false;
				})
				.catch((error) => {
					this.$message({
						showClose: true,
						message: "Ma'lumot topilmadi",
						type: "error",
					});
					this.SoliqLoading = false;
				});
		},
		CheckProposal() {
			if (this.filter.files === 0 || this.filter.files === null || this.filter.files === undefined || this.filter.files === "") {
				this.makeToast("Fayl tanlanmagan", "error");
				return false;
			}

			this.$refs.ValidationDTO.validate().then((success) => {
				if (success) {
					this.ApproveModal = true;
				}
			});
		},

		Save() {
			this.ApproveLoading = true;
			ProposalService.Create(this.filter)
				.then(() => {
					this.ApproveLoading = true;
					this.makeToast("Taklifingiz muvaffaqqiyatli yuborildi", "success");
					setTimeout(() => {
						this.$router.push({ name: "Home" });
					}, 500);
					this.ApproveLoading = false;
					this.ApproveModal = false;
				})
				.catch((error) => {
					this.showApiError(error);
					this.ApproveModal = false;
					this.ApproveLoading = false;
				});
		},
		UploadFile(event) {
			const formData = new FormData();
			formData.append("files", event.target.files[0]);
			this.fileLoading = true;
			ProposalService.UploadFiles(formData).then((res) => {
				this.filter.files.push(res.data[0]);
				this.fileLoading = false;
			});
		},
	},
};
</script>