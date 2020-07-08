using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Oxyplot_WPF_test
{
    public class MainViewModel
    {

        /// <summary>
        /// Plot title
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        ///  Plot data entries X and Y set
        /// </summary>
        public List<DataPoint> Points { get; private set; }
        public int updateInfoInterval { get; set; } = 10;

        public PlotModel UpperModel;
        LineSeries upper_series = new LineSeries();
        Axis xAxis, yAxis;
        Timer UpdateInfoTimer;
        long time = 0;


        /// <summary>
        /// Default constructor to create plot model
        /// </summary>
        public MainViewModel()
        {
            UpperModel = new PlotModel();
            xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 900,
                MajorStep = 60,
                MinorStep = 8,
                TickStyle = TickStyle.Inside,
                LabelFormatter = XAxisFormatter,
                IsZoomEnabled = false,
                IsPanEnabled = true
            };

            yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 1000,
                MajorStep = 100,
                MinorStep = 10,
                TickStyle = TickStyle.Inside,
                IsZoomEnabled = false,
                IsPanEnabled = false
            };

            UpperModel.Axes.Add(xAxis);
            UpperModel.Axes.Add(yAxis);

            UpperModel.Series.Add(upper_series);
            UpdateInfoTimer = new Timer(UpdateInfoCallback, null, 0, updateInfoInterval);

        }
        /// <summary>
        /// Update the Plot
        /// </summary>
        /// <param name="state"></param>
        private void UpdateInfoCallback(object state)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                time++;
                Random rnd = new Random();
                AddDataPoints(time, rnd.Next(100, 300));
                UpperModel.InvalidatePlot(true);
                double panStep = xAxis.Transform(-0.5 + xAxis.Offset);
                xAxis.Pan(panStep);
                
            });
        }

        private string XAxisFormatter(double i)
        {
            i = (i * updateInfoInterval / 1000.0);
            int seconds = (int)i % 60;
            int minutes = (int)i / 60;
            return String.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void SetTitle(string _title)
        {
            Title = _title;
        }


        public void AddDataPoints(double interval, double y)
        {
            upper_series.Points.Add(new DataPoint(interval, y));
        }


    }
}
