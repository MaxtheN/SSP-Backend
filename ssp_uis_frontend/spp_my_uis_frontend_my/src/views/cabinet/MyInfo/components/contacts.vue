<template>
    <div>
        <b-card class="bg-white">
            <div class="border-bottom pb-2 d-flex justify-content-between align-items-center">
                <h6>{{ $t('contacts') }}</h6>
                <b-button variant="light" size="sm" v-b-tooltip.hover :title="$t('Aloqa turini qo\'shish')" class="border-1" @click="addPhone">
                    <b-icon-plus scale="1.3" variant="info"></b-icon-plus>
                </b-button>
            </div>
            <div class="mt-4">
                <b-table :items="PhoneList" :fields="fieldsContacts" responsive bordered>
                    <template #cell(id)="{ item, index }">
                        {{ index + 1 }}
                    </template>
                    <template #cell(actions)="{ item, index }">
                        <div>
                            <b-link @click="DeletePhone(item, index)" class="mr-3">
                                <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                            </b-link>

                            <b-link @click="EditPhone(item)" style="margin-right: 5px">
                                <b-icon-pencil-square style="width: 20px; height: 20px"></b-icon-pencil-square>
                            </b-link>
                        </div>
                    </template>
                </b-table>
            </div>
        </b-card>
        <b-modal v-model="phoneModal" :title="$t('addacontacts')">
            <b-row>
                <b-col cols="12" class="mainMobileInput">
                    <custom-select
                        required
                        :label="$t('contactType')"
                        :clearable="true"
                        :valueid="'value'"
                        :valuename="'text'"
                        :options="ContactTypeList"
                        v-model="TabrowPhone.contactTypeId"
                    />
                </b-col>
                <b-col cols="12" class="mainMobileInput mt-4">
                    <custom-input required :label="$t('contact')" v-model="TabrowPhone.contact"></custom-input>
                </b-col>
            </b-row>
            <template #modal-footer>
                <div class="d-flex justify-content-between w-100">
                    <b-button variant="light" @click="closePhoneModal" class="border-secondary px-5">
                        <span style="color: #000">{{ $t('back') }}</span>
                    </b-button>
                    <b-button variant="info" class="px-5" @click="addTabrowPhone">
                        <span>{{ $t('add') }}</span>
                    </b-button>
                </div>
            </template>
        </b-modal>
    </div>
</template>

<script>
import ContractorContactService from '@/services/myinfo/contractorcontact.service';
import ManualService from '@/services/manual.service';
import CustomSelect from '../../../../components/elements/customSelect.vue';
import CustomInput from '../../../../components/elements/customInput.vue';

export default {
    components: {
        CustomSelect,
        CustomInput
    },
    data() {
        return {
            PhoneList: [],
            phoneModal: false,
            ContactTypeList: [],
            fieldsContacts: [
                {
                    key: 'id',
                    label: this.$t('id'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'contactType',
                    label: this.$t('contactType'),
                    sortable: false,
                    tdClass: 'text-left',
                    thClass: 'text-center'
                },
                {
                    key: 'contact',
                    label: this.$t('contact'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'actions',
                    tdClass: 'text-center ',
                    thClass: 'text-center',
                    thStyle: 'width:200px',
                    label: this.$t('actions')
                }
            ],
            TabrowPhone: {
                contactTypeId: null,
                contact: ''
            },
            ownerId: JSON.parse(localStorage.getItem('user_info')).contractorId
        };
    },
    created() {
        this.Refresh();
        ManualService.ContactTypeSelectList().then((res) => {
            this.ContactTypeList = res.data;
        });
    },
    methods: {
        DeletePhone(item) {
            ContractorContactService.Delete(item.id)
                .then((res) => this.makeToast(this.$t('DeleteSuccess'), 'success'))
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Refresh();
                });
        },
        addPhone() {
            this.phoneModal = true;
            ContractorContactService.Get(0).then((res) => {
                this.TabrowPhone = res.data;
            });
        },
        closePhoneModal() {
            this.phoneModal = false;
        },
        checkTabrowContact() {
            if (
                this.TabrowPhone.contactTypeId === null ||
                this.TabrowPhone.contactTypeId === undefined ||
                this.TabrowPhone.contactTypeId === 0 ||
                this.TabrowPhone.contactTypeId === ''
            ) {
                this.makeToast(this.$t('contactTypeNotSelected'), 'error');
                return false;
            }
            if (this.TabrowPhone.contact === null || this.TabrowPhone.contact === undefined || this.TabrowPhone.contact === 0 || this.TabrowPhone.contact === '') {
                this.makeToast(this.$t('contactNotSelected'), 'error');
                return false;
            }
            return true;
        },
        addTabrowPhone() {
            if (!this.checkTabrowContact()) {
                return false;
            }
            ContractorContactService.Update(this.TabrowPhone)
                .then((res) => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                })
                .catch((error) => {
                    this.saveLoading = false;
                    this.showApiError(error);
                })
                .finally(() => {
                    this.saveLoading = false;
                    this.phoneModal = false;
                    this.Refresh();
                });
        },
        Refresh() {
            ContractorContactService.GetList().then((res) => {
                this.PhoneList = res.data;
            });
        },
        EditPhone(item) {
            this.phoneModal = true;
            ContractorContactService.Get(item.id).then((res) => {
                this.TabrowPhone = res.data;
            });
        }
    }
};
</script>

<style lang="scss" scoped></style>
