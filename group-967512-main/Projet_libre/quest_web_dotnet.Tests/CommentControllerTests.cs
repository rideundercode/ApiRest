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
        public void TestAddressGetNoTokenBearer()
        {
            _client.DefaultRequestHeaders.Authorization = null;

            var result = _client.GetAsync("/address").Result;

            //HttpStatusCode.Unauthorized = 401
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public void TestOne()
        {
            //return Ok(comments) ;
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);



        }


        [Fact]
        public void TestTwo()
        {
            //return Ok(comments) ;
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);

            //return NotFound( new { message = "Pas de commentaire", article_id = id } ) ; 

        }


        [Fact]
        public void TestThree()
        {
            //return true  ;

            //return false ;
        }

        [Fact]
        public void TestFor()
        {
            //return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ;
            var result = _client.PostAsync("/comment").Result;


            //return StatusCode(409, new {message = "Vous avez deja commente" } ) ;
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);

            //return StatusCode(201,comment) ;
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);

            // return StatusCode(400, e) ; 
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);


        }

        [Fact]
        public void TestFive()
        {
            //return Unauthorized(new { message = "Vous n'avez pas l'autorisation !" });
            var result = _client.PutAsync("/comment/1").Result;

            //return NotFound(new { id = id });


            //return Ok(commentdb);
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);

            //return StatusCode(400, e);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);


        }

        [Fact]
        public void TestSix()
        {
            //return Unauthorized(new { message = "Vous n'etes pas autorise !" });
            var result = _client.DeleteAsync("/comment/1").Result;

            //return NotFound(new { success = verif });

            //return Ok(new { success = verif });
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);


        }
    }
}