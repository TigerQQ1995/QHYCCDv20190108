using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StructModel;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;



namespace SdkDemo08
{
    public partial class Form1 : Form
    {
        //判断相机是否已经链接,默认为没有连接
        //Whether the camera is connected.The default is no connection
        public int isConnect = 0;
        //handle
        public static IntPtr camhandle;
        //设置一些对象，存放相机的相关参数
        //Set some object, store the related parameters of the camera
        uint x = 0, h = 0, bpp, c, ret;
        double chipw, chiph, pixelw, pixelh;
        byte[] rawArray, rgbArray;
        System.Collections.Queue QHYQueue = new System.Collections.Queue(); 
        Bitmap bitmap;
        Rectangle rectangle;
        BitmapData bmpData;
        IntPtr ptr;
        int s;
        int index;
        Byte pixData;
        //文件夹路径用于保存图像数据
        string path = "C:\\Users\\tang\\Desktop\\image";
        uint length;
        private Thread t;
        //连续模式连接标志，1表示连接成功
        uint isLive = 0;
        //阈值化处理的阈值0~255
        int threshold = 0;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        
        /// <summary>
        /// 连续模式连接按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectLive_Click(object sender, EventArgs e)
        {
            //如果目前相机没有连接，执行以下方法连接相机
            //If the camera is not connected, perform the following method to connect camera
            if (isConnect == 0 && isLive == 0)
            {
                //初始化相机资源
                //InitQHYCCDResource
                ASCOM.QHYCCD.libqhyccd.InitQHYCCDResource();
                //获得相机链接数
                //Gain is connected the camera 
                Convert.ToInt32(ASCOM.QHYCCD.libqhyccd.ScanQHYCCD());
                //给相机设置一个ID
                //set the camera's id
                StringBuilder id = new StringBuilder(0);
                ASCOM.QHYCCD.libqhyccd.GetQHYCCDId(0, id);
                //根据ID打开相机
                //open the camera depend on ID
                camhandle = ASCOM.QHYCCD.libqhyccd.OpenQHYCCD(id);
                //根据ID赋给相机一个handle
                //According to a handle ID is assigned to the camera
                //设置相机模式为连续模式
                ASCOM.QHYCCD.libqhyccd.SetQHYCCDStreamMode(camhandle, 1);
                //初始化相机
                //Init camera
                ASCOM.QHYCCD.libqhyccd.InitQHYCCD(camhandle);
                //button3.Text = ret.ToString();
                //获取相机的碎片信息
                //Camera fragments of information
                ASCOM.QHYCCD.libqhyccd.GetQHYCCDChipInfo(camhandle, ref chipw, ref chiph, ref x, ref h, ref pixelw, ref pixelh, ref bpp);
                //设置相机的bin
                //set bin mode
                ASCOM.QHYCCD.libqhyccd.SetQHYCCDBinMode(camhandle, 1, 1);
                //设置相机分辨率
                //set resolution
                ASCOM.QHYCCD.libqhyccd.SetQHYCCDResolution(camhandle, 0, 0, x, h);
                //获取照片所占用的空间大小
                //To get photos occupied space size
                length = ASCOM.QHYCCD.libqhyccd.GetQHYCCDMemLength(camhandle);
                //将照片所占用的空间大小放入byte数组中
                //Put pictures occupied space in a byte array
                rawArray = new byte[length];

                                
                //将是否连接值改为1，表示已经连接
                //Connect whether value is changed to 1, says it has connections
                isConnect = 1;
                isLive = 1;

                

            }
            //弹出一个提示框，提示连接成功
            //Bring up a prompt box, suggesting the connection is successful
            if (isConnect == 1)
            {
                if (isLive == 0)
                //如果已经连接相机，再次点击，弹出提示框
                //If the camera is connected, click again, the pop-up prompts
                {
                    DialogResult dr = MessageBox.Show("single mode has connected camare, disconnect first and try again.");
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Live mode has connected!");
                }
            }
        }

        

