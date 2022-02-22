using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Common.OCR
{
    public class OCRManager
    {
        public static string ImageContrastAndOCR(int Cutoff = 175)
        {
            //이진화 변환.
            Bitmap currentBitmap;
            currentBitmap = new Bitmap($"{FileManager.ImageRootPath}\\capture.png");
            ApplyBinaryContrast(currentBitmap, Cutoff * 3);
            SaveBitmap(currentBitmap, $"{FileManager.ContractImagePath}\\contract.png");

            //OCR이미지 변환 및 보내기.
            return OCR($"{FileManager.ContractImagePath}\\contract.png");
        }

        #region 비트맵 구하기 - GetBitmap(filePath)

        /// <summary>
        /// 비트맵 구하기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>비트맵</returns>
        public static Bitmap GetBitmap(string filePath)
        {
            using (Bitmap bitmap = new Bitmap(filePath))
            {
                return new Bitmap(bitmap);
            }
        }

        #endregion

        #region 이진 대비 적용하기 - ApplyBinaryContrast(targetBitmap, cutoff) / 비트맵저장
        /// <summary>
        /// 이진 대비 적용하기
        /// </summary>
        /// <param name="targetBitmap">타겟 비트맵</param>
        /// <param name="cutoff">컷오프</param>
        public static void ApplyBinaryContrast(Bitmap targetBitmap, int cutoff)
        {
            CustomBitmapHelper helper = new CustomBitmapHelper(targetBitmap);

            helper.LockBitmap();

            for (int y = 0; y < targetBitmap.Height; y++)
            {
                for (int x = 0; x < targetBitmap.Width; x++)
                {
                    byte red;
                    byte green;
                    byte blue;
                    byte alpha;

                    helper.GetPixel(x, y, out red, out green, out blue, out alpha);

                    if (red + green + blue > cutoff)
                    {
                        helper.SetPixel(x, y, 255, 255, 255, 255);
                    }
                    else
                    {
                        helper.SetPixel(x, y, 0, 0, 0, 255);
                    }
                }
            }

            helper.UnlockBitmap();
        }

        /// <summary>
        /// 비트맵 저장하기
        /// </summary>
        /// <param name="bitmap">비트맵</param>
        /// <param name="filePath">파일 경로</param>
        public static void SaveBitmap(Bitmap bitmap, string filePath)
        {
            string extension = Path.GetExtension(filePath);

            switch (extension.ToLower())
            {
                case ".bmp": bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                case ".exif": bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Exif); break;
                case ".gif": bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Gif); break;
                case ".jpg":
                case ".jpeg": bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                case ".png": bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png); break;
                case ".tif":
                case ".tiff": bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Tiff); break;
                default: throw new NotSupportedException("Unknown file extension " + extension);
            }

            bitmap.Dispose();
            bitmap = null;
        }


        #endregion

        #region ocr
        public static string OCR(string filepath)
        {
            using (Bitmap bitmap = new Bitmap(filepath))
            {
                string returntext = OCR(bitmap);
                bitmap.Dispose();
                return returntext;
            }
        }

        public static string OCR(Bitmap bitmap)
        {
            try
            {
                Pix pix = PixConverter.ToPix(bitmap);
                var engine = new TesseractEngine(FileManager.TessadataFilePath, "kor", EngineMode.TesseractOnly);
                var result = engine.Process(pix);
                string resultText = result.GetText();
                if (string.IsNullOrEmpty(resultText))
                    return "fail";
                else
                    return resultText;
            }
            catch
            {
                return "OCR Error";
            }
        }
        #endregion
    }
}
