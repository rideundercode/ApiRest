import React from "react";
import { useNavigate } from "react-router-dom";
import AuthenticateService from "../services/AuthenticateService";
import { Form, Button, Col, Container, Row } from "react-bootstrap";

function Register() {
    const capitalizeFirstLetter = (string) => {
        return string[0].toUpperCase() + string.slice(1);
    }

    const navigate = useNavigate();

    const handleSubmit = (event) => {
        const formData = new FormData(event.currentTarget);
        event.preventDefault();

        let User = {};
        let Address = {};

        for (let [key, value] of formData.entries()) {
            //ici on fait la diff pour address et user
            if (["password", "username", "firstname", "lastname"].includes(key))
                User[capitalizeFirstLetter(key)] = value;
            else Address[capitalizeFirstLetter(key)] = value;
        }

        User['Address'] = Address ;

        AuthenticateService.register(User).then((res) => {
            if (res.status === 201) 
            {
                alert(
                    "Votre compte a ete cree avec succes !!!\n Vous pouvez vous connectez avec vos identifiants"
                );
                //on fait un clear sur les champs ici in case of
            } 
            
            else if (res.status === 409)
                alert("Utilisateur deja existant. \nChoisissez un autre pseudo !!!");
            
            else alert("Erreur ! Veillez reessayer plus tard :) ");
        });
    };

    const cancel = (e) => {
        e.preventDefault();
        navigate(-1);
    };

    
    return (
        <>
            <br />
        <h4>Creer un nouveau compte </h4> 
           <br />
           <br />
            
            <Container>
                <Form onSubmit={handleSubmit}>
                    <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridUsername">
                            <Form.Label>Username</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Entrer votre username"
                                name="username"
                            />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Mot de passe</Form.Label>
                            <Form.Control
                                type="password"
                                placeholder="Password"
                                name="password"
                            />
                        </Form.Group>
                    </Row>
                    <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridEmail">
                            <Form.Label>Prenom </Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Entrer votre prenom"
                                name="firstname"
                            />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridPassword">
                            <Form.Label>Nom </Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Entrer votre nom"
                                name="lastname"
                            />
                        </Form.Group>
                    </Row>
                    <Form.Group className="mb-3" controlId="formGridAddress">
                        <Form.Label>Rue </Form.Label>
                        <Form.Control placeholder="Entrer votre rue" name="street" />
                    </Form.Group>
                    <Row className="mb-3">
                        <Form.Group as={Col} controlId="formGridCity">
                            <Form.Label>Ville</Form.Label>
                            <Form.Control placeholder="Entrer votre ville" name="city" />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridCountry">
                            <Form.Label>Pays</Form.Label>
                            <Form.Control placeholder="Entrer votre pays" name="country" />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridZip">
                            <Form.Label>Code postal</Form.Label>
                            <Form.Control
                                placeholder="Entrer votre code postal"
                                name="postalcode"
                            />
                        </Form.Group>
                    </Row>
                    
                    <br /> 
                    <Button variant="primary" type="submit">
                        Creer mon compte
                    </Button>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <Button variant="primary" onClick={cancel}>
                        Annuler
                    </Button>
                </Form>
            </Container>
        </>
    );
}

export default Register;
