using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalZoo
{
  public enum AnimalType
  {
    Mammal = 1,
    Bird = 2,
    Fish = 3,
    Reptile = 4,
    Amphibian = 5
  }

  public enum MenuOption
  {
    ShowAllAnimals = 1,
    SearchByName = 2,
    ShowByIndex = 3,
    AddNewAnimal = 4,
    Exit = 5
  }

  public abstract class Animal
  {
    public string name { get; set; }
    public int age { get; set; }
    public string habitat { get; set; }
    public string diet { get; set; }
    public double weight { get; set; }
    public string color { get; set; }

    protected Animal(string animalName, int animalAge, string animalHabitat, string animalDiet, double animalWeight, string animalColor)
    {
      name = animalName;
      age = animalAge;
      habitat = animalHabitat;
      diet = animalDiet;
      weight = animalWeight;
      color = animalColor;
    }

    public virtual string GetInfo()
    {
      return $"Name: {name}, Age: {age}, Habitat: {habitat}, " +
             $"Diet: {diet}, Weight: {weight} kg, Color: {color}";
    }

    public abstract string GetAnimalType();

    public virtual bool IsEqualTo(Animal other)
    {
      if (other == null)
      {
        return false;
      }

      return this.name.ToLower() == other.name.ToLower() &&
             this.age == other.age &&
             this.habitat.ToLower() == other.habitat.ToLower() &&
             this.GetType() == other.GetType();
    }
  }

  public class Mammal : Animal
  {
    public bool hasFur { get; set; }

    public Mammal(string animalName, int animalAge, string animalHabitat, string animalDiet, double animalWeight, string animalColor, bool animalHasFur)
      : base(animalName, animalAge, animalHabitat, animalDiet, animalWeight, animalColor)
    {
      hasFur = animalHasFur;
    }

    public override string GetInfo()
    {
      string furStatus;
      furStatus = hasFur ? "yes" : "no";
      return base.GetInfo() + $", Type: Mammal, Fur: {furStatus}";
    }

    public override string GetAnimalType()
    {
      return "Mammal";
    }

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
      return this.hasFur == otherMammal.hasFur;
    }
  }

  public class Bird : Animal
  {
    public double wingSpan { get; set; }

    public Bird(string animalName, int animalAge, string animalHabitat, string animalDiet, double animalWeight, string animalColor, double animalWingSpan)
      : base(animalName, animalAge, animalHabitat, animalDiet, animalWeight, animalColor)
    {
      wingSpan = animalWingSpan;
    }

    public override string GetInfo()
    {
      return base.GetInfo() + $", Type: Bird, Wingspan: {wingSpan} m";
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
      return Math.Abs(this.wingSpan - otherBird.wingSpan) < 0.01;
    }
  }

  public class Fish : Animal
  {
    public string waterType { get; set; }

    public Fish(string animalName, int animalAge, string animalHabitat, string animalDiet, double animalWeight, string animalColor, string animalWaterType)
      : base(animalName, animalAge, animalHabitat, animalDiet, animalWeight, animalColor)
    {
      waterType = animalWaterType;
    }

    public override string GetInfo()
    {
      return base.GetInfo() + $", Type: Fish, Water type: {waterType}";
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
      return this.waterType.ToLower() == otherFish.waterType.ToLower();
    }
  }

  public class Reptile : Animal
  {
    public bool isVenomous { get; set; }

    public Reptile(string animalName, int animalAge, string animalHabitat, string animalDiet, double animalWeight, string animalColor, bool animalIsVenomous)
      : base(animalName, animalAge, animalHabitat, animalDiet, animalWeight, animalColor)
    {
      isVenomous = animalIsVenomous;
    }

    public override string GetInfo()
    {
      string venomStatus;
      venomStatus = isVenomous ? "yes" : "no";
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
      return this.isVenomous == otherReptile.isVenomous;
    }
  }

  public class Amphibian : Animal
  {
    public string skinMoisture { get; set; }

    public Amphibian(string animalName, int animalAge, string animalHabitat, string animalDiet, double animalWeight, string animalColor, string animalSkinMoisture)
        : base(animalName, animalAge, animalHabitat, animalDiet, animalWeight, animalColor)
    {
      skinMoisture = animalSkinMoisture;
    }

    public override string GetInfo()
    {
      return base.GetInfo() + $", Type: Amphibian, Skin moisture: {skinMoisture}";
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
      return this.skinMoisture.ToLower() == otherAmphibian.skinMoisture.ToLower();
    }
  }

  public sealed class AnimalManager
  {
    private static readonly AnimalManager S_instance = new AnimalManager();
    private List<Animal> _animals;

    private AnimalManager()
    {
      _animals = new List<Animal>();
    }

    public static AnimalManager Instance
    {
      get
      {
        return S_instance;
      }
    }

    public int animalsCount
    {
      get
      {
        return _animals.Count;
      }
    }

    public bool AddAnimal(Animal animal)
    {
      foreach (var existingAnimal in _animals)
      {
        if (existingAnimal.IsEqualTo(animal))
        {
          Console.WriteLine($"Error: Animal {animal.name} already exists in the zoo!");
          return false;
        }
      }

      _animals.Add(animal);
      Console.WriteLine($"Animal {animal.name} successfully added to the zoo!");
      return true;
    }

    public void ShowAllAnimals()
    {
      Console.WriteLine("\n=== ANIMALS LIST ===");

      for (int animalIndex = 0; animalIndex < _animals.Count; ++animalIndex)
      {
        Console.WriteLine($"[{animalIndex + 1}] {_animals[animalIndex].GetInfo()}");
      }
    }

    public void ShowAnimalByName(string searchQuery)
    {
      string lowerCaseSearchQuery;
      lowerCaseSearchQuery = searchQuery.ToLower();
      var foundAnimals = _animals.Where(animal =>
          animal.name.ToLower().Contains(lowerCaseSearchQuery)).ToList();

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
        Console.WriteLine(
          "\n=== MAIN MENU ===\n" +
          "1. Show all animals\n" +
          "2. Search animal by name\n" +
          "3. Show animal by number\n" +
          "4. Add new animal\n" +
          "5. Exit\n" +
          "Choose action (1-5): "
        );

        string userChoice;
        userChoice = Console.ReadLine();

        if (int.TryParse(userChoice, out int parsedChoice))
        {
          switch (parsedChoice)
          {
            case (int)MenuOption.ShowAllAnimals:
              manager.ShowAllAnimals();
              break;

            case (int)MenuOption.SearchByName:
              Console.Write("Enter animal name (or part of name): ");
              string searchName;
              searchName = Console.ReadLine();
              manager.ShowAnimalByName(searchName);
              break;

            case (int)MenuOption.ShowByIndex:
              Console.Write($"Enter animal number (from 1 to {manager.animalsCount}): ");
              string animalNumberInput;
              animalNumberInput = Console.ReadLine();

              if (int.TryParse(animalNumberInput, out int animalNumber))
              {
                manager.ShowAnimalByIndex(animalNumber - 1);
              }
              else
              {
                Console.WriteLine("Invalid input. Please enter a number.");
              }
              break;

            case (int)MenuOption.AddNewAnimal:
              AddNewAnimal(manager);
              break;

            case (int)MenuOption.Exit:
              Console.WriteLine("Exiting program. Goodbye!");
              return;

            default:
              Console.WriteLine("Invalid choice. Please try again.");
              break;
          }
        }
      }

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
        Console.WriteLine(
          "\n=== ADDING NEW ANIMAL ===\n" +
          "Select animal type:\n" +
          "1. Mammal\n" +
          "2. Bird\n" +
          "3. Fish\n" +
          "4. Reptile\n" +
          "5. Amphibian\n" +
          "Your choice (1-5): "
        );

        string selectedType;
        selectedType = Console.ReadLine();

        if (!int.TryParse(selectedType, out int typeChoice) ||
            !Enum.IsDefined(typeof(AnimalType), typeChoice))
        {
          Console.WriteLine("Invalid animal type! Operation cancelled.");
          return;
        }

        Console.Write("Enter name: ");
        string name;
        name = Console.ReadLine();

        Console.Write("Enter age (integer): ");
        string ageInput;
        ageInput = Console.ReadLine();

        if (!int.TryParse(ageInput, out int age))
        {
          Console.WriteLine("Invalid age. Operation cancelled.");
          return;
        }

        Console.Write("Enter habitat (e.g., forest, ocean, desert): ");
        string habitat;
        habitat = Console.ReadLine();

        Console.Write("Enter diet type (predator, herbivore, omnivorous): ");
        string diet;
        diet = Console.ReadLine();

        Console.Write("Enter weight in kg (decimal number allowed): ");
        string weightInput;
        weightInput = Console.ReadLine();

        if (!double.TryParse(weightInput, out double weight))
        {
          Console.WriteLine("Invalid weight. Operation cancelled.");
          return;
        }

        Console.Write("Enter color (letters only): ");
        string color;
        color = Console.ReadLine();

        if (!IsOnlyLetters(color))
        {
          Console.WriteLine("Invalid color. Color must contain only letters. Operation cancelled.");
          return;
        }

        Animal newAnimal = null;

        try
        {
          AnimalType animalTypeEnum = (AnimalType)int.Parse(selectedType);

          switch (animalTypeEnum)
          {
            case AnimalType.Mammal:
              Console.Write("Has fur? (yes/no): ");
              string hasFurInput;
              hasFurInput = Console.ReadLine();
              bool hasFur;

              if (!TryGetYesNoInput(hasFurInput, out hasFur))
              {
                Console.WriteLine("Invalid input. Expected answer starting with 'y' (yes) or 'n' (no). Operation cancelled.");
                return;
              }
              newAnimal = new Mammal(name, age, habitat, diet, weight, color, hasFur);
              break;

            case AnimalType.Bird:
              Console.Write("Enter wingspan in meters (decimal number): ");
              string wingSpanInput;
              wingSpanInput = Console.ReadLine();

              if (!double.TryParse(wingSpanInput, out double wingSpan))
              {
                Console.WriteLine("Invalid wingspan. Operation cancelled.");
                return;
              }
              newAnimal = new Bird(name, age, habitat, diet, weight, color, wingSpan);
              break;

            case AnimalType.Fish:
              Console.Write("Enter water type (fresh/sea): ");
              string waterType;
              waterType = Console.ReadLine();
              newAnimal = new Fish(name, age, habitat, diet, weight, color, waterType);
              break;

            case AnimalType.Reptile:
              Console.Write("Is venomous? (yes/no): ");
              string isVenomousInput;
              isVenomousInput = Console.ReadLine();
              bool isVenomous;

              if (!TryGetYesNoInput(isVenomousInput, out isVenomous))
              {
                Console.WriteLine("Invalid input. Expected answer starting with 'y' (yes) or 'n' (no). Operation cancelled.");
                return;
              }
              newAnimal = new Reptile(name, age, habitat, diet, weight, color, isVenomous);
              break;

            case AnimalType.Amphibian:
              Console.Write("Enter skin moisture (e.g., moist, dry): ");
              string skinMoisture;
              skinMoisture = Console.ReadLine();
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
          Console.WriteLine($"Error creating animal: {ex.Message}");
        }
      }

      static bool TryGetYesNoInput(string input, out bool result)
      {
        string processedInput = input.Trim().ToLower();
        result = false;

        if (string.IsNullOrEmpty(processedInput))
        {
          return false;
        }

        char firstCharacter = processedInput[0];

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
}