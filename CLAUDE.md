# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a C# training assignment repository for developing a customer management desktop application. The project is designed as a hands-on learning exercise for new software engineers to understand the full development process from UI design to database integration.

## Architecture and Technology Stack

- **Language**: C#
- **Framework**: .NET Framework (4.7.2+) or .NET 6+
- **UI**: Windows Forms
- **Database**: MySQL
- **ORM**: Entity Framework 6 or EF Core
- **Project Type**: Desktop application with CRUD functionality

## Key Requirements

The application consists of two main forms:
1. **Customer List Form (Read-focused)**: Main screen displaying customer data in DataGridView with buttons for Create, Update, Delete operations
2. **Customer Edit Form (Create/Update)**: Modal dialog for customer registration and editing

### Database Schema
```sql
CREATE TABLE customers (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    kana VARCHAR(255),
    phone_number VARCHAR(20),
    email VARCHAR(255) UNIQUE,
    created_at DATETIME,
    updated_at DATETIME
);
```

### Required NuGet Packages
- EntityFramework (or Microsoft.EntityFrameworkCore)
- MySql.Data.EntityFramework (or MySql.EntityFrameworkCore)

## Development Setup

Since this is a training assignment repository with no actual C# project files yet, development setup would involve:

1. Creating a new Windows Forms project in Visual Studio
2. Installing required NuGet packages
3. Setting up MySQL database and connection
4. Using Database First approach with Entity Framework to generate models
5. Implementing the two main forms with CRUD operations

## Validation Requirements

- **Required fields**: Name (氏名) and Email (メールアドレス) are mandatory
- **Unique constraint**: Email addresses must be unique across the system
- **Error handling**: Display appropriate error messages for validation failures and database connection issues

## Implementation Guidelines

- Use modal dialogs (`ShowDialog()`) for the customer edit form
- Automatically refresh the customer list after any CRUD operations
- Implement proper error handling for database operations
- Follow Windows Forms best practices for UI design
- Use Entity Framework for all database operations to prevent SQL injection

## Security Considerations

- Entity Framework provides automatic protection against SQL injection through parameterized queries
- Connection strings should be properly secured (not hardcoded in plain text)
- Implement proper input validation for all user inputs

## Testing Focus Areas

- CRUD operations (Create, Read, Update, Delete)
- Form validation and error handling
- Modal dialog behavior
- Data refresh after operations
- Database constraint handling (unique email validation)

## Current Status

This repository currently contains only documentation files (`docs/training-assignment.md`) describing the assignment requirements. No actual C# project files have been created yet. This is a training assignment template rather than an active development project.

回答は全て日本語で回答してください。

## Gemini CLI 連携ガイド

### 目的
ユーザーが **「Geminiと相談しながら進めて」** と指示した場合、
Claude は以降のタスクを **Gemini CLI** と協調しながら進める。

### トリガー
- 正規表現: `/Gemini.*相談しながら/`

### 基本フロー
1. **PROMPT 生成**
   Claude はユーザーの要件を1つのテキストにまとめ、環境変数 `$PROMPT` に格納

2. **Gemini CLI 呼び出し**
```bash
gemini <<EOF
$PROMPT
EOF

3. **結果の統合**
   - Gemini の回答を提示
   - Claude の追加分析・コメントを付加