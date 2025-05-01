using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using SkiaSharp;

namespace WEBASE.OfficeTools.Handlers
{
    public static class ImageHandler
    {
        public static (int Width, int Height) GetImage(this MemoryStream stream)
        {
            stream.Position = 0;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                stream.Position = 0;
                ms.Position = 0;

                using (var image = SKCodec.Create(ms))
                {
                    return (image.Info.Width, image.Info.Height);
                }
            }
        }

        public static ImagePartType GetImagePartType(this MemoryStream stream)
        {
            stream.Position = 0;

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                stream.Position = 0;
                ms.Position = 0;
                using (var codec = SKCodec.Create(ms))
                {
                    switch (codec.EncodedFormat)
                    {
                        case SKEncodedImageFormat.Bmp:
                            return ImagePartType.Bmp;
                        case SKEncodedImageFormat.Gif:
                            return ImagePartType.Gif;
                        case SKEncodedImageFormat.Jpeg:
                            return ImagePartType.Jpeg;
                        case SKEncodedImageFormat.Png:
                            return ImagePartType.Png;
                        default:
                            return ImagePartType.Jpeg;
                    }
                }
            }

            //stream.Position = 0;
            //using (var image = Image.FromStream(stream))
            //{
            //    stream.Position = 0;


            //    if (ImageFormat.Jpeg.Equals(image.RawFormat))
            //    {
            //        return ImagePartType.Jpeg;
            //    }
            //    else if (ImageFormat.Png.Equals(image.RawFormat))
            //    {
            //        return ImagePartType.Png;
            //    }
            //    else if (ImageFormat.Gif.Equals(image.RawFormat))
            //    {
            //        return ImagePartType.Gif;
            //    }
            //    else if (ImageFormat.Bmp.Equals(image.RawFormat))
            //    {
            //        return ImagePartType.Bmp;
            //    }
            //    else if (ImageFormat.Tiff.Equals(image.RawFormat))
            //    {
            //        return ImagePartType.Tiff;
            //    }

            //    return ImagePartType.Jpeg;
            //}
        }

        public static string GetImageType(this MemoryStream stream)
        {
            stream.Position = 0;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                stream.Position = 0;
                ms.Position = 0;
                using (var codec = SKCodec.Create(ms))
                {
                    switch (codec.EncodedFormat)
                    {
                        case SKEncodedImageFormat.Bmp:
                            return "bmp";
                        case SKEncodedImageFormat.Gif:
                            return "gif";
                        case SKEncodedImageFormat.Jpeg:
                            return "jpeg";
                        case SKEncodedImageFormat.Png:
                            return "png";
                        default:
                            return "";
                    }
                }
            }

            //stream.Position = 0;
            //using (var image = Image.FromStream(stream))
            //{
            //    stream.Position = 0;

            //    if (ImageFormat.Jpeg.Equals(image.RawFormat))
            //    {
            //        return "jpeg";
            //    }
            //    else if (ImageFormat.Png.Equals(image.RawFormat))
            //    {
            //        return "png";
            //    }
            //    else if (ImageFormat.Gif.Equals(image.RawFormat))
            //    {
            //        return "gif";
            //    }
            //    else if (ImageFormat.Bmp.Equals(image.RawFormat))
            //    {
            //        return "bmp";
            //    }
            //    else if (ImageFormat.Tiff.Equals(image.RawFormat))
            //    {
            //        return "tiff";
            //    }

            //    return "";
            //}
        }

        public static string GetBase64(this MemoryStream stream)
        {
            byte[] imageBytes = stream.ToArray();

            // Convert byte[] to Base64 String
            return Convert.ToBase64String(imageBytes);
        }
    }
}
