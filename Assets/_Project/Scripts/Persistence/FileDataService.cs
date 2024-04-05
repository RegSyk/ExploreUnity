using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Explore.Persistence
{
    public class FileDataService : IDataService 
    {
        private ISerializer _serializer;
        private string _dataPath;
        private string _fileExtension;

        public FileDataService(ISerializer serializer) 
        {
            this._dataPath = Application.persistentDataPath;
            this._fileExtension = "json";
            this._serializer = serializer;
        }

        public void Save(AppData data, bool overwrite = true) 
        {
            string fileLocation = GetPathToFile(data.Name);

            if (!overwrite && File.Exists(fileLocation)) 
            {
                throw new IOException($"The file '{data.Name}.{_fileExtension}' already exists and cannot be overwritten.");
            }

            File.WriteAllText(fileLocation, _serializer.Serialize(data));
        }
        public AppData Load(string name) 
        {
            string fileLocation = GetPathToFile(name);

            if (!File.Exists(fileLocation)) 
            {
                throw new ArgumentException($"No persisted {typeof(AppData).Name} with name '{name}'");
            }

            return _serializer.Deserialize<AppData>(File.ReadAllText(fileLocation));
        }
        public void Delete(string name) 
        {
            string fileLocation = GetPathToFile(name);

            if (File.Exists(fileLocation)) 
            {
                File.Delete(fileLocation);
            }
        }
        public void DeleteAll() 
        {
            foreach (string filePath in Directory.GetFiles(_dataPath)) 
            {
                File.Delete(filePath);
            }
        }
        public IEnumerable<string> ListSaves() 
        {
            foreach (string path in Directory.EnumerateFiles(_dataPath)) 
            {
                if (Path.GetExtension(path) == _fileExtension) 
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        private string GetPathToFile(string fileName) => Path.Combine(_dataPath, string.Concat(fileName, ".", _fileExtension));
    }
}