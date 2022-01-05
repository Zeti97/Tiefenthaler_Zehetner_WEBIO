using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiefenthaler_Zehetner_WEB_IO
{
    public class AppData    //Tiefenthaler
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
        public static AppData ReadDataLine(string dataLine, char seperator, out bool readOfDataSuccesfull)//Tiefenthaler
        {
            readOfDataSuccesfull = false;
            AppData readData = new();
            string[] parts = dataLine.Split(seperator);

            //convert stringdata to correct Datatype
            string appName = parts[0];
            string category = parts[1];
            bool successfulRating = int.TryParse(parts[2], out int rating);
            bool successfulReviews = int.TryParse(parts[3], out int reviews);
            double size = ConvertSizeToDouble(parts[4], out bool successfulSize);
            bool successfulInstall = long.TryParse(parts[5], System.Globalization.NumberStyles.Number,
                                                   System.Globalization.NumberFormatInfo.InvariantInfo,
                                                   out long install);
            EnumType.TypeSelection type;
            bool successfulType = Enum.TryParse<EnumType.TypeSelection>(parts[6], out type);
            bool successfulPrice = double.TryParse(parts[7], out double price);
            string content = parts[8];
            string genres = parts[9];
            bool successfulLastUpdated = DateTime.TryParse(parts[10], out DateTime lastUpdated);
            string currentVersion = parts[10];
            string androidVersion = parts[11];

            readOfDataSuccesfull = successfulRating && successfulReviews &&
                                   successfulSize && successfulInstall && successfulType &&
                                   successfulPrice && successfulLastUpdated;

            //create new object of class AppData
            if (readOfDataSuccesfull)
            {
                readData = new(appName, category, rating, reviews, size,
                               install, type, price, content, genres,
                               lastUpdated, currentVersion, androidVersion);
            }
            return readData;
        }
        public static double ConvertSizeToDouble(string inputSize, out bool converssionSuccess)//Tiefenthaler
        {
            double convertedSize = 0;
            converssionSuccess = false;
            if (inputSize.Trim() == "Varies with device")
            {
                convertedSize = -100;
                converssionSuccess = true;
            }
            if (inputSize.EndsWith('k'))
            {
                converssionSuccess = double.TryParse(inputSize.Remove(inputSize.Length - 1),
                                                     System.Globalization.NumberStyles.Number,
                                                     System.Globalization.NumberFormatInfo.InvariantInfo,
                                                     out double readSize);
                convertedSize = readSize / 1000;
            }
            if (inputSize.EndsWith('M'))
            {
                converssionSuccess = double.TryParse(inputSize.Remove(inputSize.Length - 1),
                                                     System.Globalization.NumberStyles.Number,
                                                     System.Globalization.NumberFormatInfo.InvariantInfo,
                                                     out convertedSize);
            }
            return convertedSize;
        }
        public string DataToCsvLine(char seperator)//Tiefenthaler
        {
            string csvLine =
                AppName + seperator + Category + seperator + Rating + seperator +
                Reviews + seperator + ConvertSizeToString(Size) + seperator + 
                Install.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "+" + seperator +
                Type + seperator + Price.ToString(System.Globalization.NumberFormatInfo.CurrentInfo) + seperator + 
                Content + seperator + Genres + seperator + LastUpdated.ToShortDateString() + seperator +
                CurrentVersion + seperator + AndroidVersion;
            return csvLine;
        }
        public string CreateLineForConsole()//Tiefenthaler
        {
            string lineForConsole = AppName.PadRight(82) + Category.PadRight(20) + Rating.ToString().PadRight(4) + 
                                    Reviews.ToString(System.Globalization.NumberFormatInfo.CurrentInfo).PadRight(10) +
                                    ConvertSizeToString(Size).PadRight(6) + 
                                    Install.ToString(System.Globalization.NumberFormatInfo.CurrentInfo).PadLeft(15) + "+  " +
                                    Type.ToString().PadRight(6) + 
                                    Price.ToString(System.Globalization.NumberFormatInfo.CurrentInfo).PadRight(8) +
                                    Content.PadRight(14) + Genres.PadRight(18) + 
                                    LastUpdated.ToShortDateString().PadRight(12) +
                                    CurrentVersion.PadRight(35) + AndroidVersion.PadRight(20);
            return lineForConsole;
        }
        public static string ConvertSizeToString(double toConvertSize)//Tiefenthaler
        {
            string sizeString = "0";
            if(toConvertSize == -100)
            {
                sizeString = "Varies with device";
            }
            if(toConvertSize < 1 && toConvertSize > 0)
            {
                double sizekFormat = toConvertSize * 1000;
                sizeString = sizekFormat.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + 'k';
            }
            if(toConvertSize >= 1)
            {
                sizeString = toConvertSize.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + 'M';
            }
            return sizeString;
        }
        #endregion
    }
}
