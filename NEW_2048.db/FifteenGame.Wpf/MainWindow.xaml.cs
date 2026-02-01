using FifteenGame.Common.BusinessModels;
using FifteenGame.Wpf.ViewModels;
using FifteenGame.Wpf.Views;
using System.Windows;
using System.Windows.Input;

namespace FifteenGame.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnGameFinished()
        {
            if (MessageBox.Show("Игра окончена. Повторить?", "2048", MessageBoxButton.YesNo, MessageBoxImage.Information) ==
                MessageBoxResult.Yes)
            {
                ViewModel.ReInitialize();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dialog = new UserLoginWindow();
            dialog.ViewModel.MainViewModel = ViewModel;
            dialog.ShowDialog();
            Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MoveDirection direction;
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    direction = MoveDirection.Up;
                    break;
                case Key.Down:
                case Key.S:
                    direction = MoveDirection.Down;
                    break;
                case Key.Left:
                case Key.A:
                    direction = MoveDirection.Left;
                    break;
                case Key.Right:
                case Key.D:
                    direction = MoveDirection.Right;
                    break;
                default:
                    return;
            }

            ViewModel.MakeMove(direction, OnGameFinished);
            e.Handled = true;
        }
    }
}
