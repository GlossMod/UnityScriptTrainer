using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptTrainer
{
    public static class IntExtensions
    {
        public static int ConvertToIntDef(this string input, int defaultValue)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
