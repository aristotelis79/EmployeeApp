
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CrateTeam] 
	@Supervisor_ID  uniqueidentifier NOT NULL
AS

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



	DECLARE @TeamValue nvarchar(50) = (SELECT Distinct TEAM 
										FROM #Employee_Hierarchy)

	PRINT 'create team of ' + @TeamValue

	INSERT INTO Attribute
	SELECT ATTR_ID,'Team', @TeamValue
	FROM #Employee_Hierarchy

	INSERT INTO EmployeeAttribute
	SELECT EMP_ID,ATTR_ID
	FROM #Employee_Hierarchy

END
GO
