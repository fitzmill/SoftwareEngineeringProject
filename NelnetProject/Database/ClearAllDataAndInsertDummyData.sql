USE [NelnetPaymentProcessing]
GO

drop table if exists [dbo].[Transaction]
drop table if exists [dbo].[Student]
drop table if exists [dbo].[User]
drop table if exists [dbo].[Address]

/****** Object:  Table [dbo].[Student]    Script Date: 2/7/2018 9:53:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[Grade] [tinyint] NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 2/7/2018 9:53:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[AmountCharged] [float] NOT NULL,
	[DateDue] [date] NOT NULL,
	[DateCharged] [date] NULL,
	[ProcessState] [tinyint] NOT NULL,
	[ReasonFailed] [tinyint] NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2/7/2018 9:53:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Hashed] [varchar](max) NOT NULL,
	[Salt] [varchar](max) NOT NULL,
	[PaymentPlan] [tinyint] NOT NULL,
	[UserType] [tinyint] NOT NULL,
	[CustomerID] [varchar](max) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_User]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_User]
GO


insert into [dbo].[User](FirstName, LastName, Email, Hashed, Salt, PaymentPlan, UserType, CustomerID) values('Cooper', 'Knaak', 'cooper@cooperknaak.dating', 'notimplementedyet', 'yet', 1, 1, 'fe1686') --Discover Card
insert into [dbo].[User](FirstName, LastName, Email, Hashed, Salt, PaymentPlan, UserType, CustomerID) values('Bill', 'Gates', 'billy@microsoft.com', 'whynoimplement', 'implement', 3, 1, '1edf63') --Visa Card
insert into [dbo].[User](FirstName, LastName, Email, Hashed, Salt, PaymentPlan, UserType, CustomerID) values('Sean', 'Fitzy', 'sean@weebnation.com', 'willimplementsometime', 'sometime', 2, 1, '5b44c3') --MasterCard


insert into [dbo].[Student](FirstName, LastName, Grade, UserID) values('Caitlin', 'Fitzy', 12, 2)
insert into [dbo].[Student](FirstName, LastName, Grade, UserID) values('Matt', 'Fitz', 6, 2)
insert into [dbo].[Student](FirstName, LastName, Grade, UserID) values('Cathy', 'Knaak', 9, 1)
insert into [dbo].[Student](FirstName, LastName, Grade, UserID) values('SonofBill', 'Gates', 0, 3)
insert into [dbo].[Student](FirstName, LastName, Grade, UserID) values('DaughterofBill', 'Gates', 3, 3)
insert into [dbo].[Student](FirstName, LastName, Grade, UserID) values('BillAdopted', 'Schmidling', 11, 3)


insert into [dbo].[Transaction](UserID, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed) values(3, (2500+2500+5000), DATEFROMPARTS(2017, 9, 5), DATEFROMPARTS(2017, 9, 5), 2, NULL)
insert into [dbo].[Transaction](UserID, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed) values(3, (2500+2500+5000), DATEFROMPARTS(2018, 9, 5), NULL, 1, NULL)
insert into [dbo].[Transaction](UserID, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed) values(2, (2500 + 5000)/9, DATEFROMPARTS(2018, 1, 5), DATEFROMPARTS(2018, 1, 8), 2, 1)
insert into [dbo].[Transaction](UserID, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed) values(2, (2500+5000)/9, DATEFROMPARTS(2018, 9, 5), NULL, 1, NULL)
insert into [dbo].[Transaction](UserID, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed) values(1, (5000)/2, DATEFROMPARTS(2018, 2, 5), NULL, 4, 3)
insert into [dbo].[Transaction](UserID, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed) values(1, (5000)/2, DATEFROMPARTS(2018, 9, 5), NULL, 1, NULL)