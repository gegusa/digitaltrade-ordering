CREATE SCHEMA IF NOT EXISTS ordering;

CREATE TABLE ordering.customers
(
    id    BIGSERIAL PRIMARY KEY,
    name  TEXT NOT NULL,
    email TEXT NOT NULL
);

CREATE TABLE ordering.orders
(
    id               BIGSERIAL PRIMARY KEY,
    customer_id      BIGINT         NOT NULL,
    shipping_address TEXT           NOT NULL,
    credit_card      TEXT NULL,
    payment          TEXT           NOT NULL,
    amount           NUMERIC(18, 2) NOT NULL,
    status           INT            NOT NULL,
    created_at       TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT fk_orders_customer FOREIGN KEY (customer_id)
        REFERENCES ordering.customers (id)
        ON DELETE CASCADE
);

CREATE TABLE ordering.order_items
(
    id             BIGSERIAL PRIMARY KEY,
    order_id       BIGINT         NOT NULL,
    product_id     BIGINT         NOT NULL,
    name           TEXT           NOT NULL,
    price_per_item NUMERIC(18, 2) NOT NULL,
    quantity       BIGINT         NOT NULL,
    CONSTRAINT fk_order_items_order FOREIGN KEY (order_id)
        REFERENCES ordering.orders (id)
        ON DELETE CASCADE
);
