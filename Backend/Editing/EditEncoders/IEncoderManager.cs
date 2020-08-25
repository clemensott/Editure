using System.Windows.Media.Imaging;

namespace Editure.Backend.Editing.EditEncoders
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

        string Extension { get; }

        BitmapEncoder Encoder { get; }
    }
}
