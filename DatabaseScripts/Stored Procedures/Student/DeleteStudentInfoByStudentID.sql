USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	<Delete the given student from the database>
-- =============================================
CREATE PROCEDURE DeleteStudentInfoByStudentID
	@ID int
AS
BEGIN
	DELETE FROM [dbo].[Student]
	WHERE StudentID=@ID
END
GO
