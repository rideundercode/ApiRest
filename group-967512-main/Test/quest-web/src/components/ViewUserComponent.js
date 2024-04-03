import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import UserService from '../services/UserService'

function ViewUserComponent() {

    const params = useParams() ;

    const [User, setUser] = useState({}) ;

    useEffect(() => {
        UserService.getUserById(params.id)
        .then( res => setUser(res.data))
    }, []) ;

    return (
            <div>
                <br></br>
                <div className = "card col-md-6 offset-md-3">
                    <h3 className = "text-center"> View User Details</h3>
                    <div className = "card-body">
                        <div className = "row">
                            <label> User First Name: </label>
                            <div> { User.username }</div>
                        </div>
                        <div className = "row">
                            <label> User Last Name: </label>
                            <div> { User.role }</div>
                        </div>
                        <div className = "row">
                            <label> User Email ID: </label>
                            <div> { User.creationDate }</div>
                        </div>
                    </div>

                </div>
            </div>
        )
    }


export default ViewUserComponent

