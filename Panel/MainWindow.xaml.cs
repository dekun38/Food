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
using static System.Net.Mime.MediaTypeNames;

namespace Panel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Database.Database.FoodDBEntities connection;

        public MainWindow()
        {
            InitializeComponent();
            connection = new Database.Database.FoodDBEntities();
            LoadDishes();
        }

        // Загрузка блюд из базы данных и обновление списков
        private void LoadDishes()
        {
            var implementedDishes = connection.Dishes.Where(d => d.Implementation).ToList();
            var unimplementedDishes = connection.Dishes.Where(d => !d.Implementation).ToList();

            ImplementedDishesList.ItemsSource = implementedDishes;
            UnimplementedDishesList.ItemsSource = unimplementedDishes;
        }

        // Перемещение выбранного блюда в список нереализованных
        private void MoveToUnimplementedButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImplementedDishesList.SelectedItem is Database.Database.Dish selectedDish)
            {
                selectedDish.Implementation = false;
                connection.SaveChanges();
                LoadDishes();
            }
        }

        // Перемещение выбранного блюда в список реализованных
        private void MoveToImplementedButton_Click(object sender, RoutedEventArgs e)
        {
            if (UnimplementedDishesList.SelectedItem is Database.Database.Dish selectedDish)
            {
                selectedDish.Implementation = true;
                connection.SaveChanges();
                LoadDishes();
            }
        }

        // Открытие окна добавления нового блюда
        private void AddDishButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditDishWindow = new AddEditDishWindow();
            addEditDishWindow.ShowDialog();
            LoadDishes();
        }

        // Открытие окна редактирования выбранного блюда
        private void EditDishButton_Click(object sender, RoutedEventArgs e)
        {
            Database.Database.Dish selectedDish = null;
            if (ImplementedDishesList.SelectedItem is Database.Database.Dish dishFromImplemented)
            {
                selectedDish = dishFromImplemented;
            }
            else if (UnimplementedDishesList.SelectedItem is Database.Database.Dish dishFromUnimplemented)
            {
                selectedDish = dishFromUnimplemented;
            }

            if (selectedDish != null)
            {
                var addEditDishWindow = new AddEditDishWindow(selectedDish);
                addEditDishWindow.ShowDialog();
                LoadDishes();
            }
        }

        // Удаление выбранного блюда
        private void DeleteDishButton_Click(object sender, RoutedEventArgs e)
        {
            Database.Database.Dish selectedDish = null;
            if (ImplementedDishesList.SelectedItem is Database.Database.Dish dishFromImplemented)
            {
                selectedDish = dishFromImplemented;
            }
            else if (UnimplementedDishesList.SelectedItem is Database.Database.Dish dishFromUnimplemented)
            {
                selectedDish = dishFromUnimplemented;
            }

            if (selectedDish != null)
            {
                connection.Dishes.Remove(selectedDish);
                connection.SaveChanges();
                LoadDishes();
            }
        }

        // Открытие окна с ингредиентами
        private void IngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            var ingredientsWindow = new IngredientsWindow();
            ingredientsWindow.ShowDialog();
        }
    }
}

