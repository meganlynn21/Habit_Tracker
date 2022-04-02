using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Habit_Tracker
{

    internal class Program
    {
        public static SQLiteConnection myConnection = new SQLiteConnection("Data Source = database.sqlite3");
        static void Main(string[] args)
        {
            // Checking if the database is there if not there then create it
            if (!File.Exists("database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
                // opens connection the database so we can interact with it
                myConnection.Open();
                //This creates 3 columns in the database id, quantity, and date
                string table = "CREATE TABLE habit (id INTEGER PRIMARY KEY AUTOINCREMENT," +
                        "quantity TEXT, date TEXT);";
                SQLiteCommand command = new SQLiteCommand(table, myConnection);
                // Non query means not returning anything
                command.ExecuteNonQuery();
                // You always need to close the connection
                myConnection.Close();
            }


            while (true)
            {
                MainMenu();
            }

        }

        // MAIN MENU

        static void MainMenu()

        {
            Console.Clear();

            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine("\nPlease select an option");
            Console.WriteLine("\nType 0 to close the application");
            Console.WriteLine("Type 1 to view all records");
            Console.WriteLine("Type 2 to insert a record");
            Console.WriteLine("Type 3 to delete a record");
            Console.WriteLine("Type 4 to update a record");

            string menuSelector = Convert.ToString(Console.ReadKey(true).KeyChar);

            switch (menuSelector)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    Console.Clear();
                    ViewRecords(menuSelector);
                    break;
                case "2":
                    Console.Clear();
                    InsertRecord();
                    break;
                case "3":
                    Console.Clear();
                    ViewRecords(menuSelector);
                    DeleteRecord();
                    break;
                case "4":
                    Console.Clear();
                    ViewRecords(menuSelector);
                    UpdateRecord();
                    break;
                default:
                    Console.WriteLine("Invalid Entry. Press any key to continue.");
                    Console.ReadKey();
                        break;
            }

        }
        //CREATE HABIT

        static void InsertRecord()
        {
            Console.WriteLine("\nInserting a record. Type MENU to return");

            Console.Write("\nPlease enter a quantity: ");
            string quantityValue = Console.ReadLine();
            if (quantityValue == "MENU") { return; }

            Console.Write("\nPlease enter a date: ");
            string dateValue = Console.ReadLine();
            if (dateValue == "MENU") { return; }

            myConnection.Open();
            string query = "INSERT INTO habit (quantity, date) VALUES (@quantity, @date)";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            command.Parameters.AddWithValue("@quantity", quantityValue);
            command.Parameters.AddWithValue("@date", dateValue);
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        //READ HABIT
        static void ViewRecords(string selector)
        {
            Console.WriteLine("\nViewing a records.. Type MENU to return\n");

            myConnection.Open();
            string query = "SELECT rowid, * FROM habit";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            // execute reader returns each column 
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("  ID  | Quantity | Date");
                Console.WriteLine("-----------------------------");   
                while (reader.Read())
                {
                    Console.WriteLine("{0,5} | {1,8} | {2})", reader["id"], reader["quantity"], reader["date"]);
                }
            }
            myConnection.Close();

            if (selector == "1")
            {
                Console.WriteLine("\nPress any key to return... ");
                Console.ReadKey();
            }
        }
         

        //UPDATE HABIT
        static void UpdateRecord()
        {
            Console.WriteLine("\nUpdating a record..Type MENU to return");

            Console.Write("\nPlease enter an ID: ");
            string idValue = Console.ReadLine();
            if (idValue == "MENU") { return; }

            Console.Write("\nPlease enter new quantity: ");
            string quantityValue = Console.ReadLine();
            if (quantityValue == "MENU") { return; }

            Console.Write("\nPlease enter new date: ");
            string dateValue = Console.ReadLine();
            if (dateValue == "MENU") { return; }


            myConnection.Open();
            string query = "UPDATE habit SET quantity=(@quantity), date=(@date) WHERE id=(@id)";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            command.Parameters.AddWithValue("@id", idValue);
            command.Parameters.AddWithValue("@quantity", quantityValue);
            command.Parameters.AddWithValue("@date", dateValue);
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        //DELETE HABIT
        static void DeleteRecord()
        {
            Console.WriteLine("\nDeleting a record. Type MENU to return");

            Console.Write("\nPlease enter an ID: ");
            string idValue = Console.ReadLine();
            if (idValue == "MENU") { return; }

            myConnection.Open();
            string query = "DELETE FROM habit WHERE id=(@id)";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            command.Parameters.AddWithValue("@id", idValue);
            command.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
