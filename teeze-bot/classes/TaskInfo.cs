namespace teeze_bot.classes
{
    public class TaskInfo
    {
        public int TaskId;
        public string Store;
        public string ShoeSizes;
        public string Product;
        public string Profile;
        public string Proxy;
        public string Account;
        public string Status;

        public void AddInfos(int taskId, string store, string shoeSize, string product, string profile, string proxy, string account)
        {
            TaskId = taskId;
            Store = store;
            ShoeSizes = shoeSize;
            Product = product;
            Profile = profile;
            Proxy = proxy;
            Account = account;
            Status = "waiting";
        }
    }
}