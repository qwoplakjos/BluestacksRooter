using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluestacksRooter
{
    public partial class Form1 : Form
    {

        string windrive = "";
        string bluestacksFilePath = "ProgramData\\BlueStacks_nxt\\bluestacks.conf";
        string fullPath = "";

        public Form1()
        {
            InitializeComponent();

            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                comboBox1.Items.Add(drive.Name);
            }

            comboBox1.SelectedIndex = 0;
            windrive = comboBox1.Items[0].ToString();
            SetPath();
        }

        private void RootDevices()
        {
            richTextBox1.Text = "";


            LogChanges($"Opening file {fullPath}...");

            var attributes = File.GetAttributes(fullPath);
            attributes = attributes & ~FileAttributes.ReadOnly;
            File.SetAttributes(fullPath, attributes);

            string file = File.ReadAllText(fullPath);
            file = file.Replace("bst.feature.rooting=\"0\"", "bst.feature.rooting=\"1\"");

            Regex reg = new Regex("bst\\.instance\\..+\\.enable_root_access=\"0\"");

            LogChanges("Finding lines to replace...");

            foreach (Match item in reg.Matches(file))
            {
                string replaced = item.Value.Replace("0", "1");
                file = file.Replace(item.Value, replaced);
            }

            File.WriteAllText(fullPath, file);

            attributes = attributes | FileAttributes.ReadOnly;
            File.SetAttributes(fullPath, attributes);

            LogChanges("Done! All devices now have root!", Color.Green);
        }


        private void UnrootDevices()
        {
            richTextBox1.Text = "";


            LogChanges($"Opening file {fullPath}...");

            var attributes = File.GetAttributes(fullPath);
            attributes = attributes & ~FileAttributes.ReadOnly;
            File.SetAttributes(fullPath, attributes);


            string file = File.ReadAllText(fullPath);

            file = file.Replace("bst.feature.rooting=\"1\"", "bst.feature.rooting=\"0\"");

            Regex reg = new Regex("bst\\.instance\\..+\\.enable_root_access=\"1\"");

            LogChanges("Finding lines to replace...");

            foreach (Match item in reg.Matches(file))
            {
                string replaced = item.Value.Replace("1", "0");
                file = file.Replace(item.Value, replaced);
            }

            File.WriteAllText(fullPath, file);

            attributes = attributes | FileAttributes.ReadOnly;
            File.SetAttributes(fullPath, attributes);

            AppendText("Done! All devices are ", Color.Green);
            AppendText("unrooted", Color.Red);
            LogChanges("!", Color.Green);
        }

        private void SetPath()
        {
            textBox1.Text = Path.Combine(windrive, bluestacksFilePath);
            fullPath = textBox1.Text;
        }

        private void LogChanges(string change, Color? color = null)
        {
            AppendText(change + Environment.NewLine, color);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RootDevices();
            }
            catch (Exception ex)
            {
                LogChanges(ex.Message, Color.Red);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                UnrootDevices();
            }
            catch (Exception ex)
            {
                LogChanges(ex.Message, Color.Red);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            windrive = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            SetPath();
        }


        public void AppendText(string text, Color? color)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = color ?? Color.Black;
            richTextBox1.AppendText(text);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }
    }
}
