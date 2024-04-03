import axios from 'axios';

const config_me = () => { return  { validateStatus: function (status) {
    return status >= 200 && status < 500; // default
  },
  
  headers: {
    Authorization: (localStorage.length > 0) ? localStorage.getItem('token') : null 
  } } }

  const config = () => { return  { validateStatus: function (status) {
    return status >= 200 && status < 500; // default
  } } }
  
    
class AuthenticateService {
    
    register(user){
        return axios.post('/register', user,config());
    }

    //gerer le authenticate ici
    authenticate(user){
        return axios.post('/authenticate', user,config());
    }

    me(){
           
        return axios.get('/me', config_me());
    }

}

export default new AuthenticateService()