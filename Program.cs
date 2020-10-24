using System;
using System.Data;
using System.IO;


namespace PhoneBook
{
    class Program
    {

        static void Main(string[] args)
        {
            //-----Start program-------
            //  1. Check for file existing
            //  1.1 If exists - run Show_dataTable                                  ------- ready
            //  1.2 If not exists - Create_new_dataset(noteBookSet, abonentTable);  ------- ready
            //  2. Run Dialog (1. Add, 2. Remove 3. Find, 4. Order by, 5. Exit      ------- ready
            //  2.1 Run Аdd_new_abonent(abonentTable);                              ------- ready
            //  2.2 Run Remove_abonent(abonentTable                                 ------- ready
            //  2.3 Run Find_abonent(abonentTable)                                  ------- ready
            //  2.4 Run Sort_abonents(abonentTable)                                 ------- ready
            //  2.5 Write XML-file, close application                               ------- ready

            Console.SetWindowSize(120, 50);//Not important. Just for comfortable display of columns without line wrapping 
            //-----------------Declaration of variables-------
            string noteBook = "NoteBook";           //Name of root rode
            string abonent = "Abonent";             //Name of parent node for abonent's data
            string file_name = "PhoneBook.xml";     //Name of phonebook-file  
            string cmd="";                          //Variable for action
            //------------End of declaration of variables---------

            //create new DataSet object
            DataSet noteBookSet = new DataSet(noteBook);
            //create new DataTable object
            DataTable abonentTable = new DataTable(abonent);
            //Checking for file existence

            //Check for file-existence
            if (!File.Exists(file_name))//file not exists
            {
                //Request for further actions
                Console.WriteLine("XML-file not found, add new abonents(1) or close application (2)?");
                //repeat until "2"-key pressed
                do
                {
                    //Get key from console
                    cmd = Console.ReadLine();
                    switch (cmd)
                    {
                        case "1":
                            //If pressed 1, then create datatable...
                            Create_new_dataset(noteBookSet, abonentTable);
                            //...and add first abonent
                            Аdd_new_abonent(abonentTable);
                            // For stop Do-While
                            cmd = "2";
                            break;
                        case "2":
                            //If pressed 2, then stop        
                            return;
                        default:
                            //If pressed not 1 and not 2
                            Console.WriteLine("Enter 1 or 2:");
                            break;
                    }
                } while (cmd != "2");
            }
            //read data from XML-file
            else //file exists
            {
                // Read XML-file to Dataset
                noteBookSet.ReadXml(file_name);
                // Assign DataTable
                abonentTable = noteBookSet.Tables[abonent];
            }
            // Clear console. Not important, just comfortable  
            Console.Clear();
            //Copy datatable to console
            Show_DataTable(abonentTable);
            
            //run cicle and show available commands
            do {
                cmd = show_actions_info();
                switch (cmd)
                {
                    case "1":// If pressed 1 - add new abonent
                        Аdd_new_abonent(abonentTable);
                        Console.Clear();
                        Show_DataTable(abonentTable);
                        break;
                    case "2":// If pressed 2 - remove abonent
                        Remove_abonent(abonentTable);
                        Console.Clear();
                        Show_DataTable(abonentTable);
                        break;
                    case "3":// If pressed 3 - find abonent
                        Find_abonent(abonentTable);
                        Console.Clear();
                        Show_DataTable(abonentTable);
                        break;
                    case "4":// If pressed 4 - find abonent
                        abonentTable=Sort_Abonents(abonentTable);
                        Show_DataTable(abonentTable);
                        break;
                    case "5":// If pressed 5 - Exit with saving data to XML-file
                        abonentTable.WriteXml(file_name);
                        Console.WriteLine("All data saved in " + file_name + " file.\nPress any key to close program");
                        Console.ReadKey();
                        break;
                }
            } while (cmd!="5");
                       
          
            //----------End of Program
        }
        /// <summary>
        /// Copy DataTable to console
        /// </summary>
        private static void Show_DataTable(DataTable abonentTable)
        {
            //Copy column's names to console
            foreach (DataColumn column in abonentTable.Columns)
                Console.Write("\t{0}", column.ColumnName);
                Console.WriteLine();
            //Copy all rows to console
            foreach (DataRow row in abonentTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (object cell in row.ItemArray)
                    Console.Write("\t{0}", cell);
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Create new Dataset with DataTable, add Column's names
        /// </summary>
        private static void Create_new_dataset(DataSet noteBookSet, DataTable abonentsTable)
            
            {
                // Set name of dataset
                noteBookSet.DataSetName= "Notebook";
                // Add table into dataset
                noteBookSet.Tables.Add(abonentsTable);
                // Create columns for table Abonents:
                //Column for ID
                DataColumn idColumn = new DataColumn("ID", Type.GetType("System.Int32"));
                //Column for Firstname, type String
                DataColumn firstNameColumn = new DataColumn("FirstName", Type.GetType("System.String"));
                //Column for Surename, type String
                DataColumn sureNameColumn = new DataColumn("SureName", Type.GetType("System.String"));
                //Column for Year of Birth, type Integer
                DataColumn birthYearColumn = new DataColumn("YearOfBirth", Type.GetType("System.Int32"));
                //Column for Phone Number, type String
                DataColumn phoneNumberColumn = new DataColumn("PhoneNumber", Type.GetType("System.String"));
                //Column for Additional Information, type String
                DataColumn additionalInfoColumn = new DataColumn("AdditionalInfo", Type.GetType("System.String"));
                //Column for Group of Abonents, type String (Manager or Employee)
                DataColumn groupColumn = new DataColumn("Group", Type.GetType("System.String"));
                
                //add columns in table
                abonentsTable.Columns.Add(idColumn);
                abonentsTable.Columns.Add(firstNameColumn);
                abonentsTable.Columns.Add(sureNameColumn);
                abonentsTable.Columns.Add(birthYearColumn);
                abonentsTable.Columns.Add(phoneNumberColumn);
                abonentsTable.Columns.Add(additionalInfoColumn);
                abonentsTable.Columns.Add(groupColumn);
                               
            }
        /// <summary>
        /// Shows list of actions, available for PhoneBook
        /// </summary>
        private static string show_actions_info()
        {
            string command="";
            Console.WriteLine("Select next action and enter relevant number:");
            Console.WriteLine("1. Add new abonent, 2. Remove abonent 3. Find abonent, 4. Sort data, 5. Exit");
            command = Console.ReadLine();
            return command;
        }
        /// <summary>
        /// Adds new abonent into DataTable
        /// </summary>
        private static void Аdd_new_abonent(DataTable abonentTable)
            {

            ////--------------------Uncomment this block to skip manual filling table...
            //abonentTable.Rows.Add(new object[] { null, "Anna", "Petrov", 1965, "9012345678", "Goods-in", "Manager" });
            //abonentTable.Rows.Add(new object[] { null, "Ivan", "Ivanov", 1985, "1234567890", "Petrov", "Employee" });
            //abonentTable.Rows.Add(new object[] { null, "Ivan", "Petrov", 1975, "0123456789", "Sales", "Manager" });
            //return;
            ////--------------------------------------------------------------
            
            


            //declaration of variables
            string fname, sname, phn, addinf, grp; 
            int yr, maxId=0;
            bool correctData=false; //for checking entered values
            //add ID
            if (abonentTable.Rows.Count == 0) maxId = 1;
            else maxId = Convert.ToInt32(abonentTable.Select("ID=max(ID)")[0][0]) + 1;
            //get Group of abonents
            Console.WriteLine("Enter group of abonents (Employee or Manager):");
            do
            {
                //Get value from console...
                grp = Console.ReadLine();
                //...if it's correct, set correctData to True and stop do-while. Strings are compared case-insensitively
                if (string.Compare("Employee", grp, true)==0| string.Compare("Manager", grp, true) == 0) correctData = true;
                else
                //...if it is uncorrect, print message and repeat cicle
                {
                    Console.WriteLine("Entered data is uncorrect, try again");
                }
            } while (correctData != true);
            // get first name, any value
            Console.WriteLine("Enter Firstname:");
            fname = Console.ReadLine();
            // get last name, any value
            Console.WriteLine("Enter Surname:");
            sname = Console.ReadLine();
            //get year of birth
             Console.WriteLine("Enter year of birth:");
            correctData = false;
            int tempInt; string tempString;
            //get current date
            DateTime currentDate = DateTime.Now;
            //Checking entered value: it must be numeric and in 0-100 range
            do
            {
                tempString = Console.ReadLine();
                //try to convert string to int...
                if (Int32.TryParse(tempString, out tempInt))
                {
                    //...if it's possible - check for range 
                    if (tempInt < currentDate.Year - 100)
                        Console.WriteLine("Abonent is more than 100 years old, it's very improbable, try again.");
                    else if (tempInt >= currentDate.Year)
                        Console.WriteLine("Abonent has not yet been born, this is not possible, try again.");
                    // Entered data is correct, convert string ro int and stop do-while
                    else correctData = true;
                }
                else Console.WriteLine("Entered a non numeric value, try again.");
            } while (correctData != true);
            yr = int.Parse(tempString);
            correctData = false;            
             Console.WriteLine("Enter phone number:");
            do 
            //Checking entered value: it must be numeric
            {
                phn = Console.ReadLine();
                if (Int32.TryParse(phn, out tempInt)) correctData = true;
                else Console.WriteLine("Entered a non numeric value, try again.");
            } while (correctData != true);
            //selection of input information depending on the abonent group
            if (string.Compare("Employee", grp, true) == 0) Console.WriteLine("Enter manager's surname for this abonent:");
             else if (string.Compare("Manager", grp, true) == 0) Console.WriteLine("Enter Department for this abonent:");
             addinf = Console.ReadLine();
             //add row in table
            abonentTable.Rows.Add(new object[] { maxId, fname, sname, yr, phn, addinf, grp });
            }
        /// <summary>
        /// Removes abonent from DataTable
        /// </summary>
        private static void Remove_abonent(DataTable abonentTable)
            {
            //declaration of variables
            string id = ""; bool tempBool = false;
            Console.WriteLine("Enter abonent's ID:");
            // Get abonent's ID
            id = Console.ReadLine();
            //Search ID in column called "ID" 
            for (int j = 0; j < abonentTable.Rows.Count; ++j)
                {
                if (abonentTable.Rows[j]["ID"].ToString() == id)
                    {
                    tempBool = true;
                    //delete row with matched ID
                    abonentTable.Rows[j].Delete();
                    Console.WriteLine("Row № "+ j + "was deleted");
                    }
                }
            // Checking for non-available ID
            if (tempBool==false) Console.WriteLine("Abonent with ID " + id + "not exists in PhoneBook.");
        }
        /// <summary>
        /// Searching for abonent in DataTable
        /// </summary>
        private static void Find_abonent(DataTable abonentTable)
        {
            //declaration of variables
            string command; string findBy = "";
            Console.WriteLine("Enter relevant number:");
            Console.WriteLine("1. Search by FirstName, 2. Search by SureName, 3. Search by PhoneNumber, 4. Exit");
            //Get sesrch conditions
            do
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "1":   //find by FirstName
                        findBy = "FirstName";
                        command = "4";
                        break;
                    case "2":   //find by SurName
                        findBy = "SureName";
                        command = "4";
                        break;
                    case "3":   //find by PhoneNumber
                        findBy = "PhoneNumber";
                        command = "4";
                        break;
                    case "4":   //Exit
                        return;
                    default:
                        Console.WriteLine("Enter number between 1 or 4:");
                        break;
                }
            } while (command != "4");
            Console.WriteLine("Enter search criteria:");
            string searchString = Console.ReadLine();
            //select rows with suitable conditions
            var selectTable = abonentTable.Select(findBy + "=" + "'" + searchString + "'");
            // copy selected rows to console
            foreach (var b in selectTable)
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", b["ID"], b["FirstName"], b["SureName"], b["YearOfBirth"],
                    b["PhoneNumber"], b["AdditionalInfo"], b["Group"]);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        /// <summary>
        /// Sorts abonents in DataTable
        /// </summary>
        private static DataTable Sort_Abonents(DataTable abonentTable) 
        {
            //declaration of variables
            string sortOrder = " DESC"; //sort order, Descending by default
            string command = "";
            string sortByColumn = "";
            Console.WriteLine("Enter relevant number:");
            Console.WriteLine("1. Sort by SureName, 2. Sort by YearOfBirth, 3. Exit");
            //Get column for sort
            do
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "1":   //sort by SureName
                        sortByColumn = "SureName";
                        command = "3";
                        break;
                    case "2":   //sort by SurName
                        sortByColumn = "YearOfBirth";
                        command = "3";
                        break;
                    case "3":   //Exit
                        return abonentTable;
                    default:
                        Console.WriteLine("Enter number between 1 and 3:");
                        break;
                }
            } while (command != "3");
            //get sort order
            Console.WriteLine("1. Sort in DESCending order (by default), 2. Sort in ASCending order");
            command = Console.ReadLine();
            if (command == "2") sortOrder = " ASC";
            // Create new dataView
            DataView newDataView = abonentTable.DefaultView;
            //Sort Dataview
            newDataView.Sort = sortByColumn + sortOrder;
            //Replace current table with sorted table
            DataTable sortedTable = newDataView.ToTable();
            // Return sorted table
            return sortedTable;
        }

    }
}


