using System.IO;
using Styx;
using Styx.Helpers;

namespace MrLunarFestival2011
{
    public class LunarSettings : Settings
    {
        public static readonly LunarSettings Instance = new LunarSettings();

        public LunarSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/Config/MrLunarFestival2011.xml")))
        {
        }

        [Setting, DefaultValue(10)]
        public int HarvestDistance { get; set; }

        [Setting, DefaultValue("Eastern Kingdoms (Alliance)")]
        public string ProfileSelect { get; set; }

        [Setting, DefaultValue("Flying Mount Name Here!!!")]
        public string MountName { get; set; }
    }
}