        private void Connection_Click(object sender, EventArgs e)
        {
            //如果目前相机没有连接，执行以下方法连接相机
            //If the camera is not connected, perform the following method to connect camera
            if (isConnect == 0 && isLive == 0)
            {
                //初始化相机资源
                //InitQHYCCDResource
                ASCOM.QHYCCD.libqhyccd.InitQHYCCDResource();
                //获得相机链接数
                //Gain is connected the camera 
                Convert.ToInt32(ASCOM.QHYCCD.libqhyccd.ScanQHYCCD());
                //给相机设置一个ID
                //set the camera's id
                StringBuilder id = new StringBuilder(0);
                ASCOM.QHYCCD.libqhyccd.GetQHYCCDId(0, id);
                //根据ID打开相机
                //open the camera depend on ID
                camhandle = ASCOM.QHYCCD.libqhyccd.OpenQHYCCD(id);
                //根据ID赋给相机一个handle
                //According to a handle ID is assigned to the camera
                ASCOM.QHYCCD.libqhyccd.SetQHYCCDStreamMode(camhandle, 0);
                //初始化相机
                //Init camera
                ASCOM.QHYCCD.libqhyccd.InitQHYCCD(camhandle);
                //button3.Text = ret.ToString();
                //获取相机的碎片信息
                //Camera fragments of information
                ASCOM.QHYCCD.libqhyccd.GetQHYCCDChipInfo(camhandle, ref chipw, ref chiph, ref x, ref h, ref pixelw, ref pixelh, ref bpp);
                //设置相机的bin
                //set bin mode
                ASCOM.QHYCCD.libqhyccd.SetQHYCCDBinMode(camhandle, 1, 1);
                //设置相机分辨率
                //set resolution
                ASCOM.QHYCCD.libqhyccd.SetQHYCCDResolution(camhandle, 0, 0, x, h);
                //获取照片所占用的空间大小
                //To get photos occupied space size
                length = ASCOM.QHYCCD.libqhyccd.GetQHYCCDMemLength(camhandle);
                //将照片所占用的空间大小放入byte数组中
                //Put pictures occupied space in a byte array
                rawArray = new byte[length*3];
              
                                              
                //将是否连接值改为1，表示已经连接
                //Connect whether value is changed to 1, says it has connections
                isConnect = 1;
                isLive = 0;
                //连接成功后，根据Path创建文件夹                
                if (Directory.Exists(path) == false)
                {//判断目录是否存在
                    Directory.CreateDirectory(path);
                }
                
            }
            //弹出一个提示框，提示连接成功
            //Bring up a prompt box, suggesting the connection is successful
            if (isConnect == 1)
            {
                if(isLive == 1)
                {
                    //如果已经连接相机，再次点击，弹出提示框
                    //If the camera is connected, click again, the pop-up prompts
                    DialogResult dr = MessageBox.Show("live mode has connected camare, disconnect first and try again.");
                }
                else
                {
                    DialogResult dr = MessageBox.Show("single mode has connected!");
                }
            }
                
        }

        private void DisConnection_Click(object sender, EventArgs e)
        {
            //如果有相机连接，执行以下方法
            //If there is a camera connection, perform the following method
            if (isConnect == 1)
            {
                //关闭连续模式曝光
                //ASCOM.QHYCCD.libqhyccd.StopQHYCCDLive(camhandle);
                //关闭相机
                //close camera
                ASCOM.QHYCCD.libqhyccd.CloseQHYCCD(camhandle);
                //释放资源
                //release resource
                ret = ASCOM.QHYCCD.libqhyccd.ReleaseQHYCCDResource();
                //将连接状态的值改为0
                //The connection state to change the value of 0
                isConnect = 0;
                isLive = 0;
                if (ret == 0)
                {
                    //关闭成功，弹出提示框
                    //Close the success, pop-up prompts
                    DialogResult dr = MessageBox.Show("disconnect success");
                }
            }
            else
            {   //如果目前没有相机连接，弹出提示框
                //If there is no camera connection, pop-up prompts
                DialogResult dr = MessageBox.Show("no camera connection now");
            }
        }

        private void setting_Click(object sender, EventArgs e)
        {
            uint ret1, ret2;
            //从控件上取下对应设置的值
            //The value of the corresponding set below is taken from the controls
            double exposure_times = Convert.ToDouble(exposure.Text);
            double gain_num = Convert.ToDouble(gain.Text);
            double offfset_num = Convert.ToDouble(offset.Text);
            double usbTraffic_num = Convert.ToDouble(usbTraffic.Text);
            x = Convert.ToUInt32(width.Text);
            h = Convert.ToUInt32(height.Text);
            uint x_num = Convert.ToUInt32(pointX.Text);
            uint y_num = Convert.ToUInt32(pointY.Text);
            double bright_num = Convert.ToDouble(bright.Text);
            double contrast_num = Convert.ToDouble(contrast.Text);
            double wbr_num = Convert.ToDouble(WBR.Text);
            double wbg_num = Convert.ToDouble(WBG.Text);
            double wbb_num = Convert.ToDouble(WBB.Text);
            //设置分辨率
            ret1 = ASCOM.QHYCCD.libqhyccd.SetQHYCCDResolution(camhandle, x_num, y_num, x, h);

            //设置参数
            //set param
            ret2 = setCameraParam(camhandle, exposure_times, gain_num, offfset_num, usbTraffic_num, bright_num, contrast_num, wbr_num, wbg_num, wbb_num);
            if (ret1 == 0 && ret2 ==0)
            {
                //如果设置成功，弹出提示框
                //If set to succeed, pop-up prompts
                DialogResult dr = MessageBox.Show("set to succeed");
            }
            else
            {
                //If set to failure, pop-up prompts
                DialogResult dr = MessageBox.Show("set to failure");
            }
        }


