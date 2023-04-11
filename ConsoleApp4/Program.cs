using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Microsoft.VisualBasic;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.Json;
using System.Configuration;



namespace TelegramBot
{

    class Program
    {
        const string fileName = "Loan.json";
        const string fileNameOtziv = "Otziv.json";
        public static Dictionary<long, string> objTypes = new Dictionary<long, string>();


        static ITelegramBotClient _botClient = new TelegramBotClient(ConfigurationManager.AppSettings.Get("DebugKey"));
        public static CancellationTokenSource cts = new CancellationTokenSource();


    public static async Task ListenForMessagesAsync()
        {
            

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() 
            };
            

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await _botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            int pricerez=988899;
            var date = update.Message!.Date;
            var datemes = DateTime.Now - (date.AddHours(7));

            if (datemes.Seconds > 20)
                 return;
            #region Keyboards                


            var keyboard =
            new KeyboardButton[][]
            {
            new KeyboardButton[]
            {
                new KeyboardButton("Контакты"),
                new KeyboardButton("Фото")
            },

            new KeyboardButton[]
            {
                new KeyboardButton("Рассчет стоимости")
            },

            new KeyboardButton[]
            {
                new KeyboardButton("О нас"),
                new KeyboardButton("Оставить отзыв"),
                
            }
            };
            var keyboardobjtype =
                new KeyboardButton[][]
 {
            
                new KeyboardButton[]
            {
                new KeyboardButton("Интерьерная"),

            },
            new KeyboardButton[]
            {
                new KeyboardButton("Промышленная")
            },
            


 };

            var keyboarddanet =
                new KeyboardButton[][]
{
            new KeyboardButton[]
            {
                new KeyboardButton("Да"),
                
            },
            new KeyboardButton[]
            {
                new KeyboardButton("Главное меню")
            },


};
            

            var rmu = new ReplyKeyboardMarkup(keyboard);
            var rmobj = new ReplyKeyboardMarkup(keyboardobjtype);
            var rmdanet = new ReplyKeyboardMarkup(keyboarddanet);
            #endregion


            if (update.Type == UpdateType.Message)
            {

                var message = update.Message;
                var messageText = message.Text;
                if (messageText == null)
                    return;

                if (messageText == "/start")
                {
                    try
                    {
                        string json = message.Chat.FirstName + " " + message.Chat.Username + " " + message.Chat.Id + " " + DateAndTime.Now;
                        Console.WriteLine("Помещаем данные в файл: " + json);
                        json = JsonSerializer.Serialize(json);

                        System.IO.File.AppendAllText(fileName, json + Environment.NewLine);
                    }
                    catch
                    {
                        throw new Exception();
                        Console.WriteLine("Что то с файлом в блоке /start");
                    }
                    
                    botClient.SendTextMessageAsync(message.Chat, $"Привет, {message.Chat.FirstName}. Пока доступ к командам осуществляется через ввод слова команды.\n" +
                        "Если меню снизу не появилось. Просто напиши главное меню и отправь. Будет доступен базовый функционал", replyMarkup: rmu);

                }

                if (messageText.ToLower() == "главное меню" || messageText.ToLower()=="/main")
                {
                    objTypes.Remove(message.Chat.Id);
                    botClient.SendTextMessageAsync(message.Chat, $"Вы перешли в главное меню", replyMarkup: rmu);

                }

                if (messageText.ToLower() == "да")
                {

                    botClient.SendTextMessageAsync(message.Chat, $"Ваши контакты отправлены менеджеру он свяжется с вами в ближайщее время", replyMarkup: rmu);

                    botClient.SendTextMessageAsync(chatId: 5654825597, $"Свяжитесь с клиентом по поводу покраски\n" +
                        $"{message.Chat.FirstName}\n" +
                        $"Имя пользователя для связи:\n" +
                        $"@{message.Chat.Username}", replyMarkup: rmu);

                }

                if (message.Text.ToLower() == "фото")
                {
                    

                    try
                    {

                        string imagepath = Path.Combine(Environment.CurrentDirectory, "v11.jpg");
                        string imagepath2 = Path.Combine(Environment.CurrentDirectory, "v12.jpg");

                        using (var stream = System.IO.File.OpenRead(imagepath))
                        {

                            await botClient.SendPhotoAsync(update.Message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, "v11.jpg"), "Покраска комнаты в доме");
                        }
                        
                            await botClient.SendPhotoAsync(update.Message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile("https://thumb.tildacdn.com/tild6630-3031-4965-a232-333231613438/-/format/webp/21.jpg"), "Красили дом");
                        
                        using (var stream = System.IO.File.OpenRead(imagepath2))
                        {

                            await botClient.SendPhotoAsync(update.Message.Chat.Id, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, "v12.jpg"), "Покраска бокса\nФото разных объектов у нас в галерее на сайте\n https://krasimpro.ru/foto", replyMarkup: rmu);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine("Error in block фото");
                    }
                    
                }

