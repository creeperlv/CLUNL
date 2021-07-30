using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using CLUNL.Localization;

namespace CLUNL.ConsoleAppHelper
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DependentFeatureAttribute : Attribute
    {
        readonly string nameString;
        readonly string featureCollectionID;
        public DependentFeatureAttribute(string featureCollectionID, string nameString)
        {
            this.nameString = nameString;
            this.featureCollectionID = featureCollectionID;
        }
        public string FeatureCollectionID { get => featureCollectionID; }
        public string Name
        {
            get { return nameString; }
        }
        public string Description { get; set; } = "";
        public string[] Options { get; set; }
        public string[] OptionDescriptions { get; set; }
    }
    public class ConsoleAppHelper
    {
        public static void Out(ErrorMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                    Console.Write("E:" + msg.ID);
            }
            else
            {
                Out(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        public static void OutLine(ErrorMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                    Console.WriteLine("E:" + msg.ID);
            }
            else
            {
                OutLine(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        public static void Out(WarnMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                    Console.Write("W:" + msg.ID);
            }
            else
            {
                Out(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        public static void OutLine(WarnMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                    Console.WriteLine("W:" + msg.ID);
            }
            else
            {
                OutLine(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        public static void Out(object o)
        {
            if (isSlientMode is false && isBatchMode == false)
            {
                Console.Write(o);
            }
        }
        public static void OutLine()
        {
            if (isSlientMode is false && isBatchMode == false)
            {
                Console.WriteLine();
            }
        }
        public static void OutLine(object o)
        {
            if (isSlientMode is false && isBatchMode == false)
            {
                Console.WriteLine(o);
            }
        }

        private const string Separator = ": ";

        public static void OutLine(object o1, object o2)
        {
            if (isSlientMode is false || isBatchMode == false)
            {
                Console.Write(o1);
                Console.Write(Separator);
                Console.WriteLine(o2);
            }
        }
        static bool isSlientMode = false;
        static bool isBatchMode = false;
        static IFeatureCollectionVersion VersionProvider = null;
        static Dictionary<string, IFeature> features = new Dictionary<string, IFeature>();
        static Dictionary<string, DependentFeatureAttribute> infos = new Dictionary<string, DependentFeatureAttribute>();
        internal static string CurrentFeatureCollectionID;
        public static void Init(string FeatureCollectionID, string ProductName = "CLUNL")
        {
            StackTrace stackTrace = new StackTrace();
            var s = stackTrace.GetFrames();
            VersionProvider = new DefaultFeatureCollectionVersion(s[1].GetMethod().ReflectedType.Assembly);
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            CurrentFeatureCollectionID = FeatureCollectionID;
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in asms)
            {
                foreach (var type in asm.GetTypes())
                {
                    var Feature = type.GetCustomAttribute<DependentFeatureAttribute>();
                    if (Feature is not null)
                    {
                        if (Feature.FeatureCollectionID == FeatureCollectionID)
                        {
                            features.Add(Feature.Name.ToUpper(), (IFeature)Activator.CreateInstance(type));
                            infos.Add(Feature.Name.ToUpper(), Feature);
                        }
                    }
                }
            }
            foreach (var asm in asms)
            {
                foreach (var type in asm.GetTypes())
                {
                    var Feature = type.GetCustomAttribute<DependentVersionAttribute>();
                    if (Feature is not null)
                    {
                        if (Feature.FeatureCollectionID == FeatureCollectionID)
                        {
                            VersionProvider = (IFeatureCollectionVersion)Activator.CreateInstance(type);
                        }
                    }
                }
            }
            if (Language.IsInited() is false)
                Language.Init(FeatureCollectionID + "Language", ProductName);
        }
        public static void PrintHelp()
        {
            OutLine(Language.Find(CurrentFeatureCollectionID + ".Title", CurrentFeatureCollectionID));
            OutLine();
            var c = Language.Find(CurrentFeatureCollectionID + ".Copyright", null);
            if (c is not null)
            {
                OutLine(c);
                OutLine();
            }
            var l = Language.Find(CurrentFeatureCollectionID + ".License", null);
            if (l is not null)
            {
                OutLine(l);
                OutLine();
            }
            OutLine(Language.Find("General.Console.Usage", "Usage:"));
            var command = Language.Find("General.Console.Command", "Command");
            var option = Language.Find("General.Console.Option", "Option");
            var MainParameter = Language.Find("General.Console.MainParameter", "Main Parameter");
            OutLine();
            OutLine($"\t{new FileInfo(Assembly.GetEntryAssembly().Location).Name} (exe) <{command}> [{option}] [...] [{MainParameter}]");
            OutLine();
            OutLine(Language.Find("General.Console.Commands", "Commands:"));
            OutLine();
            foreach (var item in features)
            {
                PrintHelp(item.Key);
            }
        }
        public static void PrintHelp(string FeatureName)
        {
            var FeatureDesc = Language.Find($"{CurrentFeatureCollectionID}.Commands.{FeatureName}", infos[FeatureName].Description);
            OutLine(FeatureName);
            OutLine();
            if (FeatureDesc != "" && FeatureName != null)
            {
                Out("\t");
                OutLine(FeatureDesc);
                OutLine();
            }
            OutLine("\t" + Language.Find("General.Console.Options", "Options:"));
            OutLine();
            ParameterList p = new ParameterList();
            p.ApplyDescription(infos[FeatureName]);
            Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();
            foreach (var item in p.Parameters)
            {
                if (keyValuePairs.ContainsKey(item.Value))
                {
                    keyValuePairs[item.Value].Add(item.Key);
                }
                else
                {
                    keyValuePairs.Add(item.Value, new List<string>() { item.Value });
                }
            }
            foreach (var item in keyValuePairs)
            {
                Out("\t");
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (i == 0)
                    {
                        Out($"-{item.Value[i]}");
                    }
                    else
                        Out($", -{item.Value[i]}");
                }
                OutLine();
                int index = -1;
                for (int i = 0; i < p.Options.Count; i++)
                {
                    if (p.Options.ElementAt(i).Key == item.Key)
                    {
                        index = i;
                        break;
                    }
                }
                string fallback = "";
                if (infos[FeatureName].OptionDescriptions is not null)
                    if (infos[FeatureName].OptionDescriptions.Length > 0 && index is not -1)
                    {
                        fallback = infos[FeatureName].OptionDescriptions[index];
                    }
                OutLine($"\t{Language.Find(CurrentFeatureCollectionID + ".Options." + item.Key, fallback)}");

            }
        }
        public static void PrintVersion()
        {
            Output.OutLine("General.Console.Version", "Version: {0}", VersionProvider.GetVersionString());
        }
        public static void Execute(params string[] parameters)
        {
            IFeature toExecute = null;
            string FeatureName = "";
            var Length = parameters.Length;
            ParameterList p = new();
            Dictionary<string, object> RawOptions = new Dictionary<string, object>();
            string MainParameter = "";
            for (int i = 0; i < Length; i++)
            {
                var item = parameters[i];
                if (item.StartsWith("--"))
                {
                    //An Option.
                    Process(item.ToUpper(), 2);
                }
                else if (item.StartsWith("-"))
                {
                    //An Option.
                    Process(item.ToUpper(), 1);
                }
                else if (item.StartsWith("/"))
                {
                    //An Option.
                    Process(item.ToUpper(), 1);
                }
                else
                {
                    if (FeatureName == "")
                    {
                        if (features.ContainsKey(item.ToUpper()))
                        {
                            toExecute = features[item.ToUpper()];
                            FeatureName = item.ToUpper();
                        }
                    }
                    else
                    {
                        MainParameter = item;
                    }
                }
                void Process(string Item, int prefixLength)
                {
                    var oN = Item.Substring(prefixLength).ToUpper();
                    if (oN == "?" || oN == "H" || oN == "HELP")
                    {
                        if (FeatureName != "")
                            PrintHelp(FeatureName);
                        else PrintHelp();
                        //Terminate immediately.
                        toExecute = new DefaultBlankFeature();
                        return;
                    }
                    if (oN == "V" || oN == "VER" || oN == "VERSION")
                    {
                        PrintVersion();
                        toExecute = new DefaultBlankFeature();
                        return;
                    }
                    else if (oN == "S" || oN == "SLIENT")
                    {
                        isSlientMode = true;
                    }
                    else if (oN == "B" || oN == "BATCH")
                    {
                        isBatchMode = true;
                    }
                    else
                    {
                        string Name;
                        string Value;
                        if (oN.Contains(':'))
                        {
                            Name = oN.Substring(0, oN.IndexOf(':'));
                            Value = oN.Substring(oN.IndexOf(':') + 1);

                        }
                        else
                        if (oN.Contains('='))
                        {

                            Name = oN.Substring(0, oN.IndexOf('='));
                            Value = oN.Substring(oN.IndexOf('=') + 1);
                        }
                        else
                        {
                            Name = oN;
                            if (i + 1 < Length)
                            {
                                Value = parameters[i + 1];
                                if (Value.StartsWith("-") || Value.StartsWith("/"))
                                {
                                    Value = bool.TrueString;
                                }
                                else
                                    i++;
                            }
                            else
                                Value = bool.TrueString;
                        }
                        if (bool.TryParse(Value, out bool a))
                        {
                            RawOptions.Add(Name, a);
                        }
                        else if (int.TryParse(Value, out int i))
                        {
                            RawOptions.Add(Name, i);
                        }
                        else if (float.TryParse(Value, out float f))
                        {
                            RawOptions.Add(Name, f);
                        }
                        else if (double.TryParse(Value, out double d))
                        {
                            RawOptions.Add(Name, d);
                        }
                        else if (BigInteger.TryParse(Value, out BigInteger BI))
                        {
                            RawOptions.Add(Name, BI);
                        }
                        else RawOptions.Add(Name, Value);
                    }
                }
            }
            if (toExecute == null)
            {
                PrintHelp();
            }
            else
            {
                if (FeatureName != "")
                {
                    p.ApplyDescription(infos[FeatureName]);
                    foreach (var item in RawOptions)
                    {
                        p.AddKey(item.Key, item.Value);
                    }
                }
                toExecute.Execute(p, MainParameter);
            }
        }
    }
    public class ParameterList
    {
        public Dictionary<string, object> Options = new Dictionary<string, object>();
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();//Variant -> main key
        internal void AddKey(string Key, object Value)
        {
            if (!Options.ContainsKey(Parameters[Key.ToUpper()]))
                Options.Add(Parameters[Key.ToUpper()], Value);
            else Options[Parameters[Key.ToUpper()]] = Value;
        }
        public object Query(string KeyVariant)
        {
            return Options[Parameters[KeyVariant.ToUpper()]];
        }
        public T Query<T>(string KeyVariant)
        {
            return (T)Options[Parameters[KeyVariant.ToUpper()]];
        }
        public void ApplyDescription(DependentFeatureAttribute dependentFeatureAttribute)
        {
            foreach (var item in dependentFeatureAttribute.Options)
            {
                var variants = item.Split(',');
                foreach (var variant in variants)
                {
                    Parameters.Add(variant.ToUpper(), variants[0].ToUpper());
                }
                Options.Add(variants[0].ToUpper(), false);
            }
        }
    }
    public interface IFeatureCollectionVersion
    {
        string GetVersionString();
    }
    public interface IFeature
    {
        void Execute(ParameterList Parameters, string MainParameter);
    }
}
