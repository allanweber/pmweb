using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ETLService.App
{
    public static class ResourceFile
    {
        public static List<string> LoadFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ETLService.App.20170519_Clientes.txt";
            List<string> result = new List<string>();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                        result.Add(reader.ReadLine());
                }
            }

            if (result.Count > 0)
            {
                if (result[0].ToLower().StartsWith("e-mail"))
                    result.RemoveAt(0);
            }

            return result;
        }
    }
}
