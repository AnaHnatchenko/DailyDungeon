namespace DailyDungeon
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DailyDungeonEntities : DbContext
    {
        public DailyDungeonEntities()
            : base("name=DailyDungeonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<avatars> avatars { get; set; }
        public virtual DbSet<backgrounds> backgrounds { get; set; }
        public virtual DbSet<habits> habits { get; set; }
        public virtual DbSet<tasks> tasks { get; set; }
        public virtual DbSet<users> users { get; set; }
    }
}
