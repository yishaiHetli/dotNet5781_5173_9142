using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DS
{
    public static class DataSource
    {
        public static List<Bus> buss;  // 20
        public static List<BusStation> busSta; // 50
        public static List<BusLine> busLine; // 10
        public static List<LineStation> lineSta; // 10 for each busLine 
        public static List<PairStations> pairSta;
        static Random rnd = new Random();

        static DataSource()
        {
            InitAllLists();
        }
        
        static void InitAllLists()
        {
            #region Bus
            buss = new List<Bus>();
            bool exsit;
            int licenseNum;
            for (int i = 0; i < 22; ++i)
            {
                exsit = false;
                licenseNum = rnd.Next(1000000, 100000000);
                for (int j = 0; j < i; ++j)
                    if (buss[j].LicenseNum == licenseNum)
                    {
                        --i;
                        exsit = true;
                        break;
                    }
                if (exsit)
                    continue;
                DateTime start = new DateTime(1985, 1, 1);
                DateTime end = new DateTime(2018, 1, 1);
                if (licenseNum.ToString().Length > 7)
                {
                    start = end;
                    end = DateTime.Today;
                }
                end = randomDate(start, end);
                buss.Add(new Bus
                {
                    LicenseNum = licenseNum,
                    StartActivity = end,
                    FuelInKm = rnd.Next(0, 1200),
                    TotalKm = rnd.Next(0, 1000000),
                    BusStatus = Status.READY
                });
            }
            #endregion
            #region Bus stations
            List<string> streets = new List<string> {
"Bar Lev / Ben Yehuda School"
,"Herzl / Bilu Junction"
,"The surge / fishermen"
,"Fried / The Six Days"
,"A. Lod Central / Download"
,"Hannah Avrech / Vulcani"
,"Herzl / Moshe Sharet"
,"The boys / Eli Cohen"
,"Weizmann / The Boys"
,"The iris / anemone"
,"The anemone / daffodil"
,"Eli Cohen / Ghetto Fighters"
,"Shabazi / Shabbat brothers"
,"Shabazi / Weizmann"
,"Haim Bar Lev / Yitzhak Rabin Boulevard"
,"Lev Hasharon Mental Health Center"
,"Lev Hasharon Mental Health Center"
,"Holtzman / Science"
,"Zrifin Camp / Club"
,"Herzl / Golani"
,"Rotem / Deganiot"
,"The prairie"
,"Introduction to the vine / Slope of the fig"
,"Introduction to the vine / extension"
,"The extension a"
,"The extension b"
,"The extension / veterans"
,"Airports / Aliyah Authority"
,"Wing / Cypress"
,"The gang / Dov Hoz"
,"Beit Halevie"
,"First / Route 5700"
,"The genius Ben Ish Chai / Ceylon"
,"Okashi / Levi Eshkol"
,"Rest and estate / Yehuda Gorodisky"
,"Gorodsky / Yechiel Paldi"
,"Derech Menachem Begin / Yaakov Hazan"
,"Through the Park / Rabbi Neria"
,"The fig / vine"
,"The fig / oak"
,"Through the Flowers / Jasmine"
,"Yitzhak Rabin / Pinchas Sapir"
,"Menachem Begin / Yitzhak Rabin"
,"Haim Herzog / Dolev"
,"Shades / Cedar School"
,"Through the trees / oak"
,"Through the Trees / Menachem Begin"
,"Independence / Weizmann"
,"Weizmann / The Magic Rug"
,"Tzala / Coral"
,"Hatzav / Tzala Garden"
,"Pines / Levinson"
,"Feinberg / Schachwitz"
,"Ben Gurion / Fox"
,"Levi Eshkol / Rabbi David Israel"
,"Lily / Oppenheimer"
,"Rabbi David Israel / Arie Dolcin"
,"Kronenberg / Crimson"
,"Jacob Freiman / Benjamin Shmotkin"
,"Rabbi David Israel / Arie Dolcin"
,"Kronenberg / Crimson"
,"Jacob Freiman / Benjamin Shmotkin"
};
            List<double> lon = new List<double>{
 34.917806,34.819541,34.790904,34.782828,34.898098
,34.796071,34.824106,34.821857,34.822237,34.818957
,34.818392,34.827023,34.828702,34.827102,34.763896
,34.912708,34.912602,34.807944,34.836363,34.825249
,34.81249,34.910842,34.948647,34.943393,34.940529
,34.939512,34.938705,34.8976,34.879725,34.818708
,34.926837,34.899465,34.775083,34.807039,34.816752
,34.823461,34.904907,34.878765,34.859437,34.864555
,34.784347,34.778239,34.782985,34.785069,34.786735
,34.786623,34.785098,34.782252,34.779753,34.787199
,34.786055,34.777574,34.775928,34.773504,34.805494
,34.805809,34.80506,34.81138,34.813355,34.789445
};
            List<double> lat = new List<double> {
 32.183921
,31.870034
,31.984553
,31.88855
,31.956392
,31.892166
,31.857565
,31.862305
,31.865085
,31.865222
,31.867597
,31.86244
,31.863501
,31.865348
,31.977409
,32.300345
,32.301347
,31.914255
,31.963668
,31.856115
,31.874963
,32.300035
,32.305234
,32.304022
,32.302957
,32.300264
,32.298171
,31.990876
,31.998767
,31.883019
,32.349776
,32.352953
,31.897286
,31.883941
,31.896762
,31.898463
,32.076535
,32.299994
,31.865457
,31.866772
,31.809325
,31.80037
,31.799224
,31.800334
,31.802319
,31.804595
,31.805041
,31.816751
,31.816579
,31.801182
,31.802279
,31.814676
,31.813285
,31.806959
,31.884187
,31.910118
,31.882474
,31.878667
,31.975479
,31.982177
        };
            List<int> staKey = new List<int> {
 38831
,38832
,38833
,38834
,38836
,38837
,38838
,38839
,38840
,38841
,38842
,38844
,38845
,38846
,38847
,38848
,38849
,38852
,38854
,38855
,38856
,38859
,38860
,38861
,38862
,38863
,38864
,38865
,38866
,38867
,38869
,38870
,38872
,38873
,38875
,38876
,38877
,38878
,38879
,38880
,38881
,38883
,38884
,38885
,38886
,38887
,38888
,38889
,38890
,38891
,38892
,38893
,38894
,38895
,38898
,38899
,38901
,38903
,38904
,66787
,85678
,38901
,38903
,38904
,66787
,85678
        };
            busSta = new List<BusStation>();
            pairSta = new List<PairStations>();
            for (int i = 0; i < 60; ++i)
            {
                busSta.Add(new BusStation
                {
                    BusStationKey = staKey[i],
                    Latitude = lat[i],
                    Longitude = lon[i],
                    Name = streets[i],
                    
                });
                if (i > 0 && Math.Abs(busSta[i].BusStationKey - busSta[i - 1].BusStationKey) == 1)
                {
                    var sCoord = new GeoCoordinate(busSta[i].Latitude, busSta[i].Longitude);
                    var eCoord = new GeoCoordinate(busSta[i - 1].Latitude, busSta[i - 1].Longitude);
                    double cord = sCoord.GetDistanceTo(eCoord) / 1000;
                    pairSta.Add(new PairStations
                    {
                        FirstKey = busSta[i - 1].BusStationKey,
                        SecondKey = busSta[i].BusStationKey,
                        Distance = cord,
                        AverageTime = TimeSpan.FromMinutes(cord)
                    });
                }
            }
            lon.Clear();
            lat.Clear();
            streets.Clear();
            staKey.Clear();
            #endregion
            #region Lines
            busLine = new List<BusLine>();
            lineSta = new List<LineStation>();
            for (int i = 0, g = 0; i < 10; ++i)
            {
                busLine.Add(new BusLine
                {
                    LineID = BusLine.ID++,
                    LineNumber = rnd.Next(1, 1000),
                    Place = (Area)rnd.Next(0, 5),
                    FirstStation = busSta[g].BusStationKey,
                    LastStation = 0
                });
                for (int n = 0; n < 10; ++n, ++g)
                {
                    lineSta.Add(new LineStation
                    {
                        BusStationKey = busSta[g].BusStationKey,
                        LineNum = busLine[i].LineNumber,
                        LIneStationIndex = n
                    });
                }
                busLine[i].LastStation = busSta[g - 1].BusStationKey;
                g -= 5;
            }
            #endregion

        }

        static DateTime randomDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(rnd.Next(range));
        }
    }
}


