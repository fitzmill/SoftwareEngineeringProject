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
	-- Add the parameters for the stored procedure here
	@Email varchar(255),
	@Exists bit output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	IF EXISTS (SELECT Email FROM [dbo].[User] WHERE Email = @Email AND Active = 1)
	SET @Exists = 1
	ELSE
	SET @Exists = 0
RETURN
END
