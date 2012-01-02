using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using Styx;
using TreeSharp;
using Action = TreeSharp.Action;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Pathing;
using Styx.Logic.Combat;
using System.Threading;
using Styx.Helpers;
using System.Diagnostics;
using Styx.Logic;
using Styx.Logic.POI;
using Styx.Logic.BehaviorTree;
using Bots.Grind;
using System.IO;

namespace MrLunarFestival2011
{
    public class MrLunarFestival2011 : BotBase
    {
        public override PulseFlags PulseFlags
        {
            get { return new PulseFlags(); }
        }

        public override string Name
        {
            get { return "Mr.LunarFestival2011"; }
        }

        private static Form configurationForm;
		public override Form ConfigurationForm
		{
			get
			{
				configurationForm = new LunarConfig();
				return configurationForm;
			}
		}

        public static double DistToHarvest
        {
            get
            {
                return LunarSettings.Instance.HarvestDistance;
            }
        }
        public static string MountName
        {
            get
            {
                return LunarSettings.Instance.MountName;
            }
        }
        public static int blackListTime = 20;//minutes
        public static List<string> blacklistMobs = new List<string>()//MUST BE LOWER CASE
        {
            "baron geddon"
        };
        static int numHarvestAttempts = 5;//number of fails before blacklisting


        static Dictionary<Stopwatch, WoWPoint> blackList = new Dictionary<Stopwatch, WoWPoint>();


        public static List<WoWPoint> profile
        {
            get
            {
                LevelbotSettings.Instance.FindVendorsAutomatically = true;
                LevelbotSettings.Instance.LearnFlightPaths = false;
                switch (LunarSettings.Instance.ProfileSelect)
                {
                    case "Eastern Kingdoms (Alliance)":
                        return Elders_EasternKingdoms;
                        break;
                    case "Kalimdor (Alliance)":
                        return Elders_Kalimdor;
                        break;
                    case "Northrend (Alliance)":
                        return Elders_Northrend;
                        break;
                }
                return Elders_EasternKingdoms;
                
            }
        }
        #region Profile

