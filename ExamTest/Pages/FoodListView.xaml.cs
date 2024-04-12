using ExamTest.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamTest.Pages
{
    /// <summary>
    /// Логика взаимодействия для FoodListView.xaml
    /// </summary>
    public partial class FoodListView : Page, INotifyPropertyChanged
    {
        public List<Foods> db = new List<Foods>() 
        {
            new Foods() {Id = 1, Name = "Dogs1", Price = 100, Kg = 10},
            new Foods() {Id = 2, Name = "Dogs2", Price = 100, Kg = 10},
            new Foods() {Id = 3, Name = "Dogs3", Price = 100, Kg = 10},
            new Foods() {Id = 4, Name = "Dogs4", Price = 100, Kg = 10},
            new Foods() {Id = 5, Name = "Dogs5", Price = 100, Kg = 10},
            new Foods() {Id = 6, Name = "Dogs6", Price = 100, Kg = 10},
            new Foods() {Id = 7, Name = "Dogs7", Price = 100, Kg = 10},
            new Foods() {Id = 8, Name = "Dogs8", Price = 100, Kg = 10}
        };

        private int countInPage = 3;
        private int maxPageCount;
        private int currentPage = 1;
        private int selectedIndexSort = 0;
        public int SelectedIndexSort 
        {
            get
            {
                return selectedIndexSort;
            }
            set
            {
                selectedIndexSort = value;
                RefreshList();
            }
        }
        public int CurrentPage 
        {
            get 
            {
                return currentPage;
            }
            set 
            {
                if (value <= maxPageCount && value > 0) 
                {
                    currentPage = value;
                    RefreshList();
                }
            }
        }

        public List<Foods> foodListBuffer = new List<Foods>() { };
        public List<Foods> Foods { get { return foodListBuffer; } }

        public FoodListView()
        {
            this.InitializeComponent();
            RefreshList();
            this.DataContext = this;
        }

        public void RefreshList() 
        {
            foodListBuffer = db.ToList();

            if (selectedIndexSort == 1)
            {
                foodListBuffer = foodListBuffer.OrderBy(x => x.Name).ToList();
            }
            else if (selectedIndexSort == 2)
            {
                foodListBuffer = foodListBuffer.OrderByDescending(x => x.Name).ToList();
            }

            maxPageCount = (int)Math.Ceiling((double)db.Count / countInPage);
            foodListBuffer = foodListBuffer.Skip((currentPage - 1) * countInPage).Take(countInPage).ToList();
            OnPropertyChanged("Foods");
            OnPropertyChanged("CurrentPage");
        }

        private void AddFood_Click(object sender, RoutedEventArgs e)
        {
            Foods foods = new Foods();

            var editFood = new EditFoodWindow();
            editFood.DataContext = foods;

            editFood.ShowDialog();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedFoodEdit = (sender as Button).DataContext as Foods;
            if (selectedFoodEdit == null) return;

            var editFood = new EditFoodWindow();
            editFood.DataContext = selectedFoodEdit;

            editFood.ShowDialog();
        }

        private void BFullBack_Click(object sender, RoutedEventArgs e)
        {
            if(maxPageCount == 0) return;
            currentPage = 1;

            RefreshList();
        }

        private void BFullNext_Click(object sender, RoutedEventArgs e)
        {
            if (maxPageCount == 0) return;
            currentPage = maxPageCount;

            RefreshList();
        }

        private void BBack_Click(object sender, RoutedEventArgs e)
        {
            if (maxPageCount == 0) return;
            if (currentPage == 1)
                currentPage = maxPageCount;
            else currentPage--;

            RefreshList();
        }

        private void BNext_Click(object sender, RoutedEventArgs e)
        {
            if (maxPageCount == 0) return;
            if (currentPage == maxPageCount)
                currentPage = 1;
            else currentPage++;

            RefreshList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }

    public class Foods 
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Kg { get; set; }
        public Foods() { }
    }
}
