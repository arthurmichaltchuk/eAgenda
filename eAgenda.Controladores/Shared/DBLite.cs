using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Configuration;

namespace eAgenda.Controladores.Shared
{
    public static class DBLite
    {
        public static int Insert(string sql, Dictionary<string, object> parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Db.connectionString))
            {

                SQLiteCommand command = new SQLiteCommand(sql.AppendSelectIdentity(), connection);

                command.SetParameters(parameters);

                connection.Open();

                int id = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return id;
            }
        }

        public static void Update(string sql, Dictionary<string, object> parameters = null)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Db.connectionString))
            {

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void Delete(string sql, Dictionary<string, object> parameters)
        {
            Update(sql, parameters);
        }

        public static List<T> GetAll<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters = null)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Db.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                var list = new List<T>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = convert(reader);
                        list.Add(obj);
                    }
                }
                connection.Close();
                return list;
            }
        }

        public static T Get<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Db.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                T t = default;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        t = convert(reader);
                }
                connection.Close();
                return t;
            }
        }

        public static bool Exists(string sql, Dictionary<string, object> parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Db.connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParameters(parameters);

                connection.Open();

                int numberRows = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return numberRows > 0;
            }
        }

        public static void SetParameters(this SQLiteCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return;

            foreach (var parameter in parameters)
            {
                string name = parameter.Key;

                object value = parameter.Value.IsNullOrEmpty() ? DBNull.Value : parameter.Value;

                SQLiteParameter dbParameter = new SQLiteParameter(name, value);

                command.Parameters.Add(dbParameter);
            }
        }

        private static string AppendSelectIdentity(this string sql)
        {
            return sql + ";SELECT last_insert_rowid()";
        }

        private static bool IsNullOrEmpty(this object value)
        {
            return (value is string && string.IsNullOrEmpty((string)value)) ||
                    value == null;
        }
    }
}
