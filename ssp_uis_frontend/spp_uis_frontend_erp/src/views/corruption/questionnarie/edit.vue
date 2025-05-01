<template>
   <b-container fluid="sm">
      <validation-observer ref="ValidationDTO">
         <b-row>
            <b-col cols="12" md="3" lg="2">
               <form-input-hrm
                  v-model="Data.orderNumber"
                  rules="required"
                  :label="$t('ordernumber')"
                  :placeholder="$t('ordernumber')"
                  type="number"
               />
            </b-col>
            <b-col cols="12" md="3" lg="2">
               <form-select
                  v-model="Data.questionnaireTypeId"
                  required-star
                  :options="QuestionnaireTypeSelectList"
                  :label="$t('questionnaireType')"
                  :placeholder="$t('questionnaireType')"
               />
            </b-col>
            <b-col cols="12" md="6" lg="8">
               <label>{{ $t('Title') }}</label>
               <QuestionareInput
                  v-model="Data.title"
                  :language-list="LanguageList"
                  :translates="Data.translates"
                  :placeholder="$t('Title')"
                  @update:translates="(e) => (Data.translates = e)"
                  column-name="title"
                  hide-plus
                  hide-x
                  rules="required"
               />
            </b-col>
         </b-row>
         <template v-if="Data.group.length > 0">
            <b-card deck no-body v-for="(group, groupIndex) in Data.group" :key="'group' + groupIndex" class="mt-1">
               <b-card-header class="d-flex full-width align-items-center">
                  <b-button variant="outline-primary" v-b-toggle="'accordion-' + groupIndex">
                     <feather-icon class="text-grey" icon="ChevronDownIcon"></feather-icon>
                  </b-button>
                  <QuestionareInput
                     v-if="group.active"
                     class="col"
                     v-model="group.title"
                     :language-list="LanguageList"
                     :translates="group.translates"
                     :placeholder="$t('groupName')"
                     @update:translates="(e) => (group.translates = e)"
                     @close="CloseGroup(groupIndex)"
                     @add="group.active = false"
                  />
                  <template v-else>
                     <h3 class="col text-center">
                        {{ group.title }}
                        <feather-icon
                           icon="EditIcon"
                           @click="Data.group[groupIndex].active = true"
                           v-b-tooltip.hover
                           :title="$t('Edit')"
                           class="text-primary cursor-pointer ml-1"
                        ></feather-icon>
                     </h3>
                     <div>
                        <b-badge variant="primary" v-b-tooltip.hover :title="$t('Questions')" pill>{{
                           group.questions.length
                        }}</b-badge>
                     </div>
                  </template>
               </b-card-header>
               <b-collapse :id="'accordion-' + groupIndex" visible accordion="my-accordion" role="tabpanel">
                  <hr class="my-0" />
                  <b-card-body>
                     <ul style="list-style-type: auto">
                        <li
                           v-for="(question, questionIndex) in group.questions"
                           :key="questionIndex + groupIndex + 'question'"
                           class="mb-2 px-1"
                        >
                           <div>
                              <QuestionareInput
                                 v-if="question.active"
                                 v-model="question.questionText"
                                 :language-list="LanguageList"
                                 :translates="question.translates"
                                 :placeholder="$t('question_name')"
                                 column-name="question_text"
                                 @update:translates="(e) => (question.translates = e)"
                                 @close="CloseQuestion(groupIndex, questionIndex)"
                                 @add="question.active = false"
                              />
                              <b-list-group-item v-else class="d-flex justify-content-between align-items-center">
                                 <div class="full-width">
                                    {{ question.questionText }}
                                 </div>
                                 <feather-icon
                                    icon="EditIcon"
                                    v-b-tooltip.hover
                                    :title="$t('Edit')"
                                    class="text-primary cursor-pointer mr-auto"
                                    @click="question.active = true"
                                 ></feather-icon>
                                 <feather-icon
                                    icon="TrashIcon"
                                    v-b-tooltip.hover
                                    :title="$t('Delete')"
                                    class="text-danger cursor-pointer ml-1 mr-auto"
                                    @click="CloseQuestion(groupIndex, questionIndex)"
                                 ></feather-icon>
                              </b-list-group-item>
                           </div>

                           <!-- question type -->
                           <div>
                              <hr />
                              <b-row>
                                 <b-col
                                    v-for="answerType in AnswerTypeSelectList"
                                    :key="answerType.value + 'answertype'"
                                 >
                                    <div
                                       class="px-2 d-flex align-items-center type-hovered"
                                       style="cursor: pointer"
                                       @click="SelectQuestionType(groupIndex, questionIndex, answerType.value)"
                                       :class="{ 'text-primary': answerType.value == question.answerTypeId }"
                                    >
                                       <feather-icon v-if="answerType.value == 1" icon="TypeIcon"></feather-icon>
                                       <feather-icon v-if="answerType.value == 2" icon="SquareIcon"></feather-icon>
                                       <feather-icon v-if="answerType.value == 3" icon="CircleIcon"></feather-icon>

                                       <b class="ml-1 text-nowrap">
                                          {{ answerType.text }}
                                       </b>
                                    </div>
                                 </b-col>
                              </b-row>
                           </div>
                           <!-- answers -->
                           <div class="pl-2 pt-2">
                              <ol style="list-style: upper-latin">
                                 <li
                                    v-for="(answer, answerIndex) in question.answers"
                                    :key="questionIndex + groupIndex + 'question' + answerIndex"
                                    class="mb-1"
                                 >
                                    <QuestionareInput
                                       v-model="answer.answerText"
                                       :language-list="LanguageList"
                                       :translates="answer.translates"
                                       :placeholder="$t('answerText')"
                                       column-name="answer_text"
                                       @update:translates="(e) => (answer.translates = e)"
                                       @close="CloseAnswer(groupIndex, questionIndex, answerIndex)"
                                       @add="answer.active = false"
                                       hide-plus
                                    />
                                 </li>
                              </ol>

                              <b-row class="mt-1">
                                 <b-col>
                                    <b-button
                                       v-b-tooltip.hover
                                       :title="$t('Answer')"
                                       pill
                                       variant="primary"
                                       v-if="!(question.answerTypeId == 3 && question.answers.length == 1)"
                                       @click="AddItemAnswer(groupIndex, questionIndex)"
                                    >
                                       <feather-icon icon="PlusIcon"></feather-icon
                                    ></b-button>
                                 </b-col>
                              </b-row>
                           </div>
                        </li>
                     </ul>

                     <!-- empty question -->
                     <b-card class="text-center" v-if="group.questions && group.questions.length == 0">
                        <i> {{ $t('questionarePlus') }} </i>
                     </b-card>

                     <b-row class="mt-1">
                        <b-col sm="12">
                           <b-button
                              v-b-tooltip.hover
                              :title="$t('Question')"
                              pill
                              variant="primary"
                              @click="AddItemQuestion(groupIndex)"
                           >
                              <feather-icon icon="PlusIcon"></feather-icon>
                           </b-button>
                        </b-col>
                     </b-row>
                  </b-card-body>
               </b-collapse>
            </b-card>
         </template>
         <!-- empty group -->
         <b-row v-if="Data.group.length == 0">
            <b-col>
               <b-card class="text-center">
                  <i>
                     Эта форма в настоящее время пуста.<br />
                     Вы можете добавить группу вопросов, примечания, подсказки или другие поля, нажав на знак «+» ниже.
                  </i>
               </b-card>
            </b-col>
         </b-row>

         <b-row>
            <b-col>
               <b-button pill v-b-tooltip.hover :title="$t('Group')" variant="primary" @click="AddItemGroup">
                  <feather-icon icon="PlusIcon"></feather-icon
               ></b-button>
            </b-col>
         </b-row>
         <b-row>
            <b-col class="text-right">
               <b-button variant="success" @click="Save"> {{ $t('Save') }} </b-button>
            </b-col>
         </b-row>
      </validation-observer>
   </b-container>
