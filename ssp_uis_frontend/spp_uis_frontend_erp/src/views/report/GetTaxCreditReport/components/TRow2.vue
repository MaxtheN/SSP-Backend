<template>
   <b-tr>
      <b-th>{{ $t('Имтиёздан фойдаланган субъект сони') }}</b-th>
      <b-th>{{ $t('Яратиладиган иш ўрни сони') }}</b-th>
      <b-th>{{ $t('Ижтимоий реестрдаги иш билан банд бўлганлар сони') }}</b-th>
      <b-th>{{ $t('Ер солиғи бўйича имтиёз суммаси(млн.сўм)') }}</b-th>
   </b-tr>
</template>

<script setup>
import { BTr, BTh } from 'bootstrap-vue';
</script>
