import { useEffect, useState } from "react";
import "./bootstrap.min.css";

interface Forecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

function App() {
  const [forecasts, setForecasts] = useState<Forecast[]>();
  const [botResponse, setBotResponse] = useState<string>();
  const [userMsg, setUserMsg] = useState<string>();

  const submitMsg = async () => {
    let res = await fetch("/api/Chat/msg", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userMsg),
    });

    let body = await res.json();
    setBotResponse(body);
  };

  useEffect(() => {
    populateWeatherData();
  }, []);

  const contents =
    forecasts === undefined ? (
      <p>
        <em>
          Loading... Please refresh once the ASP.NET backend has started. See{" "}
          <a href="https://aka.ms/jspsintegrationreact">
            https://aka.ms/jspsintegrationreact
          </a>{" "}
          for more details.
        </em>
      </p>
    ) : (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>

        </thead>

      </table>
    );

  return (
    <div>
      <h1 id="tableLabel">Shipping Chat </h1>
      <p>
        This chatbot can summarize the Pitney Bowes Shipping API documetation.
      </p>
      {contents}

      <div>
        ask me a question:
        <div className="textbox">
          <input onChange={(e) => setUserMsg(e.target.value)} />
        </div>
        <button onClick={submitMsg}>chat</button>
      </div>

      <div>{botResponse}</div>
    </div>
  );

  async function populateWeatherData() {
    const response = await fetch("weatherforecast");
    const data = await response.json();
    setForecasts(data);
  }
}

export default App;
