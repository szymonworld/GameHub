using System.Json;

namespace GameHub
{
    class Account
    {
        public int AccountID { get; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool Microphone { get; set; }
        public string ProfilePicture { get; set; }
        public string Status { get; set; }
        public string Language { get; set; }
        public double RepPoint { get; set; }

        public Account(int AccountID, string LastName, string FirstName, string Login, string Password, string Email, string Description,
                       bool Microphone, string ProfilePicture, string Status, string Language, double RepPoint)
        {
            this.AccountID = AccountID;
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Login = Login;
            this.Password = Password;
            this.Email = Email;
            this.Description = Description;
            this.Microphone = Microphone;
            this.ProfilePicture = ProfilePicture;
            this.Status = Status;
            this.Language = Language;
            this.RepPoint = RepPoint;
        }
        public Account()
        {
            this.AccountID = 0;
            this.LastName = null;
            this.FirstName = null;
            this.Login = null;
            this.Password = null;
            this.Email = null;
            this.Description = null;
            this.Microphone = false;
            this.ProfilePicture = null;
            this.Status = null;
            this.Language = null;
            this.RepPoint = 0;
        }
        public Account(JsonValue json)
        {
            this.AccountID = json["AccountID"];
            this.LastName = json["LastName"];
            this.FirstName = json["FirstName"];
            this.Login = json["Login"];
            this.Password = json["Password"];
            this.Email = json["Email"];
            this.Description = json["Description"];
            this.Microphone = json["Microphone"];
            this.ProfilePicture = json["ProfilePicture"];
            this.Status = json["Status"];
            this.Language = json["Language"];
            this.RepPoint = json["RepPoint"];
        }
    }

}