using System.Windows.Media;

namespace FifteenGame.Wpf.ViewModels
{
    public class CellViewModel
    {
        public int Num { get; set; }

        public string Text => Num == 0 ? string.Empty : Num.ToString();

        public int Row { get; set; }

        public int Column { get; set; }

        public Brush Background => new SolidColorBrush(GetBackgroundColor());

        public Brush Foreground => Num <= 4 ? new SolidColorBrush(Color.FromRgb(119, 110, 101)) : Brushes.White;

        private Color GetBackgroundColor()
        {
            return Num switch
            {
                0 => Color.FromRgb(205, 193, 180),
                2 => Color.FromRgb(238, 228, 218),
                4 => Color.FromRgb(237, 224, 200),
                8 => Color.FromRgb(242, 177, 121),
                16 => Color.FromRgb(245, 149, 99),
                32 => Color.FromRgb(246, 124, 95),
                64 => Color.FromRgb(246, 94, 59),
                128 => Color.FromRgb(237, 207, 114),
                256 => Color.FromRgb(237, 204, 97),
                512 => Color.FromRgb(237, 200, 80),
                1024 => Color.FromRgb(237, 197, 63),
                2048 => Color.FromRgb(237, 194, 46),
                _ => Color.FromRgb(60, 58, 50)
            };
        }
    }
}
