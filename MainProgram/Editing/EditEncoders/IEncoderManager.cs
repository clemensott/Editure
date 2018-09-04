using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    public enum EditEncoderType { Auto, Jpg, Bmp, Png, Gif, Tiff };

    public interface IEncoderManager
    {
        bool IsAuto { get; }

        bool IsBmp { get; }

        bool IsJpg { get; }

        bool IsPng { get; }

        bool IsGif { get; }

        bool IsTiff { get; }

        string Extention { get; }

        BitmapEncoder Encoder { get; }
    }
}
