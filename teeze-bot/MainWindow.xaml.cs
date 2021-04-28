using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using teeze_bot.classes;
using teeze_bot.classes.enums;

namespace teeze_bot
{
    public partial class MainWindow : Window
    {
        #region Global Variables

        private List<TaskInfo> taskList = new List<TaskInfo>();
        private List<Profile> profileList = new List<Profile>();
        private List<KithTask> kithTasks = new List<KithTask>();
        private List<Account> accountList = new List<Account>();
        private TaskInfo currentTask = new TaskInfo();
        private Profile currentProfile = new Profile();
        private Account currentAccount = new Account();
        private Confirm confirm = 0;

        private int taskIdCounter = 0;
        private int profileCounter = 0;
        private int accountCounter = 0;
        private int runningTasks = 0;

        #endregion Global Variables

        public MainWindow()
        {
            InitializeComponent();
            TeezeOpened();
        }

        #region Teeze Opened and Closed

        private void TeezeOpened()
        {
            ReadTasksFromJSON();
            ReadProfilesFromJSON();
            ReadAccountsFromJSON();
            RefreshAllContent();
        }

        private void TeezeClosed(object sender, CancelEventArgs e)
        {
            SaveTasksToJSON();
            SaveProfilesToJSON();
            SaveAccountsToJSON();
        }

        #endregion Teeze Opened and Closed

        #region BasicFeatures

        private void close_app_click(object sender, RoutedEventArgs e)
        {
            bool anyTaskInProgress = false;
            bool noRunningTaskFound = false;

            while (anyTaskInProgress == false && noRunningTaskFound == false)
            {
                foreach (KithTask titoloTask in kithTasks)
                {
                    if (!anyTaskInProgress)
                    {
                        anyTaskInProgress = titoloTask.InProgress;
                    }
                }
                if (!anyTaskInProgress)
                {
                    noRunningTaskFound = true;
                }
            }

            if (noRunningTaskFound)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("There are still tasks running. Please end them first");
            }
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

        #region Task

        #region General Top Options

        private void DeleteAllTasks_Click(object sender, RoutedEventArgs e)
        {
            if (taskIdCounter == 0)
            {
                MessageBox.Show("There are no tasks to delete");
            }
            else
            {
                confirm = Confirm.DeleteAllTasks;
                TaskPageOptions.Visibility = Visibility.Hidden;
                TaskPageList.Visibility = Visibility.Hidden;
                ConfirmDeleteWindow.Visibility = Visibility.Visible;
                ConfirmDeleteLabel.Content = "Do you want to delete all tasks ?";
            }
        }

        private void DeleteAllTasks()
        {
            bool anyTaskInProgress = false;
            bool noRunningTaskFound = false;

            while (anyTaskInProgress == false && noRunningTaskFound == false)
            {
                foreach (KithTask titoloTask in kithTasks)
                {
                    if (!anyTaskInProgress)
                    {
                        anyTaskInProgress = titoloTask.InProgress;
                    }
                }
                if (!anyTaskInProgress)
                {
                    noRunningTaskFound = true;
                }
            }

            if (!anyTaskInProgress)
            {
                taskList.Clear();
                SaveTasksToJSON();
                taskIdCounter = 0;
                taskListView.ItemsSource = null;
                taskListView.Items.Clear();
                RefreshAllContent();
            }
            else
            {
                MessageBox.Show("There are still tasks running. Please end them first");
            }
            TaskPageOptions.Visibility = Visibility.Visible;
            TaskPageList.Visibility = Visibility.Visible;
        }

        private void StopAllOptions_Click(object sender, RoutedEventArgs e)
        {
            if (taskIdCounter == 0)
            {
                MessageBox.Show("There are no tasks to delete");
            }
            else
            {
                foreach (KithTask titoloTask in kithTasks)
                {
                    if (titoloTask.InProgress)
                    {
                        titoloTask.QuitTask();
                    }
                }
            }
            runningTasks = 0;
            RunningTaskLabel.Content = runningTasks.ToString();
        }

        #region Create Task

