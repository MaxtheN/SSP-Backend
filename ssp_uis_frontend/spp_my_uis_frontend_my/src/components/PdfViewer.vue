<script setup>
import { computed, ref, nextTick, watch, onUnmounted } from 'vue';
import * as pdfjsLib from 'pdfjs-dist';
import ErrorPdf from '@/components/ErrorPdf.vue';
import ApiService from '@/services/api.service';
import { BIconCalendar4Week, BIconFileEarmarkRichtext, BIconChatLeftDots, BIconAppIndicator } from 'bootstrap-vue';
import packageJson from '/./package.json';
const emits = defineEmits(['print', 'download']);
const props = defineProps({
    link: String,
    name: String
});

const MAX_IMAGE_SIZE = 1024 * 1024;
const CMAP_URL = 'pdfjs-dist/cmaps/';
const CMAP_PACKED = true;

pdfjsLib.GlobalWorkerOptions.workerSrc = `https://unpkg.com/pdfjs-dist@${packageJson.dependencies['pdfjs-dist']}/build/pdf.worker.min.mjs`;

const previewRefs = ref([]);
const previewLeftRefs = ref([]);

const downloadLoading = ref(false);
const pdfLoading = ref(false);
const pdfError = ref(false); // Fixed initial state to avoid showing error unnecessarily
const activePage = ref('page-1');
const pdfZoom = ref(75);
const leftDriwer = ref(true);
const numOfPages = ref(0);
const pdfSizeInMB = ref(0);
const pdfScale = computed(() => (pdfZoom.value > 0 ? pdfZoom.value / 100 : 75 / 100));

let observer = null;
let url = null;

const loadPdf = async (isOnlyRenderLeft = false) => {
    pdfLoading.value = true;
    pdfError.value = false;

    try {
        if (!url) {
            // Faylni yuklab olish
            const response = await ApiService.print(props.link);

            // Create a Blob from the response data
            const blob = new Blob([response.data], { type: 'application/pdf' });

            // Convert the Blob to a URL
            url = URL.createObjectURL(blob);

            // Faylning o'lchamini olish
            const pdfSizeInBytes = blob.size; // Baytlarda
            pdfSizeInMB.value = (pdfSizeInBytes / (1024 * 1024)).toFixed(2); // MB ga o'tkazish
        }

        const pdf = await pdfjsLib.getDocument({
            url: url,
            maxImageSize: MAX_IMAGE_SIZE,
            cMapUrl: CMAP_URL,
            cMapPacked: CMAP_PACKED
        }).promise;

        numOfPages.value = pdf.numPages;
        await nextTick();

        for (let pageNumber = 1; pageNumber <= numOfPages.value; pageNumber++) {
            const page = await pdf.getPage(pageNumber);

            // Main preview rendering
            if (!isOnlyRenderLeft) {
                const canvas = previewRefs.value[pageNumber - 1];
                const context = canvas.getContext('2d');

                const viewport = page.getViewport({ scale: 1.5 });

                canvas.width = viewport.width;
                canvas.height = viewport.height;

                const renderContext = {
                    canvasContext: context,
                    viewport: viewport
                };
                await page.render(renderContext).promise;
            }

            // Left drawer preview rendering
            if (leftDriwer.value) {
                const canvasLeft = previewLeftRefs.value[pageNumber - 1];
                if (canvasLeft) {
                    const contextLeft = canvasLeft.getContext('2d');

                    const viewportLeft = page.getViewport({ scale: 264 / page.getViewport({ scale: 1 }).width });

                    canvasLeft.width = viewportLeft.width;
                    canvasLeft.height = viewportLeft.height;

                    const renderContextLeft = {
                        canvasContext: contextLeft,
                        viewport: viewportLeft
                    };
                    await page.render(renderContextLeft).promise;
                }
            }
        }
    } catch (error) {
        pdfError.value = true;
        console.error('PDF Error:', error);
    } finally {
        pdfLoading.value = false;
        initializeObserver();
    }
};

