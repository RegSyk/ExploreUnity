using System.Collections.Generic;
namespace Explore.Persistence 
{
    public interface IDataService 
    {
        void Save(AppData data, bool overwrite = true);
        AppData Load(string name);
        void Delete(string name);
        void DeleteAll();
        IEnumerable<string> ListSaves();
    }
}