using System;
using System.Collections.Generic;
using System.Windows;

namespace MainProgram
{
    public struct EditPictureProperties : IEquatable<EditPictureProperties>
    {
        private IntPoint wannaOffset;

        public bool FlipX { get; set; }

        public bool FlipY { get; set; }

        public IntSize Wanna { get; set; }
        
        public EditMode ModeType { get; set; }

        public IEditMode Mode { get { return GetMode(); } }

        public EditReferencePositionType ReferencePositionType { get; set; }

        public IReferencePosition ReferencePosition { get { return GetReferencePosition(); } }

        public EditPictureProperties(bool flipX, bool flipY, IntSize wanna,
            IntPoint relativeOffset, EditMode modeType, EditReferencePositionType referencePositionType)
        {
            FlipX = flipX;
            FlipY = flipY;
            Wanna = wanna;
            ModeType = modeType;
            wannaOffset = relativeOffset;
            ReferencePositionType = referencePositionType;
        }

        public IntPoint GetRelativeOffset(IntSize original)
        {
            return ReferencePosition.GetOffsetPoint(GetCrop(original), GetScale(original), Wanna);
        }

        public void SetRelativeOffset(IntPoint offset)
        {
            wannaOffset = offset;
        }

        public IntSize GetScale(IntSize original) { return Mode.GetScaleSize(Wanna, original); }

        public IntSize GetShow(IntSize original) { return Mode.GetShowSize(Wanna, GetScale(original)); }

        private IReferencePosition GetReferencePosition()
        {
            switch (ReferencePositionType)
            {
                case EditReferencePositionType.TopLeft:
                    return new ReferencePositionTopLeft();

                case EditReferencePositionType.TopCenter:
                    return new ReferencePositionTopCenter();

                case EditReferencePositionType.TopRight:
                    return new ReferencePositionTopRight();

                case EditReferencePositionType.CenterLeft:
                    return new ReferencePositionCenterLeft();

                case EditReferencePositionType.CenterCenter:
                    return new ReferencePositionCenterCenter();

                case EditReferencePositionType.CenterRight:
                    return new ReferencePositionCenterRight();

                case EditReferencePositionType.BottomLeft:
                    return new ReferencePositionBottomLeft();

                case EditReferencePositionType.BottomCenter:
                    return new ReferencePositionBottomCenter();

                case EditReferencePositionType.BottomRight:
                    return new ReferencePositionBottomRight();
            }

            return new ReferencePositionCenterCenter();
        }

        private IEditMode GetMode()
        {
            switch (ModeType)
            {
                case EditMode.Crop:
                    return new EditModeCrop();

                case EditMode.Ratio:
                    return new EditModeRatio();

                case EditMode.ScaleWidth:
                    return new EditModeWidth();

                case EditMode.ScaleHeight:
                    return new EditModeHeight();
            }

            return new EditModeCrop();
        }

        public Int32Rect GetCrop(IntSize original)
        {
            IntSize scale = GetScale(original);
            Int32Rect crop = ReferencePosition.GetCrop(scale, GetShow(original), wannaOffset);

            return GetImprovedCrop(crop,scale);
        }

        private Int32Rect GetImprovedCrop(Int32Rect crop,IntSize scale)
        {
            if (ModeType == EditMode.ScaleHeight || ModeType == EditMode.ScaleWidth) return crop;

            if (crop.X + crop.Width > scale.Width) crop.X = scale.Width - crop.Width;
            else if (crop.X < 0) crop.X = 0;

            if (crop.Y + crop.Height > scale.Height) crop.Y = scale.Height - crop.Height;
            else if (crop.Y < 0) crop.Y = 0;

            return crop;
        }

        public override int GetHashCode()
        {
            var hashCode = -223394155;
            hashCode = hashCode * -1521134295 + EqualityComparer<IntPoint>.Default.GetHashCode(wannaOffset);
            hashCode = hashCode * -1521134295 + FlipX.GetHashCode();
            hashCode = hashCode * -1521134295 + FlipY.GetHashCode();
            hashCode = hashCode * -1521134295 + ModeType.GetHashCode();
            hashCode = hashCode * -1521134295 + ReferencePositionType.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is EditPictureProperties && Equals((EditPictureProperties)obj);
        }

        public bool Equals(EditPictureProperties other)
        {
            return wannaOffset.Equals(other.wannaOffset) &&
                   FlipX == other.FlipX &&
                   FlipY == other.FlipY &&
                   ModeType == other.ModeType &&
                   ReferencePositionType == other.ReferencePositionType;
        }

        public static bool operator ==(EditPictureProperties properties1, EditPictureProperties properties2)
        {
            return properties1.Equals(properties2);
        }

        public static bool operator !=(EditPictureProperties properties1, EditPictureProperties properties2)
        {
            return !(properties1 == properties2);
        }
    }
}
