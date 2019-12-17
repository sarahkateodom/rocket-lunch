using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RocketLunch.domain.Services
{
    public class ConfigDumper
    {
        public void Run(ILogger logger, IConfiguration config)
        {
            logger.LogInformation("\nAssembly Info\n");
            var ai = AssemblyInfo();
            foreach (var kv in ai)
            {
                logger.LogInformation("{0}: {1}", kv.Key, kv.Value);
            }

            logger.LogInformation("\nConfiguration\n");
            foreach (var c in config.AsEnumerable())
            {
                logger.LogInformation("Key: {0}, Value: {1}", c.Key, c.Value);
            }
        }

        private List<KeyValuePair<string, string>> AssemblyInfo()
        {
            var results = new List<KeyValuePair<string, string>>();
            var propsToGet = new List<string>() { "AssemblyProductAttribute", "AssemblyCopyrightAttribute", "AssemblyCompanyAttribute", "AssemblyDescriptionAttribute", "AssemblyFileVersionAttribute" };
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (var attribute in assembly.GetCustomAttributesData())
            {
                if (propsToGet.Contains(attribute.AttributeType.Name))
                {
                    if (!attribute.TryParse(out string value))
                    {
                        value = string.Empty;
                    }
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        results.Add(new KeyValuePair<string, string>(attribute.AttributeType.Name, value));
                    }
                }
            }
            return results;
        }
    }

    public static class AssembyInfoHelper
    {
        /// <summary>
        /// Try Parse a <c>System.Reflection.CustomAttributeData</c> into a string
        /// </summary>
        /// <param name="attribute">(this)</param>
        /// <param name="s">Strng to parse into</param>
        /// <returns>True if success</returns>
        public static bool TryParse(this System.Reflection.CustomAttributeData attribute, out string s)
        {
            var flag = false;
            s = attribute.ToString();
            var i = s.IndexOf('"');
            if (i >= 0) { s = s.Substring(i + 1); flag = true; }
            i = s.IndexOf('"');
            if (i >= 0) { s = s.Substring(0, i); flag = true; }
            return flag;
        }
    }
}