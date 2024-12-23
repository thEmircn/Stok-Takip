using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Stok_Takip_Sistemi
{
    public partial class satisSayfası : Form
    {
        public satisSayfası()
        {
            InitializeComponent();
        }


        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=stoktakip.mdb");
        DataSet daset = new DataSet();

        private void sepetListele()
        {
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select * from sepet", baglanti);
            adtr.Fill(daset, "Sepet");
            sepetListe.DataSource = daset.Tables["Sepet"];
            baglanti.Close();
        }
        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTel.Text = "";
            }

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from musteri where tc like'" + txtTc.Text + "'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["adsoyad"].ToString();
                txtTel.Text = read["telefon"].ToString();
            }
            baglanti.Close();
        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            Temizle();
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from urun where barkodno like'" + txtBarkodNo.Text + "'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtUrunAdi.Text = read["urunadi"].ToString();
                txtSatisFiyati.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
        }

        private void Temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktari)
                        {
                            item.Text = "";
                        }
                    }
                }
            }
        }

        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select * from sepet", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString())
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if (durum == true)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into Sepet(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTel.Text);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@urunadi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktari.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(txtToplamFiyat.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                baglanti.Open();
                OleDbCommand komut2 = new OleDbCommand("update sepet set miktari=miktari+@miktar where barkodno=@barkodno", baglanti);
                komut2.Parameters.AddWithValue("@miktar", int.Parse(txtMiktari.Text));
                komut2.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut2.ExecuteNonQuery();

                OleDbCommand komut3 = new OleDbCommand("update sepet set toplamfiyati=miktari*satisfiyati where barkodno=@barkodno", baglanti);
                komut3.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut3.ExecuteNonQuery();

                baglanti.Close();
            }
            txtMiktari.Text = "1";
            daset.Tables["Sepet"].Clear();
            sepetListele();
            hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktari)
                    {
                        item.Text = "";
                    }
                }
            }
        }

        private void txtMiktari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktari.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

            }
        }

        private void txtSatisFiyati_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktari.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

            }
        }

        private void dGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("delete from sepet where barkodno='" + sepetListe.CurrentRow.Cells["barkodno"].Value.ToString() + "'", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ürün Sepetten Silindi.");
                daset.Tables["sepet"].Clear();
                sepetListele();
                hesapla();
            }
            catch (Exception)
            {
                MessageBox.Show("Lütfen seçim yapınız");
            }
        }

        private void btnSatisİptal_Click(object sender, EventArgs e)
        {


        }

        private void hesapla()
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("select sum(toplamfiyati) from sepet", baglanti);
                lblGenelToplam.Text = komut.ExecuteScalar() + " TL ";
                baglanti.Close();
            }
            catch (Exception)
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmSatisListele listele = new frmSatisListele();
            listele.ShowDialog();
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < sepetListe.Rows.Count - 1; i++)
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into satis(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTel.Text);
                komut.Parameters.AddWithValue("@barkodno", sepetListe.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", sepetListe.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(sepetListe.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(sepetListe.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(sepetListe.Rows[i].Cells["toplamfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();

                OleDbCommand komut2 = new OleDbCommand("update urun set miktari=miktari-@miktar where barkodno=@barkodno", baglanti);
                komut2.Parameters.AddWithValue("@miktar", int.Parse(sepetListe.Rows[i].Cells["miktari"].Value.ToString()));
                komut2.Parameters.AddWithValue("@barkodno", sepetListe.Rows[i].Cells["barkodno"].Value.ToString());
                komut2.ExecuteNonQuery();

                baglanti.Close();
            }
            baglanti.Open();
            OleDbCommand komut3 = new OleDbCommand("delete from sepet", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["Sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void satisSayfası_Load(object sender, EventArgs e)
        {
            sepetListele();

        }

        private void lblGenelToplam_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void sepetListe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

