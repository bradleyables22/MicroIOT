namespace Server.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumnAttribute :Attribute
    {
        public string Display { get; set; }
        public int Order { get; set; }
        public string TrueLabel { get; set; } = "True";
        public string FalseLabel { get; set; } = "False";

        public TableColumnAttribute(string display, int order)
        {
            Display = display;
            Order = order;
        }
    }
}
