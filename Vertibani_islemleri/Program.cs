using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vertibani_islemleri
{
	public class Program
	{
        static void Main(string[] args)
        {
            Musteri musteri1 = new Musteri();
            musteri1.Tc = "11111111111";
            musteri1.Adi = "Yalçın";
            musteri1.Soyadi = "ŞİMŞEK";
            musteri1.DogumTarihi = "01.02.1993";
            musteri1.DogumYeri = "İskilip";
            musteri1.AnneKızlıkSoyadi = "Aslan";

            Musteri musteri2 = new Musteri();
            musteri2.Tc = "22222222222";
            musteri2.Adi = "Ali";
            musteri2.Soyadi = "YILDIRIM";
            musteri2.DogumTarihi = "01.01.1999";
            musteri2.DogumYeri = "Çankırı";
            musteri2.AnneKızlıkSoyadi = "Kaplan";

            Musteri musteri3 = new Musteri();
            musteri3.Tc = "33333333333";
            musteri3.Adi = "Hasan";
            musteri3.Soyadi = "GÜNEŞ";
            musteri3.DogumTarihi = "02.02.1992";
            musteri3.DogumYeri = "Ankara";
            musteri3.AnneKızlıkSoyadi = "Sırtlan";

            Musteri musteri4 = new Musteri();
            musteri4.Tc = "44444444444";
            musteri4.Adi = "Talat";
            musteri4.Soyadi = "TOPRAK";
            musteri4.DogumTarihi = "03.03.1990";
            musteri4.DogumYeri = "Çankaya";
            musteri4.AnneKızlıkSoyadi = "Dağlar";

            Musteri musteri5 = new Musteri();
            musteri5.Tc = "55555555555";
            musteri5.Adi = "Hüseyin";
            musteri5.Soyadi = "ŞAHİN";
            musteri5.DogumTarihi = "04.04.1994";
            musteri5.DogumYeri = "Çorum";
            musteri5.AnneKızlıkSoyadi = "Kartal";

            Musteri[] musteriler = new Musteri[] { musteri1, musteri2, musteri3, musteri4, musteri5 };

            foreach (Musteri musteri in musteriler)
            {
                Console.WriteLine("Müşteri TC =  " + musteri.Tc);
                Console.WriteLine("Müşteri Adı =  " + musteri.Adi);
                Console.WriteLine("Müşteri Soyadı =  " + musteri.Soyadi);
                Console.WriteLine("Müşteri Doğum Tarihi =  " + musteri.DogumTarihi);
                Console.WriteLine("Müşteri Doğum Yeri =  " + musteri.DogumYeri);
                Console.WriteLine("Müşteri Anne Kızlık Soyadı =  " + musteri.AnneKızlıkSoyadi);
                Console.WriteLine("---------------");
            }
            MusteriManager musteriManager = new MusteriManager();
            musteriManager.Sıradaki(musteri2);






            //foreach (Musteri musteri in musteriler)

            musteriManager.Silme(musteri2);

        }

            //foreach (Musteri m in musteriler)
            //{
            //    musteriManager.Ekle(m);
            //    Console.ReadLine();
            //}
}

    }
