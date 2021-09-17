# HostedBlazorWebAssemblyWithFirebase
Hosted Blazor WebAssembly with Firebase


This is an example project showing how to host a Blazor SPA and use Firebase, while using the tokens to make API calls to a secured API.

To achieve this there are several parts:

- **firebase.js** - helper/interop functions for Blazor. Note that we hook into firebase' onAuthStateChanged and invoke .NET to update session storage. Since user and tokens will return null from Firebase JS library until the initial handshake is done (about 1-2 seconds), we cache these in localStorage every time the auth state changes. This will be used by the ClientSideStateProvider to get the auth before this initial handshake happens to speed things up.
- **ClientSideStateProvider** - this is what handles the "translation" of the Firebase token state into Blazor-world. Every time the client-side auth state is requested we will get it from Firebase js API. If not available, we will try to get it from cache.
- **FirebaseCache** - this caches tokens and user info in localstorage in case
- **FirebaseJsProvider** - 
- 
- 
- 


## Running the Project


1. Fill in your Firebase credentials and rename to firebase.user.js: \Client\wwwroot\firebase.user.sample.js
2. Fill in your Firebase project id (e.g. "TestProject-c4561") and rename to appsettings.user.json: Server\appsettings.user.sample.jso
3. You should be able to login and make authenticated requests.

