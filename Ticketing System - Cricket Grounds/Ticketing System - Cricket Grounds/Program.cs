using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing_System___Cricket_Grounds
{
    class Program
    {
        //Comments
        static void Comments()
        {
            /*NEW PROJECT
            * game, time, venue, ticket price, capacity (admin enters data)
            * Loops (customers coming to watch cricket game)
            * functions (everything is in seperate functions)(no code in main function)   
            * Admin delete match when its half time
            * username and password (in array)(check password and username from database) login screen
           */

           /*^^^Writing to and Reading from file
             * Delimeter (seperater)
             * Append (dont override information that's in the file) 
           */
        }



        //Utility Functions:
        static void WriteToFile(string path, string data)
        {
            using (StreamWriter streamwriter = File.AppendText(path))
            {
                streamwriter.WriteLine(data);
            }
        }
        static void Header(string header)
        {       
            Console.Clear();   
            
            int consoleWidth = Console.WindowWidth;                             // Get the width of the console

            int spacesOnEachSide = (consoleWidth - header.Length) / 2;          // Calculate the number of spaces needed on each side
                                                                                
            string leftPadding = new string(' ', spacesOnEachSide);             // Create a string with the calculated spaces for left padding
                                                                    
            Console.WriteLine("".PadLeft(consoleWidth, '*'));                   // Create the top border   
            Console.WriteLine($"{leftPadding}{header.Replace("~", " ")}");      // Print the title with centered alignment        
            Console.WriteLine("".PadLeft(consoleWidth, '*') + "\n");            // Create the bottom border
        }
        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit] = newText; // Updated to use the correct index
            File.WriteAllLines(fileName, arrLine);
        }



        //Customer Authentication and Pages:
        static void VerifiedCredentials()
        {
            string name = "";
            string path = @"..//users.txt";

            Header("LOGIN PAGE");
            Console.WriteLine("PLEASE ENTER YOUR E-MAIL: ");
            string email = Console.ReadLine();

            Console.WriteLine("\nPLEASE ENTER YOUR PASSWORD: ");
            string password = Console.ReadLine();
            Console.Clear();

            string _loginDetails = email + "~" + password;
            var lines = File.ReadAllLines(path);
            bool found = false;

            for (int i = 0; i < lines.Length; i += 1)
            {
                string line = lines[i];

                if (_loginDetails == line)
                {
                    found = true;
                    string[] userinfoArr = line.Split('~');
                    name = userinfoArr[0];
                    break;
                }
            }

            if (found == true)
            {
                Console.WriteLine("YOU HAVE BEEN LOGGED IN");
                Console.ReadLine();
                customerHomepage(name);
            }
            else
            {
                Console.WriteLine("PLEASE TRY AGAIN");
                Console.ReadLine();
                VerifiedCredentials();
            }
        }
        static void Credentials()
        {
            Header("CUSTOMER LOGIN");

            Console.WriteLine("PRESS 1 TO REGISTER");
            Console.WriteLine("PRESS 2 TO LOG-IN");
            Console.WriteLine("\nPRESS ANY KEY TO GO BACK");

            string account = Console.ReadLine();

            if (account == "1")
            {
                Header("REGISTRATION PAGE");
                Console.WriteLine("PLEASE ENTER YOUR E-MAIL: ");                  //chris123
                string email = Console.ReadLine();

                Console.WriteLine("\nPLEASE ENTER YOUR PASSWORD: ");
                string password = Console.ReadLine();
                Console.Clear();

                string loginDetails = email + "~" + password;
                string path = @"..//users.txt";
                WriteToFile(path, loginDetails);

                Console.WriteLine("YOUR DATA HAS BEEN SAVED");
                Console.ReadLine();
                Console.Clear();
                VerifiedCredentials();
            }
            else if (account == "2")
            {
                VerifiedCredentials();
            }
            else
            {
                Operator();
            }
        }
        static void LoginPage()
        {
            Credentials();
            Console.Clear();
        }
        static void customerHomepage(string names)
        {          
            Header($"CUSTOMER HOMEPAGE - {names}");
            Console.WriteLine("PRESS 1 TO VIEW UPCOMING MATCHES");
            Console.WriteLine("PRESS 2 TO VIEW TICKET HISTORY");
            Console.WriteLine("\nPRESS ANY KEY TO LOG-OUT");
            string input = Console.ReadLine();                    

            while (input=="1")
            {
                ticketPurchaseScreen();                
            }

            if (input == "2")
            {
                DisplayPaymentHistory();
            }
            else
            {
                LoginPage();
            }

            Console.ReadLine();
        } 
        static void ticketPurchaseScreen()
        {         
            string checkTicketsLeft_Path = @"..//stadiumcap.txt"; //new
            var ticketsLeft = File.ReadAllLines(checkTicketsLeft_Path); //new

            string path = @"..//matches.txt";
            var lines = File.ReadAllLines(path);
            string[] lineArr = File.ReadAllLines(path);

            int newStadiumCap = 0;

            for (int i = 0; i < 1; i++)
            {
                UpdateTicketsAmount(0); //new
                UpdateTicketSalesAmount(0); //new
            }

            for (int i = 0; i < lines.Length; i++)
            {
                viewMatchesCustomers();
                int matchNum = Convert.ToInt32(Console.ReadLine());

                if (matchNum == 0)
                {
                    customerHomepage("YOU");
                }

                string lineMatch = lines[i];
                string[] matchDataArr = lineMatch.Split('~');
              
                Console.Clear();
                Header(matchDataArr[0] + ": " + matchDataArr[1]);

                string pathPrice = @"..//prices.txt";
                string[] pricesArr = File.ReadAllLines(pathPrice);

                Console.WriteLine("HOW MANY TICKETS DO YOU WANT TO BUY?");
                int ticketNum = Convert.ToInt32(Console.ReadLine());

                string _ticketsLeft = ticketsLeft[matchNum - 1]; //new
                int ticketsLeft_ = Convert.ToInt32(_ticketsLeft); //new

                string pathStadiumCap = @"..//stadiumcap.txt";
                var lines_ = File.ReadAllLines(pathStadiumCap);

                if (ticketsLeft_ >= ticketNum) //new
                {
                    int _lines = Convert.ToInt32(lines_[matchNum - 1]);
                    newStadiumCap = _lines - ticketNum;

                    int lineNumber = matchNum - 1;
                    string newText = Convert.ToString(newStadiumCap);
                    lineChanger(newText, pathStadiumCap, lineNumber);

                    changeMatchInfo(matchNum, newStadiumCap);

                    int ticketsPrice = ticketNum * Convert.ToInt32(pricesArr[matchNum - 1]);
                    Console.WriteLine($"\nYOU HAVE PAID R{ticketsPrice}");

                    UpdateTicketsAmount(ticketNum); //new
                    UpdateTicketSalesAmount(ticketsPrice); //new

                    WriteToFile(@"..//previous_payments.txt", $"YOU PAID R{ticketsPrice} FOR {ticketNum} TICKETS TO: {lineArr[matchNum - 1]}");

                    Console.ReadLine();
                    Console.Clear();
                }
                else //new
                {
                    Console.Clear();
                    Header("NOT ENOUGH TICKETS AVAILABLE, PLEASE TRY AGAIN");
                    Console.ReadLine();
                    viewMatchesCustomers();
                }       
            }
        }
        static void UpdateTicketsAmount(int amount)
        {
            string path = @"..//tickets_amount.txt";

            // Read the existing amount from the file
            int currentAmount = 0;

            if (File.Exists(path))
            {
                string existingAmountString = File.ReadAllText(path);

                currentAmount = Convert.ToInt32(existingAmountString);
                // Update the amount with the new value

                currentAmount = currentAmount + amount;
            }

            // Write the updated amount back to the file
            File.WriteAllText(path, currentAmount.ToString());
        } //new
        static void UpdateTicketSalesAmount(int amount)
        {
            string path = @"..//ticket_sales_amount.txt";

            // Read the existing amount from the file
            int currentAmount = 0;

            if (File.Exists(path))
            {
                string existingAmountString = File.ReadAllText(path);

                currentAmount = Convert.ToInt32(existingAmountString);
                // Update the amount with the new value

                currentAmount = currentAmount + amount;
            }

            // Write the updated amount back to the file
            File.WriteAllText(path, currentAmount.ToString());
        } //new
        static void DisplayPaymentHistory()
        {
            string path = @"..//previous_payments.txt";
            var lines = File.ReadAllLines(path);

            Header("PAYMENT HISTORY");

            if (lines.Length == 0)
            {
                Console.WriteLine("No payment history available.");
            }
            else
            {
                foreach (var line in lines)
                {
                    string[] paymentInfo = line.Split(':');
                    string sentence = paymentInfo[0];

                    string[] details = paymentInfo[1].Split('~');
                    string teams = details[0].Trim();
                    string time = details[1].Trim();

                    Console.WriteLine($"{sentence} : {teams} , {time}\n");
                }
            }
       
            Console.WriteLine("\n\n\n\n\n\n\n\n\n\nPRESS ANY KEY TO GO BACK");
            Console.ReadLine();
            customerHomepage("YOU");
        }



        //Match Viewing and Purchasing:
        static void viewMatchesCustomers()
        {      
            string path = @"..//matches.txt";
            var lines = File.ReadAllLines(path);

            Header("VIEW MATCHES");
            
            Console.WriteLine(" {0,-5} {1,-20} {2,-5} {3,-20} {4,-5} {5,-20} {6,-5} {7,-20} ", "  ", "TEAMS", "   |   ", "DATE", "   |   ", "STADIUM CAPACITY", "   |   ", "TICKET PRICE\n");

            for (int i = 0; i < lines.Length; i += 1)
            {
                string line = lines[i];
                string[] matchDataArr = line.Split('~');

                string teams = matchDataArr[0];
                string date = matchDataArr[1];
                string stadiumCap = matchDataArr[2];
                string ticketPrice = matchDataArr[3];

                Console.WriteLine(" {0,-5} {1,-20} {2,-5} {3,-20} {4,-5} {5,-20} {6,-5} {7,-20} ", $"{i + 1}", teams, "   |   ", date, "   |   ", stadiumCap, "   |   ", "R" + ticketPrice + "\n"); //new                                  
            }

            Console.WriteLine("\n\n\n\n\n\n\n\n\n\nWHICH MATCH DO YOU WANT TO WATCH?");         
            Console.WriteLine("PRESS 0 TO GO BACK");           
        }
        static void viewMatchesAdmin()
        {       
            string path = @"..//matches.txt";
            var lines = File.ReadAllLines(path);

            Header("VIEW MATCHES");      

            Console.WriteLine(" {0,-5} {1,-20} {2,-5} {3,-20} {4,-5} {5,-20} {6,-5} {7,-20} ", "  ", "TEAMS", "   |   " , "DATE", "   |   ", "STADIUM CAPACITY", "   |   ", "TICKET PRICE\n");

            for (int i = 0; i < lines.Length; i += 1)
            {               
                string line = lines[i];
                string[] matchDataArr = line.Split('~');

                string teams = matchDataArr[0];
                string date = matchDataArr[1];
                string stadiumCap = matchDataArr[2];
                string ticketPrice = matchDataArr[3];

                Console.WriteLine(" {0,-5} {1,-20} {2,-5} {3,-20} {4,-5} {5,-20} {6,-5} {7,-20} ", $"{i + 1}", teams, "   |   " , date, "   |   " , stadiumCap, "   |   " , "R"+ticketPrice + "\n"); //new                                       
            }

            Console.WriteLine("\n\n\n\n\n\n\n\n\n\nPRESS ANY KEY TO GO BACK");          
        }
        static void changeMatchInfo(int matchNum, int newStadiumCap)
        {
            string path = @"..//matches.txt";
            var lines = File.ReadAllLines(path);

            // Check if matchNum is within valid range
            if (matchNum >= 1 && matchNum <= lines.Length)
            {
                int lineNumber = matchNum - 1;
                string line = lines[lineNumber];

                // Split the existing line to get individual parts
                string[] matchDataArr = line.Split('~');

                // Update the stadium capacity part
                matchDataArr[2] = newStadiumCap.ToString();

                // Join the modified parts back into a line
                string updatedLine = string.Join("~", matchDataArr);

                // Update the line in the array
                lines[lineNumber] = updatedLine;

                // Write the modified lines back to the file
                File.WriteAllLines(path, lines);
            }
            else
            {
                // Handle invalid matchNum, e.g., display an error message
                Console.WriteLine("Invalid match number");
            }
        }



        //Admin Authentication and Functions:
        static void AdminCredentials()
        {
            Console.WriteLine("ENTER YOUR USERNAME");                  //adminICC admin123
            string username = Console.ReadLine();

            Console.WriteLine("\nENTER YOUR PASSWORD");
            string password = Console.ReadLine();
            Console.Clear();

            string loginDetails = username + password;
            string[] loginDetailsArr = { "adminICCadmin123" };

            if (loginDetails == loginDetailsArr[0])
            {
                Console.WriteLine("YOU HAVE BEEN LOGGED IN");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("TRY AGAIN");
                Console.ReadLine();
                Console.Clear();
                Header("ADMIN LOGIN PAGE");
                AdminCredentials();
            }
        }      
        static void Login_Admin()
        {
            Header("ADMIN LOGIN PAGE");
            AdminCredentials();
            Console.Clear();

            adminHomepage();
        }   
        static void adminHomepage()
        {
            Header("ADMIN HOMEPAGE");
            Console.WriteLine("PRESS 1 TO ADD A NEW GAME");
            Console.WriteLine("PRESS 2 TO VIEW MATCHES ADDED");
            Console.WriteLine("PRESS 3 TO VIEW TICKET SALES\n\n\n\n\n\n\n\n\n\n");
            Console.WriteLine("PRESS ANY KEY TO GO BACK TO MAIN MENU");
            string input = Console.ReadLine();

            if(input == "1")
            {
                addMatches();
            }
            else if (input == "2")
            {
                viewMatchesAdmin();
                Console.ReadLine();

                adminHomepage();
            }
            else if (input == "3")
            {
                Console.Clear();
                Header("TICKET SALES");

                ViewTicketSalesAdmin();

                Console.WriteLine("\n\n\n\n\n\n\n\n\n\nPRESS ANY KEY TO GO BACK");
                string backToMenu = Console.ReadLine();

                if (backToMenu == "-9999")
                {
                    adminHomepage();
                }
                else
                {
                    adminHomepage();
                }

            }
            else 
            {
                Operator();
            }

            Console.ReadLine();
        }
        static void addMatches()
        {
            Header("UPCOMING MATCHES");
            addGames();

            Console.Clear();
            Console.WriteLine("DO YOU WANT TO ADD ANOTHER MATCH? (y/n)");
            string addmatch = Console.ReadLine().ToUpper();

            while (addmatch == "Y")
            {
                Header("UPCOMING MATCHES");
                addGames();

                Console.Clear();

                Console.WriteLine("DO YOU WANT TO ADD ANOTHER MATCH? (y/n)");
                addmatch = Console.ReadLine().ToUpper();
            }
            if (addmatch != "Y")
            {
                adminHomepage();
            }
            
        }
        static void addGames()
        {
            string pathStadiumCap = @"..//stadiumcap.txt";
            string path = @"..//matches.txt";
            string pathPrice = @"..//prices.txt";

            Console.WriteLine("ENTER NAME OF TEAMS");
            string teamNames = Console.ReadLine();

            Console.WriteLine("ENTER DATE OF MATCH");
            string matchTime = Console.ReadLine();

            Console.WriteLine("STADIUM CAPACITY");
            string stadiumCapacity = Console.ReadLine();

            Console.WriteLine("ENTER TICKET PRICE");
            string ticketPrice = Console.ReadLine();

            string match = $"{teamNames}~{matchTime}~{stadiumCapacity}~{ticketPrice}";


            WriteToFile(path, match);
            WriteToFile(pathPrice, ticketPrice);
            WriteToFile(pathStadiumCap, stadiumCapacity);

            Console.Clear();
            Console.WriteLine("DATA WAS ACCEPTED");
            Console.ReadLine();
        }
        static void ViewTicketSalesAdmin()
        {
            string path = @"..//tickets_amount.txt";
            var line = File.ReadAllLines(path);

            string _path = @"..//ticket_sales_amount.txt";
            var _line = File.ReadAllLines(_path);

            Console.WriteLine("{0,-25}{1,-10}{2,-25}", "AMOUNT OF TICKETS SOLD", "   |   ", "TOTAL REVENUE EARNED\n");
            Console.WriteLine("{0,-25}{1,-10}{2,-25}", line[0] , "   |   ", "R" + _line[0] + "\n");
        }


        //System Operator and Initialization:
        static void Operator()
        {
            
            Header("WELCOME TO CRICKET TICKETING SYSTEM");

            Console.WriteLine("PRESS 0 IF YOU'RE A CUSTOMER");
            Console.WriteLine("PRESS 1 IF YOU'RE AN ADMIN");
            Console.WriteLine("PRESS 2 TO EXIT SYSTEM");

            string role = Console.ReadLine();
            if(role == "0")
            {
                LoginPage();
            }
            else if(role == "1")
            {
                Login_Admin();
            }
            else if(role == "2")
            {
                
            }
            else
            {
                Console.Clear();
                Header("Invalid Input, Please Check Your Input");               
                Console.WriteLine("Try Again [ENTER]");
                Console.ReadLine();

                Operator();
            }
            


        }      
        static void Main(string[] args)
        {
            Operator();
            Console.ReadLine();
        }
    }
}
