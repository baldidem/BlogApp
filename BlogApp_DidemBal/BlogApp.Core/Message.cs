using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogApp.Core
{
    public class Message
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AlertType { get; set; }
        public static string CreateMessage(string title, string description, string alertType)
        {
            Message message = new Message()
            {
                Title = title,
                Description = description,
                AlertType = alertType
            };
            return JsonConvert.SerializeObject(message);
        }

    }
}
