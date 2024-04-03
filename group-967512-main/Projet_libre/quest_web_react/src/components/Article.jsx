import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import ArticleService from "../services/ArticleService";
import { Accordion, Button, Col, Container, Row } from 'react-bootstrap';
import CommentsService from "../services/CommentsService";
import CommandeService from "../services/CommandeService";

/*
commentaire :

https://react-bootstrap.github.io/components/accordion/
*/
function Article() {
  const params = useParams();
  const id = params.id
  const [article, setArticle] = useState({})
  const [rea, setRea] = useState({})
  //const [a_c, setA_C] = useState(false)
  const [comments, setComments] = useState([])

  const commander = () => {
    let commande = {}
    CommandeService.createCommande(commande).then(() => alert('Commander avec succes !'))
  }

  useEffect(() => {
    //recuperer ici les donnees
    ArticleService.getArticleById(id)
      .then(res => {
        //console.log(res.data)

        if (res.status === 200)
        {  
          setArticle(res.data)
          setRea(res.data.realisateur)
        }
      })
  }, [])

  /*
  useEffect(() => {
      //recuperer ici les donnees
      CommentsService.getListCommentArticle(article) 
          .then( res => {
              if(res.status === 200)
                  setComments(res.data)
          })
  },[]) */

  return (<>

    <Container>
      <Row>

        <Col>
        <br/><br/>

          <h3>{article.titre}</h3>

<br />
          <p>Categorie : {article.categorie} </p>

          <p> Date de sortie  : {article.dateSortie} </p>
          <p> {article.type === 'FILM' ? 'Durée ' : 'Nombre de pages '} {article.duree} {article.type === 'FILM' ? 'minutes' : 'pages'} </p>

          <p>De : {''} </p>

          <p> {article.type === 'FILM' ? 'Synopsis' : 'Résume'} : {article.synopsis} </p>

          <br />
          {localStorage.length > 0 ?
            <><Button variant="primary" onClick={commander}>
              Commander
            </Button><br /><br /></>

            : <></>}
        </Col>


      </Row>

    </Container>

    <Container>

      <h3>Listes des commentaires</h3>
      <Link to={'#'} > Ajouter commentaire </Link>

      {/* on fait ici la liste des comment et on map dessus 
    <Accordion defaultActiveKey={['0']} alwaysOpen>
  <Accordion.Item eventKey="0">
    <Accordion.Header>Accordion Item #1</Accordion.Header>
    <Accordion.Body>
      Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod
      tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
      veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea
      commodo consequat. Duis aute irure dolor in reprehenderit in voluptate
      velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
      cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id
      est laborum.
    </Accordion.Body>
  </Accordion.Item>
  <Accordion.Item eventKey="1">
    <Accordion.Header>Accordion Item #2</Accordion.Header>
    <Accordion.Body>
      Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod
      tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim
      veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea
      commodo consequat. Duis aute irure dolor in reprehenderit in voluptate
      velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
      cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id
      est laborum.
    </Accordion.Body>
  </Accordion.Item>
</Accordion>*/}

    </Container>
  </>

  )

}


export default Article