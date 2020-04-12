﻿using System;
using System.IO;
using Newtonsoft.Json;
using OWML.Common;

namespace OWML.ModHelper
{
    public class ModStorage : IModStorage
    {
        private readonly IModConsole _console;
        private readonly IModManifest _manifest;

        public ModStorage(IModConsole console, IModManifest manifest)
        {
            _console = console;
            _manifest = manifest;
        }

        public T Load<T>(string filename)
        {
            var path = _manifest.ModFolderPath + filename;
            if (!File.Exists(path))
            {
                return default;
            }
            try
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Error while loading {path}: {ex}");
                return default;
            }
        }

        public void Save<T>(T obj, string filename)
        {
            var path = _manifest.ModFolderPath + filename;
            try
            {
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Error while saving {path}: {ex}");
            }
        }

    }
}
