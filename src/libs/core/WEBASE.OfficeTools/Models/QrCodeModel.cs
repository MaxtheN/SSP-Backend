namespace WEBASE.OfficeTools.Models
{
    public class QrCodeModel
    {
        private string? text;
        private int height;
        private int width;
        private double dpi;

        public string? Text
        {
            get { return text; }
            set { text = value; }
        }
        public int Height
        {
            get
            {
                if (height == default)
                    return 256;
                return height;
            }
            set { height = value; }
        }
        public int Width
        {
            get
            {
                if (width == default)
                    return 256;
                return width;
            }
            set { width = value; }
        }
        public double Dpi
        {
            get
            {
                if (dpi == default)
                    return 200;
                return dpi;
            }
            set { dpi = value; }
        }

    }
    public class ImageModel
    {
        public byte[]? Bytes { get; set; } = null;
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
