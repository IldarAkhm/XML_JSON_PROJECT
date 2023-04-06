using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Drawing.Imaging;

namespace XML_JSON_PROJECT
{
    public partial class Form1 : Form
    {
        public string file;
        public XmlDocument xDoc = new XmlDocument();
        List<Lector> List = new List<Lector>();
        public string myJson = File.ReadAllText(@"./../../my.json");
        public int num;

        public Form1()
        {
            xDoc.Load(@"./../../XMLFile1.xml");
            string myJson = File.ReadAllText(@"./../../my.json");
            InitializeComponent();
            GetXMLfile();
            CreateColumns();
            LoadToDG(List);
            dataGridView1.Hide();
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Type", "Название");
            dataGridView1.Columns.Add("Name", "Имя преподавателя");
            dataGridView1.Columns.Add("Email", "Почта");
            dataGridView1.Columns.Add("Duration", "Длина курса");
            dataGridView1.Columns.Add("PublishDate", "Дата начала");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows= false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            ToolTip tooltip = new ToolTip();
            ToolTip tooltip2 = new ToolTip();

            tooltip2.SetToolTip(button2, "Таблица с сущностями и их свойствами");
            tooltip2.InitialDelay = 200;
            tooltip.SetToolTip(button1, "Выбор загружаемых файлов, сначал будет и Json и XML");
            tooltip.InitialDelay = 200;
            radioButton1.Visible = true;
            radioButton2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            radioButton2.Visible = false;
            radioButton1.Visible = false;
            if (dataGridView1.Visible == true) { dataGridView1.Visible = false; }
            else { dataGridView1.Visible = true; }
        }
        private void GetXMLfile()
        {
            XmlElement root = xDoc.DocumentElement;
            ParseMat(root);
            ParseMatjson();
        }
        private void ParseMat(XmlElement root)
        {
            foreach (XmlElement node in root)
            {
                if (node.Name == "material")
                {
                    Lector j = new Lector();
                    int cnt = 0;
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        cnt++;
                        switch (child.Name)
                        {
                            case "title":
                                j.type = $"{child.InnerText}";
                                break;
                            case "author":
                                j.name = $"{(child.FirstChild.InnerText)}";
                                j.email = $"{(child.LastChild.InnerText)}";
                                break;
                            case "duration":
                                j.duration = child.InnerText;
                                break;
                            case "publishedDate":
                                j.publishDate = Convert.ToDateTime(child.InnerText);
                                break;
                            default:
                                break;
                        }
                        if (cnt == 5)
                        {
                            List.Add(j);
                            cnt = 0;
                            j = new Lector();
                        }
                    }
                }
            }
        }
        private void ParseMatjson()
        {
            JsonNode data = JsonNode.Parse(myJson);
            for (int i = 0; i < 3; i++)
            {
                Lector j = new Lector();
                JsonNode jud = data["entities"][i];
                j.type = $"{(string)jud["title"]} ";
                j.name = $"{((string)jud["author"]["name"])}";
                j.email = (string)jud["author"]["email"];
                j.duration = ((string)jud["duration"]);
                j.publishDate = Convert.ToDateTime((string)jud["publishDate"]);
                List.Add(j);
            }
        }
        
        private void LoadToDG(List<Lector> list)
        {
            foreach (Lector person in list)
            {
                dataGridView1.Rows.Add(person.type, person.name, person.email, person.duration);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            if (radioButton1.Visible == false && radioButton2.Visible == false)
            {
                radioButton1.Visible = true;
                radioButton2.Visible = true;
            }
            else
            {
                radioButton1.Visible = false;
                radioButton2.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            List = new List<Lector>();
            dataGridView1.Rows.Clear();
            file = "json";
            ParseMatjson();
            LoadToDG(List);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            List = new List<Lector>();
            dataGridView1.Rows.Clear();
            XmlElement root = xDoc.DocumentElement;
            file = "XML";
            ParseMat(root);
            LoadToDG(List);
        }
    }
}
