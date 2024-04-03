import axios from 'axios';

//https://stackoverflow.com/questions/43842711/can-i-throw-error-in-axios-post-based-on-response-status

const USER_API_BASE_URL = "/article";

const config = { validateStatus: function (status) {
    return status >= 200 && status < 500; // default
  },
  
  headers: {
    'Authorization': (localStorage.length > 0) ? localStorage.getItem('token') : null 
  } }

class ArticleService {

    getListeArticle(){
        return axios.get(USER_API_BASE_URL,config);
    }

    getArticleById(ArticleId){
        return axios.get(USER_API_BASE_URL + '/' + ArticleId,config);
    }

    createArticle(Article){
        return axios.post(USER_API_BASE_URL, Article,config);
    }

    updateArticle(Article, ArticleId){
        return axios.put(USER_API_BASE_URL + '/' + ArticleId, Article, config);
    }

    deleteArticle(ArticleId){
        return axios.delete(USER_API_BASE_URL + '/' + ArticleId,config);
    }
}

export default new ArticleService()

