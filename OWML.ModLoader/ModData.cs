﻿using OWML.Common;

namespace OWML.ModLoader
{
    public class ModData : IModData
    {
        public IModManifest Manifest { get; }
        public IModConfig Config { get; }
        public IModConfig DefaultConfig { get; }

        public ModData(IModManifest manifest, IModConfig config, IModConfig defaultConfig)
        {
            Manifest = manifest;
            Config = config;
            DefaultConfig = defaultConfig;
        }

    }
}
