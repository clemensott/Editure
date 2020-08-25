using System.Windows.Media.Imaging;

namespace Editure.Backend.Editing.EditEncoders
{
    class EncoderManagerBmp : IEncoderManager
    {
        public bool IsAuto => false;

        public bool IsBmp => true;

        public bool IsGif => false;

        public bool IsJpg => false;

        public bool IsPng => false;

        public bool IsTiff => false;

        public string Extension => ".bmp";

        public BitmapEncoder Encoder => new BmpBitmapEncoder();
    }
}
