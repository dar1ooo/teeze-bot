using System;
using System.Collections.Generic;
using System.Text;

namespace teeze_bot.classes
{
    public class TaskInfo
    {
        public int TaskId;
        public string Store;
        public double ShoeSize;
        public string Product;
        public string Profile;
        public string Proxy;
        public string Account;

        public TaskInfo(int taskId, string store, double shoeSize, string product, string profile, string proxy, string account)
        {
            TaskId = taskId;
            Store = store;
            ShoeSize = shoeSize;
            Product = product;
            Profile = profile;
            Proxy = proxy;
            Account = account;
        }
    }
}
