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
	}
}