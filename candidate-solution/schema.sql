CREATE TABLE users (
  id UUID PRIMARY KEY,
  created_at TIMESTAMP DEFAULT NOW()
);

CREATE TABLE transactions (
  id bigint GENERATED ALWAYS AS IDENTITY,
  user_id UUID REFERENCES users(id),
  amount DECIMAL(12, 2) NOT NULL,
  currency VARCHAR(3) NOT NULL,
  date TIMESTAMP NOT NULL,
  description TEXT,
  status VARCHAR(20) NOT NULL,
  type VARCHAR(50),
  raw_data JSONB,
  created_at TIMESTAMP default NOW()
);