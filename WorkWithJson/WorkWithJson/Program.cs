using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class foo
{
    public static void Main(string[] args)
    {
        JObject o = JObject.Parse(@"{
          ""data"":[
          {
            ""dept"": ""Отдел информационных систем"",
            ""name"": ""Сотрудник 1"",
            ""phone"": ""89999999999""
          },
          {
            ""dept"": ""Отдел АСУ"",
            ""name"": ""Сотрудник 2"",
            ""phone"": ""88888888888""
          },
          {
            ""dept"": ""Отдел информационных систем"",
            ""name"": ""Сотрудник 3"",
            ""hours"": 165,
            ""phone"": ""87777777777""
          },
          {
            ""dept"": ""Отдел информационных систем"",
            ""name"": ""Сотрудник 4"",
            ""hours"": 132,
            ""phone"": ""86666666666""
          },
          {
            ""dept"": ""Отдел АСУ"",
            ""name"": ""Сотрудник 5"",
            ""hours"": 101,
            ""phone"": ""85555555555""
          },
          {
            ""dept"": ""Отдел информационных систем"",
            ""name"": ""Сотрудник 6"",
            ""hours"": 98,
            ""phone"": ""84444444444""
          }
        ]}");

        string json = o.ToString();
        Rootobject result = JsonConvert.DeserializeObject<Rootobject>(json);

        var s = from f in result.data
                orderby f.dept
                select f;

        int countOIS = 0;
        int countASU = 0;
        List<int> avgListHoursOIS = new List<int>();
        List<int> avgListHoursASU = new List<int>();
        foreach (var f in s)
        {
            if (f.dept == "Отдел информационных систем")
            {
                countOIS++;
                if (f.hours != 0) avgListHoursOIS.Add(f.hours);
            }
            if (f.dept == "Отдел АСУ")
            {
                countASU++;
                if (f.hours != 0) avgListHoursASU.Add(f.hours);
            }
        }
        double avgASU = avgListHoursASU.Average();
        double avgOIS = avgListHoursOIS.Average();


        List<Person> perOIS = new List<Person>();
        List<Person> perASU = new List<Person>();
        foreach (var f in s)
        {
            if (f.dept == "Отдел информационных систем")
                perOIS.Add(new Person { name = f.name, phone = f.phone, hours = f.hours });

            if (f.dept == "Отдел АСУ")
                perASU.Add(new Person { name = f.name, phone = f.phone, hours = f.hours });
        }

        //Новый json
        NewJson newJson = new NewJson();

        ОтделАСУ deptASU = new ОтделАСУ();
        ОтделИнформационныхСистем deptOIS = new ОтделИнформационныхСистем();

        deptASU.avg_hours = Convert.ToInt32(avgASU);
        deptOIS.avg_hours = Convert.ToInt32(avgOIS);

        deptASU.count = countASU;
        deptOIS.count = countOIS;

        deptASU.people = perASU.ToArray();
        deptOIS.people = perOIS.ToArray();

        newJson.ОтделАСУ = deptASU;
        newJson.Отделинформационныхсистем = deptOIS;

        JsonSerializer serializer = new JsonSerializer();

        using (StreamWriter sw = new StreamWriter(@"c:\json.txt"))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            serializer.Serialize(writer, newJson);
        }

        string newJsonString = null;


        NewJson finalResult;
        using (StreamReader file = File.OpenText(@"c:\json.txt"))
        {
            finalResult = (NewJson)serializer.Deserialize(file, typeof(NewJson));
        }


        Console.WriteLine("Отдел АСУ");
        foreach (Person per in finalResult.ОтделАСУ.people)
        {
            Console.WriteLine(per.name + " " + per.phone + " " + per.hours);
        }

        Console.WriteLine("\nОтдел информационных систем");
        foreach (Person per in finalResult.Отделинформационныхсистем.people)
        {
            Console.WriteLine(per.name + " " + per.phone + " " + per.hours);
        }
    }


    public class Rootobject
    {
        public Datum[] data { get; set; }
    }
    public class Datum
    {
        public string dept { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public int hours { get; set; }
        public int count { get; set; }
    }



    public class NewJson
    {
        public ОтделАСУ ОтделАСУ { get; set; }
        public ОтделИнформационныхСистем Отделинформационныхсистем { get; set; }
    }

    public class ОтделАСУ
    {
        public int count { get; set; }
        public int avg_hours { get; set; }
        public Person[] people { get; set; }
    }

    public class Person
    {
        public string name { get; set; }
        public string phone { get; set; }
        public int hours { get; set; }
    }

    public class ОтделИнформационныхСистем
    {
        public int count { get; set; }
        public int avg_hours { get; set; }
        public Person[] people { get; set; }
    }
}
