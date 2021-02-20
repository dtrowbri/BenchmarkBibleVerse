using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using BenchmarkBibleVerse.Models;
using BenchmarkBibleVerse.Service.Utility;
using System.Configuration;
using System.Web.Script.Serialization;

namespace BenchmarkBibleVerse.Service.Data
{
    public class BibleVerseDAO
    {
        readonly string createBibleVerseDB = @"if not exists(select* from master.sys.sysdatabases where name = 'BibleVerse')
                                        Begin
                                            create database BibleVerse
                                        End";

        readonly string createVersesTable = @"if not exists(select * from sysobjects where name = 'Verses' and type = 'U')
                                                Begin
	                                                CREATE TABLE [dbo].[Verses](
		                                                [Id] [int] IDENTITY(1,1) NOT NULL,
		                                                [Testament] [char](3) NULL,
		                                                [Book] [nvarchar](50) NOT NULL,
		                                                [Chapter] [int] NOT NULL,
		                                                [VerseNumber] [int] NOT NULL,
		                                                [Verse] [nvarchar](max) NOT NULL,
	                                                 CONSTRAINT [PK_Verses] PRIMARY KEY CLUSTERED 
	                                                (
		                                                [Id] ASC
	                                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	                                                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                                                End";

        readonly string createSPAddVerses = @"if not exists(select* from sysobjects where name = 'sp_addverse' and type = 'p')
                            Begin
                                Declare @SQLCMD nvarchar(max)

                                set @SQLCMD = 'CREATE procedure [dbo].[sp_addverse](

                                        @Testament char (3),
			                            @Book nvarchar(50),
			                            @Chapter int,
			                            @VerseNumber int,
			                            @Verse nvarchar(max),
			                            @InsertSucceed binary output
		                            )
		                            AS
                                    BEGIN

                                        Begin Transaction

                                            Begin Try

                                                    INSERT INTO[DBO].[VERSES]
                                    ([TESTAMENT], [BOOK], [CHAPTER], [VERSENUMBER], [VERSE]) VALUES(@Testament, @Book, @Chapter, @VerseNumber, @Verse)

						                            if @@ROWCOUNT = 1
							                            Begin
                                                            SET @InsertSucceed = 1
								                            Commit Transaction

                                                        End
						                            else
							                            Begin
                                                            SET @InsertSucceed = 0
								                            Rollback Transaction

                                                        End
                                            End Try
                                            Begin Catch
                                                Set @InsertSucceed = 0
					                            Rollback Transaction

                                            End Catch

                                    End'

		                            exec sp_sqlexec @sqlcmd
                            End";

        readonly string createSPGetVerse = @"if not exists(select * from sys.sysobjects where name = 'sp_getVerse' and type = 'p')
                                                BEGIN
	                                                DECLARE @SQLCMD NVARCHAR(MAX)
	                                                SET @SQLCMD = 'create procedure sp_getVerse(
		                                                @Testament char(3),
		                                                @Book nvarchar(50),
		                                                @Chapter int,
		                                                @VerseNumber int
	                                                )
	                                                AS
	                                                BEGIN
		                                                SELECT [VERSE] FROM [DBO].[Verses] WHERE [Testament] = @Testament AND [Book] = @Book AND [Chapter] = @Chapter AND [VerseNumber] = @VerseNumber
	                                                END'

	                                                EXEC sp_sqlexec @SQLCMD
                                                END";

        string bibleVerseConnectionString = ConfigurationManager.ConnectionStrings["BibleVerseDB"].ConnectionString;
        string masterConnectionString = ConfigurationManager.ConnectionStrings["Master"].ConnectionString;

        private readonly ILogger logger;

        public BibleVerseDAO(ILogger logger)
        {
            this.logger = logger;
        }

        public string spAddVerse = "[DBO].[sp_addverse]";

        public string spGetVerse = "[DBO].[sp_getVerse]";
    
