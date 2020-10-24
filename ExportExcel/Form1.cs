using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExportExcel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DbCOM.OraDB ora = new DbCOM.OraDB();

            // DataTable dt = ora.SelectProcedure("PKG_RPT_HRM_CONTRACT.SELECT_RENEWAL_CONTRACT_WORKER");
            DataTable dt = Master_Select("");
            MessageBox.Show(dt.Rows.Count.ToString());

        }


        private DataTable Master_Select(string argType)
        {
            DbCOM.OraDB MyOraDB = new DbCOM.OraDB();

            MyOraDB.ReDim_Parameter(4);
            MyOraDB.Process_Name = "PKG_RPT_HRM_CONTRACT.SELECT_RENEWAL_CONTRACT_WORKER";

            MyOraDB.Parameter_Name[0] = "ARG_YMD_FRM";
            MyOraDB.Parameter_Name[1] = "ARG_YMD_TO";
            MyOraDB.Parameter_Name[2] = "ARG_DEPCODE";
            MyOraDB.Parameter_Name[3] = "CV_1";

            MyOraDB.Parameter_Type[0] = (int)DbCOM.OracleType.VarChar;
            MyOraDB.Parameter_Type[1] = (int)DbCOM.OracleType.VarChar;
            MyOraDB.Parameter_Type[2] = (int)DbCOM.OracleType.VarChar;
            MyOraDB.Parameter_Type[3] = (int)DbCOM.OracleType.Cursor;

            MyOraDB.Parameter_Values[0] = "";
            MyOraDB.Parameter_Values[1] = "";
            MyOraDB.Parameter_Values[2] = "";
            MyOraDB.Parameter_Values[3] = "";

            MyOraDB.Add_Select_Parameter(true);
            DataSet retDS = MyOraDB.Exe_Select_Procedure();
            if (retDS == null) return null;

            return retDS.Tables[0];


            //command.Parameters.Add("ARG_YMD_FRM", OracleDbType.Varchar2).Value = "";
            //command.Parameters.Add("ARG_YMD_TO", OracleDbType.Varchar2).Value = "";
            //command.Parameters.Add("ARG_DEPCODE", OracleDbType.Varchar2).Value = "";
            //command.Parameters.Add("CV_1", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        }
    }
}
