using Google.Apis.Util;
using Microsoft.Net.Http.Headers;
using System.Reflection;

namespace euroma2.Models.Firebase
{
    public class Firebase_model
    {
        public int id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string msg { get; set; }
        public string image { get; set; }
        public string date { get; set; }
        public int notificationId { get; set; }
        public string? notificationName { get; set; } = "";
        public TargetType target { get; set; }


        public NotificationType notificationType { get; set; }

        public string getTarget(TargetType t) {
            switch (t) {
                case TargetType.promotionIt: return "promotion-it";
                case TargetType.promotionEn: return "promotion-en";
                case TargetType.eventsIt: return "events-it";
                case TargetType.eventsEn: return "events-en";
                case TargetType.newOpeningIt: return "newOpening-it";
                case TargetType.newOpeningEn: return "newOpening-en";
                case TargetType.none: return "";
            }
            return "";
        }

    }



    public enum TargetType
    {
        promotionIt ,
        promotionEn,
        eventsIt ,
        eventsEn ,
        newOpeningIt,
        newOpeningEn ,
        none
    }


    public enum NotificationType
    {
        promotion,
        newOpening ,
        events,
        none
    }

}
