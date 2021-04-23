using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using teeze_bot.classes;

namespace teeze_bot
{
    public partial class MainWindow : Window
    {
        private int runningTasks = 0;
        private List<TaskInfo> taskList = new List<TaskInfo>();
        private List<Profile> profileList = new List<Profile>();
        private TaskInfo editedTask = new TaskInfo();
        private List<TitoloTask> titoloTasks = new List<TitoloTask>();

        private int taskIdCounter = 0;
        private int profileCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            TeezeOpened();
        }

        #region Teeze Opened and Closed

        public void TeezeOpened()
        {
            ReadTasksFromJSON();
            ReadProfilesFromJSON();
        }

        private void TeezeClosed(object sender, CancelEventArgs e)
        {
            SaveTasksToJSON();
            SaveProfilesToJSON();
        }

        #endregion Teeze Opened and Closed

        #region BasicFeatures

        private void close_app_click(object sender, RoutedEventArgs e)
        {
            bool anyTaskInProgress = false;
            bool noRunningTaskFound = false;

            while (anyTaskInProgress == false && noRunningTaskFound == false)
            {
                foreach (TitoloTask titoloTask in titoloTasks)
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
            string dateCreated = DateTime.Now.ToString("M-d-yyyy");
            profileCounter++;
            profileList.Add(new Profile(profileCounter, firstname, lastname, eMail, phone, address1, address2, city, zip, country, dateCreated));
            profilesListView.ItemsSource = profileList;
            profilesListView.Items.Refresh();
        }

        private void CloseCreateProfileWindow()
        {
            CreateProfileWindow.Visibility = Visibility.Hidden;
            ProfilePageOptions.Visibility = Visibility.Visible;
            ProfilePageList.Visibility = Visibility.Visible;
        }

        private void DeleteAllProfiles(object sender, RoutedEventArgs e)
        {
            profileList.Clear();
            SaveProfilesToJSON();
            profileCounter = 0;
            profilesListView.ItemsSource = null;
            profilesListView.Items.Clear();
            profilesListView.Items.Refresh();
        }

        private void SaveProfilesToJSON()
        {
            File.WriteAllText(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userProfiles.json", JsonConvert.SerializeObject(profileList, Formatting.Indented));
        }

        public void ReadProfilesFromJSON()
        {
            using (StreamReader r = new StreamReader(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userProfiles.json"))
            {
                string json = r.ReadToEnd();
                profileList = JsonConvert.DeserializeObject<List<Profile>>(json);
            }
            profileCounter = 0;
            profilesListView.ItemsSource = profileList;
            profileCounter = profileList.Count;
            profilesListView.Items.Refresh();
        }

        public void ProfileListViewSizeChanged(object sender, RoutedEventArgs e)
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

        #endregion Create Profile

        #region Task

        #region General Options

        private void DeleteAllOption_Click(object sender, RoutedEventArgs e)
        {
            if (taskIdCounter == 0)
            {
                MessageBox.Show("There are no tasks to delete");
            }
            else
            {
                bool anyTaskInProgress = false;

                while (anyTaskInProgress == false)
                {
                    foreach (TitoloTask titoloTask in titoloTasks)
                    {
                        if (!anyTaskInProgress)
                        {
                            anyTaskInProgress = titoloTask.InProgress;
                        }
                    }
                }

                if (!anyTaskInProgress)
                {
                    taskList.Clear();
                    SaveTasksToJSON();
                    taskIdCounter = 0;
                    taskListView.ItemsSource = null;
                    taskListView.Items.Clear();
                    taskListView.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("There are still tasks running. Please end them first");
                }
            }
        }

        private void StopAllOptions_Click(object sender, RoutedEventArgs e)
        {
            if (taskIdCounter == 0)
            {
                MessageBox.Show("There are no tasks to delete");
            }
            else
            {
                foreach (TaskInfo task in taskList)
                {
                }
                foreach (TitoloTask titoloTask in titoloTasks)
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

        #endregion General Options

        #region Task Options

        private void editTask(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            TaskInfo task = button.CommandParameter as TaskInfo;
            if (!titoloTasks[task.TaskId - 1].InProgress)
            {
                editedTask = button.CommandParameter as TaskInfo;
                CreateTaskLabel.Content = "Task " + editedTask.TaskId.ToString();
                TaskPageList.Visibility = Visibility.Hidden;
                TaskPageOptions.Visibility = Visibility.Hidden;
                EditTaskButton.Visibility = Visibility.Visible;
                CreateTaskButton.Visibility = Visibility.Hidden;
                CreateTaskWindow.Visibility = Visibility.Visible;

                newTask_Store.SelectedIndex = -1;
                newTask_Sizes.Text = editedTask.ShoeSizes;
                newTask_Productname.Text = editedTask.Productname;
                newTask_Product.Text = editedTask.Product;
                newTask_Profile.SelectedIndex = -1;
                newTask_Proxy.SelectedIndex = -1;
                newTask_Account.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Cannot edit a Task which is running");
            }
        }

        public void SaveEditedTask_Click(object sender, RoutedEventArgs e)
        {
            GatherTaskInfo(true);
            CloseCreateTaskWindow();
        }

        public void DeleteSpecificTask(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            TaskInfo task = button.CommandParameter as TaskInfo;
            if (!titoloTasks[task.TaskId - 1].InProgress)
            {
                taskList.Remove(task);
                taskListView.ItemsSource = taskList;
                taskListView.Items.Refresh();
                taskIdCounter--;
                foreach (TaskInfo deletedTaskList in taskList)
                {
                    if (deletedTaskList.TaskId > task.TaskId)
                    {
                        deletedTaskList.TaskId--;
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot delete a Task which is running");
            }
        }

        public void StartOrEndTask(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            TaskInfo task = button.CommandParameter as TaskInfo;
            if (button.Content.ToString() == "Start")
            {
                button.Content = "End";
                switch (task.Store)
                {
                    case "Titolo":
                        titoloTasks[task.TaskId - 1].StartTask();
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
                        titoloTasks[task.TaskId - 1].QuitTask();
                        break;

                    default:
                        break;
                }
                runningTasks--;
                RunningTaskLabel.Content = runningTasks.ToString();
            }
        }

        #endregion Task Options

        #region Create Task

        private void CreateTaskOption_Click(object sender, RoutedEventArgs e)
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
            EditTaskButton.Visibility = Visibility.Hidden;
            newTask_AccountLabel.Visibility = Visibility.Visible;
            newTask_Account.Visibility = Visibility.Visible;
            newTask_errorStore.Visibility = Visibility.Hidden;
            newTask_errorSizes.Visibility = Visibility.Hidden;
            newTask_errorProduct.Visibility = Visibility.Hidden;
            newTask_errorProductname.Visibility = Visibility.Hidden;
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
            newTask_errorProfile.Visibility = newTask_Profile.SelectedIndex == -1 ? Visibility.Visible : Visibility.Hidden;
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
            string ShoeSizes = (newTask_Sizes.Text.ToString());
            string Productname = newTask_Productname.Text.ToString();
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
            if (newTask_Account.Visibility == Visibility.Hidden)
            {
                Account = "";
            }
            if (!isEdited)
            {
                taskIdCounter++;
                taskList.Add(new TaskInfo(taskIdCounter, Store, ShoeSizes, Productname, Product, Profile, Proxy, Account));
                titoloTasks.Add(new TitoloTask() { taskinfo = taskList[taskIdCounter - 1] });
            }
            if (isEdited)
            {
                taskList[editedTask.TaskId - 1].UpdateInfo(editedTask.TaskId, Store, ShoeSizes, Productname, Product, Profile, Proxy, Account);
            }
            taskListView.ItemsSource = taskList;
            taskListView.Items.Refresh();
        }

        private void SaveTasksToJSON()
        {
            File.WriteAllText(@"C:\Users\Dario\source\repos\teeze-bot\teeze-bot\Data\userTasks.json", JsonConvert.SerializeObject(taskList, Formatting.Indented));
        }

        public void ReadTasksFromJSON()
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
                titoloTasks.Add(new TitoloTask() { taskinfo = task });
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

        #endregion Task
    }
}