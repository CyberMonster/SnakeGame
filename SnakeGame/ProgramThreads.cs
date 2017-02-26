using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class ProgramThreads
    {
        public int SettingsClosed = 1;
        private List<System.Threading.Tasks.Task> Tasks;
        public ProgramThreads()
        {
            this.Tasks = new List<Task>();
        }
        private void StopProgram()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        internal void AddTask(System.Threading.Tasks.Task Task_)
        {
            this.Tasks.Add(Task_);
        }
        private int GetActiveTasks()
        {
            return this.Tasks.Where(x => x.Status == TaskStatus.Running).Count();
        }
        internal bool KillMainThread()
        {
            var a = GetActiveTasks();
            if (GetActiveTasks() + this.SettingsClosed == 1)
            {
                try
                {
                    StopProgram();
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
