import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import ListSubheader from '@material-ui/core/ListSubheader';
import HomeIcon from '@material-ui/icons/Home';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import TheatersIcon from '@material-ui/icons/Theaters';
import ShoppingCartIcon from '@material-ui/icons/ShoppingCart';
import BookIcon from '@material-ui/icons/Book';
import PeopleIcon from '@material-ui/icons/People';
import BarChartIcon from '@material-ui/icons/BarChart';
import LayersIcon from '@material-ui/icons/Layers';
import AssignmentIcon from '@material-ui/icons/Assignment';
import { useHistory } from "react-router-dom";
import { makeStyles } from '@material-ui/core/styles';

import Auth from '../../Auth';


const useStyles = makeStyles({
  active: {
    backgroundColor: "rgb(207, 157, 148,0.8)",
    '&:hover': {
    backgroundColor: "rgb(207, 157, 148,0.8)",
    }
  },
});


const MainListItems = (props) => {
  let history = useHistory();
  const classes = useStyles();
  return(
  <div>
    <ListItem button
    className={props.active=="Home"?classes.active:null}
    onClick={()=>{history.push('/home')}}
     >
      <ListItemIcon>
        <HomeIcon />
      </ListItemIcon>
      <ListItemText primary="Home" 
     
      />
    </ListItem>
    <ListItem button
    className={props.active=="Movies"?classes.active:null}
    onClick={()=>{history.push('/movies')}}

    >
      <ListItemIcon>
        <TheatersIcon />
      </ListItemIcon>
      <ListItemText primary="Movies" 

      />
    </ListItem>
    <ListItem button
    className={props.active=="Booking History"?classes.active:null}
    onClick={()=>{history.push('/bookinghistory')}}

    >
      <ListItemIcon>
        <BookIcon />
      </ListItemIcon>
      <ListItemText primary="Booking History" 

      />
    </ListItem>
    <ListItem button
    className={props.active=="Account"?classes.active:null}
    onClick={()=>{history.push('/account')}}

    >
      <ListItemIcon>
        <PeopleIcon />
      </ListItemIcon>
      <ListItemText primary="Account" 
      />
    </ListItem>
    <ListItem button
    className={props.active=="Logout"?classes.active:null}
    onClick={()=>{Auth.logout(()=>{sessionStorage.clear();history.push('/')})}}

    >
      <ListItemIcon
      >
        <ExitToAppIcon />
      </ListItemIcon>
      <ListItemText primary="Logout" 
      /> 
      
    </ListItem>
    {/* <ListItem button>
      <ListItemIcon>
        <BarChartIcon />
      </ListItemIcon>
      <ListItemText primary="Reports" />
    </ListItem>
    <ListItem button>
      <ListItemIcon>
        <LayersIcon />
      </ListItemIcon>
      <ListItemText primary="Integrations" />
    </ListItem> */}
  </div>
);

// export const secondaryListItems = (
//   <div>
//     <ListSubheader inset>Saved reports</ListSubheader>
//     <ListItem button>
//       <ListItemIcon>
//         <AssignmentIcon />
//       </ListItemIcon>
//       <ListItemText primary="Current month" />
//     </ListItem>
//     <ListItem button>
//       <ListItemIcon>
//         <AssignmentIcon />
//       </ListItemIcon>
//       <ListItemText primary="Last quarter" />
//     </ListItem>
//     <ListItem button>
//       <ListItemIcon>
//         <AssignmentIcon />
//       </ListItemIcon>
//       <ListItemText primary="Year-end sale" />
//     </ListItem>
//   </div>
// );
}

export default MainListItems;