        private void CreateNewTask_Click(object sender, RoutedEventArgs e)
        {
            if (profileCounter == 0)
            {
                MessageBox.Show("Please Create a profile first");
            }
            else
            {
                CreateTaskLabel.Content = "Create Task";
                CreateTaskWindow.Visibility = Visibility.Visible;
                TaskPageOptions.Visibility = Visibility.Hidden;
                TaskPageList.Visibility = Visibility.Hidden;
                newTask_Store.SelectedIndex = -1;
                newTask_Sizes.Text = "";
                newTask_Productname.Text = "";
                newTask_Product.Text = "";
                newTask_Profile.SelectedIndex = -1;
                newTask_Proxy.SelectedIndex = -1;
                newTask_Account.SelectedIndex = -1;
                SaveEditedTaskButton.Visibility = Visibility.Hidden;
                CreateTaskButton.Visibility = Visibility.Visible;
                newTask_AccountLabel.Visibility = Visibility.Visible;
                newTask_Account.Visibility = Visibility.Visible;
                newTask_errorStore.Visibility = Visibility.Hidden;
                newTask_errorSizes.Visibility = Visibility.Hidden;
                newTask_errorProduct.Visibility = Visibility.Hidden;
                newTask_errorProductname.Visibility = Visibility.Hidden;
                newTask_errorProfileEmpty.Visibility = Visibility.Hidden;
                newTask_errorProxy.Visibility = Visibility.Hidden;
                newTask_errorAccount.Visibility = Visibility.Hidden;
            }
        }

