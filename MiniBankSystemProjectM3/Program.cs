using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

namespace MiniBankSystemProject
{
    internal class Program
    {


        // Account Request Queue
        // Stores pending account creation requests in FIFO order 
        public static Queue<string> requestCreateAccountsInfo = new Queue<string>();

        // Customer Reviews Stack
        // Stores submitted reviews in LIFO order (most recent first)
        public static Stack<string> submittedReviews = new Stack<string>();

        public static Queue<int> adm = new Queue<int>();


        // Unique account identifiers
        static List<int> accountNumbers = new List<int>();
        // Customer full names
        static List<string> accountNames = new List<string>();
        // Current account balances
        static List<double> balances = new List<double>();

        // Links to accountNumbers

        public static List<int> transactionAccountNumbers = new List<int>();
        // Format: yyyy-MM-dd HH:mm:ss
        public static List<string> transactionTimestamps = new List<string>();
        // "DEPOSIT"/"WITHDRAWAL"
        public static List<string> transactionTypes = new List<string>();
        // Transaction value
        public static List<double> transactionAmounts = new List<double>();
        // Balance after transaction
        public static List<double> transactionBalances = new List<double>();



        // Change these declarations:
        public static string userDataPath = "UserAccounts.txt";
        public static string accountRequestsPath = "AccountRequests.txt";  // New separate file

       
        // Customer reviews storage
        public static string pathFile = "reviews.txt";
        // Transaction history storage
        public static string transactionsFile = "transactions.txt";
        // store admin accounts
        public static string pathAdmin = "admin.txt";
        // store national ID path
        public static string nationalIDPath = "nationalIDs.txt";


        // Base account number (increments for new accounts)
        public static int userCordNumber = 12062000;
        // Starting balance for new accounts
        public static int defaultBalances = 5;


        static void Main(string[] args)
        {
            // display home page in main funaction
            HomePage();
        }






        //++++++++++++++++++++++++++++++++ Home Page +++++++++++++++++++++++++++++++++++++++++


        public static void HomePage()
        {
            // load list of reviews
            LoadReviews();
            //load user data
            LoadUserData();
            // load transactions 
            LoadTransactions();
            // load account requests
            LoadAccountRequest();

            //load admin
            LoadAdmi();

            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("         Welcome To One Piece Bank System          \n ");
            Console.WriteLine("**************************************************************\n");



            // use while loop show main contante  and user can selecte opiton  

            bool Flag = true;
            while (Flag)
            {
                // Display menu options to the user

                Console.WriteLine("Select One Option");
                Console.WriteLine("1-User ");
                Console.WriteLine("2-Admin");
                Console.WriteLine("3-Exit");

                Console.WriteLine("Enter Number");

                // Input validation loop - ensures user enters a valid integer

                int number;
                while (!int.TryParse(Console.ReadLine(), out number))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
                // Process user selection using switch statement

                switch (number)
                {
                    case 1:
                        //UserIU();
                        logeInSystem();
                        break;
                    case 2:
                        logeInSystem();

                        //AdminIU();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("See Next Time My Black Cat");
                        Flag = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Number Not found: " + number + "\n");
                        break;
                }
            }
        }


        //++++++++++++++++++++++++++++++++ Login +++++++++++++++++++++++++++++++++++++++++


        // Login System for Users and admin
        public static void logeInSystem()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                          Login in Page                            \n ");
            Console.WriteLine("**************************************************************\n");

            Console.Write("Enter Account Number: ");
            // Input validation 
            int number;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }

            // check account number for users and admin if found or not found 
            if (accountNumbers.Contains(number))
            {
                // if found account number of user open user Interface
                Console.WriteLine("Account found");
                UserIU();
            }
            else if (adm.Contains(number))
            {
                // if found account number of admin open admin Interface
                Console.WriteLine("Account found.");
                AdminIU();
            }
            else
            {
                Console.WriteLine("\nAccount not found.");
                Console.Write("Would you like to create a new account? (Y/N): ");
                string response = Console.ReadLine().ToUpper();

                if (response == "Y")
                {
                    requestCreateAccounts();
                }
                else
                {
                    Console.WriteLine("\nReturning to main menu...");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }





        //++++++++++++++++++++++++++++++++ User Interface +++++++++++++++++++++++++++++++++++++++++
        public static void UserIU()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                          User Page                            \n ");
            Console.WriteLine("**************************************************************\n");

            /// Displays the User Interface Menu and handles user interactions
            bool Flag = true;
            while (Flag)
            {
                Console.WriteLine("Select One Option");
                Console.WriteLine("1- Create accounts ");
                Console.WriteLine("2- Deposit Money ");
                Console.WriteLine("3- Withdraw Money ");
                Console.WriteLine("4- View balances");
                Console.WriteLine("5- View transaction history");
                Console.WriteLine("6- Submit Review");
                Console.WriteLine("0- Back to Home");

                Console.WriteLine("Enter Number");

                int number;
                while (!int.TryParse(Console.ReadLine(), out number))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
                // Process user selection

                switch (number)
                {
                    case 1:
                        requestCreateAccounts();
                        break;
                    case 2:
                        depostitMoney();
                        break;
                    case 3:
                        withdrawMoney();
                        break;
                    case 4:
                        viewBalances();
                        break;
                    case 5:
                        viewTransactionHistory();
                        break;
                    case 6:
                        submitReview();
                        break;
                    case 0:
                        Console.Clear();
                        Flag = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Number Not found: " + number + "\n");
                        break;
                }
            }
        }