        static List<WoWPoint> Elders_EasternKingdoms = new List<WoWPoint>
       {
          new WoWPoint(-14444.71,521.8397,64.90387),
          new WoWPoint(-14426.05,533.6467,27.11354), // Elder Winterhoof, Booty Bay, Stranglethorn Vale
          new WoWPoint(-14452.5,409.1113,154.4375),
          
          new WoWPoint(-12416.45,-2213.687,409.0254),
          new WoWPoint(-11924.35,-3022.518,170.3988),
          
          new WoWPoint(-11799.46,-3154.277,4.805943),
          new WoWPoint(-11792.26,-3180.189,-28.84621), // Elder Bellowrage, Dark Portal, Blasted Lands
          new WoWPoint(-11630.3,-2939.769,256.8792),
          
          new WoWPoint(-11846.36,-2092.988,304.3647),
          new WoWPoint(-11854.64,-1620.498,186.7171),
          
          new WoWPoint(-11911.4,-1180.267,155.0313),
          new WoWPoint(-11953.41,-1169.151,79.23636),        // Elder Starglade, Zul'Gurub, Stranglethorn Vale
          new WoWPoint(-11885.04,-1139.23,224.6749),
          
          new WoWPoint(-10610.27,596.0751,331.9435),    // river between Darkshire & Westfall
          
          new WoWPoint(-10485.57,1024.18,142.198),
          new WoWPoint(-10497.9,1033.441,98.64555),    // Elder Skychaser, Sentinel Hill, Westfall
          new WoWPoint(-10439.7,942.6649,208.5033),
          
          new WoWPoint(-9482.504,390.4489,268.5028),
          
          new WoWPoint(-9138.593,314.5753,121.8163),
          new WoWPoint(-9104.347,334.9923,94.72482),    // Elder Hammershout, Stormwind gate, Elwynn Forest
          new WoWPoint(-9174.858,336.8104,106.0724),
          
          new WoWPoint(-9289.904,315.468,81.98125),
          new WoWPoint(-9376.887,247.7204,75.53066),
          
          new WoWPoint(-9407.084,189.4832,71.17542),
          new WoWPoint(-9412.97,156.0366,57.79867),    // Elder Stormbrow, Goldshire pond, Elwynn Forest
          new WoWPoint(-9468.705,90.00902,150.8339),        
          
          new WoWPoint(-9082.59,-1581.879,466.8862),    // mountain peak
          new WoWPoint(-8066.494,-2490.706,412.4322),
          
          new WoWPoint(-7951.678,-2645.531,265.9657),
          new WoWPoint(-7938.998,-2673.699,211.0266),    // Elder Rumblerock, Dreadmaul Rock, Burning Steppes
          new WoWPoint(-7904.363,-2729.845,214.3659),
          
          new WoWPoint(-7818.875,-2734.069,215.5486),
          new WoWPoint(-7729.587,-2633.41,213.0389),
          new WoWPoint(-7605.566,-2135.923,218.2539),
      
          new WoWPoint(-7532.648, -2141.919, 156.2749), 
          new WoWPoint(-7503.64,-2155.257,148.6457),    // Elder Dawnstrider, Flame Crest, Burning Steppes
          new WoWPoint(-7493.809,-2148.866,331.5963),
          
          new WoWPoint(-7359.516,-1961.382,423.03),
          new WoWPoint(-7199.546,-1393.636,405.0727),
          
          new WoWPoint(-7251.934,-862.9638,381.8157),
          new WoWPoint(-7276.146,-799.3252,298.5018),    // Elder Ironband, Blackchar Cave, Searing Gorge
          new WoWPoint(-7235.374,-857.761,353.2098),
      
          new WoWPoint(-6131.585,-1030.479,584.4174),
          
          new WoWPoint(-5570.253,-555.9869,497.9306),
          new WoWPoint(-5571.506,-504.6964,404.2694),        // Elder Goldwell, Kharanos, Dun Morogh
          new WoWPoint(-5468.634, -578.2297, 545.1437),
     
          // Ironforge-block start
          // We tried to go through the taxi tunnel, but its simply too easy to
          // dismount while using it--depending on your mount, and server lag.
          // So, we go around the outer loop instead.
          new WoWPoint(-5024.506,-829.4019,505.9398),        // under Ironforge gate
          new WoWPoint(-5008.024, -852.8991, 506.754),
          new WoWPoint(-4978.824, -884.8437, 543.3408),        // outside of outer flying pass
          new WoWPoint(-4947.092,-923.2659,543.7946),        // inside of outer flying pass
          new WoWPoint(-4920.898,-930.4252,538.1689),
          new WoWPoint(-4815.398,-882.4954,544.0896),
          new WoWPoint(-4726.005,-920.4998,527.7712),
          new WoWPoint(-4667.42,-955.2431,514.2967),
          new WoWPoint(-4661.131,-945.9443,501.5024),     // Elder Bronzebeard, Ironforge
          new WoWPoint(-4667.42,-955.2431,514.2967),
          new WoWPoint(-4726.005,-920.4998,527.7712),
          new WoWPoint(-4815.398,-882.4954,544.0896),
          new WoWPoint(-4920.898,-930.4252,538.1689),
          new WoWPoint(-4947.092,-923.2659,543.7946),        // inside of outer flying pass
          new WoWPoint(-4978.824, -884.8437, 543.3408),        // outside of outer flying pass
          new WoWPoint(-5008.024, -852.8991, 506.754),
          new WoWPoint(-5024.506,-829.4019,505.9398),        // under Ironforge gate
          // Ironforge-block end
 
          new WoWPoint(-5374.583,-821.7066,534.5256),
          new WoWPoint(-5470.374,-1201.224,514.3913),
          new WoWPoint(-5394.118,-2572.396,555.7161),
          
          new WoWPoint(-5406.709,-2829.14,421.769),
          new WoWPoint(-5345.137,-2911.056,346.6411),    // Elder Silvervein, Thelsamar, Loch Modan
          new WoWPoint(-5278.261,-2967.654,431.6458),
          
          new WoWPoint(-4621.795,-2990.866,450.7226),
          new WoWPoint(-2140.955,-3161.196,336.0871),
          new WoWPoint(-431.8305,-3678.985,431.8179),
          
          new WoWPoint(209.1653,-3528.976,200.7221),
          new WoWPoint(232.3433,-3501.301,162.7853),    // Elder Highpeak, Creeping Ruin, Hinterlands
          new WoWPoint(268.8506,-3557.625,228.026),
          
          new WoWPoint(729.8191,-4486.686,390.4666),
          new WoWPoint(1178.036,-4871.394,471.5776),
          new WoWPoint(1397.715,-4852.138,403.4043),
          new WoWPoint(2189.407,-5262.334,188.7899),
          
          new WoWPoint(2250.81,-5315.583,133.618),
          new WoWPoint(2238.451,-5339.314,86.79946),    // Elder Snowcrown,  Light's Hope Chapel, EPL
          new WoWPoint(2238.451,-5339.314,163.7533),
          
          new WoWPoint(1961.448,-4553.39,176.3605),
          new WoWPoint(1804.756,-3716.363,202.1629),
          
          new WoWPoint(1859.611,-3697.063,158.479),
          new WoWPoint(1854.688,-3718.726,160.5863),    // Elder Windrun, Crown Guard Tower, EPL
          new WoWPoint(1861.441,-3668.274,181.5179),
          
          new WoWPoint(1293.712,-2585.086,233.3421),
          new WoWPoint(1261.941, -2558.802, 119.4695),  // Elder Moonstrike, Scholomance, WPL
          new WoWPoint(1263.955,-2522.663,200.933),
          
          new WoWPoint(2220.438,-2531.714,191.4538),
          new WoWPoint(2245.585,-2412.936,67.02487),      // cave entrance
          new WoWPoint(2263.283,-2351.626,60.69508),        // just inside cave
          
          new WoWPoint(2285.164,-2320.412,63.96877),
          new WoWPoint(2327.672,-2314.385,59.38554),    // Elder Meadowrun, Weeping Cave, WPL
          new WoWPoint(2285.164,-2320.412,63.96877),
         
          new WoWPoint(2251.774,-2396.791,60.0735),       // in front of cave
          new WoWPoint(2140.954,-2442.549,173.1675),
          new WoWPoint(1176.612,-940.3203,261.9341),      // mountain peak
          new WoWPoint(554.1945,1375.337,255.925),        // Sepulcher Main gate
          
          new WoWPoint(517.9789, 1561.336, 189.5842),
          new WoWPoint(514.8612, 1559.996, 132.0863),   // Elder Obsidian, Sepulcher, Silverpine Forest
          new WoWPoint(554.1945,1375.337,255.925),
          
          new WoWPoint(730.7363,1311.886,279.576),
  
          // Undercity-block start
         new WoWPoint(1697.108,240.552,284.3854),    // high over King's chamber
         new WoWPoint(1697.108,240.552,62.59601),    // outside King's chamber
         new WoWPoint(1641.827,239.4021,62.59274),   // center of King's chamber
         new WoWPoint(1636.497,235.3643,62.59274),   // Elder Darkcore, Undercity, Tirisfall Glades
         new WoWPoint(1641.827,239.4021,62.59274),   // center of King's chamber
         new WoWPoint(1697.108,240.552,62.59601),   // outside king's chamber
         new WoWPoint(1697.108,240.552,284.3854),    // into the air
         // Undercity-block end
         
         new WoWPoint(1875.489, 193.9487, 104.7309),
         new WoWPoint(1875.489, 193.9487, 104.7309),
         new WoWPoint(1919.847, 236.2152, 83.2028),
         
         // Go back to Elder Graveborne at Brill --
         // Since he is a deathtrap (moreso for non-85s), we leave him for last
         // Park the toon in a safe location for him to be done manually
         new WoWPoint(2155.134, 221.7616, 83.54243),
         // new WoWPoint(2211.548,237.5521,34.83718),          // Elder Graveborne, Brill
         // new WoWPoint(2174.266,188.3857,157.4841),
       };

