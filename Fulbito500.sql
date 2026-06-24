-- Crear bd si no existe
IF DB_ID(N'Fulbito500') IS NULL
BEGIN
    CREATE DATABASE [Fulbito500];
END
GO
USE [Fulbito500]
GO
/****** Object:  Table [dbo].[Bitacora]    Script Date: 20/5/2026 02:34:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bitacora](
	[IdBitacora] [int] IDENTITY(1,1) NOT NULL,
	[DNI] [int] NOT NULL,
	[Accion] [nvarchar](50) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
	[Modulo] [nvarchar](50) NULL,
	[Criticidad] [nvarchar](50) NULL,
 CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED
(
	[IdBitacora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 20/5/2026 02:34:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[NombreUsuario] [nvarchar](50) NOT NULL,
	[Contrasena] [nvarchar](256) NOT NULL,
	[Estado] [bit] NOT NULL,
	[DNI] [int] NOT NULL,
	[Email] [nvarchar](51) NOT NULL,
	[Rol] [int] NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Apellido] [nvarchar](50) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bitacora] ADD  CONSTRAINT [DF_Bitacora_FechaHora]  DEFAULT (getdate()) FOR [FechaHora]
GO
ALTER TABLE [dbo].[Bitacora]  WITH CHECK ADD  CONSTRAINT [FK_Bitacora_Usuarios1] FOREIGN KEY([DNI])
REFERENCES [dbo].[Usuarios] ([DNI])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Bitacora] CHECK CONSTRAINT [FK_Bitacora_Usuarios1]
GO
ALTER TABLE [dbo].[Usuarios]
ALTER COLUMN [Estado] [bit] NULL;
GO

-- Agregar intentos fallidos con default 0
ALTER TABLE [dbo].[Usuarios]
ADD [IntentosFallidos] INT NOT NULL CONSTRAINT [DF_Usuarios_IntentosFallidos] DEFAULT (0);
GO

-- Agregar digito verificador (control de integridad sobre DNI + NombreUsuario)
ALTER TABLE [dbo].[Usuarios]
ADD [DigitoVerificador] INT NOT NULL CONSTRAINT [DF_Usuarios_DigitoVerificador] DEFAULT (0);
GO

-- Usuario de ejemplo
INSERT INTO Usuarios (DNI, Nombre, Apellido, NombreUsuario, Contrasena, Estado, Email, Rol, IntentosFallidos, DigitoVerificador)
VALUES (1, 'lucas', 'lucas', 'lucas',
'03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4',
1, 'lucas@mail.com', 1, 0, 7);
GO

-- Usuario de ejemplo
INSERT INTO Usuarios (DNI, Nombre, Apellido, NombreUsuario, Contrasena, Estado, Email, Rol, IntentosFallidos, DigitoVerificador)
VALUES (10, 'ramiro', 'ramiro', 'ramiro',
'03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4',
1, 'ramiro@mail.com', 1, 0, 6);
GO

ALTER TABLE [dbo].[Usuarios]
ADD [Lenguaje] INT NOT NULL CONSTRAINT [Lenguaje] DEFAULT (0);
GO

UPDATE [dbo].[Usuarios]
SET [Lenguaje] = 0
WHERE [Lenguaje] IS NULL;
GO

-- parte familias:

/****** Object:  Table [dbo].[Fam_Pat]    Script Date: 16/6/2026 23:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fam_Pat](
	[IdFamilia] [int] NOT NULL,
	[IdPatente] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
	[IdFamilia] ASC,
	[IdPatente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Familias]    Script Date: 16/6/2026 23:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Familias](
	[IdFamilia] [int] IDENTITY(1,1) NOT NULL,
	[NombreFamilia] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED
(
	[IdFamilia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patentes]    Script Date: 16/6/2026 23:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patentes](
	[IdPatente] [int] IDENTITY(1,1) NOT NULL,
	[NombrePatente] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED
(
	[IdPatente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol_Fam]    Script Date: 16/6/2026 23:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol_Fam](
	[IdRol] [int] NOT NULL,
	[IdFamilia] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
	[IdRol] ASC,
	[IdFamilia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol_Pat]    Script Date: 16/6/2026 23:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol_Pat](
	[IdRol] [int] NOT NULL,
	[IdPatente] [int] NOT NULL,
PRIMARY KEY CLUSTERED
(
	[IdRol] ASC,
	[IdPatente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 16/6/2026 23:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 16/6/2026 23:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- parte meada:

INSERT [dbo].[Fam_Pat] ([IdFamilia], [IdPatente]) VALUES (1, 1)
INSERT [dbo].[Fam_Pat] ([IdFamilia], [IdPatente]) VALUES (1, 2)
INSERT [dbo].[Fam_Pat] ([IdFamilia], [IdPatente]) VALUES (1, 3)
INSERT [dbo].[Fam_Pat] ([IdFamilia], [IdPatente]) VALUES (1, 4)
INSERT [dbo].[Fam_Pat] ([IdFamilia], [IdPatente]) VALUES (1, 5)
GO
SET IDENTITY_INSERT [dbo].[Familias] ON

INSERT [dbo].[Familias] ([IdFamilia], [NombreFamilia], [Descripcion]) VALUES (1, N'Admin', N'Familia que se designa a administradores')
SET IDENTITY_INSERT [dbo].[Familias] OFF
GO
SET IDENTITY_INSERT [dbo].[Patentes] ON

INSERT [dbo].[Patentes] ([IdPatente], [NombrePatente], [Descripcion]) VALUES (1, N'Gestor de Usuarios', N'Gestión de usuarios del sistema')
INSERT [dbo].[Patentes] ([IdPatente], [NombrePatente], [Descripcion]) VALUES (2, N'Bitácora', N'Visualización del registro de eventos')
INSERT [dbo].[Patentes] ([IdPatente], [NombrePatente], [Descripcion]) VALUES (3, N'Cambiar Contraseña', N'Cambio de contraseña del usuario actual')
INSERT [dbo].[Patentes] ([IdPatente], [NombrePatente], [Descripcion]) VALUES (4, N'Cerrar Sesión', N'Cierre de sesión actual')
INSERT [dbo].[Patentes] ([IdPatente], [NombrePatente], [Descripcion]) VALUES (5, N'Reiniciar Sesión', N'Inicio de nueva sesión sin cerrar el menú')
SET IDENTITY_INSERT [dbo].[Patentes] OFF
GO
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (1, 1)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (1, 2)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (1, 3)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (1, 4)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (1, 5)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (2, 3)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (2, 4)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (2, 5)
INSERT [dbo].[Rol_Pat] ([IdRol], [IdPatente]) VALUES (3, 3)
GO
SET IDENTITY_INSERT [dbo].[Roles] ON

INSERT [dbo].[Roles] ([IdRol], [NombreRol], [Descripcion]) VALUES (1, N'Administrador', N'Acceso total al sistema')
INSERT [dbo].[Roles] ([IdRol], [NombreRol], [Descripcion]) VALUES (2, N'Empleado', N'Acceso limitado al sistema')
INSERT [dbo].[Roles] ([IdRol], [NombreRol], [Descripcion]) VALUES (3, N'Usuario', N'Rol de usuario común')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO

USE [Fulbito500]
GO

-- 1) Add the new FK column (nullable at first so we can backfill)
ALTER TABLE [dbo].[Usuarios]
ADD [IdRol] INT NULL;
GO

-- 2) Backfill based on the old integer convention:
--    1 = Administrador, 2 = Empleado (matches Roles seed data IdRol 1/2)
UPDATE [dbo].[Usuarios] SET [IdRol] = [Rol] WHERE [Rol] IN (1, 2);
UPDATE [dbo].[Usuarios] SET [IdRol] = 3 WHERE [Rol] NOT IN (1, 2) OR [Rol] IS NULL; -- fallback: Usuario comun
GO

-- 3) Enforce NOT NULL now that every row has a value
ALTER TABLE [dbo].[Usuarios]
ALTER COLUMN [IdRol] INT NOT NULL;
GO

-- 4) Add the FK constraint to Roles
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY ([IdRol])
REFERENCES [dbo].[Roles] ([IdRol]);
GO

-- 5) Keep the old [Rol] column for now (avoid breaking legacy reads in one shot),
--    but it is no longer the source of truth. You can drop it later once
--    everything is confirmed working:
-- ALTER TABLE [dbo].[Usuarios] DROP COLUMN [Rol];
GO

-- 6) Make sure the "Admin" family actually grants every patent that exists,
--    so "Administrador" really means "all functionality".
--    (Insert any patents not yet linked to family 1 = Admin)
INSERT INTO [dbo].[Fam_Pat] (IdFamilia, IdPatente)
SELECT 1, p.IdPatente
FROM [dbo].[Patentes] p
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Fam_Pat] fp WHERE fp.IdFamilia = 1 AND fp.IdPatente = p.IdPatente
);
GO