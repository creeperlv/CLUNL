using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CLUNL.Localization;

namespace CLUNL.ConsoleAppHelper
{
    /// <summary>
    /// Indicates that a class is a console Application feature.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DependentFeatureAttribute : Attribute
    {
        readonly string nameString;
        readonly string featureCollectionID;
        /// <summary>
        /// Initializes a new instance of the `DependentFeatureAttribute` class.
        /// </summary>
        /// <param name="featureCollectionID">The ID of the collection, in case an assembly contains features that should be accessed in separated dependent application.</param>
        /// <param name="nameString"></param>
        public DependentFeatureAttribute(string featureCollectionID, string nameString)
        {
            this.nameString = nameString;
            this.featureCollectionID = featureCollectionID;
        }

        /// <summary>
        /// The ID of the collection, in case an assembly contains features that should be accessed in separated dependent application.
        /// </summary>
        public string FeatureCollectionID
        {
            get => featureCollectionID;
        }
        /// <summary>
        /// The name of the feature.
        /// </summary>
        public string Name
        {
            get
            {
                return nameString;
            }
        }
        /// <summary>
        /// Description of the feature.
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// Available options.
        /// </summary>
        public string[] Options
        {
            get; set;
        }
        /// <summary>
        /// Option descriptions.
        /// </summary>
        public string[] OptionDescriptions
        {
            get; set;
        }
    }
    /// <summary>
    /// Main class of the CLUNL.ConsoleAppHelper.
    /// </summary>
    public class ConsoleAppHelper
    {
        /// <summary>
        /// Method to execute before the execution (after the analysis of parameters).
        /// </summary>
        public static Action PreExecution = null;
        /// <summary>
        /// If the output uses color.
        /// </summary>
        public static bool Colorful = false;
        /// <summary>
        /// Output an error message.
        /// </summary>
        /// <param name="msg"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Out(ErrorMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                {
                    if (Colorful)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Out("E:");

