using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPWS_Project2
{
    //DEAN JONES 
    //JUN.07, 2017 
    //TRANSACTION HEADER CLASS 
    //contains all the info for the transaction header only

    //SUPPORTING FILES 
    //Transaction.cs CLASS (main class)


    public class TransactionHeader
    {
        //EXAMPLE
        //H	LIQ513	OJ0071	PO1059 	<BLANK>     60420	04/20/2017	3199.46	GST/HST	140.29	Fuel/Delivery/Freight	.00	Bottle Deposits	228.40
        //
        //FORMAT (HEADER ONLY)
        //NAME (KEY)                        VALUE
        //--------------------------------------------
        //Record Type                       H 
        //Vendor Code                       WILPAR
        //Location Code                     OJ0069
        //PO #	                            PO1059
        //Expected Delivery Date	        <BLANK>
        //
        //Vendor Invoice #	                87589   
        //Invoice Date	                    04/20/2017
        //Invoice Total	                    4014.8	
        //
        //Tax GL Description	            GST/HST                
        //Tax Value	                        140.29
        //
        //Freight/Shipping GL Description   Fuel/Delivery/Freight
        //Freight/Shipping Value	        .00
        //
        //Misc. 1 GL Description	        Bottle Deposits            
        //Misc. 1 Value                     56.8
        //	                    


        //PROPERTIES
        public string RecordType { get; set; }
        public string VendorCode { get; set; }
        public string LocationCode { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public int VendorInvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double InvoiceTotal { get; set; }
        public string TaxGlDescription { get; set; }
        public double? TaxValue { get; set; }
        public string FreightShippingGlDescription { get; set; }
        public double? FreightShippingValue { get; set; }
        public string Misc1GlDescription { get; set; }
        public double Misc1Value { get; set; }

        //CONSTRUCTOR 
        public TransactionHeader() { }

        //TOSTRING OVERRIDE
        public override string ToString()
        {
            return RecordType + "\t" +
                VendorCode + "\t" +
                LocationCode + "\t" +
                PurchaseOrderNo + "\t" +
                ((Object)ExpectedDeliveryDate ?? " ") + "\t" +
                VendorInvoiceNo + "\t" +
                InvoiceDate.ToString("MM'/'dd'/'yyyy") + "\t" +         //11/16/2016 	(forces the "/" separator)		
                InvoiceTotal.ToString("0.00") + "\t" +                  //format (two decimal places)

                (TaxGlDescription ?? " ") + "\t" +
                //(handle NULL value and converting (double? to double) in the correct format)               
                MyDoubleToString(TaxValue) + "\t" +
                //((Object)TaxValue ?? " ") + "\t" +

                (FreightShippingGlDescription ?? " ") + "\t" +
                //(handle NULL value and converting (double? to double) in the correct format)               
                MyDoubleToString(FreightShippingValue) + "\t" +
                //((Object)FreightShippingValue ?? " ") + "\t" +

                Misc1GlDescription + "\t" +
                Misc1Value.ToString("0.00");
        }

        //METHOD DOUBLE (handle NULL value and converting (double? to double) in the correct format)
        public string MyDoubleToString(double? mydouble)
        {
            if(mydouble == null)
            {
                return " ";
            }
            else
            {
                //double dd = (double)FreightShippingValue;
                double dd = (double)mydouble;
                return dd.ToString("#.00");                 //(0d), returns (.00)
            }
        }

        //TOSTRING (with delimitter)
        public string ToString(string delim)
        {
            return RecordType + delim +
                VendorCode + delim +
                LocationCode + delim +
                PurchaseOrderNo + delim +
                ((Object)ExpectedDeliveryDate ?? " ") + delim +
                VendorInvoiceNo + delim +
                InvoiceDate.ToString("MM'/'dd'/'yyyy") + delim +         //11/16/2016 	(forces the "/" separator)		
                InvoiceTotal.ToString("0.00") + delim +                  //format (two decimal places)
                (TaxGlDescription ?? " ") + delim +
                ((Object)TaxValue ?? " ") + delim +
                (FreightShippingGlDescription ?? " ") + delim +
                ((Object)FreightShippingValue ?? " ") + delim +
                Misc1GlDescription + delim +
                Misc1Value.ToString("0.00");
        }
    }
}
