export default {
  data() {
    return {};
  },
  computed: {},
  methods: {
    // scrollToTop() {
    //   var content = document.getElementById("content");
    //   if (content) {
    //     content.scrollTo(0, 0);
    //   }
    // },
    convertType(time) {
      if (time) {
        const minutes = time.split(":")[0];
        const seconds = time.split(":")[1].split(".")[0];
        const millisecond = time.split(".")[1];
        return minutes * 60000 + seconds * 1000 + millisecond * 1;
      }
    },
    millisToMinutesAndSeconds(millis) {
      if (millis == null || millis == "") {
        return "-";
      } else {
        var minutes = Math.floor(millis / 60000);
        var seconds = Math.floor((millis - minutes * 60000) / 1000);
        var millisecond = (
          millis * 1 -
          minutes * 60000 -
          seconds * 1000
        ).toFixed();
        return (
          (minutes < 10 ? "0" : "") +
          minutes +
          ":" +
          (seconds < 10 ? "0" : "") +
          seconds +
          "." +
          (millisecond < 10 ? "0" : "") +
          millisecond
        );
      }
    },
  },
};
