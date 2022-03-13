using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        /// <summary>
        /// Id is George Harrison
        /// Because I dont have delte implemented to call after creation, use random to create new primary key on automated unit test
        /// 
        /// </summary>
        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var emp = new Employee();


            emp.EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c";
            emp.FirstName="George";
            emp.LastName= "Harrison";
            emp.Position= "Developer III";
            emp.Department="Engineering";
            emp.DirectReports=null;
            Random rnd = new Random();
            int randomPrimaryKey = rnd.Next(12, 9999999);
            var comp = new Compensation()
            {
                CompensationId = randomPrimaryKey,
                employee = emp,
                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                Salary = 123456,
                EffectiveDate = DateTime.UtcNow,
            };

            var requestContent = new JsonSerialization().ToJson(comp);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var compId = 2;//loaded from compSeed in Json file

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{compId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
