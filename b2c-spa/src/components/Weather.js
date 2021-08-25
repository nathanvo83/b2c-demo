import React, { useEffect, useState } from "react";
import { useHttpClient } from "../hooks/useHttpClient";

const Weather = () => {
  const [data, setData] = useState();
  const { getAsync } = useHttpClient();

  const loadData = async () => {
    const x = await getAsync("weatherforecast");
    console.log("x => ", x);
    setData(x);
  };

  const init = async () => {
    loadData();
  };

  useEffect(() => {
    init();
  }, []);

  return (
    <div>
      <h4>Weather</h4>
      weather: {data ? data.length : 0} records
      {data &&
        data.map((i, idx) => (
          <div key={idx}>
            <div>---{idx}---</div>
            <div>date: {i.date}</div>
            <div>temperatureC: {i.temperatureC}</div>
            <div>temperatureF: {i.temperatureF}</div>
            <div>summary: {i.summary}</div>
          </div>
        ))}
    </div>
  );
};

export default Weather;
