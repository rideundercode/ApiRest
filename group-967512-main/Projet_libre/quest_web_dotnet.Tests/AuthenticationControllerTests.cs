using System;
using Xunit;
using quest_web ;
using quest_web.Models ;
using Microsoft.EntityFrameworkCore;
using System.Net.Http ;
using System.Net ;
using System.Net.Http.Headers ;
using quest_web.Utils ;

using Microsoft.AspNetCore.Mvc ;
using System.Net.Http.Json ;
using System.Text.Json.Serialization ;

namespace quest_web_dotnet.Tests
{
    public class AuthenticationTest
    {      
        private static Uri localhost = new Uri("http://localhost:8090") ; 
        private HttpClient _client = new HttpClient { BaseAddress = localhost } ;

        //
        //la route /register return 201 si l'utilisateur n'est pas deja dans la base, 409 sinon        
        [Fact]
        public void TestRegister()
        {            
            User UserTest = new User{ Username = "MyUserName", Password = "MyPass" } ;
            
            JsonContent content = JsonContent.Create(UserTest);

            //appel de la route /register
            var result =  _client.PostAsync("/register", content).Result ; 
    
            //HttpStatusCode.Created = 201
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            
            //on appelle une 2e fois avec les meme parametres
            result =  _client.PostAsync("/register", content).Result ; 
            
            //HttpStatusCode.Conflict = 409
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode) ;
        
        }

        //la route /authenticate return 200 + token avec username et mot de passe correct
        [Fact]
        public void TestAuthenticate()
        {
            User UserTest = new User{ Username = "MyUserName123", Password = "MyPass" } ;
            
            JsonContent content = JsonContent.Create(UserTest);

            //appel de la route /register
            var result =  _client.PostAsync("/authenticate", content).Result ; 
    
            //HttpStatusCode.OK = 200
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            //Si on a un token, donc le contenu du resultat est non nul  
            Assert.NotNull(result.Content.ReadAsStringAsync().Result) ;
        }

        //la route /me return 200 avec le bon token 
        [Fact]
        public void TestMe()
        {
            User UserTest = new User{ Username = "MyUserName", Password = "Mypass", Role = UserRole.ROLE_USER } ;

            var UserCo = UserTest.GetUserDetails() ; new UserDetails{ Username = UserTest.Username , Role = UserTest.Role } ; 

            var tokenclass = new JwtTokenUtil() ;
            var token = tokenclass.GenerateToken(UserCo) ;
            
            //on passe le token pour la requete
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token) ;
            
            //appel de la route /me
            var result =  _client.GetAsync("/me").Result ; 
    
            var StatusCode = result.StatusCode ;

            //HttpStatusCode.OK = 200
            Assert.Equal(HttpStatusCode.OK, StatusCode) ;
        }
    }
}
