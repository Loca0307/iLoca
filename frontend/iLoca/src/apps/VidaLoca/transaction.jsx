import React, { useEffect, useState } from 'react';
import { verifyBankIban, vidaFetch } from '../../api';

export default function Transaction(){
  const [balance, setBalance] = useState('Loading...');
  const [ibanInfo, setIbanInfo] = useState('');

  useEffect(()=>{
    async function load(){
      try{
        const id = localStorage.getItem('accountId');
        if(!id){ setBalance('No account'); return; }
        const res = await vidaFetch(`/Account/GetBalanceByAccount?accountId=${id}`);
        if(!res.ok){ setBalance('Error'); return; }
        const b = await res.json(); setBalance(Number(b).toFixed(2) + ' €');
    }catch{ setBalance('-'); }
    }
    load();
  },[]);

  async function handleTransact(e){
    e.preventDefault();
    const amount = Number(document.getElementById('amount').value);
    const bankIban = document.getElementById('bankIban').value.trim();
    const mode = document.querySelector('input[name="mode"]:checked')?.value || 'withdraw';
    const msgEl = document.getElementById('msg');
    msgEl.innerText = '';
    if(!amount || amount <= 0){ msgEl.innerText = 'Enter a valid amount'; return; }
    if(!bankIban){ msgEl.innerText = 'Enter bank IBAN'; return; }

    // verify IBAN
    const v = await verifyBankIban(bankIban);
    if(!v.ok){ msgEl.innerText = 'IBAN not found'; return; }
    const info = await v.json();
    setIbanInfo((info.firstName||'') + ' ' + (info.lastName||'') + (info.email ? ' ('+info.email+')' : ''));

    const accountId = Number(localStorage.getItem('accountId')) || null;
    if(!accountId){ msgEl.innerText = 'No account selected. Please log in.'; return; }

    const payload = { accountId, amount, bankIban, IsDeposit: mode === 'deposit' };
    const endpoint = '/Account/WithdrawFromBank';
    const res = await vidaFetch(endpoint, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
    if(!res.ok){ msgEl.innerText = 'Transaction failed'; return; }
    msgEl.innerText = (mode === 'withdraw' ? 'Withdraw successful' : 'Deposit successful');
    // refresh balance
    const fres = await vidaFetch(`/Account/GetBalanceByAccount?accountId=${accountId}`);
    if(fres.ok){ const b = await fres.json(); setBalance(Number(b).toFixed(2) + ' €'); }
  }

  return (
    <div className="container center">
      <div className="bets-card">
        <h3>Your Balance</h3>
        <p id="balance" className="large muted">{balance}</p>

        <div className="mt-1">
          <label><input type="radio" name="mode" value="withdraw" defaultChecked /> Withdraw from bank</label>
          <label className="ml-2"><input type="radio" name="mode" value="deposit" /> Deposit to bank</label>
        </div>

        <label htmlFor="bankIban">Bank IBAN</label>
        <input id="bankIban" type="text" placeholder="IT00..." />
        <div className="mt-1"><span id="ibanInfo" className="muted ml-1">{ibanInfo}</span></div>

        <label htmlFor="amount">Amount</label>
        <input id="amount" type="number" step="0.01" min="0" />

        <div className="mt-1"><button id="transactBtn" className="btn btn-primary" onClick={handleTransact}>Withdraw</button></div>
        <div className="mt-1"><a className="btn btn-ghost" href="/vida/games">Back to Games</a></div>
        <p id="msg" className="muted mt-1"></p>
      </div>
    </div>
  );
}
