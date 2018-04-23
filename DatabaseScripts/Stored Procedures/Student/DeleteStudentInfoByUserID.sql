SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DeleteStudentInfoByUserID 
	@ID int
AS
BEGIN

    -- Insert statements for procedure here
	DELETE FROM [dbo].[Student]
	WHERE UserID=@ID
END
GO
