USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Soft delete by marking the user as inactive.
-- =============================================
CREATE PROCEDURE DeletePersonalInfo 
	@ID int
AS
BEGIN
	UPDATE [dbo].[User]
	SET Active=0
	WHERE UserID=@ID;
END
GO
