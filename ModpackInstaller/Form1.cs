using Newtonsoft.Json;
using System;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ModpackInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog Modlist = new OpenFileDialog();
            Modlist.Filter = "Modpack|*.zip";
            Modlist.Title = "Select a Modpack";
 
            if (Modlist.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = Modlist.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ModFolder = new FolderBrowserDialog();
 
            if (ModFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                textBox2.Text = ModFolder.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fileURL;
            ZipFile.ExtractToDirectory(textBox1.Text, textBox2.Text);
            string json = System.IO.File.ReadAllText(textBox2.Text + "//modlist.json");
            dynamic dynJson = JsonConvert.DeserializeObject(json);
                // Determine whether the directory exists.
                if (Directory.Exists(textBox2.Text + "\\mods"))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }
            // Try to create the directory.
            Directory.CreateDirectory(textBox2.Text + "\\mods");
            foreach (var item in dynJson)
            {
                using (var client = new WebClient())
                {
                    if (item.isCurse == "true")
                    {
                        client.DownloadFile("http://minecraft.curseforge.com/projects/" + item.ProjectID + "/files/" + item.FileID + "/download", textBox2.Text + "//mods//" + item.FileName + ".jar");
                    }
                    else
                    {
                        fileURL = item.URL;
                        Console.WriteLine(item.URL);
                        client.DownloadFile(fileURL, textBox2.Text + "//mods//" + item.FileName + ".jar");
                    }
                }
            }
        }
    }
}
