using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Common.Capture
{
    public static class CapureManager
    {
        public static int FullScreenWidth = Screen.PrimaryScreen.WorkingArea.Width; //작업영역 가로크기
        public static int FullScreenHeight = Screen.PrimaryScreen.WorkingArea.Height; // 작업영역 세로크기
        public static int Cutoff = 170;

        public static void FullScreenCaptureAndSave(string filename)
        {
            // 주화면의 크기 정보 읽기
            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;

            // 화면 크기만큼의 Bitmap 생성
            using (Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                }
                // Bitmap 데이타를 파일로 저장
                bmp.Save(filename, ImageFormat.Png);
                bmp.Dispose();
            }
        }

        public static void SaveCaptureImage(int refX = 0, int refY = 0, int imgW = 0, int imgH = 0, string filepath = "")
        {
            if (imgW == 0 || imgH == 0 || !FileManager.IsFileExist(filepath))
                return;

            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)imgW, (int)imgH))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    System.Drawing.Size size = new System.Drawing.Size();
                    size.Width = bitmap.Size.Width - 4;
                    size.Height = bitmap.Size.Height - 4;
                    //g.CopyFromScreen(refX - 5, refY - 5, 0, 0, size);
                    g.CopyFromScreen(refX - 7, refY - 7, 0, 0, bitmap.Size);
                }
                bitmap.Save(filepath, ImageFormat.Png);
                bitmap.Dispose();
            }
        }

        public static void SaveContrastImage()
        {
            if(!FileManager.IsFileExist($"{FileManager.ImageRootPath}\\capture.png"))
                    return;

            //이진화 변환.
            using (Bitmap currentBitmap = new Bitmap($"{FileManager.ImageRootPath}\\capture.png"))
            {
                ApplyBinaryContrast(currentBitmap, Cutoff * 3);
                currentBitmap.Save($"{FileManager.ImageRootPath}\\contract.png", ImageFormat.Png);
                currentBitmap.Dispose();
            }
            
            //SaveBitmap(currentBitmap, $"{FileManager.ImageRootPath}\\contract.png");
        }

        public static void CutAsBorder(string Path, System.Windows.Rect Rect)
        {
            if (FileManager.IsFileExist(Path) && Rect != null)
            {
                try
                {
                    using (Bitmap bitmap = new Bitmap(Path))
                    {
                        using (Bitmap cropedBitmap = new Bitmap(bitmap.Size.Width - 4, bitmap.Size.Height - 4))
                        {
                            using (Graphics g = Graphics.FromImage(cropedBitmap))
                            {
                                g.DrawImage(bitmap, -2, -2);
                                g.Dispose();
                            }
                            bitmap.Dispose();

                            cropedBitmap.Save(Path, System.Drawing.Imaging.ImageFormat.Png);
                            cropedBitmap.Dispose();
                        }
                    }
                }
                catch
                { }

            }
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
        #endregion
    }
}
