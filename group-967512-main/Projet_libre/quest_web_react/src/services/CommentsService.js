import axios from 'axios';

const config = { validateStatus: function (status) {
    return status >= 200 && status < 500; // default
  },
  
  headers: {
    'Authorization': (localStorage.length > 0) ? localStorage.getItem('token') : null 
  } }

  const USER_API_BASE_URL = "/comment"

class CommentsService {
        
    getListComment(user){
        return axios.get(USER_API_BASE_URL, user,config);
    }

    getListCommentArticle(ArticleId){
        return axios.get(USER_API_BASE_URL +'/' + ArticleId,config);
    }

    createComment(Comment){
        return axios.post(USER_API_BASE_URL, Comment,config);
    }

    modifyComment(CommentId){
        return axios.put(USER_API_BASE_URL +'/' + CommentId,config);
    }
    
    deleteComment(CommentId){
        return axios.delete(USER_API_BASE_URL + '/' + CommentId,config);
    }
}

export default new CommentsService()