        // 1 ########################## Request For Create Account ################################### 
        public static void requestCreateAccounts()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                  Request Create Account Page                 \n ");
            Console.WriteLine("**************************************************************\n");

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Name cannot be empty");
                return;
            }

            Console.Write("Enter National ID: ");
            string nationalID;
            do
            {
                nationalID = Console.ReadLine();
                if (string.IsNullOrEmpty(nationalID))
                {
                    Console.WriteLine("National ID cannot be empty. Please enter again:");
                }
                else if (IsDuplicateNationalID(nationalID))
                {
                    Console.WriteLine("This National ID is already registered. Please enter a different one:");
                    nationalID = string.Empty;
                }
            } while (string.IsNullOrEmpty(nationalID));

            // Store as "Name|NationalID"
            string userInfo = $"{name}|{nationalID}";
            requestCreateAccountsInfo.Enqueue(userInfo);

            // Save the national ID
            SaveNationalID(nationalID);

            Console.WriteLine("Your account request has been submitted for admin approval.");
            SaveAccountRequest();
        }



        // Check for duplicate national IDs
        public static bool IsDuplicateNationalID(string nationalID)
        {
            if (!File.Exists(nationalIDPath)) return false;

            string[] existingIDs = File.ReadAllLines(nationalIDPath);
            return existingIDs.Contains(nationalID);
        }

        // Save a new national ID
        public static void SaveNationalID(string nationalID)
        {
            try
            {
                File.AppendAllText(nationalIDPath, nationalID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving national ID: {ex.Message}");
            }
        }

        // Load all national IDs (for verification)
        public static List<string> LoadNationalIDs()
        {
            List<string> ids = new List<string>();
            try
            {
                if (File.Exists(nationalIDPath))
                {
                    ids = File.ReadAllLines(nationalIDPath).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading national IDs: {ex.Message}");
            }
            return ids;
        }





        //  Prevent Duplicate Account Requests
        public static bool IsDuplicateRequest(string name, int idNumber)
        {
            foreach (var request in requestCreateAccountsInfo)
            {
                string[] parts = request.Split('|');
                if (parts.Length >= 2 && parts[0] == name && int.Parse(parts[1]) == idNumber)
                {
                    return true;
                }
            }
            return false;
        }



        //############################################## Deposit Money #####################################################
        public static void depostitMoney()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                          Deposit Page                         \n ");
            Console.WriteLine("**************************************************************\n");
            // Collect and validate Account number

            Console.Write("Enter Account Number: ");
            int accountNumber;
            while (!int.TryParse(Console.ReadLine(), out accountNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid account number.");
            }
            // User enter amount for deposit

            Console.Write("Enter Amount to Deposit: ");
            double amount;
            while (!double.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive amount.");
            }
            // Check if the account exists in our records

            if (accountNumbers.Contains(accountNumber))
            {
                int index = accountNumbers.IndexOf(accountNumber);
                // Update the account balance
                balances[index] += amount;
                Console.WriteLine($"Deposited {amount:C2}. New balance: {balances[index]:C2}");
                AddTransaction(accountNumber, "DEPOSIT", amount, balances[index]);
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
            // Save the updated data

            SaveUserData();
            // Wait for user to acknowledge before returning to menu
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();

        }

        //############################################## withdraw Money #####################################################

        public static void withdrawMoney()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                          Withdraw Page                        \n ");
            Console.WriteLine("**************************************************************\n");
            //  validate user account number

            Console.Write("Enter Account Number: ");
            int accountNumber;
            while (!int.TryParse(Console.ReadLine(), out accountNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid account number.");
            }
            //  validate input withdraw amount

            Console.Write("Enter Amount to Withdraw: ");
            double amount;
            while (!double.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive amount.");
            }


            // Check if the account number exists in the system
            if (accountNumbers.Contains(accountNumber))
            {
                // Get the index of the account number to access corresponding balance

                int index = accountNumbers.IndexOf(accountNumber);

                // Check if the account has sufficient funds for the withdrawal

                if (balances[index] >= amount)
                {
                    // Deduct the amount from the balance

                    balances[index] -= amount;
                    // Display success message with new balance

                    Console.WriteLine($"Withdrew {amount:C2}. New balance: {balances[index]:C2}");
                    AddTransaction(accountNumber, "WITHDRAWAL", amount, balances[index]);
                }
                else
                {
                    // Display error message if not enough funds

                    Console.WriteLine("Insufficient funds.");
                }
            }
            else
            {
                // Display error message if account doesn't exist

                Console.WriteLine("Account not found.");
            }
            SaveUserData();
            // Wait for user to acknowledge before returning to menu
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();

        }

        //############################################## view Balances #####################################################

        public static void viewBalances()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     VIEW ACCOUNT BALANCE                     ");
            Console.WriteLine("**************************************************************");
            // Check if there are no accounts in the system

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts found in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }
            // Prompt user to enter an account number

            Console.Write("\nEnter account number: ");
            int accountNumber;
            while (!int.TryParse(Console.ReadLine(), out accountNumber))
            {
                Console.Write("Invalid input. Please enter a valid account number: ");
            }
            // Search for the account number in the list

            int index = accountNumbers.IndexOf(accountNumber);

            // Check if the account was found 

            if (index >= 0)
            {
                // Display account info
                Console.WriteLine("\nAccount Details:");
                Console.WriteLine("----------------");
                Console.WriteLine($"Account Number: {accountNumbers[index]}");
                Console.WriteLine($"Account Holder: {accountNames[index]}");
                Console.WriteLine($"Current Balance: {balances[index]:C}");
            }
            else
            {// display message when not found account
                Console.WriteLine($"\nAccount number {accountNumber} not found.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }



        //############################################## View transaction history #####################################################


        public static void viewTransactionHistory()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   TRANSACTION HISTORY                        ");
            Console.WriteLine("**************************************************************");

            // check if account is exist 
            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }
            // Prompt user to enter an account number

            Console.Write("\nEnter account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            // Get the index of the account number to access corresponding balance

            int accountIndex = accountNumbers.IndexOf(accountNumber);

            if (accountIndex == -1)
            {// if account not found
                Console.WriteLine($"Account {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }
            // display account transaction history
            Console.WriteLine($"\nTransaction History for {accountNames[accountIndex]} (Account: {accountNumber})");
            Console.WriteLine($"Current Balance: {balances[accountIndex]:C2}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("{0,-20} {1,-12} {2,12} {3,12}",
                "Date/Time", "Type", "Amount", "Balance");
            Console.WriteLine(new string('-', 60));

            // Display the transaction details in formatted columns:
            bool foundTransactions = false;
            for (int i = 0; i < transactionAccountNumbers.Count; i++)
            {
                if (transactionAccountNumbers[i] == accountNumber)
                {
                    Console.WriteLine($"{transactionTimestamps[i],-20} {transactionTypes[i],-12} " +
                        $"{transactionAmounts[i],12:C2} {transactionBalances[i],12:C2}");
                    foundTransactions = true;
                }
            }
            // If no transactions were found for this account

            if (!foundTransactions)
            {
                Console.WriteLine("No transactions found for this account.");
            }
            // Wait for user to acknowledge before returning to menu
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        //Records a new transaction and saves it to persistent storage
        public static void AddTransaction(int accountNumber, string type, double amount, double newBalance)
        {
            transactionAccountNumbers.Add(accountNumber);
            transactionTimestamps.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            transactionTypes.Add(type);
            transactionAmounts.Add(amount);
            transactionBalances.Add(newBalance);
            SaveTransactions();
        }



        //############################################## Submit review #####################################################

        public static void submitReview()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                       Submit Review Page                      \n ");
            Console.WriteLine("**************************************************************\n");

            // Prompt user to enter their review text

            Console.Write("Enter your review: ");
            string review = Console.ReadLine();
            // Validate that review contains actual content

            if (!string.IsNullOrEmpty(review))
            {
                //  save reviews to file to prevent data loss

                submittedReviews.Push(review);
                SaveReviews();
                Console.WriteLine("Your review has been submitted.");
            }
            else
            {
                Console.WriteLine("Review cannot be empty.");
            }

            // Wait for user to acknowledge before returning to menu
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();

        }


        //############################################## Admin Interface #####################################################

        public static void AdminIU()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                         Admin Page                           \n ");
            Console.WriteLine("**************************************************************\n");

            bool Flag = true;
            //  Main admin menu loop

            while (Flag)
            {
                //  display all available admin options

                Console.WriteLine("Select One Option");
                Console.WriteLine("1- View Accounts Request");
                Console.WriteLine("2- View  All Accounts ");
                Console.WriteLine("3- View Reviews");
                Console.WriteLine("4- Process Request");
                Console.WriteLine("5- Search by National ID");
                Console.WriteLine("6- Delete Account");
                Console.WriteLine("7- Transfer Funds");  
                Console.WriteLine("0- Back to Home");

                Console.WriteLine("Enter Number");
                // Input validation for menu selection

                int number;
                while (!int.TryParse(Console.ReadLine(), out number))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
                // Handle menu selection

                switch (number)
                {
                    case 1:
                        ViewAccountRequests();
                        break;
                    case 2:
                        ViewAllAccounts();
                        break;
                    case 3:
                        ViewReviews();
                        break;
                    case 4:
                        ProcessNextAccountRequest();
                        break;
                    case 5:
                        SearchByNationalID();
                        break;
                    case 6:
                        DeleteAccount();
                        break;
                    case 7:
                        TransferFunds();
                        break;

                    case 0:
                        Console.Clear();
                        Flag = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Number Not found: " + number + "\n");
                        break;
                }
            }
        }


        //############################################## View Account Requests #####################################################

        public static void ViewAccountRequests()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                     Account Requests Page                    \n ");
            Console.WriteLine("**************************************************************\n");
            //  Handle empty state if no accounts exist

            if (requestCreateAccountsInfo.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
            }
            else
            {        // Display account table with column headers and formatting

                Console.WriteLine("Pending Account Requests:");
                Console.WriteLine("Name\tNational ID");

                //  Output each account's details in formatted columns

                foreach (string request in requestCreateAccountsInfo)
                {
                    string[] parts = request.Split('|');
                    if (parts.Length >= 2)
                    {
                        Console.WriteLine($"{parts[0]}\t{parts[1]}");
                    }
                }
            }
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        //############################################## View All Accounts #####################################################

        public static void ViewAllAccounts()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                      ALL ACCOUNTS SUMMARY                     ");
            Console.WriteLine("**************************************************************");

            // Debug output
            Console.WriteLine($"\nDebug: Found {accountNumbers.Count} accounts in memory");
            Console.WriteLine($"Debug: Data file exists: {File.Exists(userDataPath)}");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts found in the system.");
                // Additional debug:
                try
                {
                    if (File.Exists(userDataPath))
                    {
                        Console.WriteLine("\nFile content:");
                        Console.WriteLine(File.ReadAllText(userDataPath));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nDebug error reading file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\n{0,-15} {1,-25} {2,10}", "Account Number", "Account Holder", "Balance");
                Console.WriteLine(new string('-', 52));

                for (int i = 0; i < accountNumbers.Count; i++)
                {
                    Console.WriteLine("{0,-15} {1,-25} {2,10:C}",
                        accountNumbers[i],
                        accountNames[i],
                        balances[i]);
                }
                Console.WriteLine("\nTotal Accounts: " + accountNumbers.Count);
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }



        //############################################## View Reviews #####################################################

        public static void ViewReviews()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                         Reviews Page                         \n ");
            Console.WriteLine("**************************************************************\n");

            //  Check if there are any reviews to display

            if (submittedReviews.Count == 0)
            {
                Console.WriteLine("No reviews submitted yet.");
            }
            else
            {
                //  Display all reviews with separators

                Console.WriteLine("Latest Reviews:");
                foreach (var review in submittedReviews)
                {
                    Console.WriteLine(review);
                    Console.WriteLine("----------------------");
                }
            }
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }



        //############################################## Process Next Account Request #####################################################

        public static void ProcessNextAccountRequest()
        {
            if (requestCreateAccountsInfo.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
                return;
            }

            string request = requestCreateAccountsInfo.Dequeue();
            string[] parts = request.Split('|');

            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid request format.");
                return;
            }

            string name = parts[0];
            string nationalID = parts[1];

            int newAccountNumber = userCordNumber + 1;
            userCordNumber = newAccountNumber;

            accountNumbers.Add(newAccountNumber);
            accountNames.Add($"{name}|{nationalID}");
            balances.Add(defaultBalances);

            Console.WriteLine($"Account created for {name} (ID:{nationalID}) with Account Number: {newAccountNumber}");
            SaveUserData();
            SaveAccountRequest();  // Save the updated request queue
        }

        //############################################## Search By nationa ID #####################################################

        public static void SearchByNationalID()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("               SEARCH ACCOUNT BY NATIONAL ID                  ");
            Console.WriteLine("**************************************************************");

            List<string> validIDs = LoadNationalIDs();
            if (validIDs.Count == 0)
            {
                Console.WriteLine("\nNo national IDs registered in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.Write("\nEnter National ID: ");
            string nationalID = Console.ReadLine().Trim();

            if (!validIDs.Contains(nationalID))
            {
                Console.WriteLine("\nThis national ID is not registered in our system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            bool found = false;

            // Search pending requests
            foreach (string request in requestCreateAccountsInfo)
            {
                string[] parts = request.Split('|');
                if (parts.Length >= 2 && parts[1].Trim() == nationalID)
                {
                    Console.WriteLine("\n[PENDING REQUEST FOUND]");
                    Console.WriteLine($"Name: {parts[0]}");
                    Console.WriteLine($"National ID: {parts[1]}");
                    found = true;
                }
            }

            // Search active accounts
            for (int i = 0; i < accountNumbers.Count; i++)
            {
                string[] accountInfo = accountNames[i].Split('|');
                if (accountInfo.Length >= 2 && accountInfo[1].Trim() == nationalID)
                {
                    Console.WriteLine("\n[ACTIVE ACCOUNT FOUND]");
                    Console.WriteLine($"Account Number: {accountNumbers[i]}");
                    Console.WriteLine($"Name: {accountInfo[0]}");
                    Console.WriteLine($"National ID: {accountInfo[1]}");
                    Console.WriteLine($"Balance: {balances[i]:C2}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("\nNo accounts found with this National ID (though it is registered).");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }



        //############################################## delete account #####################################################

        public static void DeleteAccount()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     DELETE ACCOUNT                           ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.Write("\nEnter Account Number to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int index = accountNumbers.IndexOf(accountNumber);
            if (index == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get confirmation
            Console.Write($"\nAre you sure you want to delete account {accountNumber}? (Y/N): ");
            string confirmation = Console.ReadLine().ToUpper();

            if (confirmation == "Y")
            {
                // Remove from all parallel lists
                accountNumbers.RemoveAt(index);
                accountNames.RemoveAt(index);
                balances.RemoveAt(index);

                // Remove all transactions for this account
                for (int i = transactionAccountNumbers.Count - 1; i >= 0; i--)
                {
                    if (transactionAccountNumbers[i] == accountNumber)
                    {
                        transactionAccountNumbers.RemoveAt(i);
                        transactionTimestamps.RemoveAt(i);
                        transactionTypes.RemoveAt(i);
                        transactionAmounts.RemoveAt(i);
                        transactionBalances.RemoveAt(i);
                    }
                }

                SaveUserData();
                SaveTransactions();
                Console.WriteLine($"\nAccount {accountNumber} has been deleted successfully.");
            }
            else
            {
                Console.WriteLine("\nAccount deletion cancelled.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }


        //############################################## transfer funds #####################################################

        public static void TransferFunds()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     TRANSFER FUNDS                           ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count < 2)
            {
                Console.WriteLine("\nThere must be at least two accounts to transfer funds.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get source account
            Console.Write("\nEnter YOUR account number: ");
            if (!int.TryParse(Console.ReadLine(), out int sourceAccount))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int sourceIndex = accountNumbers.IndexOf(sourceAccount);
            if (sourceIndex == -1)
            {
                Console.WriteLine("\nSource account not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get destination account
            Console.Write("\nEnter RECIPIENT account number: ");
            if (!int.TryParse(Console.ReadLine(), out int destAccount))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int destIndex = accountNumbers.IndexOf(destAccount);
            if (destIndex == -1)
            {
                Console.WriteLine("\nDestination account not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            if (sourceAccount == destAccount)
            {
                Console.WriteLine("\nCannot transfer to the same account!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get transfer amount
            Console.Write("\nEnter amount to transfer: ");
            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Check sufficient balance
            if (balances[sourceIndex] < amount)
            {
                Console.WriteLine("\nInsufficient funds for this transfer.");
                Console.WriteLine($"Current balance: {balances[sourceIndex]:C2}");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Perform transfer
            balances[sourceIndex] -= amount;
            balances[destIndex] += amount;

            // Record transactions
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            AddTransaction(sourceAccount, "TRANSFER_OUT", amount, balances[sourceIndex]);
            AddTransaction(destAccount, "TRANSFER_IN", amount, balances[destIndex]);

            // Save changes
            SaveUserData();

            Console.WriteLine("\nTransfer successful!");
            Console.WriteLine($"\nNew balance for account {sourceAccount}: {balances[sourceIndex]:C2}");
            Console.WriteLine($"New balance for account {destAccount}: {balances[destIndex]:C2}");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

      

        // Load Reviews from the file
        static void LoadReviews()
        {
            try
            {
                if (!File.Exists(pathFile)) return;

                submittedReviews.Clear();
                using (StreamReader reader = new StreamReader(pathFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        submittedReviews.Push(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading reviews: {ex.Message}");
            }
        }
        // save user data
        public static void SaveUserData()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(userDataPath))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        writer.WriteLine($"{accountNumbers[i]}|{accountNames[i]}|{balances[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
            }
        }
        // load user data
        public static void LoadUserData()
        {
            try
            {
                if (!File.Exists(userDataPath)) return;

                accountNumbers.Clear();
                accountNames.Clear();
                balances.Clear();

                using (StreamReader reader = new StreamReader(userDataPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            accountNumbers.Add(int.Parse(parts[0]));
                            accountNames.Add(parts[1]);
                            balances.Add(double.Parse(parts[2]));

                            // Update the account number counter
                            if (int.Parse(parts[0]) > userCordNumber)
                            {
                                userCordNumber = int.Parse(parts[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user data: {ex.Message}");
            }
        }



        // Load transactions from file
        public static void LoadTransactions()
        {
            try
            {
                if (!File.Exists(transactionsFile)) return;

                string[] lines = File.ReadAllLines(transactionsFile);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 5)
                    {
                        transactionAccountNumbers.Add(int.Parse(parts[0]));
                        transactionTimestamps.Add(parts[1]);
                        transactionTypes.Add(parts[2]);
                        transactionAmounts.Add(double.Parse(parts[3]));
                        transactionBalances.Add(double.Parse(parts[4]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }



        // Save the request in the file
        public static void SaveAccountRequest()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(accountRequestsPath))  // Changed to accountRequestsPath
                {
                    foreach (var request in requestCreateAccountsInfo)
                    {
                        writer.WriteLine(request);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving account requests: {ex.Message}");
            }
        }
        // Load the request from the file
        public static void LoadAccountRequest()
        {
            try
            {
                if (!File.Exists(accountRequestsPath)) return;  // Changed to accountRequestsPath
                requestCreateAccountsInfo.Clear();
                using (StreamReader reader = new StreamReader(accountRequestsPath))  // Changed to accountRequestsPath
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        requestCreateAccountsInfo.Enqueue(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading account requests: {ex.Message}");
            }
        }





        // load admin info

        public static void LoadAdmi()
        {
            try
            {
                if (!File.Exists(userDataPath)) return;
                requestCreateAccountsInfo.Clear();
                using (StreamReader reader = new StreamReader(pathAdmin))
                {
                    int line;
                    while ((line = int.Parse(reader.ReadLine())) != null)
                    {
                        adm.Enqueue(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading account requests: {ex.Message}");
            }
        }

        // Save transactions to file
        private static void SaveTransactions()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(transactionsFile))
                {
                    for (int i = 0; i < transactionAccountNumbers.Count; i++)
                    {
                        writer.WriteLine($"{transactionAccountNumbers[i]}|{transactionTimestamps[i]}|" +
                            $"{transactionTypes[i]}|{transactionAmounts[i]}|{transactionBalances[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }
        // Save Reviews to the file
        public static void SaveReviews()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(pathFile))
                {
                    foreach (var review in submittedReviews)
                    {
                        writer.WriteLine(review);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving reviews: {ex.Message}");
            }
        }











    }
}
