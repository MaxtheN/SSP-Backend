<template>
   <div>
      <b-button @click="OpenDialog" class="mt-2" variant="primary" size="md">
         {{ $t('edocInfo') }}
      </b-button>

      <b-modal size="md" v-model="dialog" hide-footer :title="$t('edocInfo')">
         <h5 v-if="statusId == 37">
            <b>{{ $t('organization') }}</b> : {{ edocInfo.organization }}
         </h5>
         <h5>
            <b>{{ $t('ijrochi F.I.O.') }}</b> : {{ edocInfo.assignment }}
         </h5>
         <h5>
            <b>{{ $t('registrationNumber') }}</b> : {{ edocInfo.regNumber }}
         </h5>
         <h5>
            <b>{{ $t('registrationDate') }}</b> : {{ edocInfo.regDate }}
         </h5>
         <h5>
            <b>{{ $t('ijro muddati') }}</b> : {{ edocInfo.termExecution }}
         </h5>

         <b-badge
            v-for="file in edocInfo.attachments"
            :key="file.id"
            class="text-left mt-1 mr-1 d-inline-block cursor-pointer"
            pill
            download
            variant="primary"
            @click="Print(file.fileName, file.id, file.fileExtension)"
         >
            {{ file.fileName || file.id }}
         </b-badge>
      </b-modal>
   </div>
</template>

<script>
import { BLink, BModal, BTable, BBadge, BButton } from 'bootstrap-vue';
import axios from 'axios';
import AppealApplicationService from '@/services/appeal/AppealApplication.service';
import { integer } from 'vee-validate/dist/rules';

export default {
   components: {
      BLink,
      BModal,
      BTable,
      BBadge,
      BLink,
      BButton
   },
   props: {
      edocInfo: {
         type: Object,
         default: () => {}
      },
      statusId: {
         type: integer,
         default: () => null
      }
   },
   data() {
      return {
         dialog: false
      };
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `AppealApplication/DownloadAttachment?fileId=${id}&isView=true`;
      }
   },
   methods: {
      OpenDialog() {
         this.dialog = true;
      },
      Print(fileName, fileId, type) {
         AppealApplicationService.DownloadAttachment(fileId).then((res) => {
            const linkSource = `data:${'application/pdf'};base64,${res.data}`;
            const link = document.createElement('a');
            link.href = linkSource;
            link.setAttribute('download', fileName + '.pdf'); //or any other extension
            document.body.appendChild(link);
            link.click();
         });
      }
   }
};
</script>
