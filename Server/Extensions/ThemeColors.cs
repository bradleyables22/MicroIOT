namespace Server.Extensions
{
    public static class ThemeColors
    {
        private static readonly string[] _primaryPalette = new[]
        {
        "#a2e436", 
        "#68d391", 
        "#22d2ed", 
        "#ffc700", 
        "#f56565", 
        "#be82fa", 
        "#ea4998", 
        "#828df9", 
        "#61bcff",
        "#fa984a",
        "#2ed3be"  
    };

        public static string Get(int index)
        {
            if (index < _primaryPalette.Length)
                return _primaryPalette[index];

            double hue = (index * 137.508) % 360;
            return $"hsl({(int)hue}, 70%, 60%)";
        }

        public static Dictionary<string, string> AssignColors(ICollection<string> labels)
        {
            var map = new Dictionary<string, string>();
            int i = 0;
            foreach (var label in labels.OrderBy(l => l))
            {
                map[label] = Get(i++);
            }
            return map;
        }

        public static string GetByLabel(string label)
        {
            int hash = Math.Abs(label.GetHashCode());
            return Get(hash % 48); 
        }
    }

}
