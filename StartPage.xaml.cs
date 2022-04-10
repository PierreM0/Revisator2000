using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace Revisator2000
{
    /// <summary>
    /// Logique d'interaction pour StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        static int lineFinder(string fileName, string begginWith)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            for (int i = 0; i < arrLine.Length; i++)
            {
                if (arrLine[i].StartsWith(begginWith))
                {
                    return i+1;
                }
            }
            return -1;
        }

        public StartPage()
        {
            InitializeComponent();

            string config_file = AppDomain.CurrentDomain.BaseDirectory + "config.cfg";
            if (!File.Exists(config_file))
            {
                string createText = "last_path: ";
                File.WriteAllText(config_file, createText, Encoding.UTF8);
            }
            int num_line = lineFinder(config_file, "last_path: ");
            if (num_line >= 0)
            {
                string lp = File.ReadAllLines(config_file)[num_line - 1];
                path_text_box.Text = lp.Replace("last_path: ", string.Empty);
            }
        }
        
        private void path_change_Click(object sender, RoutedEventArgs e)
        {
            string config_file = AppDomain.CurrentDomain.BaseDirectory + "config.cfg";
            lineChanger("last_path: " + path_text_box.Text, config_file, lineFinder(config_file, "last_path: "));
            

            if (path_text_box.Text.EndsWith(".txt") || path_text_box.Text == "")
            {
                bool random = check_box_random_mode.IsChecked.HasValue ? check_box_random_mode.IsChecked.Value : false;
                MainPage p = new MainPage(path_text_box.Text, random);
                this.NavigationService.Navigate(p);
            }
            else
                MessageBox.Show("The file is not in .txt format, try another file.", "Wrong format", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void open_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
                path_text_box.Text = ofd.FileName;
        }

        private void path_text_box_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                path_text_box.Text = files[0];
            }
        }

        private void path_text_box_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
    }
}
