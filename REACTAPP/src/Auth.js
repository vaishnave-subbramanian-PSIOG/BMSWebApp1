class Auth{
    constructor(){
        this.authenticated=sessionStorage.getItem("UserID")?true:false;
    }
    login(cb){
        this.authenticated = true;
        cb();
    }
    logout(cb){
        console.log("logging out")
        this.authenticated = false;
        cb();
    }
    isAuthenticated(){
        return this.authenticated;
    }
}
export default new Auth();