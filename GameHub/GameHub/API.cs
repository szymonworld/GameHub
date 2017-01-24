using System;
using System.Threading.Tasks;
using System.Net;
using System.Json;
using System.IO;

namespace GameHub
{
    static class API
    {
        private static string Apiurl = "http://2bdesign.lh.pl/all/API/db_request.php";
        public static Task<bool> checkForInternetConnection()
        {
            return Task<bool>.Run(() => {
                try
                {
                    using (var client = new WebClient())
                    using (var stream = client.OpenRead(Apiurl))
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            });
        }
        public static async Task<Account> getAccountByEmail(string email)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(Apiurl + "?type=1&email=" + email));

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                        return json["success"] == 1 ? new Account(json) : null;
                    }
                }

            }
            catch
            {
                
                return null;
            }
        }
        public static async Task<LinkAccount> getLinkAccounts(string email, string password)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(Apiurl + "?type=16&email=" + email + "&password=" + password));

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                        return json["success"] == 1 ? new LinkAccount(json) : null;
                    }
                }

            }
            catch
            {
                return null;
            }
        }
        public static async Task<bool> authentification(string email, string password)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(Apiurl + "?type=13&email=" + email + "&password=" + password));

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                        return json["success"] == 1 ? true : false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> isAccountExist(string email)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(Apiurl + "?type=14&email=" + email));

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                        return json["success"] == 1 ? true : false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> createAccount(Account account/*, LinkAccount links*/)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.Password);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(Apiurl + "?type=0&email=" + account.Email + "&password=" + hashedPassword + "&login=" + account.Login + "&firstname=" + account.FirstName + "&lastname=" + account.LastName + "&mic=" + account.Microphone + "&status=" + account.Status + "&pic=" + account.ProfilePicture + "&lang=" + account.Language + "&rep=" + account.RepPoint));
               //                                                             createAccount($_GET['firstname'], $_GET['lastname'], $_GET['email'], $_GET['mic'], $_GET['pic'], $_GET['status'], $_GET['lang'], $_GET['rep'],$_GET['psn'], 
              //                                                                  $_GET['xbox'], $_GET['steam'], $_GET['origin'], $_GET['discord'], $_GET['uplay'], $_GET['battle'], $_GET['lol'], $_GET['skype']);
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(stream));
                        return json["success"] == 1 ? true : false;
                    }
                }

            }
            catch
            {
                return false;
            }
        }

    }
}