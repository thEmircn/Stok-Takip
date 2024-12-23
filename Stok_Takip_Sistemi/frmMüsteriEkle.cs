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
    public partial class frmMüsteriEkle : Form
    {
        public frmMüsteriEkle()
        {
            InitializeComponent();
        }

        // Access veritabanı bağlantısı
        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=stoktakip.mdb");

        private void frmMüsteriEkle_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Veritabanı bağlantısını açıyoruz
            baglanti.Open();

            // Access veritabanı komutu
            OleDbCommand komut = new OleDbCommand("INSERT INTO musteri (tc, adsoyad, telefon, adres, mail) VALUES (@tc, @adsoyad, @telefon, @adres, @mail)", baglanti);

            // Parametreler ile değer ekleme
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@telefon", txtTel.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@mail", txtMail.Text);

            // Komutu çalıştırıyoruz
            komut.ExecuteNonQuery();

            // Veritabanı bağlantısını kapatıyoruz
            baglanti.Close();

            // Kullanıcıya başarı mesajı gösteriyoruz
            MessageBox.Show("Müşteri Kaydı Tamamlandı. Başarı ile Eklendi");

            // Formdaki tüm TextBox'ları temizliyoruz
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }
    }
}
