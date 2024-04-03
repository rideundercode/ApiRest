import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Form, Button, Col, Container, Row } from "react-bootstrap";
import ArticleService from "../services/ArticleService";


function AddArticle() {
  
    const navigate = useNavigate();

    const handleSubmit = (event) => {
        const formData = new FormData(event.currentTarget);
        event.preventDefault();

        let Article = {};
        let Realisateur = {};

        for (let [key, value] of formData.entries()) {
            //ici on fait la diff pour address et user
            if (["titre", "duree", "categorie", "type", "datesortie", "synopsis"].includes(key))
                Article[key] = value;
            else Realisateur[key] = value;
        }

        Article['Realisateur'] = Realisateur ;

        ArticleService.createArticle(Article).then((res) => {
            if (res.status === 201) 
            {
                alert("L'article a ete cree avec succes !!!\n")
                navigate('/admin')
            } 
            
            else if (res.status === 409)
                alert("Article deja existant. \n")
            
            else alert("Erreur ! Veillez reessayer plus tard :) ");
        });
    };

    const cancel = (e) => {
        e.preventDefault();
        navigate(-1) ;
    };

    useEffect(()=>{
        if(localStorage.length <= 0 || (localStorage.getItem('role_user') !== 'ROLE_ADMIN' && localStorage.getItem('role_user') !== 'ROLE_REDACTEUR'))
            navigate(-1) ;
    },[])
    
    return (
        <>
            <br />
        <h1>Creer un nouveau article </h1> 
           
           <br /> <hr />
                    <h3> Article </h3>
        <br />

            <Container>
                <Form onSubmit={handleSubmit}>
                    <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridUsername">
                            <Form.Label>Titre</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Entrer le titre"
                                name="titre"
                            />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Categorie</Form.Label>
                            <Form.Control
                                type="type"
                                placeholder="Entrer la categorie"
                                name="categorie"
                            />
                        </Form.Group>
                    </Row>
                    <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridEmail">
                            <Form.Label>Duree ou nombre de pages </Form.Label>
                            <Form.Control
                                type="number"
                                placeholder="Entrer la duree"
                                name="duree"
                            />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Type </Form.Label>
                    
                            <Form.Select name="type" aria-label="Default select example" >
                            <option value="LIVRE">Livre</option>
                            <option value="FILM">Film</option>
                            </Form.Select>
                            
                        </Form.Group>  
                        
                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Date de sortie </Form.Label>
                            <Form.Control
                                type="date"
                                name="datesortie"
                            />
                        </Form.Group>
                    </Row>



                    <Form.Group className="mb-3" controlId="formGridAddress">
                        <Form.Label>Resume</Form.Label>
                        <Form.Control as="textarea" rows={3} placeholder="Resume..." name="synopsis" />
                    </Form.Group>

                    <br /> <hr />
                    <h3> Auteur ou Realisateur </h3>
        <br />



        <Row className="mb-3">
                       
                        <Form.Group as={Col} sm={8} controlId="formGridCountry">
                            <Form.Label>Nom complet</Form.Label>
                            <Form.Control placeholder="Entrer le nom complet" name="nomcomplet" />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridZip">
                            <Form.Label>Nationalite</Form.Label>
                            <Form.Control
                                placeholder="Entrer la nationalite"
                                name="nationalite"
                            />
                        </Form.Group>
                    </Row>

                    <Form.Group className="mb-3" controlId="formGridAddress">
                        <Form.Label>Biographie</Form.Label>
                        <Form.Control as="textarea" rows={3} placeholder="Biographie..." name="biographie" />
                    </Form.Group>


                    <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridCity">
                            <Form.Label>Photo</Form.Label>
                            <Form.Control placeholder="Chemin de l'image" name="photo" />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridCountry">
                            <Form.Label>Date de naissance</Form.Label>
                            <Form.Control type="date" placeholder="Entrer la date de naissance" name="naissance" />
                        </Form.Group>
                    </Row>
                    
                    <br /> <br />
                    <Button variant="primary" type="submit">
                        Creer l'article
                    </Button>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <Button variant="primary" onClick={cancel}>
                        Annuler
                    </Button>
                </Form>
            </Container>

            <br /> <br />
        </>
    );
}

export default AddArticle;
