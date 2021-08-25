import { useAuth } from "./useAuth";
import axios from "axios";
import { useAlert } from "./useAlert";

const useHttpClient = () => {
  const { getToken } = useAuth();
  const { errorAlert } = useAlert();

  const createClient = async (getToken) => {
    const token = await getToken();
    return axios.create({
      baseURL: "https://localhost:44323",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  };

  const getAsync = async (url) => {
    const client = await createClient(getToken);

    try {
      const res = await client.get(url);
      return res.data;
    } catch (e) {
      errorAlert(e.message);
    }
  };
  const postAsync = async (url, payload) => {
    return "post";
  };
  const putAsync = async (url, payload) => {
    return "put";
  };
  const deleteAsync = async (url) => {
    return "delete";
  };

  return {
    getAsync,
    postAsync,
    putAsync,
    deleteAsync,
  };
};

export { useHttpClient };
