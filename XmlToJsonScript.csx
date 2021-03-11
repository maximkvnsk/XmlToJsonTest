using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

        // Check if country already exist in list.
        public static bool alreadyExists(List<string> countries, string newCountry)
        {
            return countries.Any(x => x == newCountry);
        }

        // Replace '.' with "," in deserialized PRICE value, so it can be parsed to float. 
        public static string replaceDots(string inputString)
        {
            string outputString = "";
            foreach (char c in inputString)
            {
               outputString += c == '.' ? ',' : c;    
            }
            return outputString;
        }

        // Represents output data.
        public class JsonInfo
        {
            [JsonPropertyName("cdsCount")]
            public int cdsCount { get; set; }
            [JsonPropertyName("pricesSum")]
            public decimal pricesSum { get; set; }
            [JsonPropertyName("countries")]
            public List<string> countries { get; set; }
            [JsonPropertyName("minYear")]
            public int minYear { get; set; }
            [JsonPropertyName("maxYear")]
            public int maxYear { get; set; }
            public JsonInfo()
            {
                cdsCount = 0;
                pricesSum = 0;
                countries = new List<string>();
                minYear = 2021;
                maxYear = 1900;
            }
        }


            JsonInfo jsonInfo = new JsonInfo();
            XmlDocument testfile = new XmlDocument();
            while (true)
            {
            try {
            Console.WriteLine("Enter path to file");
            string filePath = Console.ReadLine(); 
            testfile.Load(filePath);
            break;
            }
           
            catch(Exception)
            { 
                Console.WriteLine("XML file not found");
            }
            }
            XmlElement catalog = testfile.DocumentElement;
            foreach (XmlNode CD in catalog)
            {
                jsonInfo.cdsCount++;
                foreach (XmlNode childnode in CD.ChildNodes)
                {
                    if (childnode.Name == "PRICE")
                    {
                        jsonInfo.pricesSum += decimal.Parse(replaceDots(childnode.InnerText));
                    }
                    if (childnode.Name == "COUNTRY")
                    {
                        if (!alreadyExists(jsonInfo.countries, childnode.InnerText))
                            jsonInfo.countries.Add(childnode.InnerText);
                    }
                    if (childnode.Name == "YEAR")
                    {
                        int year = Int32.Parse(childnode.InnerText);
                        if (year < jsonInfo.minYear)
                        {
                            jsonInfo.minYear = year;
                        } else 
                        if (year > jsonInfo.maxYear)
                        {
                            jsonInfo.maxYear = year;
                        }
                    }

                }
            }
            string json = JsonSerializer.Serialize<JsonInfo>(jsonInfo, new JsonSerializerOptions { WriteIndented = true});
            Console.WriteLine(json);
 
