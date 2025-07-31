using Microsoft.EntityFrameworkCore;
using CustomerManager.Core.Models;

namespace CustomerManager.Data.DataAccess
{
    /// <summary>
    /// Entity Framework DbContext
    /// データベースとの接続とエンティティの設定を管理
    /// </summary>
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 顧客テーブル
        /// </summary>
        public DbSet<Customer> Customers { get; set; } = null!;

        /// <summary>
        /// モデル設定
        /// </summary>
        /// <param name="modelBuilder">モデルビルダー</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // customersテーブルの設定
            modelBuilder.Entity<Customer>(entity =>
            {
                // テーブル名を明示的に指定
                entity.ToTable("customers");

                // 主キー
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd(); // AUTO_INCREMENTに対応

                // 氏名（必須）
                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsRequired();

                // フリガナ
                entity.Property(e => e.Kana)
                    .HasColumnName("kana")
                    .HasMaxLength(255);

                // 電話番号
                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(20);

                // メールアドレス（必須・一意）
                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsRequired();

                // メールアドレスの一意制約
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                // 作成日時
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime");

                // 更新日時
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");
            });
        }

        /// <summary>
        /// 保存時に作成日時・更新日時を自動設定
        /// </summary>
        /// <returns>保存された件数</returns>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// 非同期保存時に作成日時・更新日時を自動設定
        /// </summary>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>保存された件数</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// エンティティの作成日時・更新日時を更新
        /// </summary>
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<Customer>();
            var now = DateTime.Now;

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.UpdatedAt = now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        break;
                }
            }
        }
    }
}