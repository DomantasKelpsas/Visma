namespace VismaProject
{
    public class StockItem
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int PortionCount
        {
            get;
            set;
        }
        public string Unit
        {
            get;
            set;
        }
        public float PortionSize
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{Id} {Name} {PortionCount} {Unit} {PortionSize}";
        }

    }
}
