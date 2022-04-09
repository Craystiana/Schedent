USE [Schedent]
GO

	INSERT INTO [dbo].[UserRoles] ([UserRoleId], [Name]) VALUES 
				(1, 'Admin'), 
				(2, 'Student'), 
				(3, 'Professor');

	INSERT INTO [dbo].[Faculties] ([FacultyId], [Name]) VALUES
				(1, 'Agronomie'),
				(2, 'Automatica, Calculatoare si Electronica'),
				(3, 'Drept'),
				(4, 'Economie si Administrarea Afacerilor'),
				(5, 'Educatie Fizica si Sport'),
				(6, 'Horticultura'),
				(7, 'Inginerie Electrica'),
				(8, 'Litere'),
				(9, 'Mecanica'),
				(10, 'Teologie'),
				(11, 'Stiinte'),
				(12, 'Stiinte Sociale');

	--INSERT INTO [dbo].[Professors] ([ProfessorId], [FacultyId], [Name]) VALUES ();
	
	INSERT INTO [dbo].[Sections] ([SectionId], [FacultyId], [Name]) VALUES
				(1, 1, 'Agricultura'),
				(2, 1, 'Agricultura-IFR'),
				(3, 1, 'Montanologie'),
				(4, 1, 'Montanologie-IFR'),
				(5, 1, 'Controlul si Expertiza Produselor Agroalimentare'),
				(6, 1, 'Silvicultura'),
				(7, 2, 'Automatica si Informatica Aplicata'),
				(8, 2, 'Calculatoare cu predare in limba romana'),
				(9, 2, 'Calculatoare cu predare in limba engleza'),
				(10, 2, 'Electronica Aplicata'),
				(11, 2, 'Ingineria Sistemelor Multimedia'),
				(12, 2, 'Megatronica'),
				(13, 2, 'Robotica');

	INSERT INTO [dbo].[Groups] ([GroupId], [SectionId], [Name]) VALUES
				(1, 7, 'AIA1.1'),
				(2, 7, 'AIA1.2'),
				(3, 7, 'AIA1.3'),
				(4, 7, 'AIA2.1'),
				(5, 7, 'AIA2.2'),
				(6, 7, 'AIA2.3'),
				(7, 7, 'AIA3.1'),
				(8, 7, 'AIA3.2'),
				(9, 7, 'AIA3.3'),
				(10, 7, 'AIA4.1_1'),
				(11, 7, 'AIA4.1_2'),
				(12, 7, 'AIA4.2_1'),
				(13, 7, 'AIA4.2_2'),
				(14, 8, 'CR1.1'),
				(15, 8, 'CR1.2'),
				(16, 8, 'CR1.3'),
				(17, 8, 'CR2.1'),
				(18, 8, 'CR2.2'),
				(19, 8, 'CR2.3'),
				(20, 8, 'CR3.1'),
				(21, 8, 'CR3.2'),
				(22, 8, 'CR3.3'),
				(23, 8, 'CR4.S1'),
				(24, 8, 'CR4.S2'),
				(25, 8, 'CR4.H');

	INSERT INTO [dbo].[Subgroups] ([SubgroupId], [GroupId], [Name]) VALUES
				(1, 1, 'A'),
				(2, 1, 'B'),
				(3, 2, 'A'),
				(4, 2, 'B'),
				(5, 3, 'A'),
				(6, 3, 'B'),
				(7, 4, 'A'),
				(8, 4, 'B'),
				(9, 5, 'A'),
				(10, 5, 'B'),
				(11, 6, 'A'),
				(12, 6, 'B'),
				(13, 23, 'A'),
				(14, 23, 'B'),
				(15, 24, 'A'),
				(16, 24, 'B'),
				(17, 25, 'A'),
				(18, 25, 'B');

GO	

