using System.Net.Mime;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TgBot.Bot;

public class BotHandlersInlineKeyBoards
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message != null && update.Message.Text != null)
        {
            string requestMessageText = update.Message.Text;
            ChatId chatId = update.Message.Chat.Id;
            if (requestMessageText == "/start")
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "",
                    replyMarkup: GetMainInlineKeyboard(),
                    cancellationToken: cancellationToken);
            }
          
        }
        else if (update.CallbackQuery != null)
        {
            ChatId chatId = update.CallbackQuery.Message.Chat.Id;
            int messageId = update.CallbackQuery.Message.MessageId;
            string callBackData = update.CallbackQuery.Data;

            string text = "1";
            InlineKeyboardMarkup inlineKeyboardMarkup = null;
            
            switch (callBackData)
            {
                case"goto_eur":
                    text = "Выберите действие:";
                    inlineKeyboardMarkup = getEURInlineKeyboard();
                    break;
                case"goto_usd":
                    text = "Выберите действие:";
                    inlineKeyboardMarkup = getUSDInlineKeyboard();
                    break;
                case"buy_eur":
                    text = BotLogic.GetMin("EUR");
                    break;
                case"sale_eur":
                    text = BotLogic.GetMax("EUR");
                    break;
                case"buy_usd":
                    text = BotLogic.GetMin("USD");
                    break;
                case"sale_usd":
                    text = BotLogic.GetMin("USD");
                    break;
            }
            
           await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: text,
                replyMarkup: GetMainInlineKeyboard(),
                cancellationToken: cancellationToken
            );
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine($"Ошибка поймана в методе HandlePollingErrorAsync, {errorMessage}");
        return Task.CompletedTask;
    }
    
    private static InlineKeyboardMarkup GetMainInlineKeyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "EUR", callbackData: "goto_eur"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "USD", callbackData: "goto_usd"),
            }
        });
    }
    private static InlineKeyboardMarkup getEURInlineKeyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Купить", callbackData: "buy_eur"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Продать", callbackData: "sale_eur"),
            }
        });
    }
    
    private static InlineKeyboardMarkup getUSDInlineKeyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Купить", callbackData: "buy_usd"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(text: "Продать", callbackData: "sale_usd"),
            }
        });
    }
}