        private void CancelCreateTask_Click(object sender, RoutedEventArgs e)
        {
            CloseCreateTaskWindow();
        }

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            if (IsTaskFormValid())
            {
                GatherTaskInfo(false);
                SaveTasksToJSON();
                CloseCreateTaskWindow();
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
            newTask_errorSizes.Visibility = newTask_Sizes.Text == "" ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProduct.Visibility = newTask_Product.Text.Length == 0 || newTask_Product.Text == "" ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProductname.Visibility = newTask_Productname.Text.Length == 0 || newTask_Productname.Text == "" ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProfileEmpty.Visibility = newTask_Profile.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorProxy.Visibility = newTask_Proxy.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newTask_errorAccount.Visibility = newTask_Account.SelectedIndex == -1 && newTask_Account.Visibility == Visibility.Visible ? Visibility.Visible : Visibility.Hidden;

            if (newTask_Store.SelectedIndex != -1 && newTask_Sizes.Text != "" && newTask_Productname.Text != "" && newTask_Product.Text != "" && newTask_Profile.SelectedIndex != -1 && newTask_Proxy.SelectedIndex != -1 && (newTask_Account.Visibility == Visibility.Hidden || newTask_Account.SelectedIndex != -1 && newTask_Account.Visibility == Visibility.Visible))
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

        private void GatherTaskInfo(bool isEdited)
        {
            var item = (ComboBoxItem)newTask_Store.SelectedValue;
            string Store = (string)item.Content;
            int StoreIndex = newTask_Store.SelectedIndex;
            string ShoeSizes = (newTask_Sizes.Text.ToString());
            string Productname = newTask_Productname.Text.ToString();
            string Product = newTask_Product.Text.ToString();
            string Profile = newTask_Profile.SelectedValue.ToString();
            int ProfileIndex = newTask_Profile.SelectedIndex;
            item = (ComboBoxItem)newTask_Proxy.SelectedValue;
            string Proxy = (string)item.Content;
            int ProxyIndex = newTask_Proxy.SelectedIndex;
            item = (ComboBoxItem)newTask_Account.SelectedValue;
            int AccountIndex = newTask_Account.SelectedIndex;
            string Account = "";
            if (item != null)
            {
                Account = (string)item.Content;
            }
            if (newTask_Account.Visibility == Visibility.Hidden)
            {
                Account = "";
            }
            if (!isEdited)
            {
                taskIdCounter++;
                taskList.Add(new TaskInfo(taskIdCounter, Store, StoreIndex, ShoeSizes, Productname, Product, Profile, ProfileIndex, Proxy, ProxyIndex, Account, AccountIndex));
                kithTasks.Add(new KithTask() { taskinfo = taskList[taskIdCounter - 1] });
            }
            if (isEdited)
            {
                taskList[currentTask.TaskId - 1].UpdateInfo(currentTask.TaskId, Store, StoreIndex, ShoeSizes, Productname, Product, Profile, ProfileIndex, Proxy, ProxyIndex, Account, AccountIndex);
            }
            taskListView.ItemsSource = taskList;
            taskListView.Items.Refresh();
        }

        private void SaveTasksToJSON()
        {
            File.WriteAllText(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userTasks.json", JsonConvert.SerializeObject(taskList, Formatting.Indented));
        }

        private void ReadTasksFromJSON()
        {
            using (StreamReader r = new StreamReader(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userTasks.json"))
            {
                string json = r.ReadToEnd();
                taskList = JsonConvert.DeserializeObject<List<TaskInfo>>(json);
            }
            taskListView.ItemsSource = taskList;
            taskIdCounter = taskList.Count;
            taskListView.Items.Refresh();

            foreach (TaskInfo task in taskList)
            {
                kithTasks.Add(new KithTask() { taskinfo = task });
            }
        }

        private void TaskListViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar
            var col1 = 0.03;
            var col2 = 0.09714;
            var col3 = 0.18714;
            var col4 = 0.13714;
            var col5 = 0.13714;
            var col6 = 0.14714;
            var col7 = 0.10;
            var col8 = 0.1665;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
            gView.Columns[4].Width = workingWidth * col5;
            gView.Columns[5].Width = workingWidth * col6;
            gView.Columns[6].Width = workingWidth * col7;
            gView.Columns[7].Width = workingWidth * col8;
        }

        #endregion Create Task

        #endregion General Top Options

        #region Task List Options

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            currentTask = button.CommandParameter as TaskInfo;
            if (!kithTasks[currentTask.TaskId - 1].InProgress)
            {
                currentTask = button.CommandParameter as TaskInfo;
                CreateTaskLabel.Content = "Task " + currentTask.TaskId.ToString();
                TaskPageList.Visibility = Visibility.Hidden;
                TaskPageOptions.Visibility = Visibility.Hidden;
                SaveEditedTaskButton.Visibility = Visibility.Visible;
                CreateTaskButton.Visibility = Visibility.Hidden;
                CreateTaskWindow.Visibility = Visibility.Visible;

                newTask_Store.SelectedIndex = currentTask.StoreIndex;
                newTask_Sizes.Text = currentTask.ShoeSizes;
                newTask_Productname.Text = currentTask.Productname;
                newTask_Product.Text = currentTask.ProductLink;
                newTask_Profile.SelectedIndex = currentTask.ProfileIndex;
                newTask_Proxy.SelectedIndex = currentTask.ProxyIndex;
                newTask_Account.SelectedIndex = currentTask.AccountIndex;
            }
            else
            {
                MessageBox.Show("Cannot edit a Task which is running");
            }
        }

        private void SaveEditedTask_Click(object sender, RoutedEventArgs e)
        {
            GatherTaskInfo(true);
            CloseCreateTaskWindow();
        }

        private void DeleteSpecificTask_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            currentTask = button.CommandParameter as TaskInfo;
            if (!kithTasks[currentTask.TaskId - 1].InProgress)
            {
                confirm = Confirm.DeleteSpecificTask;
                TaskPageOptions.Visibility = Visibility.Hidden;
                TaskPageList.Visibility = Visibility.Hidden;
                ConfirmDeleteWindow.Visibility = Visibility.Visible;
                ConfirmDeleteLabel.Content = "Do you want to delete this task ?";
            }
            else
            {
                MessageBox.Show("Cannot delete a Task which is running");
            }
        }

        private void DeleteSpecificTask()
        {
            taskList.Remove(currentTask);
            taskIdCounter--;
            foreach (TaskInfo deletedTaskList in taskList)
            {
                if (deletedTaskList.TaskId > currentTask.TaskId)
                {
                    deletedTaskList.TaskId--;
                }
            }
            RefreshAllContent();
            RefreshAllContent();
            TaskPageOptions.Visibility = Visibility.Visible;
            TaskPageList.Visibility = Visibility.Visible;
        }

        private void StartOrEndTask_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            TaskInfo task = button.CommandParameter as TaskInfo;
            if (button.Content.ToString() == "Start")
            {
                button.Content = "End";
                switch (task.Store)
                {
                    case "Titolo":
                        kithTasks[task.TaskId - 1].StartTask();
                        break;

                    default:
                        break;
                }
                runningTasks++;
                RunningTaskLabel.Content = runningTasks.ToString();
            }
            else
            {
                button.Content = "Start";
                switch (task.Store)
                {
                    case "Titolo":
                        kithTasks[task.TaskId - 1].QuitTask();
                        break;

                    default:
                        break;
                }
                runningTasks--;
                RunningTaskLabel.Content = runningTasks.ToString();
            }
        }

