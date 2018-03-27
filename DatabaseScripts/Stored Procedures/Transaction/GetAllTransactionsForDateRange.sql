USE [NelnetPaymentProcessing]
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sean Fitzgerald
-- Create date: 2018/02/19
-- Description:	Gets all transactions that are due in between two dates.
-- =============================================
CREATE PROCEDURE GetAllTransactionsForDateRange
	-- Add the parameters for the stored procedure here
	@StartDate Date,
	@EndDate Date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	t.TransactionID,
	u.FirstName,
	u.LastName,
	u.Email,
	t.AmountCharged,
	t.DateDue,
	t.DateCharged,
	t.ProcessState,
	t.ReasonFailed
	FROM [dbo].[Transaction] t
	JOIN [dbo].[User] u ON t.UserID = u.UserID
	WHERE t.DateDue >= @StartDate AND t.DateDue <= @EndDate
END
GO
