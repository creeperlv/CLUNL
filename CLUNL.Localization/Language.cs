﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CLUNL.Localization
{
    public interface ILocalized
    {
        void ApplyLanguage();
    }
    /// <summary>
    /// Represent a language resource collection.
    /// </summary>
    public class Language
    {
        private const string Locales = "Locales";
        private const string EqualSymbol = "=";
        private const string DefaultLang = "en-US.lang";
        private const string DefaultRegion = "en-US";
        private const string GeneratorPrefix = "; Generated By CLUNL.Localization";
        static Dictionary<string, string> LanguageStrings = new Dictionary<string, string>();
        static bool isInited = false;
        public static bool IsInited() => isInited;
        public static void Init(string SettingsFileName, string ProductName)
        {
            Init(ObtainInstalled(SettingsFileName, ProductName));
        }
        static string configuration = null;
        static string ObtainInstalled(string SettingFileName, string ProductName = "CLUNL")
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
        public static void Init(string PreferredLanguageCode)
        {
            isInited = true;
            //CultureInfo.CurrentUICulture.Name -> en-US
            var d = new FileInfo(typeof(Language).Assembly.Location).Directory;
            var langfile = Path.Combine(d.FullName, Locales, DefaultLang);
            LoadLanguageFile(langfile);
            if (PreferredLanguageCode == DefaultRegion)
                return;
            langfile = Path.Combine(d.FullName, Locales, PreferredLanguageCode + ".lang");
            if (File.Exists(langfile))
                LoadLanguageFile(langfile);
        }
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
        public static void LoadLanguageFile(string FilePath)
        {
            var contents = File.ReadAllLines(FilePath);
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
        public static void LoadFromStringArray(string [] contents)
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
        public static void LoadFromString(string content)
        {
            using (StringReader stringReader = new StringReader(content))
            {
                string item=null;
                while ((item =stringReader.ReadLine())!=null)
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
        public static string FindF(string Key, string Fallback = "", params string[] parameters)
        {
            var os = Find(Key, Fallback);
            return string.Format(os, parameters);
        }
        public static string FindF(string Key, string Fallback = "", params object[] parameters)
        {
            var os = Find(Key, Fallback);
            return string.Format(os, parameters);
        }
        public static string Find(string Key, string Fallback = "")
        {
            if (!LanguageStrings.ContainsKey(Key))
                return Fallback;
            else return LanguageStrings[Key].Replace("\\r", "\r");
        }
    }
}