import React from 'react';
//import logo from './logo.svg';
import './App.css';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom'
import ListUserComponent from './components/ListUserComponent';
import NavComponent from './components/NavComponent';
import CreateUserComponent from './components/CreateUserComponent';
import ViewUserComponent from './components/ViewUserComponent';

function App() {
  return (
    <div>
        <Router>
              <NavComponent />
                <div className="container">
                    <Routes> 
                          <Route path = "/" element = {<ListUserComponent/>}></Route>
                          <Route path = "/users" element = {<ListUserComponent/>}></Route>
                          <Route path = "/register" element = {<CreateUserComponent/>}></Route>
                          <Route path = "/view-user/:id" element = {<ViewUserComponent/>}></Route>
                          {/* <Route path = "/update-User/:id" element = {<UpdateUserComponent/>}></Route> */}
                    </Routes>
                </div>
        </Router>
    </div>
    
  );
}

export default App;
















/*import logo from './logo.svg';
import './App.css';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
*/