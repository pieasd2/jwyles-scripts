using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.Plugins;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace GBProfileLoader
{
    class GBProfileLoader : HBPlugin
    {

        #region variables

        string[] ignored_gb_elements = { "GlideProfile", "Factions", "MaxLevel", "MinLevel", "NaturalRun", "SkipWaypoints", "LureMinutes", "Waypoint" };
        List<WoWPoint> waypoints = new List<WoWPoint>();

        #endregion

        #region overrides

        public override string Author
        {
            get { return "eXemplar"; }
        }

        public override string Name
        {
            get { return "GBProfileLoader"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public override void Pulse()
        {

        }

        public override void Initialize()
        {
            BotEvents.OnBotStart += BotEvents_OnBotStart;
            BotEvents.Profile.OnNewOuterProfileLoaded += Profile_OnNewOuterProfileLoaded;
            BotEvents.Profile.OnNewProfileLoaded += Profile_OnNewProfileLoaded;
            Profile.OnUnknownProfileElement += Profile_OnUnknownProfileElement;
        }

        public override void Dispose()
        {
            BotEvents.OnBotStart -= BotEvents_OnBotStart;
            BotEvents.Profile.OnNewOuterProfileLoaded -= Profile_OnNewOuterProfileLoaded;
            BotEvents.Profile.OnNewProfileLoaded -= Profile_OnNewProfileLoaded;
            Profile.OnUnknownProfileElement -= Profile_OnUnknownProfileElement;
        }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override string ButtonText
        {
            get
            {
                if (PluginManager.Plugins.Where(ret => ret.Name == Name).FirstOrDefault().Enabled)
                {
                    return "Objects";
                }
                else
                {
                    return "Blackspots";
                }
            }
        }

        public override void OnButtonPress()
        {
            if (PluginManager.Plugins.Where(ret => ret.Name == Name).FirstOrDefault().Enabled)
            {
                ObjectManager.Update();
                Logging.Write("  <Vendors>");
                foreach(WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>(false, false)) 
                {
                    if (unit.IsRepairMerchant)
                    {
                        Logging.Write("    <Vendor Name=\"{0}\" Entry=\"{1}\" Type=\"Repair\" X=\"{2}\" Y=\"{3}\" Z=\"{4}\" />", unit.Name, unit.Entry, unit.X, unit.Y, unit.Z);
                    }
                }
                Logging.Write("  </Vendors>");
                Logging.Write("  <Mailboxes>");
                foreach (WoWGameObject box in ObjectManager.GetObjectsOfType<WoWGameObject>(false, false))
                {
                    if (box.SubType == WoWGameObjectType.Mailbox)
                    {
                        Logging.Write("    <Mailbox X=\"{0}\" Y=\"{1}\" Z=\"{2}\" />", box.X, box.Y, box.Z);
                    }
                }
                Logging.Write("  </Mailboxes>");
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    XDocument badnodes = XDocument.Load(ofd.FileName);
                    Logging.Write("<Blackspots>");
                    foreach (XElement bad_location in badnodes.Descendants("nodesBlacklist").First().Elements("bad_location"))
                    {
                        string name = bad_location.Element("name").Value;
                        string waypoint = bad_location.Element("waypoint").Value;
                        string[] points = waypoint.Split(' ');
                        string comment = bad_location.Element("comment").Value;
                        Logging.Write("  <Blackspot X=\"{0}\" Y=\"{1}\" Z=\"{2}\" Radius=\"10\" /> <!-- {3} - {4} -->", points[0], points[1], points[2], name, comment);
                    }
                    Logging.Write("</Blackspots>");
                }
            }
        }

        #endregion

        #region events

        void BotEvents_OnBotStart(EventArgs args)
        {
            LoadProfile();
        }

        void Profile_OnNewProfileLoaded(BotEvents.Profile.NewProfileLoadedEventArgs args)
        {
            LoadProfile();
        }

        void Profile_OnNewOuterProfileLoaded(BotEvents.Profile.NewProfileLoadedEventArgs args)
        {
            LoadProfile();
        }

        void Profile_OnUnknownProfileElement(object sender, UnknownProfileElementEventArgs e)
        {
            if (e.Element.Name == "Waypoint")
            {
                try
                {
                    string[] s = e.Element.Value.Split(' ');
                    float x = float.Parse(s[0]);
                    float y = float.Parse(s[1]);
                    float z = float.Parse(s[2]);
                    waypoints.Add(new WoWPoint(x, y, z));
                }
                catch { }
            }
            if (ignored_gb_elements.Contains(e.Element.Name.LocalName))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region load profile

        void LoadProfile()
        {
            if (waypoints.Count <= 0)
            {
                return;
            }
            Logging.Write("GBProfileLoader: Converting Gatherbuddy Profile...");
            string name = "Gatherbuddy - " + ProfileManager.XmlLocation.Substring(ProfileManager.XmlLocation.LastIndexOf("\\") + 1);
            XElement root = new XElement("HBProfile");
            root.Add(new XElement("Name", name));
            root.Add(new XElement("MinDurability", "0.4"));
            root.Add(new XElement("MinFreeBagSlots", 1));
            root.Add(new XElement("MinLevel", "1"));
            root.Add(new XElement("MaxLevel", "99"));
            root.Add(new XElement("Factions", "99999"));
            root.Add(new XElement("MailGrey", "False"));
            root.Add(new XElement("MailWhite", "True"));
            root.Add(new XElement("MailGreen", "True"));
            root.Add(new XElement("MailBlue", "True"));
            root.Add(new XElement("MailPurple", "True"));
            root.Add(new XElement("SellGrey", "True"));
            root.Add(new XElement("SellWhite", "True"));
            root.Add(new XElement("SellGreen", "False"));
            root.Add(new XElement("SellBlue", "False"));
            root.Add(new XElement("SellPurple", "False"));
            root.Add(new XElement("SellPurple", "False"));
            root.Add(new XElement("Vendors"));
            root.Add(new XElement("Mailboxes"));
            XElement hotspots = new XElement("Hotspots");
            foreach (WoWPoint p in waypoints)
            {
                hotspots.Add(new XElement("Hotspot",
                    new XAttribute("X", p.X),
                    new XAttribute("Y", p.Y),
                    new XAttribute("Z", p.Z)));
            }
            waypoints.Clear();
            root.Add(hotspots);
            string path = Logging.ApplicationPath + "\\Default Profiles\\" + name;
            root.Save(path);
            ProfileManager.LoadNew(path);
        }

        #endregion

    }
}
