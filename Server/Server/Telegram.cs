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
            // Throw off dublicates
            if (_pendingSending.Contains(message))
                return;

            _pendingSending.Add(message);
        }

        public static void Start(ref Telegram instance)
        {
            if (_thread != null && _terminate)
                return;
            _terminate = false;
            _thread = new Thread(Telegram.Receiver);
            _thread.Start();
        }

        public static void Stop()
        {
            _terminate = true;
        }

        private static void Receiver()
        {
            while (!_terminate)
            {
                while (_pendingSending.Count != 0)
                {
                    // Deliver messages
                    DTO.Message message = _pendingSending[0];
                    _pendingSending.RemoveAt(0);

                    if (message.AnswerCallbackQuery != null)
                        AnswerQuery(message.AnswerCallbackQuery);
                    else
                        SendMessage(message);
                }

                GetUpdates();

                Thread.Sleep(Interval);
            }
            _thread = null;
        }

        private static void GetResponse(String uri, String method)
        {
            WebRequest request = WebRequest.Create(uri);
            request.Method = method;
            request.GetResponseAsync();
        }

        private static void SendMessage(DTO.Message message)
        {
            Console.WriteLine($"Sending message to {message.User.username}");
            String uri = String.Format(TelegramEnterPoint, _token, TelegramSendMessage);
            String parameters = String.Format(TelegramSendMessageFormat, message.User.id, HttpUtility.UrlEncode(message.Text));

            if (message.KeyboardMarkup != null)
            {
                // Serialize keyboard to JSON object
                String serializedKeyboard = JsonConvert.SerializeObject(message.KeyboardMarkup);
                parameters = String.Concat(parameters,
                    String.Format(TelegramSendMessageMarkup, serializedKeyboard));
            }

            try
            {
                GetResponse(String.Concat(uri, parameters), PostMethod);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private static void AnswerQuery(DTO.Telegram.AnswerCallbackQuery query)
        {
            Console.WriteLine($"Answering to query #{query.callback_query_id}");
            String uri = String.Format(TelegramEnterPoint, _token, TelegramAnswerQuery);
            String parameters = String.Format(TelegramAnswerQueryFormat, query.callback_query_id);
            if (!String.IsNullOrEmpty(query.text))
                parameters = String.Concat(parameters, String.Format(TelegramAnswerQueryText, HttpUtility.UrlEncode(query.text)));

            try
            {
                GetResponse(String.Concat(uri, parameters), GetMethod);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private static void GetUpdates()
        {
            try
            {
                using (WebClient request = new WebClient())
                {
                    // Get new messages
                    Console.WriteLine("Sending long poll /getUpdates");
                    String uri = String.Format(TelegramEnterPoint, _token, TelegramGetUpdates);
                    request.Headers.Add(HttpRequestHeader.Accept, AcceptJsonHeader);
                    String parameters = String.Format(TelegramGetUpdatesFormat, _offset, 0);
                    String response = request.DownloadString(String.Concat(uri, parameters));
                    ParseResponse(response);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
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

                if (message.callback_query != null)
                    Game.Queue(message.callback_query);
                else if (message.message != null)
                    Game.Queue(message.message);
            }
        }

        private static Int32 _offset = 0;
        private static IList<DTO.Message> _pendingSending = new List<DTO.Message>();
        private static Thread _thread = null;
        private static Boolean _terminate = false;

        private static readonly String _token = ConfigurationManager.AppSettings[TokenKey];
        private const String TokenKey = "TELEGRAM_TOKEN";
        private const Int32 Interval = 1000;

        private const String AcceptJsonHeader = "application/json";
        private const String PostMethod = "POST";
        private const String GetMethod = "GET";

        private const String TelegramEnterPoint = "https://api.telegram.org/bot{0}/{1}";
        private const String TelegramGetUpdates = "getUpdates";
        private const String TelegramGetUpdatesFormat = "?offset={0}&timeout={1}";
        private const String TelegramSendMessage = "sendMessage";
        private const String TelegramSendMessageFormat = "?chat_id={0}&text={1}";
        private const String TelegramSendMessageMarkup = "&reply_markup={0}";
        private const String TelegramAnswerQuery = "answerCallbackQuery";
        private const String TelegramAnswerQueryFormat = "?callback_query_id";
        private const String TelegramAnswerQueryText = "&text={0}";

        private const String TelegramTokenNotPrivided = "В App.config не вписан токен Telegram бота";
    }
}
