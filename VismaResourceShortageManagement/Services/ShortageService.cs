using System;
using System.Collections.Generic;
using System.Linq;
using VismaResourceShortageManagement.Models;

namespace VismaResourceShortageManagement.Services
{
    public class ShortageService
    {
        private readonly UserContext _userContext;
        private readonly IFileStorageService _fileStorageService;
        private List<Shortage> _shortages;

        public ShortageService(UserContext userContext, IFileStorageService fileStorageService)
        {
            _userContext = userContext;
            _fileStorageService = fileStorageService;
            _shortages = _fileStorageService.LoadShortages();
        }

        public enum RegistrationResult
        {
            Success,
            Updated,
            DuplicateNotOverridden,
            Error
        }

        public RegistrationResult RegisterShortage(Shortage newShortage)
        {
            if (newShortage == null)
            {
                throw new ArgumentNullException(nameof(newShortage));
            }

            var existingShortage = _shortages.FirstOrDefault(s =>
                s.Title.Equals(newShortage.Title, StringComparison.OrdinalIgnoreCase) &&
                s.Room.Equals(newShortage.Room, StringComparison.OrdinalIgnoreCase));

            if (existingShortage != null)
            {
                if (newShortage.Priority > existingShortage.Priority)
                {
                    existingShortage.Priority = newShortage.Priority;
                    existingShortage.Name = newShortage.Name;
                    existingShortage.CreatedOn = newShortage.CreatedOn;

                    _fileStorageService.SaveShortages(_shortages);
                    return RegistrationResult.Updated;
                }
                else
                {
                    return RegistrationResult.DuplicateNotOverridden;
                }
            }
            else
            {
                _shortages.Add(newShortage);
                _fileStorageService.SaveShortages(_shortages);
                return RegistrationResult.Success;
            }
        }

        public enum DeletionResult
        {
            Success,
            NotFound,
            NoPermission,
            Error
        }

        public DeletionResult DeleteShortage(string title, string room)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(room))
            {
                return DeletionResult.NotFound;
            }

            var shortageToDelete = _shortages.FirstOrDefault(s =>
                s.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                s.Room.Equals(room, StringComparison.OrdinalIgnoreCase));

            if (shortageToDelete == null)
            {
                return DeletionResult.NotFound;
            }

            if (_userContext.IsAdmin || shortageToDelete.Name.Equals(_userContext.Name, StringComparison.OrdinalIgnoreCase))
            {
                bool removed = _shortages.Remove(shortageToDelete);
                if (removed)
                {
                    _fileStorageService.SaveShortages(_shortages);
                    return DeletionResult.Success;
                }
                return DeletionResult.Error;
            }
            else
            {
                return DeletionResult.NoPermission;
            }
        }

        public List<Shortage> GetShortagesToDisplay(
            string? filterTitle = null,
            DateTime? filterStartDate = null, DateTime? filterEndDate = null,
            string? filterCategory = null,
            string? filterRoom = null)
        {
            IEnumerable<Shortage> filteredList = _shortages;

            if (!_userContext.IsAdmin)
            {
                filteredList = filteredList.Where(s => s.Name.Equals(_userContext.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filterTitle))
            {
                filteredList = filteredList.Where(s => s.Title.Contains(filterTitle, StringComparison.OrdinalIgnoreCase));
            }
            if (filterStartDate.HasValue)
            {
                filteredList = filteredList.Where(s => s.CreatedOn.Date >= filterStartDate.Value.Date);
            }
            if (filterEndDate.HasValue)
            {
                filteredList = filteredList.Where(s => s.CreatedOn.Date <= filterEndDate.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(filterCategory))
            {
                filteredList = filteredList.Where(s => s.Category.Equals(filterCategory, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(filterRoom))
            {
                filteredList = filteredList.Where(s => s.Room.Equals(filterRoom, StringComparison.OrdinalIgnoreCase));
            }

            return filteredList.OrderByDescending(s => s.Priority).ToList();
        }
    }
}