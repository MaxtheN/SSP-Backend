import { BojxonaReportVisibles } from "./data";

export default [
    {
        title: 'BojxonaReports',
        icon: 'ClipboardIcon',
        visible: BojxonaReportVisibles,
        children: [
            {
                title: 'BojxonaImtiyozReportByContractor',
                route: 'BojxonaImtiyozReportByContractor',
                visible: 'BojxonaImtiyozReportByContractorView'
            },
            {
                title: 'bojxona',
                route: 'bojxona',
                visible: 'ReportPrtnApplicationByContractTypeInfoView'
            },
        ]
    }
];
