import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { /*useNavigate,*/ Link } from "react-router-dom";
import ArticleService from "../services/ArticleService";


function ListeArticle(params) {
    //const navigate = useNavigate()

    const [articles, setArticle] = useState([])
    
    const page = params.params

    //barre de recherche
    //un tableau avec liste des films 
    //Titre, duree, realisateur, date de sortie

    //au chargement de la page on recupere la liste des films
    useEffect(() => {

        if(page === "film")
        ArticleService.getListeArticle()
            .then(res => {  console.log(res.data) ; setArticle(res.data["$values"].filter(f => f.typeArticle === "FILM")) })

        else
        ArticleService.getListeArticle()
            .then(res => setArticle(res.data["$values"].filter(f => f.typeArticle === "LIVRE")))

    }, [page])

    return (<>
        <br />
        <h1> Liste des {page}s </h1>
        <hr />
        <Table responsive="lg">
            <thead>
                <tr>
                    <th>Titre</th>
                    <th>Categorie</th>
                    <th>Duree</th>
                    <th>Date de sortie</th>
                    <th>Realisateur</th>
                </tr>
            </thead>
            <tbody>
                {articles.map(
                    article =>
                        <tr key={article.id}>
                            <td> <Link to={`/${page}/${article.id}`}>{article.titre}</Link>  </td> 
                            <td>{article.categorie}</td>
                            <td>{article.duree}</td>
                            <td>{article.dateSortie}</td>
                            <td>{article.realisateur.nomComplet}</td>

                        </tr>
                )}

            </tbody>
        </Table>
    </>)
}

export default ListeArticle