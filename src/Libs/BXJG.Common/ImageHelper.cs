using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using SkiaSharp;

namespace BXJG.Common
{
    public static class ImageHelper
    {
        /// <summary>
        /// 基于skia生成等比例缩略图
        /// </summary>
        /// <param name="orgPath">源图片路径</param>
        /// <param name="thuPath">目标图片路径</param>
        /// <param name="maxDimension">最大尺寸</param>
        /// <param name="quality">生成缩略图后，写入png的质量，0-100(最高)</param>
        /// <param name="filterQuality">生成缩略图质量</param>
        public static void MakeThumb(string orgPath, string thuPath, int maxDimension=180, int quality=100, SKFilterQuality filterQuality= SKFilterQuality.Medium)
        {
           // const int maxDimension = 180; // 最大尺寸
           // const int quality = 100; // 质量为 100%

            using var input = File.OpenRead(orgPath);
            using var inputStream = new SKManagedStream(input);
            using var original = SKBitmap.Decode(inputStream);

            int width, height;
            if (original.Width > original.Height)
            {
                width = maxDimension;
                height = original.Height * maxDimension / original.Width;
            }
            else
            {
                height = maxDimension;
                width = original.Width * maxDimension / original.Height;
            }

            using (var resized = original.Resize(new SKImageInfo(width, height), filterQuality))
            {
                if (resized != null)
                {
                    using var image = SKImage.FromBitmap(resized);
                    using var output = File.OpenWrite(thuPath);
                    image.Encode(SKEncodedImageFormat.Png, quality).SaveTo(output);
                }
            }
        }
    }
}
