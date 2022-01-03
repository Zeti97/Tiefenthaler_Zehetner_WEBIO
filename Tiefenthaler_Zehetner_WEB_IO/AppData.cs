using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiefenthaler_Zehetner_WEB_IO
{
    public class AppData
    {
        #region members
        private string _appName;
        private string _category;
        private int _rating;
        private int _reviews;
        private double _size;
        private long _install;
        private EnumType.TypeSelection _type;
        private double _price;
        private string _content;
        private string _genres;
        private DateTime _lastUpdated;
        private string _currentVersion;
        private string _androidVersion;
        #endregion
        #region properties
        public string AppName
        {
            get
            {
                return _appName;
            }
            private set
            {
                _appName = value;
            }
        }
        public string Category
        {
            get
            {
                return _category;
            }
            private set
            {
                _category = value;
            }
        }
        public int Rating
        {
            get
            {
                return _rating;
            }
            private set
            {
                _rating = value;
            }
        }
        public int Reviews
        {
            get
            {
                return _reviews;
            }
            private set
            {
                _reviews = value;
            }
        }
        public double Size
        {
            get
            {
                return _size;
            }
            private set
            {
                _size = value;
            }
        }
        public long Install
        {
            get
            {
                return _install;
            }
            private set
            {
                _install = value;
            }
        }
        public EnumType.TypeSelection Type
        {
            get
            {
                return _type;
            }
            private set
            {
                _type = value;
            }
        }
        public double Price
        {
            get
            {
                return _price;
            }
            private set
            {
                _price = value;
            }
        }
        public string Content
        {
            get
            {
                return _content;
            }
            private set
            {
                _content = value;
            }
        }
        public string Genres
        {
            get
            {
                return _genres;
            }
            private set
            {
                _genres = value;
            }
        }
        public DateTime LastUpdated
        {
            get
            {
                return _lastUpdated;
            }
            private set
            {
                _lastUpdated = value;
            }
        }
        public string CurrentVersion
        {
            get
            {
                return _currentVersion;
            }
            private set
            {
                _currentVersion = value;
            }
        }
        public string AndroidVersion
        {
            get
            {
                return _androidVersion;
            }
            private set
            {
                _androidVersion = value;
            }
        }
        #endregion
        #region constructor
        public AppData()
        {

        }
        public AppData(string appName, string category, int rating, int reviews, double size, long install, EnumType.TypeSelection type, double price, string content, string genres, DateTime lastUpdated, string currentVersion, string androidVersion)
        {
            AppName = appName;
            Category = category;
            Rating = rating;
            Reviews = reviews;
            Size = size;
            Install = install;
            Type = type;
            Price = price;
            Content = content;
            Genres = genres;
            LastUpdated = lastUpdated;
            CurrentVersion = currentVersion;
            AndroidVersion = androidVersion;
        }    
        #endregion
        #region methods
        public static AppData ReadDataLine(string dataLine, char seperator, out bool readOfDataSuccesfull)
        {
            readOfDataSuccesfull = false; //gehört beschrieben
            string[] parts = dataLine.Split(seperator);

            //convert stringdata to correct Datatype
            string appName = parts[0];
            string category = parts[1];
            int rating = int.Parse(parts[2]);
            int reviews = int.Parse(parts[3]);
            double size = 0.0;//ConvertSizeToDouble(parts[4]);  // passt noch nicht
            long install = 0;
            if(parts[5].EndsWith('+')) //Passt noch nicht
            {
                //string rowInstall = (parts[5].Remove(parts[5].Length - 1)).r);
                //install = long.Parse(rowInstall);
            }
            else
            {
               // throw new Exception("Install input doesn't ends with an \"+\"");
                readOfDataSuccesfull = false; 
            }
            EnumType.TypeSelection type = (EnumType.TypeSelection)Enum.Parse(typeof(EnumType.TypeSelection), parts[6]);
            double price = double.Parse(parts[7]);
            string content = parts[8];
            string genres = parts[9];
            DateTime lastUpdated = DateTime.Parse(parts[10]);
            string currentVersion = parts[10];
            string androidVersion = parts[11];

            //create new object of class AppData
            AppData readData = new(appName, category, rating, reviews, size, install, type, price, content, genres, lastUpdated, currentVersion, androidVersion);
            return readData;
        }
        public static double ConvertSizeToDouble(string inputSize)
        {
            double convertedSize;
            if(inputSize.Trim() == "Varies with device")
            {
                convertedSize = -100;
            }
            if(inputSize.EndsWith('k'))
            {
                convertedSize = double.Parse(inputSize.Remove(inputSize.Length - 1), System.Globalization.NumberFormatInfo.InvariantInfo);
                _ = convertedSize / 1000;
            }
            if(inputSize.EndsWith('M'))
            {
                convertedSize = double.Parse(inputSize.Remove(inputSize.Length - 1), System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            else
            {
                throw new Exception("Convertion of Input wasn't possible.");
            }
            return convertedSize;
        }
        public string DataToCsvLine(char seperator)
        {
            string csvLine =
                AppName + seperator + Category + seperator + Rating + seperator + Reviews + seperator + Size + seperator + Install + seperator + Type + seperator + Price + seperator + Content + seperator + Genres + seperator + LastUpdated.ToShortDateString() + seperator + CurrentVersion + seperator + AndroidVersion;


            return csvLine;
        }
        #endregion
    }
}
