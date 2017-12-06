using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.Xna.Framework;
using Rests;
using TShockAPI;

namespace MultiServerChat
{
    public static class RestHelper
    {
        public static void SendChatMessage(TSPlayer ply, string formatted_text)
        {
            ThreadPool.QueueUserWorkItem(f =>
            {
                bool failure = false;
                var message = new Message()
                {
                    Text = String.Format(MultiServerChat.Config.ChatFormat,
                                            TShock.Config.ServerName,
                                            formatted_text),
                    Red = ply.Group.R,
                    Green = ply.Group.G,
                    Blue = ply.Group.B
                };

                var bytes = Encoding.UTF8.GetBytes(message.ToString());
                var base64 = Convert.ToBase64String(bytes);
                var encoded = HttpUtility.UrlEncode(base64);
                foreach (var url in MultiServerChat.Config.RestURLs)
                {
                    var uri = String.Format("{0}/jl?message={1}&token={2}", url, encoded, MultiServerChat.Config.Token);

                    try
                    {
                        var request = (HttpWebRequest)WebRequest.Create(uri);
                        using (var res = request.GetResponse())
                        {
                        }
                        failure = false;
                    }
                    catch (Exception)
                    {
                        if (!failure)
                        {
                            TShock.Log.Error("Failed to make request to other server, server is down?");
                            failure = true;
                        }
                    }
                }
            });
        }

        public static void SendJoinMessage(TSPlayer ply)
        {
            ThreadPool.QueueUserWorkItem(f =>
            {
                bool failure = false;
                var message = new Message()
                {
                    Text =
                        String.Format(MultiServerChat.Config.JoinFormat, TShock.Config.ServerName, ply.Name),
                    Red = Color.Yellow.R,
                    Green = Color.Yellow.G,
                    Blue = Color.Yellow.B
                };

                var bytes = Encoding.UTF8.GetBytes(message.ToString());
                var base64 = Convert.ToBase64String(bytes);
                var encoded = HttpUtility.UrlEncode(base64);
                foreach (var url in MultiServerChat.Config.RestURLs)
                {
                    var uri = String.Format("{0}/jl?message={1}&token={2}", url, encoded, MultiServerChat.Config.Token);

                    try
                    {
                        var request = (HttpWebRequest)WebRequest.Create(uri);
                        using (var res = request.GetResponse())
                        {
                        }
                        failure = false;
                    }
                    catch (Exception)
                    {
                        if (!failure)
                        {
                            TShock.Log.Error("Failed to make request to other server, server is down?");
                            failure = true;
                        }
                    }
                }
            });
        }

        public static void SendLeaveMessage(TSPlayer ply)
        {
            ThreadPool.QueueUserWorkItem(f =>
            {
                bool failure = false;
                var message = new Message()
                {
                    Text =
                        String.Format(MultiServerChat.Config.LeaveFormat, TShock.Config.ServerName, ply.Name),
                    Red = Color.Yellow.R,
                    Green = Color.Yellow.G,
                    Blue = Color.Yellow.B
                };

                var bytes = Encoding.UTF8.GetBytes(message.ToString());
                var base64 = Convert.ToBase64String(bytes);
                var encoded = HttpUtility.UrlEncode(base64);
                foreach (var url in MultiServerChat.Config.RestURLs)
                {
                    var uri = String.Format("{0}/jl?message={1}&token={2}", url, encoded, MultiServerChat.Config.Token);

                    try
                    {
                        var request = (HttpWebRequest)WebRequest.Create(uri);
                        using (var res = request.GetResponse())
                        {
                        }
                        failure = false;
                    }
                    catch (Exception)
                    {
                        if (!failure)
                        {
                            TShock.Log.Error("Failed to make request to other server, server is down?");
                            failure = true;
                        }
                    }
                }
            });
        }

        public static void RecievedMessage(RestRequestArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.Parameters["message"]))
            {
                try
                {
                    var decoded = HttpUtility.UrlDecode(args.Parameters["message"]);
                    var bytes = Convert.FromBase64String(decoded);
                    var str = Encoding.UTF8.GetString(bytes);
                    var message = Message.FromJson(str);
                    TShock.Utils.Broadcast(message.Text, message.Red, message.Green, message.Blue);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
