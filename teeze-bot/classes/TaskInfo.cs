using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;
using teeze_bot.classes.enums;

namespace teeze_bot.classes
{
    public class TaskInfo
    {
        public int TaskId { get; set; }
        public string Store { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StoreType storeType { get; set; }

        public string Productname { get; set; }
        public string ProductLink { get; set; }
        public string ShoeSizes { get; set; }
        public string Profile { get; set; }
        public int ProfileIndex { get; set; }
        public string Proxy { get; set; }
        public int ProxyIndex { get; set; }
        public string Account { get; set; }
        public int AccountIndex { get; set; }
        public string Status { get; set; }

        public TaskInfo()
        {
        }

        public TaskInfo(int taskId, string store, int storeIndex, string shoeSize, string productname, string productLink, string profile, int profileIndex, string proxy, int proxyIndex, string account, int accountIndex)
        {
            TaskId = taskId;
            Store = store;
            storeType = (StoreType)storeIndex;
            ShoeSizes = shoeSize;
            Productname = productname;
            ProductLink = productLink;
            Profile = profile;
            ProfileIndex = profileIndex;
            Proxy = proxy;
            ProxyIndex = proxyIndex;
            Account = account;
            AccountIndex = accountIndex;
            Status = "waiting";
        }

        public void UpdateInfo(int taskId, string store, int storeIndex, string shoeSize, string productname, string productLink, string profile, int profileIndex, string proxy, int proxyIndex, string account, int accountIndex)
        {
            TaskId = taskId;
            Store = store;
            storeType = (StoreType)storeIndex;
            ShoeSizes = shoeSize;
            Productname = productname;
            ProductLink = productLink;
            Profile = profile;
            ProfileIndex = profileIndex;
            Proxy = proxy;
            ProxyIndex = proxyIndex;
            Account = account;
            AccountIndex = accountIndex;
            Status = "waiting";
        }
    }
}