        static List<WoWPoint> Elders_Kalimdor = new List<WoWPoint>
       {
          new WoWPoint(10119.04, 2546.121, 1345.15),
          new WoWPoint(10136.68, 2583.25, 1327.728),       // Elder Bladeswift, Darnassas
          new WoWPoint(10097.71, 2386.562, 1408.976),
          
          new WoWPoint(10020.85, 2008.531, 1417.904),
          new WoWPoint(10124.96, 1724.718, 1386.488),
          new WoWPoint(10037.04, 1353.393, 1347.035),
          new WoWPoint(9737.973, 954.5499, 1313.82),
          
          new WoWPoint(9759.324, 910.4705, 1310.036),
          new WoWPoint(9767.338, 896.5941, 1299.573),      // Elder Bladeleaf, Dolanaar, Teldrassil
          new WoWPoint(9717.642, 883.4667, 1328.593),
          
          new WoWPoint(9261.107, 820.6467, 1351.542),
          new WoWPoint(9107.052, 800.2144, 1359.376),
          new WoWPoint(8916.699, 771.9214, 1151.479),
          new WoWPoint(8542.029, 798.262, 175.5694),
          new WoWPoint(8228.635, 775.0378, 62.87673),
          new WoWPoint(7610.64, 394.856, 75.59877),
          
          new WoWPoint(7416.724, -142.562, 30.6413),
          new WoWPoint(7406.071, -186.8245, 8.70245),     // Elder Starweave, Darkshore
          new WoWPoint(7297.535, -68.87838, 148.7377),
          
          new WoWPoint(3762.401, 869.9405, 350.4101),
          
          new WoWPoint(2709.4, -180.1351, 404.807),
          new WoWPoint(2792.09, -348.1969, 107.4054),     // Elder Riversong, Ashenvale
          new WoWPoint(2787.015, -326.3548, 128.1904),
          
          new WoWPoint(2803.274, -304.4198, 159.2827), 
           new WoWPoint(3129.519, -322.561, 479.5324),
          new WoWPoint(4188.414, -406.9641, 509.1637),
          new WoWPoint(4779.869, -559.7806, 546.4879),
          
          new WoWPoint(5087.645, -561.1508, 371.9468),
           new WoWPoint(5101.175, -529.0582, 335.6421),     // Elder Nightwind, Felwood
          new WoWPoint(5304.068, -878.7056, 684.5632),
          
          new WoWPoint(5669.002, -1645.483, 1133.608),
          new WoWPoint(6031.707, -2553.667, 1329.51),
          new WoWPoint(6352.728, -3459.72, 913.4974),
          
          new WoWPoint(6481.792, -4230.278, 699.2571),
          new WoWPoint(6467.604, -4263.323, 670.2648),     // Elder Brightspear, Winterspring
          new WoWPoint(6489.192, -4302.014, 748.5419),
          
          new WoWPoint(6701.483, -4623.994, 754.7487),
          new WoWPoint(6743.367, -4676.522, 727.5551),     // Elder Stonespire, Winterspring
          new WoWPoint(6677.005, -4670.579, 752.949),
          
          new WoWPoint(6227.521, -4840.66, 958.2523),
          new WoWPoint(5526.521, -5153.141, 1032.555),
          
          new WoWPoint(2531.715, -6836.17, 257.664),
          new WoWPoint(2464.171, -6941.983, 122.1526),     // Elder Skygleam, Azshara
          new WoWPoint(2452.17, -6899.326, 170.0691),
          
          new WoWPoint(1319.616, -5405.789, 352.6239),
          new WoWPoint(525.908, -5027.01, 189.2937),
          
          // new WoWPoint(251.7326, -4853.543, 65.32681),
          // new WoWPoint(270.0409, -4776.955, 15.27754),     // Elder Runetotem, Durotar
           
          new WoWPoint(-256.4428, -4538.061, 201.8343),
          
          new WoWPoint(-877.7217, -4056.234, 208.625),
          new WoWPoint(-870.8623, -3725.714, 26.8684),     // Elder Windtotem, Ratchet
          new WoWPoint(-890.4214, -3654.431, 106.1221),
          
          new WoWPoint(-470.2902, -2655.692, 156.3535),
          new WoWPoint(-458.1281, -2589.682, 102.9946),     // Elder Moonwarden, N. Barrens 
          new WoWPoint(-458.7752, -2585.778, 180.2517),
          
          new WoWPoint(-684.0205, -2362.876, 186.1209),
          new WoWPoint(-1785.014, -1927.499, 173.542),
          
          new WoWPoint(-2158.338, -1758.182, 125.6535),
          new WoWPoint(-2146.611, -1725.001, 96.4761),     // Elder High Mountain, S. Barrens
          new WoWPoint(-2175.469, -1725.784, 131.0244),
          
          new WoWPoint(-2270.344, -1471.962, 153.5582),
          
          new WoWPoint(-2119.161, -497.3678, 42.81783),
          new WoWPoint(-2103.791, -441.648, -4.986813),     // Elder Bloodhoof, Mulgore
          new WoWPoint(-2080.319, -458.5359, 26.94545),
          
          new WoWPoint(-1047.238, -448.7425, 210.437),
          
          new WoWPoint(-903.3357, -305.0685, 199.658),
          new WoWPoint(-1009.771, -246.7142, 160.9891),    // Elder Wheathoof, Thunderbluff, Mulgore
          new WoWPoint(-1007.907, -248.4991, 303.2256),
          
          new WoWPoint(-1618.149, -72.16324, 278.1185),
          
          new WoWPoint(-2519.648, 494.4662, 364.5027),
          new WoWPoint(-3207.079, 931.9422, 352.6861),
          
          new WoWPoint(-3666.095, 1079.877, 263.3408),
          new WoWPoint(-3803.975, 1093.96, 135.6997),     // Elder Mistwalker, Feralas
          new WoWPoint(-3803.975, 1093.96, 281.6966),
          
          new WoWPoint(-3829.291, 729.9054, 284.6015),
          
          new WoWPoint(-4143.139, 171.2936, 92.07726),
          new WoWPoint(-4123.742, 113.8697, 80.98565),     // Elder Grimtotem, Feralas
          new WoWPoint(-4163.019, 104.7282, 108.2475),
          
          new WoWPoint(-4426.669, -73.31574, 129.3947),
          
          new WoWPoint(-5432.22, -2432.646, 164.1436),
          new WoWPoint(-5461.754, -2471.22, 92.3773),     // Elder Skyseer, Thousand Needles
          new WoWPoint(-5491.779, -2509.316, 129.0496),
          
          new WoWPoint(-5703.748, -3340.015, 163.7127),
          
          new WoWPoint(-6269.197, -3677.081, 66.01242),
          new WoWPoint(-6184.733, -3825.451, 4.724072),     // Elder Morningdew, Thousand Needles
          new WoWPoint(-6212.789, -3820.306, 54.75811),
          
          new WoWPoint(-6941.327, -3768.82, 132.4055),
          
          new WoWPoint(-7139.012, -3735.87, 35.02393),
          new WoWPoint(-7154.728, -3767.478, 10.96255),     // Elder Dreamseer, Tanaris
          new WoWPoint(-7238.512, -3775.694, 73.97723),
          
          new WoWPoint(-8406.726, -3363.155, 144.7664),
          new WoWPoint(-9440.058, -2877.295, 100.3829),
          
          new WoWPoint(-9519.72, -2765.569, 38.26007),
          new WoWPoint(-9568.983, -2749.523, 15.81504),     // Elder Ragetotem, Tanaris
          new WoWPoint(-9519.492, -2756.171, 28.56911),
          
          new WoWPoint(-8741.248, -2084.773, 109.8785),
          new WoWPoint(-8337.309, -1971.029, 116.3964),
          
          new WoWPoint(-7867.477, -1419.306, -197.7708),
          new WoWPoint(-7845.993, -1332.942, -262.2864),     // Elder Thunderhorn, Un'goro
          new WoWPoint(-7854.717, -1273.639, -202.1302),
          
          new WoWPoint(-7889.814, -837.8798, -120.7818),
          new WoWPoint(-7725.046, -33.26245, 274.5538),
          
          new WoWPoint(-6894.431, 763.0329, 122.165),
          new WoWPoint(-6834.011, 830.5678, 50.89212),     // Elder Bladesing, Silithus
          new WoWPoint(-6808.23, 899.3165, 125.8977),
          
          new WoWPoint(-6409.528, 1609.491, 168.7027),
          new WoWPoint(-6234.47, 1732.861, 6.959129),     // Elder Primestone, Silithus
          
          // Elder Runetotem, Durotar --
          // Fly back to Razor Hill to have player pick that one up manually
          new WoWPoint(-6238.845, 1492.129, 233.9813),
          new WoWPoint(-6215.214, -429.6011, 451.8841),
          new WoWPoint(-5312.444, -1831.337, 195.6271),
          new WoWPoint(-4600.857, -2954.39, 246.4333),
          new WoWPoint(-1664.011, -4378.069, 78.43668),
          new WoWPoint(-924.0878, -4657.066, 164.0993),
          new WoWPoint(231.4999, -4791.583, 39.54893),
          
          // And while you're in the area, get the Elder in Orgrimmar, manually.
          // It is too poorly placed to fetch it with a bot.
    };

