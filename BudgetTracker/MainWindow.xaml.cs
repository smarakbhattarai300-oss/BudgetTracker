using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BudgetTracker
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Transaction> transactions =
            new ObservableCollection<Transaction>();

        public MainWindow()
        {
            InitializeComponent();
            TransactionsGrid.ItemsSource = transactions;
            DateInput.SelectedDate = DateTime.Today;
            UpdateTotals();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionInput.Text))
            {
                MessageBox.Show("Please enter a description.");
                return;
            }

            decimal amount;
            if (!decimal.TryParse(AmountInput.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount greater than 0.");
                return;
            }

            var typeItem = (ComboBoxItem)TypeInput.SelectedItem;

            transactions.Add(new Transaction
            {
                Date = DateInput.SelectedDate ?? DateTime.Today,
                Description = DescriptionInput.Text.Trim(),
                Category = string.IsNullOrWhiteSpace(CategoryInput.Text) ? "Other" : CategoryInput.Text.Trim(),
                Type = typeItem.Content.ToString(),
                Amount = amount
            });

            DescriptionInput.Clear();
            AmountInput.Clear();
            DescriptionInput.Focus();

            UpdateTotals();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = TransactionsGrid.SelectedItem as Transaction;
            if (selected == null)
            {
                MessageBox.Show("Select a transaction to delete.");
                return;
            }

            transactions.Remove(selected);
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            decimal income = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
            decimal expense = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
            decimal balance = income - expense;

            IncomeText.Text = "Income: " + income.ToString("N2");
            ExpenseText.Text = "Expenses: " + expense.ToString("N2");
            BalanceText.Text = "Balance: " + balance.ToString("N2");
            BalanceText.Foreground = balance < 0 ? Brushes.Red : Brushes.Black;
        }
    }
}