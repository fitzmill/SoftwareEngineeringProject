USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Get all of the general users that are active
-- =============================================
CREATE PROCEDURE [dbo].[GetAllUsers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT u.UserID, u.FirstName, u.LastName, u.Email, u.Hashed, u.Salt, u.PaymentPlan, u.UserType, u.CustomerID, s.StudentId, s.FirstName, s.LastName, s.Grade
	FROM [dbo].[User] u JOIN [dbo].[Student] s ON u.UserID = s.UserID WHERE u.UserType = 1 AND u.Active = 1
END
