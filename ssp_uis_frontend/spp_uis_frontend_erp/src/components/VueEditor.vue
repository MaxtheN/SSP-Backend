<template>
   <div>
      <vue-editor
         :editorToolbar="customToolbar"
         :editorOptions="editorSettings"
         v-model="valuecomp"
         :label="$t('content')"
      ></vue-editor>
   </div>
</template>

<script>
import { VueEditor, Quill } from 'vue2-editor';

var fonts = ['Sans-Serif', 'Arial', 'Courier', 'Garamond', 'Tahoma', 'Times New Roman', 'Verdana'];
// generate code friendly names

function getFontName(font) {
   return font.toLowerCase().replace(/\s/g, '-');
}
const fontNames = fonts.map((font) => getFontName(font));

let fontStyles = '';
fonts.forEach((font) => {
   const fontName = getFontName(font);
   fontStyles +=
      '.ql-snow .ql-picker.ql-font .ql-picker-label[data-value=' +
      fontName +
      ']::before, .ql-snow .ql-picker.ql-font .ql-picker-item[data-value=' +
      fontName +
      ']::before {' +
      "content: '" +
      font +
      "';" +
      "font-family: '" +
      font +
      "', sans-serif;" +
      '}' +
      '.ql-font-' +
      fontName +
      '{' +
      " font-family: '" +
      font +
      "', sans-serif;" +
      '}';
});
const node = document.createElement('style');
node.innerHTML = fontStyles;
document.body.appendChild(node);

const toolbarOptions = [
   ['bold', 'italic', 'underline'],
   [{ header: [false, 1, 2, 3, 4, 5, 6] }],
   [{ font: fontNames }],
   [{ list: 'ordered' }, { list: 'bullet' }, { list: 'check' }],
   [{ color: [] }, { background: [] }],
   [{ align: [] }],
   [{ align: '' }, { align: 'center' }, { align: 'right' }, { align: 'justify' }],
   ['clean'],
   ['link', 'image', 'video'],
   ['blockquote', 'code-block']
];

// Add fonts to whitelist
const Font = Quill.import('formats/font');
Font.whitelist = fontNames;
Quill.register(Font, true);

const quill = new Quill('#editor', {
   modules: {
      toolbar: toolbarOptions
   },
   theme: 'snow'
});

export default {
   components: {
      VueEditor
   },
   props: ['value'],
   emits: ['input'],
   computed: {
      valuecomp: {
         get() {
            return this.value;
         },
         set(v) {
            console.log(v);
            this.$emit('input', v);
         }
      }
   },

   data() {
      return {
         customToolbar: toolbarOptions,
         editorSettings: {
            formats: {
               Font: true
            }
         }
      };
   }
};
</script>

<style lang="scss" scoped></style>
