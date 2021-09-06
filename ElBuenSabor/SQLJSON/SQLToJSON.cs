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

        public void Agregar(String propiedad, String SQLquery, Dictionary<string, object> parametros,bool ForceArray )
        {
            jSON += "{" + "\"" + propiedad + "\"" + ": [" + SQLQuery(SQLquery, parametros) + "]}"; //  Arma  { "propiedad":[{},{},{}] }
        }

        public void Agregar(String propiedad, String SQLquery, Dictionary<string, object> parametros)
        {
            jSON += "{" + "\"" + propiedad + "\"" + ":" + SQLQuery(SQLquery, parametros) + "}"; //  Arma  { "propiedad":[{},{},{}] }
        }

        public void Agregar(String SQLquery, Dictionary<string, object> parametros)
        {
            jSON += SQLQuery(SQLquery, parametros); //  Arma  [{},{},{}] o {}
        }

        public void Agregar(String SQLquery, bool forceArray=false)
        {
            jSON += SQLQuery(SQLquery); //  Arma  [{},{},{}] o {}
            if (forceArray && !jSON.StartsWith('['))
                jSON = "[" + jSON + "]";
        }

        public string JSON(bool noDoubleLlaves=false)
        {
            string a = jSON.Replace("}{", ",");// Une los objetos quitando las divisiones interrnas
            if (noDoubleLlaves) 
                return a.Replace("[[", "[").Replace("]]", "]");
            return a;
        }

        public static String SQLQuery(string queryString, Dictionary<string, object> parametros = null)
        {
            List<SqlParameter> sqlparametros = new List<SqlParameter>();

            if (parametros != null)
            {
                foreach (var key in parametros.Keys) sqlparametros.Add(new SqlParameter(key, parametros[key]));
            }
            string connectionString = Startup.ConnectionString;
            String Respuesta = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                if (parametros != null) command.Parameters.AddRange(sqlparametros.ToArray()); //Si hay parametros, los agrega
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
            String JSONString = JsonConvert.SerializeObject(dataTable);
            if (dataTable.Rows.Count == 1) JSONString = JSONString.Substring(1, JSONString.Length - 2); //Si solo devuelve una fila la tabla de la Query, entonces remover los [] iniciales y finales
            return JSONString;
        }

        public static String VincularArrayDeJSON(String arrayJSON1, String propId1, String arrayJSON2, String propId1enArrayJSON2, String nuevaPropEn1QueContendraA2)
        {
            //usamos el id para vincular las dos tablas, pero podria ser cualquier otra propiedad
            dynamic arrayJSON1obj = JsonConvert.DeserializeObject(arrayJSON1);
            dynamic arrayJSON2obj = JsonConvert.DeserializeObject(arrayJSON2);
            dynamic arrayObj2;
            
            foreach (var obj1 in arrayJSON1obj)
            {
                arrayObj2 = ((IEnumerable<dynamic>)arrayJSON2obj).Where(d => d[propId1enArrayJSON2] == obj1[propId1]);
                arrayObj2 = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(arrayObj2)); //...hace mucho problema para borrar un item luego de la LINQ, esto lo vuelve un json object de nuevo
                foreach (var item in arrayObj2) item.Remove(propId1enArrayJSON2); //Quita la propiedad que los vincula del objeto2, del "muchos" del "uno a muchos"
                obj1.Add(nuevaPropEn1QueContendraA2, JsonConvert.DeserializeObject(JsonConvert.SerializeObject(arrayObj2))); //Agrega el ArrayObj2 que pertenencen al obj1
            }

            return JsonConvert.SerializeObject(arrayJSON1obj); ;
        }

        //    dynamic jsonObj = JsonConvert.DeserializeObject(JSONString);
        //    jsonObj = ((IEnumerable<dynamic>)jsonObj).Where(d => d["ArticuloID"] == 1);
        //    foreach (var item in jsonObj) item["Denominacion"] = "pochoclo";
        //    foreach (var item in jsonObj) item.Add("Postre", "Durazno");
        //    foreach (var item in jsonObj) item.Remove("InsumoID");
        //    JSONString = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

    }
}
