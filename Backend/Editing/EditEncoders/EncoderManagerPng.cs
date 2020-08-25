using System.Windows.Media.Imaging;

namespace Editure.Backend.Editing.EditEncoders
{
    class EncoderManagerPng : IEncoderManager
    {
        public bool IsAuto => false;

        public bool IsBmp => false;

        public bool IsGif => false;

        public bool IsJpg => false;

        public bool IsPng => true;

        public bool IsTiff => false;

        public string Extension => ".png";

        public BitmapEncoder Encoder => new PngBitmapEncoder();
    }
}
