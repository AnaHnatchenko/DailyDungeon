namespace DailyDungeon
{
    using System;
    using System.Collections.Generic;
    
    public partial class habits
    {
        public int id_habit { get; set; }
        public string login_user { get; set; }
        public string name_habit { get; set; }
        public string description_habit { get; set; }
        public string complexity_habit { get; set; }
        public string type_habit { get; set; }
        public string tag_habit { get; set; }
        public bool is_done { get; set; }
    
        public virtual users users { get; set; }
    }
}
