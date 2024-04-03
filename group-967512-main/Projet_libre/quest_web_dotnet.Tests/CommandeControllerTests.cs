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
            //    return Ok(commandes) ;
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);


        }


        [Fact]
        public void TestTwo()
        {
            //  return Ok(liste_commande) ;
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);


            // 404 - return NotFound( new { message = "Aucune commande trouve", User_id = userdb.Id } ) ; 


        }


        [Fact]
        public void TestThree()
        {
            // return true  ;
            // return false ; 
        }

        [Fact]
        public void TestFor()
        {
            //             return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ; 
            var result = _client.PostAsync("/commande").Result;


            // return StatusCode(409, new{message = "Vous avez deja commande cet article"} ) ;
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);

            //     return StatusCode(201,commande) ;
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        }

        [Fact]
        public void TestFive()
        {
            //return Unauthorized(new { message = "Vous n'etes pas autorise !" } ) ;
            var result = _client.DeleteAsync("/commande/1").Result;


            //return NotFound(new { success = verif }) ;

            //return Ok(new { success = verif }) ;
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest);


        }

    }
}
