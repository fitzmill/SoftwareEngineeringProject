USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sean Fitzgerald
-- Create date: 2018/02/12
-- Description:	Gets all transactions for a user based on their UserID
-- =============================================
CREATE PROCEDURE GetAllTransactionsForUser
		@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Transaction] t 
	WHERE t.UserID = @UserID
END
GO
