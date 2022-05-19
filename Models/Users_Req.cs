using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Models
{
    public class Users_Req : Users
    {
        public int Request { get; set; }
        public Users_Req(int request, int Id = 0, string Username = "&&&&&&&&&", string Password_Enc = "%%%%%%%%%%", int CardId = 0)
        {
            Request = request;
            this.Id = Id;
            this.Username = Username;
            this.Password_Enc = Password_Enc;
            this.CardId = CardId;
        }
        public void RequestHandling(int Id = 0, string Username = "&&&&&&&&&", string Password_Enc = "%%%%%%%%%%", int CardId = 0, int req = -1)
        {
            // 0 - Kreiraj novog korisnika sa danim podatcima
            // 1 - Provjeri da li postoji dani korisnik
            // 2 - Pronađi korisnika sa jednim od tih inicijala te promijeni one koji su različiti (Id obavezan ako nije ništa drugo postavljeno osim Password_Enc)
            // 3 - Obriši korisnika sa jednim od tih inicijala (Id obavezan ako nije ništa drugo postavljeno osim Password_Enc)
            // 4 - Pošalji mi korisnika sa jednim od tih inicijala 

            Models.User_Methods userCon = new();
            Controllers.Controller con = new();
            Users userEmpty = new();
            string c = ",";
            string or = "OR";

            switch (req)
            {
                case 0:
                    {
                        try
                        {

                            string[] str = new string[3];

                            if (Id != 0)
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
                            if (CardId != 0)
                            {
                                str[2] = "CardId=" + CardId;
                            }
                            else
                            {
                                str[2] = "false";
                            }

                            int temp = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                if (str[i] == "false")
                                    temp++;
                            }
                            if (temp == 3)
                            {
                                con.Response_Send(200, "ERROR:0");
                                break;
                            }

                            Models.Users user = userCon.SelectUser(Id.ToString(), Username, CardId.ToString());
                            if (user == userEmpty)
                                con.Response_Send(200, "ERROR:0");

                            userCon.CreateUpdateDelete_Users("INSERT INTO Users (Username, Password_Enc, CardId) VALUES (" + Username + c + Password_Enc + c + CardId + ")");
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                        con.Response_Send(202,"DONE");
                    }
                    break;
                case 1:
                    {
                        try
                        {
                            Models.Users user = userCon.SelectUser(Id.ToString(), Username, CardId.ToString());
                            if (user == userEmpty)
                                con.Response_Send(200, "ERROR:0");
                            else
                            {
                                con.Response_Send(203, "EXISTS");
                            }
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                    }
                    break;
                case 2:
                    {
                        try
                        {
                            Models.Users user = userCon.SelectUser(Id.ToString(), Username, CardId.ToString());

                            if (user == userEmpty)
                            {
                                con.Response_Send(200, "ERROR:0");
                                break;
                            }
                            string[] str = new string[3];

                            if (Id != 0)
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
                            if (CardId != 0)
                            {
                                str[2] = "CardId=" + CardId;
                            }
                            else
                            {
                                str[2] = "false";
                            }

                            string sql = "UPDATE Users SET ";
                            if (user.Username != "&&&&&&&&&")
                                sql += "Username = '" + Username + "'";
                            if (user.Password_Enc != "%%%%%%%%%%")
                                if (sql[sql.Length - 1] == '\'')
                                    sql += ",Password_Enc = '" + Password_Enc + "'";
                                else sql += "Password_Enc = '" + Password_Enc + "'";
                            if (user.CardId != 0)
                                if (sql[sql.Length - 1] == '\'')
                                    sql += ",CardId = " + CardId;
                                else sql += "CardId = " + CardId;
                            sql += " WHERE ID = " + Id + or + ";";
                            userCon.CreateUpdateDelete_Users(sql);
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                        con.Response_Send(202, "DONE");
                    }
                    break;
                case 3:
                    {
                        try
                        {
                            Models.Users user = userCon.SelectUser(Id.ToString(), Username, CardId.ToString());

                            string[] str = new string[3];

                            if (Id != 0)
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
                            if (CardId != 0)
                            {
                                str[2] = "CardId=" + CardId;
                            }
                            else
                            {
                                str[2] = "false";
                            }

                            string sql = "DELETE FROM Users WHERE " + str[0] + or + str[1] + or + str[2] + ";";
                            userCon.CreateUpdateDelete_Users(sql);
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                        con.Response_Send(202, "DONE");
                    }
                    break;
                case 4:
                    {
                        try
                        {
                            Models.Users user = userCon.SelectUser(Id.ToString(), Username, CardId.ToString());

                            if (user == userEmpty)
                            {
                                con.Response_Send(200, "ERROR:0");
                            }
                            else
                            {
                                con.Response_Send(204, "SENT");
                                con.Object_Send(user);
                            }
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                    }
                    break;
                default:
                    {
                        con.Response_Send(201, "ERROR:1");
                    }
                    break;
            }
        }

    }
}
