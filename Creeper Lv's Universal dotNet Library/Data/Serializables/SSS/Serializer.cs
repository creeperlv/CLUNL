using System.Collections.Generic;
using System.Text;
/// <summary>
/// Shell Scrip Style
/// </summary>
namespace CLUNL.Data.Serializables.SSS
{
    /// <summary>
    /// Serializer
    /// </summary>
    public class Serializer
    {
        private const string BLANK = " ";
        private const string COLON = ":";
        private const string QUOTE = "\"";
        private const string DASH = "-";
        /// <summary>
        /// Serialize the data list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<string> Serialize<T>(List<T> data)
        {
            List<string> result = new List<string>();
            StringBuilder stringBuilder= new StringBuilder();
            foreach (var item in data)
            {
                stringBuilder.Clear();
                var Item_T=item.GetType();
                var Fields=Item_T.GetFields();
                stringBuilder.Append(Item_T.FullName);
                stringBuilder.Append(BLANK);
                foreach (var field in Fields)
                {
                    field.GetValue(item);
                    stringBuilder.Append(QUOTE);
                    stringBuilder.Append(DASH);
                    stringBuilder.Append(field.Name);
                    stringBuilder.Append(COLON);
                    stringBuilder.Append(field.GetValue(item).ToString());
                    stringBuilder.Append(QUOTE);
                    stringBuilder.Append(BLANK);
                }
                result.Add(stringBuilder.ToString());
            }
            return result;
        }
        /// <summary>
        /// Serialize the data list enumeratively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerator<string> EnumerativeSerialize<T>(List<T> data)
        {

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in data)
            {
                stringBuilder.Clear();
                var Item_T = item.GetType();
                var Fields = Item_T.GetFields();
                stringBuilder.Append(Item_T.FullName);
                stringBuilder.Append(BLANK);
                foreach (var field in Fields)
                {
                    field.GetValue(item);
                    stringBuilder.Append(QUOTE);
                    stringBuilder.Append(DASH);
                    stringBuilder.Append(field.Name);
                    stringBuilder.Append(COLON);
                    stringBuilder.Append(field.GetValue(item).ToString());
                    stringBuilder.Append(QUOTE);
                    stringBuilder.Append(BLANK);
                }
                yield return (stringBuilder.ToString());
            }
        }
    }
}
