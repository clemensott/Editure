namespace Editure.Backend.TextToVal
{
    public class TextInt : Text<int>
    {
        public TextInt(int value) : base(value)
        {

        }

        public TextInt(string value) : base(value)
        {

        }

        protected override bool TryConvert(string s, out int value)
        {
            return int.TryParse(s, out value);
        }

        public static implicit operator TextInt(string s)
        {
            return new TextInt(s);
        }
    }
}
