using TestSolution.Data;

namespace TestSolution.Repositories
{
    public class TaskRepository : RepositoryBase<Task>
    {
        protected override void UpdateMutableFields(Task oldTask, Task newTask)
        {
            oldTask.Title = newTask.Title;
            oldTask.Responsible = newTask.Responsible;
            oldTask.Executor = newTask.Executor;
        }
    }
}
