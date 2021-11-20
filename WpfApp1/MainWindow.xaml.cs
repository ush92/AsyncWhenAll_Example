using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource source = null;
        int value = 0;

        public MainWindow()
        {
            InitializeComponent();

            ryzTimeTb.Text = "";
            miesoTimeTb.Text = "";
            FinalResultTb.Text = "";

            btnCancel.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ryzTimeTb.Text = "";
            miesoTimeTb.Text = "";
            FinalResultTb.Text = "";
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            asyncBtn.IsEnabled = false;
            btnCancel.IsEnabled = true;
            ryzTimeTb.Text = "";
            miesoTimeTb.Text = "";

            source = new CancellationTokenSource();
            var token = source.Token;

            var dinnerTasks = new List<Task> { UgotujRyz(token), UpieczMieso(token) };

            FinalResultTb.Text = "Przygotowanie obiadu";
            try
            {
                await Task.WhenAll(dinnerTasks);

                if (dinnerTasks[0].Status.Equals(TaskStatus.RanToCompletion) &&
                    dinnerTasks[1].Status.Equals(TaskStatus.RanToCompletion))
                {
                    FinalResultTb.Text = $"Obiad zrobiony!";
                }
            }
            catch
            {
                FinalResultTb.Text = $"gotowanie obiadu: przerwane";
            }
            finally
            {
                source.Dispose();
            }

            asyncBtn.IsEnabled = true;
            btnCancel.IsEnabled = false;
        }

        private async Task<string> UgotujRyz(CancellationToken cn)
        {
            for (int i = 100; i >=0; i--)
            {
                await Task.Delay(50);
                ryzTimeTb.Text = $"RYZ pozostalo: {i} sekund\n";

                if (cn.IsCancellationRequested)
                {
                    cn.ThrowIfCancellationRequested();
                }
            }
            return "RYZ";
        }

        private async Task<string> UpieczMieso(CancellationToken cn)
        {
            for (int i = 180; i >= 0; i--)
            {
                await Task.Delay(60);
                miesoTimeTb.Text = $"MIESO pozostalo: {i} sekund\n";
                if (cn.IsCancellationRequested)
                {
                    cn.ThrowIfCancellationRequested();
                }
            }
            return "MIESO";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            btnCancel.IsEnabled = false;
            source.Cancel(true);
        }
    }
}
