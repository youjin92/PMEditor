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
