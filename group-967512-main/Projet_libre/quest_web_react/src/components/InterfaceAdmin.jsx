import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { /*useNavigate,*/ Link, useNavigate } from "react-router-dom";
import ArticleService from "../services/ArticleService";
import CommentsService from "../services/CommentsService";
import UserService from "../services/UserService";

function InterfaceAdmin() {

    const [articles, setArticles] = useState([])
    const [users, setUsers] = useState([])
    const [comments, setComments] = useState([])
    const [role,setRole] = useState('')

    const navigate = useNavigate()

    const supprimer = (e, id,type) =>
    {
        e.preventDefault()

        switch (type) {

            
            case 'article':
                ArticleService.deleteArticle(id).then( () => alert('Article supprime avec succes') )                    
            break;
            
            case 'comment':
                CommentsService.deleteComment(id).then( () => alert('Commentaire supprime avec succes') )
            break;
        
            case 'user':
                UserService.deleteUser(id).then( () => alert('Utilisateur supprime avec succes') ) 
                                
            break;
        
            default:
                alert('hihihiihihihi')
            break;
        }

        navigate(0)

    }
//    
    
    //barre de recherche
    //un tableau avec liste des films 
    //Titre, duree, realisateur, date de sortie

    //au chargement de la page on recupere la liste des films
    useEffect(() => {
        
        if(role === 'ROLE_USER' || localStorage.length <= 0)
            navigate('/')
        
        setRole(localStorage.getItem('role_user'))
        
        ArticleService.getListeArticle().then(res => { console.log(res.data) ; setArticles(res.data["$values"]) })

        UserService.getUser().then(res => setUsers(res.data["$values"]))

        CommentsService.getListComment().then(res => setComments(res.data["$values"]))
 

    }, [])

    return (<>
        <br />
        { role === 'ROLE_ADMIN' ? 
        <><h1> Liste des Utilisateurs </h1><hr /><Table responsive="lg">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>username</th>
                        <th>firstname</th>
                        <th>lastname</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(
                        user => <tr key={user.id}>
                            <td> {user.id}  </td>
                            <td>{user.username}</td>
                            <td>{user.firstName}</td>
                            <td>{user.lastName}</td>
                            <td> 
                                <Link to={`/edituser/${user.id}`} > Modifier </Link>  &nbsp;&nbsp;&nbsp;
                                <Link to={'/admin'} onClick={e => supprimer(e, user.id, 'user')}>Supprimer</Link> </td>

                        </tr>
                    )}

                </tbody>
            </Table></> 
        : <></>
        
        }

        <br /><hr />
        <br />
        {(role === 'ROLE_ADMIN' || role === 'ROLE_REDACTEUR') ? 
        <><h1> Liste des livres </h1> <Link to={`/add-article`} > Ajouter </Link> <hr />
        <Table responsive="lg">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>Titre</th>
                        <th>Categorie</th>
                        <th>Realisateur</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {articles.filter(f => f.typeArticle === 'LIVRE').map(
                        article => <tr key={article.id}>
                            <td> {article.id}  </td>
                            <td> <Link to={`/livre/${article.id}`}>{article.titre}</Link>  </td>
                            <td>{article.categorie}</td>
                            <td>{article.realisateur.nomComplet}</td>
                            <td> <Link to={`/editarticle/${article.id}`} > Modifier </Link>  &nbsp;&nbsp;&nbsp;
                                <Link to={'/admin'} onClick={e => supprimer(e, article.id, 'article')}>Supprimer</Link>  
                            </td>

                        </tr>
                    )}

                </tbody>
            </Table><br /><hr /><br /><h1> Liste des films </h1> <Link to={`/add-article`} > Ajouter </Link> <hr /><Table responsive="lg">
                    <thead>
                        <tr>
                            <th>id</th>
                            <th>Titre</th>
                            <th>Categorie</th>
                            <th>Realisateur</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {articles.filter(f => f.typeArticle === 'FILM').map(
                            article => <tr key={article.id}>
                                <td> {article.id}  </td>
                                <td> <Link to={`/film/${article.id}`}>{article.titre}</Link>  </td>
                                <td>{article.categorie}</td>
                                <td>{article.realisateur.nomComplet}</td>
                                <td> 
                                    <Link to={`/editarticle/${article.id}`} > Modifier </Link>  &nbsp;&nbsp;&nbsp;
                                <Link to={'/admin'} onClick={e => supprimer(e, article.id, 'article')}>Supprimer</Link>  
                                </td>
                            </tr>
                        )}

                    </tbody>
                </Table><br /><hr /></>
        : <></> }

        {(role === 'ROLE_ADMIN' || role === 'ROLE_MODO') ? 
        <><h1> Liste des Commentaires </h1><hr /><Table responsive="lg">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>username</th>
                        <th>titre</th>
                        <th>date creation</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {comments.map(
                        comment => <tr key={comment.id}>
                            <td> {comment.id}  </td>
                            <td>{comment.username}</td>
                            <td>{comment.firstName}</td>
                            <td>{comment.lastName}</td>
                            <td> <Link to={'/admin'} onClick={e => supprimer(e, comment.id, 'comment')}>Supprimer</Link>  </td>

                        </tr>
                    )}

                </tbody>
            </Table></> : <></> }
    </>
    
    )

}

export default InterfaceAdmin