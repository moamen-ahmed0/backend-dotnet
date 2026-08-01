import { useEffect, useState } from "react";

const API_URL = "/api";

export default function App() {
  const [users, setUsers] = useState([]);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [editingId, setEditingId] = useState(null);
  const [error, setError] = useState("");

  async function loadUsers() {
    try {
      const res = await fetch(`${API_URL}/users`);
      if (!res.ok) throw new Error("Failed to load users");
      setUsers(await res.json());
    } catch (err) {
      setError(err.message);
    }
  }

  useEffect(() => {
    loadUsers();
  }, []);

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");

    const method = editingId ? "PUT" : "POST";
    const url = editingId ? `${API_URL}/users/${editingId}` : `${API_URL}/users`;

    try {
      const res = await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });
      if (!res.ok) throw new Error("Request failed");

      setUsername("");
      setPassword("");
      setEditingId(null);
      await loadUsers();
    } catch (err) {
      setError(err.message);
    }
  }

  function startEdit(user) {
    setEditingId(user.id);
    setUsername(user.username);
    setPassword(user.password);
  }

  function cancelEdit() {
    setEditingId(null);
    setUsername("");
    setPassword("");
  }

  async function handleDelete(id) {
    setError("");
    try {
      const res = await fetch(`${API_URL}/users/${id}`, { method: "DELETE" });
      if (!res.ok) throw new Error("Delete failed");
      await loadUsers();
    } catch (err) {
      setError(err.message);
    }
  }

  return (
    <div style={{ maxWidth: 480, margin: "40px auto", fontFamily: "sans-serif" }}>
      <h1>Users</h1>

      {error && <p style={{ color: "red" }}>{error}</p>}

      <form onSubmit={handleSubmit} style={{ marginBottom: 24 }}>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
          style={{ marginRight: 8 }}
        />
        <input
          type="text"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          style={{ marginRight: 8 }}
        />
        <button type="submit">{editingId ? "Update" : "Add"}</button>
        {editingId && (
          <button type="button" onClick={cancelEdit} style={{ marginLeft: 8 }}>
            Cancel
          </button>
        )}
      </form>

      <ul style={{ listStyle: "none", padding: 0 }}>
        {users.map((user) => (
          <li
            key={user.id}
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              padding: "8px 0",
              borderBottom: "1px solid #ddd",
            }}
          >
            <span>{user.username}</span>
            <span>
              <button onClick={() => startEdit(user)} style={{ marginRight: 8 }}>
                Edit
              </button>
              <button onClick={() => handleDelete(user.id)}>Delete</button>
            </span>
          </li>
        ))}
      </ul>
    </div>
  );
}
