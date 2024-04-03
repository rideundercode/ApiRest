import React, { useEffect } from 'react'
import Login from './Login'
import Register from './Register'
import { Col, Container, Row } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

function Connect(){

    const navigate = useNavigate() 

    useEffect(() => {
        if(localStorage.length > 0)
            navigate('-1')
    }, []);

    //https://react-bootstrap.github.io/components/toasts/ connexion reussi
    return(    
        <>
        
        <br />
        <h1>Connection</h1> 
          <hr />
         
        <Container>
            <Row>

            <Col s={0} md={1} > </Col>
            <Col s={12} md={3} > <Login /> </Col>
            <Col className = "vl" s={0} md={1} >  </Col>
              
            <Col> <Register /> </Col>
            </Row>
        </Container>
        
        </>
        
    )
}

export default Connect