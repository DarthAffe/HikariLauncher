using System.Windows;
using System.Windows.Controls;

namespace HikariLauncher.Controls
{
    public partial class BackgroundControl : UserControl
    {
        #region Properties & Fields

        // ReSharper disable once InconsistentNaming
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(string), typeof(BackgroundControl));
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        #endregion

        #region Constructors

        public BackgroundControl()
        {
            InitializeComponent();
        }

        #endregion
    }
}
