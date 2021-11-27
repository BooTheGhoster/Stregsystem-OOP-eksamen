using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Stregsystem___eksamensopgave
{
    class TransactionLogger
    {
        private string Filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\log.txt";
        private bool IsFileMade;
        private List<Transaction> TransactionsToLog { get; set; }
        public TransactionLogger()
        {
            TransactionsToLog = new();
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(Filepath))
                {
                    File.Delete(Filepath);
                }

                // Create a new file     
                using (FileStream fs = File.Create(Filepath))
                IsFileMade = true;
            }
            catch {
                IsFileMade = false;
            }
        }

        private void AddToTransactionsToLog(Transaction transaction)
        {
            TransactionsToLog.Add(transaction);
        }

        public async Task<List<Transaction>> LogTransactionAsync(Transaction transaction)
        {
            AddToTransactionsToLog(transaction);
            List<String> transactionsToLogAsStrings = new();
            foreach (Transaction transactionIterator in TransactionsToLog)
            {
                transactionsToLogAsStrings.Add(transactionIterator.ToString());
            }
            List<Transaction> returnList = new();
            if (IsFileMade)
            {
                try //Bad things happen if some things are written to the log and it crashes but we assume it does not for now
                {
                    using StreamWriter file = new(Filepath, append: true);
                    {
                        foreach (String line in transactionsToLogAsStrings)
                        {

                            await file.WriteLineAsync(line); 
                                //WriteLine(Filepath, line);
                        }
                    }
                    
                    
                    returnList.AddRange(TransactionsToLog);
                    TransactionsToLog.Clear();
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    using (FileStream fs = File.Create(Filepath))
                    {
                        File.WriteAllLines(Filepath, transactionsToLogAsStrings);
                    }
                    returnList.AddRange(TransactionsToLog);
                    IsFileMade = true;
                    TransactionsToLog.Clear();
                }
                catch
                {

                }
            }
            return returnList;
        }
    }
}
