USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Update the information of the user
-- =============================================
CREATE PROCEDURE UpdatePersonalInfo
	@ID int,
	@FirstName varchar(255),
	@LastName varchar(255),
	@Email varchar(255),
	@Hashed varchar(255),
	@Salt varchar(255),
	@PaymentPlan tinyint,
	@UserType tinyint,
	@CustomerID varchar(255)
AS
BEGIN
	UPDATE [dbo].[User] 
	SET FirstName=@FirstName, LastName=@LastName, Email=@Email, Hashed=@Hashed, Salt=@Salt, PaymentPlan=@PaymentPlan, UserType=@UserType, CustomerID=@CustomerID
	WHERE UserID=@ID;
END
GO
