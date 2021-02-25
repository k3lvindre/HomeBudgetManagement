//****USE FOR General Reposisotry pattern rather that indivudal Repository****//

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using HomeBudgetManagement.Models;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;

//namespace HomeBudgetManagement.Domain
//{
//    public class BaseRepo<T> : IDisposable, IRepo<T> where T : BaseEntity, new() //-new() lets you instantiate an object using T like in delete function also you can do this "T item = new T();"
//    {
//        //internal - accessible only within same assembly
//        internal HomeBudgetManagementContext db;
//        internal DbSet<T> table;

//        public BaseRepo()
//        {
//            db = new HomeBudgetManagementContext();
//            table = db.Set<T>();
//        }

//        public void Dispose() => db?.Dispose();

//        public virtual int Add(T entity)
//        {
//            table.Add(entity);
//            return SaveChanges();
//        }

//        public virtual int AddRange(IList<T> entities)
//        {
//            table.AddRange(entities);
//            return SaveChanges();
//        }

//        public virtual int Delete(int id, byte[] timeStamp)
//        {
//            db.Entry(new T() { Id = id, Timestamp = timeStamp }).State = EntityState.Deleted;
//            return SaveChanges();
//        }

//        public virtual int Delete(T entity)
//        {
//            db.Entry(entity).State = EntityState.Deleted;
//            return SaveChanges();
//        }

//        public virtual List<T> ExecuteQuery(string sql) => table.SqlQuery(sql).ToList();

//        public virtual List<T> ExecuteQuery(string sql, object[] sqlParametersObjects) => table.SqlQuery(sql, sqlParametersObjects).ToList();

//        public virtual List<T> GetAll() => table.ToList();

//        public virtual Task<List<T>> GetAllAsync() => table.ToListAsync<T>();

//        public virtual  T GetOne(int? id) => table.Find(id);

//        public virtual int Save(T entity)
//        {
//            db.Entry(entity).State = EntityState.Modified;
//            return SaveChanges();
//        }

//        internal int SaveChanges()
//        {
//            try
//            {
//                return db.SaveChanges();
//            }
//            catch (DbUpdateConcurrencyException ex)
//            {
//                //Thrown when there is a concurrency error
//                //for now, just rethrow the exception
//                throw;
//            }
//            catch (DbUpdateException ex)
//            {
//                //Thrown when database update fails
//                //Examine the inner exception(s) for additional 
//                //details and affected objects
//                //for now, just rethrow the exception
//                throw;
//            }
//            catch (CommitFailedException ex)
//            {
//                //handle transaction failures here
//                //for now, just rethrow the exception
//                throw;
//            }
//            catch (Exception ex)
//            {
//                //some other exception happened and should be handled
//                throw;
//            }
//        }


//    }
//}
