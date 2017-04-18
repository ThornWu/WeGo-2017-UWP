using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WeGo
{
    public class FunctionManager
    {
        public static ApplicationDataContainer localSettings = null;
        public static ApplicationDataCompositeValue functions = null;

        static FunctionManager()
        {
            localSettings = ApplicationData.Current.LocalSettings;
            functions = localSettings.Values["functions"] as ApplicationDataCompositeValue;
            if (functions == null)
            {
                functions = new ApplicationDataCompositeValue();
            }
        }

        public static bool IsFunctionAdded(string s)
        {
            return functions[s] != null;
        }

        public static void FunctionAdd(string s)
        {
            functions[s]=true;
            localSettings.Values["functions"] = functions;
        }

        public static void FunctionRemove(string s)
        {
            functions.Remove(s);
            localSettings.Values["functions"] = functions;
        }
    }
}
