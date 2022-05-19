
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace test1.Models
{
    public class Card_Methods
    {
        public static string connStr = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
        public static SqlConnection cnn = new SqlConnection(connStr);

        public Cards SelectCard(string Id, string SerialNumber)
        {
            cnn.Open();
            
            string[] str = new string[2];

            if (Id != "0")
            {
                str[0] = "Id=" + Id;
            }
            else
            {
                str[0] = "false";
            }
            if (SerialNumber != "&&&&&&&&&")
            {
                str[1] = "SerialNumber='" + SerialNumber + "'";
            }
            else
            {
                str[1] = "false";
            }

            int temp = 0;
            for (int i = 0; i < 2; i++)
            {
                if (str[i] == "false")
                    temp++;
            }
            if (temp == 2)
            {
                Cards cardEmpty = new();
                return cardEmpty;
            }
            string or = "OR";

            string sql = "SELECT * FROM Cards WHERE " + str[0] + or + str[1] + ";";
            SqlCommand command = new(sql, cnn);
            SqlDataReader datareader = command.ExecuteReader();

            string[] card_info = new string[2];

            while (datareader.Read())
            {
                for (int i = 0; i < 3; i++)
                {
                    card_info[i] = datareader.GetValue(i).ToString();
                }
            }

            Cards card = new(Convert.ToInt32(card_info[0]), card_info[1], Convert.ToInt32(card_info[2]));

            cnn.Close();
            return card;
        }

        public void CreateUpdateDelete_Cards(string sql)
        {
            cnn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new(sql, cnn);
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();

            command.Dispose();
            cnn.Close();
        }
    }
}
