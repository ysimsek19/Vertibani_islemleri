using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vertibani_islemleri
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string ogrenci = "";
			DataTable vt = Db.DataTableGetir(@"Select * from ogrenci_table");

			if (vt.Rows.Count > 0)
			{
				foreach (DataRow item in vt.Rows)
				{
					ogrenci += @"<li><span>"+item["ogrenci_no"]+@":</span>"+item["ogrenci_adi"]+@"</li>";
				}
				test.InnerHtml = ogrenci;
			}
			
		}

		protected void LinkButton1_Click(object sender, EventArgs e)
		{
			if (no.Value != "" && ad.Value != "")
			{


				string[] alanlar = { "ogrenci_no", "ogrenci_adi" };
				string[] veriler = { no.Value, ad.Value };
				DataTable vt = Db.DataTableGetir(@"Select * from ogrenci_table where ogrenci_no='"+ no.Value+@"'");
				if (vt.Rows.Count==0)
				{
					Db.sqlInsert(alanlar, veriler, "ogrenci_table");
				}
				else
				{
					Response.Write("Bu Numara kullanılmaktador");
				}
			
			}
			
		}
	}
}