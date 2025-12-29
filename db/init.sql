-- Database initialization for iLoca project
-- Creates schemas and tables used by BankuumTubo and VidaLoca
-- Run this in PostgreSQL (e.g. docker-entrypoint-initdb.d)

-- Create separate schemas for the two services
CREATE SCHEMA IF NOT EXISTS "BankuumTubo";
CREATE SCHEMA IF NOT EXISTS "VidaLoca";

-- Table: BankuumTubo.clients
-- Columns based on Api.Models.Client
CREATE TABLE IF NOT EXISTS "BankuumTubo".clients (
    client_id SERIAL PRIMARY KEY,
    first_name TEXT NOT NULL DEFAULT '',
    last_name TEXT NOT NULL DEFAULT '',
    email TEXT NOT NULL UNIQUE,
    phone TEXT NOT NULL DEFAULT '',
    iban TEXT NOT NULL UNIQUE,
    balance NUMERIC(18,2) NOT NULL DEFAULT 0
);

-- Table: BankuumTubo.accounts
-- Columns based on Api.Models.Account (BankuumTubo)
CREATE TABLE IF NOT EXISTS "BankuumTubo".accounts (
    account_id SERIAL PRIMARY KEY,
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL,
    user_name TEXT NOT NULL DEFAULT '',
    client_id INTEGER NOT NULL REFERENCES "BankuumTubo".clients(client_id) ON DELETE CASCADE
);

-- Table: BankuumTubo.transactions
-- Columns based on Api.Models.Transaction
CREATE TABLE IF NOT EXISTS "BankuumTubo".transactions (
    transaction_id SERIAL PRIMARY KEY,
    sender_email TEXT NOT NULL,
    receiver_iban TEXT NOT NULL,
    amount INTEGER NOT NULL,
    date_time TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT now(),
    reason TEXT NOT NULL DEFAULT ''
);

-- Table: VidaLoca.accounts
-- Columns based on Api.Models.Account (VidaLoca)
CREATE TABLE IF NOT EXISTS "VidaLoca".accounts (
    account_id SERIAL PRIMARY KEY,
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL,
    user_name TEXT NOT NULL DEFAULT '',
    -- VidaLoca stores a balance as used by repository methods
    balance NUMERIC(18,2) NOT NULL DEFAULT 0
);

-- Helpful indexes
CREATE INDEX IF NOT EXISTS idx_bankuumtubo_clients_email ON "BankuumTubo".clients (email);
CREATE INDEX IF NOT EXISTS idx_bankuumtubo_clients_iban ON "BankuumTubo".clients (iban);
CREATE INDEX IF NOT EXISTS idx_bankuumtubo_accounts_email ON "BankuumTubo".accounts (email);
CREATE INDEX IF NOT EXISTS idx_vidaloca_accounts_email ON "VidaLoca".accounts (email);

-- Optional: seed a sample admin client/account for testing (uncomment if desired)
-- INSERT INTO "BankuumTubo".clients (first_name, last_name, email, phone, iban, balance)
-- VALUES ('Test','User','test@bankuum.local','123456789','IT' || LPAD((floor(random()*1e10)::bigint)::text, 25, '0'), 10000)
-- ON CONFLICT (email) DO NOTHING;

-- INSERT INTO "BankuumTubo".accounts (email, password, user_name, client_id)
-- SELECT c.email, 'changeme', 'testuser', c.client_id FROM "BankuumTubo".clients c WHERE c.email = 'test@bankuum.local' ON CONFLICT (email) DO NOTHING;
