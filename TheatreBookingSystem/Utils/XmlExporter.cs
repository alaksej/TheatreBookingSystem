using System.Collections.Generic;
using System.Xml;
using TheatreBookingSystem.Models;

namespace TheatreBookingSystem.Models
{
	public static class XmlExporter
	{
		public static void ExportOrders(string filePath, IEnumerable<OrderViewModel> ovms)
		{
			var xml = XmlWriter.Create(filePath);
			xml.WriteStartDocument();
			xml.WriteWhitespace("\n");
			xml.WriteStartElement("orders");
			xml.WriteWhitespace("\n");

			foreach (var ovm in ovms)
			{
				xml.WriteStartElement("order");

				xml.WriteAttributeString("user", ovm.UserName);
				xml.WriteAttributeString("email", ovm.User.Email);
				xml.WriteAttributeString("phone", ovm.User.Phone);
				xml.WriteWhitespace("\n");
				xml.WriteElementString("play", ovm.PlayName);
				xml.WriteWhitespace("\n");
				xml.WriteStartElement("date");
				xml.WriteValue(ovm.Date);
				xml.WriteEndElement();
				xml.WriteWhitespace("\n");
				xml.WriteElementString("category", ovm.CategoryName);
				xml.WriteWhitespace("\n");
				xml.WriteElementString("quantity", ovm.Quantity.ToString());
				xml.WriteWhitespace("\n");
				xml.WriteElementString("bill", ovm.Bill.ToString());
				xml.WriteWhitespace("\n");
				xml.WriteEndElement();
				xml.WriteWhitespace("\n");
			}
			xml.WriteWhitespace("\n");
			xml.WriteEndElement();
			xml.WriteWhitespace("\n");
			xml.WriteEndDocument();
			xml.Close();
			xml.Dispose();
		}
	}
}