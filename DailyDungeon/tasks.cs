namespace DailyDungeon
{
    using System;
    using System.Collections.Generic;
    
    public partial class tasks
    {
        public int id_task { get; set; }
        public string name_task { get; set; }
        public string description_task { get; set; }
        public string complexity_task { get; set; }
        public string tag_task { get; set; }
        public string deadline_task { get; set; }
        public bool is_done { get; set; }
    }
}
