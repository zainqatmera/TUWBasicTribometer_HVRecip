using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    internal class Helpers
    {
        public static string MakeUniqueFilePath(string dir, string filename)
        {
            string filenameRoot = filename;
            string filenameExt = "txt";
            
            int dot = filename.LastIndexOf('.');
            if (dot > -1) {
                filenameRoot = filename.Substring(0, dot);
                filenameExt = filename.Substring(dot + 1);
            }

            if (!dir.EndsWith("\\")) { 
                dir += "\\";
            }

            string currentFilePath = dir + filenameRoot + "." + filenameExt;

            int i = 1;
            while (File.Exists(currentFilePath))
            {
                currentFilePath = dir + filenameRoot + $" ({i})." + filenameExt;
                i++;
            }

            return currentFilePath;

        }
    }
}
