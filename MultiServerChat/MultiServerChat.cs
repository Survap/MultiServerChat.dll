using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using HttpServer;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Rests;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace MultiServerChat
{
    [ApiVersion(2, 1)]
    public class MultiServerChat : TerrariaPlugin
    {
        public static ConfigFile Config = new ConfigFile();
        private string savePath;

        public override string Author => "Zack Piispanen, now maintained and updated by Ryozuki";
        public override string Description => "Facilitate chat between servers.";
        public override string Name => "Multiserver Chat";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public MultiServerChat(Main game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            savePath = Path.Combine(TShock.SavePath, "multiserverchat.json");
            Config = ConfigFile.Read(savePath);
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            GeneralHooks.ReloadEvent += OnReload;
            PlayerHooks.PlayerChat += OnChat;
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin, 10);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave, 10);
            TShock.RestApi.Register(new SecureRestCommand("/msc", RestChat, "msc.canchat"));
            TShock.RestApi.Register(new SecureRestCommand("/jl", RestChat, "msc.canchat"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PlayerHooks.PlayerChat -= OnChat;
                GeneralHooks.ReloadEvent -= OnReload;
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
                ServerApi.Hooks.ServerLeave.Deregister(this, OnLeave);
            }
            base.Dispose(disposing);
        }

        private void OnInitialize(EventArgs args)
        {
            Commands.ChatCommands.Add(new Command("msc.reload", ReloadCmd, "msc_reload")
            {
                HelpText = string.Format("Usage: {0}msc_reload", TShock.Config.CommandSpecifier)
            });
        }

        private void ReloadCmd(CommandArgs args)
        {
            Config = ConfigFile.Read(savePath);
        }

        private void OnReload(ReloadEventArgs args)
        {
            if (args.Player.Group.HasPermission("msc.reload"))
            {
                Config = ConfigFile.Read(savePath);
            }
        }

        private object RestChat(RestRequestArgs args)
        {
            if (!Config.DisplayChat)
                return new RestObject();

            RestHelper.RecievedMessage(args);

            return new RestObject();
        }

        private void OnChat(PlayerChatEventArgs args)
        {
            if (!Config.SendChat)
                return;
            if (args.Handled)
                return;

            RestHelper.SendChatMessage(args.Player, args.TShockFormattedText);
        }

        private void OnJoin(JoinEventArgs args)
        {
            if (!Config.DisplayJoinLeave)
                return;

            TSPlayer ply = TShock.Players[args.Who];
            if (ply == null)
                return;

            RestHelper.SendJoinMessage(ply);
        }

        private void OnLeave(LeaveEventArgs args)
        {
            if (!Config.DisplayJoinLeave)
                return;

            TSPlayer ply = TShock.Players[args.Who];
            if (ply == null)
                return;

            RestHelper.SendLeaveMessage(ply);
        }
    }
}
