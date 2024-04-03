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
        public void TestOne()
        {
            //return Ok("success") ;
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);
        }


        [Fact]
        public void TestTwo()
        {
            //return NotFound("not found") ;
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public void TestThree()
        {
            //return StatusCode(500, "error") ;
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}
