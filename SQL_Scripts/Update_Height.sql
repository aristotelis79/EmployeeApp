PRINT 'update height'
DECLARE @emplId uniqueidentifier
DECLARE cur_emplId CURSOR FOR
								SELECT  e.EMP_ID
								FROM Employee e
								WHERE e.EMP_Supervisor IS NOT NULL
OPEN cur_emplId
FETCH NEXT FROM cur_emplId INTO @emplId

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @HeightAttrId uniqueidentifier  = (	SELECT a.ATTR_ID 
												FROM Employee e 
												JOIN EmployeeAttribute ea on e.EMP_ID = ea.EMPATTR_EmployeeID
												JOIN Attribute a on a.ATTR_ID = ea.EMPATTR_AttributeID
												WHERE e.EMP_ID = @emplId 
													AND a.ATTR_Name ='Height'
													AND e.EMP_Supervisor IS NOT NULL)
	
	IF (@HeightAttrId IS NULL)
		BEGIN 
			SET @HeightAttrId = NEWID()
			INSERT INTO Attribute VALUES (@HeightAttrId,'Height','Short')
			INSERT INTO EmployeeAttribute VALUES (@emplId,@HeightAttrId)

			PRINT 'create height with Customer Id '+ cast(@emplId as nvarchar(36))
		END
	ELSE
		BEGIN
			UPDATE Attribute 
			SET ATTR_Value = 'Short'
			WHERE ATTR_ID = @HeightAttrId

			PRINT 'update height with Customer Id '+ cast(@emplId as nvarchar(36)) 
	END 

	FETCH NEXT FROM cur_emplId INTO @emplId
END
CLOSE cur_emplId
DEALLOCATE cur_emplId
GO