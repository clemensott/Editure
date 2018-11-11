using FolderFile;
using System.IO;
using System.Xml.Serialization;

namespace MainProgram.Save
{
    public class SaveClass
    {
        public WindowInfo Window;

        public SearchInfo Search;
        public ChooseInfo Choose;
        public EditInfo Edit;
        public CopyInfo Copy;
        public MixInfo Mix;

        private SaveClass()
        {
            Window = new WindowInfo();

            Search = new SearchInfo();
            Choose = new ChooseInfo();
            Edit = new EditInfo();
            Copy = new CopyInfo();
            Mix = new MixInfo();
        }

        private SaveClass(ViewModel vm)
        {
            Window = new WindowInfo();
            Search = new SearchInfo();
            Choose = new ChooseInfo();
            Edit = new EditInfo();
            Copy = new CopyInfo();
            Mix = new MixInfo();


            Window.Width = vm.WindowWidth;
            Window.Height = vm.WindowHeight;
            Window.PositionX = vm.WindowLeft;
            Window.PositionY = vm.WindowTop;
            Window.Maximized = vm.WindowState == System.Windows.WindowState.Maximized;


            Search.SrcSubfolderType = vm.Search.Src.SubType;
            Search.ReferenceSubfolderType = vm.Search.Ref.SubType;
            Search.SrcPath = vm.Search.Src.OriginalPath;
            Search.RefPath = vm.Search.Ref.OriginalPath;

            Search.SrcFound = vm.Search.SrcFound;
            Search.SrcNot = vm.Search.SrcNot;
            Search.RefFound = vm.Search.RefFound;

            Search.WithExtentions = vm.Search.IsWithExtension;


            Choose.SrcSubfolderType = vm.Choose.Src.SubType;
            Choose.SrcPath = vm.Choose.Src.OriginalPath;

            Choose.Have = vm.Choose.Have;
            Choose.Havent = vm.Choose.Havent;

            Choose.MinWidth = vm.Choose.Min.Width;
            Choose.MinHeight = vm.Choose.Min.Height;
            Choose.MaxWidth = vm.Choose.Max.Width;
            Choose.MaxHeight = vm.Choose.Max.Height;


            Edit.SrcSubfolderType = vm.Edit.Src.SubType;
            Edit.SrcPath = vm.Edit.Src.OriginalPath;
            Edit.DestPath = vm.Edit.Dest.OriginalPath;

            Edit.FlipX = vm.Edit.IsFlipX;
            Edit.FlipY = vm.Edit.IsFlipY;

            Edit.Wanna = vm.Edit.Wanna;
            Edit.Offset = vm.Edit.Offset;

            Edit.EditMode = vm.Edit.ModeType;
            Edit.EditReferencePositionType = vm.Edit.ReferencePosition;
            Edit.EditEncoder = vm.Edit.EncoderType;


            Copy.SrcSubfolderType = vm.Copy.Src.SubType;
            Copy.SrcPath = vm.Copy.Src.OriginalPath;
            Copy.DestPath = vm.Copy.Dest.OriginalPath;


            Mix.SubfolderType = vm.Mix.Folder.SubType;
            Mix.Path = vm.Mix.Folder.OriginalPath;
            Mix.Mix = vm.Mix.IsMix;
            Mix.Auto = vm.Mix.IsAuto;
        }

        public static ViewModel Load()
        {
            return Load("Config.xml");
        }

        public static ViewModel Load(string path)
        {
            SaveClass sc = LoadSaveClass(path);
            ViewModel vm = new ViewModel
            {
                WindowWidth = sc.Window.Width,
                WindowHeight = sc.Window.Height,
                WindowLeft = sc.Window.PositionX,
                WindowTop = sc.Window.PositionY,
                WindowState = sc.Window.Maximized ? System.Windows.WindowState.Maximized : System.Windows.WindowState.Normal
            };


            vm.Search.Src = new Folder(sc.Search.SrcPath, sc.Search.SrcSubfolderType);
            vm.Search.Ref = new Folder(sc.Search.RefPath, sc.Search.ReferenceSubfolderType);

            vm.Search.SrcFound = sc.Search.SrcFound;
            vm.Search.SrcNot = sc.Search.SrcNot;
            vm.Search.RefFound = sc.Search.RefFound;

            vm.Search.IsWithExtension = sc.Search.WithExtentions;


            vm.Choose.Src = new Folder(sc.Choose.SrcPath, sc.Choose.SrcSubfolderType);

            vm.Choose.Have = sc.Choose.Have;
            vm.Choose.Havent = sc.Choose.Havent;

            vm.Choose.Min = new IntSize(sc.Choose.MinWidth, sc.Choose.MinHeight);
            vm.Choose.Max = new IntSize(sc.Choose.MaxWidth, sc.Choose.MaxHeight);


            vm.Edit.Src = new Folder(sc.Edit.SrcPath, sc.Edit.SrcSubfolderType);
            vm.Edit.Dest = new Folder(sc.Edit.DestPath, SubfolderType.This);

            vm.Edit.IsFlipX = sc.Edit.FlipX;
            vm.Edit.IsFlipX = sc.Edit.FlipY;

            vm.Edit.Wanna = sc.Edit.Wanna;
            vm.Edit.Offset = sc.Edit.Offset;

            vm.Edit.ModeType = sc.Edit.EditMode;
            vm.Edit.ReferencePosition = sc.Edit.EditReferencePositionType;
            vm.Edit.EncoderType = sc.Edit.EditEncoder;


            vm.Copy.Src = new Folder(sc.Copy.SrcPath, sc.Copy.SrcSubfolderType);
            vm.Copy.Dest = new Folder(sc.Copy.DestPath, SubfolderType.This);


            vm.Mix.Folder = new Folder(sc.Mix.Path, sc.Mix.SubfolderType);

            vm.Mix.IsMix = sc.Mix.Mix;
            vm.Mix.IsAuto = sc.Mix.Auto;

            return vm;
        }

        private static SaveClass LoadSaveClass()
        {
            return LoadSaveClass("Config.xml");
        }

        private static SaveClass LoadSaveClass(string path)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(SaveClass));

                using (Stream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    return (SaveClass)ser.Deserialize(stream);
                }
            }
            catch { }

            return new SaveClass();
        }

        public static void Save(ViewModel viewModel)
        {
            Save(viewModel, "Config.xml");
        }

        public static void Save(ViewModel vm, string path)
        {
            SaveClass saveClass = new SaveClass(vm);
            XmlSerializer ser = new XmlSerializer(typeof(SaveClass));
            StreamWriter writer = new StreamWriter(path);

            ser.Serialize(writer, saveClass);
            writer.Close();
        }
    }
}
