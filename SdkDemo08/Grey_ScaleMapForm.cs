using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SdkDemo08
{
    public partial class Grey_ScaleMapForm : Form
    {
        private System.Drawing.Bitmap bmpHist;
        private int[] countPixel;
        private int maxPixel;

        public Grey_ScaleMapForm(Bitmap bmp)
        {
            InitializeComponent();

            bmpHist = bmp;
            countPixel = new int[256];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grey_ScaleMapForm_Load(object sender, EventArgs e)
        {
            //将图像数据复制到byte中
            Rectangle rect = new Rectangle(0, 0, bmpHist.Width, bmpHist.Height);
            System.Drawing.Imaging.BitmapData bmpdata = bmpHist.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmpHist.PixelFormat);
            IntPtr ptr = bmpdata.Scan0;

            int bytes = bmpHist.Width * bmpHist.Height * 3;
            byte[] grayValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, grayValues, 0, bytes);

            //统计直方图信息
            byte temp = 0;
            maxPixel = 0;
            Array.Clear(countPixel, 0, 256);
            for (int i = 0; i < bytes; i++)
            {
                temp = grayValues[i];
                countPixel[temp]++;
                if (countPixel[temp] > maxPixel)
                {
                    maxPixel = countPixel[temp];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr, bytes);

            bmpHist.UnlockBits(bmpdata);
        }

        private void Grey_ScaleMapForm_Paint(object sender, PaintEventArgs e)
        {
            //画出坐标系
            Graphics g = e.Graphics;

            Pen curPen = new Pen(Brushes.Black, 1);

            g.DrawLine(curPen, 50, 240, 320, 240);
            g.DrawLine(curPen, 50, 240, 50, 30);
            g.DrawLine(curPen, 100, 240, 100, 242);
            g.DrawLine(curPen, 150, 240, 150, 242);
            g.DrawLine(curPen, 200, 240, 200, 242);
            g.DrawLine(curPen, 250, 240, 250, 242);
            g.DrawLine(curPen, 300, 240, 300, 242);
            g.DrawString("0", new Font("New Timer", 8), Brushes.Black, new PointF(46, 242));
            g.DrawString("50", new Font("New Timer", 8), Brushes.Black, new PointF(92, 242));
            g.DrawString("100", new Font("New Timer", 8), Brushes.Black, new PointF(139, 242));
            g.DrawString("150", new Font("New Timer", 8), Brushes.Black, new PointF(189, 242));
            g.DrawString("200", new Font("New Timer", 8), Brushes.Black, new PointF(239, 242));
            g.DrawString("250", new Font("New Timer", 8), Brushes.Black, new PointF(289, 242));
            g.DrawLine(curPen, 48, 40, 50, 40);
            g.DrawString("0", new Font("New Timer", 8), Brushes.Black, new PointF(34, 234));
            g.DrawString(maxPixel.ToString(), new Font("New Timer", 8), Brushes.Black, new PointF(18, 34));

            double temp = 0;
            for (int i = 0; i < 256; i++)
            {
                temp = 200.0 * countPixel[i] / maxPixel;
                g.DrawLine(curPen, 50 + i, 240, 50 + i, 240 - (int)temp);
            }

            curPen.Dispose();
        }
    }
}
