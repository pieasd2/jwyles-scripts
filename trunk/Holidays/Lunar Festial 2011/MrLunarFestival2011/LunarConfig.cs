using System;
using Styx;
using Styx.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MrLunarFestival2011
{
    public partial class LunarConfig : Form
    {
        public LunarConfig()
        {
            InitializeComponent();
            LunarSettings.Instance.Load();
            MountName.Text = LunarSettings.Instance.MountName;
            HarvestDistance.Value = new decimal(LunarSettings.Instance.HarvestDistance);
            ProSel.SelectedItem = LunarSettings.Instance.ProfileSelect;
        }

    
        private void ProSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LunarSettings.Instance.ProfileSelect = ProSel.SelectedItem.ToString();
            Logging.Write(LunarSettings.Instance.ProfileSelect.ToString() + " Selected");
        }

        private void HarvestDistance_ValueChanged(object sender, EventArgs e)
        {
            LunarSettings.Instance.HarvestDistance = int.Parse(HarvestDistance.Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logging.Write("Settings Saved!");
            LunarSettings.Instance.Save();
        }

        private void MountName_TextChanged(object sender, EventArgs e)
        {
            LunarSettings.Instance.MountName = MountName.Text;
        }
    }
}
