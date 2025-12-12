import { useEffect, useState } from "react";
import "../../styles/style.css";

export default function Transactions() {
  const [transactions, setTransactions] = useState([]);
  const [receiverIban, setReceiverIban] = useState("");
  const [amount, setAmount] = useState("");
  const [reason, setReason] = useState("");
  const [message, setMessage] = useState("");

  const loadTransactions = async () => {
    try {
      const response = await fetch(
        "http://localhost:5027/Transaction/ShowTransactions",
        { cache: "no-store" }
      );
      if (!response.ok) throw new Error("Failed to fetch transactions");
      const data = await response.json();
      setTransactions(data);
    } catch {
      setTransactions([]);
    }
  };

  useEffect(() => {
    loadTransactions();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const senderEmail = localStorage.getItem("loggedInEmail") || "";

    if (!senderEmail) {
      setMessage("Error: No logged-in email found. Please log in.");
      return;
    }
    if (!receiverIban) {
      setMessage("Error: Receiver IBAN is required.");
      return;
    }
    const amt = parseFloat(amount);
    if (Number.isNaN(amt) || amt <= 0) {
      setMessage("Error: Amount must be a positive number.");
      return;
    }

    const transaction = {
      SenderEmail: senderEmail,
      ReceiverIban: receiverIban,
      Amount: Math.round(amt),
      Reason: reason,
    };

    setMessage("Submitting transaction...");

    try {
      const response = await fetch(
        "http://localhost:5027/Transaction/InsertTransaction",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(transaction),
        }
      );

      if (!response.ok) {
        let errMsg = "Failed to add transaction";
        try {
          const err = await response.json();
          if (err?.message) errMsg = err.message;
        } catch {
            // ignore errors
        }
        throw new Error(errMsg);
      }

      const data = await response.json().catch(() => ({}));
      let timestampMsg = "";
      if (data?.dateTime || data?.DateTime) {
        const dt = data.dateTime || data.DateTime;
        timestampMsg = ` (Timestamp: ${new Date(dt).toLocaleString()})`;
      }

      setMessage("Transaction added successfully!" + timestampMsg);
      setReceiverIban("");
      setAmount("");
      setReason("");
      await loadTransactions();
    } catch (err) {
      setMessage("Error adding transaction: " + (err?.message || "Unknown error"));
    }
  };

  return (
    <div className="transaction-container">


      <main id="mainTransaction">
        <section className="transaction-form">
          <h2>Add New Transaction</h2>
          <form onSubmit={handleSubmit}>
            <label htmlFor="receiverIban">Receiver IBAN</label>
            <input
              type="text"
              id="receiverIban"
              value={receiverIban}
              onChange={(e) => setReceiverIban(e.target.value)}
              placeholder="Receiver IBAN"
              required
            />
            <label htmlFor="amount">Amount</label>
            <input
              type="number"
              id="amount"
              value={amount}
              onChange={(e) => setAmount(e.target.value)}
              placeholder="Amount"
              step="0.01"
              required
            />
            <label htmlFor="reason">Reason</label>
            <input
              type="text"
              id="reason"
              value={reason}
              onChange={(e) => setReason(e.target.value)}
              placeholder="Reason"
              required
            />
            <button type="submit">Add Transaction</button>
          </form>
          {message && <div id="transaction-message">{message}</div>}
        </section>

        <section className="transaction-table">
          <h2>All Transactions</h2>
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Sender</th>
                <th>Receiver IBAN</th>
                <th>Amount</th>
                <th>Date/Time</th>
                <th>Reason</th>
              </tr>
            </thead>
            <tbody>
              {transactions.length === 0 && (
                <tr>
                  <td colSpan="6">No transactions available</td>
                </tr>
              )}
              {transactions.map((t, idx) => {
                const idVal = t.transactionId || t.transactionID || t.TransactionId || t.TransactionID || "";
                const senderVal = t.sender || t.senderEmail || t.Sender || t.SenderEmail || "";
                const receiverVal = t.receiverIban || t.receiverIBAN || t.ReceiverIban || t.ReceiverIBAN || "";
                const amountVal = t.amount || t.Amount || 0;
                const dateVal = t.dateTime || t.DateTime || null;
                const reasonVal = t.reason || t.Reason || "";
                return (
                  <tr key={idx}>
                    <td>{idVal}</td>
                    <td>{senderVal}</td>
                    <td>{receiverVal}</td>
                    <td>{amountVal}</td>
                    <td>{dateVal ? new Date(dateVal).toLocaleString() : ""}</td>
                    <td>{reasonVal}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </section>
      </main>


    </div>
  );
}
