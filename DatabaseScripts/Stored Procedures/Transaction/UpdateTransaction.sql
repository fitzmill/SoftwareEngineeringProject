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
-- Author:		Joe Cowman
-- Create date: 2/21/2018
-- Description:	Updates a transaction in the transaction table.
-- =============================================
CREATE PROCEDURE UpdateTransaction
	@TransactionID int,
	@UserID int,
	@AmountCharged float,
	@DateDue date,
	@DateCharged date = NULL,
	@ProcessState tinyint,
	@ReasonFailed varchar(255) = NULL
AS
BEGIN
	UPDATE [dbo].[Transaction] SET 
		UserId = @UserID, AmountCharged = @AmountCharged, DateDue = @DateDue, DateCharged = @DateCharged, ProcessState = @ProcessState, ReasonFailed = @ReasonFailed
		WHERE TransactionID = @TransactionID
	SELECT SCOPE_IDENTITY();
END
GO
