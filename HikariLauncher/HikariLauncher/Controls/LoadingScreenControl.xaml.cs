using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HikariLauncher.Controls
{
    public partial class LoadingScreenControl : UserControl
    {
        #region Properties & Fields

        // ReSharper disable InconsistentNaming

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingScreenControl), new FrameworkPropertyMetadata(false));
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(LoadingScreenControl), new FrameworkPropertyMetadata(0.0));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(LoadingScreenControl), new FrameworkPropertyMetadata(100.0));
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LoadingScreenControl));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // ReSharper enable InconsistentNaming

        #endregion

        #region Constructors

        public LoadingScreenControl()
        {
            InitializeComponent();
        }

        #endregion

        private void LoadingScreenControl_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
