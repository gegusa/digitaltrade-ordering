create schema payment

create table if not exists payment.payment (
    id            bigserial primary key,
    order_id      bigserial not null,
    amount        decimal not null,
    status        int not null,
    created_at    timestamp with time zone not null
);

comment on table payment.payment is 'Пользователи торговой площадки.';
comment on column payment.payment.id is 'ID оплаты.';
comment on column payment.payment.order_id is 'ID заказа.';
comment on column payment.payment.amount is 'Стоимость заказа.';
comment on column payment.payment.created_at is 'Дата и время инициализации оплаты.';