using System;
using System.IO;

namespace WPWS_Project2
{
    //DEAN JONES 
    //JUN.07, 2017 
    //PROJECT (STEVE)
    //Convert an exported file (from Steve) to a text format (that works for client)(Original Joe's)

    //TODO
    //  1.class files
    //  2.test on console...
    //  3.read TEXT file (TransactionDB.cs)
    //      read a text file 
    //      put the contents READ into an OBJECT(Transaction)
    //  4.write TEXT file 
    //      read the OBJECT (transaction)
    //      write that in the (new format) to 
    //  5.handle FILE NAMING (009_OJ0069_12345_06212017.txt)
    //      file code       009_ 
    //      storeId         OJ0069_
    //      InvoiceNo       12345_
    //      Date of Invoice or Order    (MMddyyyy)
    //  6.handle MULTIPLE FILES
    //  7. AUTOMATE FTP TRANSFER 
    //      FOLDER STRUCTURE 
    //      Z:/FTP/OJ/
    //          INBOUND (files from Compass)
    //              Completed (files that have been processed)
    //          OUTBOUND (files converted)
    //              Sent (files moved here after FTP)
    //          ...to FTP?  (how? script?)

    //  	(TransactionDB.cs)????? how to skip first line but READ THE FIRST PART (outside the while statement?
    //      HOW TO AUTOMATE THE (XLS TO CSV)(how many steps do I need?)      https://stackoverflow.com/questions/2536181/is-there-any-simple-way-to-convert-xls-file-to-csv-file-excel
    
    //  FILE ALREADY EXIST?
    //  FIX (.00) IN FILE FORMAT



    class Program
    {
        //OBJECT DECLARATION
        Transaction transaction;

        //STATUS (set states for procedure to convert files)
        public enum Status { Start, ReadTransactionFromFile, SaveTransactionToFile, MoveCsvFileToCompleted, ConfirmNewFileCreated, Finished };

        //CONSTANTS (FOLDER PATHS)(get from SETTINGS) 
        string PATH_DEFAULT;
        string PATH_COMPLETED;
        string PATH_OUTBOUND;
        string PATH_SENT;

        //VARIABLES 
        string[] files; 
        string inPath;
        string inFileName;
        string outPath;
        string outFileName;

        //LOG FILE
        string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmm");      //DateTime.Now.ToLongDateString();
        string logFileName;

        //static void Main(string[] args)
        static void Main(string[] args)
        {
            //----------------
            //TEST TRANSACTION
            Program p = new Program();
            p.SetupPaths();                             //set folder paths
            p.files = p.GetFilesInDefaultFolder();      //get all current files...

            Console.WriteLine("*************** TRANSACTION ***************");
            //           p.CreateTestTransaction();
            //----------------
            //TEST FROM FILE (to Console)
            //           p.CreateTestFromFile();
            //----------------
            //TEST (WRITE TO FILE)
            //p.CreateTestWriteToFile();
            //----------------

            //TEST GET FILES 
            //p.TestGetFilesInFolder();

            //TEST WRITE (CSV FILE)
            //p.CreateTestWriteToFile2();

            //TEST WRITE (CSV new output name)
            //p.WriteToFile(p.files[0]);                        //need to GET FILES (in folder) then SELECT ONE (to run prog with...)

            //TEST WRITE ALL FILES (in folder)
            p.WriteAllFiles(p.files);

            Console.Read();     //hold console
        }

        //************************************************************
        //************************************************************
        //CREATE TEST (WRITE MULTIPLE FILES)
        public void WriteAllFiles(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                WriteToFile(files[i]);      //write each files
            }

            //write file to console
            DumpLogToConsole();
        }

