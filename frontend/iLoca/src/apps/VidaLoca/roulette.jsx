import React, { useEffect, useRef } from 'react';

export default function Roulette(){
  const canvasRef = useRef(null);

  useEffect(()=>{
    const canvas = canvasRef.current;
    if(!canvas) return;
    const ctx = canvas.getContext('2d');
    const size = Math.min(canvas.width, canvas.height);
    const center = { x: canvas.width/2, y: canvas.height/2 };
    const radius = size/2 - 6;
    const numbers = [0,32,15,19,4,21,2,25,17,34,6,27,13,36,11,30,8,23,10,5,24,16,33,1,20,14,31,9,22,18,29,7,28,12,35,3,26];
    const colors = { red: '#d32f2f', black: '#111', green: '#1b5e20' };
    function isRed(n){ const reds = new Set([1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36]); return reds.has(n); }

    function drawWheel(rotation=0){
      ctx.clearRect(0,0,canvas.width,canvas.height);
      const slice = (2*Math.PI)/numbers.length;
      ctx.save(); ctx.translate(center.x, center.y); ctx.rotate(rotation);
      for(let i=0;i<numbers.length;i++){
        const start = i*slice; const n = numbers[i]; const color = n===0?colors.green:(isRed(n)?colors.red:colors.black);
        ctx.beginPath(); ctx.moveTo(0,0); ctx.arc(0,0,radius,start,start+slice); ctx.closePath(); ctx.fillStyle=color; ctx.fill(); ctx.strokeStyle='#222'; ctx.lineWidth=2; ctx.stroke();

        // draw number labels similar to backend
        const labelAngle = start + slice/2;
        const labelRadius = radius * 0.9;
        const lx = Math.cos(labelAngle) * labelRadius;
        const ly = Math.sin(labelAngle) * labelRadius;
        ctx.save();
        ctx.setTransform(1, 0, 0, 1, 0, 0);
        ctx.translate(center.x + lx, center.y + ly);
        ctx.rotate(labelAngle + Math.PI/2);
        ctx.fillStyle = '#fff';
        ctx.font = '16px Inter, system-ui, -apple-system, Roboto, Arial';
        ctx.textAlign = 'center';
        ctx.fillText(String(n), 0, 0);
        ctx.restore();
      }
      // center hub
      ctx.beginPath(); ctx.arc(0,0,32,0,2*Math.PI); ctx.fillStyle = '#222'; ctx.fill();
      ctx.restore();
    }

    // initial draw
    drawWheel(0);

    // Betting UI logic (port from backend static page)
    const betGrid = document.getElementById('betGrid');
    const chips = Array.from(document.querySelectorAll('.chip'));
    let selectedChip = null;
    let pendingBets = {}; // number -> amount
    let colorBets = { red: 0, black: 0 };

    function applyBetToCell(cell, number){
      if(!selectedChip) return;
      const val = parseInt(selectedChip.dataset.value,10);
      pendingBets[number] = (pendingBets[number]||0) + val;
      cell.classList.add('bet-placed');
      const b = cell.querySelector('.badge');
      if(b) b.innerText = pendingBets[number];
    }

    function createBetCells(){
      if(!betGrid) return;
      // clear existing
      betGrid.innerHTML = '';
      // 0 cell
      const zeroBtn = document.createElement('button');
      zeroBtn.className = 'bet-cell bet-cell-zero';
      zeroBtn.dataset.num = '0';
      zeroBtn.innerHTML = '<span class="num">0</span><span class="badge" aria-hidden="true"></span>';
      zeroBtn.addEventListener('click', () => applyBetToCell(zeroBtn, 0));
      betGrid.appendChild(zeroBtn);

      // numbers in table order: rows of 3 (3,2,1), (6,5,4), ...
      for(let row=1; row<=12; row++){
        const a = row * 3;
        const b = a - 1;
        const c = a - 2;
        [a,b,c].forEach(n => {
          const btn = document.createElement('button');
          btn.className = 'bet-cell';
          btn.dataset.num = String(n);
          const colorClass = isRed(n) ? 'num-red' : 'num-black';
          btn.innerHTML = `<span class="num ${colorClass}">${n}</span><span class="badge" aria-hidden="true"></span>`;
          btn.addEventListener('click', ()=> applyBetToCell(btn, n));
          betGrid.appendChild(btn);
        });
      }
    }

    createBetCells();

    chips.forEach(c=> c.addEventListener('click', ()=>{
      chips.forEach(x=> x.classList.remove('chip-selected'));
      c.classList.add('chip-selected');
      selectedChip = c;
    }));

    // color bet buttons
    const betRedBtn = document.getElementById('betRed');
    const betBlackBtn = document.getElementById('betBlack');
    function updateColorButtonUI(){
      if(betRedBtn) betRedBtn.innerText = colorBets.red > 0 ? `RED \n${colorBets.red}` : 'RED';
      if(betBlackBtn) betBlackBtn.innerText = colorBets.black > 0 ? `BLACK \n${colorBets.black}` : 'BLACK';
    }
    if(betRedBtn) betRedBtn.addEventListener('click', ()=>{
      if(!selectedChip) return;
      const val = Number(selectedChip.dataset.value);
      colorBets.red += val;
      updateColorButtonUI();
      const resultEl = document.getElementById('result'); if(resultEl) resultEl.innerText = 'Placed red bet: ' + colorBets.red;
    });
    if(betBlackBtn) betBlackBtn.addEventListener('click', ()=>{
      if(!selectedChip) return;
      const val = Number(selectedChip.dataset.value);
      colorBets.black += val;
      updateColorButtonUI();
      const resultEl = document.getElementById('result'); if(resultEl) resultEl.innerText = 'Placed black bet: ' + colorBets.black;
    });

    // chips and reset
    const resetBtn = document.getElementById('resetBtn');
    if(resetBtn) resetBtn.addEventListener('click', ()=>{
      pendingBets = {};
      document.querySelectorAll('.bet-cell').forEach(b=>{
        b.classList.remove('bet-placed');
        const badge = b.querySelector('.badge'); if(badge) badge.innerText='';
        delete b.dataset.amount;
      });
      colorBets = { red: 0, black: 0 };
      updateColorButtonUI();
      const resultEl = document.getElementById('result'); if(resultEl) resultEl.innerText = 'Bets reset.';
    });

    // spin
    const spinBtn = document.getElementById('spinBtn');
    let spinning = false;

    async function postUpdateBet(amount, operation, email){
      try{
        const res = await fetch('http://localhost:5112/Account/UpdateBetMoney', {
          method: 'POST', headers: {'Content-Type':'application/json'},
          body: JSON.stringify({ Amount: amount, Operation: operation, Email: email })
        });
        return res.ok;
      }catch { return false; }
    }

    function clearPlacedBetsUI(){
      document.querySelectorAll('.bet-cell').forEach(b=>{
        b.classList.remove('bet-placed','bet-win');
        const badge = b.querySelector('.badge'); if(badge) badge.innerText='';
      });
      pendingBets = {};
      colorBets = { red:0, black:0 };
    }

    if(spinBtn) spinBtn.addEventListener('click', async ()=>{
      if(spinning) return;
      // compute stake
      const numStake = Object.values(pendingBets).reduce((s,v)=> s + Number(v||0), 0);
      const colorStake = (Number(colorBets.red||0) + Number(colorBets.black||0));
      const totalStake = numStake + colorStake;
      const email = localStorage.getItem('VidaLoca/loggedInEmail');
      const resultEl = document.getElementById('result');
      if(totalStake > 0 && !email){ if(resultEl) resultEl.innerText = 'Please login to place bets.'; return; }

      // attempt to debit on server
      if(totalStake > 0){
        // try local balance check if available
        const ok = await postUpdateBet(totalStake, false, email);
        if(!ok){ if(resultEl) resultEl.innerText = 'Bet failed: insufficient funds or server error.'; return; }
      }

      // disable controls
      const controls = Array.from(document.querySelectorAll('.chip, .bet-cell, #betRed, #betBlack, #resetBtn'));
      controls.forEach(c=> c.setAttribute('disabled','true'));
      if(spinBtn) spinBtn.setAttribute('disabled','true');
      spinning = true;

      const spins = 6 + Math.floor(Math.random()*6);
      const winningIndex = Math.floor(Math.random()*numbers.length);
      const slice = (2*Math.PI)/numbers.length;
      const targetRotation = ( - (winningIndex + 0.5) * slice ) + (-Math.PI/2);
      const start = 0; const duration = 3500 + Math.random()*1200; const final = targetRotation - (spins * 2*Math.PI);
      const startTime = performance.now();

      function animate(now){
        const t = Math.min(1, (now - startTime)/duration);
        const eased = 1 - Math.pow(1-t,3);
        const current = start + (final - start) * eased;
        drawWheel(current);
        if(t < 1) requestAnimationFrame(animate);
        else {
          (async ()=>{
            spinning = false;
            const winNum = numbers[winningIndex];
            if(resultEl) resultEl.innerText = 'Result: ' + winNum;
            document.querySelectorAll('.bet-cell').forEach(b=> b.classList.toggle('bet-win', parseInt(b.dataset.num,10)===winNum));

            let totalWin = 0;
            const betOnNumber = Number(pendingBets[winNum] || 0);
            if(betOnNumber > 0) totalWin += betOnNumber * 36;
            if(winNum !== 0){
              const winColor = isRed(winNum) ? 'red' : 'black';
              const colorBet = Number(colorBets[winColor] || 0);
              if(colorBet > 0) totalWin += colorBet * 2;
            }

            if(totalWin > 0 && email){
              const okAdd = await postUpdateBet(totalWin, true, email);
              if(okAdd){ if(resultEl) resultEl.innerText += ' — You won: ' + totalWin + ' €'; }
              else { if(resultEl) resultEl.innerText += ' — Server error applying winnings.'; }
            } else if(totalStake > 0){ if(resultEl) resultEl.innerText += ' — No win.'; }

            clearPlacedBetsUI(); updateColorButtonUI();
            // re-enable controls
            controls.forEach(c=> c.removeAttribute('disabled'));
            if(spinBtn) spinBtn.removeAttribute('disabled');
          })();
        }
      }
      requestAnimationFrame(animate);
    });

    // cleanup listeners on unmount
    return ()=>{
      try{
        chips.forEach(c=> c.replaceWith(c.cloneNode(true)));
        if(betRedBtn) betRedBtn.replaceWith(betRedBtn.cloneNode(true));
        if(betBlackBtn) betBlackBtn.replaceWith(betBlackBtn.cloneNode(true));
        if(resetBtn) resetBtn.replaceWith(resetBtn.cloneNode(true));
        if(spinBtn) spinBtn.replaceWith(spinBtn.cloneNode(true));
      }catch(err){ console.debug('roulette cleanup error', err); }
    };
  },[]);

  return (
    <div className="roulette-wrap">
      <section className="wheel-area">
        <div className="wheel-stage">
          <canvas ref={canvasRef} id="roulette" width={520} height={520} aria-label="Roulette wheel"></canvas>
          <div className="wheel-pointer" />
        </div>
        <div className="controls">
          <div style={{display:'flex',gap:8,justifyContent:'center'}}>
            <button id="spinBtn" className="btn btn-primary">Spin</button>
            <button id="resetBtn" className="btn btn-ghost">Reset Bets</button>
          </div>
          <p id="result" className="small muted mt-1">Place your bets and spin the wheel.</p>
        </div>
      </section>
      <aside className="bets-area">
        <div className="bets-card">
          <h3>Betting Table</h3>
          <div className="chips-row" style={{marginBottom:12}}>
            <button className="chip" data-value="1">1</button>
            <button className="chip" data-value="5">5</button>
            <button className="chip" data-value="10">10</button>
            <button className="chip" data-value="25">25</button>
          </div>
          <div id="usernameView" className="username" />
          <div id="balanceView" className="balance">Balance: -- €</div>
          <div className="bet-grid-wrap">
            <div className="bet-grid" id="betGrid" />
            <div className="bet-column-vertical">
              <button id="betRed" className="bet-color-tall btn-red">RED</button>
              <button id="betBlack" className="bet-color-tall btn-black">BLACK</button>
            </div>
          </div>
          <div className="mt-1"><a className="btn btn-ghost" href="/vida/games">Back to Games</a></div>
        </div>
      </aside>
    </div>
  );
}
