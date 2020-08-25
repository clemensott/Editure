using System.Windows.Media.Imaging;

namespace Editure.Backend.Editing.EditEncoders
{
    class EncoderManagerGif : IEncoderManager
    {
        public bool IsAuto => false;

        public bool IsBmp => false;

        public bool IsGif => true;

        public bool IsJpg => false;

        public bool IsPng => false;

        public bool IsTiff => false;

        public string Extension => ".gif";

        public BitmapEncoder Encoder => new GifBitmapEncoder();
    }
}
