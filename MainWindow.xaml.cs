using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Revisator2000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int SIZE = 100;

        private string[] TabUp = new string[SIZE];
        private int nbTabUp = 0;

        private string[] TabDown = new string[SIZE];
        private int nbTabDown = 0;

        private int ctrTab = 0;
        private int ctrRV = 0;

        private void path_change(string path)
        {
            string[] TabAux = new string[SIZE];
            
            if (path == "")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "/TabUp.txt";
            }
            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = "Hello and Welcome, Hello and Welcome" + Environment.NewLine;
                File.WriteAllText(path, createText, Encoding.UTF8);
            }


            TabAux = File.ReadAllText(path).Split(new char[] { '\n' }, StringSplitOptions.TrimEntries);

            for (int i = 0; i < TabAux.Length; i++)
            {
                string[] inter = TabAux[i].Split(new char[] { ',' }, StringSplitOptions.TrimEntries);
                TabUp[i] = inter[0];
                TabDown[i] = inter[1];
            }

            nbTabUp = nbTabDown = TabUp.Length;
        }
        private void ChangeFormula(bool Next = false, bool Side = false)
        {
            if (Next)
                ctrTab++;
            if (ctrTab >= nbTabUp || ctrTab >= nbTabDown)
                ctrTab = 0;
            if (Side)
                ctrRV++;

            if (ctrRV % 2 == 0)
            {
                if (TabUp[ctrTab][0] == '$' && TabUp[ctrTab].EndsWith('$')) // it s a formula
                {
                    text_input.Opacity = 0;
                    formula.Opacity = 100;
                    string fm = TabUp[ctrTab].Replace('$', ' ').Trim();
                    formula.Formula = fm;
                }
                else
                {
                    formula.Opacity = 0;
                    text_input.Opacity = 100;
                    text_input.Text = TabUp[ctrTab];
                }
            }

            else
                if (TabDown[ctrTab][0] == '$' && TabDown[ctrTab].EndsWith('$'))
            {
                text_input.Opacity = 0;
                formula.Opacity = 100;
                string fm = TabDown[ctrTab].Replace('$', ' ').Trim();
                formula.Formula = fm;
            }
            else
            {
                formula.Opacity = 0;
                text_input.Opacity = 100;
                text_input.Text = TabDown[ctrTab];
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            

            
            string path = AppDomain.CurrentDomain.BaseDirectory + "/TabUp.txt";

            path_change(path);

            ChangeFormula();

        }



        private void button_next_Click(object sender, RoutedEventArgs e)
        {
            ChangeFormula(true, false);
        }

        private void button_other_side_Click(object sender, RoutedEventArgs e)
        {
            ChangeFormula(false, true);
        }

        private void button_image_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                
            }
                
        }

        private void path_change_Click(object sender, RoutedEventArgs e)
        {
            ctrTab = 0;
            ctrRV = 0;
            path_change(path_text_box.Text);
            ChangeFormula();
        }
    }
}
