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
        static public int FullScreenWidth = Screen.PrimaryScreen.WorkingArea.Width; //작업영역 가로크기
        static public int FullScreenHeight = Screen.PrimaryScreen.WorkingArea.Height; // 작업영역 세로크기


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

        public static void ImageCapture(int refX = 0, int refY = 0, int imgW = 0, int imgH = 0, string filepath = "")
        {
            if (imgW == 0 || imgH == 0)
                return;

            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)imgW, (int)imgH))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    System.Drawing.Size size = new System.Drawing.Size();
                    size.Width = bitmap.Size.Width - 4;
                    size.Height = bitmap.Size.Height - 4;
                    //g.CopyFromScreen(refX - 5, refY - 5, 0, 0, size);
                    g.CopyFromScreen(refX-7 , refY-7 , 0, 0, bitmap.Size);
                }
                bitmap.Save(filepath, ImageFormat.Png);
                bitmap.Dispose();
            }
        }


    }
}
