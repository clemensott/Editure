using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    class EncoderManagerBmp : IEncoderManager
    {
        public bool IsAuto { get { return false; } }

        public bool IsBmp { get { return true; } }

        public bool IsGif { get { return false; } }

        public bool IsJpg { get { return false; } }

        public bool IsPng { get { return false; } }

        public bool IsTiff { get { return false; } }

        public string Extention { get { return ".bmp"; } }

        public BitmapEncoder Encoder { get { return new BmpBitmapEncoder(); } }
    }
}