        //CREATE TEST (WRITE TO FILE)(WITH NEW FILENAME METHOD)
        public Status WriteToFile(string filename)
        {
            //keep track of status through this procedure
            Status status = Status.Start;

            try
            {
                //----------
                //CHECK FILE IS ACCESSIBLE
                FileInfo f = new FileInfo(PATH_DEFAULT + "\\" + filename);
                if (TransactionDB.IsFileLocked(f))
                {
                    //write to log                  
                    WriteToLog("--------------------------------------------");
                    WriteToLog("Filename: " + filename + ", Date run: " + dateTime);
                    WriteToLog("FILE IS NOT ACCESSIBLE");
                    WriteToLog("--------------------------------------------");
                }
                else
                {
                    //----------
                    //INPUT FILE
                    if (status == Status.Start)
                    {
                        inPath = PATH_DEFAULT;
                        //CSV file...
                        //FIND ALL FILES IN THIS FOLDER (to process ONE AT A TIME)
                        inFileName = filename;                                   //"export1.txt";     

                        //create (Transaction) object
                        transaction = new Transaction();

                        //create a (Transaction) object (from file)
                        transaction = TransactionDB.ReadTransactionFromFile(inPath, inFileName, ',');

                        //write to log                  
                        WriteToLog("--------------------------------------------");
                        WriteToLog("Filename: " + inFileName + ", Date run: " + dateTime);

                        //pass status...
                        if (transaction != null)
                        {
                            status = Status.ReadTransactionFromFile;
                            //write to log file
                            WriteToLog("\t" + status.ToString());
                        }
                    }
                    //----------
                    //OUTPUT FILE
                    if (status == Status.ReadTransactionFromFile)
                    {
                        bool savedFile = false;

                        outPath = PATH_OUTBOUND;
                        outFileName = transaction.GetTransactionFileName();         //get filename FROM (Transaction) object

                        //save to file
                        savedFile = TransactionDB.SaveTransactionToFile(outPath, outFileName, transaction);
                        //----------

                        //pass status...
                        if (savedFile)
                        {
                            status = Status.SaveTransactionToFile;
                            //write to log file
                            WriteToLog("\t" + status.ToString());
                        }
                    }
                    //--------------------
                    //CONFIRM FILE CREATED
                    if (status == Status.SaveTransactionToFile)
                    {
                        bool confirmFile = false;

                        //confirm file
                        confirmFile = TransactionDB.ConfirmNewFileCreated(outPath, outFileName);

                        //pass status...
                        if (confirmFile)
                        {
                            status = Status.ConfirmNewFileCreated;
                            //write to log file
                            WriteToLog("\t" + status.ToString());
                        }
                    }
                    //--------------------
                    //MOVE CSV FILE (TO COMPLETED FOLDER)
                    if (status == Status.ConfirmNewFileCreated)
                    {
                        bool moveCsv = false;

                        //move CSV file
                        moveCsv = TransactionDB.MoveCsvFileToCompleted(PATH_DEFAULT, PATH_COMPLETED, inFileName);

                        //pass status...
                        if (moveCsv)
                        {
                            status = Status.MoveCsvFileToCompleted;
                            //write to log file
                            WriteToLog("\t" + status.ToString());
                        }
                    }
                    //--------------------
                    //FINISH STATUS
                    if (status == Status.MoveCsvFileToCompleted)
                    {
                        //then finished...
                        status = Status.Finished;
                        //write to log file
                        WriteToLog("\t" + status.ToString());
                        WriteToLog("--------------------------------------------");
                    }
                }              
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message + "\n" + ex2.StackTrace);
            }

