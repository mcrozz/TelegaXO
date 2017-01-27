using System;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace Server
{
    class Telegram
    {
        public Telegram()
        {
            if (String.IsNullOrEmpty(_token))
                throw new Exception(TelegramTokenNotPrivided);
        }

        public static void Send(DTO.Message message)
        {
            _pendingSending.Add(message);
        }

        public static void Receiver()
        {
            while (!_terminate)
            {
                while (_pendingSending.Count != 0)
                {
                    // Deliver messages
                    DTO.Message message = _pendingSending[0];
                    _pendingSending.RemoveAt(0);

                    Console.WriteLine($"Sending message to {message.User.username}");
                    String uri = String.Format(TelegramEnterPoint, _token, TelegramSendMessage);
                    String parameters = String.Format(TelegramSendMessageFormat, message.User.id, HttpUtility.UrlEncode(message.Text));
                    // TODO: add custom keyboard
                    WebRequest request = WebRequest.Create(String.Concat(uri, parameters));
                    request.Method = "POST";
                    request.GetResponse();
                }

                using (WebClient request = new WebClient())
                {
                    Console.WriteLine("Sending long poll /getUpdates");
                    String uri = String.Format(TelegramEnterPoint, _token, TelegramGetUpdates);
                    request.Headers.Add(HttpRequestHeader.Accept, "application/json");
                    String parameters = String.Format(TelegramGetUpdatesFormat, _offset, Interval / 1000);
                    String response = request.DownloadString(String.Concat(uri, parameters));
                    ParseResponse(response);
                }

                //Thread.Sleep(Interval);
            }
            _thread = null;
        }

        public static void Start(ref Telegram instance)
        {
            if (_thread != null)
                return;
            _thread = new Thread(Telegram.Receiver);
            _thread.Start();
        }

        public static void Stop()
        {
            _terminate = true;
        }

        private static void ParseResponse(String data)
        {
            DTO.Telegram.GetUpdates getUpdates = JsonConvert.DeserializeObject<DTO.Telegram.GetUpdates>(data);
            if (!getUpdates.ok)
                throw new Exception("Method /getUpdates failed");

            foreach (DTO.Telegram.Update message in getUpdates.result)
            {
                if (_offset <= message.update_id)
                    _offset = message.update_id + 1;

                Console.WriteLine($"[{message.message.@from.username}]: {message.message.text}");

                if (message.message.text.Equals("Hey"))
                {
                    DTO.Message reply = new DTO.Message();
                    reply.User = message.message.@from;
                    reply.Text = "Good day";
                    Telegram.Send(reply);
                }
            }
        }

        private static Int32 _offset = 0;
        private static IList<DTO.Message> _pendingSending = new List<DTO.Message>();

        private static readonly String _token = ConfigurationManager.AppSettings[TokenKey];
        private const String TokenKey = "TELEGRAM_TOKEN";

        private static Thread _thread = null;
        private static Boolean _terminate = false;
        private const Int32 Interval = 1000;

        private const String TelegramEnterPoint = "https://api.telegram.org/bot{0}/{1}";
        private const String TelegramGetUpdates = "getUpdates";
        private const String TelegramGetUpdatesFormat = "?offset={0}&timeout={1}";
        private const String TelegramSendMessage = "sendMessage";
        private const String TelegramSendMessageFormat = "?chat_id={0}&text={1}";

        private const String TelegramTokenNotPrivided = "В App.config не вписан токен Telegram бота";
    }
}
