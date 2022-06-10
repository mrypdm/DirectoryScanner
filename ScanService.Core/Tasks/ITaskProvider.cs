using System;
using System.Threading.Tasks;

namespace ScanService.Core.Tasks
{
    public interface ITaskProvider
    {
        Task<TResult> CreateTask<TResult>(Func<Task<TResult>> func);
    }
}