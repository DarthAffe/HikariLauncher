using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HikariLauncher.Controls
{
    public partial class BlurTextControl : UserControl
    {
        #region Properties & Fields

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BlurTextControl), new PropertyMetadata(default(string)));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextBrushProperty = DependencyProperty.Register("TextBrush", typeof(Brush), typeof(BlurTextControl),
                                                                                                  new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public Brush TextBrush
        {
            get { return (Brush)GetValue(TextBrushProperty); }
            set { SetValue(TextBrushProperty, value); }
        }

        public static readonly DependencyProperty BlurBrushProperty = DependencyProperty.Register("BlurBrush", typeof(Brush), typeof(BlurTextControl),
                                                                                                  new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public Brush BlurBrush
        {
            get { return (Brush)GetValue(BlurBrushProperty); }
            set { SetValue(BlurBrushProperty, value); }
        }

        #endregion

        #region Constructors

        public BlurTextControl()
        {
            InitializeComponent();
        }

        #endregion
    }
}
