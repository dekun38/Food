using Database.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для AddEditDishWindow.xaml
    /// </summary>
    public partial class AddEditDishWindow : Window
    {
        private readonly FoodDBEntities connection;
        public Dish _dish;
        public bool isNewDish;

        // Конструктор по умолчанию для добавления нового блюда
        public AddEditDishWindow()
        {
            InitializeComponent();
            connection = new FoodDBEntities();
            _dish = new Dish();
            isNewDish = true; // Устанавливаем флаг для нового блюда
        }

        // Конструктор с параметром для редактирования существующего блюда
        public AddEditDishWindow(Dish dish)
        {
            InitializeComponent();
            connection = new FoodDBEntities();

            if (dish != null)
            {
                _dish = dish;
                isNewDish = false; // Устанавливаем флаг для существующего блюда

                // Заполняем поля формы данными из существующего блюда
                NameTextBox.Text = _dish.Name;
                WeightTextBox.Text = _dish.Weight.ToString();
                CaloriesTextBox.Text = _dish.Calories.ToString();
                NutrientsTextBox.Text = _dish.Nutrients;
                ImplementationCheckBox.IsChecked = _dish.Implementation;
            }
            else
            {
                _dish = new Dish();
                isNewDish = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new FoodDBEntities())
                {
                    _dish.Name = NameTextBox.Text;
                    _dish.Weight = (decimal)double.Parse(WeightTextBox.Text);
                    _dish.Calories = int.Parse(CaloriesTextBox.Text);
                    _dish.Nutrients = NutrientsTextBox.Text;
                    _dish.Implementation = ImplementationCheckBox.IsChecked ?? false;

                    if (_dish.DishID == 0)
                    {
                        context.Dishes.Add(_dish);
                    }
                    else
                    {
                        var existingEntity = context.Dishes.Find(_dish.DishID); // Находим объект в текущем контексте
                        if (existingEntity != null)
                        {
                            context.Entry(existingEntity).CurrentValues.SetValues(_dish); // Обновляем значения существующего объекта
                        }
                        else
                        {
                            context.Entry(_dish).State = EntityState.Modified; // Если объект не найден в текущем контексте, присоединяем его
                        }
                    }

                    context.SaveChanges();
                    MessageBox.Show("Успешно сохранено.", "Принять", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Обработка ошибок валидации
                StringBuilder sb = new StringBuilder();
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                MessageBox.Show(sb.ToString(), "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении: " + ex.Message, "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрываем окно без сохранения изменений
        }
    }
}
