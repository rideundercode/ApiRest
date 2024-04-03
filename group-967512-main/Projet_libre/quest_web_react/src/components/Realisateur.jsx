import React, { useEffect, useState } from "react";
import { Col, Container, Row } from "react-bootstrap";
import { useParams } from "react-router-dom";
import RealisateurService from "../services/RealisateurService";

function Realisateur(props) {
    const params = useParams();

    const [realisateur, setRealisateur] = useState({ id: 0, nomcomplet: '', nationalite: '', photo: '', biographie: '', naissance: null, creation_date: null, updated_date: null })

    const id_realisateur = params.id

    useEffect(() => {
        RealisateurService.getRealisateurById(id_realisateur)
            .then((res) => {
                setRealisateur(res.data)
            })

    }, [])

    return (<>
        <Container>
            <Row>
                <Col sm={6}>

                    <div className="test-center">

                        <br />
                        <img src={realisateur.photo} alt={realisateur.nomcomplet} />
                    </div>
                </Col>

                <Col sm={6}>
                    <br />

                    <h3>{realisateur.nomcomplet}</h3>
                    <p>
                        Date de naissance : {realisateur.naissance} <br /><br />
                        Nationalité : {realisateur.nationalite} <br /><br />
                        Biographie : {realisateur.biographie} <br />

                    </p>

                </Col>
            </Row>

        </Container>






    </>)
}

export default Realisateur