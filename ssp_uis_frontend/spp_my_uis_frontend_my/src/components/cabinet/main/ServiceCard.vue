<template>
    <div id="chart">
        <apexchart type="donut" width="380" :options="chartOptions" :series="series" />
    </div>
</template>

<script>
import VueApexCharts from 'vue-apexcharts';

export default {
    components: {
        apexchart: VueApexCharts
    },
    data() {
        return {
            series: [10, 5, 25], // Har bir bo'lakning qiymati
            chartOptions: {
                chart: {
                    type: 'donut',
                    width: 380
                },
                plotOptions: {
                    pie: {
                        startAngle: 0,
                        endAngle: 360,
                        expandOnClick: true,
                        customScale: 1
                    }
                },
                donut: {
                    size: '90%',
                    background: '#000'
                },

                stroke: {
                    width: 6, // Bo'laklar orasidagi bo'shliq
                    colors: '#fff' // Bo'shliq rangi oq rang
                },
                dataLabels: {
                    enabled: true,
                    formatter: function (val, opts) {
                        return opts.w.globals.series[opts.seriesIndex]; // Har bir bo'lak qiymatini markazda ko'rsatish
                    },
                    style: {
                        fontSize: '12px',
                        fontWeight: 'bold',
                        colors: ['#000'] // Matnning rangi qora
                    }
                },
                colors: ['#E5E7EB', '#FFECB3', '#3B82F6'], // Bo'laklarning ranglari
                legend: {
                    show: false // Legenda yashirilgan
                },
                tooltip: {
                    enabled: true // Tooltip ko'rsatish
                }
            }
        };
    }
};
</script>
