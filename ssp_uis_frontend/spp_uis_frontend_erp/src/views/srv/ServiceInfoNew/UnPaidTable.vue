<template>
   <b-table-simple hover small caption-top responsive border>
      <b-thead>
         <b-tr>
            <b-th>{{ $t('order') }}</b-th>
            <b-th>{{ $t('services') }}</b-th>
            <b-th>{{ $t('Total') }}</b-th>

            <template v-for="(region, index) in items">
               <b-th
                  :key="index"
                  @click="!filter.byDistrict ? SortRegion({ regionId: region.regionId, region: region.name }) : ''"
                  style="cursor: pointer"
               >
                  {{ region.name }}
               </b-th>
            </template>
         </b-tr>

         <b-tr>
            <b-th colspan="2">{{ $t('Total') }}</b-th>
            <b-th style="text-wrap: nowrap">{{ currency(totalCounts.totalCount) }}</b-th>

            <template v-for="(region, index) in items">
               <b-th style="text-wrap: nowrap" :key="index">{{ currency(region.count) }}</b-th>
            </template>
         </b-tr>
      </b-thead>
      <b-tbody>
         <!-- Loop through grouped services by group name -->
         <template v-for="(group, groupIndex) in collectGroupedServices()">
            <b-tr variant="active" :key="groupIndex">
               <b-td colspan="100%">{{ group.groupName }}</b-td>
            </b-tr>
            <!-- Loop through the services under this group -->
            <template v-for="(service, serviceIndex) in group.services">
               <b-tr :key="serviceIndex">
                  <b-td>{{ serviceIndex + 1 }}</b-td>
                  <b-td class="text-truncate" style="max-width: 400px" v-b-tooltip.hover :title="service.serviceName">{{
                     service.serviceName
                  }}</b-td>
                  <b-td class="text-right">{{ service.totalCount }}</b-td>

                  <template v-for="(region, regionIndex) in items">
                     <!-- {{ region }} -->
                     <b-td :key="regionIndex" class="text-right">{{
                        getRegionData(service, filter.byDistrict ? region.districtId : region.regionId).count
                     }}</b-td>
                  </template>
               </b-tr>
            </template>
         </template>
      </b-tbody>
   </b-table-simple>
</template>

<script>
import { BTableSimple, BThead, BTbody, BTr, BTd, BTh } from 'bootstrap-vue';
export default {
   name: 'PaidTable',
   props: {
      items: {
         type: Array,
         required: true,
         default: () => []
      },
      filter: {
         type: Object,
         required: true,
         default: () => ({})
      },
      totals: {
         type: Object,
         required: true,
         default: () => ({})
      }
   },
   components: {
      BTableSimple,
      BThead,
      BTbody,
      BTr,
      BTd,
      BTh
   },
   computed: {
      // Calculate total count and total sum across all regions
      totalCounts() {
         return this.items.reduce(
            (totals, item) => {
               return {
                  totalCount: totals.totalCount + (item.count || 0)
               };
            },
            { totalCount: 0 }
         );
      }
   },
   methods: {
      // Group services by group name, then aggregate by region or district
      collectGroupedServices() {
         const groups = {};

         this.items.forEach((region) => {
            region?.deedGroup?.forEach((group) => {
               // Initialize group if not present
               if (!groups[group.groupName]) {
                  groups[group.groupName] = {
                     groupName: group.groupName,
                     services: {}
                  };
               }

               group.serviceNames.forEach((service) => {
                  // Initialize service under the group if it doesn't exist
                  if (!groups[group.groupName].services[service.serviceNames]) {
                     groups[group.groupName].services[service.serviceNames] = {
                        serviceName: service.serviceNames,
                        totalCount: 0,
                        regions: this.filter.byDistrict ? {} : {},
                        districts: this.filter.byDistrict ? {} : {}
                     };
                  }

                  const serviceKey = groups[group.groupName].services[service.serviceNames];

                  // Accumulate counts and sums for the service
                  serviceKey.totalCount += service.count || 0;

                  // Determine whether to use districtId or regionId based on the filter
                  const regionOrDistrictId = this.filter.byDistrict ? region.districtId : region.regionId;

                  // Initialize the region or district if it doesn't exist
                  if (!serviceKey[this.filter.byDistrict ? 'districts' : 'regions'][regionOrDistrictId]) {
                     serviceKey[this.filter.byDistrict ? 'districts' : 'regions'][regionOrDistrictId] = {
                        count: 0,
                        sum: 0
                     };
                  }

                  // Accumulate counts for the specific region or district
                  serviceKey[this.filter.byDistrict ? 'districts' : 'regions'][regionOrDistrictId].count +=
                     service.count || 0;
                  serviceKey[this.filter.byDistrict ? 'districts' : 'regions'][regionOrDistrictId].sum +=
                     service.sum || 0; // Assuming you want to sum as well
               });
            });
         });

         // Convert groups object to an array of groups
         return Object.values(groups).map((group) => ({
            groupName: group.groupName,
            services: Object.values(group.services)
         }));
      },

      // Get regional or district data for a specific service
      getRegionData(service, regionOrDistrictId) {
         const regionData = service.regions[regionOrDistrictId] || service.districts[regionOrDistrictId];
         return regionData ? regionData : { count: 0, sum: 0 };
      },

      SortRegion(item) {
         this.$emit('sortRegion', item);
      }
      // SortDistrict(item) {
      //    this.$emit('sortDistrict', item);
      // }
   }
};
</script>
<style lang="scss" scoped>
@import './styles.scss';
</style>
