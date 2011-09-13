using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LumiSoft.Media.Wave;
using WaveStream;


namespace ManipulateMicrophone
{
    class CManipulateMicrophone:IManipulateMicrophone
    {
        private bool enabled = false;
        private WaveIn m_SoundInput;
        private WaveOut m_SoundOutput;
        private double m_SoundEnergy = 0;
        private byte[] m_SoundBuffer = new byte[400];
        private ProgressBar pb;
        private bool saveflag = false;
        private bool saved = true;
        private bool echo = false;
        private WaveStreamWriter wavwrite = null;

        public CManipulateMicrophone(ProgressBar pb)
        {
            this.pb = pb;
            m_SoundInput = new WaveIn(WaveIn.Devices[0], 8000, 8, 1, 400);
            m_SoundInput.BufferFull += new BufferFullHandler(SoundBufferFull);
            m_SoundOutput = new WaveOut(WaveOut.Devices[0], 8000, 8, 1);
        }

        public void Start()
        {
            m_SoundInput.Start();
            enabled = true;
        }

        public void Stop()
        {
            m_SoundInput.Stop();
            enabled = false;
        }

        private void SoundBufferFull(byte[] buffer)
        {
            if (echo==true)
            {
                m_SoundOutput.Play(buffer, 0, buffer.Length);
            }            
            double s = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                m_SoundBuffer[i] = buffer[i];
                s += Math.Abs(((float)buffer[i] - 128.0) / 255);
            }
            m_SoundEnergy = s;
            pb.Value = (int)(s / 400 * 100);

            if (saveflag==true)
            {
                if (saved==true)
                {
                    saved = false;
                    DateTime DT = DateTime.Now;
                    string dir = DT.ToString("yyyy-MM-dd-H");
                    dir = Application.StartupPath + @"\sound\" + dir + @"\";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fname = dir + DT.ToString("mm-ss-ff") + ".wav";
                    wavwrite = new WaveStreamWriter(fname, 8000, 1, 8);                    
                }
                wavwrite.Write(buffer, 400);
            }
            else
            {
                if (saved==false)
                {
                    wavwrite.Dispose();
                    saved = true;
                }      
            }
        }

        public double getSoundEnergy()
        {
            return m_SoundEnergy;
        }

        public void setSaveFlag(bool flag)
        {
            saveflag = flag;
        }

        public bool getSaved()
        {
            return saved;
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
        }

        public void setOpenEcho(bool flag)
        {
            echo = flag;
        }
    }
}
