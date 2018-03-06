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
CREATE PROCEDURE GetUserInfoByUserID
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT u.FirstName, u.LastName, u.Email, u.Hashed, u.Salt, u.PaymentPlan, u.UserType, u.CustomerID 
	FROM [dbo].[User] u JOIN [dbo].[Student] s ON u.UserID = s.UserID WHERE u.UserID = @UserID
END
