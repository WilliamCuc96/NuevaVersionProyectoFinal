USE [farmacia]
GO
/****** Object:  Table [dbo].[producto]    Script Date: 05/21/2017 17:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[producto](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[codigo] [nvarchar](50) NULL,
	[nombre] [nvarchar](130) NULL,
	[descripcion] [varchar](200) NULL,
	[tipo] [varchar](10) NULL,
	[precio] [numeric](9, 2) NULL,
	[cantidad] [int] NULL,
 CONSTRAINT [PK_producto] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cliente]    Script Date: 05/21/2017 17:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cliente](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](100) NULL,
	[nit] [nchar](10) NULL,
	[direccion] [nvarchar](100) NULL,
	[telefono] [nchar](10) NULL,
 CONSTRAINT [PK_cliente] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[factura]    Script Date: 05/21/2017 17:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[factura](
	[id] [int] NOT NULL,
	[fecha] [datetime] NULL,
	[numero_factura] [nchar](20) NULL,
	[cliente_id] [int] NULL,
 CONSTRAINT [PK_factura] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[buscaproducto]    Script Date: 05/21/2017 17:50:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[buscaproducto]
(
@busca varchar(100)
)
as


select  nombre, nit, direccion, telefono,id from cliente where 

nombre like  '%' + LTRIM(RTRIM(@busca)) + '%'
or nit like  '%' + LTRIM(RTRIM(@busca)) + '%'
or direccion  like  '%' + LTRIM(RTRIM(@busca)) + '%'
or telefono   like  '%' + LTRIM(RTRIM(@busca)) + '%'
GO
/****** Object:  StoredProcedure [dbo].[insetcliente]    Script Date: 05/21/2017 17:50:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insetcliente]
(
@nombre nvarchar(100),
           @nit nchar(10),
           @direccion nvarchar(100),
           @telefono nchar(10)
)

AS
INSERT INTO [proyecto].[dbo].[cliente]
           ([nombre]
           ,[nit]
           ,[direccion]
           ,[telefono])
     VALUES
           (@nombre, 
           @nit, 
           @direccion,
           @telefono )
GO
/****** Object:  Table [dbo].[factura_detalle]    Script Date: 05/21/2017 17:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[factura_detalle](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[factura_id] [int] NULL,
	[producto_id] [int] NULL,
	[cantidad] [int] NULL,
	[precio] [numeric](9, 2) NULL,
 CONSTRAINT [PK_factura_detalle] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_factura_cliente]    Script Date: 05/21/2017 17:50:36 ******/
ALTER TABLE [dbo].[factura]  WITH CHECK ADD  CONSTRAINT [FK_factura_cliente] FOREIGN KEY([cliente_id])
REFERENCES [dbo].[cliente] ([id])
GO
ALTER TABLE [dbo].[factura] CHECK CONSTRAINT [FK_factura_cliente]
GO
/****** Object:  ForeignKey [FK_factura_detalle_factura]    Script Date: 05/21/2017 17:50:36 ******/
ALTER TABLE [dbo].[factura_detalle]  WITH CHECK ADD  CONSTRAINT [FK_factura_detalle_factura] FOREIGN KEY([factura_id])
REFERENCES [dbo].[factura] ([id])
GO
ALTER TABLE [dbo].[factura_detalle] CHECK CONSTRAINT [FK_factura_detalle_factura]
GO
/****** Object:  ForeignKey [FK_factura_detalle_producto]    Script Date: 05/21/2017 17:50:36 ******/
ALTER TABLE [dbo].[factura_detalle]  WITH CHECK ADD  CONSTRAINT [FK_factura_detalle_producto] FOREIGN KEY([producto_id])
REFERENCES [dbo].[producto] ([id])
GO
ALTER TABLE [dbo].[factura_detalle] CHECK CONSTRAINT [FK_factura_detalle_producto]
GO
