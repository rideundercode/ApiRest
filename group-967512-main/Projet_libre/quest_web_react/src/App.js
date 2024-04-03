import React from 'react';
import './App.css';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom'
import Navigation from './components/Navigation';
import Home from './components/Home';
import Connect from './components/Connect';
import ListeArticle from './components/ListeArticle';
import Article from './components/Article';
import Profil from './components/Profil';
import InterfaceAdmin from './components/InterfaceAdmin';
import EditUser from './components/EditUser';
import AddArticle from './components/AddArticle';
import Footer from './components/footer';
import EditArticle from './components/EditArticle';

function App() {
  return (
    
    <div className="App">
      
      <div>
        <Router>
          <Navigation />
            <div className="container">
              <Routes> 
                <Route path = "/" element = {<Home/>}></Route>
                <Route path = "/connect" element = {<Connect/>}></Route>
                <Route path = "/films" element = {<ListeArticle params = {"film"}/>}></Route>
                <Route path = "/livres" element = {<ListeArticle params = {"livre"}/>}></Route>
                <Route path = "/film/:id" element = {<Article/>}></Route>
                <Route path = "/livre/:id" element = {<Article/>}></Route>
                <Route path = "/profil" element = {<Profil/>}></Route>
                <Route path = "/admin" element = {<InterfaceAdmin/>}></Route>
                <Route path = "/edituser/:id" element = {<EditUser/>}></Route>
                <Route path = "/add-article" element = {<AddArticle/>}></Route>
                <Route path = "/editarticle/:id" element = {<EditArticle/>}></Route>
                          {/*<Route path = "/view-user/:id" element = {<ViewUserComponent/>}></Route>}
                          {/* <Route path = "/update-User/:id" element = {<UpdateUserComponent/>}></Route> */}
              </Routes>
            </div>
          {/*<Footer/>*/}
        </Router>
      </div>
    </div>
  );
}

export default App;
