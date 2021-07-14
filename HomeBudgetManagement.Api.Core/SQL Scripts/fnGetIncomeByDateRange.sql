CREATE  FUNCTION [dbo].[fnGetIncomeByDateRange](@from datetime, @to datetime)
	RETURNS  TABLE
	RETURN 
		(Select * From Income where [date] >= @from AND [date] <=  DATEADD(minute, 59, DATEADD(hour, 23, @to)))


