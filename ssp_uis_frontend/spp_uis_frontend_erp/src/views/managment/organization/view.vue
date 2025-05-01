<template>
  <div>
    <b-overlay :show="previewshow">
      <b-row>
        <b-col>
          <b-card>
              <table>
                  <tbody>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('fullname') }} :
                          </td>
                          <td class="font-weight-bold pb-1"> {{ OrganizationView.FullName }} </td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('shortname') }} :
                          </td>
                          <td class="font-weight-bold pb-1"> {{ OrganizationView.ShortName }} </td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('inn') }} :
                          </td>
                          <td class="font-weight-bold pb-1"> {{ OrganizationView.INN }} </td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('Director') }} :
                          </td>
                          <td class="font-weight-bold pb-1"> {{ OrganizationView.Director }} </td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('Accounter') }} :
                          </td>
                          <td class="pb-1"><span class="font-weight-bold"> {{ OrganizationView.Accounter }} </span></td>
                      </tr>
                      
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('Cashier') }} :
                          </td>
                          <td class="pb-1"><span class="font-weight-bold"> {{ OrganizationView.Cashier }} </span></td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('Oblast') }} :
                          </td>
                          <td class="font-weight-bold pb-1"> {{ OrganizationView.OblastName }} </td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('Region') }} :
                          </td>
                          <td class="pb-1"><span class="font-weight-bold"> {{ OrganizationView.RegionName }} </span></td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('Adress') }} :
                          </td>
                          <td class="font-weight-bold pb-1"> {{ OrganizationView.Adress }} </td>
                      </tr>
                      
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('ContactInfo') }} :
                          </td>
                          <td class="font-weight-bold pb-1"> {{ OrganizationView.ContactInfo }} </td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('ZipCode') }} :
                          </td>
                          <td class="pb-1"><span class="font-weight-bold"> {{ OrganizationView.ZipCode }} </span></td>
                      </tr>
                      <tr>
                          <td class="pr-1 pb-1">
                              {{ $t('website') }} :
                          </td>
                          <td class="pb-1"> <span class="font-weight-bold"> {{ OrganizationView.website }} </span> </td>
                      </tr>
                  </tbody>
              </table>
          </b-card>
        </b-col>
      </b-row>
    </b-overlay>
  </div>
</template>

<script>
import OrganizationService from "@/services/managment/organization.service"
import {BOverlay, BRow, BCol, BCard} from 'bootstrap-vue'
export default {
  components : { BOverlay, BRow, BCol, BCard },
  data(){
    return{
      previewshow : false,
      OrganizationView : {}
    }
  },
  created(){
    this.previewshow = true
    OrganizationService.Get(this.$route.params.id, this.$route.query.inn).then(res => {
        this.previewshow = false
        this.OrganizationView = res.data
    }).catch(error => {
        this.makeToast(error.response.data.error,'danger')
    })
  },
  methods:{

  }
}
</script>

<style>

</style>