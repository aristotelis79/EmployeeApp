PRINT 'update weight'
DECLARE @emplId uniqueidentifier
DECLARE cur_emplId CURSOR FOR
								select  e.EMP_ID
								from Employee e

OPEN cur_emplId
FETCH NEXT FROM cur_emplId INTO @emplId

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @WeightAttrId uniqueidentifier  = (
												SELECT a.ATTR_ID 
												FROM Employee e 
												JOIN EmployeeAttribute ea on e.EMP_ID = ea.EMPATTR_EmployeeID
												JOIN Attribute a on a.ATTR_ID = ea.EMPATTR_AttributeID
												WHERE e.EMP_ID = @emplId AND a.ATTR_Name ='Weight')
	IF (@WeightAttrId IS NULL)
		BEGIN 
			SET @WeightAttrId = NEWID()
			INSERT INTO Attribute VALUES (@WeightAttrId,'Weight','Thin')
			INSERT INTO EmployeeAttribute VALUES (@emplId,@WeightAttrId)

			PRINT 'create weight for Customer Id '+ cast(@emplId as nvarchar(36)) 
		END
	ELSE
		BEGIN
		UPDATE Attribute 
			SET ATTR_Value = 'Thin'
			WHERE ATTR_ID = @WeightAttrId

			PRINT 'update weight for Customer Id '+ cast(@emplId as nvarchar(36)) 
	END 
	FETCH NEXT FROM cur_emplId INTO @emplId
END
CLOSE cur_emplId
DEALLOCATE cur_emplId
GO