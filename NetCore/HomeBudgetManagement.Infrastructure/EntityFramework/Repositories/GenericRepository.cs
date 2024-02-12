//****USE FOR General Reposisotry pattern rather that indivudal Repository****//
using HomeBudgetManagement.Core.Domain;
using HomeBudgetManagement.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Infrastructure.EntityFramework.Repositories
{

    //-new() lets you instantiate an object using T like in delete function also you can do this "T item = new T();"
    //also new() constraint will make sure that this type is going to be a non-abstract type with a parameterless constructor
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _entity;
        private readonly HomeBudgetManagementDbContext _context;

        public GenericRepository(HomeBudgetManagementDbContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }

        public async virtual Task AddAsync(T entity)
        {
            await _entity.AddAsync(entity);
        }

        public async virtual Task AddRangeAsync(IList<T> entities)
        {
            await _entity.AddRangeAsync(entities);
        }

        public virtual void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual List<T> ExecuteQuery(string sql) => _entity.FromSqlInterpolated($"{sql}").ToList();

        public virtual List<T> ExecuteQuery(string sql, object[] sqlParametersObjects) => _entity.FromSql($"{sql}").ToList();

        public virtual List<T> GetAll() => _entity.ToList();

        public async virtual Task<List<T>> GetAllAsync() => await _entity.ToListAsync<T>();

        public async virtual Task<T?> GetByIdAsync(int id) => await _entity.FindAsync(id);

        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        //Save changes is handled under unitofwork
        //internal int SaveChanges()
        //{
        //    try
        //    {
        //        return _context.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        //Thrown when there is a concurrency error
        //        //for now, just rethrow the exception
        //        throw;
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        //Thrown when database update fails
        //        //Examine the inner exception(s) for additional 
        //        //details and affected objects
        //        //for now, just rethrow the exception
        //        throw;
        //    }
        //    //catch (CommitFailedException ex)
        //    //{
        //    //    //handle transaction failures here
        //    //    //for now, just rethrow the exception
        //    //    throw;
        //    //}
        //    catch (Exception ex)
        //    {
        //        //some other exception happened and should be handled
        //        throw;
        //    }
        //}
    }
}
