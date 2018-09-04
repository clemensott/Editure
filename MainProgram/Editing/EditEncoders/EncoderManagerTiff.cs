using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    class EncoderManagerTiff : IEncoderManager
    {
        public bool IsAuto { get { return false; } }

        public bool IsBmp { get { return false; } }

        public bool IsGif { get { return false; } }

        public bool IsJpg { get { return false; } }

        public bool IsPng { get { return false; } }

        public bool IsTiff { get { return true; } }

        public string Extention { get { return ".tiff"; } }

        public BitmapEncoder Encoder { get { return new TiffBitmapEncoder(); } }
    }
}
