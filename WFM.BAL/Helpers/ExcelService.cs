using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM.BAL.Helpers
{
    public class ExcelService
    {

        public DataTable ImportExceltoDatabase(string strFilePath, string connString)
        {

            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet 1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch (Exception)
            {
                dt = null;
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }

        public DataTable ImportExceltoDatabase(string strFilePath, string connString, string sheetName)
        {

            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheetName + "$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch (Exception)
            {
                dt = null;
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }

        public DataTable ImportExceltoDatabaseFromAzureBlob(string strFilePath, string connString)
        {

            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet 1$]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch (Exception)
            {
                dt = null;
            }
            finally
            {
                oledbConn.Close();
            }
            return dt;
        }
    }
}
