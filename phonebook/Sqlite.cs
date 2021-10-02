using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;
using System.ComponentModel;

namespace phonebook
{
    static class Sqlite
    {
        public static bindinglist<Phone> PhoneList = new bindinglist<Phone>();

        private static readonly string FileDB = "phonebook.db";
        /// <summary>
        /// Проверка подключения и первоночальная закрузка из бд
        /// </summary>
        /// <returns>True - подключение успеное; False - ошибка</returns>
        public static bool Init()
        {
            PhoneList.Clear();
            if (File.Exists(FileDB) == false) return false;
            try
            {
                using (SQLiteConnection connect = new SQLiteConnection($"Data Source={FileDB};"))
                {
                    connect.Open();
                    SQLiteCommand command = new SQLiteCommand
                    {
                        Connection = connect,
                        CommandText = "SELECT * FROM PhoneBook"
                    };
                    SQLiteDataReader reader = command.ExecuteReader();

                    foreach (DbDataRecord item in reader)
                    {
                        Debug.WriteLine(item["Number"]);
                        PhoneList.Add(new Phone() {
                            ID = Int32.Parse(item["ID"].ToString()),
                            Number = item["Number"].ToString(),
                            FullName = item["FullName"].ToString(),
                            Address = item["Address"].ToString()
                        });
                        
                    }
                    return true;
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return false; 
            }
        }
        /// <summary>
        /// Сохрарение всего списка в бд
        /// </summary>
        public static void Save(int index)
        {
            try
            {
                Phone item = PhoneList[index];
                string sql_command = $"UPDATE PhoneBook SET Number = '{item.Number}', FullName = '{item.FullName}', Address = '{item.Address}' WHERE ID = {item.ID}"; // UPDATE PhoneBook SET FullName = 'Test' WHERE ID = 2
                using (SQLiteConnection connect = new SQLiteConnection($"Data Source={FileDB};"))
                {
                    connect.Open();
                    SQLiteCommand command = new SQLiteCommand(sql_command, connect);
                    command.ExecuteNonQuery();
                }

            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);

            }
             
        }
        /// <summary>
        /// Удаление значений из бд
        /// </summary>
        /// <param name="id">ID записи в книге</param>
        public static void Delete(int id)
        {
            try
            {
                string sql_command = $"DELETE FROM PhoneBook WHERE ID = {id}"; // DELETE FROM PhoneBook WHERE ID = 
                using (SQLiteConnection connect = new SQLiteConnection($"Data Source={FileDB};"))
                {
                    connect.Open();
                    SQLiteCommand command = new SQLiteCommand(sql_command, connect);
                    command.ExecuteNonQuery();
                }

            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Создание новой записи в бд
        /// </summary>
        /// <param name="number"></param>
        /// <param name="fullname"></param>
        /// <param name="address"></param>
        public static void Create(int index,string number,string fullname, string address)
        {
            try
            {
                string sql_command = $"INSERT INTO PhoneBook (Number,FullName,Address) VALUES ('{number}','{fullname}','{address}');select last_insert_rowid();"; // INSERT INTO PhoneBook (Number,FullName,Address) VALUES ('+79173157740','>hjd','Saratov')
                using (SQLiteConnection connect = new SQLiteConnection($"Data Source={FileDB};"))
                {
                    connect.Open();
                    SQLiteCommand command = new SQLiteCommand(sql_command, connect);
                    int i = Int32.Parse(command.ExecuteScalar().ToString());
                    PhoneList[index].ID = i;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
