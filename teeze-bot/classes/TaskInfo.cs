namespace teeze_bot.classes
{
    public class TaskInfo
    {
        public int TaskId { get; set; }
        public string Store { get; set; }
        public string Productname { get; set; }
        public string ProductLink { get; set; }
        public string ShoeSizes { get; set; }
        public string Profile { get; set; }
        public string Proxy { get; set; }
        public string Account { get; set; }
        public string Status { get; set; }

        public TaskInfo()
        {
        }

        public TaskInfo(int taskId, string store, string shoeSize, string productname, string productLink, string profile, string proxy, string account)
        {
            TaskId = taskId;
            Store = store;
            ShoeSizes = shoeSize;
            Productname = productname;
            ProductLink = productLink;
            Profile = profile;
            Proxy = proxy;
            Account = account;
            Status = "waiting";
        }

        public void UpdateInfo(int taskId, string store, string shoeSize, string productname, string productLink, string profile, string proxy, string account)
        {
            TaskId = taskId;
            Store = store;
            ShoeSizes = shoeSize;
            Productname = productname;
            ProductLink = productLink;
            Profile = profile;
            Proxy = proxy;
            Account = account;
            Status = "waiting";
        }
    }
}