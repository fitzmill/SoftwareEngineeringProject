USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	<Delete all of the students associated with the given user>
-- =============================================
CREATE PROCEDURE DeleteStudentInfoByUserID 
	@ID int
AS
BEGIN
	DELETE FROM [dbo].[Student]
	WHERE UserID=@ID
END
GO
