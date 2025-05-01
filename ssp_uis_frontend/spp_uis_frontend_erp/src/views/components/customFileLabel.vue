<template>
   <div>
      <div class="my-input" :class="{ 'no-label': label === '', 'uppercase-text': uppertext }">
         <label class="input-color-label" v-if="label !== ''"> {{ label }} </label>
         <div class="my-label-content">
            {{ content }}
         </div>
         <div class="right-addon-icon d-flex">
            <div>
               <feather-icon
                  size="34"
                  icon="DownloadIcon"
                  v-if="!fileloading"
                  @click="$emit('download')"
                  target="_blank"
                  style="color: #00b894; cursor: pointer"
               />
               <b-spinner small v-if="fileloading"></b-spinner>
            </div>
         </div>
      </div>
   </div>
</template>

<script>
import { BIconArrowDownCircle, BIconXCircle, BSpinner } from 'bootstrap-vue';
export default {
   components: { BIconArrowDownCircle, BIconXCircle, BSpinner },
   props: {
      label: {
         type: String,
         default: ''
      },
      uppertext: {
         type: Boolean,
         default: false
      },
      content: {
         type: String,
         default: ''
      },
      fileloading: {
         type: Boolean,
         default: false
      }
   },
   data() {
      return {};
   },
   methods: {}
};
</script>

<style lang="scss" scoped>
.my-input {
   display: block;
   border-radius: 12px;
   width: 100%;
   background-color: #f7f7f7;
   position: relative;
   .my-label-content {
      min-height: 33px;
      padding-bottom: 3px;
      padding-left: 15px;
      color: #142964;
   }
   .right-addon-icon {
      position: absolute;
      top: 15px;
      right: 15px;
      vertical-align: middle;
      cursor: pointer;
   }
   label {
      width: 100%;
      padding-left: 16px;
      text-transform: uppercase;
      font-size: 12px;
   }
   input {
      width: 100%;
      outline: none;
      border: none;
      background-color: #f7f7f7;
      padding-left: 16px;
      border-radius: 12px;
      margin-bottom: 8px;
      &::placeholder {
         color: #a7bbbd;
      }
   }
}
.no-label {
   padding-top: 16px;
   padding-bottom: 8px;
   .input-color-label {
      display: none;
   }
}
.uppercase-text input {
   text-transform: uppercase;
}
</style>
