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
    public partial class personel : Form
    {
        public personel()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=stoktakip.mdb");
        DataSet daset = new DataSet();

      
        private void button5_Click(object sender, EventArgs e)
        {
            frmMüsteriEkle musteriekle = new frmMüsteriEkle();
            musteriekle.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            musterilistele musterilisteleme = new musterilistele();
            musterilisteleme.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmUrunEkle urunekle = new frmUrunEkle();
            urunekle.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmÜrünListeleme urunliste = new frmÜrünListeleme();
            urunliste.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            frmKategori kategori = new frmKategori();
            kategori.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();
        }

        private void Satis_Load(object sender, EventArgs e)
        {
        }

        

   
        bool durum;

       

        private void button9_Click(object sender, EventArgs e)
        {
            frmSatisListele listele = new frmSatisListele();
            listele.ShowDialog();
        }

    }
}
