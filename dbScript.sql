
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "OrderItems" (
    "Id" uuid NOT NULL,
    "OrderId" uuid NOT NULL,
    "ParentOrderItemId" uuid NULL,
    "ProductId" uuid NOT NULL,
    "Price" numeric(10,2) NOT NULL,
    "ProductType" integer NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "DeletedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_OrderItems" PRIMARY KEY ("Id")
);

CREATE TABLE "Orders" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Price" numeric(10,2) NOT NULL,
    "DiscountAmount" numeric(10,2) NOT NULL,
    "NetPrice" numeric(10,2) NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "DeletedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id")
);

CREATE TABLE "Products" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Price" numeric(10,2) NOT NULL,
    "ProductType" integer NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "DeletedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
);

CREATE TABLE "UserLogins" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Token" character varying(1000) NOT NULL,
    "TokenExpireDate" timestamp with time zone NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "DeletedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_UserLogins" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Email" character varying(100) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "IsAdmin" boolean NOT NULL,
    "LastLoginDate" timestamp with time zone NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "DeletedDate" timestamp with time zone NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('0e7ca561-a147-460c-98dd-c22556019836', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Tea', 3.0, 1);
INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('18be78cd-49f7-4a5c-bb7c-b86e9e125d51', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Lemon', 2.0, 2);
INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('1be28499-751c-4d31-9b90-7307c5448031', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Mocha', 6.0, 1);
INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('a3af147b-2e43-498a-9f7a-020f98ceb0a4', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Hazelnut syrup', 3.0, 2);
INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('a44fcd45-1d18-467c-af9a-21de7194d243', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Chocolate sauce', 5.0, 2);
INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('e49854e8-8b95-47ce-a622-d676e94b3e75', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Black Coffee', 4.0, 1);
INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('f5ccffc4-1b21-4581-a71d-96bc70791255', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Latte', 5.0, 1);
INSERT INTO "Products" ("Id", "CreatedDate", "DeletedDate", "Name", "Price", "ProductType")
VALUES ('ffcdafd8-5906-4414-a364-0d7f1936920b', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'Milk', 2.0, 2);

INSERT INTO "Users" ("Id", "CreatedDate", "DeletedDate", "Email", "IsAdmin", "LastLoginDate", "Password")
VALUES ('35062e26-cd41-4123-b824-ea06b0f19596', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'admin@test.com', TRUE, NULL, '202cb962ac59075b964b07152d234b70');
INSERT INTO "Users" ("Id", "CreatedDate", "DeletedDate", "Email", "IsAdmin", "LastLoginDate", "Password")
VALUES ('d20733b1-fe94-482c-bd80-d82f7d8d0ef3', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'user1@test.com', FALSE, NULL, '202cb962ac59075b964b07152d234b70');
INSERT INTO "Users" ("Id", "CreatedDate", "DeletedDate", "Email", "IsAdmin", "LastLoginDate", "Password")
VALUES ('ed8f558e-1ebb-418c-a1bb-db953ba90c58', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'user2@test.com', FALSE, NULL, '202cb962ac59075b964b07152d234b70');
INSERT INTO "Users" ("Id", "CreatedDate", "DeletedDate", "Email", "IsAdmin", "LastLoginDate", "Password")
VALUES ('f2e6040a-df19-4cec-b929-7e2c5c0e53fe', TIMESTAMPTZ '2022-01-01 00:00:00Z', NULL, 'user3@test.com', FALSE, NULL, '202cb962ac59075b964b07152d234b70');

CREATE UNIQUE INDEX "IX_Users_Email_DeletedDate" ON "Users" ("Email", "DeletedDate");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220811213814_m0', '6.0.7');

COMMIT;

