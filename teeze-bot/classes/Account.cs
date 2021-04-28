namespace teeze_bot.classes
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Store { get; set; }
        public int StoreIndex { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Account()
        {
        }

        public Account(int accountId, string store, int storeIndex, string email, string password)
        {
            AccountId = accountId;
            Store = store;
            StoreIndex = storeIndex;
            Email = email;
            Password = password;
        }

        public void UpdateInfo(int accountId, string store, int storeIndex, string email, string password)
        {
            AccountId = accountId;
            Store = store;
            StoreIndex = storeIndex;
            Email = email;
            Password = password;
        }
    }
}