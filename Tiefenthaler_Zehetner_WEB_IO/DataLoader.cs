using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tiefenthaler_Zehetner_WEB_IO
{
    public class DataLoader
    {
        #region members

        #endregion


        #region methoden
        public static List<AppData> LoadDataFromWeb(string path,char seperator, out int error)
        {
            List<AppData> appDataList = new List<AppData>();
            WebClient client = new WebClient();
            Stream contentStream = client.OpenRead(path);    //new Streamreader() -> stream
            error = 0;
            int counter = 0;

            try
            {
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    while (reader.Peek() != -1)
                    {
                        string line = reader.ReadLine();
                        if (counter > 0)
                        {
                            AppData newDataLine = AppData.ReadDataLine(line, seperator, out bool readSuccessfull);

                            if (readSuccessfull)
                            {
                                appDataList.Add(newDataLine);
                            }
                        }
                        counter++;
                    }

                }
            }
            catch (ArgumentNullException aex)
            {
                error = 1;
                //Console.WriteLine("path was null (empty)");
            }
            catch (DirectoryNotFoundException dex)
            {
                error = 3;
                //Console.WriteLine("Folder not found");
            }
            catch (FileNotFoundException fex)
            {
                error = 8;
                //Console.WriteLine("Path too Long");
            }
            catch (IOException iex)
            {
                error = 5;
                //Console.WriteLine("File interaction error");
            }
            catch (ArgumentException aex)    //case: ArgumentExeption
            {
                error = 6;
                //Console.WriteLine("path invalid");
            }
            catch (Exception)
            {
                error = 99;
            }
            return appDataList;
        }
        public static List<AppData> FilterAppData(List <AppData> appData,int filterCategory,bool minOrMaxFilter,int filterValue)
        {
            //FilterCategory
            //1...Price
            //2...Reviews
            //3...Size

            //minOrMaxFilter
            //false...min
            //true...max
            List<AppData> filterdAppData = new List<AppData>();
            List<AppData> orderedAppData = new List<AppData>();
   
                for (int i = 0; i < appData.ToArray().Length; i++)
                {
                //Price Filter
                    if (filterCategory == 1)
                    {
                        //Min Value ==> all Values over this value;
                        if (appData[i].Price >= filterValue && minOrMaxFilter == false)
                        {
                            filterdAppData.Add(appData[i]);
                        }
                        //Max Value ==> all Values under this value;
                        if (appData[i].Price <= filterValue && minOrMaxFilter == true)
                        {
                            filterdAppData.Add(appData[i]);
                        }
                    }
                //Review Filter
                if (filterCategory == 2)
                {
                    //Min Value ==> all Values over this value;
                    if (appData[i].Reviews >= filterValue && minOrMaxFilter == false)
                    {
                        filterdAppData.Add(appData[i]);
                    }
                    //Max Value ==> all Values under this value;
                    if (appData[i].Reviews <= filterValue && minOrMaxFilter == true)
                    {
                        filterdAppData.Add(appData[i]);
                    }
                }
                //Size Filter
                if (filterCategory == 3)
                {
                    //Min Value ==> all Values over this value;
                    if (appData[i].Size >= filterValue && minOrMaxFilter == false)
                    {
                        filterdAppData.Add(appData[i]);
                    }
                    //Max Value ==> all Values under this value;
                    if (appData[i].Size <= filterValue && minOrMaxFilter == true)
                    {
                        filterdAppData.Add(appData[i]);
                    }
                }
            }

            orderedAppData = filterdAppData.OrderBy(filterdAppData => filterdAppData.AppName).ToList();
            return orderedAppData;

        }
        #endregion
    }
}
