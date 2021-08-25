import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";

import { PublicClientApplication } from "@azure/msal-browser";
import { MsalProvider } from "@azure/msal-react";

const config = {
  auth: {
    clientId: "99bf99f1-0638-4a30-b430-11313deb9b1c",
    authority:
      "https://nathanadb2c.b2clogin.com/nathanadb2c.onmicrosoft.com/B2C_1_si/",
    knownAuthorities: ["https://nathanadb2c.b2clogin.com/"],
    redirectUri: "http://localhost:3000",
  },
};

// create PublicClientApplication instance
const publicClientApplication = new PublicClientApplication(config);

ReactDOM.render(
  <React.StrictMode>
    <MsalProvider instance={publicClientApplication}>
      <App />
    </MsalProvider>
  </React.StrictMode>,
  document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
