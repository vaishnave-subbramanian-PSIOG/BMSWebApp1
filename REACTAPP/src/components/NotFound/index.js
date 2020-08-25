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
import otherlogo from '../../Assets/otherlogo.PNG';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
// import {withRouter} from 'react-router-dom';
import './index.css';

class NotFound extends Component {
  constructor(props) { 
    super(props);  
    this.state = {
    };  
  }  
  render() {
    return (

      <Grid container component="main" className="root">
        <CssBaseline />
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
              404 PAGE NOT FOUND
              </Typography>
            <form className="formClass" noValidate>
              <Grid container spacing={2} style={{ marginTop: 10 }}>

                <Grid item xs={12}>
                  Sorry, we couldn't find this page.
                </Grid>
                <Grid item xs={12}>
                  You can find plenty of other things to do on our homepage.
                </Grid>
                <Grid item xs={12}>

                  <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    color="primary"
                    className="submitButton"
                    onClick={()=>(this.props.history.push("/home"))}
                  >
                    Go Home
            </Button>
                </Grid>
              </Grid>

              <Grid container style={{ marginTop: 20 }}>

              </Grid>

            </form>

          </div>
        </Grid>
      </Grid>

    );
  }
}

export default NotFound;
    // export default withRouter(SignUp);
