namespace DailyDungeon
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DailyDungeonEntities : DbContext
    {
        private static DailyDungeonEntities context;
        public DailyDungeonEntities()
            : base("name=DailyDungeonEntities1")
        {
        }

        public static DailyDungeonEntities GetContext()
        {
            if (context == null)
                context = new DailyDungeonEntities();

            return context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<habits> habits { get; set; }
        public virtual DbSet<tasks> tasks { get; set; }
        public virtual DbSet<users> users { get; set; }
    }
}
