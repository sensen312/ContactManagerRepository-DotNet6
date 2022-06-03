using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super
{
    internal class ContactRepository
    {
        public List<Contact> ReadAll()
        {
            var contacts = new List<Contact>();
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contact";
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var contact = new Contact()
                    {
                        ID = reader.GetGuid("ID")
                    };
                    contact.ID = reader.GetGuid("ID");
                    contact.LastName = reader.GetString("LastName");
                    contact.FirstName = reader.GetString("FirstName");
                    contact.BirthDate = reader.GetDateTime("BirthDate");
                    contacts.Add(contact);
                }
                Console.WriteLine("-----------------");
                Console.WriteLine("CONTACTS:");
                foreach (var contact in contacts)
                {
                    Console.WriteLine($"{contact.ID}, {contact.LastName}, {contact.FirstName}, {contact.BirthDate}");
                }
                Console.WriteLine("-----------------");
            }

            return contacts;
        }

        public void Insert(Contact contact)
        {
            var sql = "INSERT INTO Contact(ID, LastName, FirstName, BirthDate) VALUES (@ID, @LastName, @FirstName, @BirthDate)";
            using (var connection = GetConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", contact.ID);
                command.Parameters.AddWithValue("@LastName", contact.LastName);
                command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                command.Parameters.AddWithValue("@BirthDate", contact.BirthDate);

                connection.Open();
                var rowsInserted = command.ExecuteNonQuery();
            }
        }
        public void CreateTable()
        {
            var sql = "CREATE TABLE Contact (ID UNIQUEIDENTIFIER PRIMARY KEY, LastName VARCHAR(50) NOT NULL, FirstName VARCHAR(50) NOT NULL, BirthDate DateTime NOT NULL);";
            using (var connection = GetConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var rowsInserted = command.ExecuteNonQuery();
            }
        }
        public void Delete(Guid ID)
        {
            var sql = "Delete FROM Contact WHERE ID = (@ID);";
            using (var connection = GetConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", ID);
                connection.Open();
                var rowsInserted = command.ExecuteNonQuery();
            }
        }
        public void DeleteAll()
        {
            var sql = "Delete FROM Contact";
            using (var connection = GetConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var rowsInserted = command.ExecuteNonQuery();
            }
        }
        public void DeleteTable()
        {
            var sql = "DROP TABLE Contact";
            using (var connection = GetConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var rowsInserted = command.ExecuteNonQuery();
            }
        }
        public void Update(Guid ID, string fname, string lname, string bday)
        {
            Contact contact = Read(ID);


            var dateTime = DateTime.MinValue;

            if (string.IsNullOrEmpty(fname))
                fname = contact.FirstName;
            if (string.IsNullOrEmpty(lname))
                lname = contact.LastName;
            if (string.IsNullOrEmpty(bday))
                dateTime = contact.BirthDate;
            else
            {
                dateTime = DateTime.Parse(bday);
                contact.BirthDate = dateTime;
            }

            var sql = "UPDATE Contact SET LastName = @LastName, FirstName = @FirstName, BirthDate = @BirthDate WHERE ID = (@ID);";
            using (var connection = GetConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", contact.ID);
                command.Parameters.AddWithValue("@LastName", lname);
                command.Parameters.AddWithValue("@FirstName", fname);
                command.Parameters.AddWithValue("@BirthDate", dateTime);

                connection.Open();
                var rowsInserted = command.ExecuteNonQuery();
            }
        }
        public Contact Read(Guid ID) //in theory you would pass in the ID
        {
            var contact = new Contact();
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contact WHERE ID = (@ID);";
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ID", ID);

                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    contact.ID = reader.GetGuid("ID");
                    contact.LastName = reader.GetString("LastName");
                    contact.FirstName = reader.GetString("FirstName");
                    contact.BirthDate = reader.GetDateTime("BirthDate");

                }

            }

            return contact;
        }
        private static SqlConnection GetConnection()
        {
            return new SqlConnection("Server=(local);Database=ContactManager;Trusted_Connection=true;");
        }

    }
}
    