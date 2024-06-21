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
using System.Windows.Shapes;

namespace Panel
{
    /// <summary>
    /// Логика взаимодействия для IngredientsWindow.xaml
    /// </summary>
    public partial class IngredientsWindow : Window
    {
        private readonly Database.Database.FoodDBEntities connection;

        public IngredientsWindow()
        {
            InitializeComponent();
            connection = new Database.Database.FoodDBEntities();
            LoadIngredients();
        }

        private void LoadIngredients()
        {
            var ingredients = connection.Ingredients.ToList();
            IngredientsListBox.ItemsSource = ingredients;
            IngredientsListBox.DisplayMemberPath = "Name";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(WeightTextBox.Text, out decimal weight) &&
                decimal.TryParse(CaloriesTextBox.Text, out decimal calories))
            {
                var ingredient = new Database.Database.Ingredient
                {
                    Name = NameTextBox.Text,
                    Weight = weight,
                    Calories = calories,
                    Nutrients = NutrientsTextBox.Text
                };

                connection.Ingredients.Add(ingredient);
                connection.SaveChanges();
                LoadIngredients();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения для веса и калорий.");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientsListBox.SelectedItem is Database.Database.Ingredient selectedIngredient)
            {
                // Проверяем, что введенные значения корректны
                if (decimal.TryParse(WeightTextBox.Text, out decimal weight) &&
                    decimal.TryParse(CaloriesTextBox.Text, out decimal calories))
                {
                    selectedIngredient.Name = NameTextBox.Text;
                    selectedIngredient.Weight = weight;
                    selectedIngredient.Calories = calories;
                    selectedIngredient.Nutrients = NutrientsTextBox.Text;

                    connection.Entry(selectedIngredient).State = System.Data.Entity.EntityState.Modified;
                    connection.SaveChanges();
                    LoadIngredients();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректные числовые значения для веса и калорий.");
                }
            }
        }

            private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientsListBox.SelectedItem is Database.Database.Ingredient selectedIngredient)
            {
                connection.Ingredients.Remove(selectedIngredient);
                connection.SaveChanges();
                LoadIngredients();
                ClearForm();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearForm()
        {
            NameTextBox.Clear();
            WeightTextBox.Clear();
            CaloriesTextBox.Clear();
            NutrientsTextBox.Clear();
            IngredientsListBox.SelectedItem = null;
        }

        private void IngredientsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (IngredientsListBox.SelectedItem is Database.Database.Ingredient selectedIngredient)
            {
                NameTextBox.Text = selectedIngredient.Name;
                WeightTextBox.Text = selectedIngredient.Weight.ToString();
                CaloriesTextBox.Text = selectedIngredient.Calories.ToString();
                NutrientsTextBox.Text = selectedIngredient.Nutrients;
            }
        }
    }
}
