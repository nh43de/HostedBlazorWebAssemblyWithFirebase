

// Initialize Firebase
firebase.initializeApp(window.firebaseConfig);

console.log("firebase.js loaded");


//send to cache when auth state changes
firebase.auth().onAuthStateChanged(async firebaseUser => {
    console.log(firebaseUser);
    var tokenResult = await firebaseUser.getIdTokenResult();

    DotNet.invokeMethodAsync('HostedBlazorWithFirebase.Client', 'UpdateFirebaseUserAndToken', firebaseUser, tokenResult);
});

async function firebaseCreateUser(email, password) {
    try {
        await firebase.auth().createUserWithEmailAndPassword(email, password);
        await firebaseEmailSignIn(email, password);
    } catch (error) {
        var errorResult = error.code + ": " + error.message;
        return errorResult;
    };
}

async function firebaseEmailSignIn(email, password) {
    try {
        await firebase.auth().signInWithEmailAndPassword(email, password);
    } catch (error) {
        var errorResult = error.code + ": " + error.message;
        return errorResult;
    }
}

async function firebaseGetIdTokenResult() {
    console.log("Requested token from firebase");

    var user = await firebase.auth().currentUser;

    if (!user) {
        console.log("Request token from firebase: user not found");

        return null;
    }

    var token = await user.getIdTokenResult();

    console.log("Requested token from firebase and got: " + token);

    if (token) {
        const json = JSON.stringify(token);
        return json;
    } else {
        return null;
    }
}

async function firebaseGetCurrentUser() {
    var user = await firebase.auth().currentUser;
    if (user) {
        const JsonUser = JSON.stringify(user);
        return JsonUser;
    } else {
        return null;
    }
}

async function firebaseSignOut() {
    try {
        await firebase.auth().signOut();
        return false;
    } catch (error) {
        return true;
    }
}

//if using outh flow login
window.FirebaseLoginOauth = (instance) => {
    var provider = new firebase.auth.EmailAuthProvider();
    //provider.addScope('https://www.googleapis.com/auth/contacts.readonly');
 
    if (localStorage.token) {
        console.log(localStorage.token);
        instance.invokeMethod('LoginCallback', localStorage.email, localStorage.display, localStorage.token);
    }
    else {
        firebase.auth().signInWithPopup(provider).then(function (result) {
            var token = result.credential.accessToken;
            var user = result.user;

            console.log(user);
            
            user.getIdToken(/* forceRefresh */ true).then(function (idToken) {
                localStorage.display = user.displayName;
                localStorage.email = user.email;
                //localStorage.token = idToken; //don't want to do this because this token can expire
                instance.invokeMethod('LoginCallback', user.email, user.displayName, idToken);
            }).catch(function (error) {
                console.log(error);
            });
        }).catch(function (error) {
            // Handle Errors here.
            var errorCode = error.code;
            var errorMessage = error.message;
            var email = error.email;
            var credential = error.credential;
            
        });
    }
};

//async function initializeInactivityTimer(dotnetHelper) {
//    var timer;
//    let counter = 0;
//    document.addEventListener("mousemove", resetTimer);
//    document.addEventListener("keypress", resetTimer);
//    function resetTimer() {
//        if (counter == 0) {
//            clearTimeout(timer);
//            timer = setTimeout(logout, 20000);
//        }
//    }
//    function logout() {
//        dotnetHelper.invokeMethodAsync("LogoutClick");
//        counter++;
//    }
//}

//async function getRefreshToken(refreshToken) {
//    var myHeaders = new Headers();
//    myHeaders.append("Content-Type", "application/json");
//    var raw = JSON.stringify({
//        "grant_type": "refresh_token",
//        "refresh_token": refreshToken
//    });
//    var requestOptions = {
//        method: 'POST',
//        headers: myHeaders,
//        body: raw,
//        redirect: 'follow'
//    };
//    const response = await fetch("https://securetoken.googleapis.com/v1/token?key=[API_KEY]", requestOptions)
//    const responseText = await response.json();
//    return JSON.stringify(responseText);
//}