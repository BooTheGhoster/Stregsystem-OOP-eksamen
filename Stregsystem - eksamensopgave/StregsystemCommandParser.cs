using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stregsystem___eksamensopgave
{
    delegate void AdminFunction(string[] arr);    
    class StregsystemCommandParser
    {        
        IStregsystemUI StregsystemUI;
        IStregsystem Stregsystem { get; }
        Dictionary<string, AdminFunction> AdminCommands; 
        public StregsystemCommandParser(IStregsystemUI stregsystemUI, IStregsystem stregsystem)
        {
            StregsystemUI = stregsystemUI;
            Stregsystem = stregsystem;
            AdminCommands = new()
            {
                { ":q", AdminQuit},
                { ":quit", AdminQuit },
                { ":activate", AdminActivate},
                { ":deactivate", AdminDeactivate },
                { ":crediton", AdminCreditOn },
                { ":creditoff", AdminCreditOff },
                { ":addcredits", AdminAddCredits },
            };            
        }

        

        private void AdminAddCredits(string[] arr)
        {
            try
            {
                User user = Stregsystem.GetUserByUsername(arr[1]);
                int amount = Convert.ToInt32(arr[2]);
                Stregsystem.AddCreditsToAccount(user, amount);
                StregsystemUI.DisplayAdminCommandSuccess();
            }catch(UserDoesNotExistException e)
            {
                StregsystemUI.DisplayUserNotFound(arr[1]);
            }catch(Exception e)
            {
                StregsystemUI.DisplayGeneralError("Admin kommandoen kunne ikke udføres");
            }
        }

        private void AdminCreditOff(string[] arr)
        {
            try
            {
                Product product = Stregsystem.GetProductByID(Convert.ToInt32(arr[1]));
                product.SetCanBeBoughtOnCredit(false);
                StregsystemUI.DisplayAdminCommandSuccess();
            }
            catch (ProductDoesNotExistException e)
            {
                StregsystemUI.DisplayProductNotFound(arr[1]);
            }
            catch (Exception e)
            {
                StregsystemUI.DisplayGeneralError("Admin kommandoen kunne ikke udføres");
            }
        }

        private void AdminCreditOn(string[] arr)
        {
            try
            {
                Product product = Stregsystem.GetProductByID(Convert.ToInt32(arr[1]));
                product.SetCanBeBoughtOnCredit(true);
                StregsystemUI.DisplayAdminCommandSuccess();
            }
            catch (ProductDoesNotExistException e)
            {
                StregsystemUI.DisplayProductNotFound(arr[1]);
            }
            catch (Exception e)
            {
                StregsystemUI.DisplayGeneralError("Admin kommandoen kunne ikke udføres");
            }
        }

        public void AdminQuit(string[] arr)
        {
            StregsystemUI.Close();
        }
        public void AdminActivate(string[] arr)
        {
            try
            {
                Product product = Stregsystem.GetProductByID(Convert.ToInt32(arr[1]));
                product.SetActive(true);
                StregsystemUI.DisplayAdminCommandSuccess();
            }
            catch (ProductDoesNotExistException e)
            {
                StregsystemUI.DisplayProductNotFound(arr[1]);
            }
            catch (Exception e)
            {
                
                StregsystemUI.DisplayGeneralError("Admin kommandoen kunne ikke udføres");
            }
        }
        public void AdminDeactivate(string[] arr)
        {
            try
            {
                Product product = Stregsystem.GetProductByID(Convert.ToInt32(arr[1]));
                product.SetActive(false);
                StregsystemUI.DisplayAdminCommandSuccess();
            }
            catch (ProductDoesNotExistException e)
            {
                StregsystemUI.DisplayProductNotFound(arr[1]);
            }
            catch (Exception e)
            {
                StregsystemUI.DisplayGeneralError("Admin kommandoen kunne ikke udføres");
            }
        }

        public void ParseCommand(string command)
        {
            if (command[0] == ':') ParseAdminCommand(command);
            else ParseUserCommand(command);

        }
        private void ParseUserCommand(string command)
        {
            var arrOfWords = command.Split(" ");
            if (arrOfWords.Length > 3) StregsystemUI.DisplayTooManyArgumentsError(command);
            else
            {
                try
                {
                    User user = Stregsystem.GetUserByUsername(arrOfWords[0]);
                    if (arrOfWords.Length == 1)
                    {
                        StregsystemUI.DisplayUserInfo(user);
                    }
                    else if (arrOfWords.Length == 2)
                    {
                        Product product = Stregsystem.GetProductByID(Convert.ToInt32(arrOfWords[1]));
                        BuyTransaction transaction = Stregsystem.BuyProduct(user, product);
                        if (transaction != null)
                        {
                            StregsystemUI.DisplayUserBuysProduct(transaction);
                        }
                        else
                        {
                            StregsystemUI.DisplayInsufficientCash(user, product);
                        }
                    }
                    else
                    {
                        Product product = Stregsystem.GetProductByID(Convert.ToInt32(arrOfWords[2]));
                        int amount = Convert.ToInt32(arrOfWords[1]);
                        BuyTransaction transaction = Stregsystem.BuyProduct(user, product, amount);
                        StregsystemUI.DisplayUserBuysProduct(amount, transaction);
                    }
                }
                catch (ProductDoesNotExistException e)
                {
                    if (arrOfWords.Length == 2) StregsystemUI.DisplayProductNotFound(arrOfWords[1]);
                    else StregsystemUI.DisplayProductNotFound(arrOfWords[2]);
                }
                catch (ProductNotActiveException e)
                {
                    if (arrOfWords.Length == 2) StregsystemUI.DisplayProductNotFound(arrOfWords[1]);
                    else StregsystemUI.DisplayProductNotFound(arrOfWords[2]);
                }
                catch (UserDoesNotExistException e)
                {
                    StregsystemUI.DisplayUserNotFound(arrOfWords[0]);
                }catch (InsufficientCreditsException e)
                {
                    if (arrOfWords.Length == 2) StregsystemUI.DisplayInsufficientCash(
                        Stregsystem.GetUserByUsername(arrOfWords[0]), 
                        Stregsystem.GetProductByID(Convert.ToInt32(arrOfWords[1])));
                    else StregsystemUI.DisplayInsufficientCash(
                        Stregsystem.GetUserByUsername(arrOfWords[0]),
                        Stregsystem.GetProductByID(Convert.ToInt32(arrOfWords[2])));
                }
            }           
        }

        private void ParseAdminCommand(string command)
        {            
            try
            {
                var commandArr = command.Split(" ");
                AdminCommands[commandArr[0]](commandArr);
            }
            catch
            {
                StregsystemUI.DisplayAdminCommandNotFoundMessage(command);
            }
        }
    }
}
