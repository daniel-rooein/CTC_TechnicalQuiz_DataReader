﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CTCDatabaseUpdater.Models;
using CTCDatabaseUpdater.DataAccessLayer;

namespace CTCDatabaseUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFilesPath;

            if (ConfigurationManager.AppSettings["AbsoluteDataFilesPath"] == "" && ConfigurationManager.AppSettings["RelativeDataFilesPath"] == "")
            {
                throw new Exception("There is no absolute or relateive data file paths defined in Config file!");
            }
            else if(ConfigurationManager.AppSettings["AbsoluteDataFilesPath"] == "")
            {
                dataFilesPath = AppDomain.CurrentDomain.BaseDirectory + "/../../" + ConfigurationManager.AppSettings["RelativeDataFilesPath"];
            }
            else
            {
                dataFilesPath = ConfigurationManager.AppSettings["AbsoluteDataFilesPath"];
            }

            DataReader reader = new DataReader(dataFilesPath);
            List<string> dataFiles = reader.GetValidFileNames(ConfigurationManager.AppSettings["ValidDataPrefixes"],
                                                              ConfigurationManager.AppSettings["DataFileExtension"]);


            List<DataFileModel> validRecords = new List<DataFileModel>();
            foreach (var file in dataFiles)
            {
                validRecords = reader.ReadFile(dataFilesPath + "/" + file);
            }

            DAL dal = new DAL();
            // After validation I can insert the data into the database using my data access layer

            dal.InsertIntoEmployeesTable(validRecords);
            
        }
    }
}
