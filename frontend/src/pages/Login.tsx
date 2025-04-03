import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [msg, setMsg] = useState("");
  
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch("http://localhost:5269/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });
      if (response.ok) {
        const data = await response.json();
        navigate("/profile");
        localStorage.setItem("token", data.token);
        setMsg("Login successful!");
      } else {
        const errText = await response.text();
        setMsg("Login failed: " + errText);
      }
    } catch (error) {
      setMsg("Error: " + (error as Error).message);
    }
  };

  return (
    <div style={{ margin: "20px" }}>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email: </label>
          <input 
            type="email"
            value={email} 
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Password: </label>
          <input 
            type="password"
            value={password} 
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Login</button>
      </form>
      <p>{msg}</p>

      <div style={{ marginTop: "10px" }}>
        <span>Don't have an account? </span>
        <button onClick={() => navigate("/register")}>Register here</button>
      </div>
    </div>
  );
}

export default LoginPage;
