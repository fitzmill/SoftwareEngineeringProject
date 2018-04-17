USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Get the user information associated with the email
-- =============================================
CREATE PROCEDURE GetUserInfoByEmail
	@Email varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT u.UserID, u.FirstName, u.LastName, u.Hashed, u.Salt, u.PaymentPlan, u.UserType, u.CustomerID, s.StudentId, s.FirstName, s.LastName, s.Grade
	FROM [dbo].[User] u JOIN [dbo].[Student] s ON u.UserID = s.UserID WHERE u.Email = @Email AND u.Active = 1
END
