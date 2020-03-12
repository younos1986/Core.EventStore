see the samples for more info



### MySql tables


```

IF DB_ID('EventStoreDb') IS NULL
   Create database [EventStoreDb]";

            

USE [{DatabaseName}]
/****** Object:  Table [dbo].[Idempotences]    Script Date: 6-3-2020 10:59:41 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Idempotences]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Idempotences](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Idempotences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

/****** Object:  Table [dbo].[Positions]    Script Date: 6-3-2020 10:59:41 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Positions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Positions](
	[Id] [uniqueidentifier] NOT NULL,
	[CommitPosition] [bigint] NOT NULL,
	[PreparePosition] [bigint] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Positions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END



/****** Object:  Table [dbo].[Customers]    Script Date: 6-3-2020 10:59:41 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customers](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](32) NOT NULL,
	[LastName] [nvarchar](32) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END


```