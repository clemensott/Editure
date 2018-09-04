namespace MainProgram.Save
{
    public class WindowInfo
    {
        public bool Maximized = false;
        public double PositionX = 50, PositionY = 50;
        public double Width = 800, Height = 500;

        public WindowInfo()
        {
            Maximized = false;

            PositionX = 50;
            PositionY = 50;

            Width = 800;
            Height = 500;
        }
    }
}
