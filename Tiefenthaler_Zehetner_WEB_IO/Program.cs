using System;
using System.Collections.Generic;

namespace Tiefenthaler_Zehetner_WEB_IO
{
    class Program
    {
        static void Main(string[] args)
        {
            List<AppData> filteredData = new List<AppData>();
            filteredData = FilterAndReadData();

            WriteFilteredDataToConsole(filteredData);

            Console.ReadKey();
        }
        static List<AppData> FilterAndReadData()
        {
            //Initialize Data Paths
            string pathHealthFitness = "https://fhwels.s3.eu-central-1.amazonaws.com/PRO1UE_WS21/HealthFitnessApps.CSV";
            string pathPhotography = "https://fhwels.s3.eu-central-1.amazonaws.com/PRO1UE_WS21/PhotographyApps.CSV";
            string pathWeather = "https://fhwels.s3.eu-central-1.amazonaws.com/PRO1UE_WS21/WeatherApps.CSV";

            //Load Data
            List<AppData> listHealthFitness = new List<AppData>();
            List<AppData> listPhotography = new List<AppData>();
            List<AppData> listWeather = new List<AppData>();
            List<AppData> listForFilter = new List<AppData>();
            List<AppData> filterdData = new List<AppData>();

            listHealthFitness = DataLoader.LoadDataFromWeb(pathHealthFitness, ';', out int errorHealthAndFitness);
            listPhotography = DataLoader.LoadDataFromWeb(pathPhotography, ';', out int errorPhotography);
            listWeather = DataLoader.LoadDataFromWeb(pathWeather, ';', out int errorWeather);

            //Generate List For Filter
            Console.WriteLine("Wollen Sie über alle Daten Filtern? J/N");
            bool filterOverallDataSelection = AskYesOrNo();
            int categoryNumber = 0;

            if(filterOverallDataSelection == true)
            {
                listForFilter.AddRange(listHealthFitness);
                listForFilter.AddRange(listPhotography);
                listForFilter.AddRange(listWeather);
            }

            else if(filterOverallDataSelection == false)
            {
                if(categoryNumber == 1)
                {
                    listForFilter = listHealthFitness;
                }
                if (categoryNumber == 2)
                {
                    listForFilter = listPhotography;
                }
                if (categoryNumber == 3)
                {
                    listForFilter = listWeather;
                }
                categoryNumber = AskAndCheckCategoryNumber();
            }

            //Ask Filter Type
            int filterTypeNumber = AskFilterType();

            //AskIfMinOrMaxValue
            Console.WriteLine("Wollen Sie auf Minimum oder Maximum Filtern? Max/Min");
            bool minOrMax = AskMinOrMax();

            //Ask Filter Value
            int filterValue = AskFilterValue();

            //Filter Data
            filterdData = DataLoader.FilterAppData(listForFilter, filterTypeNumber, minOrMax, filterValue);

            //Ask if also other filter
            bool filterAgain = false;
            do
            {
                Console.WriteLine("Wollen Sie zusätzlich auf eine andere Kategorie filtern? J/N");
                filterAgain = AskYesOrNo();

                if (filterAgain == true)
                {
                    filterTypeNumber = AskFilterType();

                    listForFilter.Clear();
                    listForFilter = filterdData;
                    filterdData = DataLoader.FilterAppData(listForFilter, filterTypeNumber, minOrMax, filterValue);
                }
            }
            while (filterAgain);

            return filterdData;

        }
        static int AskAndCheckCategoryNumber()
        {
            bool categorySelectionInCorrectFormat = false;
            int categoryNumber;
            do
            {
                Console.WriteLine("Über welche Kategorie wollen Sie filtern? \n" +
                    "1...Gesundheit und Fitness\n" +
                    "2...Fotografie\n" +
                    "3...Wetter");
                string rowCategorySelection = Console.ReadLine();

                bool conversationOfCategorySelectionOk = int.TryParse(rowCategorySelection, out categoryNumber);

                if (conversationOfCategorySelectionOk && categoryNumber > 0 && categoryNumber <= 3)
                {
                    categorySelectionInCorrectFormat = true;
                }
                else Console.WriteLine("Daten wurden im falschen Format eingegeben versuchen Sie es erneut!");
            }
            while (!categorySelectionInCorrectFormat);

            return categoryNumber;
        }
        static bool AskYesOrNo()
        {
            bool filterOverAllDataInCorrectFormat = false;
            bool filterOverallDataSelection = false;

            do
            {
                string rowFilterOverAllData = Console.ReadLine();

                if (rowFilterOverAllData.ToLower() == "j")
                {
                    filterOverallDataSelection = true;

                }
                if (rowFilterOverAllData.ToLower() == "n" || filterOverallDataSelection == true)
                {
                    filterOverAllDataInCorrectFormat = true;
                }
                else Console.WriteLine("Daten wurden im falschen Format eingegeben versuchen Sie es erneut!");
            }
            while (!filterOverAllDataInCorrectFormat);

            return filterOverallDataSelection;
        } //Bezeichnungen in Methode Ändern
        static int AskFilterType()
        {
            bool filterTypeInCorrectFormat = false;
            int fitlerTypeNumber;
            do
            {
                Console.WriteLine("Über welches Kriterium wollen Sie filtern? \n" +
                    "1...Preis\n" +
                    "2...Rezensionen\n" +
                    "3...Dateigröße");
                string rowTypeNumber = Console.ReadLine();

                bool conversationOfTypeNumberOk = int.TryParse(rowTypeNumber, out fitlerTypeNumber);

                if (conversationOfTypeNumberOk && fitlerTypeNumber > 0 && fitlerTypeNumber <= 3)
                {
                    filterTypeInCorrectFormat = true;
                }
                else Console.WriteLine("Daten wurden im falschen Format eingegeben versuchen Sie es erneut!");
            }
            while (!filterTypeInCorrectFormat);

            return fitlerTypeNumber;
        }
        static bool AskMinOrMax () //Bezeichnungen in Methode Ändern
        {
            bool filterOverAllDataInCorrectFormat = false;
            bool filterOverallDataSelection = false;

            do
            {
                string rowFilterOverAllData = Console.ReadLine();

                if (rowFilterOverAllData.ToLower() == "max")
                {
                    filterOverallDataSelection = true;

                }
                if (rowFilterOverAllData.ToLower() == "min" || filterOverallDataSelection == true)
                {
                    filterOverAllDataInCorrectFormat = true;
                }
                else Console.WriteLine("Daten wurden im falschen Format eingegeben versuchen Sie es erneut!");
            }
            while (!filterOverAllDataInCorrectFormat);

            return filterOverallDataSelection;
        }
        static int AskFilterValue()
        {
            bool filterValueInCorrectFormat = false;
            int filterValue;
            do
            {
                Console.Write("Geben Sie den gewünschten Grenzwert ein: ");

                string rowFilterValue = Console.ReadLine();

                bool conversationOfFilterValueOK = int.TryParse(rowFilterValue, out filterValue);

                if (conversationOfFilterValueOK)
                {
                    filterValueInCorrectFormat = true;
                }
                else Console.WriteLine("Daten wurden im falschen Format eingegeben versuchen Sie es erneut!");
            }
            while (!filterValueInCorrectFormat);

            return filterValue;
        }
        static void WriteFilteredDataToConsole(List<AppData> filterdData)
        {
            for (int i = 0; i < filterdData.ToArray().Length; i++)
            {
                Console.WriteLine(filterdData[i].AppName + "  " + filterdData[i].Price);
            }
        } //Ausgabe gehört noch gemacht
    }
}
