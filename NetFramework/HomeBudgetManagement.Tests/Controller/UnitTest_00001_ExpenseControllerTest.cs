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

namespace HomeBudgetManagement.Tests.Controllers
{
    [TestClass]
    public class UnitTest_00001_ExpenseControllerTest
    {
        //Testing async method using interface(abstraction)
        [TestMethod]
        public async Task UnitTest_00001_List_00001_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IExpenseRepository>();
            mockRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult<List<Expense>>(new List<Expense>() {
                    new Expense() {Id = 100, Amount = 200.00, Description = "desc", Date = DateTime.Now},
                    new Expense() {Id = 200, Amount = 200.00, Description = "desc", Date = DateTime.Now}
            }));

            var controller = new ExpenseController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = await controller.Expenses();
            var contentResult = actionResult as OkNegotiatedContentResult<List<Expense>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Count > 0);
        }

        [TestMethod]
        public async Task UnitTest_00001_PostExpense_00001_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IExpenseRepository>();
            var controller = new ExpenseController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = await controller.AddExpense(new Expense() { Id = 10, Amount=100.00, Date= DateTime.Today, Description = "This is a Description" });
            var createdResult = actionResult as CreatedNegotiatedContentResult<Expense>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("PostExpense", createdResult.Location.ToString());
            Assert.AreEqual(10, createdResult.Content.Id);
        }

        #region EXAMPLE TEST METHODS FROM MS WEB SITE
        //[TestMethod]
        //public void PostMethodSetsLocationHeader()
        //{
        //    // Arrange
        //    var mockRepository = new Mock<IProductRepository>();
        //    var controller = new Products2Controller(mockRepository.Object);

        //    // Act
        //    IHttpActionResult actionResult = controller.Post(new Product { Id = 10, Name = "Product1" });
        //    var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

        //    // Assert
        //    Assert.IsNotNull(createdResult);
        //    Assert.AreEqual("DefaultApi", createdResult.RouteName);
        //    Assert.AreEqual(10, createdResult.RouteValues["id"]);
        //}

        //[TestMethod]
        //public void GetReturnsNotFound()
        //{
        //    // Arrange
        //    var mockRepository = new Mock<IProductRepository>();
        //    var controller = new Products2Controller(mockRepository.Object);

        //    // Act
        //    IHttpActionResult actionResult = controller.Get(10);

        //    // Assert
        //    Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        //}

        //[TestMethod]
        //public void GetReturnsProductWithSameId()
        //{
        //    // Arrange
        //    var mockRepository = new Mock<IProductRepository>();
        //    mockRepository.Setup(x => x.GetById(42))
        //        .Returns(new Product { Id = 42 });

        //    var controller = new Products2Controller(mockRepository.Object);

        //    // Act
        //    IHttpActionResult actionResult = controller.Get(42);
        //    var contentResult = actionResult as OkNegotiatedContentResult<Product>;

        //    // Assert
        //    Assert.IsNotNull(contentResult);
        //    Assert.IsNotNull(contentResult.Content);
        //    Assert.AreEqual(42, contentResult.Content.Id);
        //}

        //[TestMethod]
        //public void PutReturnsContentResult()
        //{
        //    // Arrange
        //    var mockRepository = new Mock<IProductRepository>();
        //    var controller = new Products2Controller(mockRepository.Object);

        //    // Act
        //    IHttpActionResult actionResult = controller.Put(new Product { Id = 10, Name = "Product" });
        //    var contentResult = actionResult as NegotiatedContentResult<Product>;

        //    // Assert
        //    Assert.IsNotNull(contentResult);
        //    Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
        //    Assert.IsNotNull(contentResult.Content);
        //    Assert.AreEqual(10, contentResult.Content.Id);
        //}
        #endregion
    }
}
