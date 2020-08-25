using System.Windows;

namespace Editure.Backend.Editing.ReferencePosition
{
    public enum EditReferencePositionType
    {
        TopLeft, TopCenter, TopRight, CenterLeft,
        CenterCenter, CenterRight, BottomLeft, BottomCenter, BottomRight
    };

    public interface IReferencePosition
    {
        bool IsTopLeft { get; }

        bool IsTopCenter { get; }

        bool IsTopRight { get; }

        bool IsCenterLeft { get; }

        bool IsCenterCenter { get; }

        bool IsCenterRight { get; }

        bool IsBottomLeft { get; }

        bool IsBottomCenter { get; }

        bool IsBottomRight { get; }

        Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset);

        IntPoint GetOffsetPoint(Int32Rect crop, IntSize scale, IntSize show);
    }
}
