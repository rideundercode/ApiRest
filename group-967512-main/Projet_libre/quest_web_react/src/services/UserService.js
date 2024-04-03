import axios from 'axios';

const config = () => { return  { validateStatus: function (status) {
    return status >= 200 && status < 500; // default
  },
  
  headers: {
    'Authorization': (localStorage.length > 0) ? localStorage.getItem('token') : null 
  } } }
  
const USER_API_BASE_URL = "/user";

class UserService {

    getUser(){
        return axios.get(USER_API_BASE_URL,config());
    }

    getUserById(UserId){
        return axios.get(USER_API_BASE_URL + '/' + UserId,config());
    }
    
    getUserByUsername(Username){
        return axios.get(USER_API_BASE_URL + '/name/' + Username,config());
    }

    updateUser(User, UserId){
        return axios.put(USER_API_BASE_URL + '/' + UserId, User,config());
    }

    deleteUser(UserId){
        return axios.delete(USER_API_BASE_URL + '/' + UserId,config());
    }
}

export default new UserService()