        static List<WoWPoint> Elders_Northrend = new List<WoWPoint>
       {
           new WoWPoint(5796.157,385.2876,700.1443),
           new WoWPoint(5993.72,-355.1305,570.7634),
           new WoWPoint(6108.323,-925.9988,473.7646),
           
           new WoWPoint(6164.465,-1055.299,449.6936),
           new WoWPoint(6180.194,-1084.714,418.1323),    // Elder Graymane, K3, Stormpeaks
           new WoWPoint(6229.546,-1021.766,500.2993),
       
           new WoWPoint(6425.996,-355.3815,893.8164),
           
           new WoWPoint(6552.042,-108.4708,1010.996),
           new WoWPoint(6701.36,-211.7225,976.667),    // Elder Fargal, Frosthold, Stormpeaks
           new WoWPoint(6747.271,-229.5318,1026.467),
           
           new WoWPoint(7482.671,-128.8072,1024.997),
           new WoWPoint(7955.129,-168.9987,1028.246),
           new WoWPoint(8438.446,-211.8688,1031.576),
       
           new WoWPoint(8455.443,-313.6833,906.4792),
           new WoWPoint(8444.457,-335.7659,906.608),
           new WoWPoint(8415.835,-370.8161,905.7672),
           new WoWPoint(8414.74,-379.4786,903.1189),    // Elder Stonebeard, Bouldercrag's Refuge, Storm Peaks
           new WoWPoint(8415.835,-370.8161,905.7672),
           new WoWPoint(8444.457,-335.7659,906.608),
           new WoWPoint(8455.443,-313.6833,920.4792),
      
           new WoWPoint(8540.854, -391.3542, 956.3217),
           new WoWPoint(8345.034,-723.2997,1036.897),
           new WoWPoint(8330.276,-1418.763,1301.798),
           new WoWPoint(7835.916,-1750.068,1348.811),
           new WoWPoint(7832.443,-2637.507,1286.233),
           
           new WoWPoint(7806.3,-2694.396,1216.601),
           new WoWPoint(7764.67,-2751.141,1165.662),    // Elder Muraco, Camp Taunka'lo, Stormpeaks
           new WoWPoint(7744.666,-2757.61,1202.242),
       
           new WoWPoint(7068.74,-3049.767,1105.079),
           new WoWPoint(6508.707,-3186.713,683.1038),
           new WoWPoint(5764.372,-3452.594,440.8291),
       
           new WoWPoint(5764.494,-3556.977,410.8479),
           new WoWPoint(5803.561,-3541.917,392.3495),        // Elder Tauros, Zim'Torga, Zul'Drak        
           new WoWPoint(5768.794,-3584.395,521.0557),
           
           new WoWPoint(5078.485,-4140.871,541.1716),
           new WoWPoint(4632.418,-4212.588,297.2442),
       
           new WoWPoint(4573.829,-4241.384,233.7437),
           new WoWPoint(4548.339,-4289.564,174.911),    // Elder Beldak, Westfall Brigade, Grizzly Hills
           new WoWPoint(4524.21,-4368.489,239.1223),
           
           new WoWPoint(4292.798,-5034.428,146.8104),
        
           new WoWPoint(4226.717,-5287.786,42.74117),
           new WoWPoint(4218.057,-5336.751,12.45238),    // Elder Lunaro, Ruins of Tethys, Grizzly Hills
           new WoWPoint(4176.476,-5280.446,83.82386),
           
           new WoWPoint(3966.831,-4484.409,310.0024),
           
           new WoWPoint(3867.548,-4452.104,268.0565),
           new WoWPoint(3871.941,-4478.753,224.6944),    // Elder Whurain, Camp Onequah, Grizzly Hills
           new WoWPoint(3780.239,-4440.503,327.0167),            
         
           new WoWPoint(3499.74,-3635.152,416.1246),
           new WoWPoint(2820.399,-1649.696,278.024),
           new WoWPoint(2742.192,-226.1272,280.866),
           new WoWPoint(2824.511,774.9434,171.9687),
       
           new WoWPoint(2683.285,860.6082,21.32429),
           new WoWPoint(2652.946,890.6755,5.614088),    // Elder Thoim, Moa'ki Harbor, Dragonblight
           new WoWPoint(2813.105,1012.343,226.3088),
           
           new WoWPoint(3609.7,1405.778,274.3603),
           
           new WoWPoint(3694.252,1740.235,170.2027),
           new WoWPoint(3766.094,1660.084,119.548),    // Elder Skywarden, Agmar's Hammer, Dragonblight
           new WoWPoint(3691.848,1761.323,188.4278),
       
           new WoWPoint(3558.168,1952.294,174.4),
           
           new WoWPoint(3466.372,1873.906,72.37784),
           new WoWPoint(3483.93,1957.393,65.38245),    // Elder Morthie, Star's Rest, Dragonblight
           new WoWPoint(3442.441,2049.953,175.3152),
       
           new WoWPoint(3230.373,2528.026,176.5102),
           new WoWPoint(3093.936,3626.69,122.7707),
           new WoWPoint(3010.644,3923.892,123.4755),
           new WoWPoint(2965.469,4206.975,129.2258),
       
           new WoWPoint(2416.116,5066.159,102.7059),
           new WoWPoint(2375.948,5163.715,5.021813),    // Elder Sardis, Valiance Keep, Borean Tundra
           new WoWPoint(2448.242,5204.145,111.0594),
           
           new WoWPoint(3060.423,5300.214,142.7746),
           
           new WoWPoint(3170.207,5290.354,102.6351),
           new WoWPoint(3216.466,5262.275,49.06043),    // Elder Arp, DEHTA camp, Borean Tundra
           new WoWPoint(3212.224,5325.949,117.5777),
           
           new WoWPoint(3023.261,6013.063,221.6919),
           new WoWPoint(2992.844,6094.582,143.0465),    // Elder Pamuya, Warsong Hold, Borean Tundra
           new WoWPoint(3073.698,6147.989,239.3186),
           
           new WoWPoint(3566.272,6550.884,374.8345),
          
           new WoWPoint(3610.311,6642.752,239.6441),
           new WoWPoint(3576.621,6623.554,195.2933),    // Elder Northal, Tansitus Shield, Borean Tundra
           new WoWPoint(3632.368,6609.129,275.3135),
           
           new WoWPoint(3830.644,6359.122,343.5761),
           new WoWPoint(4624.476,5524.439,248.8844),
           new WoWPoint(5069.514,5060.123,202.7397),
           
           new WoWPoint(5405.656,4814.793,-117.3061),
           new WoWPoint(5439.357,4761.817,-197.0246),    // Elder Sandrene, River's Heart, Sholazar Bazin
           new WoWPoint(5446.033,4700.682,-116.4365),
           
           new WoWPoint(5478.107,4471.036,-97.96387),
              
           new WoWPoint(5789.871,4206.644,-66.9221),
           new WoWPoint(5862.306,4150.992,-93.0735),    // Elder Wanikaya, Rainspeaker Rapids, Sholazar Basin
           new WoWPoint(5816.111,4025.469,70.47462),
           
           new WoWPoint(5459.11,3438.194,479.6923),
           new WoWPoint(5280.24,2849.35,543.7144),
           
           // Elder Bluewolf, WG fortress, Lake Wintergrasp
           // Your faction must control Wintergrasp Fortress, so we make
           // you fetch this one manually.  The other reason we can't get
           // this for you, is you have to use the Defender's Portal to get in.
       };
        #endregion

