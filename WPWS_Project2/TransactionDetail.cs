using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPWS_Project2
{
    //DEAN JONES 
    //JUN.07, 2017 
    //TRANSACTION DETAILS CLASS 
    //holds all the detail information for one row of detail data

    //SUPPORTING FILES 
    //Transaction.cs CLASS (main class)


    public class TransactionDetail
    {
        //EXAMPLE (ROW)(DETAIL)
        //D	741487	N	2	162.88	325.76	16.29
        //
        //FORMAT (DETAIL ONLY)
        //NAME (KEY)                        VALUE
        //--------------------------------------------
        //Record Type	                    D	
        //Vendor Product Number	            759064	
        //Catch Weight Indicator	        N	
        //Invoice Quantity	                1	
        //Invoice Price	                    251.48	
        //Extended Value	                264.05	
        //Tax Value                         12.57


        //PROPERTIES
        public string RecordType { get; set; }
        public int VendorProductNo { get; set; }
        public string CatchWeightIndicator { get; set; }
        public int InvoiceQuantity { get; set; }
        public double InvoicePrice { get; set; }
        public double ExtendedValue { get; set; }
        public double TaxValue { get; set; }

        //CONSTRUCTOR 
        public TransactionDetail() { }

        //TOSTRING OVERRIDE
        public override string ToString()
        {
            return RecordType + "\t" +
                VendorProductNo + "\t" +
                CatchWeightIndicator + "\t" +
                InvoiceQuantity + "\t" +
                InvoicePrice + "\t" +
                ExtendedValue + "\t" +
                TaxValue;
        }

        //TOSTRING (with delimitter)
        public string ToString(string delim)
        {
            return RecordType + delim +
                VendorProductNo + delim +
                CatchWeightIndicator + delim +
                InvoiceQuantity + delim +
                InvoicePrice + delim +
                ExtendedValue + delim +
                TaxValue;
        }
    }
}
