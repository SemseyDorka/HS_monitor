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
using System.Management;
using System.IO;
using Microsoft.Win32;
using System.Management.Instrumentation;


namespace monitor
{
    class Hardware
    {
        public string hardver { get; set; }
    }
    class Software
    {
        public string SoftwareNev { get; set; }
        public string Verzio { get; set; }
        public Software(string sor)
        {
            string[] resz = sor.Split(',');
            SoftwareNev = resz[0];
            Verzio = resz[1];
        }
    }
    public partial class MainWindow : Window
    {
        List<Software> szoftverllista = new List<Software>();
        List<string> szoftverek = new List<string>();
        List<string> hardverek = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            DataGridSoftware.DataContext = szoftverllista;
            ManagementObjectSearcher sw = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            foreach (var item in sw.Get())
            {
                szoftverek.Add((item["Name"] + "," + item["Version"]));
            }
            foreach (var item in szoftverek)
            {
                szoftverllista.Add(new Software(item));
            }
            DataGridSoftware.ItemsSource = szoftverllista;
          
        }
        private void CPU()
        {
            ManagementClass sw = new ManagementClass("Win32_Processor");
            var providers = sw.GetInstances();

            foreach (var item in providers)
            {

                string cpunev = Convert.ToString(item["Name"]);
                ListHardtware.Items.Add("Processzor:" + "" + cpunev.ToString());
            }
        }
        private void Alaplap()
        {
            ManagementClass sw = new ManagementClass("Win32_BaseBoard");
            var providers = sw.GetInstances();

            foreach (var item in providers)
            {

                string alplapnev = Convert.ToString(item["Model"]);
                ListHardtware.Items.Add("Alaplap:" + "" + alplapnev.ToString());
            }
        }
        private void GPU()
        {
            ManagementClass sw = new ManagementClass("Win32_VideoController");
            var providers = sw.GetInstances();

            foreach (var item in providers)
            {

                string GPUnev = item["Name"].ToString();
                ListHardtware.Items.Add("Videó kártya: " + "" + GPUnev.ToString());
            }
        }
        private void Op_rendszer()
        {
            ManagementClass sw = new ManagementClass("Win32_OperatingSystem");
            var providers = sw.GetInstances();

            foreach (var item in providers)
            {
                string rendszer_verzio = item["Version"].ToString();
                string rendszer_nev = item["Caption"].ToString();
                ListHardtware.Items.Add("Operációs rendszer: " + "" + rendszer_nev.ToString());
                ListHardtware.Items.Add("Operációs rendszer verzió: " + "" + rendszer_verzio.ToString());
            }
        }

        private void Button_software_Click(object sender, RoutedEventArgs e)
        {
            DataGridSoftware.Visibility = Visibility.Visible;
           ListHardtware.Visibility = Visibility.Hidden;
        }

        private void Button_hardware_Click(object sender, RoutedEventArgs e)
        {
            ListHardtware.Visibility = Visibility.Visible;
            DataGridSoftware.Visibility = Visibility.Hidden;
            CPU();
            GPU();
            Op_rendszer();


        }

        private void mentes_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridSoftware.Visibility==Visibility.Visible)
            {
                DataGridSoftware.SelectAllCells();
                ApplicationCommands.Copy.Execute(null, DataGridSoftware);
                DataGridSoftware.UnselectAllCells();
                String eredmeny = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                SaveFileDialog adat_mentes = new SaveFileDialog();
                adat_mentes.Filter = "CSV állományok (.csv)|*.csv";
                adat_mentes.ShowDialog();
                StreamWriter wr = new StreamWriter(adat_mentes.FileName, false, Encoding.UTF8);
                foreach (var item in eredmeny)
                {
                    wr.Write(item);
                }
                wr.Close();
            }
            else
            {
                SaveFileDialog adat_mentes = new SaveFileDialog();
                adat_mentes.Filter= "txt állományok (.txt)|*.txt";
                adat_mentes.ShowDialog();
                StreamWriter wr = new StreamWriter(adat_mentes.FileName, false, Encoding.UTF8);
                foreach (var item in ListHardtware.Items)
                {
                    wr.WriteLine(item);
                }
                wr.Close();

            }


        }
    }
}
