using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GridwithWForm
{
    public partial class Form1 : Form
    {
        public OpenFileDialog openFileDialog1;
        public SaveFileDialog saveFileDialog1;
        private int maxBind = 1;
        private Dictionary<int, BindingSource> bindings;
        public BindingSource Data { get; set; }
        private SeriesChartType chartType = SeriesChartType.Line;
        public void PaintGraphic()
        {
            chart1.DataSource = null;
            chart1.Series[0].ChartType = chartType;
            chart1.DataSource = Data;
        }

        public Form1()
        {
            bindings = new Dictionary<int, BindingSource>();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            Data = new BindingSource(); 
            Data.Add(new Point_() { X = 0, Y = 0 });
            Data.Add(new Point_() { X = 1, Y = 1 });
            Data.Add(new Point_() { X = 2, Y = 4 });

            bindings.Add(1, Data);

            InitializeComponent();
            dataGridView1.DataSource = Data;
            comboBoxtable.Items.Add(1);

            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            //Data.ListChanged += new ListChangedEventHandler(Data_ListChanged);
           // dataGridView1.DataSourceChanged += new EventHandler(DataGridView1_DataSourceChanged);
            //dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            //dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(DataGridView1_CellValueChanged);
            dataGridView1.SelectionChanged += new EventHandler(DataGridView1_SelectionChanged);
        }

        //private void Data_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    PaintGraphic();
        //}

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            PaintGraphic();
        }

        //private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    PaintGraphic();
        //}

        //private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    PaintGraphic();
        //}

        //private void DataGridView1_DataSourceChanged(Object sender, EventArgs e)
        //{

        //    MessageBox.Show("You are in the DataGridView.DataSourceChanged event.");

        //    PaintGraphic();

        //}

        private void AddButton_Click(object sender, EventArgs e)
        {
            Data.Add(new Point_() { X = 4, Y = 16 });
        }
        private void DrawAsLines_Click(object sender, EventArgs e)
        {
            chartType = SeriesChartType.Line;
            PaintGraphic();
        }
        private void DrawAsSpline_Click(object sender, EventArgs e)
        {
            chartType = SeriesChartType.Spline;
            PaintGraphic();
        }

        private void Load_button_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string path = openFileDialog1.FileName;

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                Data = new BindingSource();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(new char[] { ' ' });
                    Data.Add(new Point_() { X = Convert.ToDouble(words[0]), Y = Convert.ToDouble(words[1]) });
                }
                
            }
            maxBind++;
            bindings.Add(maxBind, Data);
            comboBoxtable.Items.Add(maxBind);
            label1.Text = "Graphic " + maxBind.ToString();
            comboBoxtable.SelectedItem = maxBind;
            dataGridView1.DataSource = Data;
            
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string writePath = saveFileDialog1.FileName;

            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                foreach (var item in Data.List)
                {
                    Point_ data = (Point_)item;
                    sw.Write(data.X);
                    sw.Write(" ");
                    sw.Write(data.Y);
                    sw.WriteLine();
                }
            }

        }

        private void Del_button_Click(object sender, EventArgs e)
        {
            if (bindings.Count == 1)
            {
                MessageBox.Show("Нельзя удалить последний");
                return;
            }
            bindings.Remove((int)comboBoxtable.SelectedItem);
            int i = comboBoxtable.Items.IndexOf((int)comboBoxtable.SelectedItem);
            comboBoxtable.Items.RemoveAt(i);
            var tmp = bindings.LastOrDefault();
            Data = tmp.Value;
            label1.Text = "Graphic " + tmp.Key.ToString();
            comboBoxtable.SelectedItem = tmp.Key;
            dataGridView1.DataSource = Data;
            PaintGraphic();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string selectedState = comboBox1.SelectedItem.ToString();
            switch (selectedState)
            {
                case "Draw as lines":
                    {
                        chartType = SeriesChartType.Line;
                        PaintGraphic();
                    }
                    break;
                case "Draw as spline":
                    {
                        chartType = SeriesChartType.Spline;
                        PaintGraphic();
                    }
                    break;
                default:
                    break;
            }
        }

        private void comboBoxtable_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Data = bindings[(int)comboBoxtable.SelectedItem];
            label1.Text = "Graphic " + comboBoxtable.SelectedItem.ToString();
            dataGridView1.DataSource = Data;
            
        }
    }

    public class Point_
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
