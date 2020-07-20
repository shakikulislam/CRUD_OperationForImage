﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Windows.Forms;
using CRUD_OperationForImage.Properties;

namespace CRUD_OperationForImage
{
    public partial class FormHome : Form
    {
        public FormHome()
        {
            InitializeComponent();
            LoadId();
        }

        private static SqlConnection _sqlConnection = new SqlConnection(@"Data Source=SHAKIKUL-PC\SQLEXPRESS; Initial Catalog=CRUD_OperationForImageDB; User Id=sa; Password=sa123456789");
        private static SqlCommand _sqlCommand;
        private static SqlDataAdapter _sqlDataAdapter;
        private static SqlDataReader _sqlDataReader;
        private static DataTable _dataTable;
        private static DataSet _dataSet;

        private void buttonAddImage_Click(object sender, System.EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "JPG|*.jpg|PNG|*.png", Multiselect = false })
            {
                pictureBox1.Image = openFileDialog.ShowDialog() == DialogResult.OK ? Image.FromFile(openFileDialog.FileName) : Properties.Resources.DefaultAvator;
            }
        }

        private void LoadId()
        {
            _sqlConnection.Close();
            _sqlConnection.Open();
            _sqlCommand = new SqlCommand("SELECT MAX(id) AS id FROM TableImage", _sqlConnection);
            _sqlDataReader = _sqlCommand.ExecuteReader();
            if (_sqlDataReader.Read())
            {
                textBoxId.Text = _sqlDataReader["id"] != DBNull.Value ? ((int)_sqlDataReader["id"]+ 1).ToString() : "1";
            }
        }

        private void ViewForDataGrid()
        {
            _sqlConnection.Close();
            _sqlConnection.Open();
            _sqlCommand = new SqlCommand("SELECT id AS 'ID', Name FROM TableImage", _sqlConnection);
            _sqlDataAdapter = new SqlDataAdapter(_sqlCommand);
            _dataTable = new DataTable();
            _sqlDataAdapter.Fill(_dataTable);
            dataGridView1.DataSource = _dataTable;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void buttonAdd_Click(object sender, System.EventArgs e)
        {
            ImageConverter imageConverter = new ImageConverter();
            byte[] bytesImage = (byte[])imageConverter.ConvertTo(pictureBox1.Image, Type.GetType("System.Byte[]"));

            _sqlConnection.Close();
            _sqlConnection.Open();

            _sqlCommand = new SqlCommand("INSERT INTO TableImage(id,Name,Picture)VALUES('" + textBoxId.Text + "','" + textBoxName.Text + "',@image)", _sqlConnection);
            _sqlCommand.Parameters.AddWithValue("@image", bytesImage);
            var isSuccess=_sqlCommand.ExecuteNonQuery();
            if (isSuccess>0)
            {
                labelMessge.ForeColor = Color.Green;
                labelMessge.Text = "Value insert is success...";

                textBoxName.Clear();
                pictureBox1.Image = Properties.Resources.DefaultAvator;
                
            }
            else
            {
                labelMessge.ForeColor = Color.Red;
                labelMessge.Text = "Insert failed...";
            }


            LoadId();
            ViewForDataGrid();
            
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            ViewForDataGrid();
        }

        private void textBoxId_TextChanged(object sender, EventArgs e)
        {
            _sqlConnection.Close();
            _sqlConnection.Open();
            _sqlCommand = new SqlCommand("SELECT *FROM TableImage WHERE id='" + textBoxId.Text + "'", _sqlConnection);
            _sqlDataReader = _sqlCommand.ExecuteReader();

            if (_sqlDataReader.Read())
            {
                if (_sqlDataReader["Picture"] != DBNull.Value)
                {
                    MemoryStream stream = new MemoryStream((byte[]) _sqlDataReader["Picture"]);
                    pictureBox1.Image = Image.FromStream(stream);
                }
                else
                {
                    pictureBox1.Image = Resources.DefaultAvator;
                }
            }
            else
            {
                pictureBox1.Image = Resources.DefaultAvator;
            }

        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBoxId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxName.Clear();
            pictureBox1.Image = Resources.DefaultAvator;
            LoadId();
            textBoxName.Focus();
        }


    }
}
