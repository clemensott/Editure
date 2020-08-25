using System.Windows.Media.Imaging;

namespace Editure.Backend.Editing.EditEncoders
{
    class EncoderManagerTiff : IEncoderManager
    {
        public bool IsAuto => false;

        public bool IsBmp => false;

        public bool IsGif => false;

        public bool IsJpg => false;

        public bool IsPng => false;

        public bool IsTiff => true;

        public string Extension => ".tiff";

        public BitmapEncoder Encoder => new TiffBitmapEncoder();
    }
}
