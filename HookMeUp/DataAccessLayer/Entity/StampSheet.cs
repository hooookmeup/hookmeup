using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS.DL.Entity
{
   public class StampSheet
    {
       public string SheetId { get; set; }
       public string UserId { get; set; }
       public string CustomerId { get; set; }
       public int NumPrinted { get; set; }
    }
}
