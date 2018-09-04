using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    class EncoderManagerAuto : IEncoderManager
    {
        private string extention;
        private BitmapEncoder encoder;

        public bool IsAuto { get { return true; } }

        public bool IsBmp { get { return false; } }

        public bool IsGif { get { return false; } }

        public bool IsJpg { get { return false; } }

        public bool IsPng { get { return false; } }

        public bool IsTiff { get { return false; } }

        public string Extention { get { return extention; } }

        public BitmapEncoder Encoder { get { return encoder; } }

        public EncoderManagerAuto(string extention)
        {
            this.extention = extention;

            encoder = GetEncoder();
        }

        public BitmapEncoder GetEncoder()
        {
            extention = extention.ToLower();

            if (extention == ".jpg" || extention == ".jpeg") return new JpegBitmapEncoder();
            else if (extention == ".png") return new PngBitmapEncoder();
            else if (extention == ".gif") return new GifBitmapEncoder();
            else if (extention == ".tiff") return new TiffBitmapEncoder();
            else
            {
                extention = ".bmp";

                return new BmpBitmapEncoder();
            }
        }
    }
}
