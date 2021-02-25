using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HomeBudgetManagement.Domain;
using HomeBudgetManagement.Models;
using HomeBudgetManagement.API.Controllers;
using System.Threading.Tasks;
using System.Data.Entity;
using Moq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Net.Http;

namespace HomeBudgetManagement.Tests.Repositories
{
    [TestClass]
    public class UnitTest_00001_ExpenseRepositoryTest
    {
        [TestMethod]
        public async Task UnitTest_00001_CreateAsync_00001_VerifySaveChanges()
        {
            var mockSet = new Mock<DbSet<Expense>>();

            var mockContext = new Mock<HomeBudgetManagementContext>();
            mockContext.Setup(m => m.Expenses).Returns((mockSet.Object));
            //we can also setup return value of savechanges of mocked dbcontext
            mockContext.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

            var repo = new ExpenseRepository(mockContext.Object);
            await repo.CreateAsync(new Expense() { Id = 100, Amount = 200.00, Description = "desc", Date = DateTime.Now });

            //verify that savechangesasync was called and return 1 that we set up above
            mockContext.Verify(m => m.SaveChangesAsync());
        }
    }
}
