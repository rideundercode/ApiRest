import React, { useState, useEffect } from 'react'
import UserService from '../services/UserService'
import SimpleDateTime  from 'react-simple-timestamp-to-date';
import { useLocation, useNavigate, useParams } from "react-router-dom";

function ListUserComponent(props) {
    const [Users, setUsers] = useState([]) ;

    const navigate = useNavigate() ;

    const deleteUser = (id) =>
    {
        UserService.deleteUser(id)
        .then( res => 
        {   
            if(res.status == 200)
                setUsers(Users.filter(User => User.id !== id)) ;
        });
    }

    const viewUser = (id) =>
    {    
        navigate(`/view-user/${id}`);
    }

    const editUser = (id) =>
    {
        navigate(`/add-user/${id}`);
    }

    useEffect(() => {

        UserService.getUser()
        .then( res => setUsers(res.data) );

    }, []) ;
    
    return (
            
            <div>
                 <br/>
                 <h2 className="text-center">Liste des utilisateurs</h2> <br/><br/>

                    <div className = "row">
                        <table className = "table table-striped table-bordered">

                            <thead>
                                <tr>
                                    <th> UserName</th>
                                    <th> Role </th>
                                    <th> Date de creation</th>
                                    <th> Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                   Users.map(
                                        User => 
                                        <tr key = {User.id}>
                                             <td> {User.username} </td>   
                                             <td> {User.role === "ROLE_USER" ? "Utilisateur" : "Administrateur"} </td>
                                             <td>  <SimpleDateTime dateFormat="DMY" dateSeparator="/"  timeSeparator=":">{User.creationDate}</SimpleDateTime>   </td>
                                             <td>
                                                 <button onClick={ () => editUser(User.id)} className="btn btn-info"> Modifier </button>
                                                 <button style={{marginLeft: "10px"}} onClick={ () => deleteUser(User.id)} className="btn btn-danger"> Supprimer </button>
                                                 <button style={{marginLeft: "10px"}} onClick={ () => viewUser(User.id)} className="btn btn-info"> Informations </button>
                                             </td>
                                        </tr>
                                    ) 
                                }
                            </tbody>
                        </table>

                 </div>

            </div>
        )
    }


export default ListUserComponent;

/*
const StudentTableRow = (props) => {
    const { _id, name, email, rollno } = props.obj;
    
    const deleteStudent = () => {
      axios
        .delete(
  "http://localhost:4000/students/delete-student/" + _id)
        .then((res) => {
          if (res.status === 200) {
            alert("Student successfully deleted");
            window.location.reload();
          } else Promise.reject();
        })
        .catch((err) => alert("Something went wrong"));
    };
    
    return (
      <tr>
        <td>{name}</td>
        <td>{email}</td>
        <td>{rollno}</td>
        <td>
          <Link className="edit-link" 
            to={"/edit-student/" + _id}>
            Edit
          </Link>
          <Button onClick={deleteStudent} 
            size="sm" variant="danger">
            Delete
          </Button>
        </td>
      </tr>
    );
  };
    */