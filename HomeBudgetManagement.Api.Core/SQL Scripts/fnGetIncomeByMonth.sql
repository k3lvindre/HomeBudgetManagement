CREATE FUNCTION [dbo].[fnGetIncomeByMonth](@month int)
	RETURNS  TABLE
	RETURN 
		(Select * From Income where DATEPART(mm,[date]) = @month AND DATEPART(yy,[date]) = DATEPART(yy,getdate()))
