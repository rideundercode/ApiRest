//nav bar

// [ [home]                     [liste de film][se connecter/profil[ Mon profil | Administration | Mes commandes | Se deconnecter ] ]]
import React, { useEffect, useState } from 'react'
import { Navbar, Container, Nav, NavDropdown, NavItem, Offcanvas, Button, Card } from 'react-bootstrap'
import { Link, Navigate, useNavigate } from 'react-router-dom'
import CommandeService from '../services/CommandeService';

function Admin(){
  const [role, setRole] = useState('VISITEUR')

  useEffect(()=>{
    if(localStorage.length > 0)
    setRole(localStorage.getItem('role_user'))
  },[])

  if (role !== 'VISITEUR' && role !== 'ROLE_USER')
    return (
      <><NavDropdown.Divider />
        <NavDropdown.Item as={Link} to="/admin">Administration</NavDropdown.Item>
      </>
    )
}

function Navigation() {
  const navigate = useNavigate()

  const [show, setShow] = useState(false);

  const [is_co, setCo] = useState(false)
  const [commandes, setCommandes] = useState([])

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const se_deco = () => {
    
    /*localStorage.removeItem('token');
    localStorage.removeItem('role_user');
    localStorage.removeItem('username');
    localStorage.removeItem('user_id') */
    
    localStorage.clear()

    navigate("/");
    window.location.reload(false)
  }

  useEffect(() => {
    
    if (localStorage.length > 0) 
    {
      if (localStorage.getItem('token'))
        setCo(true);
    
      const user_id = localStorage.getItem('user_id') 
      /*
      CommandeService.get_commandes_user(user_id)
        .then( res => { 

          console.log(res)
          if(res.status === 200 ) 
           setCommandes(res.data)
        } ) */
    
    }


  }, [])

 

  return (
    <><Navbar sticky="top" collapseOnSelect expand="lg" bg="dark" variant="dark">
      <Container>
        <Navbar.Brand href="/">Accueil</Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav" className="justify-content-end">

          <Nav>
            <Nav.Link as={Link} to="/films" eventKey={1}>Liste des films</Nav.Link>
            <Nav.Link as={Link} to="/livres" eventKey={2}>Liste des livres</Nav.Link>

            {(is_co) ?

              <NavDropdown title="Mon compte " id="collasible-nav-dropdown" > {/* Ajouter un icone et supprimer la fleche */}
                <NavDropdown.Item as={Link} to= '/profil' >Mon profil</NavDropdown.Item>
                <NavDropdown.Item onClick={handleShow} >Mes commandes</NavDropdown.Item>
                <Admin/>
                <NavDropdown.Divider />
                <NavDropdown.Item onClick={se_deco}>Se deconnecter </NavDropdown.Item>
              </NavDropdown>
              :
              <Nav.Link as={Link} to="/connect" eventKey={3}> Se connecter </Nav.Link>

            }

          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>


      {/* Lister les commandes */}
      <Offcanvas show={show} onHide={handleClose} placement="end" >
        <Offcanvas.Header closeButton>
          <Offcanvas.Title >Liste des commandes</Offcanvas.Title>
        </Offcanvas.Header>
        <Offcanvas.Body>


          {commandes.map( c => 
          
            <><Card className="text-center">
              <Card.Header>Featured</Card.Header>
              <Card.Body>
                <Card.Title>Commande #{c.id}</Card.Title>
                <Card.Text>
                 {c.titre}
                </Card.Text>
                <Button variant="primary">Supprimer cette commande</Button>
              </Card.Body>
              <Card.Footer className="text-muted">Date de la commande :{c.creation_date}</Card.Footer>
            </Card><br /></> )
          }

        </Offcanvas.Body>
      </Offcanvas></>
  )

}


export default Navigation;

