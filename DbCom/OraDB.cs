using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Collections;

namespace DbCOM
{
    public enum OracleType
    {
        BFile = 1,
        Blob = 2,
        Byte = 23,
        Char = 3,       
        Clob = 4,   
        //Date
        DateTime = 6,
        Float = 29,
        Double = 30,

        IntervalDayToSecond = 7,
        IntervalYearToMonth = 8,
        LongRaw = 9,
        LongVarChar = 10,
        NChar = 11,
        NClob = 12,
        Number = 13,
        NVarChar = 14,
        Raw = 15,
        RowId = 16,
        Timestamp = 18,
        TimestampLocal = 19,
        TimestampWithTZ = 20,
        Cursor = 121,
        VarChar = 126,
        
        UInt16 = 24,
        UInt32 = 25,
        SByte = 26,
        Int16 = 27,
        Int32 = 28,
        
        
    }

    public class OraDB
    {

        



        private DataSet DS_Select = new DataSet("Parameter DataSet");

        public string[] Parameter_Values { get; set; }
        public int[] Parameter_Type { get; set; }
        public string[] Parameter_Name { get; set; }
        public string Process_Name { get; set; }

        public void ReDim_Parameter(int arg_count)
        {
            Parameter_Name = new string[arg_count];
            Parameter_Type = new int[arg_count];
            Parameter_Values = new string[arg_count];
        }

        private void Clear_Select_DataSet()
        {
            DS_Select.Reset();
        }


        public bool Add_Select_Parameter(bool AfterClear) 
        {
            DataTable DT_Select = new DataTable(Process_Name);
            DataColumn[] dc = new DataColumn[3];

            try
            {
                dc[0] = new DataColumn("Parameter_Name", Type.GetType("System.String"));
                dc[1] = new DataColumn("Parameter_Type", Type.GetType("System.Int32"));
                dc[2] = new DataColumn("Parameter_Value", Type.GetType("System.String"));
                DT_Select.Columns.AddRange(dc);

                for (int i = 0; i < Parameter_Name.Length; i++)
                {
                    DataRow newRow = DT_Select.NewRow();

                    newRow["Parameter_Name"] = Parameter_Name[i];
                    newRow["Parameter_Type"] = (int)Parameter_Type[i];
                    newRow["Parameter_Value"] = (Parameter_Values[i] == null) ? "" : Parameter_Values[i];
                    DT_Select.Rows.Add(newRow);

                }
                if (AfterClear) this.Clear_Select_DataSet();
                DS_Select.Tables.Add(DT_Select);
                return true;
            }
            catch
            {                
                return false;
            }


        }


        public DataSet Exe_Select_Procedure()
        {
            try
            {
                Hashtable hash = new Hashtable();
               // foreach (OracleType val in Enum.GetValues(typeof(OracleType)))
               // {
                    hash.Add(126, OracleDbType.Varchar2);
             //   }

                using (OracleConnection connection = new OracleConnection())
                {
                    connection.ConnectionString = "user id=HUBICVJ;password=HUBICVJ;data source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 211.54.128.21)(PORT = 1521))) (CONNECT_DATA = (SID = HUBICVJ)(SERVER = DEDICATED))); pooling=true;";
                    connection.Open();

                    OracleCommand command = new OracleCommand(Process_Name, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < Parameter_Name.Length; i++)
                    {
                        if (Parameter_Type[i] == 121)
                        {
                            command.Parameters.Add(Parameter_Name[i], OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        }
                        else
                        {
                            command.Parameters.Add(Parameter_Name[i], (OracleDbType)Parameter_Type[i]).Value = Parameter_Values[i];
                        }

                    }

                    //command.Parameters.Add("ARG_YMD_FRM", OracleDbType.Varchar2).Value = "";
                    //command.Parameters.Add("ARG_YMD_TO", OracleDbType.Varchar2).Value = "";
                    //command.Parameters.Add("ARG_DEPCODE", OracleDbType.Varchar2).Value = "";
                    //command.Parameters.Add("CV_1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (DataSet dt = new DataSet(Process_Name))
                    {
                        new OracleDataAdapter(command).Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }


            public DataTable SelectProcedure(string Arg_Procedure)
        {

            try
            {
                // OracleDbType ora = GetOracleDbType("Varchar2");
                // List<string> lst = new List<string> { "", "" };

                using (OracleConnection connection = new OracleConnection())
                {
                    connection.ConnectionString = "user id=HUBICVJ;password=HUBICVJ;data source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 211.54.128.21)(PORT = 1521))) (CONNECT_DATA = (SID = HUBICVJ)(SERVER = DEDICATED))); pooling=true;";
                    connection.Open();
                    OracleCommand command = new OracleCommand(Arg_Procedure, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("ARG_YMD_FRM", OracleDbType.Varchar2).Value = "";
                    command.Parameters.Add("ARG_YMD_TO", OracleDbType.Varchar2).Value = "";
                    command.Parameters.Add("ARG_DEPCODE", OracleDbType.Varchar2).Value = "";
                    command.Parameters.Add("CV_1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (DataTable dt = new DataTable())
                    {
                        new OracleDataAdapter(command).Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        public OracleDbType GetOracleDbType(string argDBType)
        {
            return (OracleDbType)System.Enum.Parse(typeof(OracleDbType), argDBType);
        }
    }
}
