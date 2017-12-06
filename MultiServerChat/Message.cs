using Newtonsoft.Json;

namespace MultiServerChat
{
    public class Message
    {
        public string Text { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Message FromJson(string js)
        {
            return JsonConvert.DeserializeObject<Message>(js);
        }
    }
}
