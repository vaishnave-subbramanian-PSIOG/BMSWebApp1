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
import otherlogo from '../../../Assets/otherlogo.PNG';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import CircularProgress from '@material-ui/core/CircularProgress';
// import {withRouter} from 'react-router-dom';

//Component imports 
import './index.css';

class EmailVerified extends Component {
    constructor(props) { 
        super(props);  
        this.state = {
        message:null,
        spinner:false,
        };  
      }  
      componentDidMount(){
this.setState({spinner:true})

        axios.post("https://localhost:44343/api/authentication/verifyemail", null,{
          params: {
              token:this.props.location.pathname.substring(this.props.location.pathname.lastIndexOf('/') + 1)
            }
          })
             .then(response => {
                 // console.log(response.data);
this.setState({spinner:false})

                 // movies(response.data);
                 response.data=="This account has been verified."?this.setState({message:"Your email has been verified."}):null;
      
             })
             .catch( error=>{
                 console.log(error.response.data)
this.setState({spinner:false})

                 error.response.data.Message=="Already Verified"?this.setState({message:"This email has already been verified."}):null;
                 error.response.data.Message=="Invalid Token"?this.setState({message:"Please use the correct link URL to verify your account."}):null;
                //  window.location.reload(true); 
              
             })
      }
      render() {
        return (
  
            <Grid container component="main" className="root">
            <CssBaseline />
            {this.state.spinner?(    <div className="spinner">
    <CircularProgress thickness="5" />
  </div>):null}
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
                Email verification
              </Typography>
              <form className="formClass" noValidate>
              <Grid container direction="column" alignItems="center"  justify="center" spacing={2} style={{ marginTop: 10 }}>

              <Grid item xs={12}>
              {this.state.message}
              {/* <TextField
              variant="outlined"
              required
              fullWidth
              id="email"
              label="Email Address"
              name="email"
              autoComplete="email"
              autoFocus
            /> */}
            </Grid>
                  <Grid item xs={12}>

                  Sign in to the website to access your account.
            </Grid>
            <Grid item xs={12}>
{/* 
            <Button
                  type="submit"
                  fullWidth
                  variant="contained"
                  color="primary"
                  className="submitButton"
            >
              Send Link
            </Button> */}
            </Grid>
            </Grid>

            <Grid container justify="center" style={{ marginTop: 20 }}>

                  <Grid item>
      
                    <Link href="#" variant="body2" onClick={()=>(this.props.history.push("/"))}>
                      Sign in
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
    
    export default EmailVerified;
    // export default withRouter(SignUp);
    