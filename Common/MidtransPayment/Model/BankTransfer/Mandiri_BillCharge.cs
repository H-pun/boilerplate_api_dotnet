using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MidtransPayment.Model
{
    public class EChannel {
        public string bill_info1 { get; set; }
        public string bill_info2 { get; set; }
    }
    
    public class Mandiri_BillCharge : BasePaymentModel
    {
        public EChannel echannel { get; set; }
    }
}
