<template>
    <div>
        <CabinetTitle :title="$t('isMain')" />
        <div class="row p-md-4 p-3">
            <div class="col-lg-6 col-12 mb-md-0 mb-4">
                <ContractorRating />
            </div>

            <ApplicationVacancies class="mb-md-0 mb-4" />

            <MemshipPayments />
        </div>
        <div class="row px-md-4 px-3">
            <div class="col-lg-6 col-12 mb-md-0 mb-4">
                <ServiceCard />
            </div>
            <div class="col-lg-6 col-12">
                <CabinetImtiyozlar />
            </div>
        </div>

        <ContractorModal />
        <CreditDemandModal v-model="createCreaditModal" />
        <OfertaModal @offerModal="closeOfferModal($event)" v-model="ofertaModalShow" />
    </div>
</template>

<script>
import CabinetTitle from '@/components/cabinet/CabinetTitle.vue';
import ContractorRating from '@/components/cabinet/main/ContractorRating.vue';
import MemshipPayments from '@/components/cabinet/main/MemshipPayments.vue';
import ServiceCard from '@/components/cabinet/main/ServiceCard.vue';
import CabinetImtiyozlar from '@/components/cabinet/main/CabinetImtiyozlar.vue';
import ApplicationVacancies from '@/components/cabinet/main/ApplicationVacancies.vue';

import AccountService from '@/services/account.service';

import ContractorModal from '@/components/ContractorModal.vue';
import CreditDemandModal from '@/components/application/CreditDemandModal.vue';
import OfertaModal from '@/components/ofertaModal.vue';
export default {
    components: { ContractorRating, MemshipPayments, ServiceCard, CabinetImtiyozlar, CabinetTitle, ApplicationVacancies, ContractorModal, CreditDemandModal, OfertaModal },
    data() {
        return {
            createCreaditModal: false,
            ofertaModalShow: false,
            InfoCard: {},
            dialog: false
        };
    },
    computed: {
        isDev() {
            return window.location.hostname != 'my.chamber.uz' || false;
        }
    },
    created() {
        AccountService.GetContractorMemshipState().then((res) => {
            this.InfoCard = res.data;
        });
        try {
            if (!JSON.parse(localStorage.getItem('user_info')).contractor.isLastOffer) {
                this.ofertaModalShow = true;
            } else {
                this.accountModal();
            }
        } catch (e) {
            console.log(e);
        }
    },
    methods: {
        closeOfferModal(data) {
            this.ofertaModalShow = data;
            this.accountModal();
        },
        accountModal() {
            AccountService.GetContractorDocumentInfo().then((res) => {
                this.InfoCard = res.data;
                if (res.data.hasPrtnCertificate && !res.data.hasPrtnCreditDemand) {
                    this.createCreaditModal = true;
                }
            });
        }
    }
};
</script>

<style lang="scss"></style>
