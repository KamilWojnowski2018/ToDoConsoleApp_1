using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    class TaskModel
    {
        public string Task { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Priority { get; set; }
        public string TaskDuration { get; set; }


        public TaskModel(string task, DateTime dateStart, string priority)
        {
            this.Task = task;
            this.DateStart = dateStart;
            this.Priority = priority;
            this.TaskDuration = "Zadanie całodniowe";
        }

        public TaskModel(string task, DateTime dateStart, DateTime dateEnd, string priority)
        {
            this.Task = task;
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
            this.Priority = priority;
            TimeSpan time = new TimeSpan(DateEnd.Value.Ticks - DateStart.Ticks);
            this.TaskDuration = time.Days.ToString() + " dni";
        }
    }
}
