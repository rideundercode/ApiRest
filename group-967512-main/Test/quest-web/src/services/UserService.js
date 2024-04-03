import axios from 'axios';

const USER_API_BASE_URL = "/user";

class UserService {

    getUser(){
        return axios.get(USER_API_BASE_URL);
    }

    getUserById(UserId){
        return axios.get(USER_API_BASE_URL + '/' + UserId);
    }

    updateUser(User, UserId){
        return axios.put(USER_API_BASE_URL + '/' + UserId, User);
    }

    deleteUser(UserId){
        return axios.delete(USER_API_BASE_URL + '/' + UserId);
    }
}

export default new UserService()

