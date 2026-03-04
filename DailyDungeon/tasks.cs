namespace DailyDungeon
{
    public partial class tasks
    {
        public int id_task { get; set; }
        public string login_user { get; set; }
        public string name_task { get; set; }
        public string description_task { get; set; }
        public string complexity_task { get; set; }
        public string deadline_task { get; set; }
        public string tag_task { get; set; }
        public bool is_done { get; set; }

        public int ExecutionCost()
        {
            int cost = 0;
            if (complexity_task == "Легко") cost = 50;
            else if (complexity_task == "Середньо") cost = 100;
            else if (complexity_task == "Складно") cost = 150;
            return cost;
        }

        public virtual users users { get; set; }
    }
}
