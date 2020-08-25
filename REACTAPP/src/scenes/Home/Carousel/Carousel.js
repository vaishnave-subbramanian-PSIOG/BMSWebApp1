
import React from 'react';
import { Card, makeStyles } from '@material-ui/core';
import './Carousel.css';
import { FaChevronLeft, FaChevronRight } from 'react-icons/fa';
import Button from '@material-ui/core/Button';
import { useHistory } from "react-router-dom";

function Arrow(props) {
    const { direction, clickFunction } = props;
    const icon = direction === 'left' ? <FaChevronLeft /> : <FaChevronRight />;

    return <div onClick={clickFunction}>{icon}</div>;
}


export default function CarouselSlide(props) {
  let history = useHistory();

    // const { backgroundColor, title } =props.content;
    const [isShown, setIsShown] = React.useState(false);
    const Background = "https://upload.wikimedia.org/wikipedia/en/b/bb/DhoomPoster.jpg";
    const useStyles = makeStyles(() => ({
        root:{
            display:'flex',
            justifyContent:'center',
            alignItems:'center'
        },
        card: {
            // backgroundImage: `url("${Background}")`,
            borderRadius: 5,
            // padding: '75px 50px',
            margin: '0px 25px',
            width: '500px',
            // boxShadow: '20px 20px 20px grey',
            display: 'flex',
            justifyContent: 'center',
            height:'60vh',
            boxShadow: '5px 10px 18px grey',
            // '&:hover': {
            //     backgroundColor: "rgba(128, 128, 128, 0.8);",
            //     }
        }
    }));

    const classes = useStyles();
    var posterURL="";
    var name="";
    var id="";
    console.log(props)
    {props.content!=undefined?(posterURL=props.content.PosterURL,name=props.content.Name,id=props.content.ID):null}

    return (
        <div className={classes.root}>
        <Card elevation={10} className={classes.card}>
            {/* <img src={!posterURL?(<h1>{name}</h1>):posterURL}></img> */}
            
<div 
 onMouseEnter={() => setIsShown(true)}
        onMouseLeave={() => setIsShown(false)}
className="pic">
  <img src={!posterURL?(<h1>{name}</h1>):posterURL}></img>
  <div className="overlay"></div>
  {isShown && (
        <div style={{zIndex:10,color:'white',position:'fixed',top:'320px',right:'520px',fontSize:'22px'}}>
            {/* <a href="#"><b>{genre}</b></a> */}
         <Button onClick={()=>{history.push("/booking/"+String(id))}} variant="contained" color="secondary">Book now
</Button>
        </div>
      )}
</div>


        </Card>
        </div>
    );
}