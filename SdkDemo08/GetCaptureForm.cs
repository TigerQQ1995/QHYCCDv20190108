using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.IO;
using System.Threading;

namespace SdkDemo08
{
    public partial class GetCaptureForm : Form
    {
        int thres;//阈值
        private Bitmap bmpInit;//初始图像
        private Bitmap bmpDst;//处理后的图像
        private Image<Bgr, byte> img;//emgucv能处理的图像格式
        public GetCaptureForm(Bitmap bmp)
        {
            InitializeComponent();
            bmpInit = bmp;
            
            pictureBox2.Image = bmpInit;

            img = new Image<Bgr, byte>(bmpInit);
            
        }

        /// <summary>
        /// 阈值化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnThres_Click(object sender, EventArgs e)
        {
            Mat imgMat = img.Clone().Mat;

            //转成灰度图
            Mat imgGray = new Mat();
            CvInvoke.CvtColor(imgMat, imgGray, ColorConversion.Bgr2Gray);
            //阈值化处理
            Mat imgBin = new Mat();
            thres = Convert.ToInt32(thres_value.Text);
            CvInvoke.Threshold(imgGray, imgBin, thres, 250, ThresholdType.Binary);

            Mat imgHist = new Mat();
            CvInvoke.EqualizeHist(imgBin, imgHist);//直方图均衡化

            Mat imgDst = new Mat();
            CvInvoke.CvtColor(imgHist, imgDst, ColorConversion.Gray2Bgr);
            
            bmpDst = imgDst.ToImage<Bgr, byte>().ToBitmap();
            pictureBox2.Image = bmpDst;
        }

        

        

        private void btnConvert_Click(object sender, EventArgs e)
        {
            Mat imgMat = img.Clone().Mat;

            //转成灰度图
            Mat imgGray = new Mat();
            CvInvoke.CvtColor(imgMat, imgGray, ColorConversion.Bgr2Gray);
            //阈值化处理
            Mat imgBin = new Mat();
            thres = Convert.ToInt32(thres_value.Text);
            CvInvoke.Threshold(imgGray, imgBin, thres, 250, ThresholdType.BinaryInv);

            Mat imgHist = new Mat();
            CvInvoke.EqualizeHist(imgBin, imgHist);//直方图均衡化
            //转换成BGR图
            Mat imgBgr = new Mat();
            CvInvoke.CvtColor(imgHist, imgBgr, ColorConversion.Gray2Bgr);

            //imgBgr.SetTo(new Bgr(255, 0, 0).MCvScalar);
            
            bmpDst = imgBgr.ToImage<Bgr, byte>().ToBitmap();
            //逐像素点访问，将白色转成蓝色
            for(int i = 0; i < bmpDst.Width; i++)
            {
                for (int j = 0; j < bmpDst.Height; j++)
                {
                    Color c = bmpDst.GetPixel(i, j);
                    //Console.WriteLine(c);
                    if(c.B > 200 && c.G>200 && c.R>200)
                    {
                        bmpDst.SetPixel(i, j, Color.Blue);
                    }
                }
            }
            pictureBox2.Image = bmpDst;
            MessageBox.Show("转换成功");
        }

        /// <summary>
        /// 显示直方图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHist_Click(object sender, EventArgs e)
        {
            Grey_ScaleMapForm gs = new Grey_ScaleMapForm((Bitmap)pictureBox2.Image);
            gs.ShowDialog();
        }

        private void save()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Jpg|*.jpg|Bmp|*.bmp|Gif|*.gif|Png|*.png|Wmf|*.wmf";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "-";//设置默认文件名
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmpDst.Save(saveFileDialog1.FileName);
                //bmpDst.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);// image为要保存的图片
                MessageBox.Show(this, "图片保存成功！", "good");
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //saveFileDialog1.Filter = "Jpg|*.jpg|Bmp|*.bmp|Gif|*.gif|Png|*.png|Wmf|*.wmf";
            //saveFileDialog1.RestoreDirectory = true;
            //saveFileDialog1.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "-";//设置默认文件名
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    bmpDst.Save(saveFileDialog1.FileName);
            //    //bmpDst.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);// image为要保存的图片
            //    MessageBox.Show(this, "图片保存成功！", "good");
            //}

            //new Thread(new ThreadStart(save)).Start();

            //不用SaveFileDialog
            if(bmpDst == null)
            {
                MessageBox.Show("请先处理，再保存");

            }
            else
            {
                string savePath = "C:\\Users\\tang\\Desktop\\image\\" + DateTime.Now.ToString("yyyyMMddHHmmss")+"convert" + ".png";
                bmpDst.Save(savePath);
                MessageBox.Show("保存成功");

            }
        }
    }
}
