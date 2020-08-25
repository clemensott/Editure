using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using Editure.Backend.CopyMove;

namespace Editure.Backend
{
    /// <summary>
    /// Interaktionslogik für AskCopyMove.xaml
    /// </summary>
    public partial class AskCopyMove : Window
    {
        private static AskCopyMove instance;

        private static AskCopyMove Current
        {
            get
            {
                if (instance == null) instance = new AskCopyMove();

                return instance;
            }
        }

        private readonly Queue<CopyMoveErrorEventArgs> errors;

        private AskCopyMove()
        {
            InitializeComponent();

            errors = new Queue<CopyMoveErrorEventArgs>();
        }

        public static void Start()
        {
            CopyMoveFiles.Current.Error += Current.OnCopyMoveError;
        }

        private void OnCopyMoveError(object sender, CopyMoveErrorEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                errors.Enqueue(e);
                PrepareWindowAndShow();
            }));
        }

        private void PrepareWindowAndShow()
        {
            tblFilesCount.Text = "(" + errors.Count + ")";

            if (IsVisible) return;

            Prepare();
            ShowDialog();
        }

        private void Prepare()
        {
            if (errors.Count == 0) return;

            try
            {
                CopyMoveErrorEventArgs args = errors.First();

                tblSrcSize.Text = Utils.GetFileInfoSize(args.Pair.Source);
                tblSrcPath.Text = args.Pair.Source.FullName;
                tblDestSize.Text = Utils.GetFileInfoSize(new FileInfo(args.Pair.DestFilePath));
                tblDestPath.Text = args.Pair.DestFilePath;

                tblFilesCount.Text = "(" + errors.Count + ")";
            }
            catch
            {
            }
        }

        private void BtnReplace_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                ReplaceFile();

                if (errors.Count == 0)
                {
                    Hide();
                    return;
                }
            } while (cbxDoForAll.IsChecked == true);

            Prepare();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (errors.Count > 0)
            {
                e.Cancel = true;
                errors.Dequeue().Pair.Cancel();

                if (errors.Count == 0) Hide();
                else Prepare();
            }

            base.OnClosing(e);
        }

        private void BtnSkip_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                errors.Dequeue().Pair.Cancel();

                if (errors.Count == 0)
                {
                    Hide();
                    return;
                }
            } while (cbxDoForAll.IsChecked == true);

            Prepare();
        }

        private void BtnKeepBoth_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                KeepBothFiles();

                if (errors.Count == 0)
                {
                    Hide();
                    return;
                }
            } while (cbxDoForAll.IsChecked == true);

            Prepare();
        }

        private void ReplaceFile()
        {
            CopyMoveFilePair pair = errors.Dequeue().Pair;
            CopyMoveFiles.Current.EnqueueAgain(pair, CollisionType.Override);
        }

        private void KeepBothFiles()
        {
            CopyMoveFilePair pair = errors.Dequeue().Pair;
            CopyMoveFiles.Current.EnqueueAgain(pair, CollisionType.Unique);
        }
    }
}