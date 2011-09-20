using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ManipulateWebcam
{
    public class CManipulateWebcam : IManipulateWebcam
    {
        private int videodevice = 0; // zero based index of video capture device to use
        private int videowidth = 640; // Depends on video device caps
        private int videoheight = 480; // Depends on video device caps
        private short videobitsperpixel = 24; // BitsPerPixel values determined by device
        private Control hControl;
        private CaptureImage vedio;
        private IntPtr m_ip = IntPtr.Zero;


        /// <summary>
        /// private constructor function
        /// </summary>
        private CManipulateWebcam()
        {

        }

        /// <summary>
        /// constructor function
        /// </summary>
        /// <param name="iDeviceNum">video device number</param>
        /// <param name="hControl">name of the video container</param>
        public CManipulateWebcam(int iDeviceNum, Control hControl)
        {
            this.videodevice = iDeviceNum;
            this.videowidth = 640;
            this.videoheight = 480;
            this.videobitsperpixel = 24;
            this.hControl = hControl;
            this.m_ip = IntPtr.Zero;
        }


        /// <summary>
        /// constructor fucntion
        /// </summary>
        /// <param name="iDeviceNum">device number</param>
        /// <param name="iWidth">width of the image</param>
        /// <param name="iHeight">height of the image</param>
        /// <param name="iBPP">bit per pixel</param>
        /// <param name="hControl">name of the video container</param>
        public CManipulateWebcam(int iDeviceNum, int iWidth, int iHeight, short iBPP, Control hControl)
        {
            this.videodevice = iDeviceNum;
            this.videowidth = iWidth;
            this.videoheight = iHeight;
            this.videobitsperpixel = iBPP;
            this.hControl = hControl;
            this.m_ip = IntPtr.Zero;
        }


        public void Start()
        {
            this.vedio = new CaptureImage(this.videodevice, this.videowidth, this.videoheight, this.videobitsperpixel, this.hControl);
        }


        public void Close()
        {
            vedio.Dispose();

            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }


        public void SaveImage(string path)
        {
            // Release any previous buffer
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }

            // capture image
            m_ip = vedio.Click();
            Bitmap curBitmap = new Bitmap(vedio.Width, vedio.Height, vedio.Stride, PixelFormat.Format24bppRgb, m_ip);

            curBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            curBitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);


            // If the image is upsidedown
            // b.RotateFlip(RotateFlipType.RotateNoneFlipY);
            // pictureBox1.Image = b;
        }


        public Bitmap GrabImage()
        {
            // Release any previous buffer
            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }

            // capture image
            m_ip = vedio.Click();
            Bitmap curBitmap = new Bitmap(vedio.Width, vedio.Height, vedio.Stride, PixelFormat.Format24bppRgb, m_ip);

            curBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return curBitmap;

            // If the image is upsidedown
            // b.RotateFlip(RotateFlipType.RotateNoneFlipY);
            // pictureBox1.Image = b;
        }

    }

}
