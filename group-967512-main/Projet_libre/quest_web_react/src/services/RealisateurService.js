import axios from 'axios';

const config = { validateStatus: function (status) {
    return status >= 200 && status < 500; // default
  },

  headers: {
    'Authorization': (localStorage.length > 0) ? localStorage.getItem('token') : null 
  }

  


}
  
const USER_API_BASE_URL = "/realisateur";

class RealisateurService {

    getListeRealisateur(){
        return axios.get(USER_API_BASE_URL,config);
    }

    getRealisateurById(RealisateurId){
        return axios.get(USER_API_BASE_URL + '/' + RealisateurId,config);
    }

    createRealisateur(Realisateur){
        return axios.post(USER_API_BASE_URL, Realisateur,config);
    }

    updateRealisateur(Realisateur, RealisateurId){
        return axios.put(USER_API_BASE_URL + '/' + RealisateurId, Realisateur,config);
    }

    deleteRealisateur(RealisateurId){
        return axios.delete(USER_API_BASE_URL + '/' + RealisateurId,config);
    }
}

export default new RealisateurService()

