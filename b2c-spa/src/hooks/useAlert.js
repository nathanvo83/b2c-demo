const infoAlert = (msg) => {
  alert("info: " + msg);
};

const errorAlert = (msg) => {
  alert("error: " + msg);
};

const useAlert = () => {
  return {
    infoAlert,
    errorAlert,
  };
};

export { useAlert };
