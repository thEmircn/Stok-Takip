using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Stok_Takip_Sistemi
{
    public partial class frmMarka : Form
    {
        public frmMarka()
        {
            InitializeComponent();
        }

        // Access veritabanı bağlantısı
        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=stoktakip.mdb");
        bool durum;

        private void markaengelle()
        {
            durum = true;
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from markabilgileri", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (comboBox1.Text == read["kategori"].ToString() && textBox1.Text == read["marka"].ToString() || comboBox1.Text == "" || textBox1.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }

        private void frmMarka_Load(object sender, EventArgs e)
        {
            kategorigetir();
        }

        private void kategorigetir()
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from kategoribilgileri", baglanti);
            OleDbDataReader read = komut.ExecuteReader();

            while (read.Read())
            {
                comboBox1.Items.Add(read["kategori"].ToString());
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            markaengelle();
            if (durum == true)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into markabilgileri (kategori, marka) values (@kategori, @marka)", baglanti);
                komut.Parameters.AddWithValue("@kategori", comboBox1.Text);
                komut.Parameters.AddWithValue("@marka", textBox1.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("Marka ekleme başarılı.");
            }
            else
            {
                MessageBox.Show("Böyle bir Marka bulunmakta.", "Uyarı");
            }

            // TextBox ve ComboBox içeriğini temizliyoruz
            textBox1.Text = "";
            comboBox1.Text = "";
        }
    }
}
