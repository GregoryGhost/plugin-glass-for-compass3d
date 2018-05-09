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
using GlassModel;

namespace GlassPlugin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _nameProgram = "Построитель стакана";

        public MainWindow()
        {
            InitializeComponent();

            _style = (Style)this.TryFindResource("TextBoxInError");
            this.Title = _nameProgram;
        }

        private void ShowBuildMessage(bool resultBuilding)
        {
            string success;
            MessageBoxImage styleBox;
            if (resultBuilding)
            {
                success = "успех";
                styleBox = MessageBoxImage.Information;
            }
            else
            {
                success = "провал";
                styleBox = MessageBoxImage.Warning;
            }
            var msg = String.Format("Постройка модели в Компасе завершилась - {0} ", success);
            MessageBox.Show(msg,
                "Аля Компас 3д",
                MessageBoxButton.OK,
                styleBox);
        }

        private void Build_Click(object sender, RoutedEventArgs e)
        {
            var fakeGlass = _glasses.SelectedGlass;

            var isValid = fakeGlass.BuildModel();

            var msg = isValid ? "Стакан успешно построен."
                : "Исправьте параметры стакана.";
            var typeMessage = isValid ? MessageBoxImage.Information
                : MessageBoxImage.Warning;

            MessageBox.Show(msg,
                _nameProgram,
                MessageBoxButton.OK,
                typeMessage);
        }

        GlassesViewModel _glasses;
        MainWindow _main;
        Style _style;

        private void GenTextBox(List<Tuple<string, bool, string>> nameProp,
            GlassViewModel selected)
        {
            const int width = 100;
            const int widthLabel = 140;
            const int margin = 5;

            _main.test.Children.Clear();

            foreach (var prop in nameProp)
            {
                if (prop.Item2 == false)
                {
                    var labelProp = prop.Item1;

                    var t1 = new TextBox
                    {
                        Style = _style,
                        Width = width
                    };

                    var bind = new Binding
                    {
                        Source = selected,
                        Path = new PropertyPath(labelProp),
                        ValidatesOnDataErrors = true,
                        ValidatesOnExceptions = true,
                        UpdateSourceTrigger = 
                            UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay
                    };

                    t1.SetBinding(TextBox.TextProperty, bind);

                    var textProp = prop.Item3;

                    var l1 = new Label
                    {
                        Content = String.Format("{0}", textProp),
                        Width = widthLabel,
                        HorizontalContentAlignment = 
                            HorizontalAlignment.Right
                    };

                    var m = new Thickness();
                    m.Top = margin;

                    var st = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = m
                    };
                    st.Children.Add(l1);
                    st.Children.Add(t1);

                    _main.test.Children.Add(st);
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if (cmbbox.SelectedIndex == -1) return;

            var selected = _glasses.SelectedGlass;

            var nameProp = selected.Properties;

            GenTextBox(nameProp, selected);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var index = cmbbox.SelectedIndex;
            cmbbox.SelectionChanged += ComboBox_SelectionChanged;
            _main = (MainWindow)Application.Current.MainWindow;

            _glasses = this.Resources["Glasses"] 
                as GlassesViewModel;
            cmbbox.SelectedIndex = -1;
            cmbbox.SelectedIndex = index;
        }
    }
}
