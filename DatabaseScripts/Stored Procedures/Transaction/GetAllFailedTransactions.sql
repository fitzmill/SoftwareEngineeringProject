USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sean Fitzgerald
-- Create date: 2018/04/02
-- Description:	Gets all transactions marked as failed
-- =============================================
CREATE PROCEDURE GetAllFailedTransactions
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Transaction] t WHERE t.ProcessState = 4
END
GO