        #endregion Task List Options

        #endregion Task

        #region Profiles

        #region Top Profile Options

        private void DeleteAllProfiles_Click(object sender, RoutedEventArgs e)
        {
            bool anyProfileInUseTask = false;

            foreach (TaskInfo task in taskList)
            {
                foreach (Profile profile in profileList)
                {
                    if (profile.FullName == task.Profile)
                    {
                        anyProfileInUseTask = true;
                    }
                }
            }

            if (profileCounter == 0)
            {
                MessageBox.Show("There are no profiles to delete");
            }
            else if (anyProfileInUseTask)
            {
                MessageBox.Show("Cannot delete all profiles because some are used in Tasks");
            }
            else
            {
                confirm = Confirm.DeleteAllProfiles;
                ProfilePageOptions.Visibility = Visibility.Hidden;
                ProfilePageList.Visibility = Visibility.Hidden;
                ConfirmDeleteWindow.Visibility = Visibility.Visible;
                ConfirmDeleteLabel.Content = "Do you want to delete all profiles ?";
            }
        }

        private void DeleteAllProfiles()
        {
            profileList.Clear();
            SaveProfilesToJSON();
            profileCounter = 0;
            profilesListView.ItemsSource = null;
            profilesListView.Items.Clear();
            RefreshAllContent();
            ProfilePageOptions.Visibility = Visibility.Visible;
            ProfilePageList.Visibility = Visibility.Visible;
        }

        #endregion Top Profile Options

        #region Profile List Options

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            currentProfile = button.CommandParameter as Profile;
            CreateProfileLabel.Content = "Profile " + currentProfile.ProfileNumber.ToString();
            ProfilePageList.Visibility = Visibility.Hidden;
            ProfilePageOptions.Visibility = Visibility.Hidden;
            SaveEditedProfileButton.Visibility = Visibility.Visible;
            CreateProfileButton.Visibility = Visibility.Hidden;
            CreateProfileWindow.Visibility = Visibility.Visible;

            newProfile_Firstname.Text = currentProfile.Firstname;
            newProfile_Lastname.Text = currentProfile.Lastname;
            newProfile_EMail.Text = currentProfile.EMail;
            newProfile_Phone.Text = currentProfile.Phone;
            newProfile_Address1.Text = currentProfile.Address1;
            newProfile_Address2.Text = currentProfile.Address2;
            newProfile_City.Text = currentProfile.City;
            newProfile_ZIP.Text = currentProfile.ZIP;
            newProfile_Country.SelectedIndex = currentProfile.CountryIndex;
        }

        private void SaveEditedProfile_CLick(object sender, RoutedEventArgs e)
        {
            GatherProfileInfos(true);
            CloseCreateProfileWindow();
        }

        private void DeleteSpecificProfile_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            currentProfile = button.CommandParameter as Profile;
            bool anyProfileInUse = false;

            foreach (TaskInfo task in taskList)
            {
                if (task.Profile == currentProfile.FullName)
                {
                    anyProfileInUse = true;
                }
            }

