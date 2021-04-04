using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using teeze_bot.classes;

namespace teeze_bot
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public TaskCommand task = new TaskCommand();

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

        private void Page_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            switch (button.Name)
            {
                case "Tasks":
                    TaskPage.Visibility = Visibility.Visible;
                    ProxiesPage.Visibility = Visibility.Hidden;
                    ProfilesPage.Visibility = Visibility.Hidden;
                    AccountsPage.Visibility = Visibility.Hidden;
                    SettingsPage.Visibility = Visibility.Hidden;
                    break;

                case "Proxies":
                    TaskPage.Visibility = Visibility.Hidden;
                    ProxiesPage.Visibility = Visibility.Visible;
                    ProfilesPage.Visibility = Visibility.Hidden;
                    AccountsPage.Visibility = Visibility.Hidden;
                    SettingsPage.Visibility = Visibility.Hidden;
                    break;

                case "Profiles":
                    TaskPage.Visibility = Visibility.Hidden;
                    ProxiesPage.Visibility = Visibility.Hidden;
                    ProfilesPage.Visibility = Visibility.Visible;
                    AccountsPage.Visibility = Visibility.Hidden;
                    SettingsPage.Visibility = Visibility.Hidden;
                    break;

                case "Accounts":
                    TaskPage.Visibility = Visibility.Hidden;
                    ProxiesPage.Visibility = Visibility.Hidden;
                    ProfilesPage.Visibility = Visibility.Hidden;
                    AccountsPage.Visibility = Visibility.Visible;
                    SettingsPage.Visibility = Visibility.Hidden;
                    break;

                case "Settings":
                    TaskPage.Visibility = Visibility.Hidden;
                    ProxiesPage.Visibility = Visibility.Hidden;
                    ProfilesPage.Visibility = Visibility.Hidden;
                    AccountsPage.Visibility = Visibility.Hidden;
                    SettingsPage.Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }
        }

        private void CreateTaskOption_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskWindow.Visibility = Visibility.Visible;
            TaskPageOptions.Visibility = Visibility.Hidden;
            TaskPageList.Visibility = Visibility.Hidden;
        }

        private void CancelCreateTask_Click(object sender, RoutedEventArgs e)
        {
            CreateTaskWindow.Visibility = Visibility.Hidden;
            TaskPageOptions.Visibility = Visibility.Visible;
            TaskPageList.Visibility = Visibility.Visible;
        }

        #endregion BasicFeatures

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            newTask_errorStore.Visibility = newTask_Store.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorSize.Visibility = newTask_Size.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProduct.Visibility = newTask_Product.Text.Length == 0 || newTask_Product.Text == "" ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProfile.Visibility = newTask_Profile.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProxy.Visibility = newTask_Proxy.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorAccount.Visibility = newTask_Account.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
        }
    }
}