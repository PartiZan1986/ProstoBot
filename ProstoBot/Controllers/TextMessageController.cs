using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using ProstoBot.Services;




namespace ProstoBot.Controllers
{
    public class TextMessageController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Количество символов" , $"kolvo"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"sum")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот может подсчитать количество символов и сумму чисел в текст.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Выбирайте, что будем делать.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                default:
                    var code = _memoryStorage.GetSession(message.Chat.Id).ChoiceButtonCode;

                    switch (code)
                    {
                        case "kolvo":
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"kolvo: {message.Text.Length}", cancellationToken: ct);
                            break;
                        case "sum":
                            var numbers = message.Text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            var result = 0m;
                            foreach (var number in numbers)
                                result += decimal.Parse(number);

                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"sum: {result}", cancellationToken: ct);
                            break;
                        default:
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте сообщение(тестовое).", cancellationToken: ct);
                            break;
                    }

                    break;
            }

        }        
    }
}
