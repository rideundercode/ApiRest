import React, { useEffect, useState } from "react";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import ArticleService from "../services/ArticleService";
import UserService from "../services/UserService";

function EditArticle() {
    const navigate = useNavigate();
    const params = useParams() ;

    //article
    const [id, setId] = useState(params.id)
    const [titre, setTitre] = useState('')
    const [duree, setDuree] = useState('')
    const [categorie, setCategorie] = useState('')
    const [synopsis, setSynopsis] = useState('')
    const [dateSortie, setDatesortie] = useState('')
    const [typeArticle, setTypeArticle] = useState('')
    
    //realisateur
    const [nomComplet, setNomComplet] = useState('')
    const [nationalite, setNationalite] = useState('')
    const [photo, setPhoto] = useState('')
    const [biographie, setBiographie] = useState('')
    const [naissance, setNaissance] = useState('')
    
    const [role, setRole] = useState('')

    const [modifying, setModifying] = useState(false)

    const handleSubmit = (e) => {
        e.preventDefault()
        const formData = new FormData(e.currentTarget);

        let Article = {};
        let Realisateur = {};

        for (let [key, value] of formData.entries()) {
            //ici on fait la diff pour address et user
            if (["nomComplet", "nationalite", "photo", "biographie", "naissance"].includes(key))
                Realisateur[key] = value;
            else Article[key] = value;
        }

        Article.Realisateur = Realisateur

        if (modifying) {
            ArticleService.updateArticle(Article,id)
                .then((res) => {

                    if (res.status === 200)
                    {    alert('Modification avec succes')
                        navigate('/admin')
                    }

                    else
                        alert('Echec lors de la modification')

                })


        }

        setModifying(!modifying);
    }

    const cancel = () => {
        window.location.reload(false)
    }



    useEffect(() => {


        if (localStorage.length > 0 && localStorage.getItem('role') !== 'ROLE_ADMIN'  ) {
            
            ArticleService.getArticleById(id).then(res => {
                if (res.status === 200) {
                    const result = res.data

                    console.log(result)
                    
                    //article
                    setTitre(result.titre) 
                    setDuree(result.duree) 
                    setCategorie(result.categorie) 
                    setSynopsis(result.synopsis) 
                    setDatesortie(result.dateSortie) 
                    setTypeArticle(result.typeArticle) 
                    
                    //realisateur
                    setNomComplet(result.realisateur.nomComplet) 
                    setNationalite(result.realisateur.nationalite) 
                    setPhoto (result.realisateur.photo)
                    setBiographie(result.realisateur.biographie) 
                    setNaissance(result.realisateur.naissance) 
                }


                else {
                    alert('Erreur !')
                    navigate(-1)
                }

            })
        }

        else
            navigate(-1)
    }, [])



    return (<>
        <br />
        <h1>{titre}</h1>
        <hr />

        <Container>
            <Row>

                <Col>


                    <Container>
                        <Form onSubmit={handleSubmit}>
                            <fieldset disabled={!modifying}>
                            <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridUsername">
                            <Form.Label>Titre</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Entrer le titre"
                                name="titre"
                                defaultValue={titre}
                            />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Categorie</Form.Label>
                            <Form.Control
                                type="type"
                                placeholder="Entrer la categorie"
                                name="categorie"
                                defaultValue={categorie}
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
                                defaultValue={duree}
                            />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Type </Form.Label>
                    
                            <Form.Select name="typeArticle" aria-label="Default select example" >
                            <option value="LIVRE">Livre</option>
                            <option value="FILM">Film</option>
                            </Form.Select>
                            
                        </Form.Group>  
                        
                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Date de sortie </Form.Label>
                            <Form.Control
                                type="date"
                                name="dateSortie"
                                defaultValue= {dateSortie}
                            />
                        </Form.Group>
                    </Row>



                    <Form.Group className="mb-3" controlId="formGridAddress">
                        <Form.Label>Resume</Form.Label>
                        <Form.Control as="textarea" rows={3} placeholder="Resume..." name="synopsis" defaultValue={synopsis}  />
                    </Form.Group>

                    <br /> <hr />
                    <h3> Auteur ou Realisateur </h3>
        <br />



        <Row className="mb-3">
                       
                        <Form.Group as={Col} sm={8} controlId="formGridCountry">
                            <Form.Label>Nom complet</Form.Label>
                            <Form.Control placeholder="Entrer le nom complet" name="nomComplet" defaultValue={nomComplet} />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridZip">
                            <Form.Label>Nationalite</Form.Label>
                            <Form.Control
                                placeholder="Entrer la nationalite"
                                name="nationalite"
                                defaultValue={nationalite}
                            />
                        </Form.Group>
                    </Row>

                    <Form.Group className="mb-3" controlId="formGridAddress">
                        <Form.Label>Biographie</Form.Label>
                        <Form.Control as="textarea" rows={3} placeholder="Biographie..." name="biographie" defaultValue={biographie} />
                    </Form.Group>


                    <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridCity">
                            <Form.Label>Photo</Form.Label>
                            <Form.Control placeholder="Chemin de l'image" name="photo" defaultValue={photo} />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridCountry">
                            <Form.Label>Date de naissance</Form.Label>
                            <Form.Control type="date" placeholder="Entrer la date de naissance" name="naissance" defaultValue={naissance} />
                        </Form.Group>
                    </Row>
                    
                    <br /> <br />
                    </fieldset>
                    <Button variant="primary" type="submit">
                        Modifier l'article
                    </Button>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <Button variant="primary" onClick={cancel}>
                        Annuler
                    </Button>
                               
                             
                                 

                                <br />


                        </Form>
                    </Container>

                </Col>

            </Row>
        </Container>

    </>)
}

export default EditArticle
