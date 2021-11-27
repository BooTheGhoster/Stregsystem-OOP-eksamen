namespace Stregsystem___eksamensopgave
{
    public delegate void StregsystemEvent(string command);
    internal interface IStregsystemUI
    {

        public void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product); 
        public void DisplayUserInfo(User user); 
        public void DisplayTooManyArgumentsError(string command); 
        public void DisplayAdminCommandNotFoundMessage(string adminCommand); 
        public void DisplayUserBuysProduct(BuyTransaction transaction); 
        public void DisplayUserBuysProduct(int count, BuyTransaction transaction); 
        public void Close(); 
        public void DisplayInsufficientCash(User user, Product product); 
        public void DisplayGeneralError(string errorString);
        public void DisplayProductList();
        public void DisplayMainPage();
        public void DisplayAdminCommandSuccess();
        public void Start(); 


        public event StregsystemEvent CommandEntered
        {
            add
            {
                CommandEntered -= value;
                CommandEntered += value;
            }
            remove
            {
                CommandEntered -= value;
            }
        }
        
        
    }
}