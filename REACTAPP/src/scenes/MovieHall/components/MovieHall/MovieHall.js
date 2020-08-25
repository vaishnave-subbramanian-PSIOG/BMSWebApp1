import React, { Component } from 'react';
import axios from 'axios';
import jwt_decode from 'jwt-decode';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import CircularProgress from '@material-ui/core/CircularProgress';
import Snackbar from '@material-ui/core/Snackbar';
import IconButton from '@material-ui/core/IconButton';
import MuiAlert from '@material-ui/lab/Alert';
//Component Imports
import Auth from '../../../../Auth';
import './MovieHall.css';
import Seatbooking from './components/Seatbooking/Seatbooking';

export class MovieHall extends Component {

  constructor(props) {
    super(props);
    this.state = {
      date: new Date().toISOString().substring(0, 10),
      selectedShow: null,
      shows: [],
      book: false,
      displayShows: false,
      spinner:false
    };
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  snackbarClose = (e) =>{
    this.setState({snackbaropen:false});
  }

  handleChange(event) {
    const target = event.target;
    const value = target.type === 'checkbox' ? target.checked : target.value;
    const name = target.name;

    name == "date" ? (this.setState({
      [name]: value,
      book: false,
      displayShows: false,
      selectedShow:null
    })) : (this.setState({
      [name]: value,
      book: false,
    }));

  }
  handleDisplayShows = (event) => {
    // console.log(new Date(Date.parse(this.state.date)).getDate().toString())
    console.log(this.props);
    event.preventDefault();
    this.setState({ displayShows: true });

    if (Auth.isAuthenticated()) {
      var decoded = jwt_decode(sessionStorage.getItem("token"));
      var tokenExpiration = new Date(decoded.exp * 1000);
      var currentDate = new Date();
      if (currentDate > tokenExpiration) {
this.setState({snackbaropen:true , snackbartype:"warning",snackbarmsg : "Your session has expired."});

        Auth.logout(() => { sessionStorage.clear(); this.props.history.push('/') });
      }
      else{
this.setState({spinner:true})
    axios.get("https://localhost:44343/api/show", {
      params: {
        MovieID: parseInt(this.props.history.location.pathname.substring(this.props.history.location.pathname.lastIndexOf('/') + 1)),
        RequestedDate: this.state.date
      }
    })
      .then(response => {
        // console.log(response.data);
        // movies(response.data);
        var tempArray=[];
        response.data.map((show, index) => {
          var time=show.ShowTime.split(":");
          // console.log(time)
          var date = new Date (new Date(Date.parse(this.state.date)).setHours(time[0],time[1],time[2],0));
          date>new Date()?(tempArray.push(show)):null;
          
        });
        this.setState({shows:tempArray,spinner:false });

      })
      .catch(error => {
        this.setState({spinner:false });
          error.response.status == 400 ? (this.setState({snackbaropen:true , snackbartype:"error",snackbarmsg : "There are no shows for this request.",displayShows:false})): null;
          error.response.status != 400 ? (this.setState({snackbaropen:true , snackbartype:"error",snackbarmsg : "Something went wrong."}),setTimeout(() => { 
            window.location.reload(true); 
            
            }, 2000)): null;
          

      })
    }
  }

  }
  handleSubmit = (event) => {

    event.preventDefault();
    this.setState({ book: true });
  }

  render() {
    return (
      <span style={{ padding: '1rem' }}>

{this.state.spinner?(    <div className="spinner">
    <CircularProgress thickness="5" />
  </div>):null}
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
        <Grid item xs={12}>
          <Paper className="paper" elevation={10}>
            <div className="container form">
              <form onSubmit={this.handleSubmit}>


                <div className="form-group">
                  {/* <label> */}
                    Date:
            {/* </label> */}
                  <input type="date" name="date" min={new Date().toLocaleString("en-IN", { timeZone: "Asia/Kolkata" }).substring(0, 10)} defaultValue={new Date().toISOString().substring(0, 10)} required onChange={this.handleChange} />

                  <Button
                  fullWidth

                  variant="contained"
                  color="secondary"
                  style={{width:510,marginTop:10,padding:'unset',fontSize:12}}
                  disabled={new Date(Date.parse(this.state.date))<(new Date().setHours(0,0,0,0))}
                  onClick={this.handleDisplayShows} type="button" 
            >Display shows</Button>
                </div>
                

                {this.state.displayShows ? <div className="form-group">
                    {/* <label> */}
                      Shows:
            <select className="custom-select mb-2 mr-sm-2 mb-sm-0" name="selectedShow" onChange={this.handleChange}>
                        <option disabled selected value> -- select an option -- </option>
                        {this.state.shows.map((show, index) => (
                            
                          // <option value={String(show.ShowID)}>{show.ShowTime}</option>
                          <option
                            value={String(show.ShowID)}
                        >{show.ShowTime.slice(0,-3)} - {show.MovieHallType} (Rs {show.Price}) @ {show.TheatreName} - [{show.UnreservedSeatsCount} seat(s) left]</option>

                        ))}
                      </select>
                    {/* </label> */}
                  {/* <div> */}
                    {this.state.selectedShow ? (                  <Button
                  fullWidth
                  variant="contained"
                  color="secondary"
                  type="submit" 
                  style={{width:510,marginTop:10,padding:'unset',fontSize:12}}

            >Select seats</Button>
                ) : null}


                  {/* </div> */}
                  </div> : null}
                

              </form>
            </div>


          </Paper>

        </Grid>
        {this.state.shows.filter(s => s.ShowID == parseInt(this.state.selectedShow)).map((m) =>
          (
            <div>
              {this.state.book ? <Seatbooking showInfo={{ selectedDate: this.state.date, selectedShow: m }} /> : null}
            </div>
          ))};
      </span>
    )
  }
}