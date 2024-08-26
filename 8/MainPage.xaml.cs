using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using System.Data.Entity;

namespace _8
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        bool statement;
        public MainPage()
        {
            InitializeComponent();

            //Create();
        }

        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            var newStudentPage = new NewStudentPage();
            MainFrame.NavigationService.Navigate(newStudentPage);
            Hidden();
        }

        private void Edit_Btn_Click(object sender, RoutedEventArgs e)
        {
            statement = true;          

            if (StudentGrid.SelectedItem != null)
            {
                var currentStudent = StudentGrid.SelectedItem as Book;
                var editPage = new EditPage(currentStudent, statement);
                MainFrame.NavigationService.Navigate(editPage);               
            }
            else
            {
                MessageBox.Show("Студент не выбран", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Hidden();

            //StudentGrid.ItemsSource = AppData.db.Student.ToList();
        }

        private void View_Btn_Click(object sender, RoutedEventArgs e)
        {
            statement = false;
            if (StudentGrid.SelectedItem != null)
            {
                var currentStudent = StudentGrid.SelectedItem as Book;
                var editPage = new EditPage(currentStudent, statement);
                MainFrame.NavigationService.Navigate(editPage);
            }
            else
            {
                MessageBox.Show("Студент не выбран", "Уведомление",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Hidden();
        }

        private void Delete_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (StudentGrid.SelectedItem != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить студента (студентов)?", "Уведомление",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    List<Book> currentStudent = new List<Book>();

                    foreach (var student in StudentGrid.SelectedItems)
                    {
                        currentStudent.Add(student as Book);
                    }

                    foreach (var student in currentStudent)
                    {
                        if (student.Author != null)
                        {
                            AppData.db.Author.Remove(student.Author);
                        }
                    }

                    AppData.db.Book.RemoveRange(currentStudent);
                    AppData.db.SaveChanges();

                    StudentGrid.ItemsSource = AppData.db.Book.ToList();
                    MessageBox.Show(" Удаление прошло успешно", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Студент не выбран, выберите студента", "Уведомление",
                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StudentGrid.ItemsSource = AppData.db.Book.ToList();
        }

        private void ToMain_Btn_Click(object sender, RoutedEventArgs e)
        {
            Hidden();
            ToMain_btn.Visibility = Visibility.Collapsed;
            MainFrame.NavigationService.Navigate(new MainPage());
            AppData.db.ChangeTracker.Entries().ToList().ForEach(entity => entity.Reload());
            StudentGrid.ItemsSource = AppData.db.Book.ToList();
        }

        private void Hidden()
        {
            View_btn.Visibility = Visibility.Collapsed;
            Edit_btn.Visibility = Visibility.Collapsed;
            Add_btn.Visibility = Visibility.Collapsed;
            Delete_btn.Visibility = Visibility.Collapsed;
            Sorting_btn.Visibility = Visibility.Collapsed;
        }

        private void Sorting_Btn_Click(object sender, RoutedEventArgs e)
        {
            Hidden();
            var sortingForm = new SortingWindow();
            sortingForm.ShowDialog();

            var students = AppData.db.Book.ToList();

            bool sortByCourseAscending = sortingForm.CourseAscending;
            bool sortByCourseDescending = sortingForm.CourseDescending;
            bool sortByYearAscending = sortingForm.AgeAscending;
            bool sortByYearDescending = sortingForm.AgeDescending;
            bool sortByGroupAscending = sortingForm.GroupAscending;
            bool sortByGroupDescending = sortingForm.GroupDescending;
            bool sortByIDAscending = sortingForm.IDAscending;
            bool sortByIDDescending = sortingForm.IDDescending;

            if (sortByCourseAscending)
            {
                students = students.OrderBy(s => s.PageCount).ToList();
            }
            else if (sortByCourseDescending)
            {
                students = students.OrderByDescending(s => s.PageCount).ToList();
            }
            else if (sortByYearAscending)
            {
                students = students.OrderBy(s => s.PublishYear).ToList();
            }
            else if (sortByYearDescending)
            {
                students = students.OrderByDescending(s => s.PublishYear).ToList();
            }
            else if (sortByGroupAscending)
            {
                students = students.OrderBy(s => s.Format).ToList();
            }
            else if (sortByGroupDescending)
            {
                students = students.OrderByDescending(s => s.Format).ToList();
            }
            else if (sortByIDAscending)
            {
                students = students.OrderBy(s => s.UDC).ToList();
            }
            else if (sortByIDDescending)
            {
                students = students.OrderByDescending(s => s.UDC).ToList();
            }
            StudentGrid.ItemsSource = students;
        }

        private void Create()
        {
            string connectionString = "Data Source=DESKTOP-KS0KMCI\\SQLEXPRESS;Integrated Security=True;";

            string createDatabaseScript = @"
            USE [master]

            IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'OOP_UniversityDB_2')
            BEGIN

                /****** Object:  Database [OOP_UniversityDB_2] ******/
                CREATE DATABASE [OOP_UniversityDB_2]
                    CONTAINMENT = NONE
                    ON  PRIMARY 
                ( NAME = N'OOP_UniversityDB_2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\OOP_UniversityDB_2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
                    LOG ON 
                ( NAME = N'OOP_UniversityDB_2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\OOP_UniversityDB_2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
                    WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
            

                IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
                begin
                EXEC [OOP_UniversityDB_2].[dbo].[sp_fulltext_database] @action = 'enable'
                end
            

                ALTER DATABASE [OOP_UniversityDB_2] SET ANSI_NULL_DEFAULT OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET ANSI_NULLS OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET ANSI_PADDING OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET ANSI_WARNINGS OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET ARITHABORT OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET AUTO_CLOSE ON 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET AUTO_SHRINK OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET AUTO_UPDATE_STATISTICS ON 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET CURSOR_CLOSE_ON_COMMIT OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET CURSOR_DEFAULT  GLOBAL 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET CONCAT_NULL_YIELDS_NULL OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET NUMERIC_ROUNDABORT OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET QUOTED_IDENTIFIER OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET RECURSIVE_TRIGGERS OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET  ENABLE_BROKER 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET DATE_CORRELATION_OPTIMIZATION OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET TRUSTWORTHY OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET PARAMETERIZATION SIMPLE 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET READ_COMMITTED_SNAPSHOT OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET HONOR_BROKER_PRIORITY OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET RECOVERY SIMPLE 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET  MULTI_USER 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET PAGE_VERIFY CHECKSUM  
            

                ALTER DATABASE [OOP_UniversityDB_2] SET DB_CHAINING OFF 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET DELAYED_DURABILITY = DISABLED 
            

                ALTER DATABASE [OOP_UniversityDB_2] SET ACCELERATED_DATABASE_RECOVERY = OFF  
            

                ALTER DATABASE [OOP_UniversityDB_2] SET QUERY_STORE = ON
            

                ALTER DATABASE [OOP_UniversityDB_2] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
            

                ALTER DATABASE [OOP_UniversityDB_2] SET  READ_WRITE
            END";

            var createAddressScript = @"
                USE [OOP_UniversityDB_2]

                IF NOT EXISTS (SELECT 1 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE='BASE TABLE' 
                    AND TABLE_NAME='Author')
                BEGIN

                    /****** Object:  Table [dbo].[Author] ******/
                    SET ANSI_NULLS ON
            

                    SET QUOTED_IDENTIFIER ON
            

                    CREATE TABLE [dbo].[Author](
	                    [AuthorID] [int] NOT NULL,
	                    [City] [varchar](50) NOT NULL,
	                    [PostalCode] [int] NOT NULL,
	                    [FIO] [varchar](100) NOT NULL,
	                    [HouseNumber] [varchar](5) NOT NULL,
	                    [ApartmentNumber] [varchar](5) NULL,
                        CONSTRAINT [PK__Address__091C2A1B87CED139] PRIMARY KEY CLUSTERED 
                    (
	                    [AuthorID] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]
                END";

            var createStudentScript = @"
                USE [OOP_UniversityDB_2]

                IF NOT EXISTS (SELECT 1 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE='BASE TABLE' 
                    AND TABLE_NAME='Book')
                BEGIN
                    /****** Object:  Table [dbo].[Book] ******/
                    SET ANSI_NULLS ON
            

                    SET QUOTED_IDENTIFIER ON
            

                    CREATE TABLE [dbo].[Book](
	                    [UDC] [int] NOT NULL,
	                    [BookCover] [nvarchar](max) NULL,
	                    [Title] [varchar](100) NOT NULL,
	                    [PublishYear] [int] NOT NULL,
	                    [Publisher] [varchar](100) NOT NULL,
	                    [UploadDate] [date] NULL,
	                    [PageCount] [int] NOT NULL,
	                    [Format] [varchar](50) NOT NULL,
	                    [FileSize] [decimal](3, 2) NULL,
	                    [Gender] [varchar](10) NOT NULL,
	                    [AuthorID] [int] NOT NULL,
	                    [UpdatedAt] [datetime] NOT NULL,
                     CONSTRAINT [PK__Student__32C52A79AC1A9DCF] PRIMARY KEY CLUSTERED 
                    (
	                    [UDC] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
            

                    ALTER TABLE [dbo].[Book] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
            

                    ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK__Student__Address__4D94879B] FOREIGN KEY([AuthorID])
                    REFERENCES [dbo].[Author] ([AuthorID])
            

                    ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK__Student__Address__4D94879B]
                END";

            var createSprocScript = @"
                USE [OOP_UniversityDB_2]

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.procedures WITH(NOLOCK)
                    WHERE NAME = 'UpdateStudent'
                        AND type = 'P'
                )
                BEGIN
                    EXEC('CREATE PROCEDURE [dbo].[UpdateStudent]
                        @StudentId INT,
                        @Title varchar(100),
                        @BookCover nvarchar(MAX),
                        @PublishYear INT,
                        @Publisher varchar(100),
                        @UploadDate Date,
                        @PageCount INT,
                        @Format varchar(50),
                        @FileSize decimal(3, 2),
                        @Gender varchar(10),
                        @City varchar(50),
                        @PostalCode int,
                        @FIO varchar(100),
                        @HouseNumber varchar(5),
                        @ApartmentNumber varchar(5)   
                    AS
                    BEGIN
                        BEGIN TRANSACTION

                        BEGIN TRY
                            UPDATE Book
                            SET Title = @Title,
                                BookCover = @BookCover,
                                PublishYear = @PublishYear,
                                Publisher = @Publisher,
                                UploadDate = @UploadDate,
                                PageCount = @PageCount,
                                Format = @Format,
                                Gender = @Gender,
                                FileSize = @FileSize
                            FROM Book
                            JOIN Author ON Book.AuthorID = Author.AuthorID
                            WHERE StudentId = @StudentId;

                            UPDATE Author
                            SET City = @City,
                                PostalCode = @PostalCode,
                                FIO = @FIO,
                                HouseNumber = @HouseNumber,
                                ApartmentNumber = @ApartmentNumber
                            FROM Author
                            JOIN Book ON Book.AuthorID = Author.AuthorID
                            WHERE Book.StudentId = @StudentId;

                            COMMIT TRAN;
                        END TRY
                        BEGIN CATCH
                            ROLLBACK TRAN;
                        END CATCH
                    END')
                END";

            var createTriggerScript = @"
                IF NOT EXISTS (SELECT 1 FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Student_Update]'))
                BEGIN
                     EXEC('CREATE TRIGGER [dbo].[Student_Update]
                        ON [dbo].[Book]
                        AFTER UPDATE
                        AS
                            UPDATE Book
                            SET UpdatedAt = GETDATE()
                            WHERE UDC = (SELECT UDC FROM inserted)
            

                        ALTER TABLE [dbo].[Book] ENABLE TRIGGER [Student_Update]')
                END";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand commandDb = new SqlCommand(createDatabaseScript, connection))
                    using (SqlCommand commandAddress = new SqlCommand(createAddressScript, connection))
                    using (SqlCommand commandStudent = new SqlCommand(createStudentScript, connection))
                    using (SqlCommand commandSproc = new SqlCommand(createSprocScript, connection))
                    using (SqlCommand commandTrigger = new SqlCommand(createTriggerScript, connection))
                    {
                        commandDb.ExecuteNonQuery();
                        commandAddress.ExecuteNonQuery();
                        commandStudent.ExecuteNonQuery();
                        commandSproc.ExecuteNonQuery();
                        commandTrigger.ExecuteNonQuery();

                        Console.WriteLine("Database created successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating database: " + ex.Message);
            }
        }
    }
}
