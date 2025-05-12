using System.Collections.Generic;
using VismaResourceShortageManagement.Models;

namespace VismaResourceShortageManagement.Services
{
    public interface IFileStorageService
    {
        List<Shortage> LoadShortages();
        void SaveShortages(List<Shortage> shortages);
    }
}