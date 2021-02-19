using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using BenchmarkBibleVerse.Models;

namespace BenchmarkBibleVerse.Service.Data
{
    public class BibleVerseDAO
    {
        public string spAddVerse = "[DBO].[sp_addverse]";
        public void TestSQL()
        {
            using(SqlConnection conn = new SqlConnection())
            {
                using(SqlCommand cmd = new SqlCommand())
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = ".";
                    builder.InitialCatalog = "BibleVerse";
                    builder.IntegratedSecurity = true;
                    conn.ConnectionString = builder.ConnectionString;
                    cmd.CommandText = "Select @@ServerName";
                    cmd.Connection = conn;

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        conn.Close();
                    } catch (SqlException ex)
                    {
                        Console.Out.WriteLine(ex.Message);
                    } catch(Exception ex)
                    {
                        Console.Out.WriteLine(ex.Message);
                    }
                }
            }
        }
    
    
        public void AddVerse(BibleVerseModel bibleVerse)
        {
            using(SqlConnection conn = new SqlConnection())
            {
                using(SqlCommand cmd = new SqlCommand())
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = ".";
                    builder.InitialCatalog = "BibleVerse";
                    builder.IntegratedSecurity = true;

                    conn.ConnectionString = builder.ConnectionString;
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = spAddVerse;
                    SqlParameter insertSucceeded = new SqlParameter("@InsertSucceed", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output };
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
                    }
                    catch (SqlException ex)
                    {
                        Console.Out.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}