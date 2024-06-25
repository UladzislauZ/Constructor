using Constructor;
using Core;
using System.Text;

Console.WriteLine("Привет! Я служу для работы с конструкторным сервисом.");
Console.WriteLine("Инициализирую сервисы...");

IConstructorService service = new ConstructorService();
ConstructorModel model = new ();

Console.WriteLine("Я готов к работе.");
while (model.isWork)
{
    try
    {
        Console.WriteLine("Введите количество вспомогательных прямоугольников");
        int count = Convert.ToInt32(Console.ReadLine());
        model.rectangles = new Rectangle[count];
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine("Введите расположение левой нижней и правой верхней точки в формате Xmin,Ymin,Xmax,Ymax. Пример: 3.5, 5, 10, 11" +
                "(3.5 - Х левой нижней точки, 5 - У левой нижней точки, 10 - X правой верхней точки, 11 - У правой верхней точки)");
            string[] values = Console.ReadLine().Split(',');
            if (values.Length != 4)
                throw new Exception("Координаты точек введены не верно");
            Console.WriteLine("Введите цвет прямоугольника. Доступные цвета: серый, желтый, синий, красный, оранжевый, зеленый");
            Rectangle rectangle = new(Console.ReadLine(), new Point(Converters.ConvertToDouble(values[0]), Converters.ConvertToDouble(values[1])),
                                                       new Point(Converters.ConvertToDouble(values[2]), Converters.ConvertToDouble(values[3])));
            model.rectangles[i] = rectangle;
        }

        Console.WriteLine("Введите расположение объявленного прямоугольника");
        Console.WriteLine("Введите расположение левой нижней и правой верхней точки в формате Xmin,Ymin,Xmax,Ymax.");
        string[] masValues = Console.ReadLine().Split(',');
        if (masValues.Length != 4)
            throw new Exception("Координаты точек введены не верно");
        model.declaredRectangle = new(model.mainRectangleColor, new Point(Convert.ToDouble(masValues[0]), Convert.ToDouble(masValues[1])),
                                                   new Point(Convert.ToDouble(masValues[2]), Convert.ToDouble(masValues[3])));

        Console.WriteLine("Добавить возможность учитывать прямоугольники по цветам? Для включения этого функционала напишите 'у'");
        if (Console.ReadLine() == "у")
        {
            Console.WriteLine("Сейчас включен режим учета прямоугогльников по цвету со списка. Для включения режима на игнорирование прямоугольников по цвету нажмите 'у'");
            model.onTransformColorWhiteListToBlackList = Console.ReadLine() == "у"?true:false;

            Console.WriteLine("Введите цвета прямоугольников для учета в формате: цвет,цвет,... ;например:синий,желтый,красный");
            model.colorList = Console.ReadLine().Split(',');
        }
        else
        {
            model.colorList = [];
        }

        Console.WriteLine("Введите метод обработки. " +
            "\n1 - прямоугольник очертит крайние точки массива второстепенных прямоугольников" +
            "\n2 - прямоугольник очертит крайние точки массива второстепенных прямоугольников не учитывая тех, которые выходили за рамки главного прямоугольника.");
        switch (Console.ReadLine())
        {
            case "1":
                {
                    if (service.TryBuildMainRectangle(model.rectangles, model.declaredRectangle, model.colorList, model.onTransformColorWhiteListToBlackList, out model.mainRectangle))
                    {
                        Console.WriteLine($"Точки главного прямоугольника. Нижняя левая точка с координатами X - {model.mainRectangle.BotLeft.X}, Y - {model.mainRectangle.BotLeft.Y}. " +
                            $"Верхняя правая точка с координатами X - {model.mainRectangle.TopRight.X}, Y - {model.mainRectangle.TopRight.Y}.");
                    }
                    else
                    {
                        Console.WriteLine("Главный прямоугольник не содержит в себе второстепенные прямоугольники");
                    }

                    break;
                }
            case "2":
                {
                    if (service.TryBuildMainRectangleWithoutOverflow(model.rectangles, model.declaredRectangle, model.colorList, model.onTransformColorWhiteListToBlackList, out model.mainRectangle))
                    {
                        Console.WriteLine($"Точки главного прямоугольника. Нижняя левая точка с координатами X - {model.mainRectangle.BotLeft.X}, Y - {model.mainRectangle.BotLeft.Y}. " +
                            $"Верхняя правая точка с координатами X - {model.mainRectangle.TopRight.X}, Y - {model.mainRectangle.TopRight.Y}.");
                    }
                    else
                    {
                        Console.WriteLine("Главный прямоугольник не содержит в себе второстепенные прямоугольники");
                    }

                    break;
                }
            default:
                {
                    Console.WriteLine("Неверный ввод метода");
                    break;
                }
        }

        Console.WriteLine("Желаете сохранить результат в файл? Для согласия напишите 'у'");
        if(Console.ReadLine() == "у")
        {
            var data = new StringBuilder("Количество вспомогательных прямоугольников:");
            data.Append(count);
            data.Append("\nКрайние точки прямоугольников\n");
            for (int i = 0; i < count; i++) 
            {
                data.Append($"Левая нижняя точка с координатами X - {model.rectangles[i].BotLeft.X}, Y - {model.rectangles[i].BotLeft.Y}. " +
                    $"Правая верхняя точка с координатами X - {model.rectangles[i].TopRight.X}, Y - {model.rectangles[i].TopRight.Y}. Цвет - {model.rectangles[i].Color}\n");
            }

            data.Append("Объявленный прямоугольник для поиска главного\n");
            data.Append($"Левая нижняя точка с координатами X - {model.declaredRectangle.BotLeft.X}, Y - {model.declaredRectangle.BotLeft.Y}. " +
                    $"Правая верхняя точка с координатами X - {model.declaredRectangle.TopRight.X}, Y - {model.declaredRectangle.TopRight.Y}. Цвет - {model.declaredRectangle.Color}\n");
            if (model.colorList.Length > 0)
            {
                string arrayColor = "";
                foreach (var color in model.colorList)
                {
                    arrayColor += $"{color},";
                }

                if (model.onTransformColorWhiteListToBlackList)
                    data.Append($"Прямоугольники учитывались если цвет не был в блэклисте: {arrayColor}\n");
                else
                {
                    data.Append($"Прямоугольники учитывались если цвет был в вайтлисте: {arrayColor}\n"); 
                }
            }
            else
            {
                data.Append("Учитывание поиска по цвету было отключено\n");
            }

            data.Append("Главный прямоугольник\n");
            data.Append($"Точки главного прямоугольника. Нижняя левая точка с координатами X - {model.mainRectangle.BotLeft.X}, Y - {model.mainRectangle.BotLeft.Y}. " +
                $"Верхняя правая точка с координатами X - {model.mainRectangle.TopRight.X}, Y - {model.mainRectangle.TopRight.Y}.\n");

            Save.SaveResult(data.ToString());
        }

        model.isWork = false;
        Console.WriteLine("Для продолжения нажмите 'й'.");
        var pressButton = Console.ReadLine();
        model.isWork = pressButton != null && pressButton == "й";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка! {ex.Message}");
    }
}