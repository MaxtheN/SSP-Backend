<template>
    <div style="position: fixed; bottom: 2rem; top: auto; right: 1.8rem; z-index: 1200; display: inline-block">
        <div class="d-flex flex-column card shadow-lg overflow-hidden border-0" v-if="historySidebar">
            <div class="chat-conversation-header p-2 bg-white d-flex justify-content-between align-items-center" style="flex: none">
                <h6 class="mb-0 text-primary font-weight-bolder">Chat {{ documentId }}</h6>
                <b-button variant="outline" size="sm" @click="historySidebar = !historySidebar">
                    <b-icon icon="x-lg" scale="0.7" aria-hidden="true"></b-icon>
                </b-button>
            </div>
            <div class="chat-conversation p-3">
                <div v-if="chatLoading" class="d-flex flex-column justify-content-center align-items-center h-100">
                    <div>
                        <b-spinner style="width: 3rem; height: 3rem" class="text-primary" />
                    </div>
                </div>
                <ul v-else class="list-unstyled" id="chat-container">
                    <li v-for="item in chatMessagesData" :key="item.id" :class="{ right: item.contractorId }">
                        <div class="conversation-list">
                            <div class="ctext-wrap">
                                <div class="conversation-name">{{ item.name }}</div>
                                <p>{{ item.messageText }}</p>
                                <p class="chat-time mb-0">
                                    <b-icon-clock class="me-1" />
                                    {{ item.createdAt }}
                                </p>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>

            <div class="p-2 chat-input-section bg-white position-sticky" style="bottom: 0">
                <form @submit.prevent="formSubmit" class="row">
                    <div class="col">
                        <div class="position-relative">
                            <input type="text" v-model="messageText" class="form-control chat-input rounded" placeholder="Enter Message..." />
                        </div>
                    </div>
                    <div class="col-auto">
                        <button type="submit" :disabled="!messageText || sendLoading" class="btn btn-primary btn-rounded chat-send">
                            <b-spinner small v-if="sendLoading" />
                            <b-icon v-else icon="chat-text" scale="0.7" aria-hidden="true"></b-icon>
                        </button>
                    </div>
                </form>
            </div>
        </div>

        <button @click="historySidebar = !historySidebar" data-title="Call-center" class="pmd-floating-action-btn btn bg-light">
            <b-icon-messenger scale="0.8" class="text-success"></b-icon-messenger>
        </button>
    </div>
</template>

<script>
import { BSpinner, BIcon, BIconClock, BButton } from 'bootstrap-vue';
import DocumentChatService from '@/services/documentchat.service';
import { nextTick } from 'vue';

export default {
    components: {
        BSpinner,
        BButton,
        BIcon,
        BIconClock
    },
    props: {
        tableId: {
            type: Number,
            default: null
        },
        documentId: {
            type: Number,
            default: null
        }
    },
    data() {
        return {
            historySidebar: false,
            chatMessagesData: [],
            chatLoading: false,
            sendLoading: false,
            messageText: '',
            chatFilter: {
                search: '',
                sortBy: 'createdAt',
                orderType: 'desc',
                page: 1,
                pageSize: 1000
            }
        };
    },
    watch: {
        historySidebar: {
            handler(e) {
                this.GetData();
            }
        }
    },
    methods: {
        GetData() {
            this.chatLoading = true;
            DocumentChatService.GetList({ ...this.chatFilter, tableId: this.tableId, documentId: this.documentId })
                .then((res) => {
                    this.chatMessagesData = res.data.rows.reverse();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(async () => {
                    this.chatLoading = false;
                    await nextTick();
                    const container = this.$el.querySelector('#chat-container');
                    container?.scrollIntoView({ behavior: 'smooth', block: 'end' });
                });
        },
        formSubmit() {
            this.sendLoading = true;
            const createChatData = {
                messageText: this.messageText,
                tableId: this.tableId,
                documentId: this.documentId
            };
            DocumentChatService.Create(createChatData)
                .then(() => {
                    this.messageText = '';
                    this.GetData();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.sendLoading = false;
                });
        }
    }
};
</script>

<style lang="scss">
.chat-conversation {
    flex: 100%;
    max-height: 500px;
    min-height: 350px;

    overflow: auto;
    & ul {
        height: 100%;
    }
}
.chat-input-section {
    border-top: 1px solid #eff2f7;
}

.chat-conversation-header {
    box-shadow: rgba(33, 35, 38, 0.1) 0px 10px 10px -10px;
    background: rgba(2, 70, 155, 0.12) !important;
}

.chat-input {
    border-radius: 30px;
    background-color: #eff2f7 !important;
    border-color: #eff2f7 !important;
}

.chat-conversation .conversation-list {
    margin-bottom: 24px;
    display: inline-block;
    position: relative;
}

.chat-conversation li {
    clear: both;
}
.chat-conversation .conversation-list .ctext-wrap {
    padding: 12px 24px;
    background-color: rgba(85, 110, 230, 0.1);
    border-radius: 8px 8px 8px 0;
    overflow: hidden;
    max-width: 350px;
}
.chat-input-links {
    position: absolute;
    right: 16px;
    top: 50%;
    transform: translateY(-50%);
}
.chat-conversation .conversation-list .chat-time {
    font-size: 12px;
}
.list-inline,
.list-unstyled {
    padding-left: 0;
    list-style: none;
}

.chat-input-links li a {
    font-size: 16px;
    line-height: 36px;
    padding: 0 4px;
    display: inline-block;
}
.chat-conversation .conversation-list .ctext-wrap .conversation-name {
    font-weight: 600;
    color: #556ee6;
    margin-bottom: 4px;
}
.chat-conversation .right .conversation-list {
    float: right;
}
.chat-conversation .right .conversation-list .ctext-wrap {
    background-color: #eff2f7;
    text-align: right;
    border-radius: 8px 8px 0 8px;
}

.pmd-floating-action-btn {
    --highlight: #ff0196;
    float: right;
    display: flex;
    justify-content: center;
    align-items: center;
    position: relative;
    box-shadow: 0 1rem 3rem rgba(34, 41, 47, 0.175);
    border-radius: 50%;
    height: 56px;
    width: 56px;
    font-size: 1.5rem;
    padding: 0;
    animation: shadow 1.5s ease-in-out infinite alternate;
}
@keyframes shadow {
    0% {
        box-shadow: 0 0 25px 1px #28c76f;
        animation-timing-function: ease-in;
    }
    100% {
        box-shadow: 0 0 0 1px #28c76f;
    }
}
</style>
