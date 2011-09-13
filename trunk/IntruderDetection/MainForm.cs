using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using ManipulateWebcam;
using ManipulateMicrophone;

namespace IntruderDetection
{
    public partial class MainForm : Form
    {
        private IManipulateWebcam imw;
        private Bitmap pre_bitmap;
        private Bitmap cur_bitmap;

        private IManipulateMicrophone imm;

        private int videonum = 0;
        private bool videoflag = true;
        private int soundnum = 0;
        private bool soundflag = true;

        private Color dcolor = DefaultBackColor;
        SoundPlayer soundalarm = new SoundPlayer(Application.StartupPath + @"/alarm.wav");

        public MainForm()
        {
            InitializeComponent();

            imw = new CManipulateWebcam(0, pictureVideo);
            imw.Start();
            pre_bitmap = imw.GrabImage();
            timerVideo.Interval = 250;
            timerVideo.Start();

            imm = new CManipulateMicrophone(progressSound);
            imm.Start();
            timerSound.Interval = 250;
            timerSound.Start();

            timerVideoAlarm.Interval = 500;
            timerSoundAlarm.Interval = 500;
        }

        private void timerVideo_Tick(object sender, EventArgs e)
        {
            if (checkVideo.Checked == true)
            {
                cur_bitmap = imw.GrabImage();

                if (CompareImage(pre_bitmap, cur_bitmap) && timerVideoAlarm.Enabled == false)
                {
                    timerVideoAlarm.Start();
                    if (checkVideoAlarm.Checked==true)
                    {
                        soundalarm.Play();
                    }
                }
                if (timerVideoAlarm.Enabled == true)
                {
                    pre_bitmap = cur_bitmap;

                    DateTime DT = DateTime.Now;
                    string dir = DT.ToString("yyyy-MM-dd-H");
                    dir = Application.StartupPath + @"\image\" + dir + @"\";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fname = dir + DT.ToString("mm-ss-ff") + ".jpg";
                    cur_bitmap.Save(fname, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    pre_bitmap = cur_bitmap;
                }
            }
        }


        private bool CompareSound(double thres)
        {
            if (thres < 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private bool CompareImage(Bitmap a, Bitmap b)
        {
            int width = b.Width;
            int height = b.Height;
            BitmapData data_a = a.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData data_b = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* pa = (byte*)data_a.Scan0;
                byte* pb = (byte*)data_b.Scan0;
                int offset = data_b.Stride - width * 3;
                double Ra, Ga, Ba, Rb, Gb, Bb;
                double temp1 = 0, temp2 = 0;
                double max = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Ra = (double)pa[2];
                        Ga = (double)pa[1];
                        Ba = (double)pa[0];
                        Rb = (double)pb[2];
                        Gb = (double)pb[1];
                        Bb = (double)pb[0];

                        temp1 = (Math.Abs(Ra - Rb) + Math.Abs(Ga - Gb) + Math.Abs(Ba - Bb)) / 3;
                        temp2 += temp1;

                        if (temp1 > max)
                        {
                            max = temp1;
                        }
                        pa += 3;
                        pb += 3;

                    }
                    pa += offset;
                    pb += offset;
                }
                temp2 = temp2 / (height * width);
                a.UnlockBits(data_a);
                b.UnlockBits(data_b);

                if (max > 200 || temp2 > 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void timerVideoAlarm_Tick(object sender, EventArgs e)
        {
            if (videoflag == true)
            {
                panelVideoAlarm.BackColor = Color.Red;
                videoflag = false;
            }
            else
            {
                panelVideoAlarm.BackColor = dcolor;
                videoflag = true;
            }
            videonum++;

            if (videonum > 5)
            {
                videonum = 0;
                timerVideoAlarm.Stop();
            }
        }

        private void timerSound_Tick(object sender, EventArgs e)
        {
            if (checkSound.Checked == true)
            {
                if (imm.Enabled==false)
                {
                    imm.Start();
                }
                if (CompareSound(imm.getSoundEnergy()) && timerSoundAlarm.Enabled == false)
                {
                    timerSoundAlarm.Start();
                    if (checkSoundAlarm.Checked==true)
                    {
                        soundalarm.Play();
                    }                 
                }
                if (timerSoundAlarm.Enabled == true)
                {
                    imm.setSaveFlag(true);
                }
                else
                {
                    imm.setSaveFlag(false);
                }
            }
            else
            {
                if (imm.Enabled==true)
                {
                    imm.Stop();                    
                }                
            }
        }

        private void timerSoundAlarm_Tick(object sender, EventArgs e)
        {
            if (soundflag == true)
            {
                panelSoundAlarm.BackColor = Color.Red;
                soundflag = false;
            }
            else
            {
                panelSoundAlarm.BackColor = dcolor;
                soundflag = true;
            }
            soundnum++;

            if (soundnum > 5)
            {
                soundnum = 0;
                timerSoundAlarm.Stop();
            }
        }

        private void checkOpenEcho_CheckedChanged(object sender, EventArgs e)
        {
            imm.setOpenEcho(checkOpenEcho.Checked);
        }

    }
}
