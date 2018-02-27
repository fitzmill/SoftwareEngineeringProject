-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Lucas Hall
-- Create date: 2018/02/12
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE InsertPersonalInfo
	-- Add the parameters for the stored procedure here
---	<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
---	<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>

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

    -- Insert statements for procedure here
	INSERT INTO [dbo].[User] (FirstName, LastName, Email, Hashed, Salt, PaymentPlan, UserType, CustomerID)
	VALUES (@FirstName, @LastName, @Email, @Hashed, @Salt, @PaymentPlan, @UserType, @CustomerID)

	SELECT SCOPE_IDENTITY();
END
GO
