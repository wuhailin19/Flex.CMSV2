using SkiaSharp;

namespace Flex.Core
{
    public class AuthCodeHelper
    {
        public static MemoryStream CreatePng(out string code)
        {
            Random random = new();
            code = random.Next(1000, 9999).ToString();
            //验证码颜色集合  
            var colors = new[] { SKColors.Black, SKColors.Red, SKColors.DarkBlue, SKColors.Green, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
            //验证码字体集合
            var fonts = new[] { "DejaVu Sans" };
            //相当于js的 canvas.getContext('2d')
            var imagwidth = 130;
            var imageheight = 30;
            using var image2d = new SKBitmap(imagwidth, imageheight, SKColorType.Bgra8888, SKAlphaType.Premul);
            //相当于前端的canvas
            using var canvas = new SKCanvas(image2d);
            //填充白色背景
            canvas.DrawColor(SKColors.AntiqueWhite);
            //样式 跟xaml差不多
            using var drawStyle = new SKPaint();
            //填充验证码到图片
            for (int i = 0; i < code.Length; i++)
            {
                drawStyle.IsAntialias = true;
                drawStyle.TextSize = 30;
                var font = SKTypeface.FromFamilyName(fonts[random.Next(0, fonts.Length - 1)], SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
                drawStyle.Typeface = font;
                drawStyle.Color = colors[random.Next(0, colors.Length - 1)];
                //写字
                canvas.DrawText(code[i].ToString(), (i + 1) * 25, 28, drawStyle);
            }
            //生成三条干扰线
            for (int i = 0; i < 10; i++)
            {
                drawStyle.Color = colors[random.Next(colors.Length)];
                drawStyle.StrokeWidth = 1;
                canvas.DrawLine(random.Next(imagwidth), random.Next(imageheight), random.Next(imagwidth), random.Next(imageheight), drawStyle);
            }
            //巴拉巴拉的就行了
            using var img = SKImage.FromBitmap(image2d);
            using var p = img.Encode(SKEncodedImageFormat.Png, 100);
            using var ms = new MemoryStream();
            //保存到流
            p.SaveTo(ms);
            return ms;
        }
        public static string CreateCheckCodeImage(out string code)
        {
            var codestr = Convert.ToBase64String(CreatePng(out code).ToArray());
            return codestr;
        }
    }
}
