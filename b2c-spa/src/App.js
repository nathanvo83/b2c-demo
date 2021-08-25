import "./App.css";
import {
  useMsal,
  AuthenticatedTemplate,
  UnauthenticatedTemplate,
} from "@azure/msal-react";
import Weather from "./components/Weather";

function App() {
  const { instance, accounts } = useMsal();

  const signInHandler = (instance) => {
    instance.loginRedirect();
  };
  const SignInButton = () => (
    <button onClick={() => signInHandler(instance)}>Sign In</button>
  );

  const signOutHandler = (instance, account) => {
    const logoutRequest = {
      accounts: instance.getAccountByHomeId(account.homeAccountId),
      postLogoutRedirectUri: "http://localhost:3000",
    };
    instance.logoutRedirect(logoutRequest);
  };
  const SignOutButton = () => (
    <button onClick={() => signOutHandler(instance, accounts[0])}>
      Sign Out
    </button>
  );

  const Welcome = () => <div>Welcome: {accounts[0].username}</div>;

  return (
    <div className="App">
      <div>
        <AuthenticatedTemplate>
          <h3>Authenticated</h3>
          <Welcome />
          <br />
          <SignOutButton />
          <br />
          <Weather />
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
          <h3>Unauthenticated</h3>
          <br />
          <SignInButton />
          <br />
        </UnauthenticatedTemplate>
      </div>
    </div>
  );
}

export default App;
