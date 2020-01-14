USE [Employees]
GO

ALTER TABLE [dbo].[Employee] DROP CONSTRAINT [FK_Employee_Employee]
GO

--ALTER TABLE [dbo].[Employee] NOCHECK CONSTRAINT [FK_Employee_Employee]
--GO

ALTER TABLE [dbo].[EmployeeAttribute] DROP CONSTRAINT [FK_EmployeeAttribute_Attribute]
GO

--ALTER TABLE [dbo].[EmployeeAttribute] NOCHECK CONSTRAINT [FK_EmployeeAttribute_Attribute]
--GO


ALTER TABLE [dbo].[EmployeeAttribute] DROP CONSTRAINT [FK_EmployeeAttribute_Employee]
GO

--ALTER TABLE [dbo].[EmployeeAttribute] NOCHECK CONSTRAINT [FK_EmployeeAttribute_Employee]
--GO


truncate table [dbo].[Attribute]
truncate table [dbo].[EmployeeAttribute]
truncate table [dbo].[Employee]



ALTER TABLE [dbo].[Employee]  WITH NOCHECK ADD  CONSTRAINT [FK_Employee_Employee] FOREIGN KEY([EMP_Supervisor])
REFERENCES [dbo].[Employee] ([EMP_ID])
GO

ALTER TABLE [dbo].[EmployeeAttribute]  WITH NOCHECK ADD  CONSTRAINT [FK_EmployeeAttribute_Attribute] FOREIGN KEY([EMPATTR_AttributeID])
REFERENCES [dbo].[Attribute] ([ATTR_ID])
GO

ALTER TABLE [dbo].[EmployeeAttribute]  WITH NOCHECK ADD  CONSTRAINT [FK_EmployeeAttribute_Employee] FOREIGN KEY([EMPATTR_EmployeeID])
REFERENCES [dbo].[Employee] ([EMP_ID])
GO



