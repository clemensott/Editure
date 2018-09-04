using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    class EncoderManagerJpg : IEncoderManager
    {
        public bool IsAuto { get { return false; } }

        public bool IsBmp { get { return false; } }

        public bool IsGif { get { return false; } }

        public bool IsJpg { get { return true; } }

        public bool IsPng { get { return false; } }

        public bool IsTiff { get { return false; } }

        public string Extention { get { return ".jpg"; } }

        public BitmapEncoder Encoder { get { return new JpegBitmapEncoder(); } }
    }
}
