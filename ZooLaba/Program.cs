using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalZoo
{
  public abstract class Animal
  {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Habitat { get; set; }
    public string Diet { get; set; }
    public double Weight { get; set; }
    public string Color { get; set; }

    protected Animal(string name, int age, string habitat, string diet, double weight, string color)
    {
      Name = name;
      Age = age;
      Habitat = habitat;
      Diet = diet;
      Weight = weight;
      Color = color;
    }

    public virtual string GetInfo()
    {
      return $"Кличка: {Name}, Возраст: {Age}, Среда: {Habitat}, " +
             $"Питание: {Diet}, Вес: {Weight} кг, Окрас: {Color}";
    }

    public abstract string GetAnimalType();

    // Виртуальный метод для сравнения животных - нужен, чтобы проверять уникальность при добавлении, virtual позволяет переопределить его в классах-наследниках для учёта их уникальных полей
    public virtual bool IsEqualTo(Animal other)
    {
      if (other == null) return false;

      // Сравниваем по основным полям, приводя строки к нижнему регистру для "регистронезависимости", GetType() проверяет, что объекты принадлежат одному классу (оба Mammal, оба Bird и т.д.)
      return this.Name.ToLower() == other.Name.ToLower() &&
             this.Age == other.Age &&
             this.Habitat.ToLower() == other.Habitat.ToLower() &&
             this.GetType() == other.GetType();
    }
  }

  public class Mammal : Animal
  {
    public bool HasFur { get; set; }

    public Mammal(string name, int age, string habitat, string diet, double weight, string color, bool hasFur)
        : base(name, age, habitat, diet, weight, color)
    {
      HasFur = hasFur;
    }

    public override string GetInfo()
    {
      string furStatus = HasFur ? "есть" : "нет";
      return base.GetInfo() + $", Тип: Млекопитающее, Шерсть: {furStatus}";
    }

    public override string GetAnimalType() => "Млекопитающее";

    // Переопределяем IsEqualTo, чтобы учитывать наличие шерсти при сравнении
    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other)) return false;
      if (!(other is Mammal)) return false;

      Mammal otherMammal = (Mammal)other;
      return this.HasFur == otherMammal.HasFur;
    }
  }

  public class Bird : Animal
  {
    public double WingSpan { get; set; }

    public Bird(string name, int age, string habitat, string diet, double weight, string color, double wingSpan)
        : base(name, age, habitat, diet, weight, color)
    {
      WingSpan = wingSpan;
    }

    public override string GetInfo()
    {
      return base.GetInfo() + $", Тип: Птица, Размах крыльев: {WingSpan} м";
    }

    public override string GetAnimalType() => "Птица";

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other)) return false;
      if (!(other is Bird)) return false;

      Bird otherBird = (Bird)other;
      // Math.Abs и < 0.01 нужны для сравнения дробных чисел с погрешностью, из-за особенностей хранения double нельзя сравнивать через ==
      return Math.Abs(this.WingSpan - otherBird.WingSpan) < 0.01;
    }
  }

  public class Fish : Animal
  {
    public string WaterType { get; set; }

    public Fish(string name, int age, string habitat, string diet, double weight, string color, string waterType)
        : base(name, age, habitat, diet, weight, color)
    {
      WaterType = waterType;
    }

    public override string GetInfo()
    {
      return base.GetInfo() + $", Тип: Рыба, Тип воды: {WaterType}";
    }

    public override string GetAnimalType() => "Рыба";

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other)) return false;
      if (!(other is Fish)) return false;

      Fish otherFish = (Fish)other;
      return this.WaterType.ToLower() == otherFish.WaterType.ToLower();
    }
  }

  public class Reptile : Animal
  {
    public bool IsVenomous { get; set; }

    public Reptile(string name, int age, string habitat, string diet, double weight, string color, bool isVenomous)
        : base(name, age, habitat, diet, weight, color)
    {
      IsVenomous = isVenomous;
    }

    public override string GetInfo()
    {
      string venomStatus = IsVenomous ? "ядовитое" : "неядовитое";
      return base.GetInfo() + $", Тип: Пресмыкающееся, Ядовитость: {venomStatus}";
    }

    public override string GetAnimalType() => "Пресмыкающееся";

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other)) return false;
      if (!(other is Reptile)) return false;

      Reptile otherReptile = (Reptile)other;
      return this.IsVenomous == otherReptile.IsVenomous;
    }
  }

  public class Amphibian : Animal
  {
    public string SkinMoisture { get; set; }

    public Amphibian(string name, int age, string habitat, string diet, double weight, string color, string skinMoisture)
        : base(name, age, habitat, diet, weight, color)
    {
      SkinMoisture = skinMoisture;
    }

    public override string GetInfo()
    {
      return base.GetInfo() + $", Тип: Земноводное, Влажность кожи: {SkinMoisture}";
    }

    public override string GetAnimalType() => "Земноводное";

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other)) return false;
      if (!(other is Amphibian)) return false;

      Amphibian otherAmphibian = (Amphibian)other;
      return this.SkinMoisture.ToLower() == otherAmphibian.SkinMoisture.ToLower();
    }
  }

  public sealed class AnimalManager
  {
    // static - переменная принадлежит классу, а не объекту, readonly - можно задать только один раз, это основа паттерна Singleton - гарантия, что объект будет только один
    private static readonly AnimalManager _instance = new AnimalManager();
    private List<Animal> _animals;

    // Приватный конструктор - никто извне не сможет создать новый экземпляр через new
    private AnimalManager()
    {
      _animals = new List<Animal>();
    }

    public static AnimalManager Instance => _instance;

    // Свойство только для чтения, отдаёт текущее количество животных
    public int AnimalsCount => _animals.Count;

    public bool AddAnimal(Animal animal)
    {
      // Проверяем, нет ли уже такого животного, проходя по всему списку
      foreach (var existingAnimal in _animals)
      {
        // Используем виртуальный метод IsEqualTo, который учитывает тип животного и его уникальные поля
        if (existingAnimal.IsEqualTo(animal))
        {
          Console.WriteLine($"Ошибка: Животное {animal.Name} уже существует в зоопарке!");
          return false;
        }
      }

      _animals.Add(animal);
      Console.WriteLine($"Животное {animal.Name} успешно добавлено в зоопарк!");
      return true;
    }

    public void ShowAllAnimals()
    {
      Console.WriteLine("\n=== СПИСОК ВСЕХ ЖИВОТНЫХ ===");
      for (int index = 0; index < _animals.Count; ++index)
      {
        // i+1 нужно, чтобы показывать пользователю нумерацию с 1, а не с 0
        Console.WriteLine($"[{index + 1}] {_animals[index].GetInfo()}");
      }
    }

    public void ShowAnimalByName(string name)
    {
      string searchName = name.ToLower();
      // LINQ запрос: ищем всех животных, в имени которых содержится искомый текст, Where фильтрует список, Contains проверяет вхождение подстроки
      var foundAnimals = _animals.Where(a =>
          a.Name.ToLower().Contains(searchName)).ToList();

      if (foundAnimals.Count == 0)
      {
        Console.WriteLine($"\nЖивотные с именем '{name}' не найдены.");
        return;
      }

      Console.WriteLine($"\n=== РЕЗУЛЬТАТЫ ПОИСКА ПО ИМЕНИ '{name}' ===");
      foreach (var animal in foundAnimals)
      {
        Console.WriteLine(animal.GetInfo());
      }
    }

    public void ShowAnimalByIndex(int index)
    {
      // Проверяем, что индекс находится в допустимых пределах, иначе программа крашнется с ошибкой
      if (index < 0 || index >= _animals.Count)
      {
        Console.WriteLine("Животного с таким номером не существует.");
        return;
      }

      Console.WriteLine($"\n=== ЖИВОТНОЕ №{index + 1} ===");
      Console.WriteLine(_animals[index].GetInfo());
    }
  }

  class Program
  {
    static void Main(string[] rus)
    {
      Console.OutputEncoding = System.Text.Encoding.UTF8;
      Console.WriteLine("=== ДОБРО ПОЖАЛОВАТЬ В СИСТЕМУ УПРАВЛЕНИЯ ЗООПАРКОМ ===\n");

      AnimalManager manager = AnimalManager.Instance;
      SeedData(manager);
      ShowMainMenu(manager);
    }

    static void SeedData(AnimalManager manager)
    {
      manager.AddAnimal(new Mammal("Бубон", 5, "лес", "хищник", 15.5, "рыжий", true));
      manager.AddAnimal(new Bird("Кидала", 2, "город", "всеядное", 0.3, "зеленый", 0.25));
      manager.AddAnimal(new Fish("Немезис", 1, "океан", "всеядное", 0.1, "оранжевый", "морская"));
      manager.AddAnimal(new Reptile("Каракал", 8, "джунгли", "хищник", 25.0, "зеленый", true));
      manager.AddAnimal(new Amphibian("Рыброн", 3, "болото", "насекомоядное", 0.5, "зеленый", "влажная"));
      Console.WriteLine("Тестовые животные добавлены для демонстрации.\n");
    }

    static void ShowMainMenu(AnimalManager manager)
    {
      while (true)
      {
        Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
        Console.WriteLine("1. Показать всех животных");
        Console.WriteLine("2. Найти животное по имени");
        Console.WriteLine("3. Показать животное по номеру");
        Console.WriteLine("4. Добавить новое животное");
        Console.WriteLine("5. Выход");
        Console.Write("Выберите действие (1-5): ");

        string choice = Console.ReadLine();

        switch (choice)
        {
          case "1":
            manager.ShowAllAnimals();
            break;
          case "2":
            Console.Write("Введите имя животного (или часть имени): ");
            string name = Console.ReadLine();
            manager.ShowAnimalByName(name);
            break;
          case "3":
            Console.Write($"Введите номер животного (от 1 до {manager.AnimalsCount}): ");
            // TryParse пытается преобразовать ввод в число и возвращает false, если вводится не число, это защита от краша при вводе букв
            if (int.TryParse(Console.ReadLine(), out int index))
            {
              // index-1 потому что пользователь ввёл номер с 1, а в массиве индексы с 0
              manager.ShowAnimalByIndex(index - 1);
            }
            else
            {
              Console.WriteLine("Некорректный ввод. Введите число.");
            }
            break;
          case "4":
            AddNewAnimal(manager);
            break;
          case "5":
            Console.WriteLine("Выход из программы. До свидания!");
            return;
          default:
            Console.WriteLine("Некорректный выбор. Попробуйте снова.");
            break;
        }
      }
    }

    // Метод проверяет, что строка состоит только из букв, пробелов и дефисов, чтоб окрас животного не вводился числами
    static bool IsOnlyLetters(string input)
    {
      if (string.IsNullOrWhiteSpace(input)) return false;

      foreach (char c in input)
      {
        // char.IsLetter проверяет, является ли символ буквой (любого алфавита), допускаются также пробелы и дефисы для составных цветов
        if (!char.IsLetter(c) && c != ' ' && c != '-')
        {
          return false;
        }
      }
      return true;
    }

    static void AddNewAnimal(AnimalManager manager)
    {
      Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО ЖИВОТНОГО ===");

      Console.WriteLine("Выберите тип животного:");
      Console.WriteLine("1. Млекопитающее");
      Console.WriteLine("2. Птица");
      Console.WriteLine("3. Рыба");
      Console.WriteLine("4. Пресмыкающееся");
      Console.WriteLine("5. Земноводное");
      Console.Write("Ваш выбор (1-5): ");

      string typeChoice = Console.ReadLine();

      // Проверяем тип сразу, чтобы не спрашивать остальные данные при неверном типе
      if (typeChoice != "1" && typeChoice != "2" && typeChoice != "3" && typeChoice != "4" && typeChoice != "5")
      {
        Console.WriteLine("Неверный тип животного! Операция отменена.");
        return;
      }

      Console.Write("Введите кличку: ");
      string name = Console.ReadLine();

      Console.Write("Введите возраст (целое число): ");
      if (!int.TryParse(Console.ReadLine(), out int age))
      {
        Console.WriteLine("Некорректный возраст. Операция отменена.");
        return;
      }

      Console.Write("Введите среду обитания (например, лес, водоём, пустыня): ");
      string habitat = Console.ReadLine();

      Console.Write("Введите тип питания (хищник, травоядное, всеядное): ");
      string diet = Console.ReadLine();

      Console.Write("Введите вес (в кг, можно дробное число): ");
      if (!double.TryParse(Console.ReadLine(), out double weight))
      {
        Console.WriteLine("Некорректный вес. Операция отменена.");
        return;
      }

      Console.Write("Введите окрас (только буквы): ");
      string color = Console.ReadLine();

      if (!IsOnlyLetters(color))
      {
        Console.WriteLine("Некорректный окрас. Окрас должен содержать только буквы. Операция отменена.");
        return;
      }

      Animal newAnimal = null;

      try
      {
        // В зависимости от выбранного типа создаём конкретное животное и запрашиваем его уникальные поля
        switch (typeChoice)
        {
          case "1":
            Console.Write("Есть шерсть? (да/нет): ");
            bool hasFur;
            if (!TryGetYesNoInput(out hasFur))
            {
              Console.WriteLine("Некорректный ввод. Ожидался ответ, начинающийся с 'д' (да) или 'н' (нет). Операция отменена.");
              return;
            }
            newAnimal = new Mammal(name, age, habitat, diet, weight, color, hasFur);
            break;

          case "2":
            Console.Write("Введите размах крыльев (в метрах, дробное число): ");
            if (!double.TryParse(Console.ReadLine(), out double wingSpan))
            {
              Console.WriteLine("Некорректный размах крыльев. Операция отменена.");
              return;
            }
            newAnimal = new Bird(name, age, habitat, diet, weight, color, wingSpan);
            break;

          case "3":
            Console.Write("Введите тип воды (пресная/морская): ");
            string waterType = Console.ReadLine();
            newAnimal = new Fish(name, age, habitat, diet, weight, color, waterType);
            break;

          case "4":
            Console.Write("Ядовитое? (да/нет): ");
            bool isVenomous;
            if (!TryGetYesNoInput(out isVenomous))
            {
              Console.WriteLine("Некорректный ввод. Ожидался ответ, начинающийся с 'д' (да) или 'н' (нет). Операция отменена.");
              return;
            }
            newAnimal = new Reptile(name, age, habitat, diet, weight, color, isVenomous);
            break;

          case "5":
            Console.Write("Введите влажность кожи (например, влажная, сухая): ");
            string skinMoisture = Console.ReadLine();
            newAnimal = new Amphibian(name, age, habitat, diet, weight, color, skinMoisture);
            break;
        }

        if (newAnimal != null)
        {
          manager.AddAnimal(newAnimal);
          Console.WriteLine("Информация о новом животном:");
          Console.WriteLine(newAnimal.GetInfo());
        }
      }
      catch (Exception ex)
      {
        // Ловим все возможные ошибки, чтобы программа не крашнулась, а показала сообщение
        Console.WriteLine($"Ошибка при создании животного: {ex.Message}");
      }
    }

    // Метод для обработки ответов да/нет с проверкой только первой буквы, out bool result позволяет вернуть два значения: успешность операции и сам результат
    static bool TryGetYesNoInput(out bool result)
    {
      string input = Console.ReadLine().Trim().ToLower();
      result = false;

      if (string.IsNullOrEmpty(input))
      {
        return false;
      }

      char firstChar = input[0];

      // Смотрим только на первую букву - если 'д' то true, если 'н' то false
      if (firstChar == 'д')
      {
        result = true;
        return true;
      }
      else if (firstChar == 'н')
      {
        result = false;
        return true;
      }

      return false;
    }
  }
}