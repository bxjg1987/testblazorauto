using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZLJ.Migrations
{
    public partial class init5101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(maxLength: 256, nullable: true),
                    Parameters = table.Column<string>(maxLength: 1024, nullable: true),
                    ReturnValue = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Exception = table.Column<string>(maxLength: 2000, nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    CustomData = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpBackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    JobType = table.Column<string>(maxLength: 512, nullable: false),
                    JobArgs = table.Column<string>(maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(nullable: false),
                    NextTryTime = table.Column<DateTime>(nullable: false),
                    LastTryTime = table.Column<DateTime>(nullable: true),
                    IsAbandoned = table.Column<bool>(nullable: false),
                    Priority = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicParameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParameterName = table.Column<string>(nullable: true),
                    InputType = table.Column<string>(nullable: true),
                    Permission = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEditions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChangeSets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    Reason = table.Column<string>(maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChangeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    Icon = table.Column<string>(maxLength: 128, nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguageTexts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    LanguageName = table.Column<string>(maxLength: 128, nullable: false),
                    Source = table.Column<string>(maxLength: 128, nullable: false),
                    Key = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(maxLength: 67108864, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLanguageTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false),
                    UserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    ExcludedUserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    TenantIds = table.Column<string>(maxLength: 131072, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotificationSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotificationSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnitRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    OUType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenantNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenantNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserLinkId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    UserNameOrEmailAddress = table.Column<string>(maxLength: 256, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Result = table.Column<byte>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    TenantNotificationId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserOrganizationUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    AuthenticationSource = table.Column<string>(maxLength: 64, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Surname = table.Column<string>(maxLength: 64, nullable: false),
                    Password = table.Column<string>(maxLength: 128, nullable: false),
                    EmailConfirmationCode = table.Column<string>(maxLength: 328, nullable: true),
                    PasswordResetCode = table.Column<string>(maxLength: 328, nullable: true),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IsLockoutEnabled = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    IsPhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(maxLength: 128, nullable: true),
                    IsTwoFactorEnabled = table.Column<bool>(nullable: false),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedEmailAddress = table.Column<string>(maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookName = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    WebhookUri = table.Column<string>(nullable: false),
                    Secret = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Webhooks = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BXJGBaseInfoAdministratives",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGBaseInfoAdministratives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGBaseInfoAdministratives_BXJGBaseInfoAdministratives_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGBaseInfoAdministratives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BXJGCMSAdControls",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    AdControlType = table.Column<int>(nullable: false),
                    ExtensionData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGCMSAdControls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BXJGCMSAdPositions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGCMSAdPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BXJGCMSAds",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    AdType = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGCMSAds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BXJGEquipmentInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Longitude = table.Column<decimal>(nullable: false),
                    Latitude = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGEquipmentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BXJGGeneralTrees",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    IsSysDefine = table.Column<bool>(nullable: false),
                    IsTree = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGGeneralTrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGGeneralTrees_BXJGGeneralTrees_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopDictionaries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(type: "varchar(500)", nullable: true),
                    IsSysDefine = table.Column<bool>(nullable: false),
                    IsTree = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopDictionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopDictionaries_BXJGShopDictionaries_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGShopDictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopItemCategories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(type: "varchar(200)", nullable: true),
                    Image1 = table.Column<string>(type: "varchar(200)", nullable: true),
                    Image2 = table.Column<string>(type: "varchar(200)", nullable: true),
                    ShowInHome = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopItemCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopItemCategories_BXJGShopItemCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGShopItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicParameterValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    DynamicParameterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicParameterValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicParameterValues_AbpDynamicParameters_DynamicParameterId",
                        column: x => x.DynamicParameterId,
                        principalTable: "AbpDynamicParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityDynamicParameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityFullName = table.Column<string>(nullable: true),
                    DynamicParameterId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityDynamicParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityDynamicParameters_AbpDynamicParameters_DynamicParameterId",
                        column: x => x.DynamicParameterId,
                        principalTable: "AbpDynamicParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpFeatures_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChanges",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeTime = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<byte>(nullable: false),
                    EntityChangeSetId = table.Column<long>(nullable: false),
                    EntityId = table.Column<string>(maxLength: 48, nullable: true),
                    EntityTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId",
                        column: x => x.EntityChangeSetId,
                        principalTable: "AbpEntityChangeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 32, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpSettings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ConnectionString = table.Column<string>(maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserClaims_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLogins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserLogins_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Value = table.Column<string>(maxLength: 512, nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserTokens_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSendAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookEventId = table.Column<Guid>(nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSendAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "AbpWebhookEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopCustomers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Integral = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Gender = table.Column<int>(nullable: true),
                    Birthday = table.Column<DateTimeOffset>(nullable: true),
                    AreaId = table.Column<long>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopCustomers_BXJGBaseInfoAdministratives_AreaId",
                        column: x => x.AreaId,
                        principalTable: "BXJGBaseInfoAdministratives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BXJGShopCustomers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BXJGCMSAdRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    AdId = table.Column<long>(nullable: false),
                    AdPositionId = table.Column<long>(nullable: false),
                    AdControlId = table.Column<long>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    PublishStartTime = table.Column<DateTimeOffset>(nullable: true),
                    PublishEndTime = table.Column<DateTimeOffset>(nullable: true),
                    SortIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGCMSAdRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGCMSAdRecords_BXJGCMSAdControls_AdControlId",
                        column: x => x.AdControlId,
                        principalTable: "BXJGCMSAdControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGCMSAdRecords_BXJGCMSAds_AdId",
                        column: x => x.AdId,
                        principalTable: "BXJGCMSAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGCMSAdRecords_BXJGCMSAdPositions_AdPositionId",
                        column: x => x.AdPositionId,
                        principalTable: "BXJGCMSAdPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BXJGCMSColumns",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(type: "varchar(200)", nullable: true),
                    ColumnType = table.Column<int>(nullable: false),
                    ContentTypeId = table.Column<long>(nullable: false),
                    IsSysDefine = table.Column<bool>(nullable: false),
                    SeoTitle = table.Column<string>(maxLength: 2000, nullable: true),
                    SeoDescription = table.Column<string>(maxLength: 5000, nullable: true),
                    SeoKeyword = table.Column<string>(maxLength: 1000, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ListTemplate = table.Column<string>(type: "varchar(200)", nullable: true),
                    DetailTemplate = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGCMSColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGCMSColumns_BXJGGeneralTrees_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGCMSColumns_BXJGCMSColumns_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BXJGCMSColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Sku = table.Column<string>(maxLength: 50, nullable: true),
                    DescriptionShort = table.Column<string>(maxLength: 10000, nullable: true),
                    DescriptionFull = table.Column<string>(nullable: true),
                    Images = table.Column<string>(type: "varchar(5000)", nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    BrandId = table.Column<long>(nullable: true),
                    UnitId = table.Column<long>(nullable: true),
                    Specification = table.Column<string>(maxLength: 500, nullable: true),
                    OldPrice = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Integral = table.Column<int>(nullable: false),
                    Hot = table.Column<bool>(nullable: false),
                    New = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Focus = table.Column<bool>(nullable: false),
                    Published = table.Column<bool>(nullable: false),
                    AvailableStart = table.Column<DateTimeOffset>(nullable: true),
                    AvailableEnd = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopItems_BXJGGeneralTrees_BrandId",
                        column: x => x.BrandId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BXJGShopItems_BXJGShopItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BXJGShopItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopItems_BXJGGeneralTrees_UnitId",
                        column: x => x.UnitId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityDynamicParameterValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: true),
                    EntityDynamicParameterId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityDynamicParameterValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityDynamicParameterValues_AbpEntityDynamicParameters_EntityDynamicParameterId",
                        column: x => x.EntityDynamicParameterId,
                        principalTable: "AbpEntityDynamicParameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityChangeId = table.Column<long>(nullable: false),
                    NewValue = table.Column<string>(maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(maxLength: 96, nullable: true),
                    PropertyTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "AbpEntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsGranted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoleClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopOrders",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<long>(nullable: false),
                    OrderNo = table.Column<string>(type: "varchar(36)", nullable: false),
                    OrderTime = table.Column<DateTimeOffset>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CustomerRemark = table.Column<string>(maxLength: 500, nullable: true),
                    MerchandiseSubtotal = table.Column<decimal>(nullable: false),
                    Integral = table.Column<long>(nullable: false),
                    PaymentMethodId = table.Column<long>(nullable: true),
                    PaymentAmount = table.Column<decimal>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: true),
                    AreaId = table.Column<long>(nullable: false),
                    Consignee = table.Column<string>(maxLength: 20, nullable: false),
                    ConsigneePhoneNumber = table.Column<string>(type: "varchar(50)", nullable: false),
                    ReceivingAddress = table.Column<string>(maxLength: 200, nullable: false),
                    DistributionMethodId = table.Column<long>(nullable: true),
                    LogisticsNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    LogisticsStatus = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrders_BXJGBaseInfoAdministratives_AreaId",
                        column: x => x.AreaId,
                        principalTable: "BXJGBaseInfoAdministratives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrders_BXJGShopCustomers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "BXJGShopCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrders_BXJGGeneralTrees_DistributionMethodId",
                        column: x => x.DistributionMethodId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrders_BXJGGeneralTrees_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "BXJGGeneralTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BXJGCMSArticles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ColumnId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    IsSysDefine = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    SeoTitle = table.Column<string>(maxLength: 2000, nullable: true),
                    SeoDescription = table.Column<string>(maxLength: 5000, nullable: true),
                    SeoKeyword = table.Column<string>(maxLength: 1000, nullable: true),
                    Summary = table.Column<string>(maxLength: 5000, nullable: true),
                    Published = table.Column<bool>(nullable: false),
                    PublishStartTime = table.Column<DateTimeOffset>(nullable: true),
                    PublishEndTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGCMSArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGCMSArticles_BXJGCMSColumns_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "BXJGCMSColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BXJGShopOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(nullable: false),
                    ItemId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    Integral = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TotalIntegral = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BXJGShopOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrderItems_BXJGShopItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "BXJGShopItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BXJGShopOrderItems_BXJGShopOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "BXJGShopOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionDuration",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionDuration" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_UserId",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicParameters_ParameterName_TenantId",
                table: "AbpDynamicParameters",
                columns: new[] { "ParameterName", "TenantId" },
                unique: true,
                filter: "[ParameterName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicParameterValues_DynamicParameterId",
                table: "AbpDynamicParameterValues",
                column: "DynamicParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityChangeSetId",
                table: "AbpEntityChanges",
                column: "EntityChangeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityTypeFullName_EntityId",
                table: "AbpEntityChanges",
                columns: new[] { "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_CreationTime",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_Reason",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "Reason" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_UserId",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityDynamicParameters_DynamicParameterId",
                table: "AbpEntityDynamicParameters",
                column: "DynamicParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityDynamicParameters_EntityFullName_DynamicParameterId_TenantId",
                table: "AbpEntityDynamicParameters",
                columns: new[] { "EntityFullName", "DynamicParameterId", "TenantId" },
                unique: true,
                filter: "[EntityFullName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityDynamicParameterValues_EntityDynamicParameterId",
                table: "AbpEntityDynamicParameterValues",
                column: "EntityDynamicParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_EditionId_Name",
                table: "AbpFeatures",
                columns: new[] { "EditionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_TenantId_Name",
                table: "AbpFeatures",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguages_TenantId_Name",
                table: "AbpLanguages",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key",
                table: "AbpLanguageTexts",
                columns: new[] { "TenantId", "Source", "LanguageName", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_RoleId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_TenantId_Name",
                table: "AbpPermissions",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_RoleId",
                table: "AbpPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_UserId",
                table: "AbpPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_RoleId",
                table: "AbpRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_TenantId_ClaimType",
                table: "AbpRoleClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_CreatorUserId",
                table: "AbpRoles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_DeleterUserId",
                table: "AbpRoles",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_LastModifierUserId",
                table: "AbpRoles",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_TenantId_NormalizedName",
                table: "AbpRoles",
                columns: new[] { "TenantId", "NormalizedName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_UserId",
                table: "AbpSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_TenantId_Name_UserId",
                table: "AbpSettings",
                columns: new[] { "TenantId", "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenantNotifications_TenantId",
                table: "AbpTenantNotifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_CreatorUserId",
                table: "AbpTenants",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_DeleterUserId",
                table: "AbpTenants",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_EditionId",
                table: "AbpTenants",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_LastModifierUserId",
                table: "AbpTenants",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_TenancyName",
                table: "AbpTenants",
                column: "TenancyName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_EmailAddress",
                table: "AbpUserAccounts",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_UserName",
                table: "AbpUserAccounts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_EmailAddress",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "EmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserId",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserName",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_UserId",
                table: "AbpUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_TenantId_ClaimType",
                table: "AbpUserClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_UserId_TenantId",
                table: "AbpUserLoginAttempts",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result",
                table: "AbpUserLoginAttempts",
                columns: new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_UserId",
                table: "AbpUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_UserId",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserNotifications_UserId_State_CreationTime",
                table: "AbpUserNotifications",
                columns: new[] { "UserId", "State", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_UserId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_UserId",
                table: "AbpUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_RoleId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_UserId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_CreatorUserId",
                table: "AbpUsers",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_DeleterUserId",
                table: "AbpUsers",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_LastModifierUserId",
                table: "AbpUsers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedEmailAddress",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedEmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedUserName",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_UserId",
                table: "AbpUserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_TenantId_UserId",
                table: "AbpUserTokens",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhookSendAttempts_WebhookEventId",
                table: "AbpWebhookSendAttempts",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGBaseInfoAdministratives_ParentId",
                table: "BXJGBaseInfoAdministratives",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSAdRecords_AdControlId",
                table: "BXJGCMSAdRecords",
                column: "AdControlId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSAdRecords_AdId",
                table: "BXJGCMSAdRecords",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSAdRecords_AdPositionId",
                table: "BXJGCMSAdRecords",
                column: "AdPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSArticles_ColumnId",
                table: "BXJGCMSArticles",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSColumns_ContentTypeId",
                table: "BXJGCMSColumns",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGCMSColumns_ParentId",
                table: "BXJGCMSColumns",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGGeneralTrees_ParentId",
                table: "BXJGGeneralTrees",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopCustomers_AreaId",
                table: "BXJGShopCustomers",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopCustomers_UserId",
                table: "BXJGShopCustomers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopDictionaries_ParentId",
                table: "BXJGShopDictionaries",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItemCategories_ParentId",
                table: "BXJGShopItemCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItems_BrandId",
                table: "BXJGShopItems",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItems_CategoryId",
                table: "BXJGShopItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopItems_UnitId",
                table: "BXJGShopItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrderItems_ItemId",
                table: "BXJGShopOrderItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrderItems_OrderId",
                table: "BXJGShopOrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_AreaId",
                table: "BXJGShopOrders",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_CustomerId",
                table: "BXJGShopOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_DistributionMethodId",
                table: "BXJGShopOrders",
                column: "DistributionMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_OrderNo",
                table: "BXJGShopOrders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BXJGShopOrders_PaymentMethodId",
                table: "BXJGShopOrders",
                column: "PaymentMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogs");

            migrationBuilder.DropTable(
                name: "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                name: "AbpDynamicParameterValues");

            migrationBuilder.DropTable(
                name: "AbpEntityDynamicParameterValues");

            migrationBuilder.DropTable(
                name: "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "AbpFeatures");

            migrationBuilder.DropTable(
                name: "AbpLanguages");

            migrationBuilder.DropTable(
                name: "AbpLanguageTexts");

            migrationBuilder.DropTable(
                name: "AbpNotifications");

            migrationBuilder.DropTable(
                name: "AbpNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpPermissions");

            migrationBuilder.DropTable(
                name: "AbpRoleClaims");

            migrationBuilder.DropTable(
                name: "AbpSettings");

            migrationBuilder.DropTable(
                name: "AbpTenantNotifications");

            migrationBuilder.DropTable(
                name: "AbpTenants");

            migrationBuilder.DropTable(
                name: "AbpUserAccounts");

            migrationBuilder.DropTable(
                name: "AbpUserClaims");

            migrationBuilder.DropTable(
                name: "AbpUserLoginAttempts");

            migrationBuilder.DropTable(
                name: "AbpUserLogins");

            migrationBuilder.DropTable(
                name: "AbpUserNotifications");

            migrationBuilder.DropTable(
                name: "AbpUserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpUserRoles");

            migrationBuilder.DropTable(
                name: "AbpUserTokens");

            migrationBuilder.DropTable(
                name: "AbpWebhookSendAttempts");

            migrationBuilder.DropTable(
                name: "AbpWebhookSubscriptions");

            migrationBuilder.DropTable(
                name: "BXJGCMSAdRecords");

            migrationBuilder.DropTable(
                name: "BXJGCMSArticles");

            migrationBuilder.DropTable(
                name: "BXJGEquipmentInfo");

            migrationBuilder.DropTable(
                name: "BXJGShopDictionaries");

            migrationBuilder.DropTable(
                name: "BXJGShopOrderItems");

            migrationBuilder.DropTable(
                name: "AbpEntityDynamicParameters");

            migrationBuilder.DropTable(
                name: "AbpEntityChanges");

            migrationBuilder.DropTable(
                name: "AbpRoles");

            migrationBuilder.DropTable(
                name: "AbpEditions");

            migrationBuilder.DropTable(
                name: "AbpWebhookEvents");

            migrationBuilder.DropTable(
                name: "BXJGCMSAdControls");

            migrationBuilder.DropTable(
                name: "BXJGCMSAds");

            migrationBuilder.DropTable(
                name: "BXJGCMSAdPositions");

            migrationBuilder.DropTable(
                name: "BXJGCMSColumns");

            migrationBuilder.DropTable(
                name: "BXJGShopItems");

            migrationBuilder.DropTable(
                name: "BXJGShopOrders");

            migrationBuilder.DropTable(
                name: "AbpDynamicParameters");

            migrationBuilder.DropTable(
                name: "AbpEntityChangeSets");

            migrationBuilder.DropTable(
                name: "BXJGShopItemCategories");

            migrationBuilder.DropTable(
                name: "BXJGShopCustomers");

            migrationBuilder.DropTable(
                name: "BXJGGeneralTrees");

            migrationBuilder.DropTable(
                name: "BXJGBaseInfoAdministratives");

            migrationBuilder.DropTable(
                name: "AbpUsers");
        }
    }
}
