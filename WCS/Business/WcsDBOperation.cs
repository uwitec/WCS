using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using WCS.Data;
using WCS.DevSystem;
//using System.Data;

namespace WCS.Business
{
   public  class WcsDBOperation
    {
       private static WcsDBOperation _instance = new WcsDBOperation();
       public static  New_Main_Form form;
       public static WcsDBOperation GetInstance(New_Main_Form form1)
       {
           form= form1;
           return _instance;
       }

    }
}
