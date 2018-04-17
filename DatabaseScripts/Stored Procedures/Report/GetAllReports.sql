USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	<Get all reports stored in the database>
-- =============================================
CREATE PROCEDURE GetAllReports

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT * FROM [dbo].[Report] r
	ORDER BY r.ReportID DESC
END
GO
