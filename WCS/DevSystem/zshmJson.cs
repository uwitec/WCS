

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS.DevSystem
{
    /// <summary>
    /// json解析对应实体类
    /// </summary>
    public class zshmJson
    {
        public string status { get; set; }
        public string flag { get; set; }
        public Result[] result { get; set; }
    }


    public class Result
    {
        public string billId { get; set; }
        public string chCode { get; set; }
        public string ctCode { get; set; }
        public string priority { get; set; }

    }



}
