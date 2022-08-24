﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CLUNL.Localization
{
    /// <summary>
    /// Represent a language resource collection.
    /// </summary>
    public class Language
    {
        private static string DataFolder = null;
        private static string Locales = "Locales";
        internal const string EqualSymbol = "=";
        private const string DefaultLang = "en-US.lang";
        private const string DefaultRegion = "en-US";
        internal const string GeneratorPrefix = "; Generated By CLUNL.Localization";
        static LanguageDefinition LanguageStrings = new LanguageDefinition();
        /// <summary>
        /// Enumerate all key-value pairs.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> EnumerateValues()
        {
            foreach (var item in LanguageStrings)
            {
                yield return item;
            }
        }
        /// <summary>
        /// Set a value. 
        /// </summary>
        /// <param name="K"></param>
        /// <param name="V"></param>
        public void SetValue(string K, string V)
        {
            LanguageStrings[K] = V;
        }
        static bool isInited = false;
        /// <summary>
        /// If current language object is initialized with language files.
        /// </summary>
        /// <returns></returns>
        public static bool IsInited() => isInited;

        static string SettingFileName;
        static string ProductName = "CLUNL";

        /// <summary>
        ///  Init languages with given settings file and a product name.
        /// </summary>
        /// <param name="SettingsFileName"></param>
        /// <param name="ProductName"></param>
        public static void Init(string SettingsFileName, string ProductName)
        {
            SettingFileName = SettingsFileName;
            Language.ProductName = ProductName;
            Init(ObtainLanguageInUse());
        }
        static string configuration = null;
        /// <summary>
        /// Get the language code in use.
        /// </summary>
        /// <returns></returns>
        public static string ObtainLanguageInUse()
        {
            if (configuration != null)
            {
                return configuration;
            }
            configuration = DefaultRegion;
            if (File.Exists("./" + SettingFileName))
            {
                configuration = File.ReadAllLines("./" + SettingFileName)[0];
            }
            else
            {
                var p0 = Path.Combine(new FileInfo(typeof(Language).Assembly.Location).Directory.FullName, SettingFileName);
                if (File.Exists(p0))
                {
                    configuration = LoadFromFile(p0);
                }
                else
                {
                    var config = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var Configurations = Path.Combine(config, ProductName);
                    if (!Directory.Exists(Configurations)) Directory.CreateDirectory(Configurations);
                    var p1 = Path.Combine(Configurations, SettingFileName);
                    if (File.Exists(p1))
                    {
                        configuration = LoadFromFile(p1);
                    }
                    else
                    {
                        File.WriteAllText(p1, CultureInfo.CurrentUICulture.Name);
                    }
                }
            }
            string LoadFromFile(string p)
            {
                return File.ReadAllLines(p)[0];
            }
            return configuration;
        }
        /// <summary>
        /// Initialize language with given language code, eg: en-US. Then save ths language code into the configuration file.
        /// </summary>
        /// <param name="languageCode"></param>
        public static void SetLanguageCode(string languageCode)
        {
            var __path_0 = "./" + SettingFileName;
            if (File.Exists(__path_0))
            {
                File.Delete(__path_0);
                File.WriteAllText(__path_0, languageCode);
            }
            else
            {
                var DataPath = DataFolder;
                if (DataPath == null)
                {
                    var d = new FileInfo(typeof(Language).Assembly.Location).Directory;
                    DataPath = d.FullName;
                }
                var p0 = Path.Combine(DataPath, SettingFileName);
                if (File.Exists(p0))
                {
                    File.Delete(p0);
                    File.WriteAllText(p0, languageCode);
                }
                else
                {
                    var config = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var Configurations = Path.Combine(config, ProductName);
                    if (!Directory.Exists(Configurations)) Directory.CreateDirectory(Configurations);
                    var p1 = Path.Combine(Configurations, SettingFileName);
                    if (File.Exists(p1))
                    {
                        File.Delete(p1);
                        File.WriteAllText(p1, languageCode);
                    }
                    else
                    {
                        File.WriteAllText(p1, languageCode);
                    }
                }
            }
            Init(languageCode);
        }
        /// <summary>
        /// Set Locales folder. "Locals" is the default value.
        /// </summary>
        /// <param name="foldername"></param>
        public static void SetLocales(string foldername)
        {
            Locales = foldername;
        }
        /// <summary>
        /// Sets 'DataPath', null means where `CLUNL.Localization.dll` exists.
        /// </summary>
        /// <param name="DataPath"></param>
        public static void SetDataPath(string DataPath)
        {
            DataFolder = DataPath;
        }
        /// <summary>
        /// Enumerate all language files from DataPatt/Locals folder.
        /// </summary>
        /// <returns></returns>
        public static List<string> EnumerateLanguageCodes()
        {

            string DataPath = DataFolder;
            if (DataPath == null)
            {
                var d = new FileInfo(typeof(Language).Assembly.Location).Directory;
                DataPath = d.FullName;
            }
            var __lang_d = Path.Combine(DataPath, Locales);
            var L0 = (new DirectoryInfo(__lang_d)).EnumerateFiles();
            var L1 = new List<string>();
            foreach (var item in L0)
            {
                if (item.Name.ToUpper().EndsWith(".LANG"))
                {
                    L1.Add(item.Name.Substring(0, item.Name.Length - 5));
                }
            }

            return L1;
        }
        /// <summary>
        /// Initialize language with given language code, eg: en-US.
        /// </summary>
        /// <param name="PreferredLanguageCode"></param>
        public static void Init(string PreferredLanguageCode)
        {
            isInited = true;
            //CultureInfo.CurrentUICulture.Name -> en-US
            string DataPath = DataFolder;
            if (DataPath == null)
            {
                var d = new FileInfo(typeof(Language).Assembly.Location).Directory;
                DataPath = d.FullName;
            }
            var langfile = Path.Combine(DataPath, Locales, DefaultLang);
            if (File.Exists(langfile))
                LoadLanguageFile(langfile);
            if (PreferredLanguageCode == DefaultRegion)
                return;
            langfile = Path.Combine(DataPath, Locales, PreferredLanguageCode + ".lang");
            if (File.Exists(langfile))
                LoadLanguageFile(langfile);
        }
        /// <summary>
        /// Save current definitions into a file.
        /// </summary>
        /// <param name="TargetFile"></param>
        public static void SaveCurrentDefinition(FileInfo TargetFile)
        {
            TargetFile.Delete();
            var sw = TargetFile.CreateText();
            sw.WriteLine(GeneratorPrefix);
            foreach (var item in LanguageStrings)
            {
                sw.Write(item.Key);
                sw.Write(EqualSymbol);
                sw.WriteLine(item.Value);
                sw.Flush();
            }
            sw.Close();
        }
        /// <summary>
        /// Load a language file and merge it into current definitions.
        /// </summary>
        /// <param name="FilePath"></param>
        public static void LoadLanguageFile(string FilePath)
        {
            var contents = File.ReadAllLines(FilePath);
            LoadFromStringArray(contents);
        }
        /// <summary>
        /// Load a language from a string array and merge it into current definitions.
        /// </summary>
        /// <param name="contents"></param>
        public static void LoadFromStringArray(string[] contents)
        {
            foreach (var item in contents)
            {
                var pitem = item.Trim();
                if (pitem.StartsWith("#") || pitem.StartsWith(";") || pitem.StartsWith("//"))
                {
                    continue;
                }
                else
                {
                    var index = pitem.IndexOf(EqualSymbol);
                    if (index > 0)
                    {
                        var pairName = pitem.Substring(0, index);
                        var pairContent = pitem.Substring(index + 1);
                        if (!LanguageStrings.ContainsKey(pairName))
                            LanguageStrings.Add(pairName, pairContent);
                        else LanguageStrings[pairName] = pairContent;
                    }
                }
            }
        }
        /// <summary>
        /// Load a language from a string and merge it into current definitions.
        /// </summary>
        /// <param name="content"></param>
        public static void LoadFromString(string content)
        {
            using (StringReader stringReader = new StringReader(content))
            {
                string item = null;
                while ((item = stringReader.ReadLine()) != null)
                {
                    var pitem = item.Trim();
                    if (pitem.StartsWith("#") || pitem.StartsWith(";") || pitem.StartsWith("//"))
                    {
                        continue;
                    }
                    else
                    {
                        var index = pitem.IndexOf(EqualSymbol);
                        if (index > 0)
                        {
                            var pairName = pitem.Substring(0, index);
                            var pairContent = pitem.Substring(index + 1);
                            if (!LanguageStrings.ContainsKey(pairName))
                                LanguageStrings.Add(pairName, pairContent);
                            else LanguageStrings[pairName] = pairContent;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Find a string with given fallback using string.Format(...).
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Fallback"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string FindF(string Key, string Fallback = "", params string[] parameters)
        {
            var os = Find(Key, Fallback);
            return string.Format(os, parameters);
        }

        /// <summary>
        /// Find a string with given fallback using string.Format(...).
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Fallback"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string FindF(string Key, string Fallback = "", params object[] parameters)
        {
            var os = Find(Key, Fallback);
            return string.Format(os, parameters);
        }
        /// <summary>
        /// Find a string with given fallback.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Fallback"></param>
        /// <returns></returns>
        public static string Find(string Key, string Fallback = "")
        {
            if (!LanguageStrings.ContainsKey(Key))
                return Fallback;
            else return LanguageStrings[Key].Replace("\\r", "\r");
        }
        /// <summary>
        /// Remove loaded strings, hope to help reduce resource usage.
        /// </summary>
        public static void ClearLoadedStrings()
        {
            LanguageStrings.Clear();
        }
    }
}
