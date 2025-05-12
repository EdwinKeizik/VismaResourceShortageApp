using System.Collections.Generic;
using System.Linq;
using VismaResourceShortageManagement.Models;
using VismaResourceShortageManagement.Services;

namespace VismaResourceShortageManagement.Tests
{
    public class FakeFileStorageService : IFileStorageService
    {
        public List<Shortage> ShortagesStorage { get; private set; }

        public FakeFileStorageService()
        {
            ShortagesStorage = new List<Shortage>();
        }

        public FakeFileStorageService(List<Shortage>? initialShortages)
        {
            ShortagesStorage = initialShortages != null ? new List<Shortage>(initialShortages) : new List<Shortage>();
        }

        public List<Shortage> LoadShortages()
        {
            return new List<Shortage>(ShortagesStorage);
        }

        public void SaveShortages(List<Shortage> shortagesToSave)
        {
            ShortagesStorage = new List<Shortage>(shortagesToSave);
        }

        public void AddShortageDirectly(Shortage shortage)
        {
            ShortagesStorage.Add(shortage);
        }

        public void ClearStorageDirectly()
        {
            ShortagesStorage.Clear();
        }
    }
}