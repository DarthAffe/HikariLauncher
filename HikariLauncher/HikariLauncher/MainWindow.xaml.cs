using System.Windows;
using System.Windows.Input;
using HikariLauncher.ViewModels;

namespace HikariLauncher
{
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void OnTopBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //HACK: Interactivity is a crap library ...
            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            vm?.InitializeCommand.Execute(null);
        }

        #endregion
    }
}
