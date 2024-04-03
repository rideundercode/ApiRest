import React, { Component } from 'react'

class NavComponent extends Component {
    constructor(props) {
        super(props)

        this.state = {
                 
        }
    }

    render() {
        return (
            <div>
                <header>
                    <nav className="navbar navbar-expand-md navbar-dark bg-dark">
                    <div><a href="/" className="navbar-brand">Home</a></div>
                    </nav>
                </header>
            </div>
        )
    }
}

export default NavComponent ;

