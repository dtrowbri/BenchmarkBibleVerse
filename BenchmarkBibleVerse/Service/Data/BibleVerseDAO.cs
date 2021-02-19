using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using BenchmarkBibleVerse.Models;
using System.Configuration;

namespace BenchmarkBibleVerse.Service.Data
{
    public class BibleVerseDAO
    {
        string connectionString = ConfigurationManager.ConnectionStrings["BibleVerseDB"].ConnectionString;

        public string spAddVerse = "[DBO].[sp_addverse]";
    
        public bool AddVerse(BibleVerseModel bibleVerse)
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                using(SqlCommand cmd = new SqlCommand(spAddVerse, conn))
                {
                    SqlParameter insertSucceeded = new SqlParameter("@InsertSucceed", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output };
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Testament", bibleVerse.Testament);
                    cmd.Parameters.AddWithValue("@Book", bibleVerse.BookSelection);
                    cmd.Parameters.AddWithValue("@Chapter", bibleVerse.ChapterSelect);
                    cmd.Parameters.AddWithValue("@VerseNumber", bibleVerse.VerseNumber);
                    cmd.Parameters.AddWithValue("@Verse", bibleVerse.VerseString);
                    cmd.Parameters.Add(insertSucceeded);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        if(Convert.ToInt32(insertSucceeded.Value) == 1)
                        {
                            return true;
                        } else
                        {
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("An error occured inserting the bible verse.\nError number: " + ex.Number + "\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occured inserting the bible verse.\nError: " + ex.Message);

                    }
                }
            }
        }
    }
}