const initializeObserver = () => {
    // Clean up old observer
    if (observer) observer.disconnect();

    observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                activePage.value = entry.target.getAttribute('id');
                previewLeftRefs.value[Number(activePage.value.replace('page-', '')) - 1]?.scrollIntoView();
            }
        });
    });

    previewRefs.value.forEach((section) => {
        observer.observe(section);
    });
};

const download = async () => {
    downloadLoading.value = true;
    try {
        const response = await ApiService.print(props.link);

        const urlObject = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement('a');
        link.href = urlObject;
        link.setAttribute('download', 'file.pdf'); // Use dynamic name if needed
        document.body.appendChild(link);
        link.click();
        link.remove();
    } catch (error) {
        console.log('Download failed:', error);
    } finally {
        downloadLoading.value = false;
    }
};

const print = () => {
    window.open(props.link, '_blank');
};

watch(leftDriwer, (newVal) => {
    if (newVal) loadPdf(true);
});

onUnmounted(() => {
    if (observer) observer.disconnect();
});

loadPdf();
</script>

<template>
    <div class="pdf-wrapper">
        <div v-if="name" class="p-3 bg-white border-bottom">
            <div class="pdf-title">{{ name }}</div>
        </div>

        <div class="d-flex">
            <div class="pdf-main">
                <div v-if="leftDriwer" class="pdf-container-left">
                    <a v-for="item in numOfPages" :key="item + 'left'" :href="'#page-' + item" class="pdf-card" :class="{ ' active': activePage == 'page-' + item }">
                        <canvas ref="previewLeftRefs" />
                    </a>
                </div>
                <div class="pdf-container relative">
                    <b-overlay :show="pdfLoading" spinner-variant="info" rounded="sm" class="w-full mt-md-5">
                        <div class="container" id="pdf-box" :style="{ width: pdfScale * 100 + '%' }">
                            <ErrorPdf v-if="pdfError" />
                            <canvas v-for="item in numOfPages" :key="item" ref="previewRefs" :id="'page-' + item" class="pdf-preview" />
                        </div>
                    </b-overlay>
                </div>

                <div class="pdf-actions__bar">
                    <div class="d-flex justify-content-center align-items-center">
                        <div>
                            <svg width="52" height="52" viewBox="0 0 52 52" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path
                                    d="M8.9375 12.5478C8.9375 9.36375 11.5187 6.78256 14.7027 6.78256H33.4539C34.9828 6.78256 36.4493 7.38997 37.5305 8.47116L41.8402 12.7809C42.9213 13.8621 43.5288 15.3285 43.5288 16.8575V39.4521C43.5288 42.6363 40.9477 45.2173 37.7636 45.2173H14.7027C11.5187 45.2173 8.9375 42.6363 8.9375 39.4521V12.5478Z"
                                    fill="url(#paint0_linear_425_1501)"
                                />
                                <path
                                    d="M15.827 31.2176V22.9449H18.9293C19.5648 22.9449 20.098 23.0634 20.5289 23.3004C20.9625 23.5373 21.2896 23.8632 21.5105 24.2779C21.734 24.6899 21.8457 25.1585 21.8457 25.6836C21.8457 26.2141 21.734 26.6854 21.5105 27.0974C21.287 27.5094 20.9571 27.8339 20.5208 28.0709C20.0846 28.3052 19.5473 28.4223 18.9091 28.4223H16.853V27.1903H18.7071C19.0787 27.1903 19.383 27.1257 19.62 26.9964C19.857 26.8672 20.032 26.6894 20.1451 26.4632C20.2609 26.237 20.3188 25.9771 20.3188 25.6836C20.3188 25.3901 20.2609 25.1316 20.1451 24.9081C20.032 24.6845 19.8557 24.5108 19.616 24.387C19.379 24.2604 19.0734 24.1971 18.699 24.1971H17.3256V31.2176H15.827ZM25.9508 31.2176H23.1474V22.9449H26.0074C26.8287 22.9449 27.5343 23.1105 28.124 23.4417C28.7165 23.7703 29.1716 24.2429 29.4893 24.8596C29.8071 25.4763 29.966 26.2141 29.966 27.0732C29.966 27.9349 29.8058 28.6755 29.4853 29.2949C29.1675 29.9142 28.7084 30.3895 28.1078 30.7208C27.51 31.052 26.791 31.2176 25.9508 31.2176ZM24.6461 29.921H25.8781C26.4544 29.921 26.9351 29.8159 27.3202 29.6059C27.7053 29.3932 27.9947 29.0767 28.1886 28.6566C28.3825 28.2338 28.4795 27.706 28.4795 27.0732C28.4795 26.4403 28.3825 25.9152 28.1886 25.4978C27.9947 25.0777 27.7079 24.764 27.3282 24.5566C26.9512 24.3466 26.4827 24.2415 25.9225 24.2415H24.6461V29.921ZM31.3899 31.2176V22.9449H36.6896V24.2012H32.8885V26.4471H36.326V27.7033H32.8885V31.2176H31.3899Z"
                                    fill="white"
                                />
                                <path
                                    d="M37.306 8.14144L42.2752 13.1106C43.1452 13.9807 43.6341 15.1608 43.6341 16.3913H37.8689C35.7461 16.3913 34.0254 14.6705 34.0254 12.5478V6.78256C35.2559 6.78256 36.436 7.27136 37.306 8.14144Z"
                                    fill="#059875"
                                />
                                <defs>
                                    <linearGradient id="paint0_linear_425_1501" x1="26.2332" y1="6.78256" x2="26.2332" y2="45.2173" gradientUnits="userSpaceOnUse">
                                        <stop stop-color="#35C9A5" />
                                        <stop offset="1" stop-color="#0AAD86" />
                                    </linearGradient>
                                </defs>
                            </svg>
                        </div>
                        <div class="text-body1">
                            <div class="text-white font-weight-medium" v-if="name">{{ name }}</div>
                            <div class="text-grey3 font-weight-normal" v-if="pdfSizeInMB">{{ pdfSizeInMB }} мб</div>
                        </div>
                    </div>
                    <div class="d-flex gap-2">
                        <b-button size="sm" @click="pdfZoom += 10">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path
                                    fill-rule="evenodd"
                                    clip-rule="evenodd"
                                    d="M15.3431 15.2426C17.6863 12.8995 17.6863 9.1005 15.3431 6.75736C13 4.41421 9.20101 4.41421 6.85786 6.75736C4.51472 9.1005 4.51472 12.8995 6.85786 15.2426C9.20101 17.5858 13 17.5858 15.3431 15.2426ZM16.7574 5.34315C19.6425 8.22833 19.8633 12.769 17.4195 15.9075C17.4348 15.921 17.4498 15.9351 17.4645 15.9497L21.7071 20.1924C22.0976 20.5829 22.0976 21.2161 21.7071 21.6066C21.3166 21.9971 20.6834 21.9971 20.2929 21.6066L16.0503 17.364C16.0356 17.3493 16.0215 17.3343 16.008 17.319C12.8695 19.7628 8.32883 19.542 5.44365 16.6569C2.31946 13.5327 2.31946 8.46734 5.44365 5.34315C8.56785 2.21895 13.6332 2.21895 16.7574 5.34315ZM10.1005 7H12.1005V10H15.1005V12H12.1005V15H10.1005V12H7.10052V10H10.1005V7Z"
                                    fill="white"
                                />
                            </svg>
                        </b-button>
                        <b-button size="sm" @click="pdfZoom -= 10">
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path
                                    fill-rule="evenodd"
                                    clip-rule="evenodd"
                                    d="M15.3431 15.2426C17.6863 12.8995 17.6863 9.1005 15.3431 6.75736C13 4.41421 9.20101 4.41421 6.85786 6.75736C4.51472 9.1005 4.51472 12.8995 6.85786 15.2426C9.20101 17.5858 13 17.5858 15.3431 15.2426ZM16.7574 5.34315C19.6425 8.22833 19.8633 12.769 17.4195 15.9075C17.4348 15.921 17.4498 15.9351 17.4645 15.9497L21.7071 20.1924C22.0976 20.5829 22.0976 21.2161 21.7071 21.6066C21.3166 21.9971 20.6834 21.9971 20.2929 21.6066L16.0503 17.364C16.0356 17.3493 16.0215 17.3343 16.008 17.319C12.8695 19.7628 8.32883 19.542 5.44365 16.6569C2.31946 13.5327 2.31946 8.46734 5.44365 5.34315C8.56785 2.21895 13.6332 2.21895 16.7574 5.34315ZM7.10052 10V12H15.1005V10L7.10052 10Z"
                                    fill="white"
                                />
                            </svg>
                        </b-button>

                        <b-button size="sm" @click="print">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                <path
                                    d="M18 16.75H16C15.8011 16.75 15.6103 16.671 15.4697 16.5303C15.329 16.3897 15.25 16.1989 15.25 16C15.25 15.8011 15.329 15.6103 15.4697 15.4697C15.6103 15.329 15.8011 15.25 16 15.25H18C18.3315 15.25 18.6495 15.1183 18.8839 14.8839C19.1183 14.6495 19.25 14.3315 19.25 14V10C19.25 9.66848 19.1183 9.35054 18.8839 9.11612C18.6495 8.8817 18.3315 8.75 18 8.75H6C5.66848 8.75 5.35054 8.8817 5.11612 9.11612C4.8817 9.35054 4.75 9.66848 4.75 10V14C4.75 14.3315 4.8817 14.6495 5.11612 14.8839C5.35054 15.1183 5.66848 15.25 6 15.25H8C8.19891 15.25 8.38968 15.329 8.53033 15.4697C8.67098 15.6103 8.75 15.8011 8.75 16C8.75 16.1989 8.67098 16.3897 8.53033 16.5303C8.38968 16.671 8.19891 16.75 8 16.75H6C5.27065 16.75 4.57118 16.4603 4.05546 15.9445C3.53973 15.4288 3.25 14.7293 3.25 14V10C3.25 9.27065 3.53973 8.57118 4.05546 8.05546C4.57118 7.53973 5.27065 7.25 6 7.25H18C18.7293 7.25 19.4288 7.53973 19.9445 8.05546C20.4603 8.57118 20.75 9.27065 20.75 10V14C20.75 14.7293 20.4603 15.4288 19.9445 15.9445C19.4288 16.4603 18.7293 16.75 18 16.75Z"
                                    fill="white"
                                />
                                <path
                                    d="M16 8.75C15.8019 8.74741 15.6126 8.66756 15.4725 8.52747C15.3324 8.38737 15.2526 8.19811 15.25 8V4.75H8.75V8C8.75 8.19891 8.67098 8.38968 8.53033 8.53033C8.38968 8.67098 8.19891 8.75 8 8.75C7.80109 8.75 7.61032 8.67098 7.46967 8.53033C7.32902 8.38968 7.25 8.19891 7.25 8V4.5C7.25 4.16848 7.3817 3.85054 7.61612 3.61612C7.85054 3.3817 8.16848 3.25 8.5 3.25H15.5C15.8315 3.25 16.1495 3.3817 16.3839 3.61612C16.6183 3.85054 16.75 4.16848 16.75 4.5V8C16.7474 8.19811 16.6676 8.38737 16.5275 8.52747C16.3874 8.66756 16.1981 8.74741 16 8.75ZM15.5 20.75H8.5C8.16848 20.75 7.85054 20.6183 7.61612 20.3839C7.3817 20.1495 7.25 19.8315 7.25 19.5V12.5C7.25 12.1685 7.3817 11.8505 7.61612 11.6161C7.85054 11.3817 8.16848 11.25 8.5 11.25H15.5C15.8315 11.25 16.1495 11.3817 16.3839 11.6161C16.6183 11.8505 16.75 12.1685 16.75 12.5V19.5C16.75 19.8315 16.6183 20.1495 16.3839 20.3839C16.1495 20.6183 15.8315 20.75 15.5 20.75ZM8.75 19.25H15.25V12.75H8.75V19.25Z"
                                    fill="white"
                                />
                            </svg>
                        </b-button>
                        <b-button size="sm" @click="download" :disabled="downloadLoading">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                <path
                                    d="M11 5C11 4.44772 11.4477 4 12 4C12.5523 4 13 4.44772 13 5V12.1578L16.2428 8.91501L17.657 10.3292L12.0001 15.9861L6.34326 10.3292L7.75748 8.91501L11 12.1575V5Z"
                                    fill="white"
                                />
                                <path d="M4 14H6V18H18V14H20V18C20 19.1046 19.1046 20 18 20H6C4.89543 20 4 19.1046 4 18V14Z" fill="white" />
                            </svg>
                        </b-button>
                    </div>
                </div>
            </div>

            <div class="pdf-right_side">
                <!-- date -->
                <div v-if="$slots['right-side__number']" class="mb-2">
                    <div class="text-grey2 text-body1">
                        <BIconFileEarmarkRichtext class="mr-1" />
                        {{ $t('docNumber') }}:
                    </div>
                    <div class="text-black"><slot name="right-side__number"></slot></div>
                </div>
                <!-- date -->
                <div v-if="$slots['right-side__date']" class="mb-2">
                    <div class="text-grey2 text-body1">
                        <BIconCalendar4Week class="mr-1" />
                        {{ $t('docOn') }}:
                    </div>
                    <div class="text-black"><slot name="right-side__date"></slot></div>
                </div>
                <!-- status -->
                <div v-if="$slots['right-side__status']" class="mb-2">
                    <div class="text-grey2 text-body1">
                        <BIconAppIndicator class="mr-1" />
                        {{ $t('status') }}:
                    </div>
                    <div class="text-black"><slot name="right-side__status"></slot></div>
                </div>

                <!-- message -->
                <div v-if="$slots['right-side__message']" class="mb-2">
                    <div class="text-grey2 text-body1">
                        <BIconChatLeftDots class="mr-1" />
                        {{ $t('message') }}:
                    </div>
                    <div class="text-black"><slot name="right-side__message"></slot></div>
                </div>

                <!-- actions -->
                <slot name="right-side">
                    {{ $slots }}
                </slot>
            </div>
        </div>
    </div>
