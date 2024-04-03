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
            //return Ok(user_existant) ;
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);


        }


        [Fact]
        public void TestTwo()
        {
            //return Ok(addresses);
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);

        }
    }
}