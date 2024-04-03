import React, { useEffect, useState } from "react";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import UserService from "../services/UserService";

function EditUser() {
    const navigate = useNavigate();
    const params = useParams() ;

    const [id, setId] = useState(params.id)
    const [username, setUsername] = useState('')
    const [password, setpassword] = useState('')
    const [firstname, setFirstname] = useState('')
    const [lastname, setLastname] = useState('')
    const [street, setStreet] = useState('')
    const [city, setCity] = useState('')
    const [country, setCountry] = useState('')
    const [postalcode, setPostalcode] = useState('')
    const [role, setRole] = useState('')

    const [modifying, setModifying] = useState(false)

    const handleSubmit = (e) => {
        e.preventDefault()
        const formData = new FormData(e.currentTarget);

        let User = {};
        let Address = {};

        for (let [key, value] of formData.entries()) {
            //ici on fait la diff pour address et user
            if (["password", "username", "firstName", "lastName", "role"].includes(key))
                User[key] = value;
            else Address[key] = value;
        }

        User.address = Address

        if (modifying) {
            UserService.updateUser(User, id)
                .then((res) => {

                    if (res.status === 200)
                        alert('Modification avec succes')

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
            
            UserService.getUserById(id).then(res => {
                if (res.status === 200) {
                    const result = res.data

                    console.log(result)
                    setUsername(result.username)
                    setpassword(result.password)
                    setFirstname(result.firstName)
                    setLastname(result.lastName)
                    setStreet(result.address.street)
                    setCity(result.address.city)
                    setCountry(result.address.country)
                    setPostalcode(result.address.postalCode)
                    setRole(result.role)
                }


                else {
                    alert('Erreur !')
                    //navigate(-1)
                }

            })
        }

        else
            navigate(-1)
    }, [])



    return (<>
        <br />
        <h1>Profil de : {username}</h1>
        <hr />

        <Container>
            <Row>

                <Col>


                    <Container>
                        <Form onSubmit={handleSubmit}>
                            <fieldset disabled={!modifying}>

                                <Row className="mb-3">
                                    <Form.Group as={Col} controlId="formGridUsername">
                                        <Form.Label>Username</Form.Label>
                                        <Form.Control
                                            type="text"
                                            placeholder="Entrer votre username"
                                            name="username"
                                            defaultValue={username}
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
                                    
                                    <Form.Group as={Col} controlId="formGridPassword">
                                        <Form.Label>Role</Form.Label>
                
                                        <Form.Select name="role" aria-label="Default select example" defaultValue={{label : role , value : role}}>
                                            <option>Choisir le role</option>
                                            <option value="ROLE_ADMIN">Administrateur</option>
                                            <option value="ROLE_USER">Utilisateur</option>
                                            <option value="ROLE_MODO">Moderateur</option>
                                            <option value="ROLE_REDACTEUR">Redacteur</option>
                                            </Form.Select>


                                    </Form.Group>
                                </Row>
                                <Row className="mb-3">
                                    <Form.Group as={Col} controlId="formGridEmail">
                                        <Form.Label>Prenom </Form.Label>
                                        <Form.Control
                                            type="text"
                                            placeholder="Entrer votre prenom"
                                            name="firstName"
                                            defaultValue={firstname}
                                        />
                                    </Form.Group>

                                    <Form.Group as={Col} controlId="formGridPassword">
                                        <Form.Label>Nom </Form.Label>
                                        <Form.Control
                                            type="text"
                                            placeholder="Entrer votre nom"
                                            name="lastName"
                                            defaultValue={lastname}
                                        />
                                    </Form.Group>
                                </Row>
                                <Form.Group className="mb-3" controlId="formGridAddress">
                                    <Form.Label>Rue </Form.Label>
                                    <Form.Control placeholder="Entrer votre rue" name="street" defaultValue={street} />
                                </Form.Group>
                                <Row className="mb-3">
                                    <Form.Group as={Col} controlId="formGridCity">
                                        <Form.Label>Ville</Form.Label>
                                        <Form.Control placeholder="Entrer votre ville" name="city" defaultValue={city} />
                                    </Form.Group>

                                    <Form.Group as={Col} controlId="formGridCountry">
                                        <Form.Label>Pays</Form.Label>
                                        <Form.Control placeholder="Entrer votre pays" name="country" defaultValue={country} />
                                    </Form.Group>

                                    <Form.Group as={Col} controlId="formGridZip">
                                        <Form.Label>Code postal</Form.Label>
                                        <Form.Control
                                            placeholder="Entrer votre code postal"
                                            name="postalCode"
                                            defaultValue={postalcode}
                                        />
                                    </Form.Group>
                                </Row>

                                <br />

                            </fieldset>

                            <Button variant="primary" type="submit">
                                {modifying ? 'Confirmer les changements' : 'Modifier mes données'}
                            </Button>

                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <Button variant="primary" onClick={cancel}>
                                Annuler
                            </Button>


                        </Form>
                    </Container>

                </Col>

            </Row>
        </Container>

    </>)
}

export default EditUser
