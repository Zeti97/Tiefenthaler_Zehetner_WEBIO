using System;
using System.Collections.Generic;

namespace Tiefenthaler_Zehetner_WEB_IO
{
    class Program
    {
        static void Main(string[] args)
        {
            List<AppData> filteredData = new List<AppData>();

            List<AppData> listHealthFitness = new List<AppData>();
            List<AppData> listPhotography = new List<AppData>();
            List<AppData> listWeather = new List<AppData>();

            //Read Data
            ReadData(out listHealthFitness, out listPhotography, out listWeather);

            //Filter Data
            filteredData = FilterData(listHealthFitness, listPhotography, listWeather);

            //Write Filterd Data to Console
            WriteFilteredDataToConsole(filteredData);

            Console.ReadKey();
        }
        static void ReadData(out List<AppData> HealthFitness, out List<AppData> Photography, out List<AppData> Weather)//Zehetner
        {
            //Initialize Data Paths
            string pathHealthFitness = "https://fhwels.s3.eu-central-1.amazonaws.com/PRO1UE_WS21/HealthFitnessApps.CSV";
            string pathPhotography = "https://fhwels.s3.eu-central-1.amazonaws.com/PRO1UE_WS21/PhotographyApps.CSV";
            string pathWeather = "https://fhwels.s3.eu-central-1.amazonaws.com/PRO1UE_WS21/WeatherApps.CSV";

            //Health and Fitness
            HealthFitness = DataLoader.LoadDataFromWeb(pathHealthFitness, ';', out int errorHealthAndFitness, out int invalidDataLinesHealthFitness);


            if (errorHealthAndFitness > 0)
            {
                Console.WriteLine("Es ist ein unerwarteter Fehler beim Lesen der Fitness Apps aufgetretten,\n" +
                    "Es konnten möglicherweise nicht alle Daten gelesen werden!\n" +
                    "Fehler: ");
            }
            ErrorHandlingStream(errorHealthAndFitness);

            if (invalidDataLinesHealthFitness > 0)
            {
                Console.WriteLine("Es konnten " + invalidDataLinesHealthFitness + " Zeilen der Fitness Apps nicht geladen werden!\n");
            }

            //Photography
            Photography = DataLoader.LoadDataFromWeb(pathPhotography, ';', out int errorPhotography, out int invalidDataLinesPhotography);
           
            if (errorPhotography > 0)
            {
                Console.WriteLine("Es ist ein unerwarteter Fehler beim Lesen der Fotografie Apps aufgetretten,\n" +
                    "es konnten möglicherweise nicht alle Daten gelesen werden!\n" +
                    "Fehler: ");
            }
            ErrorHandlingStream(errorPhotography);

            if (invalidDataLinesPhotography > 0)
            {
                Console.WriteLine("Es konnten " + invalidDataLinesPhotography + " Zeilen der Fotografie Apps nicht geladen werden!\n");
            }

            //Weather
            Weather = DataLoader.LoadDataFromWeb(pathWeather, ';', out int errorWeather, out int invalidDataLinesWeather);
             if (errorWeather > 0)
            {
                Console.WriteLine("\nEs ist ein unerwarteter Fehler beim Lesen der Wetter Apps aufgetretten,\n" +
                    "es konnten möglicherweise nicht alle Daten gelesen werden!\n" +
                    "Fehler: ");
            }
            ErrorHandlingStream(errorWeather);

            if (invalidDataLinesWeather > 0)
            {
                Console.WriteLine("Es konnten " + invalidDataLinesWeather + " Zeilen der Wetter Apps nicht geladen werden!\n");
            }
        }
        static List<AppData> FilterData(List<AppData> listHealthFitness,List<AppData> listPhotography,List<AppData> listWeather)//Zehetner
        {
            List<AppData> listForFilter = new List<AppData>();
            List<AppData> filterdData = new List<AppData>();

            //Generate List For Filter
            Console.WriteLine("\nWollen Sie über alle Daten Filtern? J/N");
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
            bool minOrMax = AskMinOrMax();

            //Ask Filter Value
            int filterValue = AskFilterValue();

            //Filter Data
            filterdData = DataLoader.FilterAppData(listForFilter, filterTypeNumber, minOrMax, filterValue);

            //Ask if also other filter
            bool filterAgain = false;
            do
            {
                Console.WriteLine("\nWollen Sie zusätzlich auf eine andere Kategorie filtern? J/N");
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
        static int AskAndCheckCategoryNumber()//Zehetner
        {
            bool categorySelectionInCorrectFormat = false;
            int categoryNumber;
            do
            {
                Console.WriteLine("\nÜber welche Kategorie wollen Sie filtern? \n" +
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
        static bool AskYesOrNo()//Zehetner
        {
            bool inputIsinCorrectDataFormat = false;
            bool yesOrNo = false;

            do
            {
                string rowYesOrNo = Console.ReadLine();

                if (rowYesOrNo.ToLower() == "j")
                {
                    yesOrNo = true;

                }
                if (rowYesOrNo.ToLower() == "n" || yesOrNo == true)
                {
                    inputIsinCorrectDataFormat = true;
                }
                else Console.WriteLine("Daten wurden im falschen Format eingegeben versuchen Sie es erneut!");
            }
            while (!inputIsinCorrectDataFormat);

            return yesOrNo;
        }
        static int AskFilterType()//Zehetner
        {
            bool filterTypeInCorrectFormat = false;
            int fitlerTypeNumber;
            do
            {
                Console.WriteLine("\nÜber welches Kriterium wollen Sie filtern? \n" +
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
        static bool AskMinOrMax ()//Zehetner
        {
            bool inputDataInCorrectDataFormat = false;
            bool minOrMax = false;

            do
            {
                Console.WriteLine("\nWollen Sie auf Minimum oder Maximum Filtern? Max/Min");
                string rowMinOrMax = Console.ReadLine();

                if (rowMinOrMax.ToLower() == "max")
                {
                    minOrMax = true;

                }
                if (rowMinOrMax.ToLower() == "min" || minOrMax == true)
                {
                    inputDataInCorrectDataFormat = true;
                }
                else Console.WriteLine("Daten wurden im falschen Format eingegeben versuchen Sie es erneut!");
            }
            while (!inputDataInCorrectDataFormat);

            return minOrMax;
        }
        static int AskFilterValue()//Zehetner
        {
            bool filterValueInCorrectFormat = false;
            int filterValue;
            do
            {
                Console.Write("\nGeben Sie den gewünschten Grenzwert ein: ");

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
        static void WriteFilteredDataToConsole(List<AppData> filterdData)//Tiefenthaler
        {
            for (int i = 0; i < filterdData.ToArray().Length; i++)
            {
                Console.WriteLine(filterdData[i].CreateLineForConsole());
            }
        }
        static void ErrorHandlingStream(int error)//Zehetner
        {
            if (error != 0)
            {
                if (error == 1)
                {
                    Console.WriteLine("Path was null (empty)!\n");
                }
                if (error == 2)
                {
                    Console.WriteLine("Not authorized!\n");
                }
                if (error == 3)
                {
                    Console.WriteLine("Folder not found!\n");
                }
                if (error == 4)
                {
                    Console.WriteLine("Path too Long!\n");
                }
                if (error == 5)
                {
                    Console.WriteLine("File interaction error!\n");
                }
                if (error == 6)
                {
                    Console.WriteLine("Path invalid!\n");
                }
                if (error == 7)
                {
                    Console.WriteLine("Security error!\n");
                }
                if (error == 8)
                {
                    Console.WriteLine("File not found!\n");
                }
                if (error == 99)
                {
                    Console.WriteLine("Unknown Error!\n");
                }
            }
        }

        //Daten in Datei laden
    }
}