        private Composite _root;
        public override Composite Root
        {
            get
            {
                return _root ?? (_root =
                    new PrioritySelector(
                        LevelBot.CreateDeathBehavior(),
                        CreateCombatBehaviour(),
                        new Decorator(ret => !StyxWoW.Me.IsFlying,
                        LevelBot.CreateVendorBehavior()
                        ),
                        new Decorator(ret => (!StyxWoW.Me.Combat && !StyxWoW.Me.Mounted && !StyxWoW.Me.IsIndoors && !StyxWoW.Me.IsSwimming && (getNode() == null || getNode().Location.Distance(StyxWoW.Me.Location) > 20)),//mount
                            new Action(ret => mount())
                            ),
                        new Decorator(ret => StyxWoW.Me.HealthPercent < 70 && StyxWoW.Me.IsAlive && !StyxWoW.Me.Combat && StyxWoW.Me.Mounted,
                            new Sequence(
                                new Action(ret => FlyToRest()),
                                new Action(ret => WaitForHP())
                                )
                            ),
                        CreateGatherBehaviour(),
                        new Decorator(ret => getNode() == null,

                        CreateMoveBehaviour())
                        )
                    );
            }
        }

        public static void FlyToRest()
        {
            ObjectManager.Update();
            WoWMovement.ClickToMove(StyxWoW.Me.Location.Add(0, 0, (float)60));
        }

