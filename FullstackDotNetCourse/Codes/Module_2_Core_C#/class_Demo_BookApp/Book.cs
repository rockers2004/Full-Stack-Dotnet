using System;
namespace book
{
    public class Book
    {
       //Private backing fields for properties
       private string title;
       private string author;
       private int publicationYear;
       private int ISBN;

       //public properties with get and set accessors
       public string Title
        {
            get { return title; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    title = value;
                }
                else
                {
                    Console.WriteLine("Title cannot be empty.");
                }
            }
        }

        public string Author
        {
            get { return author; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    author = value;
                }
                else
                {
                    Console.WriteLine("Author cannot be empty.");
                }
            }
        }

        public int PublicationYear
        {
            get { return publicationYear; }
            set
            {
                if (value > 0)
                {
                    publicationYear = value;
                }
                else
                {
                    Console.WriteLine("Publication year must be a positive integer.");
                }
            }
        }

        public int ISBN
        {
            get { return ISBN; }
            set
            {
                if (value > 0)
                {
                    ISBN = value;
                }
                else
                {
                    Console.WriteLine("ISBN must be a positive integer.");
                }
            }
        }

        // Constructor to initialize the book object
        public Book(string title, string author, int publicationYear, int ISBN)
        {
            Title = title;
            Author = author;
            PublicationYear = publicationYear;
            this.ISBN = ISBN;
            Console.WriteLine($"Book '{Title}' by {Author} published in {PublicationYear} created.");
        }

        // Method to display book details
        public void DisplayInfo()
        {
            Console.WriteLine($"----------------------------");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Author: {Author}");
            Console.WriteLine($"Publication Year: {PublicationYear}");
            Console.WriteLine($"ISBN: {ISBN}");
            Console.WriteLine($"----------------------------");
        }

        //Method to simulate reading the book
        public void ReadBook(int pagesRead)
        {
            Console.WriteLine($"you have read {pagesRead} pages of '{Title}'. Keep reading!");
        }

        // Method to calculate and display the age of the book
        public void GetBookAge()
        {
            int currentYear = DateTime.Now.Year;
            int bookAge = currentYear - PublicationYear;
            Console.WriteLine($"'{Title}' is {bookAge} years old.");
        }
    }
}