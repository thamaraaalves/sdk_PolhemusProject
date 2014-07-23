namespace ListenPortG4
{
    partial class frmData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnexit = new System.Windows.Forms.Button();
            this.dgvSDF = new System.Windows.Forms.DataGridView();
            this.g4DataSet = new ListenPortG4.g4DataSet();
            this.g4DataSetBindingSource = new System.Windows.Forms.BindingSource();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.dgvmdf = new System.Windows.Forms.DataGridView();
            this.dbtrackerDataSetBindingSource = new System.Windows.Forms.BindingSource();
            this.db_trackerDataSet = new ListenPortG4.db_trackerDataSet();
            this.g4DataSetBindingSource1 = new System.Windows.Forms.BindingSource();
            this.pbUpload = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSDF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4DataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmdf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbtrackerDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.db_trackerDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4DataSetBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnexit
            // 
            this.btnexit.Location = new System.Drawing.Point(997, 13);
            this.btnexit.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnexit.Name = "btnexit";
            this.btnexit.Size = new System.Drawing.Size(82, 56);
            this.btnexit.TabIndex = 0;
            this.btnexit.Text = "exit";
            this.btnexit.UseVisualStyleBackColor = true;
            this.btnexit.Click += new System.EventHandler(this.btnexit_Click);
            // 
            // dgvSDF
            // 
            this.dgvSDF.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSDF.Location = new System.Drawing.Point(34, 79);
            this.dgvSDF.Name = "dgvSDF";
            this.dgvSDF.Size = new System.Drawing.Size(1045, 316);
            this.dgvSDF.TabIndex = 1;
            this.dgvSDF.DataSourceChanged += new System.EventHandler(this.dgvSDF_DataSourceChanged);
            this.dgvSDF.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // g4DataSet
            // 
            this.g4DataSet.DataSetName = "g4DataSet";
            this.g4DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // g4DataSetBindingSource
            // 
            this.g4DataSetBindingSource.DataSource = this.g4DataSet;
            this.g4DataSetBindingSource.Position = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Data\'s in database .sdf";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 420);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(227, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Data\'s in database in .mdf";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(261, 411);
            this.btnExport.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(244, 37);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export selected data";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dgvmdf
            // 
            this.dgvmdf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvmdf.Location = new System.Drawing.Point(34, 473);
            this.dgvmdf.Name = "dgvmdf";
            this.dgvmdf.Size = new System.Drawing.Size(1045, 423);
            this.dgvmdf.TabIndex = 5;
            // 
            // dbtrackerDataSetBindingSource
            // 
            this.dbtrackerDataSetBindingSource.DataSource = this.db_trackerDataSet;
            this.dbtrackerDataSetBindingSource.Position = 0;
            // 
            // db_trackerDataSet
            // 
            this.db_trackerDataSet.DataSetName = "db_trackerDataSet";
            this.db_trackerDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // g4DataSetBindingSource1
            // 
            this.g4DataSetBindingSource1.DataSource = this.g4DataSet;
            this.g4DataSetBindingSource1.Position = 0;
            // 
            // pbUpload
            // 
            this.pbUpload.Location = new System.Drawing.Point(513, 420);
            this.pbUpload.Name = "pbUpload";
            this.pbUpload.Size = new System.Drawing.Size(566, 23);
            this.pbUpload.TabIndex = 6;
            this.pbUpload.Click += new System.EventHandler(this.pbUpload_Click);
            // 
            // frmData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 922);
            this.Controls.Add(this.pbUpload);
            this.Controls.Add(this.dgvmdf);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvSDF);
            this.Controls.Add(this.btnexit);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmData";
            this.Load += new System.EventHandler(this.frmData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSDF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4DataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvmdf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbtrackerDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.db_trackerDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4DataSetBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnexit;
        private System.Windows.Forms.DataGridView dgvSDF;
        private System.Windows.Forms.BindingSource g4DataSetBindingSource;
        private g4DataSet g4DataSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridView dgvmdf;
        private System.Windows.Forms.BindingSource dbtrackerDataSetBindingSource;
        private db_trackerDataSet db_trackerDataSet;
        private System.Windows.Forms.BindingSource g4DataSetBindingSource1;
        private System.Windows.Forms.ProgressBar pbUpload;
    }
}