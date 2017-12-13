using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPWS_Project2
{
    //DEAN JONES 
    //JUN.07, 2017 
    //TRANSACTION CLASS 
    //combines info from (transaction header) and makes a list of (transaction details) to make one transaction object

        //SUPPORTING FILES 
        //TransactionHeader.cs CLASS 
        //TransactionDetails.cs CLASS

    public class Transaction
    {
        //CONSTANTS (FOLDER PATHS)(get from SETTINGS) 
        string PATH_DEFAULT;

        //PROPERTIES
        public TransactionHeader Header { get; set; }
        public List<TransactionDetail> Details { get; set; }
        //PROPERTIES (FILE NAME)
        private string filePrefix;
        private string filePurchaseOrderNo;
        private string fileExtension;
        private string fileName;

        //GETTERS AND SETTERS (FILE NAME)
        //string purchaseOrderNo = this.Header.PurchaseOrderNo;
        public string FilePrefix
        {
            get { return filePrefix; }
            set { filePrefix = value; }
        }
        public string FilePurchaseOrderNo
        {
            get { return filePurchaseOrderNo; }
            set { filePurchaseOrderNo = this.Header.PurchaseOrderNo; }      //gets the PO# from the (header object)
        }
        public string FileExtension
        {
            get { return fileExtension; }
            set { fileExtension = value; }
        }
        public string FileName
        {
            get { return filePrefix + filePurchaseOrderNo + fileExtension; }
            set { fileName = value; }
        }
        
        //CONSTRUCTOR 
        public Transaction()
        {
            this.Header = new TransactionHeader();              //need to INSTANTIATE the (TransactionHeader) object here
            this.Details = new List<TransactionDetail>();       //need to INSTANTIATE the list here            
        }

        //TOSTRING OVERRIDE
        //for output to the TEXT FILE
        public override string ToString()
        {
            //build a string
            StringBuilder sb = new StringBuilder();
            //add header info
            sb.Append(Header);  

            //add details
            for (int i = 0; i < Details.Count; i++)
            {
                sb.Append("\n" + Details[i]);
            }

            return sb.ToString();
        }

        //METHOD (FILE NAME FOR SAVING)(return FILENAME string)
        public string GetTransactionFileName()
        {
            //FORMAT
            //009_OJ0069_12345_06062017.txt
            //  file code (009_)
            //  LocationCode(StoreId) (OJ0069_)
            //  Invoice# (12345_)
            //  Date of invoice or order (MMddyyyy)

            string fileCode = "009";
            string storeId = this.Header.LocationCode;                          //OJ0069
            string invoiceNo = this.Header.VendorInvoiceNo.ToString();          //12345
            string invoiceDate = this.Header.InvoiceDate.ToString("MMddyyyy");

            string fullFileName = fileCode + "_" + storeId + "_" + invoiceNo + "_" + invoiceDate + ".txt";

            return fullFileName;
        }

        //METHOD (GET ALL FILES IN DIRECTORY)
        //public string[] GetFilesInDefaultFolder(string extension)
        //{
        //    //SETTINGS OBJECT (to retreive path values)
        //    WPWS_Project2.Properties.Settings settings = new Properties.Settings();

        //    //default path (will be... "C:\\FTP\\OJ")(will be Z:DRIVE eventually)
        //    PATH_DEFAULT = settings.PATH_DEFAULT;

        //    // Only get files that begin with the letter "c."
        //    string[] files = Directory.GetFiles(PATH_DEFAULT, extension);       //returns PATH + FILENAME (@"C:\_txttofile", "*.*")

        //    //only return file name
        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        files[i] = Path.GetFileName(files[i]);
        //    }
                
        //    return files;

        //}

    }
}