        public static void WaitForHP()
        {
            Logging.Write("Resting!");
            while (StyxWoW.Me.HealthPercent < 70)
            {
                Thread.Sleep(250);
                if (StyxWoW.Me.Combat)
                {
                    return;
                }
            }
        }

        #region CreateGatherBehaviour
        public static Composite CreateGatherBehaviour()
        {
            return new Decorator(ret => (isNode() && !StyxWoW.Me.Combat),
                new Sequence(
                    new Action(ret => moveToHarvestNode()),

                    new Decorator(ret => !StyxWoW.Me.Combat,
                        new Sequence(
                        new Action(ret => harvestNode()),
                        new Action(ret => FlyToRest()))
                        )
                    ));
        }

        static Stopwatch triesSW = new Stopwatch();
        static ulong triesGuid = 0;

        static Dictionary<ulong, int> attempts = new Dictionary<ulong, int>();

        public static void moveToHarvestNode()
        {
            WoWUnit wgo = getNode();
            if (!attempts.ContainsKey(wgo.Guid))
            {
                attempts.Add(wgo.Guid, 0);
            }
            else
            {
                attempts[wgo.Guid]++;
                if (attempts[wgo.Guid] > numHarvestAttempts)
                {
                    Stopwatch sw = new Stopwatch();
                    //Logging.Write("new WoWPoint(" + StyxWoW.Me.X.ToString() + "," + StyxWoW.Me.Y.ToString() + "," + StyxWoW.Me.Z.ToString() + "),");
                    sw.Start();
                    //blackList.Add(sw, wgo.Location);
                }
            }
            if (wgo.Guid != triesGuid)
            {
                triesSW.Reset();
                triesSW.Start();
                triesGuid = wgo.Guid;
            }
            else
            {
                if (triesSW.Elapsed.TotalSeconds > 30)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    //blackList.Add(sw, StyxWoW.Me.Location);
                    return;
                }
            }
            int tries = 0;
            while (wgo.Distance > 5)
            {
                StuckDetector();
                WoWMovement.ClickToMove(wgo.Location);
                Thread.Sleep(100);
                if (StyxWoW.Me.Combat || tries >= 3)
                {
                    return;
                }
                tries++;
            }
        }

        public static bool isNode()
        {
            if (getNode() == null)
            {
                return false;
            }
            return true;
        }

        public static double XYDist(WoWGameObject wgo)
        {
            return Math.Sqrt(Math.Pow(wgo.Location.X - StyxWoW.Me.Location.X, 2) + Math.Pow(wgo.Location.Y - StyxWoW.Me.Location.Y, 2));
        }

        public static bool nodeIsBlacklisted(WoWPoint wp)
        {
            List<Stopwatch> sw = blackList.Keys.ToList();
            for (int i = blackList.Count - 1; i >= 0; i--)
            {
                if (sw[i].Elapsed.TotalMinutes >= blackListTime)
                {
                    blackList.Remove(sw[i]);
                }
            }
            foreach (WoWPoint wowp in blackList.Values)
            {
                if (wp.Distance(wowp) < 50)
                {
                    return true;
                }
            }
            return false;
        }

        public static WoWUnit getNode()
        {
            ObjectManager.Update();

            List<WoWUnit> units = ObjectManager.GetObjectsOfType<WoWUnit>();

            WoWUnit bestNode = null;
            foreach (WoWUnit WoWElders in units)
            {
               if(WoWElders.IsQuestGiver && WoWElders.QuestGiverStatus == QuestGiverStatus.Available && WoWElders.Class == Styx.Combat.CombatRoutine.WoWClass.Mage && WoWElders.Name.Contains("Elder") && WoWElders.Distance <= DistToHarvest)
               {
                   Logging.Write(WoWElders.Name + " found!");

              
                   bestNode = WoWElders;
                    
                }
            }
            return bestNode;
        }
        //unused
        public static void dismount()
        {
            if (StyxWoW.Me.HoverHeight > 10)
            {
                return;
            }
            Lua.DoString("Dismount()");
            Thread.Sleep(1000);
        }

