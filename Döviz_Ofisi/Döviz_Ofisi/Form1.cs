using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
namespace Döviz_Ofisi
    
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=SCARFACE\\SQLEXPRESS;Initial Catalog=DövizOfisi;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dövizOfisiDataSet6.TblDoviz' table. You can move, or remove it, as needed.
            this.tblDovizTableAdapter6.Fill(this.dövizOfisiDataSet6.TblDoviz);


            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldosya = new XmlDocument();
            xmldosya.Load(bugun);
           

            string dolaral = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            lbldolaral.Text = dolaral;


            string dolarsat = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            lbldolarsat.Text = dolarsat;

            string euroal = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            lbleuroal.Text = euroal;

            string eurosat = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            lbleurosat.Text = eurosat;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btndolal_Click(object sender, EventArgs e)
        {
            txtkur.Text = lbldolaral.Text;
            lblislem.Text = "Dolar Alış İşlemi";

        }

        private void btndolsat_Click(object sender, EventArgs e)
        {
            txtkur.Text = lbldolarsat.Text;

            lblislem.Text = "Dolar Satış İşlemi";
        }

        private void btneuroal_Click(object sender, EventArgs e)
        {
            txtkur.Text = lbleuroal.Text;
            lblislem.Text = "Euro Alış İşlemi";
        }

        private void btneurosat_Click(object sender, EventArgs e)
        {
            txtkur.Text = lbleurosat.Text;
            lblislem.Text = "Euro Satış İşlemi";

        }

        private void btnsatısyap_Click(object sender, EventArgs e)
        {
           

            double kur, miktar, toplam;

            kur = Convert.ToDouble(txtkur.Text);
            miktar = Convert.ToDouble(txtmiktar.Text);
            toplam = miktar * kur;
            
            txttoplam.Text = toplam.ToString();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Insert into TblDoviz (MusteriAdSoyad,TürkLirasıMiktarı,DolarMiktarı,EuroMiktarı,İslem1) values (@p1,@p2,@p3,@p4,@p5)", baglanti);

            komut.Parameters.AddWithValue("@p1", textBox2.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(txttoplam.Text));
            if(lblislem.Text=="Dolar Alış İşlemi" || lblislem.Text=="Dolar Satış İşlemi")
            {
                komut.Parameters.AddWithValue("@p3", txtmiktar.Text);
            }
            else
            {
                komut.Parameters.AddWithValue("@p3", label0.Text);

            }
            if (lblislem.Text=="Euro Alış İşlemi" || lblislem.Text=="Euro Satış İşlemi")
            {
                komut.Parameters.AddWithValue("@p4", txtmiktar.Text);
            }
            else
            {
                komut.Parameters.AddWithValue("@p4", label0.Text);

            }

            komut.Parameters.AddWithValue("@p5", labelislem2.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();

            this.tblDovizTableAdapter6.Fill(this.dövizOfisiDataSet6.TblDoviz);



        }

        private void txtkur_TextChanged(object sender, EventArgs e)
        {
            txtkur.Text = txtkur.Text.Replace(".",",");
        }

        private void btnsatıs2_Click(object sender, EventArgs e)
        {
            double kur = Convert.ToDouble(txtkur.Text);
            int miktar = Convert.ToInt32(txtmiktar.Text);
            int toplam = Convert.ToInt32(miktar / kur);
            double kalan = miktar % kur;

            txttoplam.Text = toplam.ToString();
            txtkalan.Text = kalan.ToString() ;


            baglanti.Open();
            SqlCommand komut = new SqlCommand("Insert into TblDoviz (MusteriAdSoyad,TürkLirasıMiktarı,DolarMiktarı,EuroMiktarı,İslem2) values (@p1,@p2,@p3,@p4,@p5)", baglanti);

            komut.Parameters.AddWithValue("@p1", textBox2.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(txttoplam.Text));
            if (lblislem.Text == "Dolar Alış İşlemi" || lblislem.Text == "Dolar Satış İşlemi")
            {
                komut.Parameters.AddWithValue("@p3", txtmiktar.Text);
            }
            else
            {
                komut.Parameters.AddWithValue("@p3", label0.Text);

            }
            if (lblislem.Text == "Euro Alış İşlemi" || lblislem.Text == "Euro Satış İşlemi")
            {
                komut.Parameters.AddWithValue("@p4", txtmiktar.Text);
            }
            else
            {
                komut.Parameters.AddWithValue("@p4", label0.Text);

            }

            komut.Parameters.AddWithValue("@p5", label2.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();

            this.tblDovizTableAdapter6.Fill(this.dövizOfisiDataSet6.TblDoviz);


        }
    }
}