            if (!anyProfileInUse)
            {
                confirm = Confirm.DeleteSpecificProfile;
                ProfilePageOptions.Visibility = Visibility.Hidden;
                ProfilePageList.Visibility = Visibility.Hidden;
                ConfirmDeleteWindow.Visibility = Visibility.Visible;
                ConfirmDeleteLabel.Content = "Do you want to delete this profile ?";
            }
            else if (anyProfileInUse)
            {
                MessageBox.Show("Cannot delete this profile because it is used in a task");
            }
        }

        private void DeleteSpecificProfile()
        {
            profileList.Remove(currentProfile);
            profileCounter--;
            foreach (Profile profile in profileList)
            {
                if (profile.ProfileNumber > currentProfile.ProfileNumber)
                {
                    profile.ProfileNumber--;
                }
            }
            RefreshAllContent();
            ProfilePageOptions.Visibility = Visibility.Visible;
            ProfilePageList.Visibility = Visibility.Visible;
        }

        #endregion Profile List Options

        #region Create Profile

        private void CreateProfileOption_Click(object sender, RoutedEventArgs e)
        {
            CreateProfileLabel.Content = "Create Profile";
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
            SaveEditedProfileButton.Visibility = Visibility.Hidden;
            CreateProfileButton.Visibility = Visibility.Visible;
            CloseCreateProfileWindow();
        }

        private void CreateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (IsProfileFormValid())
            {
                GatherProfileInfos(false);
                CloseCreateProfileWindow();
                SaveProfilesToJSON();
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

        private void GatherProfileInfos(bool isEdited)
        {
            var item = (ComboBoxItem)newProfile_Country.SelectedValue;
            int countryIndex = newProfile_Country.SelectedIndex;
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
            string dateCreated = DateTime.Now.ToString("M-d-yyyy");
            if (!isEdited)
            {
                profileCounter++;
                profileList.Add(new Profile(profileCounter, firstname, lastname, eMail, phone, address1, address2, city, zip, country, countryIndex, dateCreated));
            }
            if (isEdited)
            {
                profileList[currentProfile.ProfileNumber - 1].UpdateInfo(profileCounter, firstname, lastname, eMail, phone, address1, address2, city, zip, country, countryIndex, dateCreated);
            }
            RefreshAllContent();
        }

        private void CloseCreateProfileWindow()
        {
            CreateProfileWindow.Visibility = Visibility.Hidden;
            ProfilePageOptions.Visibility = Visibility.Visible;
            ProfilePageList.Visibility = Visibility.Visible;
        }

        #endregion Create Profile

        #region Other

        private void ProfileListViewSizeChanged(object sender, RoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar
            var col1 = 0.05;
            var col2 = 0.16714;
            var col3 = 0.15714;
            var col4 = 0.15714;
            var col5 = 0.30714;
            var col6 = 0.16714;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
            gView.Columns[4].Width = workingWidth * col5;
            gView.Columns[5].Width = workingWidth * col6;
        }

        private void SaveProfilesToJSON()
        {
            File.WriteAllText(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userProfiles.json", JsonConvert.SerializeObject(profileList, Formatting.Indented));
        }

        private void ReadProfilesFromJSON()
        {
            using (StreamReader r = new StreamReader(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userProfiles.json"))
            {
                string json = r.ReadToEnd();
                profileList = JsonConvert.DeserializeObject<List<Profile>>(json);
            }
            profileCounter = 0;
            profileCounter = profileList.Count;
            RefreshAllContent();
        }

        #endregion Other

        #endregion Profiles

        #region Accounts

        #region Account Top Options

        #region Create Account

        private void CreateAccountOption_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountLabel.Content = "Create Account";
            newAccount_Email.Text = "";
            newAccount_Password.Text = "";
            newAccount_Store.SelectedIndex = -1;
            CreateAccountWindow.Visibility = Visibility.Visible;
            AccountPageOptions.Visibility = Visibility.Hidden;
            AccountsListView.Visibility = Visibility.Hidden;
            newAccount_errorStore.Visibility = Visibility.Hidden;
            newAccount_errorEmail.Visibility = Visibility.Hidden;
            newAccount_errorPassword.Visibility = Visibility.Hidden;
        }

        private void CancleCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            SaveEditedAccountButton.Visibility = Visibility.Hidden;
            CreateAccountButton.Visibility = Visibility.Visible;
            CloseCreateAccountWindow();
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (IsAccountFormValid())
            {
                GatherAccountInfos(false);
                CloseCreateAccountWindow();
                SaveAccountsToJSON();
            }
        }

        private bool IsAccountFormValid()
        {
            newAccount_errorStore.Visibility = newAccount_Store.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
            newAccount_errorEmail.Visibility = newAccount_Email.Text.Length == 0 || newAccount_Email.Text == "" ? Visibility : Visibility.Hidden;
            newAccount_errorPassword.Visibility = newAccount_Password.Text.Length == 0 || newAccount_Password.Text == "" ? Visibility : Visibility.Hidden;

            if (newAccount_Store.SelectedIndex != -1 && newAccount_Email.Text.Length != 0 && newAccount_Password.Text.Length != 0)
                return true;
            else
                return false;
        }

        private void GatherAccountInfos(bool isEdited)
        {
            var item = (ComboBoxItem)newAccount_Store.SelectedValue;
            int storeIndex = newAccount_Store.SelectedIndex;
            string store = (string)item.Content;
            string email = newAccount_Email.Text;
            string password = newAccount_Password.Text;
            if (!isEdited)
            {
                accountCounter++;
                accountList.Add(new Account(accountCounter, store, storeIndex, email, password));
            }
            if (isEdited)
            {
                accountList[currentAccount.AccountId - 1].UpdateInfo(accountCounter, store, storeIndex, email, password);
            }
            RefreshAllContent();
        }

        private void CloseCreateAccountWindow()
        {
            CreateAccountWindow.Visibility = Visibility.Hidden;
            AccountPageOptions.Visibility = Visibility.Visible;
            AccountsListView.Visibility = Visibility.Visible;
        }

        private void DeleteAllAccounts(object sender, RoutedEventArgs e)
        {
            accountList.Clear();
            SaveAccountsToJSON();
            accountCounter = 0;
            AccountsListView.ItemsSource = null;
            AccountsListView.Items.Clear();
            RefreshAllContent();
        }

        private void SaveAccountsToJSON()
        {
            File.WriteAllText(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userAccounts.json", JsonConvert.SerializeObject(accountList, Formatting.Indented));
        }

        private void ReadAccountsFromJSON()
        {
            using (StreamReader r = new StreamReader(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userAccounts.json"))
            {
                string json = r.ReadToEnd();
                accountList = JsonConvert.DeserializeObject<List<Account>>(json);
            }
            accountCounter = 0;
            accountCounter = accountList.Count;
            RefreshAllContent();
        }

        #endregion Create Account

        private void DeleteAllAccounts_Click(object sender, RoutedEventArgs e)
        {
            bool anyAccountInUseTask = false;

            foreach (TaskInfo task in taskList)
            {
                foreach (Account Account in accountList)
                {
                    if (Account.Email == task.Account)
                    {
                        anyAccountInUseTask = true;
                    }
                }
            }

            if (accountCounter == 0)
            {
                MessageBox.Show("There are no Accounts to delete");
            }
            else if (anyAccountInUseTask)
            {
                MessageBox.Show("Cannot delete all Accounts because some are used in Tasks");
            }
            else
            {
                confirm = Confirm.DeleteAllAccounts;
                AccountPageOptions.Visibility = Visibility.Hidden;
                AccountPageList.Visibility = Visibility.Hidden;
                ConfirmDeleteWindow.Visibility = Visibility.Visible;
                ConfirmDeleteLabel.Content = "Do you want to delete all Accounts ?";
            }
        }

        private void DeleteAllAccounts()
        {
            accountList.Clear();
            SaveAccountsToJSON();
            accountCounter = 0;
            AccountsListView.ItemsSource = null;
            AccountsListView.Items.Clear();
            RefreshAllContent();
            AccountPageOptions.Visibility = Visibility.Visible;
            AccountPageList.Visibility = Visibility.Visible;
        }

        #endregion Account Top Options

        #region Account List Options

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            currentAccount = button.CommandParameter as Account;
            CreateAccountLabel.Content = "Account " + currentAccount.AccountId.ToString();
            AccountsListView.Visibility = Visibility.Hidden;
            AccountPageOptions.Visibility = Visibility.Hidden;
            SaveEditedAccountButton.Visibility = Visibility.Visible;
            CreateAccountButton.Visibility = Visibility.Hidden;
            CreateAccountWindow.Visibility = Visibility.Visible;

            newAccount_Store.SelectedIndex = currentAccount.StoreIndex;
            newAccount_Email.Text = currentAccount.Email;
            newAccount_Password.Text = currentAccount.Password;
        }

        private void SaveEditedAccount_CLick(object sender, RoutedEventArgs e)
        {
            GatherAccountInfos(true);
            CloseCreateAccountWindow();
        }

        private void DeleteSpecificAccount_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            currentAccount = button.CommandParameter as Account;
            bool anyAccountInUse = false;

            foreach (TaskInfo task in taskList)
            {
                foreach (Account Account in accountList)
                {
                    if (Account.Email == task.Account)
                    {
                        anyAccountInUse = true;
                    }
                }
            }

            if (!anyAccountInUse)
            {
                confirm = Confirm.DeleteSpecificAccount;
                AccountPageOptions.Visibility = Visibility.Hidden;
                AccountPageList.Visibility = Visibility.Hidden;
                ConfirmDeleteWindow.Visibility = Visibility.Visible;
                ConfirmDeleteLabel.Content = "Do you want to delete this Account ?";
            }
            else if (anyAccountInUse)
            {
                MessageBox.Show("Cannot delete this Account because it is used in a task");
            }
        }

        private void DeleteSpecificAccount()
        {
            accountList.Remove(currentAccount);
            accountCounter--;
            foreach (Account Account in accountList)
            {
                if (Account.AccountId > currentAccount.AccountId)
                {
                    Account.AccountId--;
                }
            }
            RefreshAllContent();
            AccountPageOptions.Visibility = Visibility.Visible;
            AccountPageList.Visibility = Visibility.Visible;
        }

        #endregion Account List Options

        #region Other

        private void AccountsListViewSizeChanged(object sender, RoutedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth; // take into account vertical scrollbar
            var col1 = 0.05;
            var col2 = 0.30714;
            var col3 = 0.30714;
            var col4 = 0.20714;
            var col5 = 0.12714;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
            gView.Columns[3].Width = workingWidth * col4;
            gView.Columns[4].Width = workingWidth * col5;
        }

        #endregion Other

        #endregion Accounts

        #region global methods

        private void RefreshAllContent()
        {
            //tasks
            taskListView.ItemsSource = taskList;
            taskListView.Items.Refresh();
            newTask_Profile.ItemsSource = profileList;
            newTask_Profile.DisplayMemberPath = "FullName";
            newTask_Profile.SelectedValuePath = "FullName";

            //profiles
            profilesListView.ItemsSource = profileList;
            profilesListView.Items.Refresh();

            //accounts
            AccountsListView.ItemsSource = accountList;
            AccountsListView.Items.Refresh();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteWindow.Visibility = Visibility.Hidden;
            switch (confirm)
            {
                case Confirm.DeleteAllTasks:
                    DeleteAllTasks();
                    break;

                case Confirm.DeleteSpecificTask:
                    DeleteSpecificTask();
                    break;

                case Confirm.DeleteAllProfiles:
                    DeleteAllProfiles();
                    break;

                case Confirm.DeleteSpecificProfile:
                    DeleteSpecificProfile();
                    break;

                case Confirm.DeleteAllAccounts:
                    DeleteAllAccounts();
                    break;

                case Confirm.DeleteSpecificAccount:
                    DeleteSpecificAccount();
                    break;

                default:
                    MessageBox.Show("This command does not exist yet");
                    break;
            }
        }

        private void CancelConfirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDeleteWindow.Visibility = Visibility.Hidden;

            switch (confirm)
            {
                case Confirm.DeleteAllTasks:
                    TaskPageOptions.Visibility = Visibility.Visible;
                    TaskPageList.Visibility = Visibility.Visible;
                    break;

                case Confirm.DeleteSpecificTask:
                    TaskPageOptions.Visibility = Visibility.Visible;
                    TaskPageList.Visibility = Visibility.Visible;
                    break;

                case Confirm.DeleteAllProfiles:
                    ProfilePageOptions.Visibility = Visibility.Visible;
                    ProfilePageList.Visibility = Visibility.Visible;
                    break;

                case Confirm.DeleteSpecificProfile:
                    ProfilePageOptions.Visibility = Visibility.Visible;
                    ProfilePageList.Visibility = Visibility.Visible;
                    break;

                case Confirm.DeleteAllAccounts:
                    AccountPageOptions.Visibility = Visibility.Visible;
                    AccountPageList.Visibility = Visibility.Visible;
                    break;

                case Confirm.DeleteSpecificAccount:
                    AccountPageOptions.Visibility = Visibility.Visible;
                    AccountPageList.Visibility = Visibility.Visible;
                    break;

                default:
                    MessageBox.Show("This command does not exist yet");
                    break;
            }
        }

        #endregion global methods
    }
}