        public static void harvestNode()
        {
            WoWUnit wgo = getNode();
            if (wgo == null)
            {
                return;
            }
            if (wgo.Distance > 5)
            {
                return;
            }
            int tries = 0;
            WoWUnit newwgo = null;
            bool firstrun = true;
            while (wgo != null)
            {
                if (newwgo != null)
                {
                    if (newwgo.Guid != wgo.Guid)
                    {
                        return;
                    }
                }
                else if (!firstrun)
                {
                    return;
                }
                firstrun = false;
                ObjectManager.Update();
                if (tries > 30)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    blackList.Add(sw, wgo.Location);
                    return;
                }
                DoQuest(wgo);
                for (int i = 0; i < 10; i++)
                {
                    ObjectManager.Update();
                    newwgo = getNode();
                    if (newwgo != null || newwgo.QuestGiverStatus == QuestGiverStatus.None)
                    {
                        if (newwgo.Guid != wgo.Guid)
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                    if (StyxWoW.Me.Combat)
                    {
                        if (StyxWoW.Me.Mounted)
                        {
                            Logging.Write("Incombat, but still Mounted, Flying away");
                            KeyboardManager.PressKey((char)KeyboardManager.eVirtualKeyMessages.VK_SPACE);
                            Thread.Sleep(3000);
                            KeyboardManager.ReleaseKey((char)KeyboardManager.eVirtualKeyMessages.VK_SPACE);
                        }
                        return;
                    }
                    Thread.Sleep(500);
                }
                tries++;
                newwgo = getNode();
            }
        }

        public static void DoQuest(WoWUnit Target)
        {
            Logging.Write("Doing Quest at NPC " + Target.Name);
            Target.Interact();
            Thread.Sleep(600);
            Lua.DoString("SelectGossipAvailableQuest(1)");
            Thread.Sleep(600);
            Styx.Logic.Inventory.Frames.Quest.QuestFrame.Instance.CompleteQuest();
        }
        public static void mount()
        {
            if (!StyxWoW.IsLifetimeUser)
            {
                Logging.Write("Mr.LunarFestival Cannot Run, as Account is not lifetime. Please Upgrade your Honorbuddy Account to Lifetime, or Click Stop. Then Select a Diffrent (Mode) to Continue botting");
                return;
            }
            if (StyxWoW.Me.IsIndoors)
            {
                return;
            }
            if (StyxWoW.Me.HoverHeight > 10)
            {
                return;
            }
            WoWMovement.MoveStop();
            int tries = 0;
            while (!StyxWoW.Me.Mounted)
            {
                if (StyxWoW.Me.Combat || StyxWoW.Me.Dead)
                {
                    return;
                }
                if (tries > 4)
                {
                    Logging.Write("Failing to mount! Check mount name!");
                }
                if(StyxWoW.Me.Class == Styx.Combat.CombatRoutine.WoWClass.Druid &&(SpellManager.CanCast("Swift Flight Form") || SpellManager.CanCast("Flight Form")))
                {
                    if(SpellManager.CanCast("Swift Flight Form"))
                    {
                        Logging.Write("Casting Swift Flight Form");
                        SpellManager.Cast("Swift Flight Form");
                    }
                    else
                    if(SpellManager.CanCast("Flight Form"))
                    {
                        Logging.Write("Casting Flight Form");
                        SpellManager.Cast("Flight Form");
                    }
                }
                else
                {
                Lua.DoString("CastSpellByName(\"" + MountName + "\")");
                }
                Thread.Sleep(3000);
                tries++;
            }

        }
        #endregion

        #region CreateMoveBehaviour

        public static Composite CreateMoveBehaviour()
        {
            return new Action(ret => moveToNode());
        }


        #endregion

        public static void moveToNode()
        {
           // StuckDetector();
            if ((lastcount == -1 || profile[lastcount].Distance(StyxWoW.Me.Location) <= 10) && fullcount != lastcount)
            {
                Logging.Write("Moving to Wavepoint #{0}", lastcount.ToString());
                WoWMovement.ClickToMove(profile[GetNextNode()]);
                Thread.Sleep(1000);
            }
            else if (lastcount != -1 && !StyxWoW.Me.IsMoving && fullcount != lastcount)
                {

                        Logging.Write("Moving to Wavepoint #{0}", lastcount.ToString());
                        WoWMovement.ClickToMove(profile[lastcount]);
           
                }
            
            else if (fullcount == lastcount)
            {
                switch (LunarSettings.Instance.ProfileSelect)
                {
                    case "Eastern Kingdoms (Alliance)":
                        Logging.Write("Eastern Kindoms Profile Completed");
                        break;
                    case "Kalimdor (Alliance)":
                        Logging.Write("Kalimdor (Alliance) Profile Completed");
                        Logging.Write("Please Pick up the Orgrimmar and Razor Hill coins manually");
                        break;
                    case "Northrend (Alliance)":
                        Logging.Write("Northrend Profile Complete");
                        Logging.Write("Please Pick up the Wintergrasp Coin Manualy when your faction has the Keep");
                        break;
                }
                Styx.BotManager.Current.Stop();
            }
            return;
        }

        public static int fullcount
        {
            get
            {
                return profile.Count;
            }
        }
        public static int lastcount = -1;
        public static int GetNextNode()
        {
          if(lastcount == -1)
          {
              lastcount = 0;
              return 0;
          }
          else
          if(profile[lastcount].Distance(StyxWoW.Me.Location) <= 10)
          {
          return lastcount++;
          }
          return lastcount;
          


        }

        public static int GetClosestNode()
        {
            double d = double.MaxValue;
            int node = 0;
            for (int i = 0; i < profile.Count; i++)
            {
                if (profile[i].Distance(StyxWoW.Me.Location) < d)
                {
                    d = profile[i].Distance(StyxWoW.Me.Location);
                    node = i;
                }
            }
            return node;
        }

        public static void Target()
        {
            //List<WoWUnit> wus = ObjectManager.GetObjectsOfType<WoWUnit>();
            //foreach (WoWUnit wu in wus)
            //{
            //    if (wu.IsTargetingMeOrPet && wu.Attackable)
            //    {
            //        wu.Target();
            //        return;
            //    }
            //}
        }


