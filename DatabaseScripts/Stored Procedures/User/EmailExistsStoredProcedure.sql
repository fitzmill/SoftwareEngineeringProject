USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Check to see if an email already exists in the database.
-- =============================================
CREATE PROCEDURE EmailExists 
	@Email varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT Email FROM [dbo].[User] u WHERE u.Email = @Email AND u.Active = 1
END
