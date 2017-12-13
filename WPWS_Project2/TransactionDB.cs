using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPWS_Project2
{
    public class TransactionDB
    {
        //METHOD TO READ FROM FILE
        public static Transaction ReadTransactionFromFile(string path, string filename, char delim)
        {
            Transaction transaction = new Transaction();
            string row;                                     //one row of text (text file)
            string[] columns;                               //array of text (after delimitted)(splitted up)
            int i = 0;                                      //counter (keep track of which row)
            string fullpath = path + "\\" + filename;              //full path (from file to read)      (path + "\\" + filename)

            try
            {
                using (StreamReader reader = new StreamReader(
                    new FileStream(fullpath, FileMode.Open, FileAccess.Read)))
                {
                    //ROW DATA
                    //H	WILPAR	OJ0069	PO10967	121346	2016-11-21	2865.26	GST/HST	135.36	Fuel/Delivery/Freight	.00	Bottle Deposits	22.50	D	100594	N	24	14.57	349.68	17.48
                    //HEADER DATA (H)
                    //H	WILPAR	OJ0069	PO10967	121346	2016-11-21	2865.26	GST/HST	135.36	Fuel/Delivery/Freight	.00	Bottle Deposits	22.50
                    //DETAILS DATA (D)
                    //D	100594	N	24	14.57	349.68	17.48

                    row = reader.ReadLine();                    //skip first line (headers don't need)

                    //*********************************
                    //*********************************
                    //HEADER INFO
                    row = reader.ReadLine();                    //read first line (read HEADER info)
                    columns = row.Split(delim);                  //TAB delimitter            columns = row.Split('\t'); 
                                                                 //????? how to skip first line but READ THE FIRST PART (outside the while statement?

                    //---------------------
                    transaction.Header.RecordType = columns[0];         //H	
                    transaction.Header.VendorCode = columns[1];         //WILPAR	
                    transaction.Header.LocationCode = columns[2];       //OJ0069	
                    transaction.Header.PurchaseOrderNo = columns[3];    //PO10967	
                                                                        //need column(Expected Delivery Date)         

                    //???????????????????????????????????
                    //EXPECTED DELIVERY DATE (not in CSV?)
                    //transaction.Header.ExpectedDeliveryDate = DateTime.ParseExact(columns[5], "MM/dd/yyyy", new CultureInfo("en-CA"));  //convert to DATETIME                  
                    //???????????????????????????????????

                    transaction.Header.VendorInvoiceNo = Convert.ToInt32(columns[4]);   //121346  
                                                                                        //     transaction.Header.InvoiceDate = DateTime.ParseExact(columns[5], "MM/dd/yyyy", new CultureInfo("en-CA"));   //2016-11-21

                    //-----------
                    //split up date, to convert to (DateTime) object
                    string date = columns[5];
                    string[] datesplit = date.Split('-');

                    transaction.Header.InvoiceDate = new DateTime(Convert.ToInt32(datesplit[0]), Convert.ToInt32(datesplit[1]), Convert.ToInt32(datesplit[2]));
                    //-----------

                    transaction.Header.InvoiceTotal = Convert.ToDouble(columns[6]);                                             //2865.26 

                    //------------
                    transaction.Header.TaxGlDescription = columns[7];                                                           //GST/HST  
                    //transaction.Header.TaxValue = Convert.ToDouble(columns[8]);                                               //135.36  
                    if (columns[8] == "")
                    {
                        //transaction.Header.TaxValue = 0d;
                        transaction.Header.TaxValue = null;
                    }
                    else
                    {
                        transaction.Header.TaxValue = Convert.ToDouble(columns[8]);
                    }
                    //------------

                    //------------
                    transaction.Header.FreightShippingGlDescription = columns[9];                                               //Fuel/Delivery/Freight	
                    //transaction.Header.FreightShippingValue = Convert.ToDouble(columns[10]);                                  //.00	
                    if (columns[10] == "")
                    {
                        //transaction.Header.FreightShippingValue = 0d;
                        transaction.Header.FreightShippingValue = null;
                    }
                    else
                    {
                        transaction.Header.FreightShippingValue = Convert.ToDouble(columns[10]);
                    }
                    //------------

                    transaction.Header.Misc1GlDescription = columns[11];                                                        //Bottle Deposits	
                    transaction.Header.Misc1Value = Convert.ToDouble(columns[12]);                                              //22.50 
                                                                                                                                //---------------------

                    //i++;                                        //increment count (past header)

                    //***************************
                    //DETAILS
                    //step inside each row to pull out (Details) data
                    while (reader.Peek() != -1)                 //test to make sure file is good
                    {
                        //-----------------------
                        //FILL UP (Detail object)
                        TransactionDetail mydetail = new TransactionDetail();           //create new (TransactionDetail) object

                        mydetail.RecordType = columns[13];
                        mydetail.VendorProductNo = Convert.ToInt32(columns[14]);        //convert to INT32
                        mydetail.CatchWeightIndicator = columns[15];
                        mydetail.InvoiceQuantity = Convert.ToInt32(columns[16]);        //convert to INT32
                        mydetail.InvoicePrice = Convert.ToDouble(columns[17]);          //convert to DOUBLE
                        mydetail.ExtendedValue = Convert.ToDouble(columns[18]);         //convert to DOUBLE
                        mydetail.TaxValue = Convert.ToDouble(columns[19]);              //convert to DOUBLE

                        transaction.Details.Add(mydetail);                              //ADD object to (Transaction) object
                                                                                        //-----------------------

                        //
                        //IndexOutOfRangeException
                        //Exception

                        row = reader.ReadLine();                //gets ONE ROW of data
                        columns = row.Split(delim);              //TAB delimitter                columns = row.Split('\t');      
                        i++;                                    //increment count (for next row of Details object)
                    }
                    //*********************************
                    //*********************************
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return transaction;
        }

        //METHOD TO SAVE TO FILE
        public static bool SaveTransactionToFile(string path, string filename, Transaction transaction)
        {
            bool result = false;    //default to false
            string fullpath = path + "\\" + filename;              //full path (from file to read)

            try
            {
                using (StreamWriter writer = new StreamWriter(
                    new FileStream(fullpath, FileMode.Create, FileAccess.Write)))
                {
                    writer.Write(transaction.ToString());       //write to file
                    result = true;                              //return it was successful
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        //METHOD MOVE (CSV) FILES TO (Completed) FOLDER 
        //run this when the files have been processed
        public static bool MoveCsvFileToCompleted(string pathFrom, string pathTo, string filename)
        {
            bool result = false;

            //source file 
            string sourceFileName = pathFrom + "\\" + filename;
            string destinationFileName = pathTo + "\\" + filename;

            try
            {
                //if folder doesn't exist 
                if (!Directory.Exists(pathFrom))
                {
                    Console.WriteLine("The folder that the file is moving FROM is incorrect.");
                }
                //if folder doesn't exist 
                if (!Directory.Exists(pathTo))
                {
                    Console.WriteLine("The folder that the file is moving TO is incorrect.");
                }
                //if the file doesn't exist
                if (!File.Exists(sourceFileName))
                {
                    Console.WriteLine("The file that is moving is incorrect.");
                }
                //already confirmed that (folder to/from and file exist)
                else
                {
                    //move file (from INBOUND to Completed folder)
                    File.Move(sourceFileName, destinationFileName);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        //METHOD CONFIRM (that new file was generated)
        public static bool ConfirmNewFileCreated(string path, string filename)
        {
            bool result = false;
            string fullpath = path + "\\" + filename;

            try
            {
                //if folder doesn't exist 
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("The folder for new file is incorrect.");
                }
                //if the file doesn't exist
                if (!File.Exists(fullpath))
                {
                    Console.WriteLine("The new file is incorrect.");
                }
                else
                {
                    //confirm file exists 
                    result = File.Exists(fullpath);      //true
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        //METHOD (GET ALL FILES IN DIRECTORY)
        public string[] GetFilesInDefaultFolder(string extension)
        {
            //SETTINGS OBJECT (to retreive path values)
            WPWS_Project2.Properties.Settings settings = new Properties.Settings();

            //default path (will be... "C:\\FTP\\OJ")(will be Z:DRIVE eventually)
            string PATH_DEFAULT = settings.PATH_DEFAULT;

            // Only get files that begin with the letter "c."
            string[] files = Directory.GetFiles(PATH_DEFAULT, extension);       //returns PATH + FILENAME (@"C:\_txttofile", "*.*")

            //only return file name
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }

            return files;

        }

        //METHOD FILE ACCESSIBLE (make sure file isn't open in another program)
        //  https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }


    }   //END CLASS
}   //END NAMESPACE
