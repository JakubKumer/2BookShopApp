using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _2BookShopApp
{
    internal class BookStore
    {
        private readonly BookShopDbContext dbContext;
        private User loggedInUser;
        private const string FilePath = @"C:\Users\freez\source\repos\E-BookShopapp\E-BookShopapp\bin\Debug\net6.0\books.json";

        public BookStore(BookShopDbContext context)
        {
            dbContext = context;
        }

        public void Login()
        {
            while (true)
            {
                Console.WriteLine("Wprowadź login:");
                string username = Console.ReadLine();
                Console.WriteLine("Wprowadź hasło:");
                string password = Console.ReadLine();
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.WriteLine(".");
                Thread.Sleep(1000);
                Console.Clear();

                var verifyUser = dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (verifyUser != null)
                {
                    loggedInUser = verifyUser;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Zalogowano pomyślnie {username}");
                    Console.ResetColor();
                    LoadBooksFromDatabase();
                    while (true)
                    {
                        if (loggedInUser is Employee)
                        {
                            Console.WriteLine("1. Dodaj Książke");
                            Console.WriteLine("2. Lista Książke");                            
                            Console.WriteLine("3. Usuń Książke");
                            Console.WriteLine("4. Wczytaj Książki z Json");
                            Console.WriteLine("5. Wyjście");
                            string choice = Console.ReadLine();
                            Console.Clear();
                            switch (choice)
                            {
                                case "1":
                                    AddBook();
                                    break;
                                case "2":
                                    ListBooks();
                                    break;
                                case "3":
                                    DeleteBook();
                                    break;
                                case "4":
                                    ImportDataFromJson();
                                    break;
                                case "5":
                                    return;
                                default:
                                    Console.WriteLine("Wybierz z przedziału 1-5.");
                                    break;
                            }

                        }
                        else
                        {
                            LoadBooksFromDatabase();
                            Console.WriteLine("1. Kup książke");
                            Console.WriteLine("2. Wyświetl dostępne książki");
                            Console.WriteLine("3. Wyjdź");
                            string choice = Console.ReadLine();
                            Console.Clear();
                            switch (choice)
                            {
                                case "1":
                                    BuyBook();
                                    break;
                                case "2":
                                    ListBooks();
                                    break;
                                case "3":
                                    return;
                                default:
                                    Console.WriteLine("Wybierz z przedziału 1-3.");
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    var userExists = dbContext.Users.Any(u => u.Username == username);
                    if (userExists)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nieprawidłowe hasło.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nieprawidłowa nazwa użytkownika.");
                        Console.ResetColor();
                    }
                }
            }
        }

        public void AddBook()
        {
            Console.WriteLine("Wprowadź tytuł książki:");
            string title = Console.ReadLine();

            Console.WriteLine("Podaj imie i nazwisko autora:");
            string author = Console.ReadLine();
            while (true)
            {
                try
                {
                    Console.WriteLine("Podaj rok wydania:");
                    int publicationYear = int.Parse(Console.ReadLine());
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Wprowadź cenę:");
                            decimal price = decimal.Parse(Console.ReadLine());
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Książka dodana do listy");
                            Console.ResetColor();
                            Console.Write(".");
                            Thread.Sleep(1000);
                            Console.Write(".");
                            Thread.Sleep(1000);
                            Console.WriteLine(".");
                            Thread.Sleep(1000);
                            Console.Clear();

                            Book book = new Book()
                            {
                                Title = title,
                                Author = author,
                                PublicationYear = publicationYear,
                                Price = price
                            };

                            dbContext.Books.Add(book);
                            dbContext.SaveChanges();
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Podaj cenę używając cyfr. Możesz używać przecinka do wpisania części dziesiętnej :)");
                            Console.ResetColor();
                        }
                    }
                    break;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Podaj rok wprowadzając same cyfry :)");
                    Console.ResetColor();
                }
            }
        }

        public void DeleteBook()
        {
            Console.Clear();
            LoadBooksFromDatabase();
            Console.WriteLine("==========================================Lista=Książek==========================================");
            foreach (var book in dbContext.Books)
            {
                Console.WriteLine($"Tytuł: {book.Title}| Autor: {book.Author}| Rok wydania: {book.PublicationYear} |Cena: {book.Price}");
            }
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("Podaj tytuł książki, którą chcesz usunąć");
            string title = Console.ReadLine();
            var bookToRemove = dbContext.Books.FirstOrDefault(b => b.Title.Equals(title));
            if (bookToRemove != null)
            {
                Console.WriteLine("Czy napewno chcesz usunąć tę książkę? Y/dowolny przycisk");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    dbContext.Books.Remove(bookToRemove);
                    dbContext.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Książka o tytule {bookToRemove.Title} została usunięta z listy");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Console.WriteLine();
                    Console.Clear();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Anulowałeś usuwanie książki z listy");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Console.WriteLine();
                    Console.Clear();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nie znaleziono książki o podanym tytule");
                Console.ResetColor();
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.Write(".");
                Console.WriteLine();
                Console.Clear();
            }
        }

        public void BuyBook()
        {
            Console.Clear();
            LoadBooksFromDatabase();
            Console.WriteLine("==========================================Lista=Książek==========================================");
            foreach (var book in dbContext.Books)
            {
                Console.WriteLine($"Tytuł: {book.Title}| Autor: {book.Author}| Rok wydania: {book.PublicationYear} |Cena: {book.Price}");
            }
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("Podaj tytuł książki, którą chcesz kupić");
            string title = Console.ReadLine();
            var bookToBuy = dbContext.Books.FirstOrDefault(b => b.Title.Equals(title));
            if (bookToBuy != null)
            {
                Console.WriteLine("Czy napewno chcesz kupić tę książkę? Y/dowolny przycisk");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    dbContext.Books.Remove(bookToBuy);
                    dbContext.SaveChanges();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Książka o tytule {bookToBuy.Title} została zakupiona :)");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Console.WriteLine();
                    Console.Clear();
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Anulowałeś zakup książki");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Thread.Sleep(1000);
                    Console.Write(".");
                    Console.WriteLine();
                    Console.Clear();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nie znaleziono książki o podanym tytule");
                Console.ResetColor();
            }
        }

        public void ListBooks()
        {
            Console.Clear();
            LoadBooksFromDatabase();
            Console.WriteLine("==========================================Lista=Książek==========================================");
            foreach (var book in dbContext.Books)
            {
                Console.WriteLine($"Tytuł: {book.Title}| Autor: {book.Author}| Rok wydania: {book.PublicationYear} |Cena: {book.Price}");
            }
            Console.WriteLine("=================================================================================================");
        }

        private void LoadBooksFromDatabase()
        {
            dbContext.Books.ToList();
        }
        public void ImportDataFromJson()
        {
            using (var db = new BookShopDbContext())
            {
                var jsonString = File.ReadAllText(FilePath);
                var importedBooks = JsonSerializer.Deserialize<List<Book>>(jsonString);
                db.Books.AddRange(importedBooks);
                db.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Dane z pliku JSON zostały zaimportowane do bazy danych.");
                Console.ResetColor();
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.Write(".");
                Thread.Sleep(1000);
                Console.Write(".");
                Console.WriteLine();
                Console.Clear();
            }
        }
    }
}

