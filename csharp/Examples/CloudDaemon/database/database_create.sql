/****** Object:  Table [dbo].[Nonce]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Nonce](
	[NonceId] [int] IDENTITY(1,1) NOT NULL,
	[Context] [varchar](255) NOT NULL,
	[Code] [varchar](255) NOT NULL,
	[Issued] [datetime] NOT NULL,
	[Expires] [datetime] NOT NULL,
 CONSTRAINT [PK_Nonce] PRIMARY KEY CLUSTERED 
(
	[NonceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Nonce_Code] ON [dbo].[Nonce] 
(
	[Context] ASC,
	[Code] ASC,
	[Issued] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Nonce_Expires] ON [dbo].[Nonce] 
(
	[Expires] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [varchar](255) NOT NULL,
	[Level] [varchar](50) NOT NULL,
	[Logger] [varchar](255) NOT NULL,
	[Message] [varchar](4000) NOT NULL,
	[Exception] [varchar](2000) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE CLUSTERED INDEX [IX_Log] ON [dbo].[Log] 
(
	[Date] DESC,
	[Thread] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consumer]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consumer](
	[ConsumerId] [int] IDENTITY(1,1) NOT NULL,
	[ConsumerKey] [nvarchar](255) NOT NULL,
	[ConsumerSecret] [nvarchar](255) NULL,
	[X509Certificate] [image] NULL,
	[Callback] [nvarchar](2048) NULL,
	[VerificationCodeFormat] [int] NOT NULL,
	[VerificationCodeLength] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Consumer] PRIMARY KEY CLUSTERED 
(
	[ConsumerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Consumer] ON [dbo].[Consumer] 
(
	[ConsumerKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[EmailAddress] [nvarchar](100) NULL,
	[EmailAddressVerified] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OpenIDAssociation]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[OpenIDAssociation](
	[AssociationId] [int] IDENTITY(1,1) NOT NULL,
	[DistinguishingFactor] [varchar](255) NOT NULL,
	[AssociationHandle] [varchar](255) NOT NULL,
	[Expiration] [datetime] NOT NULL,
	[PrivateData] [binary](64) NOT NULL,
	[PrivateDataLength] [int] NOT NULL,
 CONSTRAINT [PK_OpenIDAssociations] PRIMARY KEY CLUSTERED 
(
	[AssociationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_OpenIDAssociations] ON [dbo].[OpenIDAssociation] 
(
	[DistinguishingFactor] ASC,
	[AssociationHandle] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IssuedToken]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[IssuedToken](
	[IssuedTokenId] [int] IDENTITY(1,1) NOT NULL,
	[ConsumerId] [int] NOT NULL,
	[UserId] [int] NULL,
	[Token] [nvarchar](255) NOT NULL,
	[TokenSecret] [nvarchar](255) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Callback] [nvarchar](2048) NULL,
	[VerificationCode] [nvarchar](255) NULL,
	[ConsumerVersion] [varchar](10) NULL,
	[ExpirationDate] [datetime] NULL,
	[IsAccessToken] [bit] NOT NULL,
	[Scope] [nvarchar](255) NULL,
 CONSTRAINT [PK_IssuedToken] PRIMARY KEY CLUSTERED 
(
	[IssuedTokenId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_IssuedToken] ON [dbo].[IssuedToken] 
(
	[Token] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participant]    Script Date: 03/06/2010 11:11:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Participant](
	[ParticipantId] [uniqueidentifier] NOT NULL,
	[UserId] [int] NULL,
	[OpenIdUrl] [varchar](1024) NOT NULL,
	[LoginSecret] [varchar](1024) NULL,
	[LoginSecretExpires] [datetime] NULL,
 CONSTRAINT [PK_Participant] PRIMARY KEY CLUSTERED 
(
	[ParticipantId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[ClearExpiredNonces]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClearExpiredNonces]

AS
DELETE FROM dbo.[Nonce]
WHERE [Expires] < getutcdate()
GO
/****** Object:  StoredProcedure [dbo].[ClearExpiredAssociations]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClearExpiredAssociations]

AS
DELETE FROM dbo.OpenIDAssociation
WHERE [Expiration] < getutcdate()
GO
/****** Object:  Table [dbo].[AuthenticationToken]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthenticationToken](
	[AuthenticationTokenId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OpenIdClaimedIdentifier] [nvarchar](250) NOT NULL,
	[OpenIdFriendlyIdentifier] [nvarchar](250) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[LastUsed] [datetime] NOT NULL,
	[UsageCount] [int] NOT NULL,
 CONSTRAINT [PK_AuthenticationToken] PRIMARY KEY CLUSTERED 
(
	[AuthenticationTokenId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddUser]
@firstName NVARCHAR (50), @lastName NVARCHAR (50), @openid NVARCHAR (255), @role NVARCHAR (255)
AS
DECLARE
		@roleid int,
		@userid int

	BEGIN TRANSACTION

	INSERT INTO [dbo].[User] (FirstName, LastName) VALUES (@firstName, @lastName)
	SET @userid = (SELECT @@IDENTITY)
	
	IF (SELECT COUNT(*) FROM dbo.Role WHERE [Name] = @role) = 0
	BEGIN
		INSERT INTO dbo.Role (Name) VALUES (@role)
		SET @roleid = (SELECT @@IDENTITY)
	END
	ELSE
	BEGIN
		SET @roleid = (SELECT RoleId FROM dbo.Role WHERE [Name] = @role)
	END
	
	INSERT INTO dbo.UserRole (UserId, RoleId) VALUES (@userId, @roleid)
	
	INSERT INTO dbo.AuthenticationToken 
		(UserId, OpenIdClaimedIdentifier, OpenIdFriendlyIdentifier)
		VALUES
		(@userid, @openid, @openid)
	
	COMMIT TRANSACTION
	
	RETURN @userid
GO
/****** Object:  Table [dbo].[LocalProcess]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LocalProcess](
	[LocalProcessId] [uniqueidentifier] NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](1024) NOT NULL,
	[Address] [varchar](80) NOT NULL,
	[ServerPort] [int] NOT NULL,
	[HubPort] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_Tank] PRIMARY KEY CLUSTERED 
(
	[LocalProcessId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ObjectType]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ObjectType](
	[ObjectTypeId] [uniqueidentifier] NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](80) NOT NULL,
	[Radius] [float] NOT NULL,
	[Mass] [float] NOT NULL,
	[ModelUrl] [varchar](1024) NOT NULL,
	[ModelScale] [float] NOT NULL,
	[Published] [bit] NULL,
 CONSTRAINT [PK_ObjectType] PRIMARY KEY CLUSTERED 
(
	[ObjectTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RemoteProcess]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RemoteProcess](
	[RemoteProcessId] [uniqueidentifier] NOT NULL,
	[LocalProcessId] [uniqueidentifier] NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Address] [varchar](80) NOT NULL,
	[HubPort] [int] NOT NULL,
	[Trusted] [bit] NOT NULL,
 CONSTRAINT [PK_TrustedEndPoint] PRIMARY KEY CLUSTERED 
(
	[RemoteProcessId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LocalProcessState]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalProcessState](
	[LocalProcessStateId] [uniqueidentifier] NOT NULL,
	[LocalProcessId] [uniqueidentifier] NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Cpu] [float] NOT NULL,
	[Mem] [float] NOT NULL,
 CONSTRAINT [PK_LocalProcessState] PRIMARY KEY CLUSTERED 
(
	[LocalProcessStateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LocalProcessState] ON [dbo].[LocalProcessState] 
(
	[LocalProcessId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bubble]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Bubble](
	[BubbleId] [uniqueidentifier] NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[LocalProcessId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Range] [float] NOT NULL,
	[PerceptionRange] [float] NOT NULL,
	[Published] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_Bubble] PRIMARY KEY CLUSTERED 
(
	[BubbleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CloudObject]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CloudObject](
	[CloudObjectId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[BubbleId] [uniqueidentifier] NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[ObjectTypeId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](80) NOT NULL,
	[X] [float] NOT NULL,
	[Y] [float] NOT NULL,
	[Z] [float] NOT NULL,
	[OX] [float] NOT NULL,
	[OY] [float] NOT NULL,
	[OZ] [float] NOT NULL,
	[OW] [float] NOT NULL,
	[Radius] [float] NOT NULL,
	[Mass] [float] NOT NULL,
	[ModelUrl] [varchar](1024) NOT NULL,
	[ModelScale] [float] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_CloudObject] PRIMARY KEY CLUSTERED 
(
	[CloudObjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BubbleLink]    Script Date: 03/06/2010 11:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BubbleLink](
	[BubbleLinkId] [uniqueidentifier] NOT NULL,
	[BubbleId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](1024) NOT NULL,
	[RemoteBubbleId] [uniqueidentifier] NOT NULL,
	[Address] [varchar](80) NOT NULL,
	[Port] [int] NOT NULL,
	[X] [float] NOT NULL,
	[Y] [float] NOT NULL,
	[Z] [float] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_BubbleLink] PRIMARY KEY CLUSTERED 
(
	[BubbleLinkId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_Nonce_Issued]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[Nonce] ADD  CONSTRAINT [DF_Nonce_Issued]  DEFAULT (getutcdate()) FOR [Issued]
GO
/****** Object:  Default [DF_User_EmailAddressVerified]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_EmailAddressVerified]  DEFAULT ((0)) FOR [EmailAddressVerified]
GO
/****** Object:  Default [DF_User_CreatedOn]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
/****** Object:  Default [DF_IssuedToken_CreatedOn]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[IssuedToken] ADD  CONSTRAINT [DF_IssuedToken_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
/****** Object:  Default [DF_IssuedToken_IsAccessToken]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[IssuedToken] ADD  CONSTRAINT [DF_IssuedToken_IsAccessToken]  DEFAULT ((0)) FOR [IsAccessToken]
GO
/****** Object:  Default [DF_AuthenticationToken_CreatedOn]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[AuthenticationToken] ADD  CONSTRAINT [DF_AuthenticationToken_CreatedOn]  DEFAULT (getutcdate()) FOR [CreatedOn]
GO
/****** Object:  Default [DF_AuthenticationToken_LastUsed]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[AuthenticationToken] ADD  CONSTRAINT [DF_AuthenticationToken_LastUsed]  DEFAULT (getutcdate()) FOR [LastUsed]
GO
/****** Object:  Default [DF_AuthenticationToken_UsageCount]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[AuthenticationToken] ADD  CONSTRAINT [DF_AuthenticationToken_UsageCount]  DEFAULT ((0)) FOR [UsageCount]
GO
/****** Object:  ForeignKey [FK_IssuedToken_Consumer]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[IssuedToken]  WITH CHECK ADD  CONSTRAINT [FK_IssuedToken_Consumer] FOREIGN KEY([ConsumerId])
REFERENCES [dbo].[Consumer] ([ConsumerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IssuedToken] CHECK CONSTRAINT [FK_IssuedToken_Consumer]
GO
/****** Object:  ForeignKey [FK_IssuedToken_User]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[IssuedToken]  WITH CHECK ADD  CONSTRAINT [FK_IssuedToken_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IssuedToken] CHECK CONSTRAINT [FK_IssuedToken_User]
GO
/****** Object:  ForeignKey [FK_UserRole_Role]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO
/****** Object:  ForeignKey [FK_UserRole_User]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO
/****** Object:  ForeignKey [FK_Participant_User]    Script Date: 03/06/2010 11:11:20 ******/
ALTER TABLE [dbo].[Participant]  WITH CHECK ADD  CONSTRAINT [FK_Participant_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Participant] CHECK CONSTRAINT [FK_Participant_User]
GO
/****** Object:  ForeignKey [FK_AuthenticationToken_User]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[AuthenticationToken]  WITH CHECK ADD  CONSTRAINT [FK_AuthenticationToken_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthenticationToken] CHECK CONSTRAINT [FK_AuthenticationToken_User]
GO
/****** Object:  ForeignKey [FK_LocalProcess_Participant]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[LocalProcess]  WITH CHECK ADD  CONSTRAINT [FK_LocalProcess_Participant] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Participant] ([ParticipantId])
GO
ALTER TABLE [dbo].[LocalProcess] CHECK CONSTRAINT [FK_LocalProcess_Participant]
GO
/****** Object:  ForeignKey [FK_ObjectType_Participant]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[ObjectType]  WITH CHECK ADD  CONSTRAINT [FK_ObjectType_Participant] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Participant] ([ParticipantId])
GO
ALTER TABLE [dbo].[ObjectType] CHECK CONSTRAINT [FK_ObjectType_Participant]
GO
/****** Object:  ForeignKey [FK_RemoteProcess_LocalProcess]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[RemoteProcess]  WITH CHECK ADD  CONSTRAINT [FK_RemoteProcess_LocalProcess] FOREIGN KEY([LocalProcessId])
REFERENCES [dbo].[LocalProcess] ([LocalProcessId])
GO
ALTER TABLE [dbo].[RemoteProcess] CHECK CONSTRAINT [FK_RemoteProcess_LocalProcess]
GO
/****** Object:  ForeignKey [FK_RemoteProcess_Participant]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[RemoteProcess]  WITH CHECK ADD  CONSTRAINT [FK_RemoteProcess_Participant] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Participant] ([ParticipantId])
GO
ALTER TABLE [dbo].[RemoteProcess] CHECK CONSTRAINT [FK_RemoteProcess_Participant]
GO
/****** Object:  ForeignKey [FK_LocalProcessState_LocalProcess]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[LocalProcessState]  WITH CHECK ADD  CONSTRAINT [FK_LocalProcessState_LocalProcess] FOREIGN KEY([LocalProcessId])
REFERENCES [dbo].[LocalProcess] ([LocalProcessId])
GO
ALTER TABLE [dbo].[LocalProcessState] CHECK CONSTRAINT [FK_LocalProcessState_LocalProcess]
GO
/****** Object:  ForeignKey [FK_LocalProcessState_Participant]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[LocalProcessState]  WITH CHECK ADD  CONSTRAINT [FK_LocalProcessState_Participant] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Participant] ([ParticipantId])
GO
ALTER TABLE [dbo].[LocalProcessState] CHECK CONSTRAINT [FK_LocalProcessState_Participant]
GO
/****** Object:  ForeignKey [FK_Bubble_LocalProcess]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[Bubble]  WITH CHECK ADD  CONSTRAINT [FK_Bubble_LocalProcess] FOREIGN KEY([LocalProcessId])
REFERENCES [dbo].[LocalProcess] ([LocalProcessId])
GO
ALTER TABLE [dbo].[Bubble] CHECK CONSTRAINT [FK_Bubble_LocalProcess]
GO
/****** Object:  ForeignKey [FK_Bubble_Participant]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[Bubble]  WITH CHECK ADD  CONSTRAINT [FK_Bubble_Participant] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Participant] ([ParticipantId])
GO
ALTER TABLE [dbo].[Bubble] CHECK CONSTRAINT [FK_Bubble_Participant]
GO
/****** Object:  ForeignKey [FK_CloudObject_Bubble]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[CloudObject]  WITH CHECK ADD  CONSTRAINT [FK_CloudObject_Bubble] FOREIGN KEY([BubbleId])
REFERENCES [dbo].[Bubble] ([BubbleId])
GO
ALTER TABLE [dbo].[CloudObject] CHECK CONSTRAINT [FK_CloudObject_Bubble]
GO
/****** Object:  ForeignKey [FK_CloudObject_CloudObject]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[CloudObject]  WITH CHECK ADD  CONSTRAINT [FK_CloudObject_CloudObject] FOREIGN KEY([ParentId])
REFERENCES [dbo].[CloudObject] ([CloudObjectId])
GO
ALTER TABLE [dbo].[CloudObject] CHECK CONSTRAINT [FK_CloudObject_CloudObject]
GO
/****** Object:  ForeignKey [FK_CloudObject_ObjectType]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[CloudObject]  WITH CHECK ADD  CONSTRAINT [FK_CloudObject_ObjectType] FOREIGN KEY([ObjectTypeId])
REFERENCES [dbo].[ObjectType] ([ObjectTypeId])
GO
ALTER TABLE [dbo].[CloudObject] CHECK CONSTRAINT [FK_CloudObject_ObjectType]
GO
/****** Object:  ForeignKey [FK_CloudObject_Participant1]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[CloudObject]  WITH CHECK ADD  CONSTRAINT [FK_CloudObject_Participant1] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Participant] ([ParticipantId])
GO
ALTER TABLE [dbo].[CloudObject] CHECK CONSTRAINT [FK_CloudObject_Participant1]
GO
/****** Object:  ForeignKey [FK_BubbleLink_Bubble]    Script Date: 03/06/2010 11:11:30 ******/
ALTER TABLE [dbo].[BubbleLink]  WITH CHECK ADD  CONSTRAINT [FK_BubbleLink_Bubble] FOREIGN KEY([BubbleId])
REFERENCES [dbo].[Bubble] ([BubbleId])
GO
ALTER TABLE [dbo].[BubbleLink] CHECK CONSTRAINT [FK_BubbleLink_Bubble]
GO