</template>
<style>
.pdf-title {
    color: var(--Black, #000107);
    font-family: 'Museo Sans', serif;
    font-size: 18px;
    font-style: normal;
    font-weight: 600;
    line-height: normal;
}

.pdf-wrapper {
    background: #f0f0f0;
    border-radius: 8px;
    border: 1px solid var(--Light-gray, #d5d7e1);
    overflow: hidden;
}

.pdf-container {
    height: calc(100vh - 100px) !important;
    overflow-y: auto;
    width: 100%;
    padding: 16px 0;
}

.pdf-main {
    width: 100%;
    display: flex;
    position: relative;
}

.pdf-container-left {
    height: calc(100vh - 100px) !important;
    overflow: auto;
    display: inline-flex;
    padding: 24px 24px 24px 72px;
    flex-direction: column;
    align-items: flex-start;
    gap: 12px;
    flex-shrink: 0;
    border-right: 1px solid var(--Secondary-Line-gray, #d3d3d3);
}

.pdf-preview {
    background: #fff;
    box-shadow: 0px 0px 5px 0px rgba(0, 0, 0, 0.05);
    border-radius: 7px;
    border: 1px solid rgba(26, 26, 26, 0.13);
    margin-top: 10px;
    margin-bottom: 10px;
    width: 100%;
}

.pdf-card {
    width: 264px;
    height: 341px;
    flex-shrink: 0;
    cursor: pointer;
    border-radius: 7px;
    border: 1px solid rgba(26, 26, 26, 0.13);
    background-color: #fff;
    overflow: hidden;
}

.pdf-card.active {
    background-color: #f6f7fa;
}

.pdf-right_side {
    border-radius: 0px 0px 8px 0px;
    border-left: 1px solid #d5d7e1;
    background: #fff;
    width: 275px;
    flex-shrink: 0;
    padding: 16px;
}

.pdf-actions__bar {
    position: absolute;
    bottom: 12px;
    left: 12px;
    right: 12px;
    display: flex;
    padding: 12px 72px;
    justify-content: space-between;
    align-items: center;
    border-radius: 8px;
    border: 1px solid #b8b8b8;
    background: rgba(0, 1, 7, 0.52);
    backdrop-filter: blur(6px);
}
</style>
