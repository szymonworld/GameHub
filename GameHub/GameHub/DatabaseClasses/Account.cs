using System.Json;
using System;

namespace GameHub
{
    public class Account
    {
        public int AccountID { get; set; }
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
            this.LastName = "";
            this.FirstName = "";
            this.Login = "";
            this.Password = "";
            this.Email = "";
            this.Description = "";
            this.Microphone = false;
            this.ProfilePicture = "";
            this.Status = "";
            this.Language = "";
            this.RepPoint = 0;
        }
        public Account(JsonValue json) //TODO: jakis problem z odczytem majka
        {
            this.AccountID = json["AccountID"];
            this.LastName = json["LastName"];
            this.FirstName = json["FirstName"];
            this.Login = json["Login"];
            //this.Password = json["Password"];
            this.Email = json["Email"];
            this.Description = json["Description"];
          //  this.Microphone = json["Microphone"];
            this.ProfilePicture = json["ProfilePicture"];
            this.Status = json["Status"];
            this.Language = json["Language"];
            this.RepPoint = json["RepPoint"];
        }
    }

}