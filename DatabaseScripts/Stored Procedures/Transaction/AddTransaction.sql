USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Joe Cowman
-- Create date: 2/21/2018
-- Description:	Adds a transaction to the transaction table.
-- =============================================
CREATE PROCEDURE AddTransaction
	@UserID int,
	@AmountCharged float,
	@DateDue date,
	@DateCharged date = NULL,
	@ProcessState tinyint,
	@ReasonFailed varchar(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Transaction] (UserId, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed)
		VALUES (@UserID, @AmountCharged, @DateDue, @DateCharged, @ProcessState, @ReasonFailed)
	SELECT SCOPE_IDENTITY();
END
GO
