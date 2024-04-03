import axios from 'axios';

const config = () => { return  { validateStatus: function (status) {
    return status >= 200 && status < 500; // default
  },
  
  headers: {
    'Authorization': (localStorage.length > 0) ? localStorage.getItem('token') : null 
  }

} }


const USER_API_BASE_URL = "/commande"

class CommandeService {
    
    get_commandes_user(user){
        return axios.get(USER_API_BASE_URL,config());
    }

    already_commander(ArticleId){
        return axios.get('/test-commande/' + ArticleId,config());
    }

    createCommande(Commande){
        return axios.post(USER_API_BASE_URL, Commande,config());
    }

    deleteCommande(id){
        return axios.delete(USER_API_BASE_URL +'/' + id,config());
    }

}

export default new CommandeService()
