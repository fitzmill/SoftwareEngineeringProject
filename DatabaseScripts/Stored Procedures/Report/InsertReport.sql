USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	<Insert a new report into the database>
-- =============================================
CREATE PROCEDURE InsertReport
	@StartDate date,
	@EndDate date
AS
BEGIN
	INSERT INTO [dbo].[Report](DateCreated, StartDate, EndDate) VALUES(GETDATE(), @StartDate, @EndDate)

	SELECT SCOPE_IDENTITY()
END
GO
