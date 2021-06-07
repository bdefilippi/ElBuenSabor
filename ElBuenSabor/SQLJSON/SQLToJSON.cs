using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace ElBuenSabor
{
    public class SQLToJSON
    {
        private String jSON; 

        public void Agregar(String propiedad, String SQLquery, Dictionary<string, object> parametros)
        {
            //  Arma  { "propiedad":[{},{},{}] }
            jSON += "{" + "\"" + propiedad + "\"" + ":" + SQLQuery(SQLquery, parametros) + "}";
        }

        public void Agregar(String SQLquery, Dictionary<string, object> parametros)
        {
            //  Arma  [{},{},{}] o {}
            jSON += SQLQuery(SQLquery, parametros);
        }

        public void Agregar(String SQLquery)
        {
            //  Arma  [{},{},{}] o {}
            jSON += SQLQuery(SQLquery);
        }

        public string JSON()
        {
            // Une los objetos quitando las divisiones interrnas
            return jSON.Replace("}{", ",");
        }

        public static String SQLQuery(string queryString, Dictionary<string, object> parametros = null)
        {
            List<SqlParameter> sqlparametros = new List<SqlParameter>();
            
            if (parametros!=null)
            {
                foreach (var key in parametros.Keys) sqlparametros.Add (new SqlParameter(key, parametros[key]));
            }
            string connectionString = Startup.ConnectionString;
            String Respuesta = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                if (parametros!=null) command.Parameters.AddRange(sqlparametros.ToArray());
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Respuesta = sqlDatoToJson(reader);
                command.Parameters.Clear(); //tira error si no se borran explicitamente al hacer una nueva llamada
                reader.Close();
            }
            return Respuesta;
        }

        private static String sqlDatoToJson(SqlDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            String JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            //Si solo devuelve una fila la tabla de la Query, entonces remover los [] iniciales y finales
            if (dataTable.Rows.Count == 1) JSONString = JSONString.Substring(1, JSONString.Length - 2);
            return JSONString;
        }

    }
}
