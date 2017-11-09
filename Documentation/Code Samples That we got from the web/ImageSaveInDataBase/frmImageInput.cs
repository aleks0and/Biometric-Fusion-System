using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



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
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Label label1;
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
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
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
			this.SuspendLayout();
			// 
			// lblID
			// 
			this.lblID.Location = new System.Drawing.Point(16, 40);
			this.lblID.Name = "lblID";
			this.lblID.TabIndex = 5;
			this.lblID.Text = "Id";
			// 
			// editID
			// 
			this.editID.Location = new System.Drawing.Point(128, 40);
			this.editID.Name = "editID";
			this.editID.TabIndex = 6;
			this.editID.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(8, 72);
			this.lblName.Name = "lblName";
			this.lblName.TabIndex = 7;
			this.lblName.Text = "Name";
			// 
			// editName
			// 
			this.editName.Location = new System.Drawing.Point(128, 72);
			this.editName.Name = "editName";
			this.editName.TabIndex = 8;
			this.editName.Text = "";
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
            //this.sqlConnection1.ConnectionString = ((string)(configurationAppSettings.GetValue("ConString", typeof(string))));
            this.sqlConnection1.ConnectionString = "Server=localhost\\MSSQLSERVER;Data Source=NameOfYourServer;Initial Catalog = BiometricDB; User ID = yourUser; Password = yourPassword";
            //this.sqlConnection1.ConnectionString = "Data Source=MSSQL1;Initial Catalog=AdventureWorks;" + "Integrated Security=true;";
            //Server=myServerName\myInstanceName;Database=myDataBase;User Id=myUsername;Password = myPassword;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(280, 136);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(16, 104);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.TabIndex = 1;
			this.btnLoad.Text = "Load";
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label1.Location = new System.Drawing.Point(16, 152);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 48);
			this.label1.TabIndex = 0;
			this.label1.Text = "To Load an image from Database  just provide ID value  and press Load";
			// 
			// frmImageInput
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 230);
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
			this.ResumeLayout(false);

		}
		#endregion

		
		

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
				SqlCommand cmdSelect=new SqlCommand("select Picture from tblImgData where ID=@ID",this.sqlConnection1);
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
	}
}
