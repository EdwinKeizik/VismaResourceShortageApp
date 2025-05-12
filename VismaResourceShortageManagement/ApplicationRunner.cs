using System;
using System.Collections.Generic;
using VismaResourceShortageManagement.Services;
using VismaResourceShortageManagement.UI;
using VismaResourceShortageManagement.Models;


namespace VismaResourceShortageManagement
{
    public class ApplicationRunner
    {
        private readonly MenuHandler _menuHandler;
        private readonly UserContext _userContext;
        private readonly ShortageService _shortageService;

        public ApplicationRunner()
        {
            _menuHandler = new MenuHandler();
            _userContext = new UserContext();
            IFileStorageService fileStorageService = new FileStorageService("data/shortages.json");
            _shortageService = new ShortageService(_userContext, fileStorageService);
        }

        public void Run()
        {
            _menuHandler.ShowMessage("Welcome to the resource shortage management system!\n");
            string name = _menuHandler.PromptForUserName();
            _userContext.SetCurrentUser(name);
            _menuHandler.ShowMessage($"Hello, {_userContext.Name}! " + (_userContext.IsAdmin ? "You are logged in as an Administrator." : "You are logged in as a Regular User."));


            int input = _menuHandler.MainMenu();
            while (input != 0) // Main application loop
            {
                switch (input)
                {
                    case 1: // Register Shortage
                        Shortage newShortageData = _menuHandler.PromptForNewShortageDetails(_userContext.Name);
                        ShortageService.RegistrationResult regResult = _shortageService.RegisterShortage(newShortageData);
                        switch (regResult)
                        {
                            case ShortageService.RegistrationResult.Success:
                                _menuHandler.ShowMessage("\nNew shortage registered successfully.");
                                break;
                            case ShortageService.RegistrationResult.Updated:
                                _menuHandler.ShowMessage("\nExisting shortage updated with a higher priority.");
                                break;
                            case ShortageService.RegistrationResult.DuplicateNotOverridden:
                                _menuHandler.ShowMessage("\nWARNING: A shortage with this title and room already exists, and its priority is not lower.");
                                break;
                        }
                        break;
                    case 2: // Show List (Unfiltered, but role-based)
                        List<Shortage> shortagesToDisplay = _shortageService.GetShortagesToDisplay();
                        _menuHandler.DisplayShortagesList(shortagesToDisplay);    
                        break;
                    case 3: // Filter List
                        HandleFilterSubMenu();
                        break;

                    case 4: // Delete Shortage
                        _menuHandler.ShowMessage("\n--- Delete Shortage ---");
                        string deleteTitle = _menuHandler.GetShortageTitle();
                        string deleteRoom = _menuHandler.GetShortageRoom();
                        ShortageService.DeletionResult delResult = _shortageService.DeleteShortage(deleteTitle, deleteRoom);
                        switch (delResult)
                        {
                            case ShortageService.DeletionResult.Success:
                                _menuHandler.ShowMessage("\nShortage successfully deleted.");
                                break;
                            case ShortageService.DeletionResult.NotFound:
                                _menuHandler.ShowMessage("\nShortage with the specified title and room not found.");
                                break;
                            case ShortageService.DeletionResult.NoPermission:
                                _menuHandler.ShowMessage("\nERROR: You do not have permission to delete this shortage.");
                                break;
                        }
                        break;
                    default:
                        _menuHandler.ShowMessage("\nInvalid main menu option. Please try again.");
                        break;
                }
                input = _menuHandler.MainMenu(); // Get next main menu choice
            }
            _menuHandler.ShowMessage("\nExiting application. Goodbye!");
        }

        private void HandleFilterSubMenu()
        {
            _menuHandler.ShowMessage("\n--- Filter Shortages ---");
            int filterInput = _menuHandler.GetFilterOption();
            while (filterInput != 0)
            {
                string? filterTitle = null;
                DateTime? filterStartDate = null; DateTime? filterEndDate = null;
                string? filterCategory = null;
                string? filterRoom = null;

                switch (filterInput)
                {
                    case 1: // Filter by Title
                        filterTitle = _menuHandler.GetShortageTitle();
                        break;
                    case 2: // Filter by Date
                        filterStartDate = _menuHandler.GetShortageStartDate();
                        filterEndDate = _menuHandler.GetShortageEndDate();
                        if (filterEndDate < filterStartDate)
                        {
                            _menuHandler.ShowMessage("\nError: End date cannot be earlier than the start date. Filters not applied.");
                            filterStartDate = null;
                            filterEndDate = null;
                        }
                        break;
                    case 3: // Filter by Category
                        filterCategory = _menuHandler.GetShortageCategory();
                        break;
                    case 4: // Filter by Room
                        filterRoom = _menuHandler.GetShortageRoom();
                        break;
                    default:
                        _menuHandler.ShowMessage("\nInvalid filter option selected.");
                        break;
                }
                _menuHandler.DisplayShortagesList(_shortageService.GetShortagesToDisplay(filterTitle, filterStartDate, filterEndDate, filterCategory, filterRoom));
                filterInput = _menuHandler.GetFilterOption();
            }
        }
    }
}