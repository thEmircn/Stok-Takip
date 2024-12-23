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
    public partial class frmKategori : Form
    {
        public frmKategori()
        {
            InitializeComponent();
        }

        // Access veritabanı bağlantısı
        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=stoktakip.mdb");

        bool durum;

        private void kategoriengelle()
        {
            durum = true;
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from kategoribilgileri", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (textBox1.Text == read["kategori"].ToString() || textBox1.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }

        private void frmKategori_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            kategoriengelle();
            if (durum == true)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into kategoribilgileri (kategori) values (@kategori)", baglanti);
                komut.Parameters.AddWithValue("@kategori", textBox1.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kategori ekleme başarılı.");
            }
            else
            {
                MessageBox.Show("Böyle bir kategori mevcut", "Uyarı");
            }

            textBox1.Text = ""; // TextBox temizleniyor
        }
    }
}
