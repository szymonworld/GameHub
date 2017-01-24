using System.Json;
using System;
public class LinkAccount
{
    public int LinkAccountID { get; set; }
    public int AccountID { get; set; }
    public string PSN_Account { get; set; }
    public string XBOX_Account { get; set; }
    public string STEAM_Account { get; set; }
    public string ORIGIN_Account { get; set; }
    public string DISCORD_Account { get; set; }
    public string UPLAY_Account { get; set; }
    public string BATTLE_Account { get; set; }
    public string LOL_Account { get; set; }
    public string SKYPE_Account { get; set; }

    public LinkAccount(JsonValue json)
    {
        LinkAccountID = 0;
        AccountID = 0;
        PSN_Account = "";
        XBOX_Account = "";
        STEAM_Account = "";
        ORIGIN_Account = "";
        DISCORD_Account = "";
        UPLAY_Account = "";
        BATTLE_Account = "";
        LOL_Account = "";
        SKYPE_Account = "";
        LinkAccountID = json["LinkAccountID"];
        AccountID = json["AccountID"];
        PSN_Account = json["PSN_Account"];
        XBOX_Account = json["XBOX_Account"];
        STEAM_Account = json["STEAM_Account"];
        ORIGIN_Account = json["ORIGIN_Account"];
        DISCORD_Account = json["DISCORD_Account"];
        UPLAY_Account = json["UPLAY_Account"];
        BATTLE_Account = json["BATTLE_Account"];
        LOL_Account = json["LOL_Account"];
        SKYPE_Account = json["SKYPE_Account"];
    }

    public LinkAccount()
    {

    }

}