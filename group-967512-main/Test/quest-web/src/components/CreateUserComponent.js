import React, { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom';
import AuthenticateService from '../services/AuthenticateService';

function CreateUserComponent(){

    const navigate = useNavigate() ;

    const [Users, setUsers] = useState([]) ;

        const [firstName, setFirstName] = useState('') 
        const [lastName, setLastName] = useState('') 
        const [email, setEmail] = useState('') 
        const [password, setPassword] = useState('') 
        const [Address, setAddress] = useState({}) 
        const [dob, setDob] = useState('') 
        //const [firstName, setFirstname] = useState('') 
        //const [firstName, setFirstname] = useState('') 

    const register = (e) => {
        e.preventDefault();
        
        
       
        let User = {firstName: firstName, lastName: lastName, email: email};
        
        console.log('User => ' + JSON.stringify(User));

        // step 5 
        AuthenticateService.register(User)
            .then(res => {

                if(res.status == 201)
                {}

                else
                {}

                navigate('/');
            
            });

    }
    
    const changeFirstNameHandler = (event) => {
        setFirstName(event.target.value);        
    }

    useEffect(() => console.log("firstname " + firstName), [firstName])

    const changeLastNameHandler = (event) => {
        setLastName(event.target.value);
    }

    const changeEmailHandler = (event) => {
        setEmail(event.target.value);
    }

    const cancel = () => {
        navigate('/');
    }

    const getTitle = () => {
        <h1> Connexion </h1>
    }


        return (
            <div>
                <br></br>
                   <div className = "container">
                        <div className = "row">
                            <div className = "card col-md-6 offset-md-3 offset-md-3">
                                {
                                    getTitle()
                                }
                                <div className = "card-body">
                                    <form>
                                        <div className = "form-group">
                                            <label> First Name: </label>
                                            <input placeholder="First Name" name="firstName" className="form-control" 
                                                value={firstName} onChange={changeFirstNameHandler}/>
                                        </div>
                                        <div className = "form-group">
                                            <label> Last Name: </label>
                                            <input placeholder="Last Name" name="lastName" className="form-control" 
                                                value={lastName} onChange={changeLastNameHandler}/>
                                        </div>
                                        <div className = "form-group">
                                            <label> Email Id: </label>
                                            <input placeholder="Email Address" name="emailId" className="form-control" 
                                                value={email} onChange={changeEmailHandler}/>
                                        </div>

                                        <button className="btn btn-success" onClick={register}>Creer mon compte</button>
                                        <button className="btn btn-danger" onClick={cancel} style={{marginLeft: "10px"}}>Annuler</button>
                                    </form>
                                </div>
                            </div>
                        </div>

                   </div>
            </div>
        )
}

export default CreateUserComponent

