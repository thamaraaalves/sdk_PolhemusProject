using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace ListenPortG4
{
    public partial class frmData : Form
    {
        public frmData()
        {
            InitializeComponent();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void frmData_Load(object sender, EventArgs e)
        {                      
            dgvSDF.DataSource = ComandosBD.SelectSDF("select * from tblDataG4");          
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
           // transfer data from .sdf to .mdf database
        
            try
            {
                string[] sdata = null;
              
                //perfect - it works! :)
                DataTable dt = ComandosBD.SelectSDF(@"SELECT sData FROM tblDataG4");
                string data = null;
                foreach (DataRow row in dt.Rows)
                {                 
                    foreach (object item in row.ItemArray)
                    {
                        data += item.ToString();
                    }    
                }

                int count = dt.Rows.Count; //size database sdf
                int current = 0;

                sdata = data.Split(' ');
                int aux = 1;
                int id = -1; //set up the nId

                foreach (string word in sdata)
                {                  
                   //insert in database .mdf
                    //if word to different of white space
                    if (word != "")
                    {                                               
                        if (id==-1 && aux==1)
                        {                            
                            id = Convert.ToInt32(ComandosBD.UpdateDeleteInsert("INSERT INTO tblData(s"+aux+") VALUES(@s"+aux+"); SELECT SCOPE_IDENTITY();", ComandosBD.RetornoBD.Scalar,
                                new SqlParameter[]{
                               new SqlParameter("@s"+aux, word)
                            }));          
                        }

                        //using the same id for another insertions
                        if (word != "" && id != -1)
                        {
                            //i have problem here
                            ComandosBD.UpdateDeleteInsert("update tblData set s"+aux+"=@s"+aux+ " " + "where nId=@id", ComandosBD.RetornoBD.NonQuery,
                                 new SqlParameter[]{                                 
                                    new SqlParameter("@s"+aux, word),
                                    new SqlParameter("@id",id)
                            });
                        }

                        aux++;       

                        if (aux == 21)
                        {
                            aux = 1;
                            id = -1;
                        }                                                          
                    }

                    current++;
                    pbUpload.Value = current/count;
                    Application.DoEvents();
              }
                //the end test , trying the separated all datas     
                //if finish the insertions - verify the data's
                MessageBox.Show("Finish the transfer data to database .mdf");
                //load the data from mdf
                dgvmdf.DataSource = ComandosBD.Select("SELECT * FROM tblData");
                
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("ERROR" + ex);
            }                                               
        }

        private void dgvSDF_DataSourceChanged(object sender, EventArgs e)
        {
            //autosize the all columns
            dgvSDF.Columns["sData"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;            
           
        }

        private void pbUpload_Click(object sender, EventArgs e)
        {

        }
    }
}
