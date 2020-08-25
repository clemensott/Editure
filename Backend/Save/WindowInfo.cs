namespace Editure.Backend.Save
{
    public class WindowInfo
    {
        public bool Maximized { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

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
