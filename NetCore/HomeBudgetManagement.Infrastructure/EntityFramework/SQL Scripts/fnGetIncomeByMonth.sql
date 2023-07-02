CREATE FUNCTION [dbo].[fnGetIncomeByMonth](@month int,@type varchar(50))
	RETURNS  TABLE
	RETURN 
		(Select * From Expense where DATEPART(mm,[date]) = @month AND DATEPART(yy,[date]) = DATEPART(yy,getdate()) and type  = @type)
