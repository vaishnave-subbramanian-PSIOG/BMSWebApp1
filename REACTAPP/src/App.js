import React, { Component } from 'react';
import { Route,Switch,Redirect } from 'react-router-dom';
// import MovieComponent from './Movies/MovieComponent';
import logo from './logo.svg';
import Modal from '@material-ui/core/Modal';
import {ProtectedRoute} from './ProtectedRoute';
import './App.css';
//Imports
import SignIn from './scenes/Onboarding/SignIn/index';
import ForgotPassword from './scenes/Onboarding/ForgotPassword/index';
import ResetPassword from './scenes/Onboarding/ResetPassword/index';
import SignUp from './scenes/Onboarding/SignUp/index';
import Home from './scenes/Home/index';
import MovieMain from './scenes/Movies/index';
import MovieHallMain from './scenes/MovieHall/index';
import EmailVerified from './scenes/Onboarding/EmailVerified/index'
import BookingHistory from './scenes/BookingHistory/index';
import Account from './scenes/Account/index';
import NotFound from './components/NotFound/index'


class App extends Component {
  constructor(props){
    super(props);
  }

  render() {
    return (
      
      <div>
        <Switch>
     {/* <div style={{width:"100%",height:"100%",backgroundColor:"#f2f2f2"}}><Movies/></div> */}
     {/* <div style={{width:"100%",height:"100%",backgroundColor:"#f2f2f2"}}><MovieMain/></div> */}
      {/* <div style={{width:"100%",height:"100%",backgroundColor:"#f2f2f2"}}><MovieComponent/></div> */}
      <ProtectedRoute exact path="/home" component={Home}/>
      <ProtectedRoute exact path="/movies" component={MovieMain}/>
      <ProtectedRoute path="/booking" component={MovieHallMain}/>
      <ProtectedRoute exact path="/bookinghistory" component={BookingHistory}/>
      <ProtectedRoute exact path="/account" component={Account}/>
      {/* <Route path="/" component={SignIn}/> */}
      <Route exact path="/" component={SignIn}/>
      <Route exact path="/signup" component={SignUp}/>
      <Route exact path="/forgotpassword" component={ForgotPassword}/>
      <Route path="/resetpassword" component={ResetPassword}/>
      <Route path="/verification" component={EmailVerified}/>
     {/* <div style={{width:"100%",height:"100%",backgroundColor:"#f2f2f2"}}><MovieHallMain/></div> */}
     <Route path="/404" component={NotFound}/>
     <Redirect to="/404" />
     </Switch>
     
      </div>
    );
  }
}

export default App;
