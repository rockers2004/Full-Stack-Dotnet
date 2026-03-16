using System;
using book;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("----- Creating a Book Object -----");

        // 1. Create instances (objects) of the Book class using the constructor
        Book book1 = new Book("The Hitchhiker's Guide to the Galaxy", "Douglas Adams", 1979);
        Book book2 = new Book("Pride and Prejudice", "Jane Austen", 1813);
        Book book3 = new Book("1984", "George Orwell", 1949);

        Console.WriteLine("\n----- Accessing and Modifying Properties (demonstrating encapsulation) -----");

        // 2. Demonostrate encapsulation by accessing and moodifying properties 
        // Accessing properties using the 'get' accessor
        Console.WriteLine($"Book 1: {book1.Title} by {book1.Author}, published in {book1.PublicationYear}");

        // Modifying properties using the 'set' accessor(which includes validation)
        Console.WriteLine("\nAttempting to update Book 3's publication year...");
        book3.PublicationYear = 1950; // Valid update
        Console.WriteLine($"Book 3 updated year: {book3.PublicationYear}");

        Console.WriteLine("\nAttempting to set and invalid title for Book 1...");
        book1.Title = ""; // Invalid update, should trigger validation message
        Console.WriteLine($"Book 1 title after invalid update attempt: {book1.Title}"); // Title should remain unchanged

        Console.WriteLine("\n----- Using Class Methods -----");

        // 3. Using Methods defined in the Book class to display information about the book
        Console.WriteLine("\nDisplaying information for Book 1:");
        book1.DisplayInfo();
        Console.WriteLine("\nDisplaying information for Book 2:");
        book2.DisplayInfo();
        Console.WriteLine("\nDisplaying information for Book 3:");
        book3.DisplayInfo();

        book1.ReadBook(50); // Simulate reading 50 pages of Book 1
        book2.ReadBook(100); // Simulate reading 100 pages of Book 2

        Console.WriteLine("\n----- End of Book App Demo -----");
    }
}