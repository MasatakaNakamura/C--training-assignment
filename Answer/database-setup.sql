-- 顧客管理システム用データベースセットアップスクリプト
-- MySQL用

-- データベースの作成
CREATE DATABASE IF NOT EXISTS customer_management 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

-- 開発用データベースの作成
CREATE DATABASE IF NOT EXISTS customer_management_dev 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

-- 本番用データベースを使用
USE customer_management;

-- customersテーブルの作成
CREATE TABLE IF NOT EXISTS customers (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL COMMENT '氏名（必須）',
    kana VARCHAR(255) COMMENT 'フリガナ',
    phone_number VARCHAR(20) COMMENT '電話番号',
    email VARCHAR(255) NOT NULL UNIQUE COMMENT 'メールアドレス（必須・一意）',
    created_at DATETIME COMMENT '作成日時',
    updated_at DATETIME COMMENT '更新日時'
) COMMENT '顧客情報テーブル';

-- インデックスの作成
CREATE INDEX idx_customers_name ON customers(name);
CREATE INDEX idx_customers_email ON customers(email);

-- 開発用データベースにも同じテーブルを作成
USE customer_management_dev;

CREATE TABLE IF NOT EXISTS customers (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL COMMENT '氏名（必須）',
    kana VARCHAR(255) COMMENT 'フリガナ',
    phone_number VARCHAR(20) COMMENT '電話番号',
    email VARCHAR(255) NOT NULL UNIQUE COMMENT 'メールアドレス（必須・一意）',
    created_at DATETIME COMMENT '作成日時',
    updated_at DATETIME COMMENT '更新日時'
) COMMENT '顧客情報テーブル';

CREATE INDEX idx_customers_name ON customers(name);
CREATE INDEX idx_customers_email ON customers(email);

-- サンプルデータの挿入（開発用）
INSERT INTO customers (name, kana, phone_number, email, created_at, updated_at) VALUES
('田中太郎', 'タナカタロウ', '03-1234-5678', 'tanaka@example.com', NOW(), NOW()),
('佐藤花子', 'サトウハナコ', '090-1234-5678', 'sato@example.com', NOW(), NOW()),
('鈴木次郎', 'スズキジロウ', '080-9876-5432', 'suzuki@example.com', NOW(), NOW()),
('高橋美咲', 'タカハシミサキ', '070-5555-1234', 'takahashi@example.com', NOW(), NOW()),
('山田健一', 'ヤマダケンイチ', '03-9999-8888', 'yamada@example.com', NOW(), NOW());

-- 確認用クエリ
SELECT '=== 本番用データベース ===' as info;
USE customer_management;
SELECT COUNT(*) as customer_count FROM customers;

SELECT '=== 開発用データベース ===' as info;
USE customer_management_dev;
SELECT COUNT(*) as customer_count FROM customers;
SELECT * FROM customers ORDER BY id;

-- 完了メッセージ
SELECT 'データベースのセットアップが完了しました！' as result;