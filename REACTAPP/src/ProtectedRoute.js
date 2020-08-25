import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import Auth from './Auth';

export const ProtectedRoute = ({ component: Component, ...rest }) => {
    return (
        <Route
            {...rest} render={
                (props) => {
                    console.log(Auth.isAuthenticated())
                    if (Auth.isAuthenticated()) {
                        return <Component {...props} />
                    }
                    else {
                        return <Redirect to={
                            {
                                pathname: "/",
                                state: {
                                    from: props.location
                                }
                            }} />
                    }
                }

            }
        />
    )
}

