using HtmlAgilityPack;
using Newtonsoft.Json;


class foo 
{
	public static void Main(string[] args) 
	{
		List<Film> resultList = ParsingMethod(url: "https://www.kinopoisk.ru/premiere/ru/");

		Rootobject rootobject = new Rootobject();

		Datum[] resultData = new Datum[resultList.Count];
		for (int i = 0; i < resultList.Count; i++)
		{
			resultData[i] = DatumConvert(resultList[i]);
		}

		rootobject.data = resultData;

        JsonSerializer serializer = new JsonSerializer();

        using (StreamWriter sw = new StreamWriter(@"c:\json.txt"))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            serializer.Serialize(writer, rootobject);
        }

    }

    private static Datum DatumConvert(Film film)
	{
		Datum datum = new Datum();
		datum.name = film.name;
		datum.name_eng = film.name_eng;
		datum.film_link = film.film_link;
		datum.film_rating = film.film_rating;
		datum.wait_rating = film.wait_rating;
		datum.votes = film.votes;
		datum.date = film.date;
		datum.company = film.company;
		datum.genres = film.genres;

		return datum;
	}

	private static List<Film> ParsingMethod(string url) 
	{
		List<Film> films = new List<Film>();


		try
		{
			var doc = new HtmlDocument();


			doc.Load(@"C:\Users\viplo\Desktop\fap\fooapp\resources\График кинопремьер в России — Кинопоиск.html");


			var result = doc.DocumentNode.SelectNodes(".//td[@class='news']//div[@class='prem_list']//div[contains(@style, 'background: #f2f2f2')]//div[@class='premier_item']");

			if (result != null)
			{
				foreach (var element in result)
				{

					var name = element.SelectSingleNode(".//div[@class='text']//div[@class='textBlock']//span[@class='name_big']//a[text()]");
					var link = "https://www.kinopoisk.ru/" + element.SelectSingleNode(".//div[@class='text']//div[@class='textBlock']//span[@class='name_big']//a").GetAttributeValue("href", "");
					var rating = "";
					var waitRating = "";
					var votes = "";
					var company = "";
					string name_english = "";
					string[] stringGenres = null;
					var date = element.SelectSingleNode(".//meta[contains(@itemprop, 'startDate')]").GetAttributeValue("content", "");

					if (element.SelectSingleNode(".//span[starts-with(@id, 'rating')]").InnerText.Contains("Рейтинг фильма"))
					{
						rating = element.SelectSingleNode(".//span[starts-with(@id, 'rating')]//i//u//text()").InnerText.Split('&').First(); //если равен нулю то это значит, что недостаотчно голосов
						votes = element.SelectSingleNode(".//span[starts-with(@id, 'rating')]//i//u//b//text()").InnerText.Split(';').Last();

                        if (rating.Replace(" ", string.Empty) == "") rating = "-";
                    }
					else if (element.SelectSingleNode(".//span[starts-with(@class, 'ajax_rating await_rating')]") != null)
					{
						waitRating = element.SelectSingleNode(".//span[@class='ajax_rating await_rating']//i//u//text()").InnerText.Split('&').First(); //если ноль то недостаточно голосов
						votes = element.SelectSingleNode(".//span[@class='ajax_rating await_rating']//i//u//b//text()").InnerText.Split(';').Last();

						if (waitRating.Replace(" ", string.Empty) == "") waitRating = "-";
					}

					var companys = element.SelectNodes(".//div[@class='prem_day']//s[@class='company']");
					foreach (var comp in companys)
					{
						company += comp.InnerText + " ";
					}

					var names_eng = element.SelectNodes(".//div[@class='text']//div[@class='textBlock']//span");
					for (int i = 0; i < names_eng.Count; i++)
					{
						if (i == 1)
						{
							name_english = names_eng[i].InnerText;
						}
						if (i == 3)
						{
							stringGenres = names_eng[i].InnerText.Split(',');
						}
					}

					films.Add(new Film(name.InnerText, name_english, link, rating, waitRating, Convert.ToInt32(votes), date, company, stringGenres));
				}
			}
			else Console.WriteLine("Ошибка");

			return films;

		}
		catch (ApplicationException)
		{
			Console.WriteLine("Серъезная ошибка");
			return films;
		}
	}
}

class Film
{
    public string name { get; set; }
    public string name_eng { get; set; }
	public string film_link { get; set; }
    public string film_rating { get; set; }
    public string wait_rating { get; set; }
    public int votes { get; set; }
    public string date { get; set; }
    public string company { get; set; }
    public string[] genres { get; set; }

    public Film(string name, string name_eng, string film_link, string film_rating, string wait_rating, int votes, string date, string company, string[] genres)
    {
        this.name = name;
        this.name_eng = name_eng;
		this.film_link = film_link;
        this.film_rating = film_rating;
        this.wait_rating = wait_rating;
        this.votes = votes;
        this.date = date;
        this.company = company;
        this.genres = genres;
    }
}


public class Rootobject
{
    public Datum[] data { get; set; }
}
public class Datum
{
    public string name { get; set; }
    public string name_eng { get; set; }
    public string film_link { get; set; }
    public string film_rating { get; set; }
    public string wait_rating { get; set; }
    public int votes { get; set; }
    public string date { get; set; }
    public string company { get; set; }
    public string[] genres { get; set; }
}