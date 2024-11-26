using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using GeneticSquares;

namespace GeneticApplication
{
    /// <summary>
    /// Логика взаимодействия для LoadWindow.xaml
    /// </summary>

    public class LoadData
    {
        public List<int> Ids { get; set; }
        public List<string> Names { get; set; }
        public int Selected { get; set; } = -1;
    }

    public class LoadButtonEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value != -1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }

    public partial class LoadWindow : Window
    {
        public LoadData loadData { get; set; }
        public LoadWindow()
        {
            InitializeComponent();

            using (var db = new LibraryContext())
            {
                List<string> Names = db.AllPopulations.Select(pop => pop.Name).ToList();
                List<int> Ids = db.AllPopulations.Select(pop => pop.Id).ToList();
                loadData = new LoadData() { Ids=Ids, Names = Names };
            }
            DataContext = loadData;

            Binding PopListBoxBind = new()
            {
                Source = loadData,
                Path = new PropertyPath("Selected")
            };
            PopListBox.SetBinding(ListBox.SelectedIndexProperty, PopListBoxBind);

        }

        public void LoadPopulation(Data data)
        {
            using (var db = new LibraryContext())
            {
                int Id = loadData.Ids[loadData.Selected];
                var pop = db.AllPopulations.Where(pop => pop.Id == Id).FirstOrDefault();
                data.BestPopulation = new Population()
                {
                    size = pop.Size,
                    nums = [pop.num1, pop.num2, pop.num3],
                };
                data.A = pop.num1;
                data.B = pop.num2;
                data.C = pop.num3;
                data.Epochs = 0;

                data.BestPopulation.Members = new List<Individual>();
                var IndividualsId = db.AllIndividuals.Where(indi => indi.Population.Id == Id).Select(indi => indi.Id).ToList();
                foreach(var indiId in IndividualsId)
                {
                    Individual indi = new Individual() { nums = [pop.num1, pop.num2, pop.num3] };
                    indi.Genom = new List<Gen>();
                    var Genes = db.AllGens.Where(gen => gen.Individual.Id == indiId);
                    foreach (var genEntity in Genes)
                    {
                        Gen gen = new Gen(genEntity.Width, genEntity.Height, genEntity.Size);
                        indi.Genom.Add(gen);
                    }
                    data.BestPopulation.Members.Add(indi);
                }
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            LoadPopulation(((MainWindow)Application.Current.MainWindow).data);
            this.DialogResult = true;
        }
    }
}
