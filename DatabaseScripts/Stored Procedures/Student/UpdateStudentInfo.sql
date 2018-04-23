USE [NelnetPaymentProcessing]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	<Update the information of the associated student>
-- =============================================
CREATE PROCEDURE UpdateStudentInfo
	@ID int,
	@FirstName varchar(255),
	@LastName varchar(255),
	@Grade tinyint
AS
BEGIN
	UPDATE [dbo].[Student]
	SET FirstName=@FirstName, LastName=@LastName, Grade=@Grade
	WHERE StudentID=@ID
END
GO
