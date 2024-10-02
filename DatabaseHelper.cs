using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Library_Management_System
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            // Update the connection string with your MySQL credentials
            connectionString = "server=localhost;database=LibraryDB;uid=root;pwd=yourpassword;";
        }

        // Get all books
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            string query = "SELECT * FROM Books";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Book book = new Book
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                            Author = reader.IsDBNull(reader.GetOrdinal("Author")) ? "" : reader.GetString("Author"),
                            Price = reader.GetDecimal("Price"),
                            QtyOnHand = reader.GetInt32("QtyOnHand")
                        };
                        books.Add(book);
                    }
                }
            }

            return books;
        }

        // Add a new book
        public void AddBook(Book book)
        {
            string query = "INSERT INTO Books (Name, Description, Author, Price, QtyOnHand) VALUES (@Name, @Description, @Author, @Price, @QtyOnHand)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", book.Name);
                cmd.Parameters.AddWithValue("@Description", book.Description);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@QtyOnHand", book.QtyOnHand);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Remove a book by Id
        public void RemoveBook(int id)
        {
            string query = "DELETE FROM Books WHERE Id = @Id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Update a book (Optional)
        public void UpdateBook(Book book)
        {
            string query = "UPDATE Books SET Name=@Name, Description=@Description, Author=@Author, Price=@Price, QtyOnHand=@QtyOnHand WHERE Id=@Id";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", book.Name);
                cmd.Parameters.AddWithValue("@Description", book.Description);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@QtyOnHand", book.QtyOnHand);
                cmd.Parameters.AddWithValue("@Id", book.Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
