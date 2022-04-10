using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Revisator2000
{
    /// <summary>
    /// Logique d'interaction pour MainPage.xaml
    /// </summary>
    /// 
    public partial class MainPage : Page
    {


        const int SIZE = 100;

        private string[][] TabCard = new string[SIZE][];
        private int nbTabCard = 0;

        private int ctrTab = 0;
        private int ctrRV = 0;


        /* @string path
         * change the data in tab with data in file */
        private void path_change(string path)
        {
            string[] TabAux;

            if (path == "")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "/TabUp.txt";
            }
            
            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = "Hello and Welcome, Hello and Welcome";
                File.WriteAllText(path, createText, Encoding.UTF8);
            }


            TabAux = File.ReadAllText(path).Split(new char[] { '\n' }, StringSplitOptions.TrimEntries);

            for (int i = 0; i < TabAux.Length; i++)
            {
                string[] inter = TabAux[i].Split(new char[] { ',' }, StringSplitOptions.TrimEntries);
                TabCard[i] = inter;
            }
            nbTabCard = TabAux.Length;
        }

        private void ChangeFormula(bool Next = false, bool Side = false)
        {
            if (Next)
            {
                ctrTab = (ctrTab+1) % nbTabCard ;
                ctrRV = 0;
            }
            if (Side)
            {
                ctrRV = (ctrRV+1) % TabCard[ctrTab].Length;
            }
            
            if (TabCard[ctrTab][ctrRV][0] == '$' && TabCard[ctrTab][ctrRV].EndsWith('$')) // it s a formula
                {
               text_output.Opacity = 0;
               formula.Opacity = 100;
               string fm = TabCard[ctrTab][ctrRV].Replace('$', ' ').Trim();
               formula.Formula = fm;
            }
            else
            {
                formula.Opacity = 0;
                text_output.Opacity = 100;
                text_output.Text = TabCard[ctrTab][ctrRV];
            }
        }
    

        private void randomize_all_tabs()
        {
            int cnt = 0;
            Random rng = new Random();
            TabCard = TabCard.OrderBy(x => rng.Next()).ToArray();
            for (int i = 0; i < TabCard.Length; i++)
                if(TabCard[i] != null)
                {
                    string[] aux = TabCard[i];
                    TabCard[i] = TabCard[cnt];
                    TabCard[cnt++] = aux;
                }
        }

        public MainPage(string path, bool random_mode)
        {
            InitializeComponent();

            path_change(path);

            if (random_mode)
                randomize_all_tabs();

            ChangeFormula();
        }


        private void animate_main_grid()
        {
            Monitor.Enter(this);
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            //da.AutoReverse = true;

            main_grid.BeginAnimation(OpacityProperty, da);
            Monitor.Exit(this);
        }

        private void button_next_Click(object sender, RoutedEventArgs e)
        {   
            Dispatcher.Invoke(animate_main_grid);
            ChangeFormula(true, false);
        }

        private void button_other_side_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(animate_main_grid);
            ChangeFormula(false, true);
        }

        private void button_image_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {

            }
        }

        private void button_back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("StartPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
