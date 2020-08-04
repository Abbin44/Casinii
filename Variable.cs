namespace Esolang
{
    class Variable
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }

        public Variable(string name, int value, int index)
        {
            Name = name;
            Value = value;
            Index = index;
        }
    }
}
