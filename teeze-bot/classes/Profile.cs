namespace teeze_bot.classes
{
    public class Profile
    {
        public int ProfileNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FullName { get; set; }
        public string EMail { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZIP { get; set; }
        public string Country { get; set; }
        public int CountryIndex { get; set; }
        public string DateCreated { get; set; }

        public Profile()
        {
        }

        public Profile(int profileNumber, string firstname, string lastname, string eMail, string phone, string address1, string address2, string city, string zip, string country, int countryIndex, string dateCreated)
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
            CountryIndex = countryIndex;
            DateCreated = dateCreated;
            FullName = string.Join(" ", firstname, lastname);
        }

        public void UpdateInfo(int profileNumber, string firstname, string lastname, string eMail, string phone, string address1, string address2, string city, string zip, string country, int countryIndex, string dateCreated)
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
            CountryIndex = countryIndex;
            DateCreated = dateCreated;
            FullName = string.Join(" ", firstname, lastname);
        }
    }
}