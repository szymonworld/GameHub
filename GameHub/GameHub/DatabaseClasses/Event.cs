using System.Json;
namespace GameHub
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string GameID { get; set; }
        public string Description { get; set; }
        public string Datee { get; set; }
        public string Hourr { get; set; }
        public string Timespan { get; set; }
        public bool Public { get; set; }
        public string Platform { get; set; }
        public int Slots { get; set; }
        public bool Microphone { get; set; }

        public Event()
        {

        }

        public Event(JsonValue json)
        {
            EventID = json["EventID"];
            EventName = json["EventName"];
            GameID = json["GameID"];
            Description = json["Description"];
            Datee = json["Datee"];
            Hourr = json["Hourr"];
            Timespan = json["Timespan"];
          //  Public = json["Public"];
            Platform = json["Platform"];
            Slots = json["Slots"];
          //  Microphone = json["Microphone"];
        }
    }
}