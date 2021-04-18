﻿using System.IO;

namespace ArchitectsLibrary.CreatorKit.Utility
{
    internal static class PackFolderUtils
    {
        private static string packFolderPath;

        public const string packFolderName = "CreationKit";

        /// <summary>
        /// Returns the path to the Packs folder, an example would be C:\Program Files (x86)\Steam\steamapps\common\Subnautica\SNCK. Generates the folder if it doesn't exist.
        /// </summary>
        public static string GetPackFolderPath()
        {
            if (string.IsNullOrEmpty(packFolderPath)) //It has no reason to be cached but IDC.
            {
                RefreshPackFolderPath();
            }
            if (!Directory.Exists(packFolderPath)) //Create directory if it doesn't exist
            {
                Directory.CreateDirectory(packFolderPath);
            }
            return packFolderPath;
        }

        /// <summary>
        /// Updates the cached folder path <see cref="GetPackFolderPath"/>. This is generally done automatically so I'm not sure if it will ever be necessary.
        /// </summary>
        public static void RefreshPackFolderPath()
        {
            packFolderPath = Path.Combine(Utils.GetSNFolderPath, packFolderName);
        }
    }
}
