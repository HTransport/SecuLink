
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace test1.Models
{
    public class User_Methods
    {
        public static string connStr = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";
        public static SqlConnection cnn = new SqlConnection(connStr);

        public void CreateUpdateDelete_Users(string sql)
        {
            cnn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(sql, cnn);
            adapter.UpdateCommand = command;
            adapter.UpdateCommand.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }
        public Users SelectUser(string Id="0", string Username="&&&&&&&&&", string CardId="0")
        {
            cnn.Open();

            string[] str = new string[3];

            if (Id != "0")
            {
                str[0] = "Id=" + Id;
            }
            else
            {
                str[0] = "false";
            }
            if (Username != "&&&&&&&&&")
            {
                str[1] = "Username='" + Username + "'";
            }
            else
            {
                str[1] = "false";
            }
            if (CardId != "0")
            {
                str[2] = "CardId=" + CardId;
            }
            else
            {
                str[2] = "false";
            }

            int temp = 0;
            for (int i = 0; i < 3;i++)
            {
                if (str[i] == "false")
                    temp++;
            }
            if(temp == 3)
            {
                Users userEmpty = new();
                return userEmpty;
            }
            string or = "OR";

            string sql = "SELECT * FROM Users WHERE " + str[0] + or + str[1] + or + str[2] + ";";
            SqlCommand command = new SqlCommand(sql, cnn);

            SqlDataReader datareader = command.ExecuteReader();

            string[] user_info = new string[4];

            while (datareader.Read())
            {
                for (int i = 0; i < 4; i++)
                {
                    user_info[i] = datareader.GetValue(i).ToString();
                }
            }
            Users user = new(Convert.ToInt32(user_info[0]), user_info[1], user_info[2], Convert.ToInt32(user_info[3]));
            cnn.Close();
            return user;
        }

    }
}
