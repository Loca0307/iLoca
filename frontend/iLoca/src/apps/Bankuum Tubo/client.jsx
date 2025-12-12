import { useEffect, useState } from "react";
import "../../styles/style.css";

export default function Clients() {
  const [clients, setClients] = useState([]);
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    iban: ""
  });

  // Load all clients 
  async function loadClients() {
    try {
      const res = await fetch("http://localhost:5027/client/ShowClients");
      if (!res.ok) throw new Error("Failed to fetch clients");

      const data = await res.json();
      setClients(data);
    } catch (err) {
      console.error(err);
    }
  }

  useEffect(() => {
    loadClients();
  }, []);

  // Add new client 
  async function handleSubmit(e) {
    e.preventDefault();

    try {
      const res = await fetch("http://localhost:5027/client/InsertClient", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData),
      });

      if (!res.ok) throw new Error("Failed to add client");

      // Clear the form
      setFormData({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        iban: "",
      });

      loadClients();
    } catch (err) {
      console.error(err);
      alert("Error adding client");
    }
  }

  // Delete client 
  async function deleteClient(id) {
    try {
      const res = await fetch("http://localhost:5027/client/DeleteClient", {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ clientId: id })
      });

      if (!res.ok) throw new Error("Failed to delete client");

      loadClients();
    } catch (err) {
      console.error(err);
      alert("Error deleting client");
    }
  }

  // Handle form updates 
  function handleChange(e) {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  }

  return (
    <div className="clients-page">
     
      <main id="mainTransaction">
        
        
        <section className="client-table">
          <h2>Clients</h2>

          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
                <th>Phone</th>
                <th>IBAN</th>
                <th>Balance</th>
                <th>Delete</th>
              </tr>
            </thead>

            <tbody>
              {clients.length === 0 ? (
                <tr>
                  <td colSpan="8">No clients found</td>
                </tr>
              ) : (
                clients.map(c => (
                  <tr key={c.clientId}>
                    <td>{c.clientId}</td>
                    <td>{c.firstName}</td>
                    <td>{c.lastName}</td>
                    <td>{c.email}</td>
                    <td>{c.phone}</td>
                    <td>{c.iban || ""}</td>
                    <td>{c.balance != null ? Number(c.balance).toFixed(2) : "0.00"}</td>
                    <td>
                      <button
                        className="remove-btn"
                        onClick={() => deleteClient(c.clientId)}
                      >
                        Remove
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </section>

        {/* ADD CLIENT */}
        <section className="add-client">
          <h2>Add New Client</h2>

          <form onSubmit={handleSubmit}>
            <input 
              type="text" 
              id="firstName" 
              placeholder="First Name" 
              value={formData.firstName}
              onChange={handleChange}
              required 
            />

            <input 
              type="text" 
              id="lastName" 
              placeholder="Last Name" 
              value={formData.lastName}
              onChange={handleChange}
              required 
            />

            <input 
              type="email" 
              id="email" 
              placeholder="Email" 
              value={formData.email}
              onChange={handleChange}
              required 
            />

            <input 
              type="text" 
              id="phone" 
              placeholder="Phone" 
              value={formData.phone}
              onChange={handleChange}
              required 
            />

            <input 
              type="text" 
              id="iban" 
              placeholder="IBAN" 
              value={formData.iban}
              onChange={handleChange}
              required 
            />

            <button type="submit">Add Client</button>
          </form>
        </section>

      </main>


    </div>
  );
}
