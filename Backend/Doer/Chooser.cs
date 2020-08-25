using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Editure.Backend.CopyMove;
using Editure.Backend.Editing.PictureEditing;
using Editure.Backend.ViewModels;
using StdOttStandard;

namespace Editure.Backend.Doer
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

        protected override async Task Do(FileInfo file)
        {
            Task haveLockObj = null, haventLockObj = null;
            try
            {
                if (HaveSize(EditImage.LoadBitmap(file.FullName)))
                {
                    if (!viewModel.Have.IsDo) return;

                    haveLockObj = CopyMoveFiles.Current.ToFolder(file,
                        viewModel.Have.Dest.FullName, viewModel.Have.IsCopy);
                }
                else if (viewModel.Havent.IsDo)
                {
                    haventLockObj = CopyMoveFiles.Current.ToFolder(file,
                        viewModel.Havent.Dest.FullName, viewModel.Havent.IsCopy);
                }
            }
            catch { }

            await haveLockObj.ToNotNull();
            await haventLockObj.ToNotNull();
        }

        private bool HaveSize(BitmapSource bmp)
        {
            if (viewModel.IsAnd)
            {
                return minW <= bmp.PixelWidth && maxW >= bmp.PixelWidth && minH <= bmp.PixelHeight &&
                       maxH >= bmp.PixelHeight;
            }

            return (minW <= bmp.PixelWidth && maxW >= bmp.PixelWidth) ||
                   (minH <= bmp.PixelHeight && maxH >= bmp.Height);
        }
    }
}