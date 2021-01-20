using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vertibani_islemleri
{
	public class MusteriManager
	{
        public void Sıradaki(Musteri musteri)
        {
            Console.WriteLine("Sıradaki Müşteri :  " + musteri.Adi);

        }
        //public void Ekle2(string Tc, string Adi, string Soyadi, string DogumTarihi, string DogumYeri, string AnneKızlıkSoyadı)
        //{
        //    Console.WriteLine("Müşteri Listeye Eklenmiştir=  " + Adi);
        //}
        public void Silme(Musteri musteri)
        {
            Console.WriteLine("Müşteri Silindi:" + musteri.Adi);
        }
        public void Ekle(Musteri musteri)
        {
            Console.WriteLine("Sitemize Hoşgeldiniz " + musteri.Adi);
        }
    }
}
