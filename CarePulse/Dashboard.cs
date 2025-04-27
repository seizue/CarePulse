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
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;

namespace CarePulse
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadSurveyCounts();
            UpdateChart();
        }

        private Dictionary<DateTime, int> pendingCountsByDate = new Dictionary<DateTime, int>();
        private Dictionary<DateTime, int> postedCountsByDate = new Dictionary<DateTime, int>();

        private void LoadSurveyCounts()
        {
            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey", "FinalizedSurveys");
            string pendingPath = basePath;
            string postedPath = Path.Combine(basePath, "Posted");

            pendingCountsByDate = CountJsonFilesByDate(pendingPath);
            postedCountsByDate = CountJsonFilesByDate(postedPath);

            int pendingCount = pendingCountsByDate.Values.Sum();
            int postedCount = postedCountsByDate.Values.Sum();
            int totalCount = pendingCount + postedCount;

            txtboxPendingSurveyCount.Text = pendingCount.ToString();
            txtboxPostedSurveyCount.Text = postedCount.ToString();
            txtboxTotalSurveyCount.Text = totalCount.ToString();
        }

        private Dictionary<DateTime, int> CountJsonFilesByDate(string directoryPath)
        {
            var countsByDate = new Dictionary<DateTime, int>();

            try
            {
                if (Directory.Exists(directoryPath))
                {
                    var files = Directory.GetFiles(directoryPath, "*.json");
                    if (files.Length == 0)
                    {
                        Console.WriteLine($"No JSON files found in directory: {directoryPath}");
                        return countsByDate;
                    }

                    foreach (var file in files)
                    {
                        string content = File.ReadAllText(file);
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                        if (data != null && data.ContainsKey("Date") && DateTime.TryParse(data["Date"].ToString(), out DateTime date))
                        {
                            if (!countsByDate.ContainsKey(date))
                            {
                                countsByDate[date] = 0;
                            }
                            countsByDate[date]++;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Directory does not exist: {directoryPath}");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                MessageBox.Show($"Error accessing directory: {directoryPath}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return countsByDate;
        }

        private void UpdateChart()
        {
            if (!pendingCountsByDate.Any() && !postedCountsByDate.Any())
            {
                Console.WriteLine("No data available to display in the chart.");
                cartesianChart1.Series = new SeriesCollection();
                cartesianChart1.AxisX = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Date",
                        Labels = new[] { "No Data" }
                    }
                };
                cartesianChart1.AxisY = new AxesCollection
                {
                    new Axis
                    {
                        Title = "Count",
                        LabelFormatter = value => value.ToString("N0")
                    }
                };
                return;
            }

            var allDates = pendingCountsByDate.Keys.Union(postedCountsByDate.Keys).OrderBy(date => date).ToList();
            var pendingValues = allDates.Select(date => pendingCountsByDate.ContainsKey(date) ? pendingCountsByDate[date] : 0).ToList();
            var postedValues = allDates.Select(date => postedCountsByDate.ContainsKey(date) ? postedCountsByDate[date] : 0).ToList();

            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pending",
                    Values = new ChartValues<int>(pendingValues),
                    LineSmoothness = 0 
                },
                new LineSeries
                {
                    Title = "Posted",
                    Values = new ChartValues<int>(postedValues),
                    LineSmoothness = 0 
                }
            };

            cartesianChart1.AxisX = new AxesCollection
            {
                new Axis
                {
                    Title = "Date",
                    Labels = allDates.Select(date => date.ToString("yyyy-MM-dd")).ToArray()
                }
            };

            cartesianChart1.AxisY = new AxesCollection
            {
                new Axis
                {
                    Title = "Count",
                    LabelFormatter = value => value.ToString("N0")
                }
            };
        }
    }
}
