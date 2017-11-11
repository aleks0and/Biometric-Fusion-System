using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace ImageSaveInDataBase
{
	/// <summary>
	/// Summary description for frmImageInput.
	/// </summary>
	public class frmImageInput : System.Windows.Forms.Form
	{
		
		private System.Windows.Forms.Label lblID;
		private System.Windows.Forms.TextBox editID;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox editName;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Data.SqlClient.SqlCommand sqlCommand1;
		private System.Data.SqlClient.SqlConnection sqlConnection1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private long m_lImageFileLength=0;
		private byte[] m_barrImg;
        private byte[] _fileData;
        private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Label label1;
        private Button button1;
        private Button button2;
        private Button button3;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

		public frmImageInput()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblID = new System.Windows.Forms.Label();
            this.editID = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.editName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sqlCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.Location = new System.Drawing.Point(16, 40);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(100, 23);
            this.lblID.TabIndex = 5;
            this.lblID.Text = "Id";
            // 
            // editID
            // 
            this.editID.Location = new System.Drawing.Point(128, 40);
            this.editID.Name = "editID";
            this.editID.Size = new System.Drawing.Size(100, 20);
            this.editID.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(8, 72);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 23);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "Name";
            // 
            // editName
            // 
            this.editName.Location = new System.Drawing.Point(128, 72);
            this.editName.Name = "editName";
            this.editName.Size = new System.Drawing.Size(100, 20);
            this.editName.TabIndex = 8;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(248, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 112);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(144, 104);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "All Files|*.*|BMP Files|*.bmp|JPG Files|*.jpg";
            // 
            // sqlCommand1
            // 
            this.sqlCommand1.Connection = this.sqlConnection1;
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "Server=localhost\\MSSQLSERVER;Data Source=DESKTOP-TVN9FHA;Initial Catalog = Biomet" +
    "ricDB; User ID = sa; Password = 1234qwerAS";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(280, 136);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(16, 104);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(252, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "To Load an image from Database  just provide ID value  and press Load";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Load Wav";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(144, 133);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Save wav";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 133);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(122, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Load Wav from DB";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmImageInput
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(432, 230);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.editID);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.editName);
            this.Name = "frmImageInput";
            this.Load += new System.EventHandler(this.frmImageInput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		
		private void UploadWavFile()
        {
            try
            {
                this.openFileDialog1.ShowDialog(this);
                string fileName = this.openFileDialog1.FileName;
                FileInfo WavFile = new FileInfo(fileName);
                this.m_lImageFileLength = WavFile.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                _fileData = new byte[Convert.ToInt32(this.m_lImageFileLength)];
                int iBytesRead = fs.Read(_fileData, 0, Convert.ToInt32(this.m_lImageFileLength));
                fs.Close();
                Debug.WriteLine("Wav file loaded");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

		private void pictureBox_Click(object sender, System.EventArgs e)
		{

			LoadImage();
		}

		protected void LoadImage()
		{

			try
			{
				this.openFileDialog1.ShowDialog(this);
				string strFn=this.openFileDialog1.FileName;
				this.pictureBox1.Image=Image.FromFile(strFn);
				FileInfo fiImage=new FileInfo(strFn);
				this.m_lImageFileLength=fiImage.Length;
				FileStream fs=new FileStream(strFn,FileMode.Open,FileAccess.Read,FileShare.Read);
				m_barrImg=new byte[Convert.ToInt32(this.m_lImageFileLength)];
				int iBytesRead=fs.Read(m_barrImg,0,Convert.ToInt32(this.m_lImageFileLength));
				fs.Close();

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
        [STAThread]
		public static void Main()
		{
			Application.Run(new frmImageInput());
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				

				this.sqlConnection1.Open();
				if (sqlCommand1.Parameters.Count ==0 )
				{
					this.sqlCommand1.CommandText="INSERT INTO FaceBiometric (ID,Picture) values(@ID,@Picture)";
					this.sqlCommand1.Parameters.Add("@ID",System.Data.SqlDbType.Int,4);
					//this.sqlCommand1.Parameters.Add("@Name",System.Data.SqlDbType.VarChar,50);
					this.sqlCommand1.Parameters.Add("@Picture",System.Data.SqlDbType.Image);
				}
				this.sqlCommand1.Parameters["@ID"].Value=this.editID.Text;
				//this.sqlCommand1.Parameters["@Name"].Value=this.editName.Text;
				this.sqlCommand1.Parameters["@Picture"].Value=this.m_barrImg;



			int iresult=this.sqlCommand1.ExecuteNonQuery();
				MessageBox.Show(Convert.ToString(iresult));								

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);

			}
			finally
			{
				this.sqlConnection1.Close();

			}
		}

		private void btnViewReport_Click(object sender, System.EventArgs e)
		{
			ImageSaveInDataBase.frmImageReport fir=new frmImageReport();
			fir.ShowDialog(this);
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
				LoadImage();
		}

		private void btnLoad_Click(object sender, System.EventArgs e)
		{
			try
			{
				SqlCommand cmdSelect=new SqlCommand("select Picture from dbo.FaceBiometric where ID=@ID",this.sqlConnection1);
				cmdSelect.Parameters.Add("@ID",SqlDbType.Int,4);
				cmdSelect.Parameters["@ID"].Value=this.editID.Text;

				this.sqlConnection1.Open();
				byte[] barrImg=(byte[])cmdSelect.ExecuteScalar();
				string strfn=Convert.ToString(DateTime.Now.ToFileTime());
				FileStream fs=new FileStream(strfn,FileMode.CreateNew,FileAccess.Write);
				fs.Write(barrImg,0,barrImg.Length);
				fs.Flush();
				fs.Close();

				pictureBox1.Image=Image.FromFile(strfn);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);

			}
			finally
			{
				this.sqlConnection1.Close();

			}
		}

		private void frmImageInput_Load(object sender, System.EventArgs e)
		{

		}

        private void button1_Click(object sender, EventArgs e)
        {
            UploadWavFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.sqlConnection1.Open();
                if (sqlCommand1.Parameters.Count == 0)
                {
                    this.sqlCommand1.CommandText = "INSERT INTO VoiceBiometric (ID,Speech) values(@ID,@Speech)";
                    this.sqlCommand1.Parameters.Add("@ID", System.Data.SqlDbType.Int, 4);
                    this.sqlCommand1.Parameters.Add("@Speech", System.Data.SqlDbType.VarBinary);
                }
                this.sqlCommand1.Parameters["@ID"].Value = this.editID.Text;
                this.sqlCommand1.Parameters["@Speech"].Value = this._fileData;
                int iresult = this.sqlCommand1.ExecuteNonQuery();
                MessageBox.Show(Convert.ToString(iresult));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.sqlConnection1.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmdSelect = new SqlCommand("select Speech from dbo.VoiceBiometric where ID=@ID", this.sqlConnection1);
                cmdSelect.Parameters.Add("@ID", SqlDbType.Int, 4);
                cmdSelect.Parameters["@ID"].Value = this.editID.Text;
                this.sqlConnection1.Open();
                byte[] fileLoaded = (byte[])cmdSelect.ExecuteScalar();
                string fileName = Convert.ToString(DateTime.Now.ToFileTime());
                fileName += ".wav";
                FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                fs.Write(fileLoaded, 0, fileLoaded.Length);
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.sqlConnection1.Close();
            }
        }
    }
}
