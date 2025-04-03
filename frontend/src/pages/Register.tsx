// src/pages/RegisterPage.tsx
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

function RegisterPage() {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [msg, setMsg] = useState("");
  
  const navigate = useNavigate(); 

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {

      const response = await fetch("http://localhost:5269/api/Auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ fullName, email, password }),
      });

      if (response.ok) {
        const text = await response.text();
        setMsg("Registration successful! " + text);
        navigate("/login");
      } else {
        const errText = await response.text();
        setMsg("Registration failed: " + errText);
      }
    } catch (error) {
      setMsg("Error: " + (error as Error).message);
    }
  };

  return (
    <div style={{ margin: "20px" }}>
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Full Name: </label>
          <input
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
            required
          />
        </div>
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
        <button type="submit">Create Account</button>
      </form>
      <p>{msg}</p>

      <div style={{ marginTop: "10px" }}>
        <span>Already have an account? </span>
        <button onClick={() => navigate("/login")}>Login here</button>
      </div>
    </div>
  );
}

export default RegisterPage;
