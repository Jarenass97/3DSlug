using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using System;

namespace Assets.Scripts
{
    public class SQLite : MonoBehaviour
    {
        private string dbName = "URI=file:datosGuardados.db";
        private const string TABLE = "datos";
        private const string ESCENA = "escena";
        private SqliteConnection connection;
        private SqliteCommand command;

        public void CreateDB()
        {
            connection = new SqliteConnection(dbName);
            connection.Open();
            command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE + "(" + ESCENA + ");";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void saveGame()
        {
            Scene scene = SceneManager.GetActiveScene();
            Debug.Log("guardando");
            deleteLastData();
            Debug.Log("borrado anterior");
            connection.Open();
            Debug.Log(scene);
            command.CommandText = "INSERT INTO " + TABLE + " VALUES(" + scene + ");";
            command.ExecuteNonQuery();
            Debug.Log("insertado nuevo dato");
            connection.Close();
            Debug.Log("Guardado completo");
        }

        public void deleteLastData()
        {
            connection.Open();
            command.CommandText = "DELETE FROM " + TABLE + ";";
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void loadGame()
        {
            Scene scene = new Scene();
            connection.Open();
            command.CommandText = "SELECT * FROM " + TABLE + ";";
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                    scene = (Scene)reader[0];
            }
            SceneManager.SetActiveScene(scene);
            connection.Close();
        }

        public bool dataExists()
        {
            bool exists = false;
            connection.Open();
            command.CommandText = "SELECT * FROM " + TABLE + ";";
            using (IDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                    exists = true;
            }
            connection.Close();
            Debug.Log(exists);
            return exists;
        }
    }
}
