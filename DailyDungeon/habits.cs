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

        public int ExecutionCost()
        {
            int cost = 0;

            if (complexity_habit == "Легко")
            {
                cost = 50;
            }
            else if (complexity_habit == "Середньо")
            {
                cost = 100;
            }
            else if (complexity_habit == "Складно")
            {
                cost = 150;
            }

            if (type_habit == "Нейтральна")
            {
                cost = 0;
            }
            else if (type_habit == "Негативна")
            {
                cost = -cost;
            }

            return cost;
        }

        public virtual users users { get; set; }
    }
}
