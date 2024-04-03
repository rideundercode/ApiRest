using System;
using Xunit;

using quest_web.Models ;
using System.Net.Http ;
using System.Net ;
using System.Net.Http.Headers ;
using quest_web.Utils ;

using System.Net.Http.Json ;
using System.Text.Json.Serialization ;
using System.Text.Json ;

namespace quest_web_dotnet.Tests
{
    public class AddressControllerTest
    {
        private static readonly Uri localhost = new Uri("http://localhost:8090") ; 
        private HttpClient _client = new HttpClient { BaseAddress = localhost } ;

        [Fact]
        public void TestAddressGetNoTokenBearer()
        {
            _client.DefaultRequestHeaders.Authorization = null ;
            
            var result = _client.GetAsync("/address").Result ; 

            //HttpStatusCode.Unauthorized = 401
            Assert.Equal(HttpStatusCode.Unauthorized , result.StatusCode )  ; 
        }

        [Fact]
        public void TestAddressGetTokenBearer()
        {
            var userco = new UserDetails{Username = "MyUserName", Role = UserRole.ROLE_USER } ;
            var tokenclass = new JwtTokenUtil() ;

            var token = tokenclass.GenerateToken(userco) ;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
                
            var result =  _client.GetAsync("/address").Result ;  
            var StatusCodeTest = result.StatusCode ; 
            
            //HttpStatusCode.Ok = 200
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest)  ; 
        }


        [Fact]
        public void TestAddAddress()
        {
            var UserTest = new User{ Username = "MyUserName", Password = "MyPass", Role = UserRole.ROLE_USER } ;
            var UserDetails = UserTest.GetUserDetails() ;

            var tokenclass = new JwtTokenUtil() ;
            var token = tokenclass.GenerateToken( UserDetails ) ;
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;

            var AddressTest = new Address { 
                                UserId = UserTest,                    
                                Street = "Street The",
                                PostalCode = "123456" ,
                                City = "QWERTY" ,
                                Country = "ASDFG"
                            } ;
            
            var optionsjson = new JsonSerializerOptions {
                            ReferenceHandler = ReferenceHandler.Preserve
                        } ;
            
            JsonContent content = JsonContent.Create(AddressTest, null , optionsjson);

            //appel de la route /address
            var result =  _client.PostAsync("/address", content).Result ; 
    
            //HttpStatusCode.Created = 201
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }


        [Fact]
        public void TestDeleteAddressUser()
        {            
            //utilisateur qui existe bien en base
            var userco = new UserDetails{Username = "MyUserName", Role = UserRole.ROLE_USER } ;
            
            //on recupere 
            var tokenclass = new JwtTokenUtil() ;
            var token = tokenclass.GenerateToken(userco) ;
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
                
            //en admettant que l'addresse a l'id n'est pas pour utilisateur MyUserName  
            var result =  _client.DeleteAsync("/address/1").Result ; 
    
            var StatusCodeTest = result.StatusCode ;
            
            //HttpStatusCode.Forbidden = 403
            Assert.Equal(HttpStatusCode.Forbidden, StatusCodeTest)  ; 

        }

        [Fact]
        public void TestDeleteAddressAdmin()
        {
            var userco = new UserDetails{Username = "MyUserNameAdmin", Role = UserRole.ROLE_ADMIN } ; 
            var tokenclass = new JwtTokenUtil() ;

            var token = tokenclass.GenerateToken(userco) ;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
                
            //en admettant qu'il y a des utilisateurs dans la table  et que "MyUserNameAdmin" est admin
            var result =  _client.DeleteAsync("/address/1").Result ; 
    
            var StatusCodeTest = result.StatusCode ; // montest.StatusCode ;
            
            //HttpStatusCode.OK = 200
            Assert.Equal(HttpStatusCode.OK, StatusCodeTest) ;
        }
        
    }
}
