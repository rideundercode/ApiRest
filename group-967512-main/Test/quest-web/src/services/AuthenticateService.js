import axios from 'axios';

class AuthenticateService {
    
    register(user){
        return axios.post('/register', user);
    }

    authenticate(user){
        return axios.post('/authenticate', user);
    }

    me(){
        return axios.get('/me');
    }
}

export default new AuthenticateService()

