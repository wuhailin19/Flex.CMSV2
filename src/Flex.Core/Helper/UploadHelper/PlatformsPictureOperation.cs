using Flex.Core.Extensions;
using Flex.Core.Helper.ImgFiles;
using Microsoft.Extensions.PlatformAbstractions;
using SkiaSharp;

namespace Flex.Core.Helper.UploadHelper
{
    /// <summary>
    /// 多平台试用
    /// </summary>
    public class PlatformsPictureOperation
    {
        /// <summary>
        /// 储存位置
        /// </summary>
        private static string path = PlatformServices.Default.Application.ApplicationBasePath + "upload/image/{0}/thumb";
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgpath">待裁图片</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">裁减模式</param>
        /// <returns></returns>
        public static string MakeThumb(string imgpath, int width, int height, ImageThumbEnum mode = ImageThumbEnum.Cut)
        {
            const int quality = 100; //质量为100%
            using (var input = File.OpenRead(imgpath))
            using (var inputStream = new SKManagedStream(input))
            using (var original = SKBitmap.Decode(inputStream))
            {
                string filename = imgpath.Split('/').Last();
                string[] arr_filename = filename.Split('.');
                string ext = "";
                if (arr_filename.Length >= 2)
                {
                    ext = arr_filename[arr_filename.Length - 1];    //最后一个为扩展名
                }
                string thumb_name = $"{filename.Remove(filename.Length - ext.Length - 1)}_{width}x{height}.{ext}";  //文件名，缩略图保存为png
                string save_dir = $"{DateTime.Now.ToDefaultDateTimeStr()}";
                string savepath = string.Format(path, save_dir);
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                string thumb_file = $"{save_dir}/{thumb_name}";

                var baseimage = SKImage.FromBitmap(original);
                int towidth = width;
                int toheight = height;
                int ow = baseimage.Width;
                int oh = baseimage.Height;
                SKBitmap croppedBitmap = new SKBitmap();
                switch (mode)
                {
                    case ImageThumbEnum.HW://指定高宽缩放（可能变形）
                        croppedBitmap = original.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
                        break;
                    case ImageThumbEnum.W://指定宽，高按比例　　　　　　　　　　
                        toheight = baseimage.Height * width / baseimage.Width;
                        croppedBitmap = original.Resize(new SKImageInfo(width, toheight), SKFilterQuality.High);
                        break;
                    case ImageThumbEnum.H://指定高，宽按比例
                        towidth = baseimage.Width * height / baseimage.Height;
                        croppedBitmap = original.Resize(new SKImageInfo(towidth, height), SKFilterQuality.High);
                        break;
                    case ImageThumbEnum.Cut://指定高宽裁减（不变形）　
                        SKRect cropRect = new SKRect();
                        SKRect dest = new SKRect();
                        if ((double)baseimage.Width / (double)baseimage.Height > (double)towidth / (double)toheight)
                        {
                            oh = baseimage.Height;
                            ow = baseimage.Height * towidth / toheight;
                            cropRect = new SKRect(0, 0, towidth, toheight);
                            dest = new SKRect(0, 0, ow, oh);
                        }
                        else
                        {
                            dest = new SKRect(0, 0, baseimage.Width, baseimage.Height);
                            cropRect = new SKRect(0, 0, width, height);
                        }
                        croppedBitmap = new SKBitmap(towidth, toheight);
                        SKCanvas canvas = new SKCanvas(croppedBitmap);
                        canvas.DrawColor(SKColors.Transparent);
                        canvas.DrawBitmap(original, dest, cropRect);
                        canvas.Dispose();
                        break;
                    default:
                        break;
                }
                if (croppedBitmap == null) return "";
                using var image = SKImage.FromBitmap(croppedBitmap);
                using var output = File.OpenWrite($"{savepath}/{thumb_name}");
                image.Encode(GetFormat(ext), quality).SaveTo(output);
                croppedBitmap.Dispose();
                return thumb_file;
            }
        }
        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="ext">文件后缀</param>
        /// <returns></returns>
        public static SKEncodedImageFormat GetFormat(string ext)
        {
            switch (ext)
            {
                case "ico":
                    return SKEncodedImageFormat.Ico;
                case "bmp":
                    return SKEncodedImageFormat.Bmp;
                case "png":
                    return SKEncodedImageFormat.Png;
                case "gif":
                    return SKEncodedImageFormat.Gif;
                default:
                    return SKEncodedImageFormat.Jpeg;
            }
        }
    }
}
