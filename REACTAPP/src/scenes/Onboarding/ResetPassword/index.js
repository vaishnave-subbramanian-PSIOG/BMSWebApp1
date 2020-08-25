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
import Snackbar from '@material-ui/core/Snackbar';
import IconButton from '@material-ui/core/IconButton';
import MuiAlert from '@material-ui/lab/Alert';
import CircularProgress from '@material-ui/core/CircularProgress';
// import {withRouter} from 'react-router-dom';
import './index.css';

class ResetPassword extends Component {
  constructor(props) {
    super(props);
    this.state = {
      password: null,
      confirmPassword: null,
      snackbaropen: false, snackbarmsg: '', snackbaropen: false, spinner: false,

    };
    this.handleChange = this.handleChange.bind(this);
  }
  snackbarClose = (e) => {
    this.setState({ snackbaropen: false });
  }

  PasswordReset = () => {
    console.log(this.state);
    this.setState({ spinner: true })

    axios.put('https://localhost:44343/api/authentication/resetpassword', {
      token: this.props.location.pathname.substring(this.props.location.pathname.lastIndexOf('/') + 1),
      newpassword: this.state.password

    })
      .then(json => {
        this.setState({ spinner: false })

        json.status == 200 ? (this.setState({snackbaropen:true , snackbartype:"success",snackbarmsg : "Your password has been reset!"}),
        setTimeout(() => { 
          this.props.history.push("/")
          }, 5000)) : null;


      }).catch(e => {
        console.log(e.response)
        this.setState({ spinner: false })
        e.response.status == 400 && e.response.data.Message == "Reset password session has expired." ? (this.setState({ snackbaropen: true, snackbartype: "error", snackbarmsg: "Reset password session has expired." }),
          setTimeout(() => {
            window.location.reload(true);

          }, 3000)) : null;
        e.response.status == 400 && e.response.data.Message == "Invalid Token" ? (this.setState({ snackbaropen: true, snackbartype: "error", snackbarmsg: "Please use the correct link URL to reset your password." }),
          setTimeout(() => {
            window.location.reload(true);

          }, 3000)) : null;
        e.response.status == 500 ? (this.setState({ snackbaropen: true, snackbartype: "error", snackbarmsg: "Something went wrong. Please try again." }),
          setTimeout(() => {
            window.location.reload(true);

          }, 3000)) : null;
        // window.location.reload(true); 

      })
  }

  handleChange = (e) => {
    this.setState({ [e.target.name]: e.target.value });
    this.setState({ isAvailable: true });
    console.log(this.state);

  }

  handleSubmit = (e) => {
    e.preventDefault();

    if (this.state.password === this.state.confirmPassword) {
      this.PasswordReset();
    }
    else {
      // this.setState({snackbaropen:true , snackbarmsg : 'Enter a valid email.'});
      this.setState({snackbaropen:true , snackbartype:"error",snackbarmsg : "Passwords do not match!"});

    }
  }
  render() {
    return (

      <Grid container component="main" className="root">
        <CssBaseline />
        {this.state.spinner ? (<div className="spinner">
          <CircularProgress thickness="5" />
        </div>) : null}
        <Snackbar
          anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
          open={this.state.snackbaropen}
          autoHideDuration={6000}
          onClose={this.snackbarClose}
          message={<span id="message-id">{this.state.snackbarmsg}</span>}
          action={[
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
              style={{ padding: 'unset', width: 250, height: 60 }} src={otherlogo}></img>
            <Avatar className="avatar"

            >
              <LockOutlinedIcon />
            </Avatar>
            <Typography component="h1" variant="h5">
              Reset password
              </Typography>
            <form className="formClass" noValidate>
              <Grid container spacing={2} style={{ marginTop: 10 }}>

                <Grid item xs={12}>

                  <TextField
                    variant="outlined"
                    required
                    fullWidth
                    name="password"
                    label="Password"
                    type="password"
                    id="password"
                    onChange={this.handleChange}

                  />
                </Grid>
                <Grid item xs={12}>

                  <TextField
                    variant="outlined"
                    required
                    fullWidth
                    name="confirmPassword"
                    label="Confirm Password"
                    type="password"
                    id="confirmpassword"
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
                    disabled={!this.state.confirmPassword || !this.state.password}
                  >
                    Set Password
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

export default ResetPassword;
    // export default withRouter(SignUp);
