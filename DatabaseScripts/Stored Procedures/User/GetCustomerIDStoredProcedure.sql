USE [NelnetPaymentProcessing]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Get the customer id of the associated user
-- =============================================
CREATE PROCEDURE [dbo].[GetCustomerID]
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CustomerID FROM [dbo].[User] u WHERE u.UserID = @UserID
END
GO