        //设置相机参数，这里我只设置了曝光、gain、offset、usbTraffic,你可以根据你的实际情况增删参数
        //set camera param,I just set the exposure, gain, offset, usbTraffic, you can add or delete parameters according to your actual condition
        public static uint setCameraParam(IntPtr handle, double exposure_times, double gain_num, double offSet_num, double usbTraffic_num, double bright_num, double contrast_num, 
                                            double wbr_num, double wbg_num, double wbb_num)
        {
            //设置参数成功的返回值为0
            //Set parameters successful return value is zero
            uint ret = 0;
            if (ret == 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_EXPOSURE, exposure_times); //exposure
            }
            else
            {
                DialogResult dr = MessageBox.Show("failure set exposure");
            }
            if (ret == 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_GAIN, gain_num); //Gain
            }
            else
            {
                DialogResult dr = MessageBox.Show("failure set gain");
            }
            if (ret == 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_OFFSET, offSet_num);//offset
            }
            else
            {
                DialogResult dr = MessageBox.Show("failure set offset");
            }
            if (ret == 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_USBTRAFFIC, usbTraffic_num);//usbTraffic

            }
            else
            {
                DialogResult dr = MessageBox.Show("failure set usbTraffic");
            }
            if(ret == 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_BRIGHTNESS, bright_num);
            }
            else
            {
                DialogResult dr = MessageBox.Show("failure set bright");
            }
            if (ret == 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_CONTRAST, contrast_num);
            }
            else
            {
                DialogResult dr = MessageBox.Show("failure set contrast");
            }
            //if (ret == 0)
            //{
            //    ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_WBR, wbr_num);
            //    ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_WBG, wbg_num);
            //    ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_WBB, wbb_num);
            //}
            //else
            //{
            //    DialogResult dr = MessageBox.Show("failure set white balance");
            //}
            
            return 0;


        }

        private void ShowImage()
        {
            //计算帧率
            uint ret = 1;
            int frames = 0;
            int fps = 0;
            int t_interval = 0;
            DateTime t_start, t_end;
            TimeSpan ts;
            t_start = DateTime.Now;
            int num = 0;

            //Image<Bgr, byte> img;
            //Mat imgMat;
            
            while (isLive == 1)
            {
                bitmap = new Bitmap((int)x, (int)h);
                
                while (ret != 0)
                {
                    ret = ASCOM.QHYCCD.libqhyccd.C_GetQHYCCDLiveFrame(camhandle, ref x, ref h, ref bpp, ref c, rawArray);

                }
                //Console.WriteLine("x = {0} h = {1} bpp = {2} c = {3}", x,h,bpp,c);

                //显示图片 内存法  
                if (ret == 0)
                {
                    ret = 1;


                    rectangle = new Rectangle(0, 0, (int)x, (int)h);
                    bmpData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    //Console.WriteLine("rectangle = {0} ReadWrite = {1} PixelFormat = {2}",rectangle,ImageLockMode.ReadWrite,bitmap.PixelFormat);
                    ptr = bmpData.Scan0;

                    s = 0;
                    index = 0;
                    pixData = 0;
                    rgbArray = new Byte[x * h * 4];
                    for (int i = 0; i < h; i++)
                    {
                        for (int y = 0; y < x; y++)
                        {
                            if (bpp == 16 && c == 3)
                            {
                                rgbArray[s] = rawArray[index + 1];//b
                                //Console.WriteLine(rgbArray[s]);
                                rgbArray[s + 1] = rawArray[index + 3];//g
                                rgbArray[s + 2] = rawArray[index + 5];//r
                                rgbArray[s + 3] = 255;//alpha

                                s += 4;
                                index += 6;
                            }
                            else if (bpp == 16 && c == 1)
                            {
                                rgbArray[s] = rawArray[index + 1];
                                rgbArray[s + 1] = rawArray[index + 1];
                                rgbArray[s + 2] = rawArray[index + 1];
                                rgbArray[s + 3] = 255;

                                s += 4;
                                index += 2;
                            }
                            else if (bpp == 8 && c == 3)
                            {
                                rgbArray[s] = rawArray[index];
                                rgbArray[s + 1] = rawArray[index + 1];
                                rgbArray[s + 2] = rawArray[index + 2];
                                rgbArray[s + 3] = 255;

                                s += 4;
                                index += 3;
                            }
                            else if (bpp == 8 && c == 1)
                            {
                                rgbArray[s] = rawArray[index];
                                rgbArray[s + 1] = rawArray[index];
                                rgbArray[s + 2] = rawArray[index];
                                rgbArray[s + 3] = 255;

                                s += 4;
                                index += 1;
                            }
                        }
                    }

                    Marshal.Copy(rgbArray, 0, ptr, (int)(x * h * 4));

                    bitmap.UnlockBits(bmpData);
                    
                    //img = new Image<Bgr, byte>(bitmap);
                    //imgMat = img.Mat;
                    //CvInvoke.Imshow("imgMat", imgMat);
                    //pictureBox1.Image = imgMat.ToImage<Bgr, byte>().ToBitmap();
                    pictureBox1.Image = bitmap;

                }

                frames++;




                t_end = DateTime.Now;
                ts = t_end.Subtract(t_start);
                t_interval += ts.Milliseconds;
                if (t_interval >= 2 * 1000)
                {
                    num++;
                    fps = 1000 * frames / t_interval;
                    fps_text.Text = "" + fps;
                    //Console.WriteLine(frames);
                    //Console.WriteLine("fps:{0}", fps);
                    //Console.WriteLine();
                    
                    frames = 0;
                    t_interval = 0;
                }

                if (num == 10)
                {
                    //time.Text = "" + DateTime.Now;
                    //Console.WriteLine(DateTime.Now);
                    num = 0;

                }


                time.Text = "" + DateTime.Now;
                t_start = DateTime.Now;

                //ret = 1;
            }


            //Application.DoEvents();
        }
        private void Live_Click(object sender, EventArgs e)
        {
            //设置相机的图片位数
            //set camare bits mode
            ASCOM.QHYCCD.libqhyccd.SetQHYCCDBitsMode(camhandle, 16);
            uint ret = 1;
            ASCOM.QHYCCD.libqhyccd.InitQHYCCD(camhandle);
            //开启曝光
            //exposure            
            ASCOM.QHYCCD.libqhyccd.BeginQHYCCDLive(camhandle);
            //debayer
            ret = ASCOM.QHYCCD.libqhyccd.SetQHYCCDDebayerOnOff(camhandle, true);
            rawArray = new byte[length * 2];

            //ASCOM.QHYCCD.libqhyccd.SetQHYCCDResolution(camhandle, 0, 0, 800, 600);

            //将循环获取图像放在一个线程中
            t = new Thread(new ThreadStart(ShowImage));
            t.Start();
                                   

        }

        private void btnHistogram_Click(object sender, EventArgs e)
        {
            Grey_ScaleMapForm gs = new Grey_ScaleMapForm((Bitmap)pictureBox1.Image);
            gs.ShowDialog();
        }

        private void capture()
        {
            string filePath = "C:\\Users\\tang\\Desktop\\image\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";//给照片文件命名
            pictureBox1.Image.Save(filePath);
            //Bitmap bmp = (Bitmap)pictureBox1.Image.Clone();
            Bitmap bmp = new Bitmap(filePath);

            GetCaptureForm gc = new GetCaptureForm(bmp);
            gc.ShowDialog();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            //将图像处理工作新开一个线程来完成
            new Thread(new ThreadStart(capture)).Start();

            //string filePath = "C:\\Users\\tang\\Desktop\\yang\\" + "img_16_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";//给照片文件命名
            //pictureBox1.Image.Save(filePath);
            ////Bitmap bmp = (Bitmap)pictureBox1.Image.Clone();
            //Bitmap bmp = new Bitmap(filePath);

            //GetCaptureForm gc = new GetCaptureForm(bmp);
            //gc.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void single_Click(object sender, EventArgs e)
        {   

            //设置相机的图片位数
            //set camare bits mode
            ASCOM.QHYCCD.libqhyccd.SetQHYCCDBitsMode(camhandle, 16);
            uint ret = 1;
            ASCOM.QHYCCD.libqhyccd.InitQHYCCD(camhandle);
            //开启曝光
            //exposure
            ASCOM.QHYCCD.libqhyccd.ExpQHYCCDSingleFrame(camhandle);
            //彩色模式
            //debayer
            ASCOM.QHYCCD.libqhyccd.SetQHYCCDDebayerOnOff(camhandle, true);
            rawArray = new byte[length * 3 ];
            //获取照片的信息
            //Obtain information on the photos
            while (ret != 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.C_GetQHYCCDSingleFrame(camhandle, ref x, ref h, ref bpp, ref c, rawArray);
            }
            //显示图片 内存法  
            if (ret == 0)
            {
                Image<Bgr, byte> img;
                Mat imgMat;
                bitmap = new Bitmap((int)x, (int)h);
                rectangle = new Rectangle(0, 0, (int)x, (int)h);
                bmpData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                ptr = bmpData.Scan0;

                s = 0;
                index = 0;
                pixData = 0;
                rgbArray = new Byte[x * h * 4];
                for (int i = 0; i < h; i++)
                {
                    for (int y = 0; y < x; y++)
                    {
                        rgbArray[s ] = rawArray[index + 1];
                        rgbArray[s + 1] = rawArray[index + 3];
                        rgbArray[s + 2] = rawArray[index + 5];
                        rgbArray[s + 3] = 255;

                        s += 4;
                        index += 6;
                    }
                }

                Marshal.Copy(rgbArray, 0, ptr, (int)(x * h * 4));

                bitmap.UnlockBits(bmpData);

                img = new Image<Bgr, byte>(bitmap);
                CvInvoke.NamedWindow("img", NamedWindowType.Normal);
                CvInvoke.Imshow("img", img);
                
                imgMat = img.Mat;
                //CvInvoke.NamedWindow("imgMat", NamedWindowType.Normal);
                //CvInvoke.Imshow("imgMat", imgMat);
                
                Mat imgGray = new Mat();
                CvInvoke.CvtColor(imgMat, imgGray, ColorConversion.Bgr2Gray);
                //CvInvoke.NamedWindow("imgGray", NamedWindowType.Normal);
                //CvInvoke.Imshow("imgGray", imgGray);

                Mat imgBin = new Mat();
                threshold = Convert.ToInt32(Threshold_value.Text);
                CvInvoke.Threshold(imgGray, imgBin, threshold, 250, ThresholdType.Binary);
                CvInvoke.NamedWindow("imgBin", NamedWindowType.Normal);
                CvInvoke.Imshow("imgBin", imgBin);

                Mat imgHist = new Mat();
                CvInvoke.EqualizeHist(imgBin, imgHist);//直方图均衡化
                CvInvoke.NamedWindow("imgHist", NamedWindowType.Normal);
                CvInvoke.Imshow("imgHist", imgHist);
                

                Mat imgChanged = new Mat();
                CvInvoke.CvtColor(imgHist, imgChanged, ColorConversion.Gray2Bgr);
                CvInvoke.NamedWindow("imgChanged", NamedWindowType.Normal);
                CvInvoke.Imshow("imgChanged", imgChanged);

                //查看像素
                //Image<Bgr, byte> imgImg = imgChanged.ToImage<Bgr, byte>();
                //for (int i = 1500; i < 1600; i++)
                //{
                //    for (int j = 1000; j < 1100; j++)
                //    {
                //        Console.Write("imgImg.Data[{0}, {1}, 0]: {2}  ", i, j, imgImg.Data[i, j, 0]);
                //        Console.Write("imgImg.Data[{0}, {1}, 1]: {2}  ", i, j, imgImg.Data[i, j, 1]);
                //        Console.Write("imgImg.Data[{0}, {1}, 2]: {2}  ", i, j, imgImg.Data[i, j, 2]);
                //        Console.WriteLine();
                //    }
                //    Console.WriteLine();
                //}

                //开运算
                //Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
                //CvInvoke.MorphologyEx(imgMat, imgChanged, MorphOp.Close, kernel, new Point(-1, -1), 2, BorderType.Default, new MCvScalar());


                //pictureBox1.Image = imgChanged.ToImage<Bgr, byte>().ToBitmap();
                pictureBox1.Image = bitmap;
                //CvInvoke.Imwrite(path + "\\" + "mat.png", imgMat);

            }


            string fileName = "img_16_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";//给照片文件命名
            //bitmap.Save(path + "\\" + fileName);//保存照片

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //设置相机的图片位数
            //set camare bits mode
            ASCOM.QHYCCD.libqhyccd.SetQHYCCDBitsMode(camhandle, 8);
            uint ret = 1;
            ASCOM.QHYCCD.libqhyccd.InitQHYCCD(camhandle);
            //开启曝光
            //exposure
            ASCOM.QHYCCD.libqhyccd.ExpQHYCCDSingleFrame(camhandle);

            ASCOM.QHYCCD.libqhyccd.SetQHYCCDDebayerOnOff(camhandle, true);

            rawArray = new byte[length * 3];

            //获取照片的信息
            //Obtain information on the photos
            while (ret != 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.C_GetQHYCCDSingleFrame(camhandle, ref x, ref h, ref bpp, ref c, rawArray);
            }

            //显示图片 内存法  
            if (ret == 0)
            {
                bitmap = new Bitmap((int)x, (int)h);
                rectangle = new Rectangle(0, 0, (int)x, (int)h);
                bmpData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                ptr = bmpData.Scan0;

                s = 0;
                index = 0;
                pixData = 0;
                rgbArray = new Byte[x * h * 4];
                for (int i = 0; i < h; i++)
                {
                    for (int y = 0; y < x; y++)
                    {
                        //blue
                        rgbArray[s ] = rawArray[index+1];
                        //Green
                        rgbArray[s + 1] = rawArray[index + 3];
                        //red
                        rgbArray[s + 2] = rawArray[index + 5];
                        rgbArray[s + 3] = 255;

                        s += 4;
                        index += 6;
                    }
                }

                Marshal.Copy(rgbArray, 0, ptr, (int)(x * h * 4));

                bitmap.UnlockBits(bmpData);

                pictureBox1.Image = bitmap;

                
            }
            string fileName = "img_8_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";//给照片文件命名
            bitmap.Save(path + "\\" + fileName);//保存照片
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //设置相机的图片位数
            //set camare bits mode
            ASCOM.QHYCCD.libqhyccd.SetQHYCCDBitsMode(camhandle, 8);
            uint ret = 1;
            ASCOM.QHYCCD.libqhyccd.InitQHYCCD(camhandle);
            //开启曝光
            //exposure
            ASCOM.QHYCCD.libqhyccd.ExpQHYCCDSingleFrame(camhandle);
            //获取照片的信息
            //Obtain information on the photos
            while (ret != 0)
            {
                ret = ASCOM.QHYCCD.libqhyccd.C_GetQHYCCDSingleFrame(camhandle, ref x, ref h, ref bpp, ref c, rawArray);
            }

            //显示图片 内存法  
            if (ret == 0)
            {
                bitmap = new Bitmap((int)x, (int)h);
                rectangle = new Rectangle(0, 0, (int)x, (int)h);
                bmpData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                ptr = bmpData.Scan0;

                s = 0;
                index = 0;
                pixData = 0;
                rgbArray = new Byte[x * h * 4];
                for (int i = 0; i < h; i++)
                {
                    for (int y = 0; y < x; y++)
                    {
                        pixData = rawArray[index + 1];
                        rgbArray[s] = pixData;
                        rgbArray[s + 1] = pixData;
                        rgbArray[s + 2] = pixData;
                        rgbArray[s + 3] = 125;

                        s += 4;
                        index += 2;
                    }
                }

                Marshal.Copy(rgbArray, 0, ptr, (int)(x * h * 4));

                bitmap.UnlockBits(bmpData);

                pictureBox1.Image = bitmap;


            }
            string fileName = "img_gray_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";//给照片文件命名
            bitmap.Save(path + "\\" + fileName);//保存照片
        }

        private void cfw1_Click(object sender, EventArgs e)
        {
            ASCOM.QHYCCD.libqhyccd.SendOrder2QHYCCDCFW(camhandle, "0", 1);
            uint t;
            for (int i = 0; i < 50; i++)
            {
                t = (uint)ASCOM.QHYCCD.libqhyccd.GetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_CFWPORT);
                Console.WriteLine("i = {0} t = {1}", i, t);
                Application.DoEvents();
            }
        }

        private void cfw2_Click(object sender, EventArgs e)
        {
            ASCOM.QHYCCD.libqhyccd.SendOrder2QHYCCDCFW(camhandle, "1", 1);
            uint t;
            for (int i = 0; i < 50; i++)
            {
                t = (uint)ASCOM.QHYCCD.libqhyccd.GetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_CFWPORT);
                Console.WriteLine("i = {0} t = {1}", i, t);
                Application.DoEvents();
            }
        }

        private void cfw3_Click(object sender, EventArgs e)
        {
            ASCOM.QHYCCD.libqhyccd.SendOrder2QHYCCDCFW(camhandle, "2", 1);
            uint t;
            for (int i = 0; i < 50; i++)
            {
                t = (uint)ASCOM.QHYCCD.libqhyccd.GetQHYCCDParam(camhandle, CONTROL_ID.CONTROL_CFWPORT);
                Console.WriteLine("i = {0} t = {1}", i, t);
                Application.DoEvents();
            }
        }

        private void cfw4_Click(object sender, EventArgs e)
        {
            ASCOM.QHYCCD.libqhyccd.SendOrder2QHYCCDCFW(camhandle, "3", 1);
        }

        private void cfw5_Click(object sender, EventArgs e)
        {
            ASCOM.QHYCCD.libqhyccd.SendOrder2QHYCCDCFW(camhandle, "4", 1);
        }
    }
}
    
    namespace StructModel
    {
        public enum CONTROL_ID
        {
            CONTROL_BRIGHTNESS = 0, //!< image brightness
            CONTROL_CONTRAST,       //!< image contrast 
            CONTROL_WBR,            //!< red of white balance 
            CONTROL_WBB,            //!< blue of white balance
            CONTROL_WBG,            //!< the green of white balance 
            CONTROL_GAMMA,          //!< screen gamma 
            CONTROL_GAIN,           //!< camera gain 
            CONTROL_OFFSET,         //!< camera offset 
            CONTROL_EXPOSURE,       //!< expose time (us)
            CONTROL_SPEED,          //!< transfer speed 
            CONTROL_TRANSFERBIT,    //!< image depth bits 
            CONTROL_CHANNELS,       //!< image channels 
            CONTROL_USBTRAFFIC,     //!< hblank 
            CONTROL_ROWNOISERE,     //!< row denoise 
            CONTROL_CURTEMP,        //!< current cmos or ccd temprature 
            CONTROL_CURPWM,         //!< current cool pwm 
            CONTROL_MANULPWM,       //!< set the cool pwm 
            CONTROL_CFWPORT,        //!< control camera color filter wheel port 
            CONTROL_COOLER,         //!< check if camera has cooler
            CONTROL_ST4PORT,        //!< check if camera has st4port
            CAM_COLOR,
            CAM_BIN1X1MODE,         //!< check if camera has bin1x1 mode 
            CAM_BIN2X2MODE,         //!< check if camera has bin2x2 mode 
            CAM_BIN3X3MODE,         //!< check if camera has bin3x3 mode 
            CAM_BIN4X4MODE,         //!< check if camera has bin4x4 mode 
            CAM_MECHANICALSHUTTER,                   //!< mechanical shutter  
            CAM_TRIGER_INTERFACE,                    //!< triger  
            CAM_TECOVERPROTECT_INTERFACE,            //!< tec overprotect
            CAM_SINGNALCLAMP_INTERFACE,              //!< singnal clamp 
            CAM_FINETONE_INTERFACE,                  //!< fine tone 
            CAM_SHUTTERMOTORHEATING_INTERFACE,       //!< shutter motor heating 
            CAM_CALIBRATEFPN_INTERFACE,              //!< calibrated frame 
            CAM_CHIPTEMPERATURESENSOR_INTERFACE,     //!< chip temperaure sensor
            CAM_USBREADOUTSLOWEST_INTERFACE,         //!< usb readout slowest 

            CAM_8BITS,                               //!< 8bit depth 
            CAM_16BITS,                              //!< 16bit depth
            CAM_GPS,                                 //!< check if camera has gps 

            CAM_IGNOREOVERSCAN_INTERFACE,            //!< ignore overscan area 

            QHYCCD_3A_AUTOBALANCE,
            QHYCCD_3A_AUTOEXPOSURE,
            QHYCCD_3A_AUTOFOCUS,
            CONTROL_AMPV,                            //!< ccd or cmos ampv
            CONTROL_VCAM,                            //!< Virtual Camera on off 
            CAM_VIEW_MODE,

            CONTROL_CFWSLOTSNUM,         //!< check CFW slots number
            IS_EXPOSING_DONE,
            ScreenStretchB,
            ScreenStretchW,
            CONTROL_DDR,
            CAM_LIGHT_PERFORMANCE_MODE,

            CAM_QHY5II_GUIDE_MODE,
            DDR_BUFFER_CAPACITY,
            DDR_BUFFER_READ_THRESHOLD
        };

        public enum BAYER_ID
        {
            BAYER_GB = 1,
            BAYER_GR,
            BAYER_BG,
            BAYER_RG
        };
    }

    namespace ASCOM.QHYCCD
    {

        class libqhyccd
        {
            [DllImport("qhyccd.dll", EntryPoint = "InitQHYCCDResource",
                CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 InitQHYCCDResource();

            [DllImport("qhyccd.dll", EntryPoint = "ReleaseQHYCCDResource",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 ReleaseQHYCCDResource();

            [DllImport("qhyccd.dll", EntryPoint = "ScanQHYCCD",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 ScanQHYCCD();

            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDId",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDId(int index, StringBuilder id);

            [DllImport("qhyccd.dll", EntryPoint = "OpenQHYCCD",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern IntPtr OpenQHYCCD(StringBuilder id);

            [DllImport("qhyccd.dll", EntryPoint = "InitQHYCCD",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 InitQHYCCD(IntPtr handle);

            [DllImport("qhyccd.dll", EntryPoint = "CloseQHYCCD",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 CloseQHYCCD(IntPtr handle);

            [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDBinMode",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 SetQHYCCDBinMode(IntPtr handle, UInt32 wbin, UInt32 hbin);

            [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDParam",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 SetQHYCCDParam(IntPtr handle, CONTROL_ID controlid, double value);

            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDMemLength",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDMemLength(IntPtr handle);

            [DllImport("qhyccd.dll", EntryPoint = "ExpQHYCCDSingleFrame",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 ExpQHYCCDSingleFrame(IntPtr handle);

            [DllImport("qhyccd.dll", EntryPoint = "CancelQHYCCDExposing",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 CancelQHYCCDExposing(IntPtr handle);

            [DllImport("qhyccd.dll", EntryPoint = "CancelQHYCCDExposingAndReadout",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 CancelQHYCCDExposingAndReadout(IntPtr handle);

            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDSingleFrame",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDSingleFrame(IntPtr handle, ref UInt32 w, ref UInt32 h, ref UInt32 bpp, ref UInt32 channels, byte* rawArray);
            public unsafe static UInt32 C_GetQHYCCDSingleFrame(IntPtr handle, ref UInt32 w, ref UInt32 h, ref UInt32 bpp, ref UInt32 channels, byte[] rawArray)
            {
                UInt32 ret;
                fixed (byte* prawArray = rawArray)
                    ret = GetQHYCCDSingleFrame(handle, ref w, ref h, ref bpp, ref channels, prawArray);
                return ret;
            }
            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDChipInfo",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDChipInfo(IntPtr handle, ref double chipw, ref double chiph, ref UInt32 imagew, ref UInt32 imageh, ref double pixelw, ref double pixelh, ref UInt32 bpp);

            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDOverScanArea",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDOverScanArea(IntPtr handle, ref UInt32 startx, ref UInt32 starty, ref UInt32 sizex, ref UInt32 sizey);

            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDEffectiveArea",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDEffectiveArea(IntPtr handle, ref UInt32 startx, ref UInt32 starty, ref UInt32 sizex, ref UInt32 sizey);

            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDFWVersion",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDFWVersion(IntPtr handle, byte* verBuf);

            public unsafe static UInt32 C_GetQHYCCDFWVersion(IntPtr handle, byte[] verBuf)
            {
                fixed (byte* pverBuf = verBuf)
                    return GetQHYCCDFWVersion(handle, pverBuf);
            }

            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDParam",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern double GetQHYCCDParam(IntPtr handle, CONTROL_ID controlid);


            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDParamMinMaxStep",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDParamMinMaxStep(IntPtr handle, CONTROL_ID controlid, ref double min, ref double max, ref double step);

            [DllImport("qhyccd.dll", EntryPoint = "ControlQHYCCDGuide",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 ControlQHYCCDGuide(IntPtr handle, byte Direction, UInt16 PulseTime);

            [DllImport("qhyccd.dll", EntryPoint = "ControlQHYCCDTemp",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 ControlQHYCCDTemp(IntPtr handle, double targettemp);

            [DllImport("qhyccd.dll", EntryPoint = "SendOrder2QHYCCDCFW",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 SendOrder2QHYCCDCFW(IntPtr handle, String order, int length);

            [DllImport("qhyccd.dll", EntryPoint = "IsQHYCCDControlAvailable",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 IsQHYCCDControlAvailable(IntPtr handle, CONTROL_ID controlid);

            [DllImport("qhyccd.dll", EntryPoint = "ControlQHYCCDShutter",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 ControlQHYCCDShutter(IntPtr handle, byte targettemp);

            [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDResolution",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 SetQHYCCDResolution(IntPtr handle, UInt32 startx, UInt32 starty, UInt32 sizex, UInt32 sizey);

            [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDStreamMode",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 SetQHYCCDStreamMode(IntPtr handle, UInt32 mode);

            //EXPORTFUNC uint32_t STDCALL GetQHYCCDCFWStatus(qhyccd_handle *handle,char *status)
            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDCFWStatus",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 GetQHYCCDCFWStatus(IntPtr handle, StringBuilder cfwStatus);
            [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDBitsMode",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 SetQHYCCDBitsMode(IntPtr handle, UInt32 bits);

            [DllImport("qhyccd.dll", EntryPoint = "BeginQHYCCDLive",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 BeginQHYCCDLive(IntPtr handle);
            [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDLiveFrame",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]

            public unsafe static extern UInt32 GetQHYCCDLiveFrame(IntPtr handle, ref UInt32 w, ref UInt32 h, ref UInt32 bpp, ref UInt32 channels, byte* rawArray);
            public unsafe static UInt32 C_GetQHYCCDLiveFrame(IntPtr handle, ref UInt32 w, ref UInt32 h, ref UInt32 bpp, ref UInt32 channels, byte[] rawArray)
            {
                UInt32 ret;
                fixed (byte* prawArray = rawArray)
                    ret = GetQHYCCDLiveFrame(handle,ref w,ref h,ref bpp,ref channels,prawArray);
                return ret;
            }
            //public unsafe static extern UInt32 GetQHYCCDLiveFrame(IntPtr handle, UInt32 w, UInt32 h, UInt32 bpp, UInt32 channels, byte* imgdata);
            //public unsafe static UInt32 C_GetQHYCCDLiveFrame(IntPtr handle, UInt32 w, UInt32 h, UInt32 bpp, UInt32 channels, byte[] imgdata)
            //{
            //    UInt32 ret;
            //    fixed (byte* prawArray = imgdata)
            //        ret = GetQHYCCDLiveFrame(handle, ref w, ref h, ref bpp, ref channels, prawArray);
            //    return ret;
            //}

            [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDDebayerOnOff",
             CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 SetQHYCCDDebayerOnOff(IntPtr handle,bool onoff);

            [DllImport("qhyccd.dll", EntryPoint = "StopQHYCCDLive",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            public unsafe static extern UInt32 StopQHYCCDLive(IntPtr handle);

        }
    }


