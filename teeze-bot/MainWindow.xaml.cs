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

        private TaskInfo taskInfo = new TaskInfo();
        private Profile profile = new Profile();

        private int taskIdCounter = 0;
        private int profileCounter = 0;

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

        #endregion BasicFeatures

        #region Create Profile

        private void CreateProfileOption_Click(object sender, RoutedEventArgs e)
        {
            newProfile_Firstname.Text = "";
            newProfile_Lastname.Text = "";
            newProfile_EMail.Text = "";
            newProfile_Phone.Text = "";
            newProfile_Address1.Text = "";
            newProfile_Address2.Text = "";
            newProfile_City.Text = "";
            newProfile_ZIP.Text = "";
            newProfile_Country.SelectedIndex = -1;
            CreateProfileWindow.Visibility = Visibility.Visible;
            ProfilePageOptions.Visibility = Visibility.Hidden;
            ProfilePageList.Visibility = Visibility.Hidden;
            newProfile_errorFirstname.Visibility = Visibility.Hidden;
            newProfile_errorLastname.Visibility = Visibility.Hidden;
            newProfile_errorEmail.Visibility = Visibility.Hidden;
            newProfile_errorPhone.Visibility = Visibility.Hidden;
            newProfile_errorAddresse1.Visibility = Visibility.Hidden;
            newProfile_errorCity.Visibility = Visibility.Hidden;
            newProfile_errorZip.Visibility = Visibility.Hidden;
            newProfile_errorCountry.Visibility = Visibility.Hidden;
        }

        private void CancleCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            CloseCreateProfileWindow();
        }

        private void CreateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (IsProfileFormValid())
            {
                GatherProfileInfos();
                CloseCreateProfileWindow();
                AddProfileToList();
            }
        }

        private bool IsProfileFormValid()
        {
            newProfile_errorFirstname.Visibility = newProfile_Firstname.Text.Length == 0 || newProfile_Firstname.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorLastname.Visibility = newProfile_Lastname.Text.Length == 0 || newProfile_Lastname.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorEmail.Visibility = newProfile_EMail.Text.Length == 0 || newProfile_EMail.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorPhone.Visibility = newProfile_Phone.Text.Length == 0 || newProfile_Phone.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorAddresse1.Visibility = newProfile_Address1.Text.Length == 0 || newProfile_Address1.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorCity.Visibility = newProfile_City.Text.Length == 0 || newProfile_City.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorZip.Visibility = newProfile_ZIP.Text.Length == 0 || newProfile_ZIP.Text == "" ? Visibility : Visibility.Hidden;
            newProfile_errorCountry.Visibility = newProfile_Country.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;

            if (newProfile_Firstname.Text.Length != 0 && newProfile_Lastname.Text.Length != 0 && newProfile_EMail.Text.Length != 0 && newProfile_Phone.Text.Length != 0 && newProfile_Address1.Text.Length != 0 && newProfile_City.Text.Length != 0 && newProfile_ZIP.Text.Length != 0 && newProfile_City.Text.Length != 0)
                return true;
            else
                return false;
        }

        private void GatherProfileInfos()
        {
            var item = (ComboBoxItem)newProfile_Country.SelectedValue;
            string country = (string)item.Content;
            string firstname = newProfile_Firstname.Text;
            string lastname = newProfile_Lastname.Text;
            string eMail = newProfile_EMail.Text;
            string phone = newProfile_Phone.Text;
            string address1 = newProfile_Address1.Text;
            string address2 = newProfile_Address2.Text;
            if (address2 == null)
            {
                address2 = "";
            }
            string city = newProfile_City.Text;
            string zip = newProfile_ZIP.Text;
            profileCounter++;
            profile.AddProfileInfos(profileCounter, firstname, lastname, eMail, phone, address1, address2, city, zip, country);
        }

        private void CloseCreateProfileWindow()
        {
            CreateProfileWindow.Visibility = Visibility.Hidden;
            ProfilePageOptions.Visibility = Visibility.Visible;
            ProfilePageList.Visibility = Visibility.Visible;
        }

        private void AddProfileToList()
        {
            var profileNumber = new TextBlock()
            {
                Text = profile.ProfileNumber.ToString()
            };
            profileListNumber.Items.Add(profileNumber);

            var profileName = new TextBlock()
            {
                Text = string.Join(" ", profile.Firstname, profile.Lastname)
            };
            profileListName.Items.Add(profileName);

            var dateCreated = new TextBlock()
            {
                Text = DateTime.Now.ToString("M-d-yyyy")
            };
            profileListDateCreated.Items.Add(dateCreated);

            var profileCountry = new TextBlock()
            {
                Text = profile.Country
            };
            profileListCountry.Items.Add(profileCountry);

            var profileActions = new Button()
            {
                Content = "Delete"
            };
            profileListActions.Items.Add(profileActions);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            profileListNumber.Items.Clear();
            profileListName.Items.Clear();
            profileListDateCreated.Items.Clear();
            profileListCountry.Items.Clear();
            profileListActions.Items.Clear();
            profileCounter = 0;
        }

        #endregion Create Profile

        #region Create Task

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

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            if (IsTaskFormValid())
            {
                switch (newTask_Store.SelectedIndex)
                {
                    case 0:
                        GatherTaskInfo();
                        CloseCreateTaskWindow();
                        AddTaskToTaskList();
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

        private bool IsTaskFormValid()
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

        private void GatherTaskInfo()
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
            taskIdCounter++;
            taskInfo.AddInfos(taskIdCounter, Store, ShoeSize, Product, Profile, Proxy, Account);
        }

        private void AddTaskToTaskList()
        {
            TextBlock taskStore = new TextBlock()
            {
                Text = taskInfo.Store
            };
            taskListStore.Items.Add(taskStore);

            TextBlock taskProduct = new TextBlock()
            {
                Text = taskInfo.Product
            };
            taskListProduct.Items.Add(taskProduct);

            TextBlock taskSize = new TextBlock()
            {
                Text = taskInfo.ShoeSize.ToString()
            };
            taskListSizes.Items.Add(taskSize);

            TextBlock taskProfile = new TextBlock()
            {
                Text = taskInfo.Profile
            };
            taskListProfile.Items.Add(taskProfile);

            TextBlock taskProxies = new TextBlock()
            {
                Text = taskInfo.Proxy
            };
            taskListProxies.Items.Add(taskProxies);

            TextBlock taskStatus = new TextBlock()
            {
                Text = taskInfo.Status
            };
            taskListStatus.Items.Add(taskStatus);

            Button taskActions = new Button()
            {
                Content = "start"
            };
            taskListActions.Items.Add(taskActions);
        }

        private void DeleteAllOption_Click(object sender, RoutedEventArgs e)
        {
            taskListStore.Items.Clear();
            taskListProduct.Items.Clear();
            taskListSizes.Items.Clear();
            taskListProfile.Items.Clear();
            taskListProxies.Items.Clear();
            taskListStatus.Items.Clear();
            taskListActions.Items.Clear();
            taskIdCounter = 1;
        }

        #endregion Create Task
    }
}