USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sean Fitzgerald
-- Create date: 2018/2/16
-- Description:	Gets all transactions not marked as successful or failed
-- =============================================
CREATE PROCEDURE GetAllUnsettledTransactions
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[Transaction] t
	WHERE t.ProcessState <> 2 AND t.ProcessState <> 4 --Not successful or failed
END
GO
