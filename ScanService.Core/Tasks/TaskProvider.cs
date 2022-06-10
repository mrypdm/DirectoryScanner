using System;
using System.Threading.Tasks;

namespace ScanService.Core.Tasks
{
    public class TaskProvider : ITaskProvider
    {
        public Task<TResult> CreateTask<TResult>(Func<Task<TResult>> func) => Task.Run(func);
    }
}