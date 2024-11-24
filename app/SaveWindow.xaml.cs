using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для SaveWindow.xaml
    /// </summary>
    public class SaveData : INotifyPropertyChanged
    {
        public string populationName;
        public string PopulationName 
        {
            get { return populationName; }
            set
            {
                populationName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class SaveButtonEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value != "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }

    public partial class SaveWindow : Window
    {
        SaveData saveData { get; set; }
        public SaveWindow()
        {
            InitializeComponent();
            saveData = new SaveData() { PopulationName = "" };
            DataContext = saveData;
        }
        public void SavePopulation(Population population, string Name)
        {
            using(var db = new LibraryContext())
            {
                PopulationEntity pop = new PopulationEntity() { Name=Name,
                                                                Size=population.size,
                                                                num1 = population.nums[0],
                                                                num2 = population.nums[1],
                                                                num3 = population.nums[2]};
                foreach (var indi in population.Members)
                {
                    IndividualEntity indiEntity = new IndividualEntity() { Population = pop };
                    indiEntity.Gens = new List<GenEntity>();
                    foreach (var gen in indi.Genom)
                    {
                        GenEntity genEntity = new GenEntity() { Individual = indiEntity, Width = gen.width, Height = gen.height, Size = gen.size };
                        indiEntity.Gens.Add(genEntity);
                        db.AllGens.Add(genEntity);

                    }
                    pop.Individuals.Add(indiEntity);
                    db.AllIndividuals.Add(indiEntity);
                }
                db.AllPopulations.Add(pop);
                db.SaveChanges();
            }
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if(saveData.PopulationName != "")
            {
                SavePopulation(((MainWindow)Application.Current.MainWindow).data.BestPopulation, saveData.PopulationName);
                this.DialogResult = true;
            }
        }
    }
}
