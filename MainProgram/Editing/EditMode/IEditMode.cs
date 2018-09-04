using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProgram
{
    public enum EditMode { Ratio, Crop, ScaleWidth, ScaleHeight };

    public interface IEditMode
    {
        bool IsCrop { get; }

        bool IsRatio { get; }

        bool IsWidth { get; }

        bool IsHeight { get; }

        IntSize GetScaleSize(IntSize wanna, IntSize original);

        IntSize GetShowSize(IntSize wanna, IntSize scale);
    }
}