        public bool AddVerse(BibleVerseModel bibleVerse)
        {
            CreateBibleVerseDB();
            using (SqlConnection conn = new SqlConnection(bibleVerseConnectionString))
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
                        logger.Info("Attempting to insert BibleVerse with paramters " + new JavaScriptSerializer().Serialize(bibleVerse) + " to the database.");

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        if(Convert.ToInt32(insertSucceeded.Value) == 1)
                        {
                            logger.Info("Bible verse was successfully saved to the database.");
                            return true;
                        } else
                        {
                            logger.Warning("Bible verse was successfully saved to the database.");
                            return false;
                        }
                    }
                    catch (SqlException ex)
                    {
                        logger.Error("An error occured inserting the bible verse.\nError number: " + ex.Number + "\nError: " + ex.Message);
                        throw new Exception("An error occured inserting the bible verse.\nError number: " + ex.Number + "\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("An error occured inserting the bible verse.\nError: " + ex.Message);
                        throw new Exception("An error occured inserting the bible verse.\nError: " + ex.Message);

                    }
                }
            }
        }

        public BibleVerseModel GetVerse(BibleVerseModel verse)
        {
            CreateBibleVerseDB();
            using (SqlConnection conn = new SqlConnection(bibleVerseConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spGetVerse, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Testament", verse.Testament);
                    cmd.Parameters.AddWithValue("@Book", verse.BookSelection);
                    cmd.Parameters.AddWithValue("@Chapter", verse.ChapterSelect);
                    cmd.Parameters.AddWithValue("@VerseNumber", verse.VerseNumber);


                    try
                    {
                        logger.Info("Attempting to retrieve verse " + new JavaScriptSerializer().Serialize(verse) + " from the database.");
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        if (reader.HasRows)
                        {
                            reader.Read();
                            verse.VerseString = reader["Verse"].ToString();
                        }
                        
                        conn.Close();
                        return verse;
                    }
                    catch (SqlException ex)
                    {
                        logger.Error("An error occured inserting the bible verse.\nError number: " + ex.Number + "\nError: " + ex.Message);
                        throw new Exception("An error occured inserting the bible verse.\nError number: " + ex.Number + "\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("An error occured inserting the bible verse.\nError: " + ex.Message);
                        throw new Exception("An error occured inserting the bible verse.\nError: " + ex.Message);

                    }
                }
            }
        }

        public void CreateBibleVerseDB()
        {
            using(SqlConnection conn = new SqlConnection(masterConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand(createBibleVerseDB, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        logger.Error("An error occured during the 'creating database if not exists' check on the database.\nError number: " + ex.Number + "\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError number: " + ex.Number + "\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("An error occured during the 'creating database if not exists' check on the database.\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError: " + ex.Message);
                    }
                }
            }

            CreateVersesTable();
        }

        public void CreateVersesTable()
        {
            using (SqlConnection conn = new SqlConnection(bibleVerseConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(createVersesTable, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        logger.Error("An error occured during the 'creating Verses table if not exists' check on the database.\nError number: " + ex.Number + "\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError number: " + ex.Number + "\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("An error occured during the 'creating Verses table if not exists' check on the database.\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError: " + ex.Message);

                    }
                }
            }

            CreateSPAddVerses();
            CreateSPGetVerse();
        }

        public void CreateSPAddVerses()
        {
            using (SqlConnection conn = new SqlConnection(bibleVerseConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(createSPAddVerses, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        logger.Error("An error occured during the 'creating sp_AddVerses if not exists' check on the database.\nError number: " + ex.Number + "\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError number: " + ex.Number + "\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("An error occured during the 'creating sp_AddVerses if not exists' check on the database.\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError: " + ex.Message);

                    }
                }
            }
        }

        public void CreateSPGetVerse()
        {
            using (SqlConnection conn = new SqlConnection(bibleVerseConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(createSPGetVerse, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        logger.Error("An error occured during the 'creating sp_GetVerses if not exists' check on the database.\nError number: " + ex.Number + "\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError number: " + ex.Number + "\nError: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("An error occured during the 'creating sp_GetVerses if not exists' check on the database.\nError: " + ex.Message);
                        throw new Exception("An error occured. Bible Verse Database is not available at this time.\nError: " + ex.Message);

                    }
                }
            }
        }

    }
}