USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	<Insert the student information provided and associate it with the given user>
-- =============================================
CREATE PROCEDURE InsertStudentInfo 
	@FirstName varchar(255),
	@LastName varchar(255),
	@Grade tinyint,
	@UserID int
AS
BEGIN
	INSERT INTO [dbo].[Student] (FirstName, LastName, Grade, UserID)
	VALUES (@FirstName, @LastName, @Grade, @UserID)

	SELECT SCOPE_IDENTITY();
END
GO
