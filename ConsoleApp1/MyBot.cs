using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class MyBot
    {
        DiscordClient client;
        CommandService commands;

        public MyBot()
        {
            var _client = new DiscordClient();
            serverStatusProcess();
            client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            commands = client.GetService<CommandService>();

            Response();

            AllDeleteMessages();

            JoinedBannedLeftMessage();

            client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    await client.Connect("MzYxODU3MDg4OTExMTc5Nzc3.DMUkww.LKO969s0xTmeVpCh3IITRPL1Og4", TokenType.Bot);
                    break;
                }
            });
        }

        private void AllDeleteMessages()
        {
            
            commands.CreateCommand("clear").Parameter("messages", ParameterType.Multiple).Do(async(e) =>
            {
                int server = 0;
                server = Convert.ToInt32(e.Args[0]);
                server = server + 1;
                Message[] messages;
                messages = await e.Channel.DownloadMessages(server);
                await e.Channel.DeleteMessages(messages);
            });
        }

        private void Response()
        {
            commands.CreateCommand("hi").Do(async (e) =>
            {
                if (e.User.Name.Contains("CharizardRaged") || e.User.Name.Contains("Precious"))
                    await e.Channel.SendMessage("Hi Master :heart:");
                else
                    await e.Channel.SendMessage("Hi Master" + e.User.NicknameMention + " :heart:");
            });
            commands.CreateCommand("hello").Do(async (e) =>
            {
                if (e.User.Name.Contains("CharizardRaged") || e.User.Name.Contains("Precious"))
                    await e.Channel.SendMessage("Hi Master :heart:");
                else
                    await e.Channel.SendMessage("Hi Master" + e.User.NicknameMention + " :heart:");
            });
            commands.CreateCommand("yo").Do(async (e) =>
            {
                
                if (e.User.Name.Contains("CharizardRaged") || e.User.Name.Contains("Precious"))
                    await e.Channel.SendMessage("Hi Master :heart:");
                else
                    await e.Channel.SendMessage("Hi Master" + e.User.NicknameMention + " :heart:");
            });
            commands.CreateCommand("halo").Do(async (e) =>
            {               
                if(e.User.Name.Contains("CharizardRaged") || e.User.Name.Contains("Precious"))
                    await e.Channel.SendMessage("Hi Master :heart:");
                else
                    await e.Channel.SendMessage("Hi Master " + e.User.NicknameMention + " :heart:");
            });
            
        }

        private void JoinedBannedLeftMessage()
        {
            client.UserJoined += async (s, e) =>
            {
                var channel = e.Server.FindChannels("testchats", ChannelType.Text).FirstOrDefault();
                var name = e.User;
                await channel.SendMessage(string.Format("{0} has joined the server.", name.Name));
            };

            client.UserLeft += async (s, e) =>
            {
                var channel = e.Server.FindChannels("testchats", ChannelType.Text).FirstOrDefault();
                var name = e.User;
                await channel.SendMessage(string.Format("{0} has left the server.", name.Name));
            };

            client.UserBanned += async (s, e) =>
            {
                var channel = e.Server.FindChannels("testchats", ChannelType.Text).FirstOrDefault();
                var name = e.User;
                await channel.SendMessage(string.Format("{0} was banned from the server.", name.Name));
            };

            client.UserUnbanned += async (s, e) =>
            {
                var channel = e.Server.FindChannels("testchats", ChannelType.Text).FirstOrDefault();
                var name = e.User;
                await channel.SendMessage(string.Format("{0} was unbanned from the server", name.Name));
            };
        }
        private async void serverStatusProcess()
        {
            var url = "https://pokemon-revolution-online.net/serverstatus.php";
            string redServer = "";
            string blueServer = "";
            string yellowServer = "";
            string serverStatusRed = "";
            string serverStatusBlue = "";
            string serverStatusYellow = "";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(html);

            var serverNames = htmlDocument.DocumentNode.Descendants("td").Where(node => node.GetAttributeValue("class", "").Equals("auto-style12")).ToList();
            var status = htmlDocument.DocumentNode.Descendants("td").Where(node => node.GetAttributeValue("class", "").Equals("auto-style16")).ToList();

            

            string xd = "";
            string xd2 = "";
            string xd3 = "";
            xd = status[0].InnerHtml;
            xd2 = status[1].InnerHtml;
            xd3 = status[2].InnerHtml;
            Regex tester = new Regex(">.*</");
            Match matchTester1 = tester.Match(xd);
            Match matchTester2 = tester.Match(xd2);
            Match matchTester3 = tester.Match(xd3);
            string lol = matchTester1.Value;
            string lol2 = matchTester2.Value;
            string lol3 = matchTester3.Value;
            lol = lol.Replace(">", "");
            lol = lol.Replace("</", "");
            lol2 = lol2.Replace(">", "");
            lol2 = lol2.Replace("</", "");
            lol3 = lol3.Replace(">", "");
            lol3 = lol3.Replace("</", "");

            redServer = serverNames[0].InnerText;
            blueServer = serverNames[1].InnerText;
            yellowServer = serverNames[2].InnerText;

            redServer = redServer.Replace("Server Status:", "");
            blueServer = blueServer.Replace("Server Status:", "");
            yellowServer = yellowServer.Replace("Server Status:", "");

            serverStatusRed = lol;
            serverStatusBlue = lol2;
            serverStatusYellow = lol3;

            redServer += serverStatusRed;
            blueServer += serverStatusBlue;
            yellowServer += serverStatusYellow;

            commands.CreateCommand("status").Do(async (e) =>
            {
                 await e.Channel.SendMessage("```" + redServer + "\n" + blueServer + "\n" + yellowServer + "```");
            });
        }
        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message + "!");
        }
    }
}
