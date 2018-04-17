USE [NelnetPaymentProcessing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Lucas Hall
-- Create date: 2018/02/12
-- Description:	Insert the user information and create the user, returning their id.
-- =============================================
CREATE PROCEDURE InsertPersonalInfo
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
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[User] (FirstName, LastName, Email, Hashed, Salt, PaymentPlan, UserType, CustomerID)
	VALUES (@FirstName, @LastName, @Email, @Hashed, @Salt, @PaymentPlan, @UserType, @CustomerID)

	SELECT SCOPE_IDENTITY();
END
GO
