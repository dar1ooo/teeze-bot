using System;
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

        private int taskIdCounter = 1;
        private TitoloTask titoloTask = new TitoloTask();

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

            //reset Form Inputs
            newTask_Store.SelectedIndex = -1;
            newTask_Size.SelectedIndex = -1;
            newTask_Product.Text = "";
            newTask_Profile.SelectedIndex = -1;
            newTask_Proxy.SelectedIndex = -1;
            newTask_Account.SelectedIndex = -1;
            newTask_AccountLabel.Visibility = Visibility.Visible;
            newTask_Account.Visibility = Visibility.Visible;
            newTask_errorStore.Visibility = Visibility.Hidden;
            newTask_errorSize.Visibility = Visibility.Hidden;
            newTask_errorProduct.Visibility = Visibility.Hidden;
            newTask_errorProfile.Visibility = Visibility.Hidden;
            newTask_errorProxy.Visibility = Visibility.Hidden;
            newTask_errorAccount.Visibility = Visibility.Hidden;
        }

        private void CancelCreateTask_Click(object sender, RoutedEventArgs e)
        {
            CloseCreateTaskWindow();
        }
        private void CreateProfileOption_Click(object sender, RoutedEventArgs e)
        {
            CreateProfileWindow.Visibility = Visibility.Visible;
            ProfilePageOptions.Visibility = Visibility.Hidden;
            ProfilePageList.Visibility = Visibility.Hidden;

        }
        private void CancleCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            CreateProfileWindow.Visibility = Visibility.Hidden;
            ProfilePageOptions.Visibility = Visibility.Visible;
            ProfilePageList.Visibility = Visibility.Visible;
        }
        
        private void CreateProfile_Click(object sender, RoutedEventArgs e)
        {
            newProfile_errorFirstname.Visibility = newProfile_Firstname.Text.Length == 0 || newProfile_Firstname.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorLastname.Visibility = newProfile_Lastname.Text.Length == 0 || newProfile_Lastname.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorEmail.Visibility = newProfile_EMail.Text.Length == 0 || newProfile_EMail.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorPhone.Visibility = newProfile_Phone.Text.Length == 0 || newProfile_Phone.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorAdresse1.Visibility = newProfile_Adress1.Text.Length == 0 || newProfile_Adress1.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorCity.Visibility = newProfile_City.Text.Length == 0 || newProfile_City.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorZip.Visibility = newProfile_ZIP.Text.Length == 0 || newProfile_ZIP.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorCountry.Visibility = newProfile_Country.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;

        }


        #endregion BasicFeatures

        #region Create Task

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                switch (newTask_Store.SelectedIndex)
                {
                    case 0:
                        GatherInfo();
                        CloseCreateTaskWindow();
                        break;

                    default:
                        break;
                }
            }
        }

        private void CloseCreateTaskWindow()
        {
            CreateTaskWindow.Visibility = Visibility.Hidden;
            TaskPageOptions.Visibility = Visibility.Visible;
            TaskPageList.Visibility = Visibility.Visible;
        }

        private bool IsFormValid()
        {
            newTask_errorStore.Visibility = newTask_Store.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorSize.Visibility = newTask_Size.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProduct.Visibility = newTask_Product.Text.Length == 0 || newTask_Product.Text == "" ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProfile.Visibility = newTask_Profile.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProxy.Visibility = newTask_Proxy.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;    
            newTask_errorAccount.Visibility = newTask_Account.SelectedIndex == -1 && newTask_Account.Visibility == Visibility.Visible ? Visibility.Visible : Visibility.Hidden;

            if (newTask_Store.SelectedIndex != -1 && newTask_Size.SelectedIndex != -1 && newTask_Product.Text != "" && newTask_Profile.SelectedIndex != -1 && newTask_Proxy.SelectedIndex != -1 && (newTask_Account.SelectedIndex == -1 && newTask_Account.Visibility == Visibility.Hidden))
                return true;
            else
                return false;
        }

        private void SelectStore(object sender, SelectionChangedEventArgs e)
        {
            if (newTask_Store.SelectedIndex == 0)
            {
                newTask_AccountLabel.Visibility = Visibility.Hidden;
                newTask_Account.Visibility = Visibility.Hidden;
            }
        }

        private TaskInfo GatherInfo()
        {
            var item = (ComboBoxItem)newTask_Store.SelectedValue;
            string Store = (string)item.Content;
            item = (ComboBoxItem)newTask_Size.SelectedValue;
            double ShoeSize = Convert.ToDouble(item.Content);
            string Product = newTask_Product.Text.ToString();
            item = (ComboBoxItem)newTask_Profile.SelectedValue;
            string Profile = (string)item.Content;
            item = (ComboBoxItem)newTask_Proxy.SelectedValue;
            string Proxy = (string)item.Content;
            item = (ComboBoxItem)newTask_Account.SelectedValue;
            string Account = "";
            if (item != null)
            {
                Account = (string)item.Content;
            }
            TaskInfo taskinfo = new TaskInfo(taskIdCounter, Store, ShoeSize, Product, Profile, Proxy, Account);
            taskIdCounter++;
            return taskinfo;
        }

        #endregion Create Task
    }
}