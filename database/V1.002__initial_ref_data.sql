INSERT INTO loan_statuses (id, name) VALUES
    (1, 'Active'),
    (2, 'Paid'),
    (3, 'Defaulted'),
    (4, 'Refinanced');

INSERT INTO warranty_types (id, name) VALUES
    (1, 'Basic Warranty'),
    (2, 'Extended Warranty'),
    (3, 'Powertrain Warranty'),
    (4, 'Bumper-to-Bumper Warranty');

INSERT INTO roles (id, description, name) VALUES
    ('Admin', 'Full access to all features', 'Administrator'),
    ('User', 'Limited access to features', 'Standard User'),
    ('Guest', 'Read-only access to features', 'Guest User'),
    ('Manager', 'Access to management features', 'Manager');

INSERT INTO users (id, username, password_hash, salt, role_id, created_at, force_password_reset) VALUES
    (1, 'admin', '56307jSNH5tPp2QU/j6EqKXlf8BwpihWASYdKR5h0Ms=', '/jgOBO6nRKbhU7yGLI9vXA==', 'Admin', '2025-04-08 10:06:49.238', FALSE);
