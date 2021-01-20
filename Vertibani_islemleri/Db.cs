using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Vertibani_islemleri
{
	public class Db
	{

        public static string lang = "1";

        public static DataSet DataSetGetir(string sql, string cnnStr = "allData")
        {
            // ConStrAyarla()
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnStr].ToString()))
            {
                try
                {
                    DataSet ds = new DataSet();
                    cnn.Close();
                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
                    da.Fill(ds);
                    cnn.Close();
                    return ds;
                }
                finally
                {
                    cnn.Close();
                }
            }
        }

        public static DataTable DataTableGetir(string sql, string cnnStr = "allData")
        {
            // ConStrAyarla()
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnStr].ToString()))
            {
                try
                {
                    DataTable ds = new DataTable();
                    cnn.Close();
                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
                    da.Fill(ds);
                    cnn.Close();
                    return ds;
                }
                finally
                {
                    cnn.Close();
                }
            }
        }

        public static bool ExecuteSQL(string sql, string cnnStr = "allData")
        {
            //  ConStrAyarla()
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnStr].ToString()))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, cnn);
                    cnn.Close();
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    return false;
                }
                finally
                {
                    cnn.Close();
                }
                return true;
            }
        }

        public static bool sorguGuncelleEkle(string sql, string cnnStr = "allData")
        {
            bool bSuccess = true;
            SqlConnection cn = null;
            cn = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnStr].ToString());
            SqlCommand cmd = new SqlCommand();
            cmd = cn.CreateCommand();
            cmd.CommandText = sql;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bSuccess = false;
            }
            cn.Close();
            return bSuccess;
        }

        //'
        //string[] dataNames = { "ad", "soyad" };
        //string[] dataValues = { "salih", "kiraz" };
        // Db.sqlUpdate(dataNames, dataValues, "kullanici", "WHERE id=1");
        /// <summary>
        /// Veritabanı güncelleme işlemi
        /// </summary>
        /// <param name="dataNames">veri alanları</param>
        /// <param name="dataValues">Veriler</param>
        /// <param name="tableName">tablo ismi</param>
        /// <param name="condition">koşul</param>
        /// <returns>true/false</returns>
        public static bool sqlUpdate(string[] alanlar, string[] veriler, string tabloismi, string condition, string cnnStr = "allData")
        {
            //  ConStrAyarla()

            string sql = null;
            int x = 0;
            ArrayList aList = new ArrayList();
            aList.AddRange(alanlar);
            int say = 0;
            for (int i = 0; i <= aList.Count - 1; i++)
            {
                if (!object.ReferenceEquals(aList[i], ""))
                {
                    say += 1;
                }
            }

            sql = "  UPDATE " + tabloismi + " SET ";
            for (x = 0; x <= say - 1; x++)
            {
                if (x == say - 1)
                {
                    if (!string.IsNullOrEmpty(alanlar[x]))
                    {
                        if (string.IsNullOrEmpty(sql_injection(veriler[x])))
                        {
                            sql = sql + alanlar[x] + "= null  ";
                        }
                        else
                        {
                            if (sql_injection(veriler[x]) == "GETDATE()")
                            {
                                sql = sql + alanlar[x] + "= GETDATE() ";
                            }
                            else
                            {
                                sql = sql + alanlar[x] + "= '" + sql_injection(veriler[x]) + "'";
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(alanlar[x]))
                    {
                        if (string.IsNullOrEmpty(sql_injection(veriler[x])))
                        {
                            sql = sql + alanlar[x] + "= null, ";
                        }
                        else
                        {
                            if (sql_injection(veriler[x]) == "GETDATE()")
                            {
                                sql = sql + alanlar[x] + "= GETDATE(), ";
                            }
                            else
                            {
                                sql = sql + alanlar[x] + "= '" + sql_injection(veriler[x]) + "', ";
                            }
                        }
                    }
                }
            }
            sql = "SET DATEFORMAT dmy; " + sql + condition;
            return ExecuteSQL(sql, cnnStr);
        }

        //string[] dataNames = { "ad", "soyad" };
        //string[] dataValues = { "salih", "kiraz" };
        //Db.sqlInsert(dataNames, dataValues, "kullanici")
        /// <summary>
        /// Veritabanı ekleme işlemi
        /// </summary>
        /// <param name="dataNames">veri alanları</param>
        /// <param name="dataValues">Veriler</param>
        /// <param name="tableName">tablo ismi</param>
        /// <returns>true/false</returns>
        public static bool sqlInsert(string[] alanlar, string[] veriler, string tabloismi, string cnnStr = "allData")
        {
            //   ConStrAyarla()
            string sqlNames = null;
            string sqlValues = null;
            string sql = null;

            int x = 0;
            ArrayList aList = new ArrayList();
            aList.AddRange(alanlar);
            int say = 0;
            for (int i = 0; i <= aList.Count - 1; i++)
            {
                if (!object.ReferenceEquals(aList[i], ""))
                {
                    say += 1;
                }
            }
            sqlNames = "INSERT INTO " + tabloismi + "(";
            for (x = 0; x <= aList.Count - 1; x++)
            {
                if (x == say - 1)
                {
                    if (!string.IsNullOrEmpty(alanlar[x]))
                    {
                        sqlNames = sqlNames + alanlar[x] + ") VALUES(";
                        if (string.IsNullOrEmpty(sql_injection(veriler[x])))
                        {
                            sqlValues = sqlValues + "null)";
                        }
                        else
                        {
                            if (sql_injection(veriler[x]) == "GETDATE()")
                            {
                                sqlValues = sqlValues + " GETDATE(), ";
                            }
                            else
                            {
                                sqlValues = sqlValues + "'" + sql_injection(veriler[x]) + "')";
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(alanlar[x]))
                    {
                        sqlNames = sqlNames + alanlar[x] + ", ";
                        if (string.IsNullOrEmpty(sql_injection(veriler[x])))
                        {
                            sqlValues = sqlValues + "null, ";
                        }
                        else
                        {
                            if (sql_injection(veriler[x]) == "GETDATE()")
                            {
                                sqlValues = sqlValues + " GETDATE(), ";
                            }
                            else
                            {
                                sqlValues = sqlValues + "'" + sql_injection(veriler[x]) + "', ";
                            }
                        }
                    }
                }
            }
            sql = "SET DATEFORMAT dmy;  " + sqlNames + sqlValues;

            return ExecuteSQL(sql, cnnStr);
        }

        public static string sql_injection(string deger)
        {
            try
            {
                deger = deger.Replace("'", "`");
            }
            catch
            {
            }
            return deger;
        }

        public static SqlDataReader sqlSelect(string sQuery, string cnnStr = "allData")
        {
            // ConStrAyarla()

            SqlConnection conn = null;
            SqlCommand db = null;
            SqlDataReader rs = null;

            conn = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnStr].ToString());
            conn.Open();
            db = new SqlCommand(sQuery, conn);
            rs = db.ExecuteReader(CommandBehavior.CloseConnection);
            return rs;
        }

        public static bool sqlDelete(string tabloismi, string kosul, string cnnStr = "allData")
        {
            // ConStrAyarla()
            string sql = "DELETE FROM " + tabloismi + " " + kosul;
            return ExecuteSQL(sql, cnnStr);
        }

        /// <summary>
        /// dropdown list oluşturur
        /// </summary>
        /// <param name="sql">select value,text from test </param>
        /// <returns></returns>
        public static void DdlDoldur(string sql, bool seciniz, DropDownList ddl, string ilkAlanStr = "Seçiniz", bool ilkAlan = true, string cnnStr = "allData", bool IcerigiBosalt = true)
        {
            // ConStrAyarla()
            if ((IcerigiBosalt))
            {
                ddl.Items.Clear();
                if (seciniz)
                {
                    ddl.Items.Add(new ListItem(ilkAlanStr, "0"));
                }
            }
            DataSet ds = DataSetGetir(sql, cnnStr);
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                if (ilkAlan)
                {
                    ddl.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString()));
                }
                else
                {
                    ddl.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                }
            }
        }

        public static void listBoxDoldur(string sql, ListBox lst, string cnnStr = "allData")
        {
            // ConStrAyarla()
            lst.Items.Clear();
            DataSet ds = DataSetGetir(sql, cnnStr);
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                lst.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString()));
            }
        }

        public static void chkListDoldur(string sql, bool seciniz, CheckBoxList chk, string cnnStr = "allData")
        {
            chk.Items.Clear();
            DataSet ds = DataSetGetir(sql, cnnStr);
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                chk.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString()));
            }
        }

        public static string sqlAlanDegeri(string tablo, string alanadi, string kosul, string gosterilecekAlan = "")
        {
            // ConStrAyarla()
            string sonuc = null;
            SqlDataReader dr = sqlSelect("SELECT " + alanadi + " FROM  " + tablo + "  " + kosul);
            if (dr.Read())
            {
                if (!string.IsNullOrEmpty(gosterilecekAlan))
                {
                    sonuc = dr[gosterilecekAlan].ToString();
                }
                else
                {
                    sonuc = dr[alanadi].ToString();
                }
            }
            else
            {
                sonuc = "";
            }
            dr.Close();
            return sonuc;
        }

        public static bool sqlEkleVarsaGuncelle(string[] alanlar, string[] veriler, string tabloismi, string kosul, string cnnStr = "allData")
        {
            if (Db.sqlSelect("SELECT * FROM " + tabloismi + " " + kosul, cnnStr).HasRows)
            {
                if (Db.sqlUpdate(alanlar, veriler, tabloismi, kosul, cnnStr))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Db.sqlInsert(alanlar, veriler, tabloismi, cnnStr))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string KarakterDuzelt(string metin)
        {
            string Temp = metin.ToLower();
            Temp = Temp.Replace("-", ""); Temp = Temp.Replace(" ", "-");
            Temp = Temp.Replace("ç", "c"); Temp = Temp.Replace("ğ", "g");
            Temp = Temp.Replace("ı", "i"); Temp = Temp.Replace("ö", "o");
            Temp = Temp.Replace("ş", "s"); Temp = Temp.Replace("ü", "u");
            Temp = Temp.Replace("\"", ""); Temp = Temp.Replace("/", "");
            Temp = Temp.Replace("(", ""); Temp = Temp.Replace(")", "");
            Temp = Temp.Replace("{", ""); Temp = Temp.Replace("}", "");
            Temp = Temp.Replace("%", ""); Temp = Temp.Replace("&", "");
            Temp = Temp.Replace("+", ""); Temp = Temp.Replace(".", "");
            Temp = Temp.Replace("?", ""); Temp = Temp.Replace(",", "");
            Temp = Temp.Replace("'", "-"); Temp = Temp.Replace("!", "");
            Temp = Temp.Replace("amp;", ""); Temp = Temp.Replace(":", "-");
            Temp = Temp.Replace("#", "");
            Temp = Temp.Replace(";", "");

            return Temp;
        }

        public static string siteKey()
        {
            //return WebConfigurationManager.AppSettings["SiteKey"];
            string siteKey = String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["siteKey"])) ? "0" : Convert.ToString(HttpContext.Current.Session["siteKey"]);

            return siteKey;
        }

        public static string siteDomain()
        {
            //return WebConfigurationManager.AppSettings["Sitedomain"];
            string siteDomain = String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["siteDomain"])) ? "0" : Convert.ToString(HttpContext.Current.Session["siteDomain"]);

            return siteDomain;
        }

        public static string siteName()
        {
            //return WebConfigurationManager.AppSettings["SiteName"];
            string siteName = String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["siteName"])) ? "0" : Convert.ToString(HttpContext.Current.Session["siteName"]);

            return siteName;
        }

        public static string yetkiKodu()
        {
            //return WebConfigurationManager.AppSettings["SiteName"];
            string yetkiKodu = String.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["yetkiKodu"])) ? "0" : Convert.ToString(HttpContext.Current.Session["yetkiKodu"]);

            return yetkiKodu;
        }

        //private static string site_key = "6LeQUJ0UAAAAAHjsiGVD6TEcUXRazEmYHvCrUNRq";
        private static string secret_key = "6LeQUJ0UAAAAAEilqd9x1L0l7LtnpYZ3R7AolXik";



        public class GoogleApiJsonModel
        {
            public string success { get; set; }
        }

        public static void notificationGonder(string message, string link)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json";

            request.Headers.Add("authorization", "Basic ZWU0NDljNTMtYjZjZS00Y2MwLWE0NTktMDU1YjhhYjg4NjI4");

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                    + "\"app_id\": \"0834a152-7772-4eec-be86-0a6055b094e1\","
                                                    + "\"contents\": {\"en\": \"" + message + "\"},"
                                                    + "\"url\": \"http://karatekin.edu.tr/" + link + "\","
                                                    + "\"included_segments\": [\"All\"]}");

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }

        public static void logtut(string islem)
        {
            string[] alanlar = { "adsoyad", "giris_tar", "sicil", "islem_turu", "ip", "site_id", "site_domain" };
            string[] veriler = { Convert.ToString(HttpContext.Current.Session["userName"]), DateTime.Now.ToString(), Convert.ToString(HttpContext.Current.Session["siciL"]), islem, IpAddress(), Db.siteKey(), Db.siteDomain() };
            Db.sqlInsert(alanlar, veriler, "log");

        }

        public static string IpAddress()
        {
            string strIpAddress;
            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIpAddress == null)
            {
                strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return strIpAddress;
        }


        public static void MailGonder(string ToAddress, string BodyText, string Subject, string fromadress = "Karatekin Üniversitesi - Bilgi İşlem Daire Başkanlığı Yazılım Grubu <webdestek@karatekin.edu.tr>")
        {
            string FromAddress = fromadress;
            System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();

            mailMsg.From = new MailAddress(FromAddress);
            mailMsg.To.Add(new MailAddress(ToAddress));

            mailMsg.Subject = Subject;
            mailMsg.IsBodyHtml = true;
            mailMsg.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-9");

            System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
            (System.Text.RegularExpressions.Regex.Replace(BodyText, @"<(.|\n)*?>", string.Empty), null, "text/plain");
            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(BodyText, null, "text/html");

            mailMsg.AlternateViews.Add(plainView);
            mailMsg.AlternateViews.Add(htmlView);
            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment(attac);
            //mailMsg.Attachments.Add(attachment);
            // Smtp configuration
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.karatekin.edu.tr";
            smtp.Port = 25;
            //smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("webdestek@karatekin.edu.tr", "1973aSD");
            //smtp.EnableSsl = true;

            try
            {
                smtp.Send(mailMsg);


            }
            catch (Exception)
            {

            }

        }
        public static Boolean localmi()
        {
            if (HttpContext.Current.Request.IsLocal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string getProfileImage(string sicilno)
        {
            string imagePath = "";
            DataTable ayarCek = new DataTable();

            ayarCek = Db.DataTableGetir(@"Select p_resim from websitem.users where user_id = '" + sicilno + "'", "allData");


            if (ayarCek.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ayarCek.Rows[0]["p_resim"])))
                {
                    imagePath = @"https://websitem.karatekin.edu.tr" + Convert.ToString(ayarCek.Rows[0]["p_resim"]).Replace(@"../", @"/") + "";
                }
                else
                {
                    DataTable CinsiyetGetir = Db.DataTableGetir(@"Select cinsiyet from telefonRehberi where psicilno = '" + sicilno + "'", "personelData");

                    imagePath = PersonelSvgImage(CinsiyetGetir.Rows[0]["cinsiyet"].ToString());
                }
            }
            else
            {
                DataTable CinsiyetGetir = Db.DataTableGetir(@"Select cinsiyet from telefonRehberi where psicilno = '" + sicilno + "'", "personelData");

                imagePath = PersonelSvgImage(CinsiyetGetir.Rows[0]["cinsiyet"].ToString());


            }


            return imagePath;

        }

        public static string PersonelSvgImage(string cinsiyet)
        {
            string image = "";
            string Renklendir = "";
            if (!string.IsNullOrEmpty(cinsiyet))
            {
                if (cinsiyet == "Kadın" || cinsiyet == "K" || cinsiyet == "Kadin")
                {
                    Renklendir = "#E40066";
                    image = @"data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 105.86 123.71'%3e%3cdefs%3e%3cstyle%3e.cls-1%7bfill:%23ddd;%7d.cls-2%7bfill:" + Renklendir.Replace("#", "%23") + @";%7d%3c/style%3e%3c/defs%3e%3ctitle%3eAsset 1%3c/title%3e%3cg id='Layer_2' data-name='Layer 2'%3e%3cg id='OBJECTS'%3e%3cpath class='cls-1' d='M99.8,85.84c-2.14-5.88-28-9.19-33.14-10.43S63.45,61,63.45,61l-8-3.09V56l-2.48,1-2.48-1v1.9L42.4,61s2,13.18-3.21,14.43S8.19,80,6.06,85.84,0,123.71,0,123.71H105.86S101.94,91.71,99.8,85.84Z'/%3e%3cpath class='cls-1' d='M33.6,29.33V41.5A24.36,24.36,0,0,0,35,49.63c1.45,4.11,4.16,10,8.65,14.07a14.82,14.82,0,0,0,9.94,3.79h0A14.82,14.82,0,0,0,63.5,63.7c4.48-4,7.19-10,8.65-14.07a24.36,24.36,0,0,0,1.39-8.13V29.33Z'/%3e%3cpath class='cls-2' d='M69.89,78.21c-1.14-3.63-5.47-5.37-5.61-5.43C68,80.37,53,102.09,53,102.09S38.11,80.37,41.8,72.78c-.14.05-4.48,1.8-5.61,5.43s-7.89,15.48-10.31,16c0,0,9.73-4.37,12.73-4.79,0,0,14.18,25.27,14.44,25.65.25-.38,14.43-25.65,14.43-25.65,3,.42,12,3.4,12,3.4C77.07,92.3,71,81.9,69.89,78.21Z'/%3e%3cpath class='cls-1' d='M39.53,58.31s-11.58-9-12.26-16.8S32.32-1.42,46,2l2.46.68S57.2-1.56,61.85.63,78.91,20.42,80.31,25.24c2.1,7.27,6.62,32.81-9.38,43.06,0,0,10.94-12.4,7.41-29.71,0,0,1.55,15.68-6.74,24.7,0,0,2.78-7.61,3.46-12.4,0,0-3.78,12.08-10.2,16.86a29.72,29.72,0,0,0,6-17.5l-4.77,9Z'/%3e%3c/g%3e%3c/g%3e%3c/svg%3e";
                }
                else if (cinsiyet == "Erkek" || cinsiyet == "E")
                {
                    Renklendir = "#345995";
                    image = @"data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 113.29 122.7'%3e%3cdefs%3e%3cstyle%3e.cls-1%7bfill:%23ddd;%7d.cls-2%7bfill:%23f2f2f2;%7d.cls-3%7bfill:" + Renklendir.Replace("#", "%23") + @";%7d%3c/style%3e%3c/defs%3e%3ctitle%3eAsset 2%3c/title%3e%3cg id='Layer_2' data-name='Layer 2'%3e%3cg id='OBJECTS'%3e%3cpath class='cls-1' d='M113.29,122.7H0S8.38,86.3,10.7,82.23c1.84-3.22,20.08-7.76,28.53-10.6,2.23-.75,3.77-1.38,4.16-1.84l.13-.18c1.29-1.88,1.42-7.52,1.41-10.24H68.37c0,2.81.13,8.74,1.54,10.41a1.26,1.26,0,0,0,.19.18,21.14,21.14,0,0,0,4.7,1.91c8.72,2.86,26,7.23,27.79,10.36C104.92,86.3,113.29,122.7,113.29,122.7Z'/%3e%3cpath class='cls-1' d='M33,38.76s-1.38-14-.84-15.66S35.86,7.67,44.39,4.64L41,3.67A33.65,33.65,0,0,1,51.73,1.73S49.46,0,49.9,0s13,1.08,16.41,4.86L64.15,1.51s8,3.67,9.61,9.61L73.87,9s5.08,9.72,5.62,16.74-.62,10.39-.62,10.39S33.13,38.87,33,38.76Z'/%3e%3cpath class='cls-1' d='M36.45,30.88V43.07a24.23,24.23,0,0,0,1.4,8.15c1.47,4.11,4.2,10,8.73,14.1a15,15,0,0,0,10,3.79h0a15,15,0,0,0,10-3.79c4.53-4.06,7.26-10,8.73-14.1a24.23,24.23,0,0,0,1.4-8.15V30.88Z'/%3e%3cpath class='cls-1' d='M35.51,34.75s-3.59-1.08-3.59,1.52.87,4,.4,5.58A4.42,4.42,0,0,0,36.69,47C40.36,47,35.51,34.75,35.51,34.75Z'/%3e%3cpath class='cls-1' d='M76.5,34.75s3.41-.91,3.41,1.69-.69,3.86-.22,5.4A4.42,4.42,0,0,1,75.32,47C71.65,47,76.5,34.75,76.5,34.75Z'/%3e%3cpath class='cls-2' d='M74.8,71.87,70.13,122.7H43.92L39.23,71.62c2.23-.75,3.77-1.38,4.16-1.84l.13-.18L57,80.38,70.1,70A21.14,21.14,0,0,0,74.8,71.87Z'/%3e%3cpolygon class='cls-3' points='46.26 122.7 51.46 93.21 49.4 86.97 57.03 81.83 64.65 86.97 62.59 93.21 67.79 122.7 46.26 122.7'/%3e%3c/g%3e%3c/g%3e%3c/svg%3e";
                }
            }

            else
            {
                Renklendir = "#BF4342";
                image = @"data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 113.29 122.7'%3e%3cdefs%3e%3cstyle%3e.cls-1%7bfill:%23ddd;%7d.cls-2%7bfill:%23f2f2f2;%7d.cls-3%7bfill:" + Renklendir.Replace("#", "%23") + @";%7d%3c/style%3e%3c/defs%3e%3ctitle%3eAsset 2%3c/title%3e%3cg id='Layer_2' data-name='Layer 2'%3e%3cg id='OBJECTS'%3e%3cpath class='cls-1' d='M113.29,122.7H0S8.38,86.3,10.7,82.23c1.84-3.22,20.08-7.76,28.53-10.6,2.23-.75,3.77-1.38,4.16-1.84l.13-.18c1.29-1.88,1.42-7.52,1.41-10.24H68.37c0,2.81.13,8.74,1.54,10.41a1.26,1.26,0,0,0,.19.18,21.14,21.14,0,0,0,4.7,1.91c8.72,2.86,26,7.23,27.79,10.36C104.92,86.3,113.29,122.7,113.29,122.7Z'/%3e%3cpath class='cls-1' d='M33,38.76s-1.38-14-.84-15.66S35.86,7.67,44.39,4.64L41,3.67A33.65,33.65,0,0,1,51.73,1.73S49.46,0,49.9,0s13,1.08,16.41,4.86L64.15,1.51s8,3.67,9.61,9.61L73.87,9s5.08,9.72,5.62,16.74-.62,10.39-.62,10.39S33.13,38.87,33,38.76Z'/%3e%3cpath class='cls-1' d='M36.45,30.88V43.07a24.23,24.23,0,0,0,1.4,8.15c1.47,4.11,4.2,10,8.73,14.1a15,15,0,0,0,10,3.79h0a15,15,0,0,0,10-3.79c4.53-4.06,7.26-10,8.73-14.1a24.23,24.23,0,0,0,1.4-8.15V30.88Z'/%3e%3cpath class='cls-1' d='M35.51,34.75s-3.59-1.08-3.59,1.52.87,4,.4,5.58A4.42,4.42,0,0,0,36.69,47C40.36,47,35.51,34.75,35.51,34.75Z'/%3e%3cpath class='cls-1' d='M76.5,34.75s3.41-.91,3.41,1.69-.69,3.86-.22,5.4A4.42,4.42,0,0,1,75.32,47C71.65,47,76.5,34.75,76.5,34.75Z'/%3e%3cpath class='cls-2' d='M74.8,71.87,70.13,122.7H43.92L39.23,71.62c2.23-.75,3.77-1.38,4.16-1.84l.13-.18L57,80.38,70.1,70A21.14,21.14,0,0,0,74.8,71.87Z'/%3e%3cpolygon class='cls-3' points='46.26 122.7 51.46 93.21 49.4 86.97 57.03 81.83 64.65 86.97 62.59 93.21 67.79 122.7 46.26 122.7'/%3e%3c/g%3e%3c/g%3e%3c/svg%3e";
            }
            return image;
        }

        #region renkLendir
        public static string Renklendir(string renk, string sitekey)
        {
            DataTable renkCek = Db.DataTableGetir(@"Select " + renk + @" from ayarlar where site_id = '" + sitekey + "'");
            string renkver = "";

            if (renkCek.Rows.Count > 0)
            {
                renkver = Convert.ToString(renkCek.Rows[0][renk]);

            }
            else
            {
                renkver = @"#3E885B";
            }
            return renkver;
        }
        #endregion

        public static string CapitalizeText(string text)
        {
            string capitext = "";
            capitext = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
            return capitext;
        }
    }
}