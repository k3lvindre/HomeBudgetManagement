namespace HomeBudgetManagement.SharedKernel
{
    //Unlike in HomeBudgetManagement.API project here we removed the implementation of IDisposable because .net core handles the disposal by adding scoped or transient lifetime in startup
    //Here, we are creating the IGenericRepository interface as a Generic Interface
    //Here, we are applying the Generic Constraint 
    //The constraint is T which is going to be a class
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(IList<T> entities);

        void Delete(T entity);

        List<T> ExecuteQuery(string sql);

        List<T> ExecuteQuery(string sql, object[] sqlParametersObjects);

        List<T> GetAll();

        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        void Update(T entity);
    }
}
