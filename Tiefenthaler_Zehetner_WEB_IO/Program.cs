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

            //Write all Data to File
            string filePathOfCompletList = @"..\..\..\..\completedAppData.csv";
            filteredData.Clear();
            filteredData.AddRange(listHealthFitness);
            filteredData.AddRange(listPhotography);
            filteredData.AddRange(listWeather);
            WriteAllAppDataToFile(filteredData.ToArray(), filePathOfCompletList);
            
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
                ErrorHandlingStream(errorHealthAndFitness, "Lesen der Fitness Apps");
            }
            if (invalidDataLinesHealthFitness > 0)
            {
                WriteNumberOfInvalidLinesToConsole(invalidDataLinesHealthFitness, "Fitness");
            }

            //Photography
            Photography = DataLoader.LoadDataFromWeb(pathPhotography, ';', out int errorPhotography, out int invalidDataLinesPhotography);
            if (errorPhotography > 0)
            {
                ErrorHandlingStream(errorPhotography, "Lesen der Fotografie Apps");
            }
            if (invalidDataLinesPhotography > 0)
            {
                WriteNumberOfInvalidLinesToConsole(invalidDataLinesPhotography, "Fotografie");
            }

            //Weather
            Weather = DataLoader.LoadDataFromWeb(pathWeather, ';', out int errorWeather, out int invalidDataLinesWeather);
            if (errorWeather > 0)
            {
                ErrorHandlingStream(errorWeather, "Lesen der Wetter Apps");
            }
            if (invalidDataLinesWeather > 0)
            {
                WriteNumberOfInvalidLinesToConsole(invalidDataLinesWeather, "Wetter");
            }
        }
        static List<AppData> FilterData(List<AppData> listHealthFitness, List<AppData> listPhotography, List<AppData> listWeather)//Zehetner
        {
            List<AppData> listForFilter = new List<AppData>();
            List<AppData> filterdData = new List<AppData>();

            //Generate List For Filter
            Console.WriteLine("\nWollen Sie über alle Daten Filtern? J/N");
            bool filterOverallDataSelection = AskYesOrNo();
            int categoryNumber = 0;
           

            if (filterOverallDataSelection == true)
            {
                listForFilter.AddRange(listHealthFitness);
                listForFilter.AddRange(listPhotography);
                listForFilter.AddRange(listWeather);
            }

                
            if (filterOverallDataSelection == false)
            {
                categoryNumber = AskAndCheckCategoryNumber();
                if (categoryNumber == 1)
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
            }

            //Ask if also other filter
            bool filterAgain = false;
            int oldfilterTypeNumer = 0;
            do
            {
                //Ask Filter Type
                int filterTypeNumber = AskFilterType();

                //AskIfMinOrMaxValue
                bool minOrMax = AskMinOrMax();

                //Ask Filter Value
                double filterValue = AskFilterValue();

                //Filter Data
                if (oldfilterTypeNumer == 0)
                {
                    filterdData = DataLoader.FilterAppData(listForFilter, filterTypeNumber, minOrMax, filterValue);
                }

                else if (oldfilterTypeNumer != filterTypeNumber)
                {
                    listForFilter.Clear();
                    listForFilter = filterdData;
                    filterdData = DataLoader.FilterAppData(listForFilter, filterTypeNumber, minOrMax, filterValue);
                }

                Console.WriteLine("\nWollen Sie zusätzlich auf eine andere Kategorie filtern? J/N");
                filterAgain = AskYesOrNo();

                oldfilterTypeNumer = filterTypeNumber;
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
                    "1...Preis[Euro]\n" +
                    "2...Rezensionen[Anzahl]\n" +
                    "3...Dateigröße[MB]");
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
        static bool AskMinOrMax()//Zehetner
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
        static double AskFilterValue()//Zehetner
        {
            bool filterValueInCorrectFormat = false;
            double filterValue;
            do
            {
                Console.Write("\nGeben Sie den gewünschten Grenzwert ein: ");

                string rowFilterValue = Console.ReadLine();

                bool conversationOfFilterValueOK = double.TryParse(rowFilterValue, out filterValue);

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
            Console.WriteLine();
            if (filterdData.ToArray().Length > 0)
            {
                for (int i = 0; i < filterdData.ToArray().Length; i++)
                {
                    Console.WriteLine(filterdData[i].CreateLineForConsole());
                }
            }
            else Console.WriteLine("Es wurden keine Daten in diesem Bereich gefunden!");
        }
        static void ErrorHandlingStream(int error, string nameOfAktion)//Zehetner
        {
            if (error != 0)
            {
                Console.WriteLine("Es ist ein unerwarteter Fehler beim " + nameOfAktion + " aufgetretten,\n" +
                    "Es konnten möglicherweise nicht alle Daten gelesen werden!\n" +
                    "Fehler: ");
                switch (error)
                {
                    case 1:
                        {
                            Console.WriteLine("Path was null (empty)!\n");
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Not authorized!\n");
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Folder not found!\n");
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Path too Long!\n");
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("File interaction error!\n");
                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("Path invalid!\n");
                            break;
                        }
                    case 7:
                        {
                            Console.WriteLine("Security error!\n");
                            break;
                        }
                    case 8:
                        {
                            Console.WriteLine("File not found!\n");
                            break;
                        }
                    case 9:
                        {
                            Console.WriteLine("The access to the network wasn't available.\n");
                            break;
                        }
                    case 99:
                        {
                            Console.WriteLine("Unknown Error!\n");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Value of Error is invalid!\n");
                            break;
                        }
                }
            }
        }
        static void WriteNumberOfInvalidLinesToConsole(int counter, string typOfApp)//Tiefenthaler
        {
            string naming = "n";
            if (counter == 1)
            {
                naming = null;
            }
            Console.WriteLine("Es konnte" + naming + " " + counter + " Zeile" + naming + " der " + typOfApp + " Apps nicht geladen werden!\n");
        }
        static void WriteAllAppDataToFile(AppData[] dataArrayToWrite,  string filePath)//Tiefenthaler
        {
            DataLoader.WriteAppDataToFile(dataArrayToWrite, filePath, false, out int errorWriterHealth);
            if (errorWriterHealth > 0)
            {
                ErrorHandlingStream(errorWriterHealth, "Schreiben der Appdaten in eine Datei");
            }
        }
            
    }
}
