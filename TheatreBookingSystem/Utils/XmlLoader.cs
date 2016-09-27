using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Xml;

namespace TheatreBookingSystem.Models
{
	public static class XmlLoader
	{
		private static ICollection<Author> authors;
		private static ICollection<Genre> genres;

		private static Author GetAuthor(XmlReader reader, string authorTag)
		{
			reader.MoveToAttribute(authorTag);
			var name = reader.ReadContentAsString();
			Author author = authors.FirstOrDefault(a => a.Name == name);
			if (author == null)
			{
				author = new Author { Name = name };
				authors.Add(author);
			}
			return author;
		}
		private static Genre GetGenre(XmlReader reader, string genreTag)
		{
			reader.MoveToAttribute(genreTag);
			var name = reader.ReadContentAsString();
			Genre genre = genres.FirstOrDefault(a => a.Name == name);
			if (genre == null)
			{
				genre = new Genre { Name = name };
				genres.Add(genre);
			}
			return genre;
		}
		private static void GetDates(XmlReader reader, Play play, string dateTag, string dateIdTag)
		{
			if (reader.ReadToDescendant(dateTag))
				do
				{
					reader.MoveToAttribute(dateIdTag);
					int dateId = reader.ReadContentAsInt();
					reader.MoveToContent();
					var date = new PlayDate
					{
						DateID = dateId,
						PerformanceDate = reader.ReadElementContentAsDateTime(),
						Play = play
					};
					play.Dates.Add(date);
				} while (reader.ReadToNextSibling(dateTag));
		}

		public static List<Play> GetRepertoire(string xmlPath)
		{
			authors = new List<Author>();
			genres = new List<Genre>();
			List<Play> listOfPlays = new List<Play>();
			using (XmlReader reader = XmlReader.Create(xmlPath))
			{
				while (reader.ReadToFollowing("play"))
				{
					Play play = new Play();
					reader.MoveToAttribute("id");
					play.PlayID = reader.ReadContentAsInt();
					play.Author = GetAuthor(reader, "author");
					play.Genre = GetGenre(reader, "genre");
					reader.ReadToFollowing("name");
					play.Name = reader.ReadElementContentAsString();
					reader.ReadToFollowing("dates");
					GetDates(reader, play, "date", "id");
					listOfPlays.Add(play);
				}
			}
			return listOfPlays;
		}

		public static List<Category> GetCategories(string xmlPath)
		{
			List<Category> list = new List<Category>();
			using (XmlReader reader = XmlReader.Create(xmlPath))
			{
				while (reader.ReadToFollowing("category"))
				{
					var cat = new Category();
					reader.MoveToAttribute("id");
					cat.CategoryID = reader.ReadContentAsInt();
					reader.ReadToFollowing("name");
					cat.CategoryName = reader.ReadElementContentAsString();
					reader.ReadToFollowing("totalTickets");
					cat.TotalTickets = reader.ReadElementContentAsInt();
					reader.ReadToFollowing("ticketPrice");
					cat.TicketPrice = reader.ReadElementContentAsDecimal();
					list.Add(cat);
				}
			}
			return list;
		}
	}
}