import PositionClassificationService from '@/services/hrm/positionclassification.service';
import StaffingService from '@/services/hrm/staffing.service';
import TariffScaleService from '@/services/hrm/tariffscale.service';
import TariffScaleCoefService from '@/services/hrm/tariffscalecoef.service';
import DepartmentService from '@/services/info/department.service';
import PositionService from '@/services/info/position.service';
import OrganizationService from '@/services/managment/organization.service';
import ManualService from '@/services/others/manual.service';
function setSelected(value, array) {
   const obj = array.find((item) => item.value === value);

   if (!obj) return null;

   return obj;
}
const defPositions = {
   id: 0,
   ownerId: null,
   departmentId: null,
   positionId: null,
   positionCategoryId: null,
   positionClassificationId: 8,
   positionPeriodId: 1,
   positionTypeId: null,
   qualificationCategoryId: null,
   tariffScaleTypeId: 1,
   tariffScaleId: 1,
   rankId: null,
   quantity: 1,
   totalSum: 0,
   rankCode: '',
   rankCoef: 0,
   corrCoef: '1.0',
   fot: 0,
   forMonth: 12,
   salary: 0,
   ownerDocDate: '',
   calcKinds: []
};

const defStata = {
   positions: { ...defPositions },
   orgSettlementAccountList: [],
   staffTypeBasicTariffList: [],
   positionCategoryListFiltered: [],
   OrgSettlementSourceSelectList: [],
   departmentList: [],
   positionList: [],
   PositionClassificationList: [],
   positionCategoryList: [],
   positionPeriodList: [],
   positionTypeList: [],
   disabledPositionCategory: false,
   btnDisabled1: false,
   isBaseSalary: false,
   innerPositions: {},
   editedIndex2: -1,
   rankList: [],
   Data: {
      positions: [],
      orgSettlementAccountId: null,
      docSum: null,
      docNumber: null
   },
   isDisabledTable: true,
   isDisabledTopHeader: true,
   getLoading: false
};
const getters = {
   getData: (state) => state.Data,
   getOrgSettleMentAccountList: (state) => state.orgSettlementAccountList,
   getStaffTypeBasicTariffList: (state) => state.staffTypeBasicTariffList,
   getPositions: (state) => state.positions,
   getDepartMentList: (state) => state.departmentList,
   getPositionList: (state) => state.positionList,
   getRankList: (state) => state.rankList,
   getIsBaseSalary: (state) => state.isBaseSalary,
   getIsDisabledTable: (state) => state.isDisabledTable,
   getIsDisabledTopHeader: (state) => state.isDisabledTopHeader,
   getbtnDisabled1: (state) => state.btnDisabled1,
   getEditedIndex2: (state) => state.editedIndex2,
   getLoading: (state) => state.getLoading
};
const mutations = {
   setData(state, value) {
      state.Data = value;
   },
   setPositions(state, value) {
      state.positions = value;
   },
   setEditedIndex2(state, val) {
      state.editedIndex2 = val;
   },
   setOrgSettleMentAccountLIst(state, value) {
      state.orgSettlementAccountList = value;
   },
   setStaffTypeBasicTariffList(state, value) {
      state.staffTypeBasicTariffList = value;
   },
   setDepartMentList(state, value) {
      state.departmentList = value;
   },
   setPositionsList(state, value) {
      state.positionList = value;
   },
   setCalcKinds(state, value) {
      const changedCalcKinds = [];
      if (value) {
         value.forEach((element) => {
            const obj = {
               id: 0,
               calculationKindId: element.id,
               calculationKindName: element.fullName
            };
            changedCalcKinds.push(obj);
         });
         state.positions.calcKinds = changedCalcKinds;
      }
   },
   setTariffScaleList(state, value) {
      state.tariffScaleList = value;
   },
   setRankList(state, value) {
      state.rankList = value;
   },
   ChangePositionQuantity(state, value) {
      state.positions.quantity = value;
   },
   ChangePositionForMonth(state, value) {
      state.positions.forMonth = value;
      if (Number(state.positions.forMonth) >= 12) {
         state.positions.forMonth = 12;
      }
   },
   clearPositions(state) {
      state.positions = {
         ...defPositions,
         positionPeriodId: state.positions.positionPeriodId,
         rankCode: state.positions.rankCode,
         rankCoef: state.positions.rankCoef,
         corrCoef: state.positions.corrCoef,
         calcKinds: state.positions.calcKinds
      };
   },
   setLoading(state, value) {
      state.getLoading = value;
   }
};
const actions = {
   ChangePositionQuantity({ commit, dispatch }, val) {
      commit('ChangePositionQuantity', val);
      dispatch('RecalcStaffingCalcKindTables');
   },
   getCloneData({ commit, dispatch }, id) {
      commit('setLoading', true);
      StaffingService.GetClone(id)
         .then(async (res) => {
            const promises = [];
            if (res.data && Array.isArray(res.data.positions)) {
               res.data.positions.forEach((pos) => {
                  promises.push(StaffingService.RecalcStaffingCalcKindTables(pos))
               })
            }
            await Promise.all(promises).then((values) => {
               values.forEach((pos, i) => {
                  res.data.positions[i] = pos.data
               })

            });
            commit('setData', res.data);
         })
         .finally(() => {
            commit('setLoading', false);
         });
   },
   getDataList({ commit }, id) {
      commit('setLoading', true);
      StaffingService.Get(id)
         .then((res) => {
            commit('setData', res.data);
         })
         .finally(() => {
            commit('setLoading', false);
         });
   },
   getOrgSettleMentAccountList({ commit }) {
      OrganizationService.AsSelectListOrgSettlementAccount().then((res) => {
         commit('setOrgSettleMentAccountLIst', res.data);
      });
   },
   getStaffTypeBasicTariffList({ commit }) {
      ManualService.StaffingTypeSelectList().then((res) => {
         commit('setStaffTypeBasicTariffList', res.data);
      });
   },
   getDepartMentList({ commit }) {
      DepartmentService.GetAsSelectList(null, {}).then((res) => {
         commit('setDepartMentList', res.data);
      });
   },
   getPositionList({ commit }) {
      PositionService.GetAsSelectList().then((res) => {
         commit('setPositionsList', res.data);
      });
   },
   getTableAsSelectList({ commit }, id) {
      TariffScaleService.GetTableAsSelectList(id).then((res) => {
         commit('setRankList', res.data);
      });
   },
   RecalcStaffingCalcKindTables({ state }) {
      state.btnDisabled1 = true;
      state.positions.ownerDocDate = state.Data.docOn;

      StaffingService.RecalcStaffingCalcKindTables(state.positions)
         .then((res) => {
            state.positions.salary = res.data.salary;
            state.positions.fot = res.data.fot;
            res.data.calcKinds.map((item) => {
               item.calcCoef == 0 ? (item.editable = true) : (item.editable = false);
               return item;
            });
            state.positions.calcKinds = res.data.calcKinds;
            state.positions.totalSum = res.data.totalSum;
         })
         .catch((error) => {
            // this.$message(error.response.data.error);
         })
         .finally(() => {
            state.btnDisabled1 = false;
         });
   },
   ChangeRank({ state, dispatch }, id) {
      state.positions.rankCode = '';
      if (setSelected(id, state.rankList)) {
         TariffScaleCoefService.GetTableAsSelectList('', id)
            .then((res) => {
               state.positions.rankCoef = res.data[0]?.text;
               state.positions.rankCode = res.data[0]?.rankCode;
            })
            .finally(() => {
               dispatch('RecalcStaffingCalcKindTables');
            });
      }
   },
   AddPositions({ state, commit }) {
      state.positions.ownerDocDate = state.Data.docOn;

      state.positions.departmentName = state.departmentList.find(
         (el) => el.value === state.positions.departmentId
      )?.text;
      state.positions.positionName = state.positionList.find((el) => el.value === state.positions.positionId)?.text;
      state.positions.rankName = state.rankList.find((el) => el.value === state.positions.rankId)?.text;

      if (state.editedIndex2 > -1) {
         Object.assign(state.Data.positions[state.editedIndex2], state.positions);
      } else {
         state.Data.positions.push(state.positions);
      }

      const docSum = state.Data.positions.reduce((sum, pos) => sum + pos.totalSum, 0);

      commit('setData', { ...state.Data, docSum });

      commit('clearPositions');
   },
   DeletePositions({ state }, index) {
      state.Data.positions.splice(index, 1);
   },
   EditPositions({ state, commit }, { item, idx }) {
      state.editedIndex2 = idx;
      commit('setPositions', { ...item });
   },
   ChangePositions({ dispatch, state }, item) {
      state.positions.rankId = item.rankId;
      state.positions.positionClassificationId = 8;
      state.positions.rankCode = item.rankId;
      const id = this.PositionList?.filter((item2) => item2.value === item.positionId)[0]?.positionClassificationId;

      PositionClassificationService.GetList({
         page: 1,
         pageSize: 1000,
         id: item.positionClassificationId ? item.positionClassificationId : id
      }).then((res) => {
         if (res.data.rows.length > 0) {
            state.positions.positionClassificationId = res.data.rows[0].id;
            state.positions.positionClassification = res.data.rows[0].fullName;
         }
      });

      if (state.positions.rankId) {
         TariffScaleCoefService.GetTableAsSelectList('', state.positions.rankId).then((res) => {
            state.positions.rankCoef = res.data[0]?.value;
            state.positions.rankCode = res.data[0]?.rankCode;
         });
      }
      setTimeout(() => {
         dispatch('RecalcStaffingCalcKindTables');
      }, 1000);
   },
   saveData({ state }, { cb = () => { }, err = () => { } }) {
      state.Data.docOn = state.Data.startOn;
      state.Data.docNumber = '1';
      let orderNumber = 1;
      state.Data.positions.forEach((item) => {
         item.orderNumber = orderNumber;
         orderNumber++;
      });
      state.btnDisabled = true;
      StaffingService.Update(state.Data)
         .then(() => {
            cb();
            state.btnDisabled = false;
         })
         .catch((error) => {
            err(error);
            state.btnDisabled = false;
         });
   },

   resetPage({ commit }) {
      commit('clearPositions');
      commit('setOrgSettleMentAccountLIst', []);
      commit('setOrgSettleMentAccountLIst', []);
      commit('setDepartMentList', []);
      commit('setPositionsList', []);
      commit('setCalcKinds', []);
      commit('setRankList', []);
   }
};

// eslint-disable-next-line import/prefer-default-export
export const staffingStore = {
   namespaced: true,
   state: defStata,
   getters,
   actions,
   mutations
};
