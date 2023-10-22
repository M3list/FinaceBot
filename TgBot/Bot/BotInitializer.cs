using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace TgBot.Bot;

public class BotInitializer
{
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public BotInitializer()
    {
        _botClient = new TelegramBotClient("6943595388:AAEXqe1p5002Wkl7aibdsYTOguzCKQqoLRw");
        _cancellationTokenSource = new CancellationTokenSource();

        Console.WriteLine("Выполнена инициализация Бота");
    }

    public void Start()
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        BotHandlersInlineKeyBoards botRequestHandlers = new BotHandlersInlineKeyBoards();

        _botClient.StartReceiving(
            botRequestHandlers.HandleUpdateAsync,
            botRequestHandlers.HandlePollingErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );

        Console.WriteLine("Бот запущен");
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();

        Console.WriteLine("Бот остановлен");
    }
}