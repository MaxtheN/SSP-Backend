import { VueEditor, Quill } from 'vue2-editor';



export const toolbarOptions = {

 toolbarOptions :[
   ['bold', 'italic', 'underline'],
   [{ list: 'ordered' }, { list: 'bullet' }],
   [{ color: [] }, { background: [] }],
   [{ font: ['initial', 'serif', 'monospace'] }],
   [{ align: [] }],
   ['clean']
],

var Font = Quill.import('formats/font');
Font.whitelist = ['initial', 'serif', 'monospace'];
}