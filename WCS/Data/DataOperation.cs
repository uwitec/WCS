using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WCS.Data
{
   public  class DataOperation
    {
       public static int ChangeScannerData2StrLst(string strResult, ref string[] msgArray)
       {
           int intRst = 0;
           try
           {

               //Regex r1 = new Regex(@"^(<[0-9]{2}>[0-9]{9}\r\n)|(<[0-9]{2}>[0-9]{13}\r\n)|(<[0-9]{2}>NoRead\r\n)+$");//带拐角的.VS2010看不见
               //Regex r1 = new Regex("^(([0-9]{2}\\}[0-9]{7})|([0-9]{2}\\}[0-9]{14})|([0-9]{2}\\}NoRead))+$");
               //Regex r1 = new Regex(@"<[0-9]{2}>[0-9]{7,13}\r\n");
               msgArray = strResult.Substring(0, strResult.ToString().Length).Split('}');


               //Regex r1 = new Regex(@"^(<[0-9]{2}>[A-Z,a-z,0-9]{10})|(<[0-9]{2}>NoRead)+$");

               //if (r1.IsMatch(strResult))
               //{
               //    MatchCollection mc = r1.Matches(strResult);

               //    msgArray = null;
               //    if (mc.Count == 0)
               //    {
               //        return 0;
               //    }
               //    msgArray = new string[mc.Count];

               //    for (int i = 0; i < mc.Count; i++)
               //    {

               //        string tmp = mc[i].Value;
               //        tmp = tmp.Substring(0, tmp.Length );

               //        msgArray[i] = tmp;
               //        intRst = 0;

               //    }
               //}


           }

           catch (System.Exception e)
           {
               intRst = -1;
           }
           return intRst;
       }
    }
}
