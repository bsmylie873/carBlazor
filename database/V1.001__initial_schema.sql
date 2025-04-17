-- V1__Initial_Schema.sql
CREATE TABLE IF NOT EXISTS loan_statuses (
    id SERIAL PRIMARY KEY,
    name TEXT
);

CREATE TABLE IF NOT EXISTS warranty_types (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS cars (
    id SERIAL PRIMARY KEY,
    make TEXT,
    model TEXT,
    year INTEGER,
    color TEXT,
    production_date TIMESTAMP
);

CREATE TABLE IF NOT EXISTS customers (
    id SERIAL PRIMARY KEY,
    first_name TEXT,
    last_name TEXT,
    email TEXT,
    phone_number TEXT,
    address TEXT
);

CREATE TABLE IF NOT EXISTS customer_cars (
    id SERIAL PRIMARY KEY,
    customer_id INTEGER NOT NULL,
    car_id INTEGER NOT NULL,
    purchase_date TIMESTAMP,
    is_primary_owner BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (customer_id) REFERENCES customers(id),
    FOREIGN KEY (car_id) REFERENCES cars(id)
);

CREATE TABLE IF NOT EXISTS loans (
    id SERIAL PRIMARY KEY,
    car_id INTEGER NOT NULL,
    customer_id INTEGER NOT NULL,
    amount DECIMAL(10, 2),
    interest_rate DECIMAL(5, 2),
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    monthly_payment DECIMAL(10, 2),
    remaining_balance DECIMAL(10, 2),
    status_id INTEGER NOT NULL,
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (customer_id) REFERENCES customers(id),
    FOREIGN KEY (status_id) REFERENCES loan_statuses(id)
);

CREATE TABLE IF NOT EXISTS roles (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username TEXT NOT NULL,
    password_hash TEXT NOT NULL,
    salt TEXT NOT NULL,
    role_id TEXT NOT NULL,
    force_password_reset BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (role_id) REFERENCES roles(id)
);

CREATE TABLE IF NOT EXISTS warranties (
    id SERIAL PRIMARY KEY,
    car_id INTEGER NOT NULL,
    warranty_type_id INTEGER NOT NULL,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    coverage_details TEXT,
    provider TEXT,
    cost DECIMAL(10, 2),
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (warranty_type_id) REFERENCES warranty_types(id)
);