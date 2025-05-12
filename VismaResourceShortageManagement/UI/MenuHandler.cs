using VismaResourceShortageManagement.Models;

namespace VismaResourceShortageManagement.UI
{

    public class MenuHandler
    { 

        public MenuHandler()
        {
        
        }


        public string PromptForUserName()
        {

            string name = InputValidator.GetValidString(
                "Enter your name: ",
                "Name cannot be empty. Please enter a valid name: "
                );
            return name;
        }

        public int MainMenu()
        {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Register Shortage");
                Console.WriteLine("2. Show List (Unfiltered)");
                Console.WriteLine("3. Filter List");
                Console.WriteLine("4. Delete Shortage");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an action: ");  
                int input = InputValidator.GetValidMenuOption(
                "Invalid menu option. Please try again: ",
                0, 4
                );
                return input;     
        }


        public Shortage PromptForNewShortageDetails(string creatorName)
        {
            Console.WriteLine("\n--- Register New Shortage ---");
            string title = InputValidator.GetValidString(
                "Enter the title of the shortage: ",
                "Title cannot be empty. Please enter a valid title: "
                );

            DisplayStringOptions("\nChoose the room:", FixedValues.RoomOptions);
            string room = InputValidator.GetOptionByNumberFromArray(
                "Enter the room number: ",
                "Invalid room number. Try again: ",
                FixedValues.RoomOptions
                );
            
            DisplayStringOptions("\nChoose the category:", FixedValues.CategoryOptions);
            string category = InputValidator.GetOptionByNumberFromArray(
                "Enter the category number: ",
                "Invalid category number. Try again: ",
                FixedValues.CategoryOptions
                );

            int priority = InputValidator.GetValidPriority(
                "\nEnter the priority (1-10): ",
                "Invalid priority. Please enter a number between 1 and 10",
                1, 10
                );

            return new Shortage
            {
                Title = title,
                Name = creatorName,
                Room = room,
                Category = category,
                Priority = priority,
                CreatedOn = DateTime.Now
            };

        }

        public void DisplayShortagesList(List<Shortage> shortages)
        {
            if (shortages == null || !shortages.Any())
            {
                Console.WriteLine("\nNo shortages to display.");
                return;
            }

            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine("             LIST OF SHORTAGES                  ");
            Console.WriteLine("--------------------------------------------------");

            foreach (var shortage in shortages)
            {
                Console.WriteLine($"Title:       {shortage.Title}");
                Console.WriteLine($"Reported by: {shortage.Name}");
                Console.WriteLine($"Room:        {shortage.Room}");
                Console.WriteLine($"Category:    {shortage.Category}");
                Console.WriteLine($"Priority:    {shortage.Priority}");
                Console.WriteLine($"Created On:  {shortage.CreatedOn:yyyy-MM-dd HH:mm}");
                Console.WriteLine("--------------------------------------------------");
            }
        }

        public int GetFilterOption()
        {
            Console.WriteLine("\nYou can filter the list by:");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Date");
            Console.WriteLine("3. Category");
            Console.WriteLine("4. Room");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Choose an option: ");

            int filterOption = InputValidator.GetValidMenuOption(
                "Invalid option. Please try again: ",
                0, 4
            );

            return filterOption;
        }
        public string GetShortageTitle()
        {
            return InputValidator.GetValidString(
                "\nEnter the title to filter by: ",
                "Title cannot be empty. Please enter a valid title: "
            );
        }
        public DateTime GetShortageStartDate()
        {
            DateTime startDate = InputValidator.GetValidDate(
                "\nEnter the start date (yyyy-mm-dd): ",
                "Invalid date format. Please try again: "
            );
            
            return startDate;
        }
        public DateTime GetShortageEndDate()
        {
            DateTime endDate = InputValidator.GetValidDate(
                "\nEnter the end date (yyyy-mm-dd): ",
                "Invalid date format. Please try again: "
            );
            
            return endDate;
        }

        public string GetShortageCategory()
        {
            DisplayStringOptions("\nChoose the category:", FixedValues.CategoryOptions);
            string category = InputValidator.GetOptionByNumberFromArray(
                "\nEnter the category number: ",
                "Invalid category number. Try again: ",
                FixedValues.CategoryOptions
            );
            return category;
        }

        public string GetShortageRoom()
        {
            DisplayStringOptions("\nChoose the room:", FixedValues.RoomOptions);
            string room = InputValidator.GetOptionByNumberFromArray(
                "\nEnter the room number: ",
                "Invalid room number. Try again: ",
                FixedValues.RoomOptions
            );
            return room;
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        private void DisplayStringOptions(string promptHeader, string[] options)
        {
            Console.WriteLine(promptHeader);
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
        }
    }
}