using System;
using Xunit;

using quest_web.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using quest_web.Utils;

using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace quest_web_dotnet.Tests
{
    public class AddressControllerTest
    {
        private static readonly Uri localhost = new Uri("http://localhost:8090");
        private HttpClient _client = new HttpClient { BaseAddress = localhost };

        [Fact]
        public void TestAddressGetOne()
        {
            //return db.Users.Where(a => a.Username == usernameToken).FirstOrDefault();
            _client.DefaultRequestHeaders.Authorization = null;

            var result = _client.GetAsync("/article").Result;


        }

        [Fact]
        public void TestAddressGetSecond()
        {
            var result = _client.GetAsync("/article/1").Result;

            //401
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        }


        [Fact]
        public void TestThirdAddAddressNoToken()
        {
            //401  -  return Ok(article);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            //404  -  return NotFound(new { message = "article non trouve", article_id = id });



        }


        [Fact]
        public void Testforth()
        {
            //return Unauthorized(new { message = "Vous n'etes pas autorise !" });
            var result = _client.PostAsync("/article").Result;

            //409   -   return StatusCode(409, ex);
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);

            //201   -   return StatusCode(201, article);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        }

        [Fact]
        public void TestFifth()
        {
            //200  -  return Ok(articledb);
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);

            //400  -  return StatusCode(400, e);
            Assert.Equal(HttpStatusCode.BadRequest, StatusCodeTest);
        }

        [Fact]
        public void TestSixth()
        {

            //return Unauthorized(new { message = "Vous n'etes pas autorise !" });
            var result = _client.DeleteAsync("/article/1").Result;


            //return NotFound(new { success = verif });

            //200
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);
        }

    }
}
