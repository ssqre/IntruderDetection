using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManipulateMicrophone
{
    interface IManipulateMicrophone
    {
        bool Enabled{get;}
        void Start();
        void Stop();
        double getSoundEnergy();
        void setSaveFlag(bool flag);
        bool getSaved();
        void setOpenEcho(bool flag);
    }
}
