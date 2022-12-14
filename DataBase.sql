USE [master]
GO
/****** Object:  Database [SysTest]    Script Date: 21.10.2022 9:01:53 ******/
CREATE DATABASE [SysTest]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SysTest', FILENAME = N'F:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SysTest.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SysTest_log', FILENAME = N'F:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SysTest_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SysTest] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SysTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SysTest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SysTest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SysTest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SysTest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SysTest] SET ARITHABORT OFF 
GO
ALTER DATABASE [SysTest] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SysTest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SysTest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SysTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SysTest] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SysTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SysTest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SysTest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SysTest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SysTest] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SysTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SysTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SysTest] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SysTest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SysTest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SysTest] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SysTest] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SysTest] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SysTest] SET  MULTI_USER 
GO
ALTER DATABASE [SysTest] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SysTest] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SysTest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SysTest] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SysTest] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SysTest] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SysTest] SET QUERY_STORE = OFF
GO
USE [SysTest]
GO
/****** Object:  Table [dbo].[Вопрос]    Script Date: 21.10.2022 9:01:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Вопрос](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_теста] [int] NULL,
	[Номер_вопроса] [int] NULL,
	[Наименование] [varchar](255) NULL,
 CONSTRAINT [PK_Вопрос] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Должность]    Script Date: 21.10.2022 9:01:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Должность](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Наименование] [varchar](50) NULL,
 CONSTRAINT [PK_Должность] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[История]    Script Date: 21.10.2022 9:01:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[История](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Дата] [datetime] NULL,
	[Логин] [varchar](50) NULL,
	[Номер_теста] [int] NULL,
	[Количество_баллов] [int] NULL,
 CONSTRAINT [PK_История] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ответ]    Script Date: 21.10.2022 9:01:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ответ](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_вопроса] [int] NULL,
	[Вариант_ответа] [int] NULL,
	[Наименование] [varchar](255) NULL,
	[Статус_ответа] [bit] NULL,
 CONSTRAINT [PK_Ответ] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Пользователь]    Script Date: 21.10.2022 9:01:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Пользователь](
	[Логин] [varchar](50) NOT NULL,
	[Пароль] [varchar](50) NULL,
	[ФИО] [varchar](255) NULL,
	[id_должности] [int] NULL,
	[Почта] [varchar](100) NULL,
 CONSTRAINT [PK_Пользователь] PRIMARY KEY CLUSTERED 
(
	[Логин] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Тест]    Script Date: 21.10.2022 9:01:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Тест](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Наименование] [varchar](255) NULL,
	[Количество_вопросов] [int] NULL,
 CONSTRAINT [PK_Тест] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Вопрос] ON 

INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (53, 23, 1, N'Сколько существует материков?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (54, 23, 2, N'Какой океан омывает Англию')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (55, 23, 3, N'Самая длинная река в России?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (56, 23, 4, N'Самый глубокий водоём в мире?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (57, 23, 5, N'Столица китая?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (58, 23, 6, N'Столица ОАЭ?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (59, 24, 1, N'Кто первым начал осваивать американские земли?
')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (60, 24, 2, N'В каком году была принята Конституция США?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (62, 24, 3, N'Кто стал первым президентом США?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (63, 24, 4, N'Назови имя первого римского императора')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (64, 24, 5, N'В каком году началась война алой и белой розы?')
INSERT [dbo].[Вопрос] ([id], [id_теста], [Номер_вопроса], [Наименование]) VALUES (68, 25, 1, N'Вопрос №3 ?')
SET IDENTITY_INSERT [dbo].[Вопрос] OFF
GO
SET IDENTITY_INSERT [dbo].[Должность] ON 

INSERT [dbo].[Должность] ([id], [Наименование]) VALUES (1, N'Администратор')
INSERT [dbo].[Должность] ([id], [Наименование]) VALUES (2, N'Учитель')
INSERT [dbo].[Должность] ([id], [Наименование]) VALUES (3, N'Ученик')
SET IDENTITY_INSERT [dbo].[Должность] OFF
GO
SET IDENTITY_INSERT [dbo].[История] ON 

INSERT [dbo].[История] ([id], [Дата], [Логин], [Номер_теста], [Количество_баллов]) VALUES (24, CAST(N'2022-10-21T07:26:54.000' AS DateTime), N'igor', 23, 96)
INSERT [dbo].[История] ([id], [Дата], [Логин], [Номер_теста], [Количество_баллов]) VALUES (25, CAST(N'2022-10-21T07:27:26.000' AS DateTime), N'igor', 24, 100)
INSERT [dbo].[История] ([id], [Дата], [Логин], [Номер_теста], [Количество_баллов]) VALUES (26, CAST(N'2022-10-21T07:28:10.000' AS DateTime), N'a', 23, 48)
INSERT [dbo].[История] ([id], [Дата], [Логин], [Номер_теста], [Количество_баллов]) VALUES (27, CAST(N'2022-10-21T07:28:21.000' AS DateTime), N'a', 24, 40)
INSERT [dbo].[История] ([id], [Дата], [Логин], [Номер_теста], [Количество_баллов]) VALUES (28, CAST(N'2022-10-21T08:44:12.000' AS DateTime), N'igor', 23, 100)
INSERT [dbo].[История] ([id], [Дата], [Логин], [Номер_теста], [Количество_баллов]) VALUES (29, CAST(N'2022-10-21T08:44:32.000' AS DateTime), N'igor', 23, 50)
SET IDENTITY_INSERT [dbo].[История] OFF
GO
SET IDENTITY_INSERT [dbo].[Ответ] ON 

INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (112, 53, 1, N'3', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (113, 53, 2, N'5', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (114, 53, 3, N'6', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (115, 54, 1, N'Атлантический', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (116, 54, 2, N'Индийский', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (117, 54, 3, N'Тихий', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (118, 55, 1, N'Иртыш', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (119, 55, 2, N'Обь', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (120, 55, 3, N'Лена', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (121, 56, 1, N'Каспийское море', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (122, 56, 2, N'Байкал', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (123, 56, 3, N'Восток', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (124, 57, 1, N'Шанхай', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (125, 57, 2, N'Гонконг', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (126, 57, 3, N'Пекин', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (127, 58, 1, N'Абу-Даби', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (128, 58, 2, N'Катар', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (129, 58, 3, N'Дубай', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (130, 59, 1, N'Французы', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (131, 59, 2, N'Англичане', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (132, 59, 3, N'Испанцы', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (133, 60, 1, N'1780', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (134, 60, 2, N'1787', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (135, 60, 3, N'1778', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (139, 62, 1, N'Джордж Вашингтон', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (140, 62, 2, N'Джон Адамс', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (141, 62, 3, N'Томас Джефферсон12', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (142, 63, 1, N'Юлий Цезарь', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (143, 63, 2, N'Октавиан Август', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (144, 63, 3, N'Цезарион', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (145, 64, 1, N'1485', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (146, 64, 2, N'1455', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (147, 64, 3, N'1634', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (157, 68, 1, N'Ответ №7', 1)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (158, 68, 2, N'Ответ №8', 0)
INSERT [dbo].[Ответ] ([id], [id_вопроса], [Вариант_ответа], [Наименование], [Статус_ответа]) VALUES (159, 68, 3, N'Ответ №9', 0)
SET IDENTITY_INSERT [dbo].[Ответ] OFF
GO
INSERT [dbo].[Пользователь] ([Логин], [Пароль], [ФИО], [id_должности], [Почта]) VALUES (N'a', N'a', N'afio', 3, N'a@a.a')
INSERT [dbo].[Пользователь] ([Логин], [Пароль], [ФИО], [id_должности], [Почта]) VALUES (N'admin', N'admin', N'admin', 1, N'admin')
INSERT [dbo].[Пользователь] ([Логин], [Пароль], [ФИО], [id_должности], [Почта]) VALUES (N'igor', N'igor', N'Большаков Игорь Алексеевич', 3, N'volnas.111@gmail.com')
INSERT [dbo].[Пользователь] ([Логин], [Пароль], [ФИО], [id_должности], [Почта]) VALUES (N'q', N'q', N'fio', 2, N'q@q.q')
GO
SET IDENTITY_INSERT [dbo].[Тест] ON 

INSERT [dbo].[Тест] ([id], [Наименование], [Количество_вопросов]) VALUES (23, N'Тест по географии №1', 6)
INSERT [dbo].[Тест] ([id], [Наименование], [Количество_вопросов]) VALUES (24, N'Тест проверка интеллектуального развития "Всеобщие знания"', 5)
INSERT [dbo].[Тест] ([id], [Наименование], [Количество_вопросов]) VALUES (25, N'Новый Измененный Тест', 1)
SET IDENTITY_INSERT [dbo].[Тест] OFF
GO
ALTER TABLE [dbo].[Вопрос]  WITH CHECK ADD  CONSTRAINT [FK_Вопрос_Тест] FOREIGN KEY([id_теста])
REFERENCES [dbo].[Тест] ([id])
GO
ALTER TABLE [dbo].[Вопрос] CHECK CONSTRAINT [FK_Вопрос_Тест]
GO
ALTER TABLE [dbo].[История]  WITH CHECK ADD  CONSTRAINT [FK_История_Пользователь] FOREIGN KEY([Логин])
REFERENCES [dbo].[Пользователь] ([Логин])
GO
ALTER TABLE [dbo].[История] CHECK CONSTRAINT [FK_История_Пользователь]
GO
ALTER TABLE [dbo].[История]  WITH CHECK ADD  CONSTRAINT [FK_История_Тест] FOREIGN KEY([Номер_теста])
REFERENCES [dbo].[Тест] ([id])
GO
ALTER TABLE [dbo].[История] CHECK CONSTRAINT [FK_История_Тест]
GO
ALTER TABLE [dbo].[Ответ]  WITH CHECK ADD  CONSTRAINT [FK_Ответ_Вопрос] FOREIGN KEY([id_вопроса])
REFERENCES [dbo].[Вопрос] ([id])
GO
ALTER TABLE [dbo].[Ответ] CHECK CONSTRAINT [FK_Ответ_Вопрос]
GO
ALTER TABLE [dbo].[Пользователь]  WITH CHECK ADD  CONSTRAINT [FK_Пользователь_Должность] FOREIGN KEY([id_должности])
REFERENCES [dbo].[Должность] ([id])
GO
ALTER TABLE [dbo].[Пользователь] CHECK CONSTRAINT [FK_Пользователь_Должность]
GO
USE [master]
GO
ALTER DATABASE [SysTest] SET  READ_WRITE 
GO
