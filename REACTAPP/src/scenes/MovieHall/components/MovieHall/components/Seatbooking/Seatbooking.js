import React, { Component } from 'react';
import axios from 'axios';
import {withRouter} from 'react-router-dom';
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Snackbar from '@material-ui/core/Snackbar';
import IconButton from '@material-ui/core/IconButton';
import MuiAlert from '@material-ui/lab/Alert';
import CircularProgress from '@material-ui/core/CircularProgress';

//Component imports
import './Seatbooking.css';

class Seatbooking extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      seat: [],
      seatAvailable: [],
      seatSelected: [],
      seatReserved: [],
      confirmation: false,
      showSeatReservation:true,
      spinner:true,
      snackbaropen :false, snackbarmsg:'',snackbaropen:false,
      confirmBtnClickOnce:false,


    }
    console.log(this.props);

  }
  componentDidMount(){


   axios.get("https://localhost:44343/api/seats/", {
    params: {
        ShowID:this.props.showInfo.selectedShow.ShowID,
        RequestedDate: this.props.showInfo.selectedDate
      }
    })
       .then(response => {
           // console.log(response.data);
           // seats(response.data);
           console.log(response.data)

           this.setState({seat:response.data.Seats,
            seatReserved:response.data.ReservedSeats,
            spinner:false});



       })
       .catch(error=>{
           error.log(error);
       })

}
snackbarClose = (e) =>{
  this.setState({snackbaropen:false});
}
  onClickData(seat) {
    if(this.state.seatSelected.indexOf(seat) > -1 ) {
      this.setState({
        seatAvailable: this.state.seatAvailable.concat(seat),
        seatSelected: this.state.seatSelected.filter(res => res != seat),
        //seatReserved: this.state.seatReserved.filter(res => res != seat)
      })
    } else {
      this.setState({
        seatSelected: this.state.seatSelected.concat(seat),
        //seatReserved: this.state.seatReserved.concat(seat),
        seatAvailable: this.state.seatAvailable.filter(res => res != seat)
      })
    }
  }
  checktrue(row) {
    if(this.state.seatReserved.indexOf(row) > -1){
      return false
    }else{
      return true
    }
  }

  handleSubmited() {
    this.setState({seatReserved: this.state.seatReserved.concat(this.state.seatSelected)})
    // this.setState({
    //   seatSelected: [],
    // })
    // console.log(this.state.seatSelected.length)
    if(this.state.seatSelected.length>0){
      this.setState({
      confirmation:true,
      showSeatReservation:false
      })
    }
  }

  render() {
    return (
      <div>
        <DrawGrid
          seat={ this.state.seat }
          available={ this.state.seatAvailable }
          reserved={ this.state.seatSelected }
          selected={ this.state.seatReserved }
          onClickData={ this.onClickData.bind(this)}
          checktrue={ this.checktrue.bind(this)}
          handleSubmited={ this.handleSubmited.bind(this)}
          confirmation={this.state.confirmation}
          showSeatReservation={this.state.showSeatReservation}
          showInfo={this.props.showInfo}
          parentProps={this.props}
        /></div>
    )
  }
}

class DrawGrid extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      Transaction:null
    }
    this.handleChange = this.handleChange.bind(this);

  }
  
  confirmBooking(){
    this.setState({confirmBtnClickOnce:true,spinner:true})

    axios.post('https://localhost:44343/api/booking',{
      CustomerID:sessionStorage.getItem("UserID"),
      ShowID:this.props.showInfo.selectedShow.ShowID,
      ShowDate:this.props.showInfo.selectedDate,
      PaymentAmount:parseFloat((this.props.showInfo.selectedShow.Price)*(this.props.reserved.length)),
      TransactionMode:this.state.Transaction,
      ConfirmedSeats:this.props.reserved
  
  })  
  .then(json => {  
    console.log(json);  

  if(json.status===200){  
this.setState({spinner:false})
this.setState({snackbaropen:true , snackbartype:"success",snackbarmsg : "Booking confirmed"});
setTimeout(() => { 
this.props.parentProps.history.push("/movies");  
}, 2000)
 
  }  
  else{  
 this.setState({snackbaropen:true , snackbartype:"error",snackbarmsg : "Something went wrong. Please try again."});
setTimeout(() => { 
window.location.reload(true); 

}, 3000);
  }  
  }) 
  }



handleChange(event) {
  const target = event.target;
  const value = target.type === 'checkbox' ? target.checked : target.value;
  const name = target.name;
  this.setState({
    [name]: value,
  });

}
  render() {
    return (
      <div className="container">
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
         {this.props.showSeatReservation?<div>
          <h1>Seat Reservation System</h1>
        <h2></h2>
        <table className="grid">
          <tbody>
            <tr>
            { this.props.seat.map( row =>
              <td
              className={this.props.selected.indexOf(row) > -1? 'reserved': (this.props.reserved.indexOf(row) > -1? 'selected':'available')}
              key={row} onClick={this.props.checktrue(row) ? e => this.onClickSeat(row) : null} >{row} </td>) }
            </tr>
          </tbody>
        </table>
        <div style={{width:530,padding:'unset',display:'flex',justifyContent:'center',alignItems:'center',marginTop:20}}>
        <Button
        type="button"
        fullWidth
                  variant="contained"
                  color="primary"
                  onClick={() => this.props.handleSubmited()}
            >Book</Button>
            </div>
        <div>
      </div>
        </div>
  :<Grid item xs={13}>{this.props.confirmation ? 
    
    
    <Paper className="paper confirmationSection" elevation={10}>
  {/* <div className="confirmationSection"> */}
    
   
    <h2>CONFIRMATION</h2>
  <div><label>Show date: {this.props.showInfo.selectedDate}</label></div>
  <div><label>Show time: {this.props.showInfo.selectedShow.ShowTime}</label></div>
  <div><label>Theatre: {this.props.showInfo.selectedShow.TheatreName}, {this.props.showInfo.selectedShow.TheatreLocation}</label></div>
  <div><label>Tickets chosen:{this.props.reserved} </label></div>
  <div><b><label>Total price: Rs {(this.props.showInfo.selectedShow.Price)*(this.props.reserved.length)}</label></b></div>
  <div><label>Transaction mode: 
  
  <select className="custom-select mb-2 mr-sm-2 mb-sm-0" name="Transaction" onChange={this.handleChange}>
            <option disabled selected value> -- select an option -- </option>
            <option value="Netbanking">Netbanking</option>
            <option value="Credit Card">Credit Card</option>
            <option value="Debit Card">Debit Card</option>
            <option value="UPI">UPI</option>
  </select>
  </label>
  </div>

  <Button
        type="button"
                  variant="contained"
                  color="primary"
                  fullWidth
                  onClick={() => this.confirmBooking()}
                  style={{marginTop:10,padding:'unset',fontSize:12}}

                  disabled={!this.state.Transaction || this.state.confirmBtnClickOnce}
            >Confirm Booking</Button>
  

  {/* </div> */}
  </Paper>
  : null }</Grid>} 
  </div>
      )
    }

    onClickSeat(seat) {
      this.props.onClickData(seat);
    }
  }
  export default withRouter(Seatbooking);