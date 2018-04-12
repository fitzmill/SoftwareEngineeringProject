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
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE UpdatePersonalInfo
	-- Add the parameters for the stored procedure here
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

    -- Insert statements for procedure here
	UPDATE [dbo].[User] 
	SET FirstName=@FirstName, LastName=@LastName, Email=@Email, Hashed=@Hashed, Salt=@Salt, PaymentPlan=@PaymentPlan, UserType=@UserType, CustomerID=@CustomerID
	WHERE UserID=@ID;
END
GO
