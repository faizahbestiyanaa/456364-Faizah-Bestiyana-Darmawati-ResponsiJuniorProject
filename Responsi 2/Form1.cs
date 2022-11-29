using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Responsi2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private NpgsqlConnection conn;
        string connstring = "Host = locallhost; Port = 2022; Username= postgres; password = informatika; Database = ResponsiBesty";
        public static NpgsqlCommand cmd;
        public DataTable dt;
        private DataGridViewRow r;
        private string sql = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"INSERT INTO public.departemen (nama_dep) values '" + cbDept.Text + "', and INSERT INTO public.karyawan (nama) values '" + tbNama.Text + "'";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue ("nama_dep", cbDept.Text);
                cmd.Parameters.AddWithValue ("nama", tbNama.Text);
                MessageBox.Show("Data has been successfully added!");
                tbNama.Text = cbDept.Text = null;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                conn.Open();
                dataGridView1.DataSource = null;
                sql = "SELECT * from karyawan AND SELECT * from departemen";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            if (e.RowIndex == 0)
            {
                r = dataGridView1.Rows[e.RowIndex];
                tbNama.Text = r.Cells["nama"].Value.ToString();
                cbDept.Text = r.Cells["nama_dep"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Anda belum melakukan edit","masukkan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conn.Open();
                sql = "UPDATE karyawan SET name = '" + tbNama.Text + "', AND UPDATE departemen SET nama_dep = '" + cbDept.Text + "'";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id_karyawan", r.Cells["id_karyawan"].Value.ToString());
                cmd.ExecuteNonQueryAsync();
                MessageBox.Show("Update sukses");
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Anda belum melakukan edit", "masukkan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Apa kamu yakin menghapus ini" + r.Cells["nama"].Value.ToString() + "?", "Konfirmasi hapus",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;
            try
            {
                conn.Open();
                sql = "DELETE from karyawan where id_karyawan = '";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id_karyawan", r.Cells["id_karyawan"].Value.ToString());
                cmd.ExecuteNonQueryAsync();
                MessageBox.Show("Data sukses dihapus");
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
