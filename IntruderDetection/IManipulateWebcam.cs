using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ManipulateWebcam
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManipulateWebcam
    {
        void Start();
        void Close();
        void SaveImage(string path);
        Bitmap GrabImage();
    }
}
