import React, { Component } from 'react'
import UserService from '../services/UserService'

class ListUserComponent extends Component {
    constructor(props) {
        super(props)

        this.state = {
                Users: []
        }
        
        this.addUser = this.addUser.bind(this); 
        this.editUser = this.editUser.bind(this);
        this.deleteUser = this.deleteUser.bind(this);
    }

    deleteUser(id) 
    {
        UserService.deleteUser(id).then( res => {
            this.setState({Users: this.state.Users.filter(User => User.id !== id)});
        });
    }

    viewUser(id)
    {
        this.props.history.push(`/view-user/${id}`);
    }

    editUser(id)
    {
        this.props.history.push(`/add-user/${id}`);
    }

    componentDidMount()
    {
        UserService.getUser().then((res) => 
        {
            this.setState({ Users: res.data});
        });
    }

    addUser()
    {
        this.props.history.push('/add-user/_add');
    }

    render() {
        return (
            <div>
                 <h2 className="text-center">Users List</h2>
                 <div className = "row">
                    <button className="btn btn-primary" onClick={this.addUser}> Add User</button>
                 </div>
                 <br></br>
                 <div className = "row">
                        <table className = "table table-striped table-bordered">

                            <thead>
                                <tr>
                                    <th> UserName</th>
                                    <th> Role de l'utilisateur</th>
                                    <th> Date de creation</th>
                                    <th> Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                   this.state.Users.map(
                                        User => 
                                        <tr key = {User.id}>
                                             <td> {User.username} </td>   
                                             <td> {User.role} </td>
                                             <td> {User.creationDate}</td>
                                             <td>
                                                 <button onClick={ () => this.editUser(User.id)} className="btn btn-info">Update </button>
                                                 <button style={{marginLeft: "10px"}} onClick={ () => this.deleteUser(User.id)} className="btn btn-danger">Delete </button>
                                                 <button style={{marginLeft: "10px"}} onClick={ () => this.viewUser(User.id)} className="btn btn-info">View </button>
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
}

export default ListUserComponent ;

