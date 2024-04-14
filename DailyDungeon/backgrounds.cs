namespace DailyDungeon
{
    using System;
    using System.Collections.Generic;
    
    public partial class backgrounds
    {
        public int id_background { get; set; }
        public string login_user { get; set; }
        public string background_color { get; set; }
        public bool is_used { get; set; }
    
        public virtual users users { get; set; }
    }
}
