namespace Esolang
{
    class Dice
    {
        public int Index { get; set; }
        public int Value { get; set; }
        public string VariableName { get; set; }
        public bool IsVariable { get; set; }

        public Dice(int index, int value, string variableName, bool isVariable)
        {
            Index = index;
            Value = value;
            VariableName = variableName;
            IsVariable = isVariable;
        }
    }
}
