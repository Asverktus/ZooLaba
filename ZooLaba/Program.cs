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

    // Virtual позволяет наследникам расширить информацию, не переписывая базовую часть
    public virtual string GetInfo()
    {
      return $"Name: {Name}, Age: {Age}, Habitat: {Habitat}, " +
             $"Diet: {Diet}, Weight: {Weight} kg, Color: {Color}";
    }

    public abstract string GetAnimalType();

    // Сравнение по основным полям + GetType() чтобы не смешивать рыб с птицами
    public virtual bool IsEqualTo(Animal other)
    {
      if (other == null)
      {
        return false;
      }

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
      string furStatus = HasFur ? "yes" : "no";
      return base.GetInfo() + $", Type: Mammal, Fur: {furStatus}";
    }

    public override string GetAnimalType()
    {
      return "Mammal";
    }

    // Проверка на уникальные поля - шерсть, чтобы отличить двух млекопитающих
    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other))
      {
        return false;
      }

      if (!(other is Mammal))
      {
        return false;
      }

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
      return base.GetInfo() + $", Type: Bird, Wingspan: {WingSpan} m";
    }

    public override string GetAnimalType()
    {
      return "Bird";
    }

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other))
      {
        return false;
      }

      if (!(other is Bird))
      {
        return false;
      }

      Bird otherBird = (Bird)other;
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
      return base.GetInfo() + $", Type: Fish, Water type: {WaterType}";
    }

    public override string GetAnimalType()
    {
      return "Fish";
    }

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other))
      {
        return false;
      }

      if (!(other is Fish))
      {
        return false;
      }

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
      string venomStatus = IsVenomous ? "yes" : "no";
      return base.GetInfo() + $", Type: Reptile, Venomous: {venomStatus}";
    }

    public override string GetAnimalType()
    {
      return "Reptile";
    }

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other))
      {
        return false;
      }

      if (!(other is Reptile))
      {
        return false;
      }

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
      return base.GetInfo() + $", Type: Amphibian, Skin moisture: {SkinMoisture}";
    }

    public override string GetAnimalType()
    {
      return "Amphibian";
    }

    public override bool IsEqualTo(Animal other)
    {
      if (!base.IsEqualTo(other))
      {
        return false;
      }

      if (!(other is Amphibian))
      {
        return false;
      }

      Amphibian otherAmphibian = (Amphibian)other;
      return this.SkinMoisture.ToLower() == otherAmphibian.SkinMoisture.ToLower();
    }
  }

  public sealed class AnimalManager
  {
    // static + readonly + private constructor - классический Singleton, чтобы менеджер был в единственном экземпляре
    private static readonly AnimalManager _instance = new AnimalManager();
    private List<Animal> _animals;

    private AnimalManager()
    {
      _animals = new List<Animal>();
    }

    public static AnimalManager Instance
    {
      get
      {
        return _instance;
      }
    }

    public int AnimalsCount
    {
      get
      {
        return _animals.Count;
      }
    }

    public bool AddAnimal(Animal animal)
    {
      // Проверяются дубликаты перед добавлением - защита от одинаковых записей
      foreach (var existingAnimal in _animals)
      {
        if (existingAnimal.IsEqualTo(animal))
        {
          Console.WriteLine($"Error: Animal {animal.Name} already exists in the zoo!");
          return false;
        }
      }

      _animals.Add(animal);
      Console.WriteLine($"Animal {animal.Name} successfully added to the zoo!");
      return true;
    }

    public void ShowAllAnimals()
    {
      Console.WriteLine("\n=== ANIMALS LIST ===");

      for (int animalIndex = 0; animalIndex < _animals.Count; ++animalIndex)
      {
        // +1 чтобы пользователь видел счет с 1
        Console.WriteLine($"[{animalIndex + 1}] {_animals[animalIndex].GetInfo()}");
      }
    }

    public void ShowAnimalByName(string searchQuery)
    {
      string lowerCaseSearchQuery = searchQuery.ToLower();
      // LINQ-фильтр: поиск вхождения подстроки без учета регистра
      var foundAnimals = _animals.Where(animal =>
          animal.Name.ToLower().Contains(lowerCaseSearchQuery)).ToList();

      if (foundAnimals.Count == 0)
      {
        Console.WriteLine($"\nAnimals with name '{searchQuery}' not found.");
        return;
      }

      Console.WriteLine($"\n=== SEARCH RESULTS FOR '{searchQuery}' ===");

      foreach (var animal in foundAnimals)
      {
        Console.WriteLine(animal.GetInfo());
      }
    }

    public void ShowAnimalByIndex(int animalIndex)
    {
      // Защита от выхода за границы массива - иначе IndexOutOfRangeException
      if (animalIndex < 0 || animalIndex >= _animals.Count)
      {
        Console.WriteLine("Animal with this number does not exist.");
        return;
      }

      Console.WriteLine($"\n=== ANIMAL #{animalIndex + 1} ===");
      Console.WriteLine(_animals[animalIndex].GetInfo());
    }
  }

  class Program
  {
    // Константы защищают от опечаток и позволяют изменить значение в одном месте
    private const string MammalType = "1";
    private const string BirdType = "2";
    private const string FishType = "3";
    private const string ReptileType = "4";
    private const string AmphibianType = "5";

    private const string ShowAllAnimalsOption = "1";
    private const string SearchByNameOption = "2";
    private const string ShowByIndexOption = "3";
    private const string AddNewAnimalOption = "4";
    private const string ExitOption = "5";

    static void Main(string[] commandLineArgs)
    {
      Console.WriteLine("=== WELCOME TO ZOO MANAGEMENT SYSTEM ===\n");

      AnimalManager manager = AnimalManager.Instance;
      AddTestData(manager);
      ShowMainMenu(manager);
    }

    static void AddTestData(AnimalManager manager)
    {
      manager.AddAnimal(new Mammal("Bob", 5, "forest", "predator", 15.5, "ginger", true));
      manager.AddAnimal(new Bird("Kesha", 2, "city", "omnivorous", 0.3, "green", 0.25));
      manager.AddAnimal(new Fish("Nemo", 1, "ocean", "omnivorous", 0.1, "orange", "sea"));
      manager.AddAnimal(new Reptile("Snake", 8, "jungle", "predator", 25.0, "green", true));
      manager.AddAnimal(new Amphibian("Frog", 3, "swamp", "insectivorous", 0.5, "green", "moist"));
      Console.WriteLine("Test animals added for demonstration.\n");
    }

    static void ShowMainMenu(AnimalManager manager)
    {
      while (true)
      {
        Console.WriteLine("\n=== MAIN MENU ===");
        Console.WriteLine("1. Show all animals");
        Console.WriteLine("2. Search animal by name");
        Console.WriteLine("3. Show animal by number");
        Console.WriteLine("4. Add new animal");
        Console.WriteLine("5. Exit");
        Console.Write("Choose action (1-5): ");

        string userChoice = Console.ReadLine();

        switch (userChoice)
        {
          case ShowAllAnimalsOption:
            manager.ShowAllAnimals();
            break;

          case SearchByNameOption:
            Console.Write("Enter animal name (or part of name): ");
            string searchName = Console.ReadLine();
            manager.ShowAnimalByName(searchName);
            break;

          case ShowByIndexOption:
            Console.Write($"Enter animal number (from 1 to {manager.AnimalsCount}): ");

            if (int.TryParse(Console.ReadLine(), out int animalNumber))
            {
              // Минус 1 потому что пользовательский счет с 1, а не с 0
              manager.ShowAnimalByIndex(animalNumber - 1);
            }
            else
            {
              Console.WriteLine("Invalid input. Please enter a number.");
            }
            break;

          case AddNewAnimalOption:
            AddNewAnimal(manager);
            break;

          case ExitOption:
            Console.WriteLine("Exiting program. Goodbye!");
            return;

          default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
        }
      }
    }

    // Проверяется, что цвет не содержит цифр
    static bool IsOnlyLetters(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
      {
        return false;
      }

      foreach (char character in input)
      {
        if (!char.IsLetter(character) && character != ' ' && character != '-')
        {
          return false;
        }
      }

      return true;
    }

    static void AddNewAnimal(AnimalManager manager)
    {
      Console.WriteLine("\n=== ADDING NEW ANIMAL ===");

      Console.WriteLine("Select animal type:");
      Console.WriteLine("1. Mammal");
      Console.WriteLine("2. Bird");
      Console.WriteLine("3. Fish");
      Console.WriteLine("4. Reptile");
      Console.WriteLine("5. Amphibian");
      Console.Write("Your choice (1-5): ");

      string selectedType = Console.ReadLine();

      // Проверяется тип сразу, чтобы не мучить пользователя вводом данных при неверном выборе
      if (selectedType != MammalType && selectedType != BirdType &&
          selectedType != FishType && selectedType != ReptileType && selectedType != AmphibianType)
      {
        Console.WriteLine("Invalid animal type! Operation cancelled.");
        return;
      }

      Console.Write("Enter name: ");
      string name = Console.ReadLine();

      Console.Write("Enter age (integer): ");
      if (!int.TryParse(Console.ReadLine(), out int age))
      {
        Console.WriteLine("Invalid age. Operation cancelled.");
        return;
      }

      Console.Write("Enter habitat (e.g., forest, ocean, desert): ");
      string habitat = Console.ReadLine();

      Console.Write("Enter diet type (predator, herbivore, omnivorous): ");
      string diet = Console.ReadLine();

      Console.Write("Enter weight in kg (decimal number allowed): ");
      if (!double.TryParse(Console.ReadLine(), out double weight))
      {
        Console.WriteLine("Invalid weight. Operation cancelled.");
        return;
      }

      Console.Write("Enter color (letters only): ");
      string color = Console.ReadLine();

      if (!IsOnlyLetters(color))
      {
        Console.WriteLine("Invalid color. Color must contain only letters. Operation cancelled.");
        return;
      }

      Animal newAnimal = null;

      try
      {
        switch (selectedType)
        {
          case MammalType:
            Console.Write("Has fur? (yes/no): ");
            bool hasFur;
            if (!TryGetYesNoInput(out hasFur))
            {
              Console.WriteLine("Invalid input. Expected answer starting with 'y' (yes) or 'n' (no). Operation cancelled.");
              return;
            }
            newAnimal = new Mammal(name, age, habitat, diet, weight, color, hasFur);
            break;

          case BirdType:
            Console.Write("Enter wingspan in meters (decimal number): ");
            if (!double.TryParse(Console.ReadLine(), out double wingSpan))
            {
              Console.WriteLine("Invalid wingspan. Operation cancelled.");
              return;
            }
            newAnimal = new Bird(name, age, habitat, diet, weight, color, wingSpan);
            break;

          case FishType:
            Console.Write("Enter water type (fresh/sea): ");
            string waterType = Console.ReadLine();
            newAnimal = new Fish(name, age, habitat, diet, weight, color, waterType);
            break;

          case ReptileType:
            Console.Write("Is venomous? (yes/no): ");
            bool isVenomous;
            if (!TryGetYesNoInput(out isVenomous))
            {
              Console.WriteLine("Invalid input. Expected answer starting with 'y' (yes) or 'n' (no). Operation cancelled.");
              return;
            }
            newAnimal = new Reptile(name, age, habitat, diet, weight, color, isVenomous);
            break;

          case AmphibianType:
            Console.Write("Enter skin moisture (e.g., moist, dry): ");
            string skinMoisture = Console.ReadLine();
            newAnimal = new Amphibian(name, age, habitat, diet, weight, color, skinMoisture);
            break;
        }

        if (newAnimal != null)
        {
          manager.AddAnimal(newAnimal);
          Console.WriteLine("New animal information:");
          Console.WriteLine(newAnimal.GetInfo());
        }
      }
      catch (Exception ex)
      {
        // Проверяются все исключения, чтобы программа не крашала
        Console.WriteLine($"Error creating animal: {ex.Message}");
      }
    }

    // out-параметр позволяет вернуть и успешность операции, и само значение, важна только первая буква - прощается "yes", "yeah", "yep"
    static bool TryGetYesNoInput(out bool result)
    {
      string input = Console.ReadLine().Trim().ToLower();
      result = false;

      if (string.IsNullOrEmpty(input))
      {
        return false;
      }

      char firstCharacter = input[0];

      if (firstCharacter == 'y')
      {
        result = true;
        return true;
      }
      else if (firstCharacter == 'n')
      {
        result = false;
        return true;
      }

      return false;
    }
  }
}
