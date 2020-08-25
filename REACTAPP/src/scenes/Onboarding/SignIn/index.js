import React, { Component } from 'react';
import axios from 'axios';
import Avatar from '@material-ui/core/Avatar';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
// import FormControlLabel from '@material-ui/core/FormControlLabel';
// import Checkbox from '@material-ui/core/Checkbox';
import Link from '@material-ui/core/Link';
import Paper from '@material-ui/core/Paper';
import Box from '@material-ui/core/Box';
import Grid from '@material-ui/core/Grid';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import Snackbar from '@material-ui/core/Snackbar';
import IconButton from '@material-ui/core/IconButton';
import MuiAlert from '@material-ui/lab/Alert';
import CloseIcon from '@material-ui/icons/Close';
// import {withRouter} from 'react-router-dom';
import './index.css';
import Auth from '../../../Auth';
import otherlogo from '../../../Assets/otherlogo.PNG';

class SignIn extends Component {
  constructor(props) { 
    super(props);  
    this.state = {
    email:null,
    password:null,
    snackbaropen :false, snackbarmsg:'',snackbartype:"",
    isAvailable:false,
    SubmissionStatus:false, 
    };  
    this.handleChange = this.handleChange.bind(this); 
  }  

  snackbarClose = (e) =>{
    this.setState({snackbaropen:false});
  }
  
  GetLoginToken=()=>{  
    console.log(this.state);  

    axios.post('https://localhost:44343/api/token',{
      email: this.state.email,
      password: this.state.password
  } )  
  .then(json => {  
    console.log(json);  
    json.status==200?(sessionStorage.setItem("token",json.data.AuthToken),
    sessionStorage.setItem("UserID",json.data.Customer.CustomerID),
    sessionStorage.setItem("UserName",json.data.Customer.CustomerName),
sessionStorage.setItem("UserEmail",json.data.Customer.CustomerEmail),
sessionStorage.setItem("isAdmin",json.data.Customer.isAdmin==false?0:1)):null;
this.setState({snackbaropen:true , snackbartype:"success",snackbarmsg : "Welcome back, "+json.data.Customer.CustomerName+" !"});
setTimeout(() => { 
Auth.login(()=>{this.props.history.push("/home")})
}, 2000)

  }).catch(e => {
    console.log(e.response);
    e.response.status==400 && e.response.data.Message=="Invalid Credentials"?(this.setState({snackbaropen:true , snackbartype:"error",snackbarmsg : "Invalid credentials"}),
    setTimeout(() => { 
    window.location.reload(true); 

    }, 4000)):null;
    e.response.status==400 && e.response.data.Message=="User Doesn't Exist"?(this.setState({snackbaropen:true , snackbartype:"error",snackbarmsg : "This user doesn't exist."}),
    setTimeout(() => { 
    window.location.reload(true); 

    }, 4000)):null;
    e.response.status==400 && e.response.data.Message=="User Not Verified"?(this.setState({snackbaropen:true , snackbartype:"error",snackbarmsg : "This user is not verified."}),
    setTimeout(() => { 
    window.location.reload(true); 

    }, 4000)):null;
    // e.response.status!=400?alert("There was an error. Please try again."):null;
    // window.location.reload(true); 

    })  
  }  
  
  handleChange= (e)=> {  
  this.setState({[e.target.name]:e.target.value});
  this.setState( {isAvailable: true });  
  console.log(this.state);

  } 
  
  handleSubmit=(e)=>{
    e.preventDefault();
        this.GetLoginToken();
  }
      render() {
        return (
  
            <Grid container component="main" className="root">
            <CssBaseline />
            <Snackbar 
          anchorOrigin={{vertical:'top',horizontal:'right'}}
          open = {this.state.snackbaropen}
          autoHideDuration = {6000}
          onClose={this.snackbarClose}
          message = {<span id="message-id">{this.state.snackbarmsg}</span>}
          action ={[
            <IconButton 
            key="close"
            arial-label="close"
            color="#FFFFFF"
            onClick={this.snackbarClose}>
            </IconButton>
          ]}
          >
                  <MuiAlert elevation={6} variant="filled" onClose={this.state.snackbaropen} severity={this.state.snackbartype}>
          {this.state.snackbarmsg}
        </MuiAlert>
        </Snackbar>
            <Grid item xs={false} sm={4} md={7} className="image" />
            <Grid className="paperContainer" item xs={12} sm={8} md={5} component={Paper} elevation={6} square>
              <div className="paper">
              <img 
            // ,zIndex:30,width:'170px',height:'80px',position:"fixed",left:10
          style={{padding:'unset',width:250,height:60}} src={otherlogo}></img>
              <Avatar className="avatar"
              
              >

                <LockOutlinedIcon />
              </Avatar>
              <Typography component="h1" variant="h5">
                Sign in
              </Typography>
              <form className="formClass" noValidate>
              <Grid container spacing={2} style={{ marginTop: 10 }}>

              <Grid item xs={12}>

              <TextField
              variant="outlined"
              required
              fullWidth
              id="email"
              label="Email Address"
              name="email"
              autoComplete="email"
              onChange={this.handleChange}
              
              autoFocus
            />
            </Grid>
                  <Grid item xs={12}>

            <TextField
              variant="outlined"
              required
              fullWidth
              name="password"
              label="Password"
              type="password"
              id="password"
              autoComplete="current-password"
              onChange={this.handleChange}

            />
            </Grid>
            <Grid item xs={12}>

            <Button
                  type="submit"
                  fullWidth
                  variant="contained"
                  color="primary"
                  className="submitButton"
                  onClick={this.handleSubmit}
                  
                  disabled={!this.state.email||!this.state.password}
            >
              Sign In
            </Button>
            </Grid>
            </Grid>

            <Grid container style={{ marginTop: 20 }}>
              <Grid item xs>
                <Link href="#" variant="body2" onClick={()=>(this.props.history.push("/forgotpassword"))}>
                  Forgot password?
                </Link>
              </Grid>
              <Grid item>
                <Link href="#" variant="body2" onClick={()=>(this.props.history.push("/signup"))}>
                  {"Don't have an account? Sign Up"}
                </Link>
              </Grid>
            </Grid>

              </form>
      
              </div>
            </Grid>
          </Grid>
        
        );
      }
    }
    
    export default SignIn;
    // export default withRouter(SignUp);
    