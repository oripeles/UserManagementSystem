
import React, { useState } from "react";

function ProfilePage() {
  const [profile, setProfile] = useState<any>(null);
  const [error, setError] = useState("");

  const handleGetProfile = async () => {
    const token = localStorage.getItem("token"); 
    if (!token) {
      setError("No token found. Please login first.");
      return;
    }

    try {
    
      const response = await fetch("http://localhost:5269/api/auth/me", {
        method: "GET",
        headers: {
          "Authorization": `Bearer ${token}`
        }
      });

      if (response.ok) {
        
        const data = await response.json();

        setProfile(data);
        setError("");
      } else {
        const text = await response.text();
        setError("Error: " + text);
      }
    } catch (err) {
      setError("Error: " + (err as Error).message);
    }
  };

  return (
    <div style={{ margin: "20px" }}>
      <h2>My Profile</h2>
      {!profile && (
        <button onClick={handleGetProfile}>Load Profile</button>
      )}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {profile && (
        <div>
          <p>ID: {profile.id}</p>
          <p>Email: {profile.email}</p>
          <p>Full Name: {profile.fullName}</p>
        </div>
      )}
    </div>
  );
}

export default ProfilePage;
