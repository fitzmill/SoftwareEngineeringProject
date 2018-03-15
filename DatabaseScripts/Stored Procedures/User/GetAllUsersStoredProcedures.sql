USE [NelnetPaymentProcessing]
GO
/****** Object:  StoredProcedure [dbo].[GetUserInfoUserID]    Script Date: 2/19/2018 9:15:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE GetAllUsers
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT u.UserID, u.FirstName, u.LastName, u.Email, u.Hashed, u.Salt, u.PaymentPlan, u.UserType, u.CustomerID, s.StudentId, s.FirstName, s.LastName, s.Grade
	FROM [dbo].[User] u JOIN [dbo].[Student] s ON u.UserID = s.UserID
END
