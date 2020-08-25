using System.Windows.Media.Imaging;

namespace Editure.Backend.Editing.EditEncoders
{
    class EncoderManagerJpg : IEncoderManager
    {
        public bool IsAuto => false;

        public bool IsBmp => false;

        public bool IsGif => false;

        public bool IsJpg => true;

        public bool IsPng => false;

        public bool IsTiff => false;

        public string Extension => ".jpg";

        public BitmapEncoder Encoder => new JpegBitmapEncoder();
    }
}
