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
using GlassPlugin.Models;
using GlassPlugin.Models.ExceptionsOfGlassParameters;

namespace GlassPlugin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Parameters _glass;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TypeGlass typeGlass = TypeGlass.Clean;
            switch (cbTypeGlass.SelectedIndex)
            {
                case (int)TypeGlass.Faceted:
                    typeGlass = TypeGlass.Faceted;
                    break;
                case (int)TypeGlass.Crimp:
                    typeGlass = TypeGlass.Crimp;
                    break;
                case (int)TypeGlass.Clean:
                    typeGlass = TypeGlass.Clean;
                    break;
            }
            Parameters model = new Parameters(Convert.ToDouble(eCountOfFace.Text),
                                               Convert.ToDouble(eDepthBottom),
                                               Convert.ToDouble(eDiameterTop),
                                               Convert.ToDouble(eDiameterBottom),
                                               Convert.ToDouble(eSideDepth),
                                               Convert.ToDouble(eHeightFace),
                                               Convert.ToDouble(eHeight),
                                               typeGlass);

            //Parameters model = new Parameters(-9, 10, 1, 10,
            //                                  2,                                              
            //                                  3,
            //                                  4,
            //                                  typeGlass);
            GlassProxy creator = new GlassProxy();
         //   try
         //   {
         //       creator.CreateModel(model);
         //   }
         //   catch (ArgumentOutOfRangeException eD)
         //   {
         //       MessageBox.Show(eD.Message, eD.Source,
         //MessageBoxButton.OK, MessageBoxImage.Error);
         //   }
        }
    }
}
