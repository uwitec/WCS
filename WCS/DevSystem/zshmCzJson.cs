
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WCS.DevSystem
{
     public class zshmCzJson
     {
         public string status { get; set; }
         public string flag { get; set; }
         public Result1[] result { get; set; }
     }
     public class Result1
     {
         public string outResult { get; set; }
         public string outPort { get; set; }
     
     }
}
