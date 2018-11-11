using System.IO;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    public class Chooser : Doer<ViewModelChoose, FileInfo>
    {
        private int minW, minH, maxW, maxH;

        public Chooser(ViewModelChoose parent) : base(parent)
        {
        }

        protected override FileInfo[] Initialize()
        {
            FileInfo[] files = Utils.SetSubTypeAndRefresh(viewModel.Src, "Refresh Choose Src failed");

            if (viewModel.Have.IsDo) Utils.DeleteContent(viewModel.Have, "Delete Have Content failed");
            if (viewModel.Havent.IsDo) Utils.DeleteContent(viewModel.Havent, "Delete Haven't Content failed");

            minW = viewModel.Min.Width;
            minH = viewModel.Min.Height;
            maxW = viewModel.Max.Width;
            maxH = viewModel.Max.Height;

            return files;
        }

        protected override void Do(FileInfo file)
        {
            try
            {
                if (HaveSize(EditImage.LoadBitmap(file.FullName)))
                {
                    if (viewModel.Have.IsDo)
                    {
                        string DestPath = Path.Combine(viewModel.Have.Dest.FullName, file.Name);
                        Utils.CopyMoveFile(file, new FileInfo(DestPath), viewModel.Have.IsCopy);
                    }
                }
                else
                {
                    if (viewModel.Havent.IsDo)
                    {
                        string DestPath = Path.Combine(viewModel.Havent.Dest.FullName, file.Name);
                        Utils.CopyMoveFile(file, new FileInfo(DestPath), viewModel.Havent.IsCopy);
                    }
                }
            }
            catch { }
        }

        private bool HaveSize(BitmapImage bmp)
        {
            if (viewModel.IsAnd)
            {
                return minW <= bmp.PixelWidth && maxW >= bmp.PixelWidth && minH <= bmp.PixelHeight && maxH >= bmp.PixelHeight;
            }

            return (minW <= bmp.PixelWidth && maxW >= bmp.PixelWidth) || (minH <= bmp.PixelHeight && maxH >= bmp.Height);
        }
    }
}
