using System;
public class Car
{
    private string color;
    public string Color
    {
        get { return color; }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                color = value;
            }
            else
            {
                Console.WriteLine("Color cannot be empty.");
            }
        }
    }

    public string Model { get; set; } // Auto-implemented property

    public Car(string carColor, string carModel)
    {
        Color = carColor;
        Model = carModel;
    }

    public void StartEngine()
    {
        Console.WriteLine($"The {Color} {Model}'s engine is starting.");
    }
}

// Usage:
public class Program
{
    public static void Main(string[] args)
    {
Car myCar = new Car("Blue", "Sedan")
{
    // Valid modification via public property
    Color = "Green"
};

// Invalid modification attempt (will fail if 'color' was public, but here it's handled by the setter)
myCar.Color = ""; // Triggers validation: "Color cannot be empty."

// Direct access to private field 'color' is not allowed from outside the class.
// myCar.color = "Red"; // This would cause a compile-time error.
    }
}