                if (update.Message.Text.ToLower() == "контакты")
                {
                    
                    botClient.SendTextMessageAsync(message.Chat, "Cайт:\nhttps://\n\n" +
                        "Телефон:\n\n\n" +
                        "Группа в ВК:\n\n\n" +
                        "Работаем без выходных с\n8:00 - 20:00", replyMarkup: rmu);

                }

                if (update.Message.Text.ToLower() == "о нас")
                {
                    
                    botClient.SendTextMessageAsync(message.Chat, "Компания занимается покраской стен и потолков, а также домов и коттеджей\n" +
                        "Работаем с объектами с высокими требованияк покраске\n" +
                        "Также осуществляем безвоздушную покраску промышленных помещений в кратчайщие сроки", replyMarkup: rmu);

                }

                if (objTypes.ContainsKey(message.Chat.Id))
                {
                    var objType= objTypes[message.Chat.Id];
                    if (objType == "Интерьерная" || objType == "Промышленная")
                    {
                        Console.WriteLine("Вы попали в блок расчета стоимости по меню");
                        try
                        {
                            pricerez = int.Parse(message.Text);
                            if (pricerez != 988899 && pricerez != 0)
                            {
                                switch (objType)
                                {
                                    case "Промышленная":

                                        pricerez *= 70;
                                        await botClient.SendTextMessageAsync(message.Chat, "Стоимость промышленной покраски равна: " + (pricerez + pricerez * 0.3) + "\n\n" +
                                            "Стоимость укрывных работ от 5% до 30% от суммы покраски в зависимости от сложности объекта\n" +
                                            $"Для вашего объекта необходимо:\nКраски: {pricerez / 70 * 0.4:##} кг\n" +
                                            $"Грунтовки: {pricerez / 70 * 0.15:##} кг\n\n" +
                                            "Данные расчеты приблезительны. Для более точных рассчетов вы можете пригласить менеджера на бесплатный замер\n\n" +
                                            "Номер телефона менеджера для связи\n");

                                        await botClient.SendContactAsync(message.Chat, "PHONE_NUMBER\n", "Отвечу по покраске");

                                        await botClient.SendTextMessageAsync(message.Chat, "Или задайте ему вопрос в телеграм @USER_NAME");

                                        if (pricerez / 70 > 900)
                                            await botClient.SendTextMessageAsync(message.Chat, "\nВам предоставлена скидка в 20% на покраску. \nСтоимость с учетом скидки:\n" + (pricerez / 70 * 56) + "\nСкидка за объем свыше 900 м");

                                        await botClient.SendTextMessageAsync(message.Chat, "Хотите чтобы менеджер связался с вами и проконсультировал по поводу покраски?", replyMarkup: rmdanet);



                                        break;

                                    case "Интерьерная":

                                        pricerez *= 150;
                                        await botClient.SendTextMessageAsync(message.Chat, "Стоимость интерьерной покраски равна: " + (pricerez + pricerez * 0.3) + "\n\n" +
                                            "Стоимость укрывных работ от 5% до 30% от суммы покраски в зависимости от сложности объекта\n" +
                                            $"Для вашего объекта необходимо:\nКраски: {pricerez / 150 * 0.4:##} кг\n" +
                                            $"Грунтовки: {pricerez / 150 * 0.15:##} кг\n\n" +
                                            "Данные расчеты приблезительны. Для более точных рассчетов вы можете пригласить менеджера на бесплатный замер\n\n" +
                                            "Номер телефона менеджера для связи\n");


                                        await botClient.SendContactAsync(message.Chat, "PHONE_NUMBER\n", "Отвечу по покраске");
                                        await botClient.SendTextMessageAsync(message.Chat, "Или задайте ему вопрос в телеграм @USER_NAME");

                                        await botClient.SendTextMessageAsync(message.Chat, "Хотите чтобы менеджер связался с вами и проконсультировал по поводу покраски?", replyMarkup: rmdanet);


                                        break;
                                    default:
                                        break;
                                }



                            }
                            objTypes.Remove(message.Chat.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("произошла ошибка\n" + ex.ToString());
                            botClient.SendTextMessageAsync(message.Chat, "Ошибка. Вы ввели\n" + message.Text + "\nНужно ввести только число без букв или других знаков\n" +
                                "Для расчета заново введите вашу площадь. Если хотите выйти из расчета стоимости нажмите главное меню", replyMarkup: new ReplyKeyboardMarkup(new KeyboardButton("Главное меню")));
                           

                        }
                        
                    }
                    else if (objType == "оставить отзыв")
                    {


                        try
                        {
                            botClient.SendTextMessageAsync(message.Chat, $"Ваш отзыв отправлен", replyMarkup: rmu);
                            
                            botClient.SendTextMessageAsync(chatId: "MANAGER_ID", $"Клиент @{message.Chat.Username} оставил отзыв\n" +
                                $"\n{messageText}", replyMarkup: rmu);
                            

                            string jsonotziv = message.Chat.FirstName + " " + message.Chat.Username + " " + message.Chat.Id + " " + DateAndTime.Now + " " + messageText;
                            Console.WriteLine("Помещаем данные в файл: " + jsonotziv);
                            jsonotziv = JsonSerializer.Serialize(jsonotziv);

                            System.IO.File.AppendAllText(fileNameOtziv, jsonotziv + Environment.NewLine);
                        }
                        catch
                        {
                            throw new Exception();
                            Console.WriteLine("Что то с файлом в блоке оставить отзыв");
                        }
                        objTypes.Remove(message.Chat.Id);



                    }

                }
                if (messageText.ToLower() == "оставить отзыв")
                {
                    objTypes.Add(message.Chat.Id, messageText.ToLower());
                    botClient.SendTextMessageAsync(message.Chat, $"Спасибо что хотите оставить отзыв. Это поможет нам стать лучше\n" +
                        $"Просто введите сообщение и ваш отзыв будет отправлен директору", replyMarkup: rmu);

                }

                if (message.Text == "Промышленная" || message.Text == "Интерьерная")
                {

                    objTypes.Add(message.Chat.Id, message.Text);
                   
                    botClient.SendTextMessageAsync(message.Chat, "Пожалуйста введите необходиму площадь покраски без букв. Только цифры. Например: 100 или 200");

                }

                if (message.Text.ToLower() == "рассчет стоимости")
                {

                    botClient.SendTextMessageAsync(message.Chat, "Какая покраска вам нужна?\n" +
                        "Интерьерная\n" +
                        "Промышленная", replyMarkup: rmobj);

                }

                //For debug
                //Console.WriteLine($"Received a '{messageText}' message in chat {message.Chat.Id}.");
                //Console.WriteLine($"Received a '{messageText}' message in chat {message.Chat.FirstName}.");

                
            }
            if (update.Type == UpdateType.CallbackQuery)
            {
                return;

            }


        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task Main()
        {

            await ListenForMessagesAsync();

            Console.ReadLine();


        }




    }
}






