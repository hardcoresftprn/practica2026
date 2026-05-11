CREATE DATABASE IF NOT EXISTS practica
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;
USE practica;

-- Schema simplificata: doar tabelele esentiale pentru prezentare

CREATE TABLE clients (
    client_id INT AUTO_INCREMENT PRIMARY KEY,
    full_name VARCHAR(120) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    email VARCHAR(120),
    address VARCHAR(255),
    idnp VARCHAR(20),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_clients_phone (phone),
    UNIQUE KEY uq_clients_email (email)
) ENGINE=InnoDB;

CREATE TABLE vehicles (
    vehicle_id INT AUTO_INCREMENT PRIMARY KEY,
    client_id INT NOT NULL,
    license_plate VARCHAR(20) NOT NULL,
    brand VARCHAR(60) NOT NULL,
    model VARCHAR(60) NOT NULL,
    production_year SMALLINT,
    vin VARCHAR(30) NOT NULL,
    mileage INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_vehicles_license_plate (license_plate),
    UNIQUE KEY uq_vehicles_vin (vin),
    CONSTRAINT fk_vehicles_client
        FOREIGN KEY (client_id) REFERENCES clients(client_id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    full_name VARCHAR(120) NOT NULL,
    email VARCHAR(120) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role ENUM('ADMINISTRATOR','USER') NOT NULL DEFAULT 'USER',
    is_active TINYINT(1) NOT NULL DEFAULT 1,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE service_orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    client_id INT NOT NULL,
    vehicle_id INT NOT NULL,
    assigned_mechanic_id INT,
    order_number VARCHAR(30) NOT NULL,
    complaint TEXT NOT NULL,
    diagnosis TEXT,
    status ENUM('NEW','IN_PROGRESS','COMPLETED','CANCELLED') NOT NULL DEFAULT 'NEW',
    labor_cost DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    planned_start DATETIME,
    planned_end DATETIME,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    closed_at DATETIME NULL,
    UNIQUE KEY uq_service_orders_order_number (order_number),
    CONSTRAINT fk_orders_client
        FOREIGN KEY (client_id) REFERENCES clients(client_id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT fk_orders_vehicle
        FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT fk_orders_mechanic
        FOREIGN KEY (assigned_mechanic_id) REFERENCES users(user_id)
        ON UPDATE CASCADE
        ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE invoices (
    invoice_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    invoice_number VARCHAR(30) NOT NULL,
    issue_date DATE NOT NULL,
    due_date DATE,
    subtotal DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    vat_percent DECIMAL(5,2) NOT NULL DEFAULT 20.00,
    vat_amount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    total_amount DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    payment_status ENUM('UNPAID','PARTIAL','PAID') NOT NULL DEFAULT 'UNPAID',
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uq_invoices_number (invoice_number),
    UNIQUE KEY uq_invoices_order (order_id),
    CONSTRAINT fk_invoices_order
        FOREIGN KEY (order_id) REFERENCES service_orders(order_id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE payments (
    payment_id INT AUTO_INCREMENT PRIMARY KEY,
    invoice_id INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL,
    payment_method ENUM('CASH','CARD','BANK_TRANSFER') NOT NULL,
    paid_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    reference_no VARCHAR(60),
    CONSTRAINT fk_payments_invoice
        FOREIGN KEY (invoice_id) REFERENCES invoices(invoice_id)
        ON UPDATE CASCADE
        ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE INDEX idx_orders_status ON service_orders(status);
CREATE INDEX idx_orders_created_at ON service_orders(created_at);
CREATE INDEX idx_payments_paid_at ON payments(paid_at);
