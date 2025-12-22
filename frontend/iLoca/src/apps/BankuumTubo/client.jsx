import { useState, useEffect  } from "react";

export default function Clients() {
  const [clients, setClients] = useState([]); 
  // Defines a variable to locally store the clients and defines 
  // the method to update the local variable (create a new one. Initialises the list as empty.
  const [formData, setFormData] = useState({
    // Define formData as a local variable and "setFormData" as the method 
    // to update it (create a new one) to create the client. Initialises it as a "Javascript Object" object with blank fields.
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    iban: ""
  });

  // Load all clients 
  async function loadClients() { // asynchronously request the clients list
    try {
      const res = await fetch("http://localhost:5027/client/ShowClients");
      if (!res.ok) throw new Error("Failed to fetch clients");

      const data = await res.json(); // The response is a string that gets JSON'ed to update the local variable clients
      setClients(data);
    } catch (err) {
      console.error(err);
    }
  }

 // To asynchronously run the method
  useEffect(() => {
    loadClients();
  }, []); // The "dependency array" [], tells when to rerun the method, in this case just once

  // Add new client 
  async function handleSubmit(event) {
    event.preventDefault(); // Prevents default HTML behavior

    try {
      const res = await fetch("http://localhost:5027/client/InsertClient", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData), // Stringify the JSON saved "formData" as the request body
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

      // To update the list instantly when a new client is added
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

  function handleChange(event) { // This method is called when something in the HTML form is modified so it keeps the formData object updated
    setFormData({ ...formData, [event.target.id]: event.target.value });
  } // ...formData == to select the old object except the property with a new value

  return (
    <div className="clients-page">
      <main className="mainSection">

        
        <section className="transaction-table">
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

        
        <section className="transaction-form">
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
