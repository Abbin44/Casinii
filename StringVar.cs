namespace Esolang
{
    class StringVar
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public StringVar(string name, string text, int index)
        {
            Name = name;
            Text = text;
            Index = index;
        }
    }
}
