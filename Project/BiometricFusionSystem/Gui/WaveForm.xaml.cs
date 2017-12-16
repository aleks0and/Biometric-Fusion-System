using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui
{
    /// <summary>
    /// Interaction logic for WaveForm.xaml
    /// </summary>
    public partial class WaveForm : UserControl
    {
        public WaveForm()
        {
            InitializeComponent();
        }

        public List<Line> Samples
        {
            get { return (List<Line>)GetValue(SamplesProperty); }
            set { SetValue(SamplesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SamplesProperty =
            DependencyProperty.Register("Samples", typeof(List<Line>), typeof(WaveForm), 
                new PropertyMetadata(default(List<Line>), OnSamplesChanged));


        private static void OnSamplesChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var waveForm = source as WaveForm;
            AddToCanvas(e.NewValue as List<Line>, waveForm.Width, waveForm.Height, waveForm.canvas);
        }
        private static void AddToCanvas(List<Line> samples, double width, double height, Canvas target)
        {
            if(samples == null)
            {
                return;
            }
            target.Children.Clear();
            double min = samples.Min(l => l.Y1);
            double max = samples.Max(l => l.Y1);
            for (int i = 0; i < samples.Count; i++)
            {
                samples[i].X1 = Remap(samples[i].X1, 0, samples.Count - 1, 0, width - 1);
                samples[i].X2 = samples[i].X1;
                samples[i].Y1 = Remap(samples[i].Y1, min, max, 0, height - 1);
                samples[i].Y2 = (height / 2) + samples[i].Y1;
                samples[i].Y1 = (height / 2) - samples[i].Y1;
                samples[i].Visibility = Visibility.Visible;
                samples[i].Stroke = Brushes.Red;
                samples[i].StrokeThickness = 0.1;
                target.Children.Add(samples[i]);
            }
        }

        private static double Remap(double value, double preMin, double preMax, double postMin, double postMax)
        {
            return (value - preMin) / (preMax - preMin) * (postMax - postMin) + postMin;
        }


    }
}