                    if (Colorful)
                    {
                        Console.ResetColor();
                    }
                    Out(msg.ID);

                }
            }
            else
            {
                if (Colorful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Out("Error:");
                if (Colorful)
                {
                    Console.ResetColor();
                }
                Out(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        /// <summary>
        /// Output an error message, followed by the current line terminator.
        /// </summary>
        /// <param name="msg"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OutLine(ErrorMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                {
                    if (Colorful)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Out("E:");

                    if (Colorful)
                    {
                        Console.ResetColor();
                    }
                    OutLine(msg.ID);
                }
            }
            else
            {
                if (Colorful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Out("Error:");
                if (Colorful)
                {
                    Console.ResetColor();
                }
                OutLine(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        /// <summary>
        /// Output a warning message.
        /// </summary>
        /// <param name="msg"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Out(WarnMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                {

                    if (Colorful)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Out("W:");
                    if (Colorful)
                    {
                        Console.ResetColor();
                    }
                    Out(msg.ID);
                }
            }
            else
            {

                if (Colorful)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Out("Warning:");
                if (Colorful)
                {
                    Console.ResetColor();
                }
                Out(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        /// <summary>
        /// Output a warning message, followed by the current line terminator.
        /// </summary>
        /// <param name="msg"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OutLine(WarnMsg msg)
        {
            if (isBatchMode)
            {
                if (!isSlientMode)
                {


                    if (Colorful)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Out("W:");
                    if (Colorful)
                    {
                        Console.ResetColor();
                    }
                    OutLine(msg.ID);
                }
            }
            else
            {
                if (Colorful)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Out("Warning:");
                if (Colorful)
                {
                    Console.ResetColor();
                }
                OutLine(Language.Find(CurrentFeatureCollectionID + ".Errors." + msg.ID, msg.Fallback));
            }
        }
        /// <summary>
        /// Output an object.
        /// </summary>
        /// <param name="o"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Out(object o)
        {
            if (isSlientMode is false && isBatchMode == false)
            {
                Console.Write(o);
            }
        }
        /// <summary>
        /// Output a blank line.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OutLine()
        {
            if (isSlientMode is false && isBatchMode == false)
            {
                Console.WriteLine();
            }
        }
        /// <summary>
        /// OUtput on object, followed by the current line terminator.
        /// </summary>
        /// <param name="o"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OutLine(object o)
        {
            if (isSlientMode is false && isBatchMode == false)
            {
                Console.WriteLine(o);
            }
        }

        private const string Separator = ": ";
        /// <summary>
        /// Output two object in format of "`o1` : `o1`", followed by the current line terminator.
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// <summary>
        /// Initialize ConsoleAppHelper
        /// </summary>
        /// <param name="FeatureCollectionID"></param>
        /// <param name="ProductName"></param>
        public static void Init(string FeatureCollectionID, string ProductName = "CLUNL")
        {
            StackTrace stackTrace = new StackTrace();
            var s = stackTrace.GetFrames();
            VersionProvider = new DefaultFeatureCollectionVersion(s[1].GetMethod().ReflectedType.Assembly);
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            CurrentFeatureCollectionID = FeatureCollectionID;
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var LibAsm = typeof(ConsoleAppHelper).Assembly;
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
            var T_DFCV = typeof(DefaultFeatureCollectionVersion);
            foreach (var asm in asms)
            {
                if (asm != LibAsm)
                    foreach (var type in asm.GetTypes())
                    {
                        var Feature = type.GetCustomAttribute<DependentVersionAttribute>();
                        if (Feature is not null)
                            if (type != T_DFCV)
                            {
                                if (Feature.FeatureCollectionID == FeatureCollectionID)
                                {
                                    VersionProvider = (IFeatureCollectionVersion)Activator.CreateInstance(type);
                                }
                            }
                    }
            }
            if (Language.IsInited() is false)
                if (LanguageConfigurationFile == null)
                {
                    Language.Init(FeatureCollectionID + "Language", ProductName);
                }
                else
                {
                    Language.Init(LanguageConfigurationFile, ProductName);
                }
        }
        /// <summary>
        /// The name of configuration, setting it to null will make ConsoleAppHelper use `FeatureCollectionID + "Language"`.
        /// </summary>
        public static string LanguageConfigurationFile = null;
        /// <summary>
        /// Print out a auto-generated help document.
        /// </summary>
        public static void PrintHelp()
        {
            OutLine(Language.Find(CurrentFeatureCollectionID + ".Title", CurrentFeatureCollectionID));
            OutLine();
            {
                var c = Language.Find(CurrentFeatureCollectionID + ".Description", null);
                if (c is not null)
                {
                    OutLine(c);
                    OutLine();
                }
            }
            {
                var c = Language.Find(CurrentFeatureCollectionID + ".Copyright", null);
                if (c is not null)
                {
                    OutLine(c);
                    OutLine();
                }
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
        /// <summary>
        /// Print out a auto-generated help document for a certain feature.
        /// </summary>
        /// <param name="FeatureName"></param>
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
            if (infos[FeatureName].Options != null)
                if (infos[FeatureName].OptionDescriptions != null)
                {
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
                        var final = Language.Find(CurrentFeatureCollectionID + ".Options." + item.Key, fallback);
                        if (final != "")
                        {

                            OutLine(Environment.NewLine);
                            OutLine($"\t\t{Language.Find(CurrentFeatureCollectionID + ".Options." + item.Key, fallback)}");
                            OutLine();
                        }

                    }

                }
        }
        /// <summary>
        /// Print out localized version message.
        /// </summary>
        public static void PrintVersion()
        {
            Output.OutLine("General.Console.Version", "Version: {0}", VersionProvider.GetVersionString());
        }
        /// <summary>
        /// Use it to handle parameters.
        /// </summary>
        /// <example>
        ///     class Program
        ///    {
        ///        static void Main(string[] args)
        ///        {
        ///            ConsoleAppHelper.ConsoleAppHelper.Init("Sample");
        ///            ConsoleAppHelper.ConsoleAppHelper.Execute(args);
        ///        }
        ///    }
        /// </example>
        /// <param name="parameters"></param>
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
                        if (PreExecution is not null)
                            PreExecution();
                        if (FeatureName != "")
                            PrintHelp(FeatureName);
                        else
                            PrintHelp();
                        //Terminate immediately.
                        toExecute = new DefaultBlankFeature();
                        return;
                    }
                    if (oN == "V" || oN == "VER" || oN == "VERSION")
                    {
                        if (PreExecution is not null)
                            PreExecution();
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
                        else
                            RawOptions.Add(Name, Value);
                    }
                }
            }
            if (toExecute == null)
            {
                if (PreExecution is not null)
                    PreExecution();
                PrintHelp();
            }
            else
            {
                if (toExecute is DefaultBlankFeature)
                {
                    return;
                }
                if (FeatureName != "")
                {
                    p.ApplyDescription(infos[FeatureName]);
                    foreach (var item in RawOptions)
                    {
                        p.AddKey(item.Key, item.Value);
                    }
                }
                if (PreExecution is not null)
                    PreExecution();
                toExecute.Execute(p, MainParameter);
            }
        }
    }
    /// <summary>
    /// Defines a method of getting version.
    /// </summary>
    public interface IFeatureCollectionVersion
    {
        /// <summary>
        /// Returns a version string.
        /// </summary>
        /// <returns></returns>
        string GetVersionString();
    }
    /// <summary>
    /// Defines a method of Console feature.
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// Entry method of a feature.
        /// </summary>
        /// <param name="Parameters"></param>
        /// <param name="MainParameter"></param>
        void Execute(ParameterList Parameters, string MainParameter);
    }
}
