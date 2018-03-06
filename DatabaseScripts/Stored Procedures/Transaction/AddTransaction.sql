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
-- Description:	Adds a transaction to the transaction table.
-- =============================================
CREATE PROCEDURE AddTransaction
	@UserID int,
	@AmountCharged float,
	@DateDue date,
	@DateCharged date,
	@ProcessState tinyint,
	@ReasonFailed varchar(255)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[Transaction] (UserId, AmountCharged, DateDue, DateCharged, ProcessState, ReasonFailed)
		VALUES (@UserID, @AmountCharged, @DateDue, @DateCharged, @ProcessState, @ReasonFailed)
	SELECT SCOPE_IDENTITY();
END
GO
