IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE TABLE [Clientes] (
        [Id] int NOT NULL IDENTITY,
        [Nome] Varchar(100) NOT NULL,
        [Phone] Char(11) NULL,
        [CEP] Char(8) NOT NULL,
        [Estado] Char(2) NOT NULL,
        [Cidade] nvarchar(60) NOT NULL,
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE TABLE [Produtos] (
        [Id] int NOT NULL IDENTITY,
        [CodigoBarras] Varchar(14) NOT NULL,
        [Descricao] varchar(60) NULL,
        [Valor] decimal(18,2) NOT NULL,
        [TipoProduto] nvarchar(max) NOT NULL,
        [Ativo] bit NOT NULL,
        CONSTRAINT [PK_Produtos] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE TABLE [Pedidos] (
        [Id] int NOT NULL IDENTITY,
        [ClienteId] int NOT NULL,
        [IniciadoEm] datetime2 NOT NULL DEFAULT (GETDATE()),
        [FinalizadoEm] datetime2 NOT NULL,
        [TipoFrete] int NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [Observacao] Varchar(512) NULL,
        CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Pedidos_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE TABLE [PedidoItens] (
        [Id] int NOT NULL IDENTITY,
        [PedidoId] int NOT NULL,
        [ProdutoId] int NOT NULL,
        [Quantidade] int NOT NULL DEFAULT 1,
        [Valor] decimal(18,2) NOT NULL,
        [Desconto] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_PedidoItens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PedidoItens_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PedidoItens_Produtos_ProdutoId] FOREIGN KEY ([ProdutoId]) REFERENCES [Produtos] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE INDEX [idx_cliente_telefone] ON [Clientes] ([Phone]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE INDEX [IX_PedidoItens_PedidoId] ON [PedidoItens] ([PedidoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE INDEX [IX_PedidoItens_ProdutoId] ON [PedidoItens] ([ProdutoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    CREATE INDEX [IX_Pedidos_ClienteId] ON [Pedidos] ([ClienteId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213140209_InitialMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211213140209_InitialMigration', N'5.0.0');
END;
GO

COMMIT;
GO

