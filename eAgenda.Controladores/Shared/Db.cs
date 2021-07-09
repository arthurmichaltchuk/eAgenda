using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace eAgenda.Controladores.Shared
{
    public delegate T ConverterDelegate<T>(IDataReader reader);

    public static class Db
    {
        public static readonly string connectionString = "";
        private static readonly string bancoEscolhido = "";

        static Db()
        {
            bancoEscolhido = ConfigurationManager.AppSettings["bancodedados"].ToLower().Trim();
            connectionString = ConfigurationManager.ConnectionStrings[bancoEscolhido].ConnectionString;
        }

        public static int Insert(string sql, Dictionary<string, object> parameters)
        {
            int id = 0;

            if (bancoEscolhido == "dbsqlite")
                id = DBLite.Insert(sql, parameters);

            if (bancoEscolhido == "DBAgenda")
                id = DBSql.Insert(sql, parameters);

            return id;
        }

        public static void Update(string sql, Dictionary<string, object> parameters = null)
        {
            if (bancoEscolhido == "dbsqlite")
                DBLite.Update(sql, parameters);

            if (bancoEscolhido == "DBAgenda")
                DBSql.Update(sql, parameters);
        }

        public static void Delete(string sql, Dictionary<string, object> parameters)
        {
            Update(sql, parameters);
        }

        public static List<T> GetAll<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters = null)
        {
            if (bancoEscolhido == "dbsqlite")
                return DBLite.GetAll(sql, convert, parameters);

            if (bancoEscolhido == "DBAgenda")
                return DBSql.GetAll(sql, convert, parameters);

            return new List<T>();
        }

        public static T Get<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters)
        {

            if (bancoEscolhido == "dbsqlite")
                return DBLite.Get(sql, convert, parameters);

            if (bancoEscolhido == "DBAgenda")
                return DBSql.Get(sql, convert, parameters);

            return default;
        }

        public static bool Exists(string sql, Dictionary<string, object> parameters)
        {
            if (bancoEscolhido == "dbsqlite")
                return DBLite.Exists(sql, parameters);

            if (bancoEscolhido == "DBAgenda")
                return DBSql.Exists(sql, parameters);

            return false;
        }
    }
}
