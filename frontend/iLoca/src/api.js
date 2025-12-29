// Minimal API helpers using the nginx proxy prefixes used in docker-compose
// The nginx config already proxies:
//   /api/vidaloca/ -> vidaloca:5112
//   /api/bankuumtubo/ -> bankuumtubo:5027

const VIDA_BASE = '/api/vidaloca';
const BANK_BASE = '/api/bankuumtubo';

// Basic fetch wrappers. They return the raw fetch Response so callers
// can call .json(), handle status codes, etc.
export function vidaFetch(path, opts = {}) {
  return fetch(VIDA_BASE + path, opts);
}

export function bankFetch(path, opts = {}) {
  return fetch(BANK_BASE + path, opts);
}

// Convenience helpers
export function getVidaBalance(accountId) {
  return vidaFetch(`/Account/GetBalanceByAccount?accountId=${accountId}`);
}

export function verifyBankIban(iban) {
  return bankFetch(`/Account/VerifyBankIban?iban=${encodeURIComponent(iban)}`);
}

export function withdrawFromBank(payload) {
  // payload: { accountId, amount, bankIban, IsDeposit: false }
  return vidaFetch('/Account/WithdrawFromBank', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
}

export function depositToBank(payload) {
  // payload: { accountId, amount, bankIban, IsDeposit: true }
  return vidaFetch('/Account/WithdrawFromBank', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });
}
