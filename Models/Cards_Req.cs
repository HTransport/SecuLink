using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test1.Models
{
    public class Cards_Req : Cards
    {
        public int Request { get; set; }
        public Cards_Req(int request, int Id = 0, string SerialNumber = "&&&&&&&&&", int Pin = -1)
        {
            Request = request;
            this.Id = Id;
            this.SerialNumber = SerialNumber;
            this.Pin = Pin;
        }

        public void RequestHandling(int Id = 0, string SerialNumber = "&&&&&&&&&", int Pin = -1, int req = -1)
        {
            // 0 - Kreiraj novu karticu sa danim podatcima
            // 1 - Provjeri da li postoji dana kartica
            // 2 - Pronađi karticu sa jednim od tih inicijala te promijeni one koji su različiti (Id obavezan ako nije ništa drugo postavljeno osim Pin)
            // 3 - Obriši karticu sa jednim od tih inicijala (Id obavezan ako nije ništa drugo postavljeno osim Pin)
            // 4 - Pošalji mi karticu sa jednim od tih inicijala (Osim Pin)

            Models.Card_Methods cardCon = new();
            Controllers.Controller con = new();
            Cards cardEmpty = new();
            string c = ",";
            string or = "OR";

            switch (req)
            {
                case 0:  // 0 - Kreiraj novu karticu sa danim podatcima
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
                            if (SerialNumber != "&&&&&&&&&")
                            {
                                str[1] = "SerialNumber='" + SerialNumber + "'";
                            }
                            else
                            {
                                str[1] = "false";
                            }

                            Models.Cards card = cardCon.SelectCard(Id.ToString(), SerialNumber);
                            if (card == cardEmpty)
                                con.Response_Send(200, "ERROR:0");

                           cardCon.CreateUpdateDelete_Cards("INSERT INTO Users (Username, Password_Enc, CardId) VALUES (" + SerialNumber + c + Pin + ")");
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                        con.Response_Send(202, "DONE");
                    }
                    break;
                case 1:  // 1 - Provjeri da li postoji dana kartica
                    {
                        try
                        {
                            Models.Cards card = cardCon.SelectCard(Id.ToString(), SerialNumber);
                            if (card == cardEmpty)
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
                case 2:  // 2 - Pronađi karticu sa jednim od tih inicijala te promijeni one koji su različiti (Id obavezan ako nije ništa drugo postavljeno osim Pin)
                    {
                        try
                        {
                            Models.Cards card = cardCon.SelectCard(Id.ToString(), SerialNumber);

                            if (card == cardEmpty)
                            {
                                con.Response_Send(200, "ERROR:0");
                            }
                            string sql = "UPDATE Users SET ";

                            string[] str = new string[2];

                            if (Id != 0)
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

                            if (card.SerialNumber != "&&&&&&&&&")
                                sql += "SerialNumber = '" + SerialNumber + "'";
                            if (card.Pin != 0)
                                if (sql[sql.Length - 1] == '\'')
                                    sql += ",Pin = " + Pin;
                                else sql += "Pin = " + Pin;
                            sql += " WHERE " + str[0] + Id + or + str[1] + SerialNumber + ";";
                            cardCon.CreateUpdateDelete_Cards(sql);
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                        con.Response_Send(202, "DONE");
                    }
                    break;
                case 3:  // 3 - Obriši karticu sa jednim od tih inicijala (Id obavezan ako nije ništa drugo postavljeno osim Pin)
                    {
                        try
                        {
                            Models.Cards card = cardCon.SelectCard(Id.ToString(), SerialNumber);

                            string[] str = new string[2];

                            if (Id != 0)
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

                            string sql = "DELETE FROM Users WHERE " + str[0] + or + str[1] + ";";
                            cardCon.CreateUpdateDelete_Cards(sql);
                        }
                        catch (Exception ex)
                        {
                            con.Response_Send(205, ex.ToString());
                        }
                        con.Response_Send(202, "DONE");
                    }
                    break;
                case 4:  // 4 - Pošalji mi karticu sa jednim od tih inicijala (Osim Pin) **
                    {
                        try
                        {
                            Models.Cards card = cardCon.SelectCard(Id.ToString(), SerialNumber);

                            if (card == cardEmpty)
                            {
                                con.Response_Send(200, "ERROR:0");
                            }
                            else
                            {
                                con.Object_Send(card);
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
