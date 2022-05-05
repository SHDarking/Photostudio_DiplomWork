using Microsoft.AspNetCore.Mvc;
using lab1.Controllers;
using lab1.Dto;
using lab1.Models;
using Xunit;
using System.Threading.Tasks;

namespace lab1.Tests
{
    public class RegisterAPI
    {
        [Fact]
        public async Task RegisterApiTest()
        {
            // arrange
            PhotostudioInnoDbContext context = new PhotostudioInnoDbContext();
            AccountController controller = new AccountController(context);
            RegisterModel model = new RegisterModel("Dima", "Gladkiy", "380501112233", "123456Test.mail@gmail.com", "1234");
            // act
            var result = controller.Registration(model);
            //assert
                        
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Home",redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        } 
        [Fact]
        public async Task LoginApiTest()
        {
            //arrange
            PhotostudioInnoDbContext context = new PhotostudioInnoDbContext();
            AccountController controller = new AccountController(context);
            LoginModel model = new LoginModel("admin.mail@gmail.com", "Admin");

            // act
            var result = controller.Login(model);

            // assert

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Manage", redirectToActionResult.ControllerName);
            Assert.Equal("UserAdministrationPanel", redirectToActionResult.ActionName);
        }
    }
}
