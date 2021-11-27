using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    class StregsystemCLI : IStregsystemUI
    {
        private IStregsystem Stregsystem { get; set; }

        public StregsystemCLI(IStregsystem stregsystem)
        {
            Stregsystem = stregsystem;
            Stregsystem.UserBalanceWarning += LowBalance;
        }

        public void Close()
        {
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            DisplayErrorMessage("Den følgende kommando kunne ikke findes" + adminCommand);
        }

        public void DisplayAdminCommandSuccess()
        {
            DisplaySuccessMessage("Kommandoen var successfuld");
        }
        public void DisplayGeneralError(string errorString)
        {
            DisplayErrorMessage("Der skete følgende fejl: " + errorString);
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            DisplayErrorMessage("Brugeren " + user.GetUsername() + " har en for lav balance til at foretage transaktionen med varenummer " + product.GetId());
        }

        public void ResetConsoleColors()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void DisplayMainPage()
        {
            Console.CursorLeft = Console.BufferWidth / 2;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Stregsystem");
            ResetConsoleColors();
            Console.WriteLine();
            Console.WriteLine("Du kan \"sætte streger\" på to måder:");
            Console.WriteLine("1. Indtast dit brugernavn efterfulgt af et produktnummer");
            Console.WriteLine("2. Indtast dit brugernavn efterfulgt af et antal stykker du vil købe efterfulgt af et produktnummer");
            Console.CursorTop = Console.CursorTop + 3;
            DisplayProductList();
        }
        public void DisplayProductList()
        {
            List<Product> products = Stregsystem.GetActiveProducts();
            int longestName = 0;
            int longestId = 0;
            int longestPrice = 0;
            int totalLength = 0;
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].GetName().Length > longestName) longestName = products[i].GetName().Length;
                if (products[i].GetId().ToString().Length > longestId) longestId = products[i].GetId().ToString().Length;
                if (products[i].GetPrice().ToString().Length > longestPrice) longestPrice = products[i].GetPrice().ToString().Length;
            }
            totalLength = longestName + longestId + longestPrice;
            int j = Console.BufferWidth * 2 + 7;
            int js = totalLength;
            if (Console.BufferWidth  > totalLength * 2 + 7)
            {
                String titleRow = String.Format($"{{0,-{longestId}}} {{1,-{longestName}}} {{2,-{longestPrice}}} | {{0,-{longestId}}} {{1,-{longestName}}} {{2,-{longestPrice}}}", "ID", "Produkt", "Pris");
                Console.CursorLeft = (Console.BufferWidth - (totalLength * 2 + 7)) / 2;
                Console.WriteLine(titleRow);
                for (int i = 0; i < products.Count - 1; i += 2)
                {
                    String row = String.Format($"{{0,-{longestId}}} {{1,-{longestName}}} {{2,-{longestPrice}}} | {{3,-{longestId}}} {{4,-{longestName}}} {{5,-{longestPrice}}}",
                        products[i].GetId().ToString(),
                        products[i].GetName(),
                        products[i].GetPrice().ToString(),
                        products[i + 1].GetId().ToString(),
                        products[i + 1].GetName(),
                        products[i + 1].GetPrice().ToString());
                    int rowLength = row.Length;
                    Console.CursorLeft = (Console.BufferWidth - (totalLength * 2 + 7)) / 2;
                    Console.WriteLine(row);
                    if (products.Count - 3 == i)
                    {
                        String lastRow = String.Format($"{{0,-{longestId}}} {{1,-{longestName}}} {{2,-{longestPrice}}} |",
                            products[i + 2].GetId().ToString(),
                            products[i + 2].GetName(),
                            products[i + 2].GetPrice().ToString());
                        Console.CursorLeft = (Console.BufferWidth - (totalLength * 2 + 7)) / 2;
                        Console.WriteLine(lastRow);
                    }
                }
            }
            else
            {
                String titleRow = String.Format($"{{0,-{longestId}}} {{1,-{longestName}}} {{2,-{longestPrice}}} |", "ID", "Produkt", "Pris");
                Console.WriteLine(titleRow);
                for (int i = 0; i < products.Count - 1; i++)
                {

                    String row = String.Format($"{{0,-{longestId}}} {{1,-{longestName}}} {{2,-{longestPrice}}} |",
                                            products[i].GetId().ToString(),
                                            products[i].GetName(),
                                            products[i].GetPrice().ToString());
                    Console.WriteLine(row);
                }                
            }
        }          
  
        private void DisplayErrorMessage(string error)
        {
            Console.Clear();
            DisplayMainPage();            
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(error);
            ResetConsoleColors();
            ReceiveCommands();
        }
        
        private void DisplaySuccessMessage(string message)
        {
            Console.Clear();
            DisplayMainPage();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            ResetConsoleColors();
            ReceiveCommands();
        }
        public void DisplayProductNotFound(string product)
        {
            DisplayErrorMessage($"Produktet med id {product} findes ikke");
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            DisplayErrorMessage($"Kommandoen {command} har for mange argumenter");            
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {            
            DisplaySuccessMessage($"{transaction.GetUser().GetUsername()} har købt produkt {transaction.GetProduct().GetId().ToString()}");
        }

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            DisplaySuccessMessage($"{transaction.GetUser().GetUsername()} har købt produkt-nummer {transaction.GetProduct().GetId().ToString()} {count} gange");
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine("Sidste køb af: " + user);
            Console.WriteLine("Din saldo er: " + user.GetBalance());
            IEnumerable<Transaction> list = Stregsystem.GetTransactions(user, 10);
            foreach (Transaction transaction in list)
            {
                Console.WriteLine(transaction);
            }
            ReceiveCommands();
        }
        public event StregsystemEvent CommandEntered;
        public delegate void UserBalanceNotification(User user, decimal balance);
        public event UserBalanceNotification UserBalanceWarning;

        public void DisplayUserNotFound(string username)
        {
            DisplayErrorMessage("Bruger: " + username + " kunne ikke findes.");
        }

        public void Start()
        {
            try
            {
                Console.CursorSize = 100;
            }
            catch
            {

            }            
            DisplayMainPage();
            ReceiveCommands();
        }

        private void ReceiveCommands()
        {
            string command = "";
            while (command.Length == 0)
            {
                command = Console.ReadLine();
            }
            Console.Clear();
            CommandEntered?.Invoke(command);            
        }
        
        private void LowBalance(User user, Decimal amount)
        {
            DisplayErrorMessage($"Saldoen for {user.GetUsername()} er lav. Du har {amount}dkk tilbage.");
        }
    }
}
