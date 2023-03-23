using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Glavni_projekt
{
    [ComVisible(true)]
    public class MyScriptingClass
    {
        private string SomeData;
        public string GetSomeData()
        {
            return SomeData + " Something";
        }
        public void SetSomeData(string some)
        {
            SomeData = some;
        }
    }
 
}
