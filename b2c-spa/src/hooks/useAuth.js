import { useMsal } from "@azure/msal-react";

const getToken = async (instance, account) => {
  //
  if (!account) {
    return null;
  }

  const tokenResult = await instance.acquireTokenSilent({
    scopes: [],
    account,
  });
  return tokenResult.idToken;
};

const useAuth = () => {
  const { instance, accounts } = useMsal();

  return {
    getToken: async () => getToken(instance, accounts[0] || null),
  };
};

export { useAuth };
