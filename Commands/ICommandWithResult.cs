namespace ar.AzureFunctions.Commands
{
    /// <summary>
    /// Command with a result that takes in additional criteria
    /// </summary>
    /// <typeparam name="TCriteria">Additional criteria for the command</typeparam>
    /// <typeparam name="TResult">Results of the command</typeparam>
    public interface ICommandWithResult<in TCriteria, out TResult > : ICommand
    {
        TCriteria Criteria{set;}
        TResult Result {get;}
    }

    /// <summary>
    /// Command with a result
    /// </summary>
    /// <typeparam name="TResult">Results of the command</typeparam>
    public interface ICommandWithResult<out TResult> : ICommand
    {
        TResult Result{get;}
    }
}