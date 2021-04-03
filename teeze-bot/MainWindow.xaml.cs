using System.Windows;
using System.Windows.Input;

namespace teeze_bot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region BasicFeatures

        private void close_app_click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void minimize_app_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bool mouseIsDown = System.Windows.Input.Mouse.LeftButton == MouseButtonState.Pressed;
            if (mouseIsDown)
            {
                base.OnMouseLeftButtonDown(e);
                this.DragMove();
            }
        }

        #endregion BasicFeatures
    }
}