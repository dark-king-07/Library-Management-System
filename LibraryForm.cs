using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class LibraryForm : Form
    {
        private DatabaseHelper dbHelper;
        private List<Book> books;

        public LibraryForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadBooks();
        }

        private void LoadBooks()
        {
            books = dbHelper.GetAllBooks();
            dataGridViewBooks.DataSource = null;
            dataGridViewBooks.DataSource = books;
            dataGridViewBooks.Columns["Id"].Visible = false; // Hide the Id column if desired
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs (basic validation)
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Book name is required.");
                    return;
                }

                // Create a new Book object
                Book newBook = new Book
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Author = txtAuthor.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    QtyOnHand = int.Parse(txtQtyOnHand.Text)
                };

                // Add to database
                dbHelper.AddBook(newBook);

                // Refresh the book list
                LoadBooks();

                // Clear input fields
                ClearInputs();

                MessageBox.Show("Book added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding book: {ex.Message}");
            }
        }

        private void btnRemoveBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewBooks.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a book to remove.");
                    return;
                }

                // Get the selected book's Id
                int selectedIndex = dataGridViewBooks.SelectedRows[0].Index;
                int bookId = books[selectedIndex].Id;

                // Confirm deletion
                var confirmResult = MessageBox.Show("Are you sure to delete this book?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Remove from database
                    dbHelper.RemoveBook(bookId);

                    // Refresh the book list
                    LoadBooks();

                    MessageBox.Show("Book removed successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing book: {ex.Message}");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBooks();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtDescription.Clear();
            txtAuthor.Clear();
            txtPrice.Clear();
            txtQtyOnHand.Clear();
        }
    }
}
