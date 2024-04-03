import React, { useEffect, useState } from "react";
import { Form, Button } from 'react-bootstrap'
import { useNavigate } from "react-router-dom";
import AuthenticateService from "../services/AuthenticateService";
import UserService from "../services/UserService";

function Login() {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')

  const navigate = useNavigate()

  const changeusernameHandler = (event) => {
    setUsername(event.target.value);
  }

  const changepasswordHandler = (event) => {
    setPassword(event.target.value);
  }

  const clic_connecter = (e) => {
    e.preventDefault()
    var currentuser = { username: username, password: password }

    AuthenticateService.authenticate(currentuser)
      .then(res => {

        if (res.status === 200) {

          let token = res.data.token

          //on stocke le token ici
          localStorage.setItem("token", `Bearer ${token}`)

          AuthenticateService.me().then(res => 
          {

            if(res.status === 200)
            {
              const username = res.data.username ;
              const role = res.data.role ;

              localStorage.setItem('role_user', role);
              localStorage.setItem('username', username);

              UserService.getUserByUsername(username).then(res => 
                {
                   
                  if(res.status === 200 )
                    localStorage.setItem('user_id', res.data.id)
                  
                    navigate("/")
                    window.location.reload(false)
                })
            }
            

          })

        
        }

        else
          alert("Username ou mot de passe incorrect !!!")
      }

      )

  }

  useEffect(() => { }, [])

  return (
    <><br /><h4>Vous avez deja un compte ?</h4><br /><Form>
      <Form.Group className="mb-3" controlId="formBasicEmail">
        <Form.Label>Pseudo</Form.Label>
        <Form.Control placeholder="Entrer votre pseudo" name="username" onChange={changeusernameHandler} />
      </Form.Group>

      <br />

      <Form.Group className="mb-3" controlId="formBasicPassword">
        <Form.Label>Mot de passe </Form.Label>
        <Form.Control type="password" placeholder="Entrer votre mot de passe" name="password" onChange={changepasswordHandler} />
      </Form.Group>

      <br />

      <Button onClick={clic_connecter} variant="primary" type="submit">
        Se connecter
      </Button>
    </Form></>

  )

}

export default Login