            return status;
        }
        //************************************************************
        //************************************************************

        //WRITE LOG FILE
        public void WriteToLog(string message)
        {
            //set log file path
            logFileName = PATH_COMPLETED + "\\" + dateTime + "_" + "log.txt";

            try
            {
                using (StreamWriter w = File.AppendText(logFileName))
                {
                    Logger.Log(message, w);
                }
            }
            catch (FileNotFoundException ex2)
            {
                Console.WriteLine("Error: DumpLogToConsole, " + ex2.Message + "\n" + ex2.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: DumpLogToConsole, " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        //DUMP LOG TO CONSOLE 
        public void DumpLogToConsole()
        {
            //set log file path
            logFileName = PATH_COMPLETED + "\\" + dateTime + "_" + "log.txt";

            try
            {
                using (StreamReader r = File.OpenText(logFileName))
                {
                    Logger.DumpLog(r);
                }              
            }
            catch(FileNotFoundException ex2)
            {
                Console.WriteLine("Error: DumpLogToConsole, " + ex2.Message + "\n" + ex2.StackTrace);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: DumpLogToConsole, " + ex.Message + "\n" + ex.StackTrace);
            }

        }

        //SET FOLDER PATHS 
        public void SetupPaths()
        {
            //SETTINGS OBJECT (to retreive path values)
            WPWS_Project2.Properties.Settings settings = new Properties.Settings();

            //default path (will be... "C:\\FTP\\OJ")(will be Z:DRIVE eventually)
            PATH_DEFAULT = settings.PATH_DEFAULT;
            PATH_COMPLETED = settings.PATH_COMPLETED;
            PATH_OUTBOUND = settings.PATH_OUTBOUND;
            PATH_SENT = settings.PATH_OUTBOUND;
        }

        //GET (ALL FILES) IN DEFAULT FOLDER 
        public string[] GetFilesInDefaultFolder()
        {
            TransactionDB transactionDB = new TransactionDB();
            string[] files = transactionDB.GetFilesInDefaultFolder("*.csv");          

            return files;
        }

        //TEST RETURN ALL FILES IN FOLDER 
        public void TestGetFilesInFolder()
        {
            TransactionDB transactionDB = new TransactionDB();
            string[] files = transactionDB.GetFilesInDefaultFolder("*.csv");

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);
            }
        }

        ////GET (ALL FILES) IN DEFAULT FOLDER 
        //public string[] GetFilesInDefaultFolder()
        //{
        //    Transaction transaction = new Transaction();
        //    string[] files = transaction.GetFilesInDefaultFolder("*.csv");

        //    return files;
        //}

        ////TEST RETURN ALL FILES IN FOLDER 
        //public void TestGetFilesInFolder()
        //{
        //    Transaction transaction = new Transaction();
        //    string[] files = transaction.GetFilesInDefaultFolder("*.csv");

        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        Console.WriteLine(files[i]);
        //    }
        //}


        //CREATE A TEST (TO WRITE TO FILE)
        public void CreateTestWriteToFile2()
        {
            //----------
            //INPUT FILE
            string inPath = PATH_DEFAULT;
            string inFileName = "Export506.csv";

            //create (Transaction) object
            Transaction transaction = new Transaction();

            //create a (Transaction) object (from file)
            transaction = TransactionDB.ReadTransactionFromFile(inPath, inFileName, ',');
            //----------
            //OUTPUT FILE
            //string outPath = PATH_DEFAULT + "\\" + PATH_COMPLETED;
            string outPath = PATH_OUTBOUND;
            string outFileName = "output1.txt";

            //save to file
            TransactionDB.SaveTransactionToFile(outPath, outFileName, transaction);
            //----------

            Console.WriteLine("complete...");
        }

        //CREATE A TEST (TO WRITE TO FILE)
        public void CreateTestWriteToFile()
        {
            //----------
            //INPUT FILE
            string inPath = "C:\\_txttofile";
            string inFileName = "export1.txt";

            Transaction transaction = new Transaction();
            //create a (Transaction) object (from file)
            transaction = TransactionDB.ReadTransactionFromFile(inPath, inFileName, '\t');
            //----------
            //OUTPUT FILE
            string outPath = "C:\\_txttofile";
            string outFileName = "output1.txt";
            TransactionDB.SaveTransactionToFile(outPath, outFileName, transaction);
            //----------

        }

        //CREATE A TEST FROM FILE 
        public void CreateTestFromFile()
        {
            string path = "C:\\_txttofile";
            string filename = "export1.txt";

            Transaction transaction = new Transaction();
            transaction = TransactionDB.ReadTransactionFromFile(path, filename, '\t');

            Console.WriteLine("---testfromfile---");
            Console.WriteLine(transaction.ToString());
            Console.WriteLine("---testfromfile---");

        }

        //CREATE A TEST TRANSACTION (object)
        public void CreateTestTransaction()
        {          
            //HEADER INFO
            TransactionHeader header = new TransactionHeader();
            header.RecordType = "H";
            header.VendorCode = "WILPAR";
            header.LocationCode = "OJ0069";
            header.PurchaseOrderNo = "PO1059";
            header.ExpectedDeliveryDate = null;
            header.VendorInvoiceNo = 87589;
            header.InvoiceDate = new DateTime(2015, 11, 14);        //return dateTimeValue.ToString("MM-dd-yyyy");
            header.InvoiceTotal = 4014.8;
            header.TaxGlDescription = null;
            header.TaxValue = null;
            header.FreightShippingGlDescription = null;
            header.FreightShippingValue = null;
            header.Misc1GlDescription = "Bottle Deposits";
            header.Misc1Value = 56.8;

            //DETAIL INFO
            TransactionDetail detail1 = new TransactionDetail();
            detail1.RecordType = "D";
            detail1.VendorProductNo = 759064;
            detail1.CatchWeightIndicator = "N";
            detail1.InvoiceQuantity = 1;
            detail1.InvoicePrice = 251.48;
            detail1.ExtendedValue = 264.05;
            detail1.TaxValue = 12.57;

            //DETAIL INFO
            TransactionDetail detail2 = new TransactionDetail();
            detail2.RecordType = "D";
            detail2.VendorProductNo = 721900;
            detail2.CatchWeightIndicator = "N";
            detail2.InvoiceQuantity = 36;
            detail2.InvoicePrice = 9.52;
            detail2.ExtendedValue = 359.86;
            detail2.TaxValue = 17.14;

            //TRANSACTION (header and details)
            transaction = new Transaction();
            transaction.Header = header;
            transaction.Details.Add(detail1);
            transaction.Details.Add(detail2);
            //transaction.Details[0] = detail1;
            //transaction.Details[1] = detail2;

            //-----------
            //PRINT HEADER
            //Console.WriteLine(transaction.Header);
            ////Console.WriteLine(header);

            ////PRINT DETAILS
            //for (int i = 0; i < transaction.Details.Count; i++)
            //{
            //    Console.WriteLine(transaction.Details[i]);
            //}
            ////Console.WriteLine(transaction.Details[0]);
            ////Console.WriteLine(transaction.Details[1]);
            //-----------

            //************
            //PRINT from TRANSACTION (TOSTRING)
            Console.WriteLine(transaction.ToString());
            //************

            Console.WriteLine("************ END OF TRANSACTION ***********");

            Console.WriteLine("\n\nthere are {0} rows of details", transaction.Details.Count);

            
        }
    }
}
