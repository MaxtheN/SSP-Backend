<template>
    <div class="py-3">
        <h5 class="font-weight-bold">
            <span class="text-danger">*</span>
            {{ $t(columnName) }}
        </h5>

        <div class="d-flex mt-2">
            <b-form-file type="file" :key="fileInputKey" :placeholder="$t('selectFile')" @change="UploadFile" :browse-text="$t('select')" :disabled="fileLoading" />
            <b-spinner variant="primary" v-if="fileLoading" label="Spinning"></b-spinner>
        </div>
        <div class="mt-3" v-for="item in filteredFiles" :key="item.id">
            <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{ item.fileName || item.id }}</b-link>
            <b-button v-if="canEdit" variant="danger" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                <b-icon-trash scale="0.7" />
            </b-button>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import MonoApplicationService from '@/services/mono/monoapplication.service';

export default {
    props: {
        columnName: {
            type: String, // total_cost, dep_schema_url ,auditories_photos_url , conf_doc_url , deed_url,
            default: ''
        },
        files: {
            type: Array,
            default: () => []
        },
        canEdit: {
            type: Boolean,
            default: true
        }
    },
    emits: ['update:files'],
    data() {
        return {
            fileLoading: false,
            fileInputKey: 0
        };
    },
    computed: {
        filteredFiles() {
            return this.files.filter((e) => e.columnName == this.columnName);
        },
        FileSrc() {
            return (id) => axios.defaults.baseURL + `MonoApplication/DownloadFile/${id}`;
        }
    },
    methods: {
        FileDownload(item) {
            this.downloadLoading = true;
            MonoApplicationService.DownloadPdf(item.id2, this.lang)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('MonoApplication'));
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadLoading = false;
                });
        },
        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            MonoApplicationService.UploadFile(formData)
                .then((res) => {
                    this.$emit('update:files', [...this.files, ...res.data.map((e) => ({ ...e, columnName: this.columnName }))]);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            MonoApplicationService.DeleteFile(id).then(() => {
                this.$emit(
                    'update:files',
                    this.files.filter((item) => item.id != id)
                );
            });
            this.resetFileInput();
        },
        resetFileInput() {
            this.fileInputKey++; // Fayl inputni qayta yaratish uchun keyni o'zgartirish
        }
    }
};
</script>
