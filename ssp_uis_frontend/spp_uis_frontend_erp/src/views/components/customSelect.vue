<template>
   <div>
      <div class="custom_select">
         <b-form-input @focus="onFocus" v-model="inputValue" @blur="onBlur"></b-form-input>
         <div v-if="isOpen" class="custom_select_modal">
            <div v-if="options.length != 0">
               <ul class="custom_select_item">
                  <li v-for="(item, index) in options" :key="index" @click="SelectItem(item)">{{ item[label] }}</li>
               </ul>
            </div>
            <div v-if="options.length == 0" class="custom_select_no_data">
               <b> Malumot topilmadi </b>
            </div>
         </div>
      </div>
   </div>
</template>

<script>
import { BFormInput } from 'bootstrap-vue';
export default {
   components: { BFormInput },
   props: {
      options: {
         type: Array,
         default: () => []
      },
      label: {
         type: String,
         default: 'name'
      }
   },
   data() {
      return {
         isOpen: false,
         inputValue: ''
      };
   },
   methods: {
      onFocus() {
         this.isOpen = true;
      },
      SelectItem(item) {
         this.inputValue = item[this.label];
         this.isOpen = false;
      }
   }
};
</script>

<style>
.custom_select_modal {
   width: 100%;
   max-height: 200px;
   background-color: #fff;
   border-radius: 4px;
   z-index: 999999999;
   position: absolute;
   margin-top: 12px;
   box-shadow: rgba(50, 50, 93, 0.25) 0px 13px 27px -5px, rgba(0, 0, 0, 0.3) 0px 8px 16px -8px;
   overflow: auto;
}
.custom_select {
   display: inline-block;
   width: 100%;
   position: relative;
}
.custom_select_no_data {
   text-align: center;
   padding: 10px;
}
.custom_select_item {
   margin-bottom: 0;
   list-style-type: none;
   padding-left: 0;
}
.custom_select_item li {
   padding: 8px 10px;
   cursor: pointer;
}
.custom_select_item li:hover {
   background-color: rgba(110, 96, 237, 0.1);
   color: #6e60ed;
}
</style>