</template>
<script>
import ManualService from '@/services/others/manual.service';
import {
   BCard,
   BRow,
   BCol,
   BContainer,
   BButton,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BLink,
   BListGroup,
   BListGroupItem,
   BCollapse,
   VBToggle,
   BCardBody,
   BCardHeader,
   BBadge,
   VBTooltip
} from 'bootstrap-vue';
import QuestionareInput from '@/views/components/managment/QuestionareInput.vue';
import QuestionnarieService from '@/services/managment/questionnarie.service';
import FeatherIcon from '@/@core/components/feather-icon/FeatherIcon.vue';
const groupItemDef = {
   id: 0,
   stateId: 0,
   orderNumber: null,
   title: '',
   translates: [],
   questions: [],
   active: true
};
const questionItemDef = {
   id: 0,
   stateId: 0,
   orderNumber: null,
   questionText: '',
   hint: '',
   answerTypeId: 1,
   translates: [],
   answers: [],
   active: true
};

const answerItemDef = {
   id: 0,
   stateId: 0,
   orderNumber: null,
   answerText: '',
   translates: [],
   active: true
};

export default {
   components: {
      BCard,
      BRow,
      BCol,
      BContainer,
      BButton,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BLink,
      BListGroup,
      BListGroupItem,
      QuestionareInput,
      BCollapse,
      BCardBody,
      BCardHeader,
      FeatherIcon,
      BBadge
   },
   directives: {
      'b-toggle': VBToggle,
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         Data: {
            state: null,
            translates: [],
            group: [],
            id: 0,
            stateId: 0,
            orderNumber: null,
            title: null,
            details: null
         },
         LanguageList: [],
         AnswerTypeSelectList: [],
         QuestionnaireTypeSelectList: []
      };
   },
   created() {
      QuestionnarieService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = {
               ...res.data,
               group: res.data.group.map((e) => ({
                  ...e,
                  questions: e.questions.map((b) => ({ ...b, active: false })),
                  active: false
               }))
            };
         })
         .catch((err) => {
            this.showApiError(err);
         });

      ManualService.LanguageSelectList()
         .then((res) => {
            this.LanguageList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.AnswerTypeSelectList()
         .then((res) => {
            this.AnswerTypeSelectList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.QuestionnaireTypeSelectList()
         .then((res) => {
            this.QuestionnaireTypeSelectList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      AddItemGroup() {
         this.Data.group.push(JSON.parse(JSON.stringify(groupItemDef)));
      },
      AddItemQuestion(groupIndex) {
         this.Data.group[groupIndex].questions.push(Object.assign({}, { ...questionItemDef, answers: [] }));
      },
      CloseGroup(index) {
         this.Data.group.splice(index, 1);
      },
      CloseQuestion(groupIndex, questionIndex) {
         this.Data.group[groupIndex].questions.splice(questionIndex, 1);
      },
      AddItemAnswer(groupIndex, questionIndex) {
         this.Data.group[groupIndex].questions[questionIndex].answers.push(Object.assign({}, answerItemDef));
      },
      CloseAnswer(groupIndex, questionIndex, answerIndex) {
         this.Data.group[groupIndex].questions[questionIndex].answers.splice(answerIndex, 1);
      },
      SelectQuestionType(groupIndex, questionIndex, answerType) {
         const question = this.Data.group[groupIndex].questions[questionIndex];

         if (answerType == 3) {
            question.answers.splice(1);
         }

         this.Data.group[groupIndex].questions.splice(questionIndex, 1, {
            ...question,
            answerTypeId: answerType
         });
      },
      Save() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               QuestionnarieService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'Questionnarie' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      }
   }
};
</script>
<style lang="scss">
.type-hovered:hover {
   color: #003188;
}

.question_class::placeholder {
   color: red;
}
</style>
