namespace teeze_bot.classes
{
    public class Profile
    {
        public int ProfileNumber;
        public string Firstname;
        public string Lastname;
        public string EMail;
        public string Phone;
        public string Address1;
        public string Address2;
        public string City;
        public string ZIP;
        public string Country;
        public string DateCreated;

        public void AddProfileInfos(int profileNumber, string firstname, string lastname, string eMail, string phone, string address1, string address2, string city, string zip, string country, string dateCreated)
        {
            ProfileNumber = profileNumber;
            Firstname = firstname;
            Lastname = lastname;
            EMail = eMail;
            Phone = phone;
            Address1 = address1;
            Address2 = address2;
            City = city;
            ZIP = zip;
            Country = country;
            DateCreated = dateCreated;
        }
    }
}