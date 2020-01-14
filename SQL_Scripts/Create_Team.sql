
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DECLARE @Supervisor_ID uniqueidentifier = '82D58D49-72A2-42B0-A250-471E5C10D7D9' 

BEGIN
	IF OBJECT_ID('tempdb..#Employee_Hierarchy') IS NOT NULL
	DROP TABLE tempdb..#Employee_Hierarchy

	CREATE TABLE #Employee_Hierarchy
	(
		EMP_ID uniqueidentifier NOT NULL,
		TEAM nvarchar(100) NOT NULL,
		ATTR_ID uniqueidentifier NOT NULL
	);
  
	DECLARE @Supervisor_Name varchar(100) = (	SELECT EMP_Name
												FROM Employee
												WHERE EMP_ID = @Supervisor_ID )

	 ;WITH cte (EMP_ID, TEAM, ATTR_ID)
	 AS
	 (
		SELECT EMP_ID, CAST('' as nvarchar(100)) AS TEAM, NEWID() AS ATTR_ID
		FROM Employee
		WHERE EMP_ID = @Supervisor_ID
		UNION ALL
		SELECT CE.EMP_ID,CAST(@Supervisor_Name as nvarchar(100)) AS TEAM, NEWID() AS ATTR_ID
		FROM Employee CE
		INNER JOIN cte on CE.EMP_Supervisor = cte.EMP_ID
	 )
	 INSERT INTO #Employee_Hierarchy (EMP_ID, TEAM, ATTR_ID)
	 SELECT EMP_ID, TEAM, ATTR_ID
	 FROM cte 
	 WHERE len(TEAM) > 0


	DECLARE @teamValue nvarchar(50), @teamNumber int;
	
	SELECT Distinct @teamValue= TEAM, @teamNumber = Count(*) 
	FROM #Employee_Hierarchy 
	GROUP BY TEAM
	
	INSERT INTO Attribute
	SELECT ATTR_ID,'Team', @teamValue
	FROM #Employee_Hierarchy

	INSERT INTO EmployeeAttribute
	SELECT EMP_ID,ATTR_ID
	FROM #Employee_Hierarchy

	PRINT  'Created the team of ' + @teamValue + ' with '+ CAST(@teamNumber as nvarchar(50)) + ' members'

END
GO