        private static Composite CreateCombatBehaviour()
        {

            return new Decorator(ret => (StyxWoW.Me.Combat && !StyxWoW.Me.Mounted),
                             new PrioritySelector(
                                 new Decorator(ret => StyxWoW.Me.CurrentTarget == null,
                                        new Action(ret => Target())
                                     ),
                                 new Decorator(ret => (!StyxWoW.Me.IsFacing(StyxWoW.Me.CurrentTarget.Location)),
                                     new Action(ret => WoWMovement.ClickToMove(StyxWoW.Me.CurrentTarget.Location))
                                     ),

            #region Heal

 new PrioritySelector(
                // Use the Behavior
                                     new Decorator(ctx => RoutineManager.Current.HealBehavior != null,
                                         new Sequence(
                                             RoutineManager.Current.HealBehavior,
                                             new Action(delegate { return RunStatus.Success; })
                                             )),

                                     // Don't use the Behavior
                                     new Decorator(ctx => RoutineManager.Current.NeedHeal,
                                         new Sequence(
                                             new Action(ret => TreeRoot.StatusText = "Healing"),
                                             new Action(ret => RoutineManager.Current.Heal())
                                             ))),

            #endregion

            #region Combat Buffs

 new PrioritySelector(
                // Use the Behavior
                                     new Decorator(ctx => RoutineManager.Current.CombatBuffBehavior != null,
                                                 new Sequence(
                                                     RoutineManager.Current.CombatBuffBehavior,
                                                     new Action(delegate { return RunStatus.Success; })
                                                     )
                                         ),

                                     // Don't use the Behavior
                                     new Decorator(ctx => RoutineManager.Current.NeedCombatBuffs,
                                                 new Sequence(
                                                     new Action(ret => TreeRoot.StatusText = "Applying Combat Buffs"),
                                                     new Action(ret => RoutineManager.Current.CombatBuff())
                                                     ))),

            #endregion

            #region Combat

 new PrioritySelector(
                // Use the Behavior
                                     new Decorator(ctx => RoutineManager.Current.CombatBehavior != null,
                                         new PrioritySelector(
                                             RoutineManager.Current.CombatBehavior,
                                             new Action(delegate { return RunStatus.Success; })
                                             )),

                                     // Don't use the Behavior
                                     new Sequence(
                                         new Action(ret => TreeRoot.StatusText = "Combat"),
                                         new Action(ret => RoutineManager.Current.Combat())))

            #endregion

));
        }

        public static void MoveToBody()
        {
            while (StyxWoW.Me.Location.Distance(StyxWoW.Me.CorpsePoint) > 5)
            {
                StuckDetector();
                Navigator.MoveTo(StyxWoW.Me.CorpsePoint);
                Thread.Sleep(100);
            }
        }

        public static void Retrieve()
        {
            Lua.DoString("RetrieveCorpse()");
        }

        public static void Release()
        {
            Lua.DoString("RepopMe()");
        }

        public static Composite CreateCorpseBehaviour()
        {
            return new Decorator(ret => !StyxWoW.Me.IsAlive,
                new Sequence(
                    new Action(ret => Release()),
                    new Action(ret => MoveToBody()),
                    new Action(ret => Retrieve())
                    )
                    );
        }


        static Stopwatch stuckSW = new Stopwatch();
        static WoWPoint stuckWP;
        public static void StuckDetector()
        {
            if (!StyxWoW.Me.Dead)
            {
                if (!stuckSW.IsRunning)
                {
                    stuckSW.Start();
                    stuckWP = StyxWoW.Me.Location;
                }
                else if (stuckSW.Elapsed.TotalSeconds > 15 || stuckWP.Distance(StyxWoW.Me.Location) > 3)
                {
                    stuckSW.Reset();
                    stuckSW.Start();
                    stuckWP = StyxWoW.Me.Location;
                }
                else if (stuckWP.Distance(StyxWoW.Me.Location) < 5 && ((stuckSW.Elapsed.Seconds > 2 && !StyxWoW.Me.IsSwimming) || stuckSW.Elapsed.Seconds > 20))
                {//is stuck
                    StuckBehaviour();
                    stuckSW.Reset();
                    stuckSW.Start();
                    stuckWP = StyxWoW.Me.Location;
                }
            }
        }
        static bool lastRight = true;
        public static void StuckBehaviour()
        {
            if (StyxWoW.Me.IsSwimming)
            {
                KeyboardManager.PressKey((char)KeyboardManager.eVirtualKeyMessages.VK_SPACE);
                Thread.Sleep(1000);
                KeyboardManager.ReleaseKey((char)KeyboardManager.eVirtualKeyMessages.VK_SPACE);
                Thread.Sleep(1000);
            }
            Logging.Write("I am stuck! Trying to unstuck!");
            WoWMovement.MoveStop();
            Thread.Sleep(200);
            KeyboardManager.PressKey((char)KeyboardManager.eVirtualKeyMessages.VK_DOWN);
            Thread.Sleep(1000);
            KeyboardManager.ReleaseKey((char)KeyboardManager.eVirtualKeyMessages.VK_DOWN);
            Thread.Sleep(200);
            KeyboardManager.PressKey((char)KeyboardManager.eVirtualKeyMessages.VK_SPACE);
            Thread.Sleep(1000);
            KeyboardManager.ReleaseKey((char)KeyboardManager.eVirtualKeyMessages.VK_SPACE);
            Thread.Sleep(200);
            char direction;
            if (lastRight)
            {
                direction = (char)KeyboardManager.eVirtualKeyMessages.VK_LEFT;
                lastRight = false;
            }
            else
            {
                direction = (char)KeyboardManager.eVirtualKeyMessages.VK_RIGHT;
                lastRight = true;
            }
            KeyboardManager.PressKey(direction);
            Thread.Sleep(500);
            KeyboardManager.ReleaseKey(direction);
            Thread.Sleep(200);
            KeyboardManager.PressKey((char)KeyboardManager.eVirtualKeyMessages.VK_UP);
            Thread.Sleep(1000);
            KeyboardManager.ReleaseKey((char)KeyboardManager.eVirtualKeyMessages.VK_DOWN);
            Thread.Sleep(200);
        }

    }
}
