using Microsoft.EntityFrameworkCore;
using WebAPITemp.Models.Entities;

namespace WebAPITemp.DBContexts.EFCore
{
    public class ProjectDBContext : DbContext
    {
        #region Entities
        public DbSet<User> User { get; set; }
        public DbSet<UserPasswordRecord> UserPasswordRecord { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }

        public DbSet<Page> Page { get; set; }
        public DbSet<PageSection> PageSection { get; set; }
        public DbSet<Models.Entities.System> System { get; set; }
        public DbSet<LogInRecord> LogInRecord { get; set; }
        public DbSet<Bulletin> Bulletin { get; set; }
        #endregion

        public ProjectDBContext(DbContextOptions<ProjectDBContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                //設定索引
                entity.HasIndex(e => e.UserAccount).IsClustered(false).HasDatabaseName("IX_User_UserID");
                entity.HasIndex(e => e.RoleID).IsClustered(false).HasDatabaseName("IX_User_RoleID");
                entity.HasIndex(e => e.Language).IsClustered(false).HasDatabaseName("IX_User_Language");
                entity.HasIndex(e => e.Status).IsClustered(false).HasDatabaseName("IX_User_Status");
                entity.HasIndex(e => e.CreateOn).IsClustered(false).HasDatabaseName("IX_User_CreateOn");
                entity.HasIndex(e => e.UpdateOn).IsClustered(false).HasDatabaseName("IX_User_UpdateOn");

                //新增欄位備註
                entity.Property(e => e.UserID).HasComment("使用者編號");
                entity.Property(e => e.UserAccount).HasComment("使用者帳號");
                entity.Property(e => e.RoleID).HasComment("角色編號");
                entity.Property(e => e.Language).HasComment("使用語言");
                entity.Property(e => e.TimeZone).HasComment("使用者時區");
                entity.Property(e => e.Status).HasComment("狀態");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");
                entity.Property(e => e.UpdateBy).HasComment("更新者");
                entity.Property(e => e.UpdateOn).HasComment("更新時間");
            });
            modelBuilder.Entity<UserPasswordRecord>(entity => 
            {
                //設定索引
                entity.HasKey(e => new { e.UserID, e.Password, e.IsEnable }).IsClustered(false);
                entity.HasIndex(e => e.UserID).IsClustered(true).HasDatabaseName("IX_UserPasswordRecord_UserID");

                //新增欄位備註
                entity.Property(e => e.UserID).HasComment("使用者編號");
                entity.Property(e => e.Password).HasComment("密碼");
                entity.Property(e => e.IsEnable).HasComment("是否啟用");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");

            });         
            modelBuilder.Entity<UserProfile>(entity =>
            {
                //新增欄位備註
                entity.Property(e => e.UserID).HasComment("使用者編號");
                entity.Property(e => e.Address).HasComment("聯絡地址");
                entity.Property(e => e.ENName).HasComment("使用者姓名(英文)");
                entity.Property(e => e.CNName).HasComment("使用者姓名(中文)");
                entity.Property(e => e.Gender).HasComment("性別");
                entity.Property(e => e.Nationality).HasComment("國籍");
                entity.Property(e => e.ContactNumber).HasComment("連絡電話");
                entity.Property(e => e.OtherNumber).HasComment("其他連絡電話");
                entity.Property(e => e.ContactEmail).HasComment("電子信箱");
                entity.Property(e => e.OtherEmail).HasComment("其他電子信箱");
                entity.Property(e => e.Certifications).HasComment("相關證照");
                entity.Property(e => e.Introduction).HasComment("簡介");
                entity.Property(e => e.Note).HasComment("備註");
                entity.Property(e => e.ReviewStatus).HasComment("資料審查狀態");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");
                entity.Property(e => e.UpdateBy).HasComment("更新者");
                entity.Property(e => e.UpdateOn).HasComment("更新時間");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                //設定索引
                entity.HasIndex(e => e.RoleName).IsClustered(false).HasDatabaseName("IX_Role_RoleName");
                entity.HasIndex(e => e.IsHightestAdministrator).IsClustered(false).HasDatabaseName("IX_Role_IsHightestAdministrator");
                entity.HasIndex(e => e.IsEnable).IsClustered(false).HasDatabaseName("IX_Role_IsEnable");

                //新增欄位備註
                entity.Property(e => e.RoleID).HasComment("角色編號");
                entity.Property(e => e.RoleName).HasComment("角色名稱");
                entity.Property(e => e.IsHightestAdministrator).HasComment("是否為最高權限");
                entity.Property(e => e.IsEnable).HasComment("是否啟用");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");
                entity.Property(e => e.UpdateBy).HasComment("更新者");
                entity.Property(e => e.UpdateOn).HasComment("更新時間");

                //預設資料
                entity.Property(e => e.IsHightestAdministrator).HasDefaultValue(false);
                entity.HasData(new Role { RoleID = 1, RoleName = "Management", IsHightestAdministrator = true, IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new Role { RoleID = 2, RoleName = "Admin", IsHightestAdministrator = false, IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new Role { RoleID = 13, RoleName = "Developer", IsHightestAdministrator = true, IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
            });
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => new { e.RoleID, e.PageID }).IsClustered(false);

                //新增欄位備註
                entity.Property(e => e.RoleID).HasComment("角色編號");
                entity.Property(e => e.PageID).HasComment("功能頁編號");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");

                //預設資料
                #region Management
                entity.HasData(new Permission { RoleID = 1, PageID = 1, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 2, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 3, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 4, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 5, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 6, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 7, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 8, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 9, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 10, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 11, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 12, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 13, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 14, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 15, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 16, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 17, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 18, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 19, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 20, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 21, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 22, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 23, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 24, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 25, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 26, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 27, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 28, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 29, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 30, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 31, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 32, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 33, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 34, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 35, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 36, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 37, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 38, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 39, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 40, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 1, PageID = 41, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                #endregion
                #region Admin
                entity.HasData(new Permission { RoleID = 2, PageID = 4, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 5, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 6, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 7, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 8, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 9, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 10, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 11, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 12, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 13, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 14, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 15, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 16, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 17, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 18, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 19, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 20, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 21, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 22, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 23, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 24, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 25, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 26, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 27, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 28, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 29, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 30, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 31, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 32, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 33, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 34, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 35, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 36, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 37, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 38, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 39, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 40, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 2, PageID = 41, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                #endregion

                #region Developer
                entity.HasData(new Permission { RoleID = 13, PageID = 1, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 2, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 3, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 4, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 5, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 6, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 7, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 8, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 9, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 10, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 11, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 12, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 13, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 14, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 15, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 16, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 17, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 18, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 19, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 20, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 21, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 22, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 23, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 24, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 25, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 26, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 27, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 28, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 29, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 30, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 31, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 32, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 33, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 34, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 35, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 36, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 37, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 38, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 39, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 40, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 41, CreateBy = 0, CreateOn = new DateTime(2023, 6, 8) });
                entity.HasData(new Permission { RoleID = 13, PageID = 42, CreateBy = 0, CreateOn = new DateTime(2024, 1, 18) });
                #endregion
            });
            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(e => new { e.PageID, e.SystemID }).IsClustered(false);

                //新增欄位備註
                entity.Property(e => e.PageID).HasComment("功能頁編號");
                entity.Property(e => e.SystemID).HasComment("所屬系統編號");
                entity.Property(e => e.PageName).HasComment("功能頁名稱");
                entity.Property(e => e.ParentPageID).HasComment("上層功能頁編號");
                entity.Property(e => e.PageFor).HasComment("頁面角色分類");
                entity.Property(e => e.SectionID).HasComment("功能頁分類編號");
                entity.Property(e => e.IsEnable).HasComment("是否啟用");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");
                entity.Property(e => e.UpdateBy).HasComment("更新者");
                entity.Property(e => e.UpdateOn).HasComment("更新時間");

                #region 預設資料
                entity.Property(e => e.SectionID).HasDefaultValue(1);
                entity.HasData(new Page { PageID = 1, SystemID = 1, IsEnable = true, PageName = "Announcement", PageFor = "Admin", ParentPageID = 0, SectionID = 2, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new Page { PageID = 2, SystemID = 1, IsEnable = true, PageName = "Punch System", PageFor = "Admin", ParentPageID = 0, SectionID = 2, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new Page { PageID = 3, SystemID = 1, IsEnable = true, PageName = "Salary Management", PageFor = "Admin", ParentPageID = 0, SectionID = 2, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                #endregion
            });
            modelBuilder.Entity<PageSection>(entity =>
            {
                //設定索引
                entity.HasIndex(e => e.IsEnable).IsClustered(false).HasDatabaseName("IX_PageSection_IsEnable");

                //新增欄位備註
                entity.Property(e => e.PageSectionID).HasComment("功能頁分類編號");
                entity.Property(e => e.PageSectionName).HasComment("功能頁分類名稱");
                entity.Property(e => e.IsEnable).HasComment("是否啟用");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");
                entity.Property(e => e.UpdateBy).HasComment("更新者");
                entity.Property(e => e.UpdateOn).HasComment("更新時間");

                //預設資料
                entity.HasData(new PageSection { PageSectionID = 1, PageSectionName = "Default", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 2, PageSectionName = "Management", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 3, PageSectionName = "Admin", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 4, PageSectionName = "Administrative Performance", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 5, PageSectionName = "Teaching Performance", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 6, PageSectionName = "Scheduling System", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 7, PageSectionName = "Lead & Senior Teacher", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 8, PageSectionName = "Links", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
                entity.HasData(new PageSection { PageSectionID = 9, PageSectionName = "DashBoard", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 6, 17) });
            });
            modelBuilder.Entity<Models.Entities.System>(entity =>
            {
                //新增欄位備註
                entity.Property(e => e.SystemID).HasComment("系統編號");
                entity.Property(e => e.SystemName).HasComment("系統名稱");
                entity.Property(e => e.Memo).HasComment("備註");
                entity.Property(e => e.IsEnable).HasComment("是否啟用");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");
                entity.Property(e => e.UpdateBy).HasComment("更新者");
                entity.Property(e => e.UpdateOn).HasComment("更新時間");

                //預設資料
                entity.HasData(new Models.Entities.System { SystemID = 1, SystemName = "教學系統", Memo = "教學系統", IsEnable = true, CreateBy = 0, CreateOn = new DateTime(2023, 5, 14) });
            });
            modelBuilder.Entity<LogInRecord>(entity => {
                entity.HasKey(e => new { e.SystemID, e.UserID, e.Token }).IsClustered(false);
                entity.HasIndex(e => e.UserID).IsClustered(true);

                //新增欄位備註
                entity.Property(e => e.SystemID).HasComment("系統編號");
                entity.Property(e => e.UserID).HasComment("使用者編號");
                entity.Property(e => e.Token).HasComment("登入Token");
                entity.Property(e => e.IsEnable).HasComment("是否啟用");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");

                //預設資料
                entity.Property(e => e.IsEnable).HasDefaultValue(true);
            });
            modelBuilder.Entity<Bulletin>(entity => {
                entity.HasIndex(e => e.Area).IsClustered(false).HasDatabaseName("IX_Bulletin_Area");

                //新增欄位備註
                entity.Property(e => e.SerialNumber).HasComment("流水編號");
                entity.Property(e => e.Subject).HasComment("公告主題");
                entity.Property(e => e.Article).HasComment("公告內容");
                entity.Property(e => e.Area).HasComment("顯示區域");
                entity.Property(e => e.StartOn).HasComment("上架時間");
                entity.Property(e => e.EndOn).HasComment("下架時間");
                entity.Property(e => e.CreateBy).HasComment("建立者");
                entity.Property(e => e.CreateOn).HasComment("建立時間");
                entity.Property(e => e.UpdateBy).HasComment("更新者");
                entity.Property(e => e.UpdateOn).HasComment("更新時間");
            });
        }
    }
}
