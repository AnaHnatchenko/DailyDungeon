namespace DailyDungeon
{
    public partial class avatars
    {
        public int id_avatar { get; set; }
        public string login_user { get; set; }
        public string image_source { get; set; }
        public bool is_used { get; set; }
    
        public virtual users users { get; set; }
    }
}
